using System;
using Leap;

namespace GestureRecognition.Implementation.Serializer
{
    [Serializable]
    public class SerializableBone
    {
        public SerializableVector prevJoint;
        public SerializableVector nextJoint;
        public SerializableVector center;
        public SerializableVector direction;
        public float width;
        public float length;
        public Bone.BoneType type;
        public SerializableQuaternion rotation;
    }
}