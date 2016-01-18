using System;
using System.Collections.Generic;
using System.Linq;
using GestureRecognition.Utility;
using Trame;
using TrameSkeleton.Math;

namespace GestureRecognition.Implementation.Pipeline.Interpreted
{
    /// <summary>
    /// The point extractor extracts a set of points from the given stream of skeletons based on a configurable list.
    /// The movement data will be smoothened by the mean value in a window.
    /// </summary>
    public class PointExtractor : IPipeline
    {
        public event Action<DataContainer> Ready;
        private const int WindowSize = 3;

        private readonly IList<JointType> _usedJoints = new[] {JointType.HAND_LEFT, JointType.HAND_RIGHT, JointType.HIP_LEFT, JointType.HIP_RIGHT, JointType.SHOULDER_LEFT, JointType.SHOULDER_RIGHT};

        public void OnNewData(DataContainer dc)
        {
            foreach (var jointType in _usedJoints)
            {
                var vector = dc.Stream.Select(s => s.GetJoint(jointType).Point);
                // use window function to smooth the motion
                dc.Input[jointType] = new InputVector(vector.ChunkBy(WindowSize).Select(Mean));
            }

            FireReady(dc);
        }

        private Vector3 Mean(IList<Vector3> vectorList)
        {
            var first = vectorList.FirstOrDefault();
            var last = vectorList.LastOrDefault();
            return last + (last - first)/vectorList.Count;
        }

        private void FireReady(DataContainer dc)
        {
            if (Ready != null)
            {
                Ready(dc);
            }
        }
    }
}
