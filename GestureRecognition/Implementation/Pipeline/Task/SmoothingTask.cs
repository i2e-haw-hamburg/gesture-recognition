using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Trame;
using TrameSkeleton.Math;
using GestureRecognition.Utility;
using Trame.Implementation.Skeleton;

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
		public void Do(BlockingCollection<ISkeleton> input, BlockingCollection<ISkeleton> output)
        {
            var skeletons = new List<ISkeleton>();
            try
			{
			    foreach (var skeleton in input.GetConsumingEnumerable())
			    {
                    skeletons.Add(skeleton);
			        if (skeletons.Count < 3)
			        {
			            continue;
			        }
                    var first = skeletons.First();
                    var tail = skeletons.Skip(1);
                    foreach (var joint in first.Joints)
                    {
                        var tailJoints = tail.Select(s => s.GetJoint(joint.JointType));
                        joint.Point = Mean(new List<Vector3> { joint.Point }.Concat(tailJoints.Select(j => j.Point)).ToList());
                        joint.Orientation = Mean(new List<Vector4> { joint.Orientation }.Concat(tailJoints.Select(j => j.Orientation)).ToList());
                        first.UpdateSkeleton(joint.JointType, joint);
                    }
                    output.Add(first);
                    skeletons.Clear();
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
		public static Vector3 Mean(IList<Vector3> vectorList)
		{
			var first = vectorList.FirstOrDefault();
			var last = vectorList.LastOrDefault();
			return first + (last - first)/vectorList.Count;
		}

        /// <summary>
        /// Mean the specified vectorList.
        /// </summary>
        /// <param name="vectorList">Vector list.</param>
        public static Vector4 Mean(IList<Vector4> vectorList)
        {
            var first = vectorList.FirstOrDefault();
            var last = vectorList.LastOrDefault();
            return first + (last - first) / vectorList.Count;
        }
    }
}

