using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GestureRecognition.Implementation.Pipeline.Interpreted;
using GestureRecognition.Implementation.TrameGestureController.Tasks;
using GestureRecognition.Interface;
using GestureRecognition.Interface.Commands;
using Trame;

namespace GestureRecognition.Implementation.TrameGestureController
{
    /// <summary>
    /// The controller handles the data stream and holds the pipelines for interpreted and physical gestures.
    /// </summary>
    public class TrameGestureController : IController
    {
        /// <summary>
        /// The action that should be fired on a new command.
        /// </summary>
        public event Action<AUserCommand> NewPhysicsCommand;

        public event Action<IEnumerable<Result>> NewMotions;
        private readonly BlockingCollection<ISkeleton> _skeletonBuffer;
        private readonly BlockingCollection<ISkeleton> _skeletonBuffer2;
        private Thread _thread;

        /// <summary>
        /// Setup a controller with the recognizer and a data container instance.
        /// 
        /// Creates the pipelines for physical and interpreted gestures.
        /// </summary>
        /// <param name="recognizer">the recognizer</param>
        public TrameGestureController(IRecognizer recognizer)
        {
            // tasks
            var smoothingTask = new SmoothingTask();
            var recognitionTask = new RecognitionTask(recognizer);
            var physicsCalculationTask = new PhysicCalculationTask();
            // buffers
            _skeletonBuffer = new BlockingCollection<ISkeleton>();
            _skeletonBuffer2 = new BlockingCollection<ISkeleton>();
            var secondBuffer = new BlockingCollection<ISkeleton>(1000);

            var f = new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.None);
            // interpreted
            var smoothing = f.StartNew(() => smoothingTask.Do(_skeletonBuffer, secondBuffer));
            var recognition = f.StartNew(() => recognitionTask.Do(secondBuffer, FireNewMotions));
            // physics
            var physics = f.StartNew(() => physicsCalculationTask.Do(_skeletonBuffer2, FireNewCommand));
            _thread = new Thread(() => { System.Threading.Tasks.Task.WaitAll(smoothing, physics, recognition); });
            _thread.Start();
        }

        /// <summary>
        /// Add new data to the data stream
        /// </summary>
        /// <param name="skeleton">a new skeleton</param>
        public void PushNewSkeleton(ISkeleton skeleton)
        {
            _skeletonBuffer.Add(skeleton.Clone());
            _skeletonBuffer2.Add(skeleton.Clone());
        }

        public void Stop()
        {
            // pass
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataContainer"></param>
        public void FireNewCommand(AUserCommand command)
        {
            NewPhysicsCommand?.Invoke(command);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataContainer"></param>
        public void FireNewMotions(IEnumerable<Result> results)
        {
            NewMotions?.Invoke(results);
        }
    }
}
