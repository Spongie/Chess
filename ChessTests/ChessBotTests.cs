using System.Linq;
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
        public void InCheckWhiteIllegalMoveBot()
        {
            var bot = new ChessBot(new OnlyPiecesMatterEvaluator(), 3);

            var c = new ChessBoard();
            var board = new Piece[8, 8];

            board[0, 2] = new King(Color.Black);
            board[0, 4] = new Rook(Color.Black);
            board[1, 6] = new Knight(Color.Black);
            board[2, 6] = new Pawn(Color.Black);
            board[2, 7] = new Pawn(Color.Black);
            board[3, 0] = new Pawn(Color.Black);
            board[5, 1] = new Bishop(Color.Black);

            board[2, 2] = new Bishop(Color.White);
            board[3, 1] = new Knight(Color.White);
            board[4, 0] = new Pawn(Color.White);
            board[4, 1] = new Pawn(Color.White);
            board[4, 3] = new Pawn(Color.White);
            board[5, 4] = new King(Color.White);
            board[5, 6] = new Pawn(Color.White);
            board[6, 3] = new Bishop(Color.White);

            c.Board = board;

            var allMoves = c.GetAllAvailableMoves(Color.White);
            bot.FindMoveForColor(Color.White, c);

            Assert.AreEqual(6, allMoves.Count());
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

        //[TestMethod]
        //public void Depth_4_Performance()
        //{
        //    var bot = new ChessBot(new OnlyPiecesMatterEvaluator(), 4);

        //    var board = new ChessBoard();

        //    var botMove = bot.FindMoveForColor(Color.White, board);

        //    Assert.IsTrue(true);
        //}

        //[TestMethod]
        //public void Depth_5_Performance()
        //{
        //    var bot = new ChessBot(new OnlyPiecesMatterEvaluator(), 5);

        //    var board = new ChessBoard();

        //    var botMove = bot.FindMoveForColor(Color.White, board);

        //    Assert.IsTrue(true);
        //}

        //[TestMethod]
        //public void Depth_6_Performance()
        //{
        //    var bot = new ChessBot(new OnlyPiecesMatterEvaluator(), 6);

        //    var board = new ChessBoard();

        //    var botMove = bot.FindMoveForColor(Color.White, board);

        //    Assert.IsTrue(true);
        //}
    }
}
