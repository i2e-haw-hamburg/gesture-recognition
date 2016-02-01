using System.Collections.Generic;
using GestureRecognition.Utility;
using NUnit;
using NUnit.Framework;

namespace GestureRecognitionTest.Utility
{
    [TestFixture]
    public class ListExtensionTest
    {
        [Test]
        public void TestChunkBy()
        {
            var list = new List<int> {0, 1, 2, 3, 4, 5, 6, 7, 8};
            var chunked = list.ChunkBy(3);
            Assert.AreEqual(3, chunked.Count);
            Assert.AreEqual(new List<int> { 0, 1, 2 }, chunked[0]);
            Assert.AreEqual(new List<int> { 3, 4, 5 }, chunked[1]);
            Assert.AreEqual(new List<int> { 6, 7, 8 }, chunked[2]);
        }
    }
}
