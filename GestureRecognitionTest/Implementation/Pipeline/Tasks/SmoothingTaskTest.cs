using System.Collections.Generic;
using GestureRecognition.Implementation.TrameGestureController.Tasks;
using NUnit.Framework;
using TrameSkeleton.Math;

namespace GestureRecognitionTest.Pipeline.Tasks
{
    [TestFixture]
    public class SmoothingTaskTest
    {
        [Test]
        public void TestMeanVector3()
        {
            var first = new Vector3();
            var second = new Vector3(1);
            var third = new Vector3(2);
            Assert.AreEqual(new Vector3(0.5), SmoothingTask.Mean(new List<Vector3> {first, second}));
            Assert.AreEqual(new Vector3(2.0 / 3), SmoothingTask.Mean(new List<Vector3> { first, second, third }));

        }


        [Test]
        public void TestMeanVector4()
        {
            var first = new Vector4();
            var second = new Vector4(1);
            var third = new Vector4(2);
            Assert.AreEqual(new Vector4(0.5), SmoothingTask.Mean(new List<Vector4> { first, second }));
            Assert.AreEqual(new Vector4(2.0 / 3), SmoothingTask.Mean(new List<Vector4> { first, second, third }));
        }

    }
}