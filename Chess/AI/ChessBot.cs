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

        public ChessBot(IBoardEvaluator evaluator, int depth)
        {
            boardEvaluator = evaluator;
            maxDepth = depth;
            boardEvalCache = new ConcurrentDictionary<EvalCacheKey, float>();
        }

        public Move FindMoveForColor(Color color, ChessBoard board)
        {
            var baseMoves = board.GetAllAvailableMoves(color);
            var rootNodes = new ConcurrentBag<MoveNode>();

            Parallel.ForEach(baseMoves, baseMove =>
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
                    node.Children = BuildMoveTreeForColor(ChessBoard.InvertColor(color), color,board.CopyWithMove(baseMove), 2);
                }

                rootNodes.Add(node);

                if (node.Children == null || !node.Children.Any())
                {
                    cacheKey = new EvalCacheKey(board.CopyWithMove(node.Move).GetFenString(), color);

                    if (boardEvalCache.ContainsKey(cacheKey))
                        node.Score = boardEvalCache[cacheKey];
                    else
                    {
                        float score = boardEvaluator.EvaluateBoard(board.CopyWithMove(node.Move), color);
                        boardEvalCache.TryAdd(cacheKey, score);
                        node.Score = score;
                    }
                }
                else
                {
                    node.Score = node.GetBestChild();
                }
            });

            var random = new Random();

            if (!rootNodes.Any())
                return null;

            return rootNodes.OrderByDescending(node => node.Score).ThenBy(node => random.Next()).First().Move;
        }

        private List<MoveNode> BuildMoveTreeForColor(Color color, Color originalColor, ChessBoard board, int currentLevel)
        {
            if (currentLevel > maxDepth)
                return null;

            var nodes = new List<MoveNode>();

            foreach (var move in board.GetAllAvailableMoves(color))
            {
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

                if (currentLevel == maxDepth)
                {
                    var node = new MoveNode
                    {
                        Children = null
                    };

                    var boardCopy = board.CopyWithMove(move);

                    cacheKey = new EvalCacheKey(boardCopy.GetFenString(), color);

                    if (boardEvalCache.ContainsKey(cacheKey))
                        node.Score = boardEvalCache[cacheKey];
                    else
                    {
                        var score = boardEvaluator.EvaluateBoard(boardCopy, color);

                        if (!boardEvalCache.ContainsKey(cacheKey))
                            boardEvalCache.TryAdd(cacheKey, score);

                        node.Score = score;

                    }

                    nodes.Add(node);
                }
                else
                {
                    var chessBoard = board.DeepClone();

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
                                boardEvalCache.TryAdd(cacheKey, score);

                            node.Score = score;
                        }
                    }

                    nodes.Add(node);
                }
            }

            return nodes;
        }
    }
}
