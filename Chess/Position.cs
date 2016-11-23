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
    }
}