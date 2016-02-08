using System;
using System.Linq;
using GestureRecognition.Interface.Commands;

namespace GestureRecognition.Implementation.Pipeline.Interpreted
{
    /// <summary>
    /// This class provides the recognizer and matches recognized templates with the best suited command.
    /// </summary>
    public class Matcher : IPipeline
    {
        /// <summary>
        /// 
        /// </summary>
        public event Action<DataContainer> Ready;
        private IRecognizer _recognizer;
        private double _threshold = 0.6;

        /// <summary>
        /// 
        /// </summary>
        public Matcher(IRecognizer recognizer)
        {
            _recognizer = recognizer;
            recognizer.Initialize(TemplateFactory.CreateTemplates());
        }

        /// <summary>
        /// Examined if a result is available and if it should used as a new command.
        /// </summary>
        /// <param name="dc">the data stream</param>
        public void OnNewData(DataContainer dc)
        {
            var results = _recognizer.Recognize(dc.Input);
            if (!results.Any())
            {
               return;
            }
            var result = results.OrderByDescending(r => r.prob).First();
            if (result.prob > _threshold)
            {
                // remove skeletons
                dc.Clear();
                dc.Command = result.template.Command;
                dc.Command.Input = dc.Input;
                FireReady(dc);
            }
        }

        private void FireReady(DataContainer dc)
        {
            if (Ready != null)
            {
                Ready(dc);
            }
        }
    }
}
