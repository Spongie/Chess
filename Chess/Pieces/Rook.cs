using System.Collections.Generic;

namespace Chess.Pieces
{
    public class Rook : Piece
    {
        public Rook(Color color) : base(color)
        {
            MaxMoveLength = 7;
        }

        public override IEnumerable<Move> GetLegalMoves()
        {
            var legalMoves = new List<Move>();

            legalMoves.AddRange(GetVerticalUpMoves());
            legalMoves.AddRange(GetVerticalDownMoves());
            legalMoves.AddRange(GetHorizontalMoves());

            return legalMoves;
        }
    }
}
