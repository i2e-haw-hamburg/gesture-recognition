using Leap;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace GestureRecognition.Implementation.Serializer
{
    public class LeapQuaternionBsonSerializer : SerializerBase<LeapQuaternion>
    {
        public override LeapQuaternion Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return new LeapQuaternion((float) context.Reader.ReadDouble(), (float)context.Reader.ReadDouble(), (float)context.Reader.ReadDouble(), (float)context.Reader.ReadDouble());
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, LeapQuaternion value)
        {
            context.Writer.WriteDouble(value.x);
            context.Writer.WriteDouble(value.y);
            context.Writer.WriteDouble(value.z);
            context.Writer.WriteDouble(value.w);
        }
    }
}