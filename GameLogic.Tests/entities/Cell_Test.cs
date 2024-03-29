using GameLogic.Entities;
using NUnit.Framework;

namespace GameLogic.UnitTests
{
    [TestFixture]
    public class Cell_Test
    {
        [Test]
        [TestCase(0, 4, 1, 1, 1, 5)]
        [TestCase(1, 5, -1, -1, 0, 4)]
        [TestCase(0, 4, 0, 0, 0, 4)]
        public void CellShift(int xCell, int yCell, int xShift, int yShift, int xExpectedCell, int yExpectedCell)
        {

            var cell = new Cell(xCell, yCell);
            var shift = new Cell(xShift, yShift);
            var expectedCell = new Cell(xExpectedCell, yExpectedCell);

            Assert.That(expectedCell, Is.EqualTo(cell + shift));
        }
    }
}
