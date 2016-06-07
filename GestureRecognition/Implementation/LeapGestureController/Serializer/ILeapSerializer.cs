using System.Security.Cryptography.X509Certificates;
using Leap;

namespace LeapRecorder
{
    public interface ILeapSerializer
    {
        byte[] Serialize(Frame f);

        Frame Deserialize(byte[] b);
    }
}