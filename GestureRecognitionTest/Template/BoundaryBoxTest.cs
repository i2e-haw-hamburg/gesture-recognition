using GestureRecognition.Implementation.Pipeline.Interpreted.Template;
using TrameSkeleton.Math;
using NUnit;
using NUnit.Framework;

namespace GestureRecognitionTest.Template
{
    [TestFixture]
    public class BoundaryBoxTest
    {
        [Test]
        public void TestCreation()
        {
            var box = BoundaryBox.Create(new Vector3(), 1, 1, 1);
            Assert.AreEqual(new Vector3(), box.Position);
            Assert.AreEqual(new Vector3(1), box.Size);
        }

        [Test]
        public void TestInBox()
        {
            
        }
    }
}
