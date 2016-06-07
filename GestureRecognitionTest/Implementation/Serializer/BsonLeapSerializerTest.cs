using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Leap;
using LeapRecorder;
using NUnit;
using NUnit.Framework;

namespace LeapRecorderTest
{
    [TestFixture]
    public class BsonLeapSerializerTest
    {
        [Test]
        public void TestDeserializeFromFile()
        {
            var fileName = "right_hand.bson";
            var b = new byte[4];
            var file = new FileStream(fileName, FileMode.Open);
            file.Read(b, 0, 4);
            var length = BitConverter.ToInt32(b, 0);
            var objBuffer = new byte[length];
            var serializer = new BsonLeapSerializer();
            file.Read(objBuffer, 0, length);
            var frame = serializer.Deserialize(objBuffer);
            Assert.AreEqual(1, frame.Id);
        }

        [Test]
        public void TestSerialize()
        {
            var frame = new Frame(1, 1, 30, new InteractionBox(Vector.Down, Vector.Right), new List<Hand>());
            var serializer = new BsonLeapSerializer();
            var serializedData = serializer.Serialize(frame);
            Assert.AreEqual(1, frame.Id);
            Assert.IsNotEmpty(serializedData);
        }

        [Test]
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
