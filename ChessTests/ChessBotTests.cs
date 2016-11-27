using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess;
using Chess.AI;
using Chess.Pieces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChessTests
{
    [TestClass]
    public class ChessBotTests
    {
        [TestMethod]
        public void MoveIsChosen()
        {
            var bot = new ChessBot(new OnlyPiecesMatterEvaluator(), 1);

            var board = new ChessBoard();

            var move = new Move
            {
                Piece = board.GetPieceAtPosition(0, 1),
                TargetPosition = new Position(5, 0)
            };

            board.MakeMove(move);

            var botMove = bot.FindMoveForColor(Color.White, board);

            Assert.AreEqual(new Position(5, 0), botMove.TargetPosition);
        }

        [TestMethod]
        public void Depth_2_Performance()
        {
            var bot = new ChessBot(new OnlyPiecesMatterEvaluator(), 2);

            var board = new ChessBoard();

            var botMove = bot.FindMoveForColor(Color.White, board);

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Depth_3_Performance()
        {
            var bot = new ChessBot(new OnlyPiecesMatterEvaluator(), 3);

            var board = new ChessBoard();

            var botMove = bot.FindMoveForColor(Color.White, board);

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Depth_4_Performance()
        {
            var bot = new ChessBot(new OnlyPiecesMatterEvaluator(), 4);

            var board = new ChessBoard();

            var botMove = bot.FindMoveForColor(Color.White, board);

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Depth_5_Performance()
        {
            var bot = new ChessBot(new OnlyPiecesMatterEvaluator(), 5);

            var board = new ChessBoard();

            var botMove = bot.FindMoveForColor(Color.White, board);

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Depth_6_Performance()
        {
            var bot = new ChessBot(new OnlyPiecesMatterEvaluator(), 6);

            var board = new ChessBoard();

            var botMove = bot.FindMoveForColor(Color.White, board);

            Assert.IsTrue(true);
        }
    }
}
