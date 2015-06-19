namespace AStar
{
    public sealed class Node
    {
        private readonly Coordinate position;
        private int heuristic;

        public Node(Coordinate position)
        {
            this.position = position;
        }

        public Coordinate Position
        {
            get { return position; }
        }

        public int Heuristic
        {
            get { return heuristic; }
            set
            {
                if (value > 0)
                {
                    heuristic = value;
                }
            }
        }
    }
}