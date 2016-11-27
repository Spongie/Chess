using System.Collections.Generic;
using Chess.Pieces;

namespace Chess
{
    public class Move
    {
        private Dictionary<int, string> xToLetterCache;

        public Move()
        {
            xToLetterCache = new Dictionary<int, string>();
            xToLetterCache.Add(0, "A");
            xToLetterCache.Add(1, "B");
            xToLetterCache.Add(2, "C");
            xToLetterCache.Add(3, "D");
            xToLetterCache.Add(4, "E");
            xToLetterCache.Add(5, "F");
            xToLetterCache.Add(6, "G");
            xToLetterCache.Add(7, "H");
        }

        public Piece Piece { get; set; }
        public Position TargetPosition { get; set; }

        public override string ToString()
        {
            return $"{Piece.Color}: {Piece.GetName()} -> {xToLetterCache[TargetPosition.X]}{8 - TargetPosition.Y}";
        }
    }
}