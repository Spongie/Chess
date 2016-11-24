using System.Collections.Generic;

namespace Chess.Pieces
{
    public class Bishop : Piece
    {
        public Bishop(Color color) : base(color)
        {
            MaxMoveLength = 7;
        }

        public override IEnumerable<Move> GetLegalMoves()
        {
            return GetDiagonalMoves();
        }
    }
}