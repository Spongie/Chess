using System.Linq;
using Chess;
using Chess.Pieces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChessTests
{
    [TestClass]
    public class ChessBoardTests
    {
        [TestMethod]
        public void GetAllAvailableMoves_Start20()
        {
            var c = new ChessBoard();

            Assert.AreEqual(20, c.GetAllAvailableMoves(Color.White).Count());
        }

        [TestMethod]
        public void GetLegalMoves_IHasOne()
        {
            var c = new ChessBoard();
            var board = new Piece[8, 8];

            board[0, 0] = new King(Color.Black);
            board[5, 0] = new Queen(Color.White);
            board[1, 5] = new Rook(Color.White);
            board[7,7] = new King(Color.White);

            c.Board = board;

            var moves = c.GetAllAvailableMoves(Color.Black);

            Assert.AreEqual(1, moves.Count());
        }

        [TestMethod]
        public void GetLegalMoves_IHasDead()
        {
            var c = new ChessBoard();
            var board = new Piece[8, 8];

            board[0, 0] = new King(Color.Black);
            board[0, 5] = new Queen(Color.White);
            board[1, 5] = new Rook(Color.White);
            board[7, 7] = new King(Color.White);

            c.Board = board;

            var moves = c.GetAllAvailableMoves(Color.Black);

            Assert.AreEqual(0, moves.Count());
        }

        [TestMethod]
        public void GetLegalMoves_IHasSacrifice()
        {
            var c = new ChessBoard();
            var board = new Piece[8, 8];

            board[0, 0] = new King(Color.Black);
            board[5, 1] = new Queen(Color.Black);
            board[0, 5] = new Queen(Color.White);
            board[1, 5] = new Rook(Color.White);
            board[7, 7] = new King(Color.White);

            c.Board = board;

            var moves = c.GetAllAvailableMoves(Color.Black);

            Assert.AreEqual(1, moves.Count());
        }

        [TestMethod]
        public void KnightAttacksOwnQueen()
        {
            var c = new ChessBoard();
            var board = new Piece[8, 8];

            board[0, 0] = new Rook(Color.Black);
            board[0, 2] = new Bishop(Color.Black);
            board[0, 3] = new Queen(Color.Black);
            board[0, 5] = new Bishop(Color.Black);
            board[0, 7] = new Rook(Color.Black);

            board[2, 0] = new Pawn(Color.Black);
            board[1, 1] = new Pawn(Color.Black);
            board[1, 2] = new Pawn(Color.Black);
            board[3, 3] = new Pawn(Color.Black);
            board[1, 4] = new Pawn(Color.Black);
            board[1, 5] = new Pawn(Color.Black);
            board[1, 6] = new Pawn(Color.Black);
            board[1, 7] = new Pawn(Color.Black);

            board[2, 3] = new King(Color.Black);
            board[2, 2] = new Knight(Color.Black);
            board[2, 5] = new Knight(Color.Black);

            c.Board = board;

            var moves = c.GetAllAvailableMoves(board[2, 2]);

            Assert.AreEqual(6, moves.Count());
        }

        [TestMethod]
        public void InCheckWhiteIllegalMove()
        {
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

            Assert.AreEqual(6, allMoves.Count());
        }

        [TestMethod]
        public void GetLegalMoves_Knight()
        {
            var c = new ChessBoard();
            var board = new Piece[8, 8];

            board[4,3] = new King(Color.White);
            board[4,4] = new Knight(Color.White);

            c.Board = board;

            Assert.AreEqual(8, c.GetAllAvailableMoves(board[4, 4]).Count());
        }

        [TestMethod]
        public void GetLegalMoves_PawnHas2MovesBeforeMoving()
        {
            var c = new ChessBoard();
            var board = new Piece[8, 8];

            board[0, 3] = new King(Color.Black);
            board[1, 3] = new Pawn(Color.Black);

            c.Board = board;

            Assert.AreEqual(2, c.GetAllAvailableMoves(board[1, 3]).Count());
        }

        [TestMethod]
        public void GetLegalMoves_PawnHas1MovesAfterMoving()
        {
            var c = new ChessBoard();
            var board = new Piece[8, 8];

            board[0, 3] = new King(Color.Black);
            board[1, 3] = new Pawn(Color.Black);

            board[7,7] = new King(Color.White);

            c.Board = board;

            c.MakeMove(new Move {Piece = board[1, 3], TargetPosition = new Position(2, 3)});

            Assert.AreEqual(1, c.GetAllAvailableMoves(board[1, 3]).Count());
        }

        [TestMethod]
        public void GetKingPosition_Black()
        {
            var c = new ChessBoard();
            var board = new Piece[8, 8];

            board[0, 3] = new King(Color.Black);
            board[5, 3] = new King(Color.White);

            c.Board = board;

            Assert.AreEqual(new Position(0, 3), c.GetKingPosition(Color.Black));
        }

        [TestMethod]
        public void GetKingPosition_White()
        {
            var c = new ChessBoard();
            var board = new Piece[8, 8];

            board[0, 3] = new King(Color.Black);
            board[5, 3] = new King(Color.White);

            c.Board = board;

            Assert.AreEqual(new Position(5, 3), c.GetKingPosition(Color.White));
        }

        [TestMethod]
        public void QueenBlockedAllWays_0Moves()
        {
            var c = new ChessBoard();

            Assert.AreEqual(0, c.GetAllAvailableMoves(c.Board[7, 3]).Count());
        }

        [TestMethod]
        public void BlackQueenBlockedFrontOpen_1Move()
        {
            var c = new ChessBoard();

            var move = new Move
            {
                Piece = c.GetPieceAtPosition(3, 1),
                TargetPosition = new Position(2, 3)
            };

            c.MakeMove(move);

            Assert.AreEqual(1, c.GetAllAvailableMoves(c.Board[0, 3]).Count());
        }

        [TestMethod]
        public void BlackKingBlockedFrontOpen_1Move()
        {
            var c = new ChessBoard();

            var move = new Move
            {
                Piece = c.GetPieceAtPosition(4, 1),
                TargetPosition = new Position(2, 4)
            };

            c.MakeMove(move);

            Assert.AreEqual(1, c.GetAllAvailableMoves(c.Board[0, 4]).Count());
        }

        [TestMethod]
        public void WhiteQueenBlockedFrontOpen_1Move()
        {
            var c = new ChessBoard();

            var move = new Move
            {
                Piece = c.GetPieceAtPosition(3, 6),
                TargetPosition = new Position(5, 3)
            };

            c.MakeMove(move);

            Assert.AreEqual(1, c.GetAllAvailableMoves(c.Board[7, 3]).Count());
        }

        [TestMethod]
        public void PawnAttacksDiagonal()
        {
            var c = new ChessBoard();

            var board = new Piece[8, 8];

            board[1,0] = new Pawn(Color.Black);
            board[2,1] = new Rook(Color.White);

            c.Board = board;

            Assert.AreEqual(3, c.GetAllAvailableMoves(c.Board[1,0]).Count());
        }

        [TestMethod]
        public void PawnNoMovesIfBlocked()
        {
            var c = new ChessBoard();

            var board = new Piece[8, 8];

            board[1, 0] = new Pawn(Color.Black);
            board[2, 0] = new Rook(Color.White);

            c.Board = board;

            Assert.AreEqual(0, c.GetAllAvailableMoves(c.Board[1, 0]).Count());
        }

        [TestMethod]
        public void PawnAtEndBecomesQueen()
        {
            var c = ChessBoard.CreateFromFenString("rn1qk2r/1pP3p1/8/pb1n3p/N7/7P/PPPbNPP1/R1BQKBR1");

            var move = new Move
            {
                Piece = c.Board[1, 2],
                TargetPosition = new Position(0, 2)
            };

            c.MakeMove(move);

            Assert.IsTrue(c.Board[0, 2] is Queen);
        }
    }
}
