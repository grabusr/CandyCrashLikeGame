using System.Collections.Generic;

using NUnit.Framework;
using NSubstitute;

using core;

namespace LocalModel.Tests
{
    public class RandomBoardCreatorTests
    {
        [Test]
        public void GeneratingTest()
        {
            // given
            var gameConfig = Substitute.For<IGameConfig>();
            gameConfig.RowsCount.Returns(3);
            gameConfig.ColumnsCount.Returns(3);
            var pool = new BlockData[] { new BlockData(0), new BlockData(1), new BlockData(2) };
            gameConfig.BlockDataPool.Returns(pool);

            var randomDevice = Substitute.For<IBlockDataProvider>();
            randomDevice.GetBlockDataFromPool(Arg.Any<List<BlockData>>()).Returns(x => {
                // pick first option from pool
                var blockPool = (List<BlockData>)x[0];
                return blockPool[0];
            });

            var sut = new RandomBoardGenerator(randomDevice, gameConfig);

            // when:
            var generatedBoard = sut.CreateBoard();

            // then:
            // 0 0 1
            // 0 0 1
            // 1 1 0
            Assert.AreEqual(0, generatedBoard[0, 0].Type);
            Assert.AreEqual(0, generatedBoard[0, 1].Type);
            Assert.AreEqual(1, generatedBoard[0, 2].Type);
            Assert.AreEqual(0, generatedBoard[1, 0].Type);
            Assert.AreEqual(0, generatedBoard[1, 1].Type);
            Assert.AreEqual(1, generatedBoard[1, 2].Type);
            Assert.AreEqual(1, generatedBoard[2, 0].Type);
            Assert.AreEqual(1, generatedBoard[2, 1].Type);
            Assert.AreEqual(0, generatedBoard[2, 2].Type);
        }
    }

} // namespace LocalModel.Tests