using System.Collections.Generic;

namespace AStar
{
    public sealed class Grid
    {
        public readonly IEnumerable<Node> Nodes;

        public Grid(IEnumerable<Node> nodes)
        {
            Nodes = nodes;
        }

        public Node EndNode { get; set; }
        public Node StartNode { get; set; }

        public void Reset()
        {
            EndNode = null;
            StartNode = null;
        }
    }
}