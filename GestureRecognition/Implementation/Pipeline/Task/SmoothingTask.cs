using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Trame;
using TrameSkeleton.Math;
using GestureRecognition.Utility;

namespace GestureRecognition
{
	/// <summary>
	/// Smoothing task.
	/// </summary>
	public class SmoothingTask
	{

		/// <summary>
		/// The size of the window.
		/// </summary>
		private const int WindowSize = 3;

		/// <summary>
		/// Do the specified input and output.
		/// </summary>
		/// <param name="input">Input.</param>
		/// <param name="output">Output.</param>
		public void Do(BlockingCollection<IDictionary<JointType, Vector3>> input, BlockingCollection<IDictionary<JointType, Vector3>> output)
		{
			try
			{
				var skeletons = input.GetConsumingEnumerable();
				var skeletonStreams = skeletons.ChunkBy(WindowSize).Select(ListExtension.Compress);
				foreach (var window in skeletonStreams) {
					var smoothed = new Dictionary<JointType,Vector3>();
					foreach(var jt in window.Keys) {
						smoothed.Add(jt, Mean(window[jt]));
					}
					output.Add(smoothed);
				}
			}
			finally
			{
				output.CompleteAdding();
			}
		}

		/// <summary>
		/// Mean the specified vectorList.
		/// </summary>
		/// <param name="vectorList">Vector list.</param>
		public Vector3 Mean(IList<Vector3> vectorList)
		{
			var first = vectorList.FirstOrDefault();
			var last = vectorList.LastOrDefault();
			return last + (last - first)/vectorList.Count;
		}
	}
}

