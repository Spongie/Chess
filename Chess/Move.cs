using Chess.Pieces;

namespace Chess
{
    public class Move
    {
        public Piece Piece { get; set; }
        public Position TargetPosition { get; set; }
    }
}