using System.Threading;
using GestureRecognition.Implementation;
using GestureRecognition.Interface;
using GestureRecognition.Interface.Commands;
using NUnit.Framework;

namespace GestureRecognitionTest
{

    public class LeapGestureControllerTest
    {
        
        public void testInit()
        {
            var commandReceived = false;
            var commandWaiter = new ManualResetEventSlim();
            var gestureRecognition = GestureRecognitionFactory.Create(new LeapGestureController());
            gestureRecognition.SubscribeToCommand<PhysicCommand>(x =>
            {
                commandReceived = true;
                commandWaiter.Set();
            });

            commandWaiter.Wait(5000);

            Assert.IsTrue(commandReceived);
        }
    }
}