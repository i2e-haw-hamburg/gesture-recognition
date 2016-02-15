using System;
using System.Collections.Concurrent;

namespace GestureRecognition
{
	public class SmoothingTask
	{

		/// <summary>
		/// Do the specified input and output.
		/// </summary>
		/// <param name="input">Input.</param>
		/// <param name="output">Output.</param>
		public void Do(BlockingCollection<IDictionary<JointType, Vector3>> input, BlockingCollection<IDictionary<JointType, Vector3>> output)
		{
			try
			{
				var dict = new Dictionary<JointType, Vector3>();
				var skeletons = input.GetConsumingEnumerable();
				foreach (var jointType in _usedJoints)
				{
					var smoothed = skeletons
						.Select(s => s.GetJoint(jointType).Point)
						.ChunkBy(WindowSize)
						.Select(Mean);
				}
				output.Add(dict);
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

