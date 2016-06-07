using System;
using Leap;

namespace GestureRecognition.Implementation.Serializer
{
    [Serializable]
    public class SerializableVector
    {
        public float x;
        public float y;
        public float z;

        public Vector ToVector() => new Vector(x, y, z);
    }
}