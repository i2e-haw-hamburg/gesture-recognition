using System;

namespace GestureRecognition.Implementation.Serializer
{
    [Serializable]
    public class SerializableArm
    {
        public SerializableVector elbow;
        public SerializableVector wrist;
        public SerializableVector center;
        public SerializableVector direction;
        public float length;
        public float width;
        public SerializableQuaternion rotation;
    }
}