using System;
using Chess.Pieces;

namespace Chess.AI
{
    [Serializable]
    public class EvalCacheKey
    {
        public Color Color1 { get; set; }
        public string FenString { get; set; }

        public EvalCacheKey()
        {
            
        }

        public EvalCacheKey(string fenString, Color color)
        {
            this.FenString = fenString;
            this.Color1 = color;
        }

        public override bool Equals(object obj)
        {
            var other = obj as EvalCacheKey;

            if (other == null)
                return false;

            return other.FenString == FenString && other.Color1 == Color1;
        }

        public override int GetHashCode()
        {
            return (FenString + Color1).GetHashCode();
        }
    }
}
