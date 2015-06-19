using NUnit.Framework;

namespace AStar.Tests
{
    [TestFixture]
    public class NodeTests
    {
        [Test]
        public void NewNode_WithCoordinates12_HasCoordinates12()
        {
            int x = 1;
            int y = 2;

            var node = new Node(new Coordinate(x, y));

            Assert.AreEqual(1, node.Position.X);
            Assert.AreEqual(2, node.Position.Y);
        }
    }
}