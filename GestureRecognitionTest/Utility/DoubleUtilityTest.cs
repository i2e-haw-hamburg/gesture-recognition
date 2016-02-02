using System.Collections.Generic;
using GestureRecognition.Utility;
using NUnit;
using NUnit.Framework;

namespace GestureRecognitionTest.Utility
{
    [TestFixture]
    public class DoubleUtilityTest
    {
        [Test]
        public void TestIsBetween()
        {
            Assert.IsTrue(1.5d.IsBetween(1, 2));
            Assert.IsTrue(1.5d.IsBetween(1.5d, 1.5d));
            Assert.IsTrue(1.5d.IsBetween(2, 1));
            Assert.IsTrue((-1.5d).IsBetween(-1, -2));
            Assert.IsFalse(1.5d.IsBetween(1, 0));
            Assert.IsFalse(1.5d.IsBetween(0, 1));
            Assert.IsFalse(1.5d.IsBetween(2, 3));
            Assert.IsFalse((-1.5d).IsBetween(1, 3));
        }
    }
}
