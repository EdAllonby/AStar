namespace AStar
{
    public sealed class Node
    {
        private readonly Coordinate position;

        public Node(Coordinate position)
        {
            this.position = position;
        }

        public Coordinate Position
        {
            get { return position; }
        }
    }
}