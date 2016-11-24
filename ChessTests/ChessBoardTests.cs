using System;
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

            c.Board = board;

            var moves = c.GetAllAvailableMoves(Color.Black);

            Assert.AreEqual(1, moves.Count());
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

            c.Board = board;

            c.MakeMove(new Move {Piece = board[1, 3], TargetPosition = new Position(2, 3)});

            Assert.AreEqual(1, c.GetAllAvailableMoves(board[1, 3]).Count());
        }
    }
}
