using GestureRecognition.Implementation;
using GestureRecognition.Implementation.Pipeline.Interpreted;
using GestureRecognition.Implementation.Pipeline.Interpreted.Blank;

namespace GestureRecognition.Interface
{
    /// <summary>
    /// Factury for creating the implementation of the IGestureRecognition interface.
    /// </summary>
    public static class GestureRecognitionFactory
    {
        /// <summary>
        /// Creates the recognition with a givern recognizer.
        /// </summary>
        /// <param name="recognizer">the recognizer that will be used for matching the movement with a gesture template</param>
        /// <returns></returns>
        public static IGestureRecognition Create(IRecognizer recognizer)
        {
            var controller = new Controller(recognizer);
            return new Implementation.GestureRecognition(controller);
        }

        public static IGestureRecognition Create(IController controller)
        {
            return new Implementation.GestureRecognition(controller);
        }

        /// <summary>
        /// Create the recognition with the default recognizer (ThreeDGestureRecognizer).
        /// </summary>
        /// <returns></returns>
        public static IGestureRecognition Create()
        {
            var recognition = new ThreeDGestureRecognizer();
            return Create(recognition);
        }
    }
}
