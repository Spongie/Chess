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
        private static ConcurrentDictionary<EvalCacheKey, float> boardEvalCache;
        private bool testMode;
        private bool cacheChanged;

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
            var baseMoves = board.GetAllAvailableMoves(Color);
            var rootNodes = new ConcurrentBag<MoveNode>();
            cacheChanged = false;

            Parallel.ForEach(baseMoves, baseMove =>
            {
                rootNodes.Add(CreateRootMoveNode(Color, board.JsonCopy(), baseMove));
            });

            var random = new Random();

            if (!rootNodes.Any())
                return null;

            if (!testMode && cacheChanged)
                SaveCache();

            return rootNodes.OrderByDescending(node => node.Score).ThenBy(node => random.Next()).First().Move;
        }

        private MoveNode CreateRootMoveNode(Color color, ChessBoard board, Move baseMove)
        {
            var node = new MoveNode
            {
                Move = baseMove
            };

            var cacheKey = new EvalCacheKey(board.CopyWithMove(node.Move).GetFenString(), color);

            if (boardEvalCache.ContainsKey(cacheKey))
                node.Score = boardEvalCache[cacheKey];
            else
            {
                node.Children = BuildMoveTreeForColor(ChessBoard.InvertColor(color), color, board.CopyWithMove(baseMove), 2);
            }

            if (node.Children == null || !node.Children.Any())
            {
                cacheKey = new EvalCacheKey(board.CopyWithMove(node.Move).GetFenString(), color);

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
                node.Score = node.GetBestChild();
            }

            return node;
        }

        private List<MoveNode> BuildMoveTreeForColor(Color color, Color originalColor, ChessBoard board, int currentLevel)
        {
            if (currentLevel > maxDepth)
                return null;

            var nodes = new List<MoveNode>();

            var cacheKey = new EvalCacheKey(board.GetFenString(), color);

            if (boardEvalCache.ContainsKey(cacheKey))
            {
                return new List<MoveNode>
                    {
                        new MoveNode
                        {
                            Score = boardEvalCache[cacheKey],
                            Children = null
                        }
                    };
            }

            foreach (var move in board.GetAllAvailableMoves(color))
            {
                if (currentLevel == maxDepth)
                {
                    var node = GetMoveNodeMaxDepth(color, board, move);

                    nodes.Add(node);
                }
                else
                {
                    var chessBoard = board.JsonCopy();

                    chessBoard.Board = chessBoard.GetBoardAfterMove(move);

                    var node = new MoveNode
                    {
                        Children = BuildMoveTreeForColor(ChessBoard.InvertColor(color), originalColor, chessBoard, currentLevel + 1)
                    };

                    if (node.Children.Any())
                        node.Score = color == originalColor ? node.GetBestChild() : node.GetWorstChild();
                    else
                    {
                        cacheKey = new EvalCacheKey(board.GetFenString(), color);

                        if (boardEvalCache.ContainsKey(cacheKey))
                            node.Score = boardEvalCache[cacheKey];
                        else
                        {
                            var score = boardEvaluator.EvaluateBoard(chessBoard.CopyWithMove(move), color);

                            if (!boardEvalCache.ContainsKey(cacheKey))
                            {
                                boardEvalCache.TryAdd(cacheKey, score);
                                cacheChanged = true;
                            }

                            node.Score = score;
                        }
                    }

                    nodes.Add(node);
                }
            }

            return nodes;
        }

        private MoveNode GetMoveNodeMaxDepth(Color color, ChessBoard board, Move move)
        {
            var node = new MoveNode
            {
                Children = null
            };

            var boardCopy = board.CopyWithMove(move);

            var cacheKey = new EvalCacheKey(boardCopy.GetFenString(), color);

            if (boardEvalCache.ContainsKey(cacheKey))
                node.Score = boardEvalCache[cacheKey];
            else
            {
                var score = boardEvaluator.EvaluateBoard(boardCopy, color);

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
