using System;
using System.Collections.Generic;
using Trame;

namespace GestureRecognition.Implementation.Pipeline.Interpreted
{
    /// <summary>
    /// Checks if a stream contains user inactivity and clears the stream if user was inactive.
    /// </summary>
    public class ActiveChecker : IPipeline
    {
        public event Action<DataContainer> Ready;

        public void OnNewData(DataContainer dc)
        {
            // check for the current activity
            if (NoActivity(dc.Stream))
            {
                dc.Clear();
            }
            FireReady(dc);
        }

        private bool NoActivity(List<ISkeleton> stream)
        {
            throw new NotImplementedException();
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