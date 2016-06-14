using System;
using Leap;

namespace GestureRecognition.Utility
{
    /// <summary>
    /// Helper methods to solve geometric problems. Allows you to draw three lines in 2d which are all perpendicular to each other.
    /// </summary>
    public static class Geometry
    {
        /// <summary>
        /// Determines if a given normal points to a target from a given position. The 
        /// </summary>
        /// <param name="position">The start position of the normal.</param>
        /// <param name="normal">The normal itself.</param>
        /// <param name="target">The target to check.</param>
        /// <param name="threshold">The turning angle threshold in degree. 45° means an overall cone of 90°.</param>
        /// <returns>Whether the given normal points to the target from a position or not</returns>
        public static bool DirectTo(Vector position, Vector normal, Vector target, double threshold = 15.0)
        {
            var direction = target - position;
            // angle in radians
            double turningAngle = normal.AngleTo(direction);
            // angle in degrees
            turningAngle = turningAngle*180/Math.PI;
            return turningAngle <= threshold;
        }
    }
}