using GestureRecognition.Implementation.Pipeline.Physical;
using NUnit.Framework;
using TrameSkeleton.Math;

namespace GestureRecognitionTest.Pipeline.Physical
{
    [TestFixture]
    public class PhysicCalculationTest
    {
        [Test]
        public void TestCalculateVelocity()
        {
            var j1 = new Vector3();
            var j2 = new Vector3(1);
            var time = 1;
            Assert.AreEqual(new Vector3(1), PhysicCalculation.CalculateVelocity(j1, j2, time));
        }

        [Test]
        public void TestCalculateAcceleration()
        {
            var j1 = new Vector3();
            var j2 = new Vector3(1);
            var j3 = new Vector3(3);
            var time1 = 1;
            var time2 = 1;
            Assert.AreEqual(new Vector3(1), PhysicCalculation.CalculateAcceleration(j1, j2, j3, time1, time2));
        }

        [Test]
        public void TestCreatePhysicCommand()
        {
            
        }
    }
}