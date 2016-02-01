using System;
using TrameSkeleton.Math;

namespace GestureRecognition.Implementation.Pipeline.Interpreted.Template
{
    public class BoundaryBox
    {
        /// <summary>
        /// Creates a bounding box from a set of parameters.
        /// </summary>
        /// <param name="position">the position of a corner of the bounding box</param>
        /// <param name="height">the height of the bounding box</param>
        /// <param name="width">the width of the bounding box</param>
        /// <param name="depth">the depth of the bounding box</param>
        /// <returns>the new created bounding box</returns>
        public static BoundaryBox Create(Vector3 position, int height, int width, int depth)
        {
            var box = new BoundaryBox
            {
                Position = position,
                Size = new Vector3(width, height, depth)
            };
            return box;
        }
        
        public Vector3 Size { get; set; }
        
        public Vector3 Position { get; set; }

        /// <summary>
        /// Checks if a point is in a bounding box.
        /// </summary>
        /// <param name="point">the point</param>
        /// <returns>TRUE if point is in bounding box</returns>
        public Boolean IsIn(Vector3 point)
        {
            // get rotation
            
            // rotate point
            // move point into internal space
            throw new NotImplementedException();
        }
    }
}