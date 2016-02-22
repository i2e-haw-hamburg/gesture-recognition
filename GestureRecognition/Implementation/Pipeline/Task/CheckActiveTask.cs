using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Trame;

namespace GestureRecognition.Implementation.Pipeline.Task
{
    /// <summary>
    /// Checks if a stream contains user inactivity and clears the stream if user was inactive.
    /// </summary>
    public class CheckActiveTask
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

        public bool NoActivity(List<ISkeleton> stream)
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