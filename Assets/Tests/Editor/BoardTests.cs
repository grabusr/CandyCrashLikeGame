using NUnit.Framework;
using core;

namespace LocalModel.Tests
{
    public class BoardTests
    {
        [Test]
        public void BoardSwapBlocksHorizontallyCorrectly()
        {
            // given:
            const int rowsCount = 3;
            const int columnsCount = 3;
            // 0 2 0
            // 2 0 1
            // 1 1 0
            var sut = new Board(rowsCount, columnsCount);
            sut[0, 0] = new core.BlockData(1);
            sut[0, 1] = new core.BlockData(1);
            sut[0, 2] = new core.BlockData(0);

            sut[1, 0] = new core.BlockData(2);
            sut[1, 1] = new core.BlockData(0);
            sut[1, 2] = new core.BlockData(1);

            sut[2, 0] = new core.BlockData(0);
            sut[2, 1] = new core.BlockData(2);
            sut[2, 2] = new core.BlockData(0);

            // when:
            sut.SwapFields(new core.Coordinate(1, 0), new core.Coordinate(1, 1));

            // then:
            // 0 2 0
            // 0 2 1
            // 1 1 0
            Assert.AreEqual(1, sut[0, 0].Type);
            Assert.AreEqual(1, sut[0, 1].Type);
            Assert.AreEqual(0, sut[0, 2].Type);

            Assert.AreEqual(0, sut[1, 0].Type);
            Assert.AreEqual(2, sut[1, 1].Type);
            Assert.AreEqual(1, sut[1, 2].Type);

            Assert.AreEqual(0, sut[2, 0].Type);
            Assert.AreEqual(2, sut[2, 1].Type);
            Assert.AreEqual(0, sut[2, 2].Type);
        }

        [Test]
        public void BoardSwapBlocksVerticallyCorrectly()
        {
            // given:
            const int rowsCount = 2;
            const int columnsCount = 4;
            // 2 0 1 1
            // 1 1 0 2
            var sut = new Board(rowsCount, columnsCount);
            sut[0, 0] = new core.BlockData(1);
            sut[0, 1] = new core.BlockData(1);
            sut[0, 2] = new core.BlockData(0);
            sut[0, 3] = new core.BlockData(2);

            sut[1, 0] = new core.BlockData(2);
            sut[1, 1] = new core.BlockData(0);
            sut[1, 2] = new core.BlockData(1);
            sut[1, 3] = new core.BlockData(1);

            // when:
            sut.SwapFields(new core.Coordinate(0, 3), new core.Coordinate(1, 3));

            // then:
            // 2 0 1 2
            // 1 1 0 1
            Assert.AreEqual(1, sut[0, 0].Type);
            Assert.AreEqual(1, sut[0, 1].Type);
            Assert.AreEqual(0, sut[0, 2].Type);
            Assert.AreEqual(1, sut[0, 3].Type);

            Assert.AreEqual(2, sut[1, 0].Type);
            Assert.AreEqual(0, sut[1, 1].Type);
            Assert.AreEqual(1, sut[1, 2].Type);
            Assert.AreEqual(2, sut[1, 3].Type);
        }

        [Test]
        public void BoardDetectsMatches()
        {
            // given:
            const int rowsCount = 3;
            const int columnsCount = 3;
            const int minimalMatchCount = 3;
            // 0 2 2
            // 2 0 0
            // 1 1 1
            var sut = new Board(rowsCount, columnsCount);
            sut[0, 0] = new core.BlockData(1);
            sut[0, 1] = new core.BlockData(1);
            sut[0, 2] = new core.BlockData(1);

            sut[1, 0] = new core.BlockData(2);
            sut[1, 1] = new core.BlockData(0);
            sut[1, 2] = new core.BlockData(0);

            sut[2, 0] = new core.BlockData(0);
            sut[2, 1] = new core.BlockData(2);
            sut[2, 2] = new core.BlockData(2);

            // when:
            var matches = sut.GetMatches(minimalMatchCount);

            // then:
            Assert.AreEqual(1, matches.Count);

            var match = matches[0];
            Assert.AreEqual(3, match.Length);
            Assert.AreEqual(new Coordinate(0, 0), match[0]);
            Assert.AreEqual(new Coordinate(0, 1), match[1]);
            Assert.AreEqual(new Coordinate(0, 2), match[2]);

        }

        [Test]
        public void BoardApplyGravityCorrectlyAfterRemovingBlocksVertically()
        {
            // given:
            const int rowsCount = 3;
            const int columnsCount = 2;
            const int matchCount = 2;
            // 0 2
            // 2 0
            // 1 1
            var sut = new Board(rowsCount, columnsCount);
            sut[0, 0] = new core.BlockData(1);
            sut[0, 1] = new core.BlockData(1);
            
            sut[1, 0] = new core.BlockData(2);
            sut[1, 1] = new core.BlockData(0);

            sut[2, 0] = new core.BlockData(0);
            sut[2, 1] = new core.BlockData(2);

            var expectedMove1 = new core.MoveElementData(new core.Coordinate(1, 0), new core.Coordinate(0, 0));
            var expectedMove2 = new core.MoveElementData(new core.Coordinate(1, 1), new core.Coordinate(0, 1));
            var expectedMove3 = new core.MoveElementData(new core.Coordinate(2, 0), new core.Coordinate(1, 0));
            var expectedMove4 = new core.MoveElementData(new core.Coordinate(2, 1), new core.Coordinate(1, 1));

            var matches = sut.GetMatches(matchCount);

            // when:
            var movedElements = sut.RemoveBlocks(matches);

            // then:
            // -1 -1
            //  0  2
            //  2  0
            Assert.AreEqual(2, sut[0, 0].Type);
            Assert.AreEqual(0, sut[0, 1].Type);
            Assert.AreEqual(0, sut[1, 0].Type);
            Assert.AreEqual(2, sut[1, 1].Type);
            Assert.AreEqual(core.BlockData.invalidColorId, sut[2, 0].Type);
            Assert.AreEqual(core.BlockData.invalidColorId, sut[2, 1].Type);

            Assert.AreEqual(4, movedElements.Length);

            Assert.AreEqual(movedElements[0], expectedMove1);
            Assert.AreEqual(movedElements[1], expectedMove2);
            Assert.AreEqual(movedElements[2], expectedMove3);
            Assert.AreEqual(movedElements[3], expectedMove4);
        }
    }

} // namespace LocalModel.Tests
