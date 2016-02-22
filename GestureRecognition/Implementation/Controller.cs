using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GestureRecognition.Implementation.Pipeline;
using GestureRecognition.Implementation.Pipeline.Interpreted;
using GestureRecognition.Implementation.Pipeline.Physical;
using GestureRecognition.Implementation.Pipeline.Task;
using GestureRecognition.Interface.Commands;
using Trame;

namespace GestureRecognition.Implementation
{
    /// <summary>
    /// The controller handles the data stream and holds the pipelines for interpreted and physical gestures.
    /// </summary>
    public class Controller
    {
        /// <summary>
        /// The action that should be fired on new data.
        /// </summary>
        public event Action<DataContainer> NewData;
        /// <summary>
        /// The action that should be fired on a new command.
        /// </summary>
        public event Action<AUserCommand> NewCommand;
        private readonly BlockingCollection<ISkeleton> _skeletonBuffer;
        private readonly BlockingCollection<ISkeleton> _skeletonBuffer2;
        private Thread _thread;

        /// <summary>
        /// Setup a controller with the recognizer and a data container instance.
        /// 
        /// Creates the pipelines for physical and interpreted gestures.
        /// </summary>
        /// <param name="recognizer">the recognizer</param>
        /// <param name="dataStream">the container for all stream data</param>
        public Controller(IRecognizer recognizer, DataContainer dataStream)
        {
            // tasks
            var smoothingTask = new SmoothingTask();
            var recognitionTask = new RecognitionTask(recognizer);
            var decisionTask = new DecisionTask();
            var physicsCalculationTask = new PhysicCalculationTask();
            // buffers
            _skeletonBuffer = new BlockingCollection<ISkeleton>();
            _skeletonBuffer2 = new BlockingCollection<ISkeleton>();
            var secondBuffer = new BlockingCollection<ISkeleton>(1000);

            var f = new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.None);
            // interpreted
            var smoothing = f.StartNew(() => smoothingTask.Do(_skeletonBuffer, secondBuffer));
            var recognition = f.StartNew(() => recognitionTask.Do(secondBuffer, FireNewCommand));
            //var decision = f.StartNew(() => decisionTask.Do(thirdBuffer, FireNewCommand));
            // physics
            var physics = f.StartNew(() => physicsCalculationTask.Do(_skeletonBuffer2, FireNewCommand));
            _thread = new Thread(() => { Task.WaitAll(smoothing, physics, recognition); });
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataContainer"></param>
        public void FireNewCommand(AUserCommand command)
        {
            NewCommand?.Invoke(command);
        }
    }
}
