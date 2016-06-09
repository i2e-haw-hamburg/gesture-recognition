using System;
using Leap;

namespace GestureRecognition.Implementation.Serializer
{
    [Serializable]
    public class SerializableQuaternion
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public LeapQuaternion ToLeapQuaternion() => new LeapQuaternion(x, y, z, w);
    }
}