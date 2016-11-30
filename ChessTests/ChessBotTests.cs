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
            var bot = new ChessBot(new OnlyPieceCountMatterEvaluator(), 1, Color.White, true);

            var board = new ChessBoard();

            var move = new Move
            {
                Piece = board.GetPieceAtPosition(0, 1),
                TargetPosition = new Position(5, 0)
            };

            board.MakeMove(move);

            var botMove = bot.FindMove(board);

            Assert.AreEqual(new Position(5, 0), botMove.TargetPosition);
        }

        [TestMethod]
        public void InCheckWhiteIllegalMoveBot()
        {
            var bot = new ChessBot(new OnlyPieceCountMatterEvaluator(), 3, Color.White, true);

            var c = new ChessBoard();
            var board = new Piece[8, 8];

            board[0, 2] = new King(Color.Black) {AmountOfMoves = 2};
            board[0, 4] = new Rook(Color.Black) { AmountOfMoves = 1 };
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
            bot.FindMove(c);

            Assert.AreEqual(6, allMoves.Count());
        }

        [TestMethod]
        public void Depth_2_Performance()
        {
            var bot = new ChessBot(new OnlyPieceCountMatterEvaluator(), 2, Color.White, true);

            var board = new ChessBoard();

            var botMove = bot.FindMove(board);

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Depth_3_Performance()
        {
            var bot = new ChessBot(new OnlyPieceCountMatterEvaluator(), 3, Color.White, true);

            var board = new ChessBoard();

            var botMove = bot.FindMove(board);

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Depth_4_Performance()
        {
            var bot = new ChessBot(new OnlyPieceCountMatterEvaluator(), 4, Color.White, true);

            var board = new ChessBoard();

            var botMove = bot.FindMove(board);

            Assert.IsTrue(true);
        }


        [TestMethod]
        public void PawnAtEndBecomesQueen_BotHandlesIt()
        {
            var c = ChessBoard.CreateFromFenString("rn1qk2r/1pP3p1/8/pb1n3p/N7/7P/PPPbNPP1/R1BQKBR1");

            var bot = new ChessBot(new OnlyPieceCountMatterEvaluator(), 3, Color.White, true);

            var move = bot.FindMove(c);

            Assert.IsTrue(true);
        }

        //[TestMethod]
        //public void Depth_5_Performance()
        //{
        //    var bot = new ChessBot(new OnlyPieceCountMatterEvaluator(), 5, Color.White, true);
        //
        //    var board = new ChessBoard();
        //
        //    var botMove = bot.FindMove(board);
        //
        //    Assert.IsTrue(true);
        //}

        //[TestMethod]
        //public void Depth_6_Performance()
        //{
        //    var bot = new ChessBot(new OnlyPieceCountMatterEvaluator(), 6, Color.White, true);
        //
        //    var board = new ChessBoard();
        //
        //    var botMove = bot.FindMove(board);
        //
        //    Assert.IsTrue(true);
        //}
        
        //[TestMethod]
        //public void Depth_7_Performance()
        //{
        //    var bot = new ChessBot(new OnlyPieceCountMatterEvaluator(), 7, Color.White, true);
        //
        //    var board = new ChessBoard();
        //
        //    var botMove = bot.FindMove(board);
        //
        //    Assert.IsTrue(true);
        //}
    }
}
