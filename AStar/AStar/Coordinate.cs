namespace AStar
{
    public sealed class Coordinate
    {
        private readonly int x;
        private readonly int y;

        public Coordinate(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int X
        {
            get { return x; }
        }

        public int Y
        {
            get { return y; }
        }
    }
}