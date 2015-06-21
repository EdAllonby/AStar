using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace AStar.Tests
{
    [TestFixture]
    public sealed class ManhattanCalculatorTests
    {
        [Test]
        public void CalculateHeuristics_WhenANodeIsTwoNodesLeftOfTarget_UsingManhattan()
        {
            Node endNode = new Node(new Coordinate(3, 2));
            Node startNode = new Node(new Coordinate(1, 2));

            var calculator = new ManhattanCalculator();

            calculator.CalculateHeuristic(startNode, endNode);

            Assert.AreEqual(startNode.Heuristic, 2);
        }

        [Test]
        public void CalculateHeuristics_EndNodeShouldHaveAHeuristicOf0_UsingManhattan()
        {
            Node endNode = new Node(new Coordinate(2, 2));

            var calculator = new ManhattanCalculator();

            calculator.CalculateHeuristic(endNode, endNode);

            Assert.AreEqual(endNode.Heuristic, 0);
        }
    }
}
