using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GestureRecognition.Implementation.Pipeline
{
    /// <summary>
    /// Initializer shortcuts for pipeline creation.
    /// </summary>
    static class Initializer
    {
        public static IPipeline CreatePipeline()
        {
            throw new System.NotImplementedException();
        }

        public static IPipeline CreatePipeline(Controller controller, IPipeline pipeline1)
        {
            controller.NewData += pipeline1.OnNewData;
            pipeline1.Ready += controller.FireNewCommand;
            return pipeline1;
        }

        public static IPipeline CreatePipeline(Controller controller, IPipeline pipeline1, IPipeline pipeline2)
        {
            controller.NewData += pipeline1.OnNewData;
            pipeline1.Ready += pipeline2.OnNewData;
            pipeline2.Ready += controller.FireNewCommand;
            return pipeline1;
        }

        public static IPipeline CreatePipeline(Controller controller, IPipeline pipeline1, IPipeline pipeline2, IPipeline pipeline3)
        {
            controller.NewData += pipeline1.OnNewData;
            pipeline1.Ready += pipeline2.OnNewData;
            pipeline2.Ready += pipeline3.OnNewData;
            pipeline3.Ready += controller.FireNewCommand;
            return pipeline1;
        }

        public static IPipeline CreatePipeline(Controller controller, IPipeline pipeline1, IPipeline pipeline2, IPipeline pipeline3, IPipeline pipeline4)
        {
            controller.NewData += pipeline1.OnNewData;
            pipeline1.Ready += pipeline2.OnNewData;
            pipeline2.Ready += pipeline3.OnNewData;
            pipeline3.Ready += pipeline4.OnNewData;
            pipeline4.Ready += controller.FireNewCommand;
            return pipeline1;
        }
    }
}
