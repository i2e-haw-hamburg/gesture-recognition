using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trame;
using TrameSkeleton.Math;

namespace GestureRecognition.Implementation.Pipeline.Interpreted
{
    public class InputVector
    {
        private IList<Vector3> _stream;

        public InputVector(IList<Vector3> stream)
        {
            Stream = stream;
        }
        public InputVector()
        {
            Stream = new List<Vector3>();
        }

        public IList<Vector3> Stream
        {
            get { return _stream; }
            set { _stream = value; }
        }

        public Vector3 First
        {
            get { return Stream.FirstOrDefault(); }
        }

        public Vector3 Last
        {
            get { return Stream.LastOrDefault(); }
        }

        public Vector3 Get(int i)
        {
            if (i >= 0 && i < Stream.Count())
            {
                return Stream.ToArray()[i];
            }
            throw new IndexOutOfRangeException();
        }
    }
}
