﻿using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using GestureRecognition.Implementation.Pipeline.Physical;
using GestureRecognition.Interface.Commands;
using GestureRecognition.Utility;
using Trame;

namespace GestureRecognition
{
	public class PhysicCalculationTask
	{
		private const double Threshold = 0.6;
        private PhysicCalculation _physicCalculation = new PhysicCalculation();

		public void Do(BlockingCollection<ISkeleton> input, Action<AUserCommand> fireNewCommand)
		{
            var skeletons = new List<ISkeleton>();
			try
			{
			    foreach (var skeleton in input.GetConsumingEnumerable())
			    {
                    skeletons.Add(skeleton);
                    if (skeletons.Count < 3)
			        {
                        continue;
			        }
			        var data = Enumerable.Reverse(skeletons).Take(3).ToList();
                    fireNewCommand(_physicCalculation.CreatePhysicCommand(data));
                    skeletons.RemoveAt(0);
                }
			}
			catch
			{
			}
		}
        
	}
}

