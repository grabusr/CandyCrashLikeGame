using NUnit.Framework;
using NSubstitute;

namespace LocalModel.Tests
{
    public class RandomBoardCreatorTests
    {
        [Test]
        public void ExampleTest()
        {
            var randomDevice = Substitute.For<IRandomTypeProvider>();
            Assert.IsTrue(true);
        }
    }

} // namespace LocalModel.Tests