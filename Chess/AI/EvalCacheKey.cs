using System;
using Chess.Pieces;

namespace Chess.AI
{
    [Serializable]
    public class EvalCacheKey
    {
        public Color Color { get; set; }
        public string FenString { get; }

        public EvalCacheKey()
        {
            
        }

        public EvalCacheKey(string fenString, Color color)
        {
            FenString = fenString;
            Color = color;
        }

        public EvalCacheKey(string fenString)
        {
            FenString = fenString;
        }

        public override bool Equals(object obj)
        {
            var other = obj as EvalCacheKey;

            if (other == null)
                return false;

            return other.FenString == FenString;
        }

        public override int GetHashCode()
        {
            return FenString.GetHashCode();
        }
    }
}
