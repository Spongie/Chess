using System.Collections.Generic;

namespace Chess.Pieces
{
    internal class King : Piece
    {
        public King(Color color) : base(color)
        {
            MaxMoveLength = 1;
        }

        public override IEnumerable<Move> GetLegalMoves()
        {
            throw new System.NotImplementedException();
        }
    }
}