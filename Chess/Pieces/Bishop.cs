using System;
using System.Collections.Generic;

namespace Chess.Pieces
{
    [Serializable]
    public class Bishop : Piece
    {
        public Bishop()
        {
            
        }

        public Bishop(Color color) : base(color)
        {
            MaxMoveLength = 7;
        }

        public override IEnumerable<Move> GetLegalMoves(ChessBoard board)
        {
            return GetDiagonalMoves(board);
        }

        public override string GetFenRepresentation()
        {
            return Color == Color.Black ? "b" : "B";
        }

        public override string GetName()
        {
            return "Bishop";
        }
    }
}