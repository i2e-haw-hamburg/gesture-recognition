using System;
using GestureRecognition.Implementation.Pipeline.Interpreted;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;

namespace GestureRecognition
{
	public class DecisionTask
	{
		private const double Threshold = 0.6;

		public void Do(BlockingCollection<IEnumerable<Result>> input, BlockingCollection<Result> output)
		{
			try
			{
				foreach (var results in input.GetConsumingEnumerable())
				{
					output.Add(MakeDecision(results));
				}
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

