using GestureRecognition.Implementation.Pipeline.Interpreted;
using NUnit.Framework;

namespace GestureRecognitionTest.Pipeline.Interpreted
{
    [TestFixture]
    public class ResultTest
    {
        [Test]
        public void TestCompareTo()
        {
            var resultA = new Result(null, 1, null);
            var resultB = new Result(null, 1, null);
            var resultC = new Result(null, 0, null);
            var resultD = new Result(null, 1.01, null);
            Assert.AreEqual(0, resultA.CompareTo(resultB));
            Assert.AreEqual(-1, resultA.CompareTo(resultC));
            Assert.AreEqual(1, resultA.CompareTo(resultD));
        }

    }
}