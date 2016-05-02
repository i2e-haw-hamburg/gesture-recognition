using GestureRecognition.Implementation.Pipeline.Interpreted;
using GestureRecognition.Implementation.Pipeline.Interpreted.Blank;
using GestureRecognition.Implementation.Pipeline.Task;
using NUnit.Framework;

namespace GestureRecognitionTest.Pipeline.Task
{
    [TestFixture]
    public class RecognitionTaskTest
    {
        [Test]
        public void TestInitialisation()
        {
            var task = new RecognitionTask(new ThreeDGestureRecognizer());
            Assert.IsTrue(task.Recognizer.GetType().IsInstanceOfType(new ThreeDGestureRecognizer()));
        }

    }
}