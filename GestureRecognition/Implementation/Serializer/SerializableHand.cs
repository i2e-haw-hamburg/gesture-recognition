using System;
using System.Collections.Generic;

namespace GestureRecognition.Implementation.Serializer
{
    [Serializable]
    public class SerializableHand
    {
        public long frameID;
        public int id;
        public float confidence;
        public float grabStrength;
        public float grabAngle;
        public float pinchStrength;
        public float pinchDistance;
        public float palmWidth;
        public bool isLeft;
        public float timeVisible;
        public SerializableArm arm;
        public List<SerializableFinger> fingers;
        public SerializableVector palmPosition;
        public SerializableVector stabilizedPalmPosition;
        public SerializableVector palmVelocity;
        public SerializableVector palmNormal;
        public SerializableVector direction;
        public SerializableVector wristPosition;
    }
}