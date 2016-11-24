using System.Collections.Generic;

namespace Chess.Pieces
{
    public class King : Piece
    {
        public King(Color color) : base(color)
        {
            MaxMoveLength = 1;
        }

        public override IEnumerable<Move> GetLegalMoves(ChessBoard board)
        {
            var legalMoves = new List<Move>();

            legalMoves.AddRange(GetVerticalUpMoves(board));
            legalMoves.AddRange(GetVerticalDownMoves(board));
            legalMoves.AddRange(GetHorizontalMoves(board));
            legalMoves.AddRange(GetDiagonalMoves(board));

            return legalMoves;
        }
    }
}