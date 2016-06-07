using System;
using System.Collections.Generic;

namespace GestureRecognition.Implementation.Serializer
{
    [Serializable]
    public class SerializableFrame
    {
        public long id;
        public long timestamp;
        public float fps;
        public List<SerializableHand> hands;
    }
}