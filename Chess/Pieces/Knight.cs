using System.Collections.Generic;

namespace Chess.Pieces
{
    public class Knight : Piece
    {
        public Knight(Color color) : base(color)
        {
            MaxMoveLength = 7;
        }

        public override IEnumerable<Move> GetLegalMoves()
        {
            throw new System.NotImplementedException();
        }
    }
}