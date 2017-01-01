using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chess.Pieces;

namespace Chess.AI
{
    public class ChessBot
    {
        private readonly IBoardEvaluator boardEvaluator;
        private readonly int maxDepth;
        private ConcurrentDictionary<EvalCacheKey, float> boardEvalCache;
        private readonly bool testMode;
        private bool cacheChanged;
        private bool winFoundOnRoot;

        public ChessBot(IBoardEvaluator evaluator, int depth, Color color, bool test)
        {
            Color = color;
            testMode = test;
            boardEvaluator = evaluator;
            maxDepth = depth;
            boardEvalCache = new ConcurrentDictionary<EvalCacheKey, float>();
        }

        public ChessBot(IBoardEvaluator evaluator, int depth, Color color)
        {
            Color = color;
            boardEvaluator = evaluator;
            maxDepth = depth;
            boardEvalCache = new ConcurrentDictionary<EvalCacheKey, float>(ConfigManager.LoadCache(boardEvaluator, maxDepth, Color));
        }

        public Color  Color { get; set; }

        public void SaveCache()
        {
            ConfigManager.SaveCache(boardEvaluator, boardEvalCache, maxDepth, Color);
        }

        public Move FindMove(ChessBoard board)
        {
            var color = Color == Color.Nobody ? board.ColorToMove : Color;

            boardEvalCache = new ConcurrentDictionary<EvalCacheKey, float>();
            var baseMoves = board.GetAllAvailableMoves(color);
            var rootNodes = new ConcurrentBag<MoveNode>();
            cacheChanged = false;

            Parallel.ForEach(baseMoves, baseMove =>
            {
                rootNodes.Add(CreateRootMoveNode(color, board.JsonCopy(), baseMove));
            });

            var random = new Random();

            if (!rootNodes.Any())
                return null;

            //if (!testMode && cacheChanged)
            //    SaveCache();

            return rootNodes.OrderByDescending(node => node.Score).ThenBy(node => random.Next()).First().Move;
        }

        public List<MoveNode> GetMovesList(ChessBoard board)
        {
            boardEvalCache = new ConcurrentDictionary<EvalCacheKey, float>();
            var baseMoves = board.GetAllAvailableMoves(Color);
            var rootNodes = new ConcurrentBag<MoveNode>();
            cacheChanged = false;

            Parallel.ForEach(baseMoves, baseMove =>
            {
                rootNodes.Add(CreateRootMoveNode(Color, board.JsonCopy(), baseMove));
            });

            //foreach (var baseMove in baseMoves)
            //{
            //    rootNodes.Add(CreateRootMoveNode(Color, board.JsonCopy(), baseMove));
            //}

            var random = new Random();

            if (!rootNodes.Any())
                return null;

            //if (!testMode && cacheChanged)
            //    SaveCache();

            return rootNodes.OrderByDescending(node => node.Score).ToList();
        }

        private MoveNode CreateRootMoveNode(Color color, ChessBoard board, Move baseMove)
        {
            float baseValue = boardEvaluator.EvaluateBoard(board, color);

            var node = new MoveNode
            {
                Move = baseMove
            };

            var boardWithMove = board.CopyWithMove(node.Move);
            var cacheKey = new EvalCacheKey(boardWithMove.GetFenString());

            if (boardWithMove.Winner.HasWinner)
            {
                float score = boardEvaluator.EvaluateBoard(board.CopyWithMove(node.Move), color);
                boardEvalCache.TryAdd(cacheKey, score);
                node.Score = score;
                winFoundOnRoot = true;
                return node;
            }

            if (boardEvalCache.ContainsKey(cacheKey))
                node.Score = boardEvalCache[cacheKey];
            else
            {
                node.Children = BuildMoveTreeForColor(ChessBoard.InvertColor(color), color, board.CopyWithMove(baseMove), 2, baseValue);
            }

            if (node.Children == null || !node.Children.Any())
            {
                cacheKey = new EvalCacheKey(board.CopyWithMove(node.Move).GetFenString());

                if (boardEvalCache.ContainsKey(cacheKey))
                    node.Score = boardEvalCache[cacheKey];
                else
                {
                    float score = boardEvaluator.EvaluateBoard(board.CopyWithMove(node.Move), color);
                    boardEvalCache.TryAdd(cacheKey, score);
                    cacheChanged = true;
                    node.Score = score;
                }
            }
            else
            {
                node.Score = node.GetWorstChild();
            }

            return node;
        }

        private List<MoveNode> BuildMoveTreeForColor(Color color, Color originalColor, ChessBoard board, int currentLevel, float baseValue)
        {
            if (winFoundOnRoot)
                return new List<MoveNode>();

            if (currentLevel > maxDepth)
                return null;

            var nodes = new List<MoveNode>();

            var cacheKey = new EvalCacheKey(board.GetFenString());

            var initScore = boardEvaluator.EvaluateBoard(board, originalColor);
            if (initScore - baseValue <= -4)
            {
                nodes.Add(new MoveNode
                {
                    Score = initScore - baseValue,
                    Children = null
                });
                return nodes;
            }

            if (boardEvalCache.ContainsKey(cacheKey))
            {
                return new List<MoveNode>
                    {
                        new MoveNode
                        {
                            Score = boardEvalCache[cacheKey] - baseValue,
                            Children = null
                        }
                    };
            }

            foreach (var move in board.GetAllAvailableMoves(color))
            {
                if (winFoundOnRoot)
                    return new List<MoveNode>();

                if (currentLevel == maxDepth)
                {
                    var node = GetMoveNodeMaxDepth(color, board, move, baseValue);

                    nodes.Add(node);
                }
                else
                {
                    var chessBoard = board.CopyWithMove(move);

                    float testScore = boardEvaluator.EvaluateBoard(board.CopyWithMove(move), originalColor);
                    boardEvalCache.TryAdd(cacheKey, testScore);

                    var node = new MoveNode
                    {
                        Children = BuildMoveTreeForColor(ChessBoard.InvertColor(color), originalColor, chessBoard, currentLevel + 1, baseValue),
                        Move = move
                    };

                    if (node.Children.Any())
                        node.Score = color == originalColor ? node.GetWorstChild() : node.GetBestChild();
                    else
                    {
                        cacheKey = new EvalCacheKey(board.GetFenString());

                        if (boardEvalCache.ContainsKey(cacheKey))
                            node.Score = boardEvalCache[cacheKey] - baseValue;
                        else
                        {
                            var score = boardEvaluator.EvaluateBoard(chessBoard.CopyWithMove(move), originalColor);

                            if (!boardEvalCache.ContainsKey(cacheKey))
                            {
                                boardEvalCache.TryAdd(cacheKey, score);
                                cacheChanged = true;
                            }

                            node.Score = score - baseValue;
                        }
                    }

                    nodes.Add(node);
                }
            }

            return nodes;
        }

        private MoveNode GetMoveNodeMaxDepth(Color color, ChessBoard board, Move move, float baseValue)
        {
            var node = new MoveNode
            {
                Children = null,
                Move = move
            };

            var boardCopy = board.CopyWithMove(move);

            var cacheKey = new EvalCacheKey(boardCopy.GetFenString());

            if (boardEvalCache.ContainsKey(cacheKey))
                node.Score = boardEvalCache[cacheKey];
            else
            {
                var score = boardEvaluator.EvaluateBoard(boardCopy, color) - baseValue;

                if (!boardEvalCache.ContainsKey(cacheKey))
                {
                    boardEvalCache.TryAdd(cacheKey, score);
                    cacheChanged = true;
                }

                node.Score = score;
            }

            return node;
        }
    }
}
