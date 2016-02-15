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
    public class MatchingTask
    {
        private IRecognizer _recognizer;
        private const double Threshold = 0.6;

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
        public MatchingTask(IRecognizer recognizer)
        {
            Recognizer = recognizer;
            recognizer.Initialize(TemplateFactory.CreateTemplates());
        }
        

        public void Do(BlockingCollection<IDictionary<JointType, Vector3>> input, BlockingCollection<Result> output)
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
                var results = Recognizer.Recognize(dict);
                var result = MakeDecision(results);
                output.Add(result);
            }
            finally
            {
                output.CompleteAdding();
            }
        }

        /// <summary>
        /// Examined if a result is available and if it should used as a new command.
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        public Result MakeDecision(IEnumerable<Result> results)
        {
            var enumerable = results as IList<Result> ?? results.ToList();
            if (!enumerable.Any())
            {
                throw new Exception();
            }
            var result = enumerable.OrderByDescending(r => r.prob).First();
            if (result.prob > Threshold)
            {
                return result;
            }
            throw new Exception();
        }
    }
}
