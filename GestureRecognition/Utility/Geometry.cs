using System;
using System.Runtime.Remoting.Messaging;
using Leap;
using TrameSkeleton.Math;

namespace GestureRecognition.Utility
{
    /// <summary>
    /// Helper methods to solve geometric problems. Allows you to draw three lines in 2d which are all perpendicular to each other.
    /// </summary>
    public static class Geometry
    {
        /// <summary>
        /// Determines if a given normal points to a target from a given position.
        /// </summary>
        /// <param name="position">The start position of the normal.</param>
        /// <param name="normal">The normal itself.</param>
        /// <param name="target">The target to check.</param>
        /// <param name="threshold">The turning angle threshold in degree. 45° means an overall cone of 90°.</param>
        /// <returns>Whether the given normal points to the target from a position or not</returns>
        public static bool DirectTo(Vector3 position, Vector3 normal, Vector3 target, double threshold = 15.0)
        {
            var direction = target - position;
            // angle in radians
            double turningAngle = Vector3.Angle(normal, direction);
            // angle in degrees
            turningAngle = turningAngle*180/Math.PI;
            return turningAngle <= threshold;
        }

        /// <summary>
        /// Returns the rotation around x, y, and z axis from two given quaternions. If both quaternions are equal it will result in zero vector.
        /// The angle with the lesser absolute value should be used as rotation.
        /// </summary>
        /// <param name="start">the start quaternion</param>
        /// <param name="target">the target quaternion</param>
        /// <returns></returns>
        public static Vector3 RelativeRotation(Quaternion start, Quaternion target)
        {
            if (start.Equals(target))
            {
                return Vector3.Zero;
            }
            var normalizedStart = Quaternion.Normalize(start);
            var normalizedTarget = Quaternion.Normalize(target);
            var rotation =  normalizedStart * Quaternion.Inverse(normalizedTarget);
            return rotation.GetYawPitchRollVector();
        }

        public static bool DirectTo(Vector position, Vector normal, Vector target, double threshold = 15.0)
        {
            return DirectTo(
                new Vector3(position.x, position.y, position.z),
                new Vector3(normal.y, normal.y, normal.z),
                new Vector3(target.x, target.y, target.z),
                threshold);
        }

        private static double Reset(double x)
        {
            return Math.Abs(x - 2 * Math.PI) < 0.000001 ? 0.0 : x;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="q">the rotation quaternion</param>
        /// <returns>The rotation angle around the x axis</returns>
        public static double Pitch(Quaternion q)
        {
            return Math.Asin(2.0 * (q.X * q.Y + q.W * q.Z));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="q">the rotation quaternion</param>
        /// <returns>The rotation angle around the y axis</returns>
        public static double Yaw(Quaternion q)
        {
            return Math.Atan2(2.0 * (q.Y * q.Z + q.W* q.X), q.W* q.W- q.X * q.X - q.Y * q.Y + q.Z * q.Z);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="q">the rotation quaternion</param>
        /// <returns>The rotation angle around the z axis</returns>
        public static double Roll(Quaternion q)
        {
            return Math.Atan2(2.0 * (q.X * q.Y + q.W* q.Z), q.W* q.W+ q.X * q.X - q.Y * q.Y - q.Z * q.Z);
        }
    }

    internal static class QuaternionExtensions
    {
        internal static Vector3 GetYawPitchRollVector(this Quaternion q)
        {
            return new Vector3(q.GetYaw(), q.GetPitch(), q.GetRoll());
        }

        private static double GetYaw(this Quaternion q)
        {
            var x2 = q.X * q.X;
            var y2 = q.Y * q.Y;
            return Math.Atan2(2f * q.Y * q.W - 2f * q.Z * q.X, 1f - 2f * y2 - 2f * x2);
        }

        private static double GetPitch(this Quaternion q)
        {
            return -Math.Asin(2f * q.Z * q.Y + 2f * q.X * q.W);
        }

        private static double GetRoll(this Quaternion q)
        {
            var x2 = q.X * q.X;
            var z2 = q.Z * q.Z;
            return -Math.Atan2(2f * q.Z * q.W - 2f * q.Y * q.X, 1f - 2f * z2 - 2f * x2);
        }
    }
}