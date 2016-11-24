namespace Chess
{
    public class Position
    {
        public Position(int y, int x)
        {
            Y = y;
            X = x;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public override bool Equals(object obj)
        {
            var otherPosition = obj as Position;

            if (otherPosition == null)
                return false;

            return otherPosition.X == X && otherPosition.Y == Y;
        }
    }
}