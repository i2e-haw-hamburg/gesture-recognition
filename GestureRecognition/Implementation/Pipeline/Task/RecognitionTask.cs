using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using GestureRecognition.Implementation.Pipeline.Interpreted;
using GestureRecognition.Interface.Commands;
using Trame;
using TrameSkeleton.Math;

namespace GestureRecognition.Implementation.Pipeline.Task
{
    /// <summary>
    /// This class provides the recognizer and matches recognized templates with the best suited command.
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
        public void Do(BlockingCollection<IDictionary<JointType, Vector3>> input, BlockingCollection<IEnumerable<Result>> output)
        {
            try
            {
                var data = input.GetConsumingEnumerable();
                var dict = new Dictionary<JointType, InputVector>();
                foreach (var chunk in data)
                {
                    foreach (var jointType in chunk.Keys)
                    {
                        if (!dict.ContainsKey(jointType))
                        {
                            dict.Add(jointType, new InputVector());
                        }
                        dict[jointType].Stream.Add(chunk[jointType]);
                    }
                }
				output.Add(Recognizer.Recognize(dict));
            }
            finally
            {
                output.CompleteAdding();
            }
        }
		        
    }
}
