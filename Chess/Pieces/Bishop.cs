using System.Collections.Generic;

namespace Chess.Pieces
{
    internal class Bishop : Piece
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