using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestureRecognition.Utility
{
    public static class DoubleUtility
    {
        public static Boolean IsBetween(this double value, double firstBoundary, double secondBoundary)
        {
            if (firstBoundary > secondBoundary)
            {
                return value >= secondBoundary && value <= firstBoundary;
            }
            else
            {
                return value >= firstBoundary && value <= secondBoundary;
            }
        }
    }
}
