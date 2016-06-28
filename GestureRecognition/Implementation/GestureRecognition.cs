#region usages

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GestureRecognition.Implementation.Serializer;
using Leap;
using Trame;

#endregion

namespace GestureRecognition.Implementation
{
    #region usages

    using Interface;
    using Interface.Commands;

    #endregion
    /// <summary>
    /// Implementation of the gesture recognition interface.
    /// </summary>
    public class GestureRecognition : IGestureRecognition
    {
        /// <summary>
        /// The current controller.
        /// </summary>
        private readonly IController _controller;

        /// <summary>
        /// The current decider for interpreted gestures
        /// </summary>
        private readonly IDecider _decider;

        /// <summary>
        /// Creates the gesture recognition. The given controller will be stored for later use and
        /// an handler for the NewPhysicsCommand event in the controller is registered.
        /// </summary>
        /// <param name="controller">the controller for the gesture recognition</param>
        public GestureRecognition(IController controller)
        {
            _controller = controller;
            _decider = new Decider();
            _decider.NewInterpretedCommand += FireNewCommand;
            _controller.NewPhysicsCommand += FireNewCommand;
            _controller.NewMotions += _decider.Decide;
            _controller.NewFrame += WriteFrames;
        }

        private void FireNewCommand(AUserCommand cmd)
        {
            CallListenersForCommand(cmd);
        }

        #region Fields

        private Dictionary<Type, IList<IUserCommandListener>> listeners =
            new Dictionary<Type, IList<IUserCommandListener>>();

        private LeapFrameSerializer _serializer;
        private FileStream _file;

        #endregion

        #region Interfaces

        private interface IUserCommandListener
        {
            #region Public Methods and Operators

            void CallListener(AUserCommand command);

            bool IsInternalDelegateEqual(Delegate other);

            #endregion
        }

        #endregion

        #region Public Methods and Operators

        public void Stop()
        {
            _decider.NewInterpretedCommand -= FireNewCommand;
            _controller.NewPhysicsCommand -= FireNewCommand;
            _controller.NewMotions -= _decider.Decide;
            _controller.NewFrame -= WriteFrames;
            _controller.Stop();
            StopRecording();
        }
        

        public void SubscribeToCommand<T>(Action<T> commandListener) where T : AUserCommand
        {
            var typeOfCommand = typeof(T);
            var listener = new UserCommandListener<T>(commandListener);

            if (this.listeners.ContainsKey(typeOfCommand))
            {
                this.listeners[typeOfCommand].Add(listener);
            }
            else
            {
                this.listeners[typeOfCommand] = new List<IUserCommandListener>() { listener };
            }
        }

        public void UnsubscribeFromCommand<T>(Action<T> commandListener) where T : AUserCommand
        {
            var typeOfCommand = typeof(T);

            if (!this.listeners.ContainsKey(typeOfCommand))
            {
                throw new ArgumentException(
                    "Failed to unsubscribe listener: There are no listeners subscribed for the type of Command "
                    + typeOfCommand);
            }

            var wantedListener =
                this.listeners[typeOfCommand].SingleOrDefault(x => x.IsInternalDelegateEqual(commandListener));

            if (wantedListener == null)
            {
                throw new ArgumentException(
                    "Failed to unsubscribe listener: The listener " + commandListener
                    + " is not subscribed to the event " + typeOfCommand);
            }

            this.listeners[typeOfCommand].Remove(wantedListener);
        }

        public void OnNewSkeleton(ISkeleton skeleton)
        {
            _controller.PushNewSkeleton(skeleton);
        }

        public void StartRecording()
        {
            var fileName = DateTime.Now.ToFileTime() + ".frames";
            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), fileName);
            StopRecording();
            _serializer = new LeapFrameSerializer();
            _file = new FileStream(filePath, FileMode.OpenOrCreate);
        }

        private void WriteFrames(Frame f)
        {
            var frame = _serializer.Serialize(f);
            try
            {
                var length = frame.Length;
                _file.Write(BitConverter.GetBytes(length), 0, 4);
                _file.Write(frame, 0, length);
            }
            catch (Exception)
            { }
        }

        public void StopRecording()
        {
            _file?.Close();
        }

        #endregion

        #region Methods

        private void CallListenersForCommand<T>(T command) where T : AUserCommand
        {
            var typeOfCommand = command.GetType();

            if (!this.listeners.ContainsKey(typeOfCommand))
            {
                // There are no subscribed listeners for this Command.
                return;
            }

            foreach (var userCommandListener in this.listeners[typeOfCommand])
            {
                userCommandListener.CallListener(command);
            }
        }

        #endregion

        private class UserCommandListener<T> : IUserCommandListener
            where T : AUserCommand
        {
            #region Fields

            private Action<T> listenerDelegate;

            #endregion

            #region Constructors and Destructors

            public UserCommandListener(Action<T> listenerDelegate)
            {
                this.listenerDelegate = listenerDelegate;
            }

            #endregion

            #region Public Methods and Operators

            public void CallListener(AUserCommand command)
            {
                this.listenerDelegate(command as T);
            }

            public bool IsInternalDelegateEqual(Delegate other)
            {
                return other.Equals(this.listenerDelegate);
            }

            #endregion
        }
    }
}