using System.Collections.Generic;

namespace Chess.Pieces
{
    public class Queen : Piece
    {
        public Queen(Color color) : base(color)
        {
            MaxMoveLength = 7;
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