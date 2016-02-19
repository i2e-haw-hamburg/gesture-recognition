using System;
using System.Linq;
using System.Collections.Concurrent;
using GestureRecognition.Implementation.Pipeline.Physical;
using GestureRecognition.Interface.Commands;
using Trame;

namespace GestureRecognition
{
	public class PhysicCalculationTask
	{
		private const double Threshold = 0.6;
        private PhysicCalculation _physicCalculation = new PhysicCalculation();

		public void Do(BlockingCollection<ISkeleton> input, Action<AUserCommand> fireNewCommand)
		{
			try
			{
			    var skeletons = input.GetConsumingEnumerable();
                var data = skeletons
                    .Reverse()
                    .Take(3)
                    .ToList();
                fireNewCommand(_physicCalculation.CreatePhysicCommand(data));
			}
			catch
			{
			}
		}
        
	}
}

