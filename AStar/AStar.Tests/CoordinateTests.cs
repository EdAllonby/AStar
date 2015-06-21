using NUnit.Framework;

namespace AStar.Tests
{
    [TestFixture]
    public class CoordinateTests
    {
        [Test]
        public void IsCoordinateNextTo_WithTwoCoordinatesDiagonalToEachOther_ReturnsTrue()
        {
            Coordinate first = new Coordinate(3, 3);
            Coordinate second = new Coordinate(2, 2);

            Assert.IsTrue(first.IsCoordinateNextTo(second));
        }

        [Test]
        public void IsCoordinateNextTo_WithTwoCoordinatesHorizontal_ReturnsTrue()
        {
            Coordinate first = new Coordinate(1, 2);
            Coordinate second = new Coordinate(2, 2);

            Assert.IsTrue(first.IsCoordinateNextTo(second));
        }

        [Test]
        public void IsCoordinateNextTo_WithTwoCoordinatesNotNextToEachOther_ReturnsFalse()
        {
            Coordinate first = new Coordinate(2, 5);
            Coordinate second = new Coordinate(2, 2);

            Assert.IsFalse(first.IsCoordinateNextTo(second));
        }

        [Test]
        public void IsCoordinateNextTo_WithTwoCoordinatesTwoPointsAwayFromEachOther_ReturnsFalse()
        {
            Coordinate first = new Coordinate(2, 4);
            Coordinate second = new Coordinate(2, 2);

            Assert.IsFalse(first.IsCoordinateNextTo(second));
        }

        [Test]
        public void IsCoordinateNextTo_WithTwoCoordinatesVertical_ReturnsTrue()
        {
            Coordinate first = new Coordinate(2, 3);
            Coordinate second = new Coordinate(2, 2);

            Assert.IsTrue(first.IsCoordinateNextTo(second));
        }
    }
}