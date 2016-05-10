using System;
using System.Collections.Generic;
using System.Linq;
using GestureRecognition.Implementation.Pipeline.Interpreted;
using GestureRecognition.Interface.Commands;

namespace GestureRecognition.Implementation
{
	public class Decider : IDecider
	{
		private const double Threshold = 0.6;

        public event Action<AInterpretedCommand> NewInterpretedCommand;

	    public void Decide(IEnumerable<Result> motions)
	    {
            var enumerable = motions as IList<Result> ?? motions.ToList();
            if (!enumerable.Any())
            {
                return;
            }
            var result = enumerable.OrderByDescending(r => r.prob).First();
            if (result.prob > Threshold)
            {
                NewInterpretedCommand?.Invoke(result.template.Command);
            }
        }

	}
}

