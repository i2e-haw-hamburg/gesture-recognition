using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections;

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
        public static IList<IList<T>> ChunkBy<T>(this IEnumerable<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
				.Select(x => x.Select(v => v.Value).ToList<T>())
				.ToList<IList<T>>();
        }

		/// <summary>
		/// Compress the specified source.
		/// </summary>
		/// <param name="source">Source.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		/// <typeparam name="U">The 2nd type parameter.</typeparam>
		public static IDictionary<T,IList<U>> Compress<T,U>(this IEnumerable<IDictionary<T, U>> source) {
			var dict = new Dictionary<T, IList<U>>();
			foreach (var chunk in source)
			{
				
				foreach (var type in chunk.Keys)
				{
					if (!dict.ContainsKey(type))
					{
						dict.Add(type, new List<U>());
					}
					dict[type].Add(chunk[type]);
				}
			}
			return dict;
		}
    }
}