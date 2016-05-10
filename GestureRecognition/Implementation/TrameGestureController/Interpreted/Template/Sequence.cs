using System.Collections.Generic;
using Trame;
using TrameSkeleton.Math;

namespace GestureRecognition.Implementation.Pipeline.Interpreted.Template
{
    public class Sequence<T>
    {
        private const int MaxNumberOfPoints = 3;
        private IList<T> _points;
        private JointType _identifier;

        public Sequence(JointType identifier)
            : this(identifier, new List<T>())
        {}

        public Sequence(JointType identifier, IList<T> points)
        {
            _points = points;
            _identifier = identifier;
        }

        public void AddPoint(T point)
        {
            if (_points.Count < MaxNumberOfPoints)
            {
                _points.Add(point);
            }
        }

        public JointType Identifier
        {
            get { return _identifier; }
        }

        public IList<T> Points
        {
            get { return _points; }
        }
    }
}
