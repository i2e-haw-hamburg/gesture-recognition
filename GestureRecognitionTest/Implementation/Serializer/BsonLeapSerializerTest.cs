using System.Collections.Generic;
using GestureRecognition.Implementation.Serializer;
using Leap;
using NUnit.Framework;

namespace GestureRecognitionTest.Implementation.Serializer
{
    public class BsonLeapSerializerTest
    {
        public void TestSerialize()
        {
            var frame = new Frame(1, 1, 30, new InteractionBox(Vector.Down, Vector.Right), new List<Hand>());
            var serializer = new BsonLeapSerializer();
            var serializedData = serializer.Serialize(frame);
            Assert.AreEqual(1, frame.Id);
            Assert.IsNotEmpty(serializedData);
        }
        
        public void TestDeserialize()
        {
            var frame = new Frame(1, 1, 30, new InteractionBox(Vector.Down, Vector.Right), new List<Hand>());
            var serializer = new BsonLeapSerializer();
            var serializedData = serializer.Serialize(frame);
            var deserializedData = serializer.Deserialize(serializedData);
            Assert.AreEqual(1, deserializedData.Id);
            Assert.AreEqual(1, deserializedData.Timestamp);
            Assert.AreEqual(30, deserializedData.CurrentFramesPerSecond);
            Assert.IsEmpty(deserializedData.Hands);
        }

    }
}
