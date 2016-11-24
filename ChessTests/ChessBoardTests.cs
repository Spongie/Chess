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
    }
}
