using System.Collections.Generic;
using System.Linq;

namespace GestureRecognition.Utility
{
    /// <summary>
    /// Helper methods for lists.
    /// </summary>
    public static class ListExtension
    {
        /// <summary>
        /// Chunk a given list by the chunk size and return a list of lists.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="chunkSize"></param>
        /// <returns></returns>
        public static List<List<T>> ChunkBy<T>(this IEnumerable<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
    }
}