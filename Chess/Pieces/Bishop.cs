using System.Collections.Generic;

namespace Chess.Pieces
{
    public class Bishop : Piece
    {
        public Bishop(Color color) : base(color)
        {
            MaxMoveLength = 7;
        }

        public override IEnumerable<Move> GetLegalMoves(ChessBoard board)
        {
            return GetDiagonalMoves(board);
        }
    }
}