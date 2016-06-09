using Leap;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace GestureRecognition.Implementation.Serializer
{
    public class BsonLeapSerializer : ILeapSerializer
    {

        public BsonLeapSerializer()
        {
            try
            {
                BsonSerializer.RegisterSerializer(typeof(Vector), new VectorBsonSerializer());
                BsonSerializer.RegisterSerializer(typeof(LeapQuaternion), new VectorBsonSerializer());
            }
            catch (BsonSerializationException)
            {
                // C# doesn't kill the static context of the registry
            }
        }

        public byte[] Serialize(Frame f)
        {
            return f.ToBson();
        }

        public Frame Deserialize(byte[] b)
        {
            return BsonSerializer.Deserialize<Frame>(b);
        }
    }
}