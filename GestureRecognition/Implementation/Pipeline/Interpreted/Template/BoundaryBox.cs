using System;
using TrameSkeleton.Math;

namespace GestureRecognition.Implementation.Pipeline.Interpreted.Template
{
    public class BoundaryBox
    {
        public static BoundaryBox Create(Vector3 first, Vector3 vector3, int i, int i1, int i2)
        {
            return new BoundaryBox();
        }

        public Boolean IsIn(Vector3 first)
        {
            throw new System.NotImplementedException();
        }
    }
}