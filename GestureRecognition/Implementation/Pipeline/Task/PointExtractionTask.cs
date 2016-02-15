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
        private const int WindowSize = 3;

        private readonly IList<JointType> _usedJoints = new[] {JointType.HAND_LEFT, JointType.HAND_RIGHT, JointType.HIP_LEFT, JointType.HIP_RIGHT, JointType.SHOULDER_LEFT, JointType.SHOULDER_RIGHT, JointType.CENTER};

        public void OnNewData(DataContainer dc)
        {
            foreach (var jointType in _usedJoints)
            {
                var vector = dc.Stream.Select(s => s.GetJoint(jointType).Point);
                // use window function to smooth the motion
                dc.Input[jointType] = new InputVector(vector.ChunkBy(WindowSize).Select(Mean));
            }
        }

        public void Do(BlockingCollection<ISkeleton> input, BlockingCollection<IDictionary<JointType, Vector3>> output)
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

        public Vector3 Mean(IList<Vector3> vectorList)
        {
            var first = vectorList.FirstOrDefault();
            var last = vectorList.LastOrDefault();
            return last + (last - first)/vectorList.Count;
        }
    }
}
