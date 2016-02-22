using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using GestureRecognition.Implementation.Pipeline.Interpreted;
using GestureRecognition.Utility;
using Trame;
using TrameSkeleton.Math;

namespace GestureRecognition.Implementation.Pipeline.Task
{
    /// <summary>
    /// The point extractor extracts a set of points from the given stream of skeletons based on a configurable list.
    /// The movement data will be smoothened by the mean value in a window.
    /// </summary>
    public class PointExtractionTask
    {
		/// <summary>
		/// The used joints.
		/// </summary>
        private readonly IList<JointType> _usedJoints = new[] {JointType.HAND_LEFT, JointType.HAND_RIGHT, JointType.HIP_LEFT, JointType.HIP_RIGHT, JointType.SHOULDER_LEFT, JointType.SHOULDER_RIGHT, JointType.CENTER};

		/// <summary>
		/// Do the specified input and output.
		/// </summary>
		/// <param name="input">Input.</param>
		/// <param name="output">Output.</param>
        public void Do(BlockingCollection<ISkeleton> input, BlockingCollection<IDictionary<JointType, Vector3>> output)
        {
            try
            {
                var dict = new Dictionary<JointType, Vector3>();
                var skeletons = input.GetConsumingEnumerable();
                foreach (var jointType in _usedJoints)
                {
                    var smoothed = skeletons
						.Select(s => s.GetJoint(jointType).Point);
                }
                output.Add(dict);
            }
            finally
            {
                output.CompleteAdding();
            }
        }
    }
}
