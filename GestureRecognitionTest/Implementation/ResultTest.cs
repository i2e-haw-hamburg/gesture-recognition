using GestureRecognition.Implementation.Pipeline.Interpreted;
using GestureRecognition.Interface.Commands;
using NUnit.Framework;

namespace GestureRecognitionTest.Pipeline.Interpreted
{
    [TestFixture]
    public class ResultTest
    {
        [Test]
        public void TestCompareTo()
        {
            var resultA = new Result(new ScaleAndRotate(), 1);
            var resultB = new Result(new ScaleAndRotate(), 1);
            var resultC = new Result(new ScaleAndRotate(), 0);
            var resultD = new Result(new ScaleAndRotate(), 1.01);
            Assert.AreEqual(0, resultA.CompareTo(resultB));
            Assert.AreEqual(-1, resultA.CompareTo(resultC));
            Assert.AreEqual(1, resultA.CompareTo(resultD));
        }

    }
}