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
            var box = BoundaryBox.Create(new Vector3(), 1, 1, 1);
            Assert.IsTrue(box.IsIn(new Vector3(0.01)));
            Assert.IsTrue(box.IsIn(new Vector3(0.99)));
            Assert.IsTrue(box.IsIn(new Vector3(0.5)));

            Assert.IsFalse(box.IsIn(new Vector3(1.01)));
            Assert.IsFalse(box.IsIn(new Vector3(-0.1, 0.5, 0.3)));
            Assert.IsFalse(box.IsIn(new Vector3(-2)));
        }
    }
}
