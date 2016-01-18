using System.Collections.Generic;
using System.Linq;
using GestureRecognition.Implementation.Pipeline.Interpreted;
using GestureRecognition.Interface.Commands;
using Trame;

namespace GestureRecognition
{
    /// <summary>
    /// 
    /// </summary>
    public class DataContainer
    {
        /// <summary>
        /// The raw stream of skeletons
        /// </summary>
        public readonly List<ISkeleton> Stream;
        private const int MaxCountOfSkeletons = 39;
        /// <summary>
        /// The formated input data
        /// </summary>
        private IDictionary<JointType, InputVector> _input; 

        /// <summary>
        /// 
        /// </summary>
        public ISkeleton Newest => Stream.Count > 0 ? Stream.Last() : null;

        private AUserCommand _command = null;

        /// <summary>
        /// 
        /// </summary>
        public DataContainer()
        {
            Input = new Dictionary<JointType, InputVector>();
            Stream = new List<ISkeleton> {  };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skeleton"></param>
        public DataContainer(ISkeleton skeleton)
        {
            Input = new Dictionary<JointType, InputVector>();
            Stream = new List<ISkeleton> { skeleton };
        }

        /// <summary>
        /// 
        /// </summary>
        public AUserCommand Command
        {
            get { return _command; }
            set { _command = value; }
        }

        public IDictionary<JointType, InputVector> Input
        {
            get { return _input; }
            private set { _input = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="skeleton"></param>
        /// <returns></returns>
        public bool Add(ISkeleton skeleton)
        {
            var newest = Newest;
            if (newest != null && skeleton.Timestamp <= newest.Timestamp)
            {
                return false;
            }
            if (Stream.Count >= MaxCountOfSkeletons)
            {
                Stream.RemoveAt(0);
            }
            Stream.Add(skeleton);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool HasCommand()
        {
            return _command != null;
        }

        public void Clear()
        {
            Stream.Clear();
            Input.Clear();
        }
    }
}
