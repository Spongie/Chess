using System.Collections.Generic;

namespace Chess.Pieces
{
    public class Queen : Piece
    {
        public Queen(Color color) : base(color)
        {
            MaxMoveLength = 7;
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