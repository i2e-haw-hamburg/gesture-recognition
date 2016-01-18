using System;
using System.Collections.Generic;
using GestureRecognition.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GestureRecognitionTest.Utility
{
    [TestClass]
    public class ListExtensionTest
    {
        [TestMethod]
        public void TestChunkBy()
        {
            var list = new List<int> {0, 1, 2, 3, 4, 5, 6, 7, 8};
            var chunked = list.ChunkBy(3);
            Assert.AreEqual(3, chunked.Count);
            CollectionAssert.AreEqual(new List<int> { 0, 1, 2 }, chunked[0]);
            CollectionAssert.AreEqual(new List<int> { 3, 4, 5 }, chunked[1]);
            CollectionAssert.AreEqual(new List<int> { 6, 7, 8 }, chunked[2]);
        }
    }
}
