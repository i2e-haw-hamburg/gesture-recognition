using Leap;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace GestureRecognition.Implementation.Serializer
{
    public class VectorBsonSerializer : SerializerBase<Vector>
    {
        public override Vector Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            if (context.Reader.CurrentBsonType == BsonType.Double)
            {
                return new Vector((float) context.Reader.ReadDouble(), (float) context.Reader.ReadDouble(),
                    (float) context.Reader.ReadDouble());
            }
            return new Vector();
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Vector value)
        {
            if (context.Writer.State == BsonWriterState.Value)
            {
                context.Writer.WriteDouble(value.x);
                context.Writer.WriteDouble(value.y);
                context.Writer.WriteDouble(value.z);
            }
        }
    }
}