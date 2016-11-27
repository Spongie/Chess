using Chess.Pieces;

namespace Chess.AI
{
    public class EvalCacheKey
    {
        private readonly Color color;
        private readonly string fenString;

        public EvalCacheKey(string fenString, Color color)
        {
            this.fenString = fenString;
            this.color = color;
        }

        public override bool Equals(object obj)
        {
            var other = obj as EvalCacheKey;

            if (other == null)
                return false;

            return other.fenString == fenString && other.color == color;
        }

        public override int GetHashCode()
        {
            return (fenString + color).GetHashCode();
        }
    }
}
