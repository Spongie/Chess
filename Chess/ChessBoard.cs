using System;
using System.Collections.Generic;
using System.Linq;
using Chess.Pieces;

namespace Chess
{
    public class ChessBoard
    {
        private static ChessBoard instance;
        private Piece[,] board;

        public ChessBoard()
        {
            instance = this;
            board = new Piece[8,8];

            GenerateBlackSide();
            GenerateWhiteSide();
        }

        public static ChessBoard Instance => instance;

        private void GenerateWhiteSide()
        {
            board[7, 0] = new Rook(Color.White);
            board[7, 1] = new Knight(Color.White);
            board[7, 2] = new Bishop(Color.White);
            board[7, 3] = new Queen(Color.White);
            board[7, 4] = new King(Color.White);
            board[7, 5] = new Bishop(Color.White);
            board[7, 6] = new Knight(Color.White);
            board[7, 7] = new Rook(Color.White);

            for (int i = 0; i < 8; i++)
            {
                board[6, i] = new Pawn(Color.White);
            }
        }

        private void GenerateBlackSide()
        {
            board[0, 0] = new Rook(Color.Black);
            board[0, 1] = new Knight(Color.Black);
            board[0, 2] = new Bishop(Color.Black);
            board[0, 3] = new Queen(Color.Black);
            board[0, 4] = new King(Color.Black);
            board[0, 5] = new Bishop(Color.Black);
            board[0, 6] = new Knight(Color.Black);
            board[0, 7] = new Rook(Color.Black);

            for (int i = 0; i < 8; i++)
            {
                board[1, i] = new Pawn(Color.Black);
            }
        }

        public void GetAvailableMoves(Piece piece)
        {
            
        }

        public Position GetPiecePosition(Piece piece)
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    if (board[y, x] == piece)
                        return new Position(y, x);
                }
            }

            throw new ArgumentOutOfRangeException("FUCKING ERROR NOOB");
        }

        public bool IsPositionInsideBoard(int x, int y)
        {
            return x >= 0 && x < 8 && y >= 0 && y < 8;
        }

        public Piece GetPieceAtPosition(int x, int y)
        {
            return board[y, x];
        }
    }
}
