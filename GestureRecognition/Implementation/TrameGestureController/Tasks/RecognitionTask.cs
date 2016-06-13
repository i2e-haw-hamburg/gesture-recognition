using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using GestureRecognition.Implementation.Pipeline.Interpreted;
using Trame;

namespace GestureRecognition.Implementation.TrameGestureController.Tasks
{
    /// <summary>
    /// This class provides the recognizer and matches recognized templates with the best suited Command.
    /// </summary>
    public class RecognitionTask
    {
		/// <summary>
		/// The recognizer.
		/// </summary>
        private IRecognizer _recognizer;

		/// <summary>
		/// Gets or sets the recognizer.
		/// </summary>
		/// <value>The recognizer.</value>
        public IRecognizer Recognizer
        {
            get
            {
                return _recognizer;
            }

            set
            {
                _recognizer = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public RecognitionTask(IRecognizer recognizer)
        {
            Recognizer = recognizer;
            recognizer.Initialize(TemplateFactory.CreateTemplates());
        }
        
		/// <summary>
		/// Do the specified input and output.
		/// </summary>
		/// <param name="input">Input.</param>
		/// <param name="output">Output.</param>
        public void Do(BlockingCollection<ISkeleton> input, Action<IEnumerable<Result>> fireNewCommand)
        {
            var data = new Dictionary<JointType, InputVector>();
            foreach (var skeleton in input.GetConsumingEnumerable())
            {
                foreach (var joint in skeleton.Joints)
                {
                    if (!data.ContainsKey(joint.JointType))
                    {
                        data.Add(joint.JointType, new InputVector());
                    }
                    data[joint.JointType].Stream.Add(joint.Point);
                }
                if (data.First().Value.Stream.Count < 5)
                {
                    continue;
                }
                var results = Recognizer.Recognize(data);
                try
                {
                    fireNewCommand(results);
                }
                catch (Exception)
                {
                    if (data.First().Value.Stream.Count > 40)
                    {
                        data.Clear();
                    }
                    continue;
                }
                data.Clear();
            }
        }
		        
    }
}
