using System.Collections.Generic;
using Chess.Pieces;

namespace Chess
{
    public class Move
    {
        private static readonly Dictionary<int, string> xToLetterCache = new Dictionary<int, string>
        {
            {0, "A"},
            {1, "B"},
            {2, "C"},
            {3, "D"},
            {4, "E"},
            {5, "F"},
            {6, "G"},
            {7, "H"}
        };
        
        public Piece Piece { get; set; }
        public Position TargetPosition { get; set; }
        public Position FromPosition { get; set; }
        public bool IsCastleMove { get; set; }
        public Rook CastleRook { get; set; }
        public Position RookTargetPosition { get; set; }

        public override string ToString()
        {
            return $"{Piece.Color}: {Piece.GetName()} -> {xToLetterCache[TargetPosition.X]}{8 - TargetPosition.Y}";
        }

        public string GetUCIString()
        {
            return $"{xToLetterCache[FromPosition.X].ToLower()}{8 - FromPosition.Y}{xToLetterCache[TargetPosition.X].ToLower()}{8 - TargetPosition.Y}";
        }
    }
}