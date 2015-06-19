using System.Collections.Generic;

namespace AStar
{
    public sealed class Grid
    {
        private readonly List<Node> nodes;

        public Grid(List<Node> nodes)
        {
            this.nodes = nodes;
        }

        public Node EndNode { get; set; }
        public Node StartNode { get; set; }

        public IEnumerable<Node> Nodes
        {
            get { return nodes; }
        }

        public void CalculateHeuristics(IHeuristicCalculator calculator)
        {
            foreach (Node node in nodes)
            {
                calculator.CalculateHeuristic(node, EndNode);
            }
        }
    }
}