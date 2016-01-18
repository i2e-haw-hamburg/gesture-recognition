using System.Collections.Generic;
using GestureRecognition.Utility;
using Xunit;

namespace GestureRecognitionTest.Utility
{
    public class ListExtensionTest
    {
        [Fact]
        public void TestChunkBy()
        {
            var list = new List<int> {0, 1, 2, 3, 4, 5, 6, 7, 8};
            var chunked = list.ChunkBy(3);
            Assert.Equal(3, chunked.Count);
            Assert.Equal(new List<int> { 0, 1, 2 }, chunked[0]);
            Assert.Equal(new List<int> { 3, 4, 5 }, chunked[1]);
            Assert.Equal(new List<int> { 6, 7, 8 }, chunked[2]);
        }
    }
}
