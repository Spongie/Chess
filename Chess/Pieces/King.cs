using System.Collections.Generic;

namespace Chess.Pieces
{
    public class King : Piece
    {
        public King(Color color) : base(color)
        {
            MaxMoveLength = 1;
        }

        public override IEnumerable<Move> GetLegalMoves()
        {
            var legalMoves = new List<Move>();

            legalMoves.AddRange(GetVerticalUpMoves());
            legalMoves.AddRange(GetVerticalDownMoves());
            legalMoves.AddRange(GetHorizontalMoves());
            legalMoves.AddRange(GetDiagonalMoves());

            return legalMoves;
        }
    }
}