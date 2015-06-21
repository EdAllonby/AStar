using System;

namespace AStar
{
    public sealed class Coordinate : IEquatable<Coordinate>
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

        public bool IsCoordinateNextTo(Coordinate other)
        {
            if (Equals(other))
            {
                return false;
            }

            return Math.Abs(other.X - X) <= 1 && Math.Abs(other.Y - Y) <= 1;
        }

        public bool IsCoordinateDiagonal(Coordinate other)
        {
            if (Equals(other))
            {
                return false;
            }

            return Math.Abs(other.X - X) == 1 && Math.Abs(other.Y - Y) == 1;
        }

        public bool Equals(Coordinate other)
        {
            return other.X == X && other.Y == Y;
        }
    }
}