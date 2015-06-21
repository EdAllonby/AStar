namespace AStar
{
    public sealed class Node
    {
        private readonly Coordinate position;
        private int heuristic;
        private int moveCost;
        private Node parent;

        public Node(Coordinate position)
        {
            this.position = position;
        }

        public Node Parent
        {
            get { return parent; }
            set
            {
                if (value != null)
                {
                    parent = value;
                }
            }
        }

        public int F
        {
            get { return Heuristic + MoveCost; }
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

        public int MoveCost
        {
            get { return moveCost; }
            set
            {
                if (value > 0)
                {
                    moveCost = value;
                }
            }
        }

        public bool IsNew
        {
            get { return MoveCost == 0; }
        }
    }
}