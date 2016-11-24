using System;
using System.Collections.Generic;
using System.Linq;
using Chess.Pieces;

namespace Chess
{
    public class ChessBoard
    {
        private const int BoardSize = 8;

        public ChessBoard()
        {
            Board = new Piece[BoardSize, BoardSize];

            GenerateBlackSide();
            GenerateWhiteSide();
        }
        
        public Piece[,] Board { get; set; }

        private void GenerateWhiteSide()
        {
            Board[7, 0] = new Rook(Color.White);
            Board[7, 1] = new Knight(Color.White);
            Board[7, 2] = new Bishop(Color.White);
            Board[7, 3] = new Queen(Color.White);
            Board[7, 4] = new King(Color.White);
            Board[7, 5] = new Bishop(Color.White);
            Board[7, 6] = new Knight(Color.White);
            Board[7, 7] = new Rook(Color.White);

            for (int i = 0; i < BoardSize; i++)
            {
                Board[6, i] = new Pawn(Color.White);
            }
        }

        private void GenerateBlackSide()
        {
            Board[0, 0] = new Rook(Color.Black);
            Board[0, 1] = new Knight(Color.Black);
            Board[0, 2] = new Bishop(Color.Black);
            Board[0, 3] = new Queen(Color.Black);
            Board[0, 4] = new King(Color.Black);
            Board[0, 5] = new Bishop(Color.Black);
            Board[0, 6] = new Knight(Color.Black);
            Board[0, 7] = new Rook(Color.Black);

            for (int i = 0; i < BoardSize; i++)
            {
                Board[1, i] = new Pawn(Color.Black);
            }
        }

        public Piece[,] GetBoardAfterMove(Move move)
        {
            var newBoard = (Piece[,])Board.Clone();
            var position = GetPiecePosition(move.Piece);

            newBoard[position.Y, position.X] = null;
            newBoard[move.TargetPosition.Y, move.TargetPosition.X] = move.Piece;

            return newBoard;
        }

        private Position GetKingPosition(Color color, Piece[,] board)
        {
            for (int x = 0; x < BoardSize; x++)
            {
                for (int y = 0; y < BoardSize; y++)
                {
                    if (board[y, x] != null && board[y, x] is King && board[y, x].Color == color)
                        return new Position(y, x);
                }
            }

            throw new InvalidOperationException("Error no king noob");
        }

        public void MakeMove(Move move)
        {
            Board = GetBoardAfterMove(move);
            move.Piece.OnMoved();
        }

        public IEnumerable<Move> GetAllAvailableMoves(Piece piece)
        {
            return piece.GetLegalMoves(this);
        }

        public bool IsInCheckAfterMove(Color color, Move move)
        {
            var boardAfterMove = GetBoardAfterMove(move);
            var kingPosition = GetKingPosition(color, boardAfterMove);

            var moves = GetAllAvailableMovesWithBoard(InvertColor(color), new ChessBoard {Board = boardAfterMove}, false);

            return moves.Any(opponentMove => opponentMove.TargetPosition.Equals(kingPosition));
        }

        public IEnumerable<Move> GetAllAvailableMovesWithBoard(Color color, ChessBoard board, bool checkIfCheck = true)
        {
            var legalMoves = new List<Move>();

            foreach (Piece piece in board.Board)
            {
                if (piece != null && piece.Color == color)
                    legalMoves.AddRange(piece.GetLegalMoves(board));
            }

            if (checkIfCheck)
                return legalMoves.Where(move => !IsInCheckAfterMove(color, move));

            return legalMoves;
        }

        public IEnumerable<Move> GetAllAvailableMoves(Color color)
        {
            return GetAllAvailableMovesWithBoard(color, this);
        }

        public static Color InvertColor(Color color)
        {
            return (Color)((int)color * -1);
        }

        public void GetAvailableMoves(Piece piece)
        {
            
        }

        public Position GetPiecePosition(Piece piece)
        {
            for (int x = 0; x < BoardSize; x++)
            {
                for (int y = 0; y < BoardSize; y++)
                {
                    if (Board[y, x] == piece)
                        return new Position(y, x);
                }
            }

            throw new ArgumentOutOfRangeException(nameof(piece), piece, "Piece not found on board");
        }

        public bool IsPositionInsideBoard(int x, int y)
        {
            return x >= 0 && x < BoardSize && y >= 0 && y < BoardSize;
        }

        public Piece GetPieceAtPosition(int x, int y)
        {
            return Board[y, x];
        }
    }
}
