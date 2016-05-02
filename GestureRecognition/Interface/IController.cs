using System;
using GestureRecognition.Interface.Commands;
using Trame;

namespace GestureRecognition.Interface
{
    public interface IController
    {
        event Action<AUserCommand> NewCommand;
        void PushNewSkeleton(ISkeleton skeleton);
    }
}