using System;
using Leap;

namespace GestureRecognition.Implementation.Serializer
{
    [Serializable]
    public class SerializableFinger
    {
        public long frameID;
        public int handId;
        public int fingerId;
        public float timeVisible;
        public SerializableVector tipPosition;
        public SerializableVector tipVelocity;
        public SerializableVector direction;
        public SerializableVector stabilizedTipPosition;
        public float width;
        public float length;
        public bool isExtended;
        public Finger.FingerType type;
        public SerializableBone metacarpal;
        public SerializableBone proximal;
        public SerializableBone intermediate;
        public SerializableBone distal;
    }
}