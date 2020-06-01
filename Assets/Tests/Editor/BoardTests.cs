using NUnit.Framework;
using System.Collections.Generic;
using QuickTurnStudio.CandyCrashLike.Core;

namespace QuickTurnStudio.CandyCrashLike.LocalModel.Tests
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
            sut[0, 0] = new BlockData(1);
            sut[0, 1] = new BlockData(1);
            sut[0, 2] = new BlockData(0);

            sut[1, 0] = new BlockData(2);
            sut[1, 1] = new BlockData(0);
            sut[1, 2] = new BlockData(1);

            sut[2, 0] = new BlockData(0);
            sut[2, 1] = new BlockData(2);
            sut[2, 2] = new BlockData(0);

            // when:
            sut.SwapFields(new Coordinate(1, 0), new Coordinate(1, 1));

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
            sut[0, 0] = new BlockData(1);
            sut[0, 1] = new BlockData(1);
            sut[0, 2] = new BlockData(0);
            sut[0, 3] = new BlockData(2);

            sut[1, 0] = new BlockData(2);
            sut[1, 1] = new BlockData(0);
            sut[1, 2] = new BlockData(1);
            sut[1, 3] = new BlockData(1);

            // when:
            sut.SwapFields(new Coordinate(0, 3), new Coordinate(1, 3));

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
        public void BoardDetectsMatchesWithMinimumLength()
        {
            // given:
            const int rowsCount = 3;
            const int columnsCount = 3;
            const int minimalMatchCount = 3;
            // 0 2 2
            // 2 0 0
            // 1 1 1
            var sut = new Board(rowsCount, columnsCount);
            sut[0, 0] = new BlockData(1);
            sut[0, 1] = new BlockData(1);
            sut[0, 2] = new BlockData(1);

            sut[1, 0] = new BlockData(2);
            sut[1, 1] = new BlockData(0);
            sut[1, 2] = new BlockData(0);

            sut[2, 0] = new BlockData(0);
            sut[2, 1] = new BlockData(2);
            sut[2, 2] = new BlockData(2);

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
        public void BoardDetectsMatchesLongerThanMinimalLength()
        {
            // given:
            const int minimalMatchCount = 3;
            var types = new List<int[]>
            {
                new int[]{ 2, 0, 2, 2, 0, 1, },
                new int[]{ 1, 2, 2, 2, 2, 1, },
                new int[]{ 3, 1, 0, 1, 0, 1, },
                new int[]{ 1, 2, 2, 0, 1, 1, },
                new int[]{ 0, 0, 2, 0, 1, 2, },
            };
            var sut = CreateBoardWithElements(types);

            // when:
            var matches = sut.GetMatches(minimalMatchCount);

            // then:
            Assert.AreEqual(2, matches.Count);

            var verticalMatch = matches[1];
            Assert.AreEqual(4, verticalMatch.Length);
            Assert.Contains(new Coordinate(3, 1), verticalMatch);
            Assert.Contains(new Coordinate(3, 2), verticalMatch);
            Assert.Contains(new Coordinate(3, 3), verticalMatch);
            Assert.Contains(new Coordinate(3, 4), verticalMatch);

            var horizontalMatch = matches[0];
            Assert.AreEqual(4, horizontalMatch.Length);            
            Assert.Contains(new Coordinate(1, 5), horizontalMatch);
            Assert.Contains(new Coordinate(2, 5), horizontalMatch);
            Assert.Contains(new Coordinate(3, 5), horizontalMatch);
            Assert.Contains(new Coordinate(4, 5), horizontalMatch);
        }

        [Test]
        public void BoardApplyGravityCorrectlyAfterRemovingBlocksHorizontally()
        {
            // given:
            const int rowsCount = 3;
            const int columnsCount = 2;
            const int matchCount = 2;
            // 0 2
            // 2 0
            // 1 1
            var sut = new Board(rowsCount, columnsCount);
            sut[0, 0] = new BlockData(1);
            sut[0, 1] = new BlockData(1);
            
            sut[1, 0] = new BlockData(2);
            sut[1, 1] = new BlockData(0);

            sut[2, 0] = new BlockData(0);
            sut[2, 1] = new BlockData(2);

            var expectedMove1 = new MoveElementData(new Coordinate(1, 0), new Coordinate(0, 0));
            var expectedMove2 = new MoveElementData(new Coordinate(1, 1), new Coordinate(0, 1));
            var expectedMove3 = new MoveElementData(new Coordinate(2, 0), new Coordinate(1, 0));
            var expectedMove4 = new MoveElementData(new Coordinate(2, 1), new Coordinate(1, 1));

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
            Assert.AreEqual(BlockData.invalidColorId, sut[2, 0].Type);
            Assert.AreEqual(BlockData.invalidColorId, sut[2, 1].Type);

            Assert.AreEqual(4, movedElements.Length);

            Assert.AreEqual(movedElements[0], expectedMove1);
            Assert.AreEqual(movedElements[1], expectedMove2);
            Assert.AreEqual(movedElements[2], expectedMove3);
            Assert.AreEqual(movedElements[3], expectedMove4);
        }

        [Test]
        public void BoardApplyGravityCorrectlyAfterRemovingBlocksVertically()
        {
            // given:
            const int minimalMatchCount = 3;
            var types = new List<int[]>
            {
                new int[]{ 2, 2, },
                new int[]{ 3, 1, },
                new int[]{ 1, 2, },
                new int[]{ 1, 0, },
                new int[]{ 1, 2, },
                new int[]{ 4, 2, },
            };

            var expectedMove1 = new MoveElementData(new Coordinate(4, 0), new Coordinate(1, 0));
            var expectedMove2 = new MoveElementData(new Coordinate(5, 0), new Coordinate(2, 0));

            var sut = CreateBoardWithElements(types);
            var matches = sut.GetMatches(minimalMatchCount);

            // when:
            var movedElements = sut.RemoveBlocks(matches);

            // then:
            // -1 2
            // -1 1
            // -1 2
            //  2 0
            //  3 2
            //  4 2

            Assert.AreEqual(4, sut[0, 0].Type);
            Assert.AreEqual(3, sut[1, 0].Type);
            Assert.AreEqual(2, sut[2, 0].Type);
            Assert.AreEqual(BlockData.invalidColorId, sut[3, 0].Type);
            Assert.AreEqual(BlockData.invalidColorId, sut[4, 0].Type);
            Assert.AreEqual(BlockData.invalidColorId, sut[5, 0].Type);

            Assert.AreEqual(2, sut[0, 1].Type);
            Assert.AreEqual(2, sut[1, 1].Type);
            Assert.AreEqual(0, sut[2, 1].Type);
            Assert.AreEqual(2, sut[3, 1].Type);
            Assert.AreEqual(1, sut[4, 1].Type);
            Assert.AreEqual(2, sut[5, 1].Type);

            Assert.AreEqual(2, movedElements.Length);
            Assert.AreEqual(movedElements[0], expectedMove1);
            Assert.AreEqual(movedElements[1], expectedMove2);
        }

        private Board CreateBoardWithElements(List<int[]> types)
        {
            var board = new Board(types.Count, types[0].Length);
            for (var i = 0; i < types.Count; ++i)
            {
                var row = types.Count - 1 - i;
                for (var column = 0; column < types[i].Length; ++column)
                {
                    board[row, column] = new BlockData(types[i][column]);
                }
            }
            return board;
        }
    }

}
