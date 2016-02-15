using System;
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
        private IPipeline _physicsPipeline;
        private IPipeline _interpretedPipeline;
        private DataContainer _dataStream;

        /// <summary>
        /// Setup a controller with the recognizer and a data container instance.
        /// 
        /// Creates the pipelines for physical and interpreted gestures.
        /// </summary>
        /// <param name="recognizer">the recognizer</param>
        /// <param name="dataStream">the container for all stream data</param>
        public Controller(IRecognizer recognizer, DataContainer dataStream)
        {
            _physicsPipeline = Initializer.CreatePipeline(this, new PhysicCalculation());
            var f = new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.None);
            _interpretedPipeline = Initializer.CreatePipeline(this, new PointExtractionTask(), new MatchingTask(recognizer));
            _dataStream = dataStream;
        }

        /// <summary>
        /// Add new data to the data stream
        /// </summary>
        /// <param name="skeleton">a new skeleton</param>
        public void PushNewSkeleton(ISkeleton skeleton)
        {
            _dataStream.Add(skeleton.Clone());
            if (NewData != null)
            {
                NewData(_dataStream);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataContainer"></param>
        public void FireNewCommand(DataContainer dataContainer)
        {
            if (NewCommand != null && dataContainer.HasCommand())
            {
                NewCommand(dataContainer.Command);
            }
        }
    }
}
