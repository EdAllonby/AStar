using System;
using System.Collections.Generic;

namespace AStar
{
    public sealed class AStarPathFinderDetails : EventArgs
    {
        private readonly List<Node> closedNodes;
        private readonly List<Node> openNodes;

        public AStarPathFinderDetails(List<Node> openNodes, List<Node> closedNodes)
        {
            this.openNodes = openNodes;
            this.closedNodes = closedNodes;
        }

        public List<Node> OpenNodes
        {
            get { return openNodes; }
        }

        public List<Node> ClosedNodes
        {
            get { return closedNodes; }
        }
    }
}