using System.Collections.Generic;
using GestureRecognition.Implementation.Pipeline.Interpreted.Template;
using TrameSkeleton.Math;

namespace GestureRecognition.Implementation.Pipeline.Interpreted.Kristensson
{
    class Pattern
    {
        public Pattern(ITemplate template, IList<IList<Vector3>> segments)
        {
            this.Template = template;
            this.Segments = segments;
        }

        public ITemplate Template { get; set; }

        public IList<IList<Vector3>> Segments { get; set; }
    }
}