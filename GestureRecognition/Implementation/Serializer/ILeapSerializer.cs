using Leap;

namespace GestureRecognition.Implementation.Serializer
{
    public interface ILeapSerializer
    {
        byte[] Serialize(Frame f);

        Frame Deserialize(byte[] b);
    }
}