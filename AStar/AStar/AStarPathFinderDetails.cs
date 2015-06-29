using System;
using System.Collections.Generic;

namespace AStar
{
    public sealed class AStarPathFinderDetails : EventArgs
    {
        private readonly Node newClosedNode;
        private readonly List<Node> openNodes;

        public AStarPathFinderDetails(List<Node> openNodes, Node newClosedNode)
        {
            this.openNodes = openNodes;
            this.newClosedNode = newClosedNode;
        }

        public List<Node> OpenNodes
        {
            get { return openNodes; }
        }

        public Node NewClosedNode
        {
            get { return newClosedNode; }
        }
    }
}