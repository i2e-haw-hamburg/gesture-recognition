using GestureRecognition.Implementation.Pipeline.Interpreted.Blank;
using GestureRecognition.Implementation.TrameGestureController.Tasks;
using NUnit.Framework;

namespace GestureRecognitionTest.Pipeline.Tasks
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