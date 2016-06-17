using System;
using System.Collections.Generic;
using GestureRecognition.Utility;
using NUnit;
using NUnit.Framework;
using TrameSkeleton.Math;

namespace GestureRecognitionTest.Utility
{
    [TestFixture]
    public class GeometryTest
    {
        [Test]
        public void TestDirectTo()
        {
            Assert.IsTrue(Geometry.DirectTo(Vector3.Zero, Vector3.Right, new Vector3(2, 0, 0)));
            Assert.IsFalse(Geometry.DirectTo(Vector3.Zero, Vector3.Left, new Vector3(2, 0, 0)));
            Assert.IsTrue(Geometry.DirectTo(Vector3.Zero, Vector3.Right, new Vector3(4, 1, 0), 45));
            Assert.IsFalse(Geometry.DirectTo(Vector3.Zero, Vector3.Right, new Vector3(3, 1, 0)));

            Assert.IsTrue(Geometry.DirectTo(new Vector3(123.5262, 432.3945, 133.2612), new Vector3(-0.3469545, -0.5146277, -0.7840797), new Vector3(11.2197, 261.1236, -58.43291), 35));
            Assert.IsTrue(Geometry.DirectTo(new Vector3(11.2197, 261.1236, -58.43291), new Vector3(0.704173, 0.105439, 0.7021537), new Vector3(123.5262, 432.3945, 133.2612), 35));
        }

        [Test]
        public void TestRelativeRotationAroundX()
        {
            var start = new Quaternion(1, 0, 0, 0);
            var target = new Quaternion(1, 1, 0, 0);
            var result = Geometry.RelativeRotation(start, target);
            Assert.AreEqual(0, result.X, 0.000001);
            Assert.AreEqual(0, result.Y, 0.000001);
            Assert.AreEqual(Math.PI / 2, result.Z, 0.000001);
            
            target = new Quaternion(0, 1, 0, 0);
            result = Geometry.RelativeRotation(start, target);
            Assert.AreEqual(0, result.X, 0.000001);
            Assert.AreEqual(0, result.Y, 0.000001);
            Assert.AreEqual(Math.PI, result.Z, 0.000001);

            target = new Quaternion(0, 0, 0.7071, -0.7071);
            result = Geometry.RelativeRotation(start, target);
            Assert.AreEqual(Math.PI, result.X, 0.000001);
            Assert.AreEqual(0, result.Y, 0.000001);
            Assert.AreEqual(-Math.PI / 2, result.Z, 0.000001);

            target = new Quaternion(0.5, -0.5, -0.5, 0.5);
            result = Geometry.RelativeRotation(start, target);
            Assert.AreEqual(-Math.PI / 2, result.X, 0.000001);
            Assert.AreEqual(0, result.Y, 0.000001);
            Assert.AreEqual(-Math.PI / 2, result.Z, 0.000001);

            target = new Quaternion(0.5, 0.5, 0.5, 0.5);
            result = Geometry.RelativeRotation(start, target);
            Assert.AreEqual(Math.PI / 2, result.X, 0.000001);
            Assert.AreEqual(0, result.Y, 0.000001);
            Assert.AreEqual(Math.PI / 2, result.Z, 0.000001);
        }
        
        [Test]
        public void TestRelativeRotationAroundY()
        {
            var start = new Quaternion(1, 0, 0, 0);
            var target = new Quaternion(1, 0, 1, 0);
            var result = Geometry.RelativeRotation(start, target);
            Assert.AreEqual(Math.PI / 2, result.X, 0.000001);
            Assert.AreEqual(0, result.Y, 0.000001);
            Assert.AreEqual(0, result.Z, 0.000001);

            target = new Quaternion(0.7071, 0, 0.7071, 0);
            result = Geometry.RelativeRotation(start, target);
            Assert.AreEqual(Math.PI / 2, result.X, 0.000001);
            Assert.AreEqual(0, result.Y, 0.000001);
            Assert.AreEqual(0, result.Z, 0.000001);

            target = new Quaternion(0, 0, 1, 0);
            result = Geometry.RelativeRotation(start, target);
            Assert.AreEqual(Math.PI, result.X, 0.000001);
            Assert.AreEqual(0, result.Y, 0.000001);
            Assert.AreEqual(0, result.Z, 0.000001);

            target = new Quaternion(0.7071, 0, -0.7071, 0);
            result = Geometry.RelativeRotation(start, target);
            Assert.AreEqual(-Math.PI / 2, result.X, 0.000001);
            Assert.AreEqual(0, result.Y, 0.000001);
            Assert.AreEqual(0, result.Z, 0.000001);
        }

        [Test]
        public void TestRelativeRotationAroundZ()
        {
            var start = new Quaternion(1, 0, 0, 0);
            var target = new Quaternion(0.7071, 0, 0, 0.7071);
            var result = Geometry.RelativeRotation(start, target);
            Assert.AreEqual(0, result.X, 0.000001);
            Assert.AreEqual(-Math.PI / 2, result.Y, 0.000001);
            Assert.AreEqual(0, result.Z, 0.000001);

            target = new Quaternion(0.5, 0.5, -0.5, 0.5);
            result = Geometry.RelativeRotation(start, target);
            Assert.AreEqual(0, result.X, 0.000001);
            Assert.AreEqual(-Math.PI / 2, result.Y, 0.000001);
            Assert.AreEqual(0, result.Z, 0.000001);

            target = new Quaternion(0.5, 0.5, 0.5, -0.5);
            result = Geometry.RelativeRotation(start, target);
            Assert.AreEqual(0, result.X, 0.000001);
            Assert.AreEqual(Math.PI / 2, result.Y, 0.000001);
            Assert.AreEqual(0, result.Z, 0.000001);
            
            target = new Quaternion(0, 0.7071, 0.7071, 0);
            result = Geometry.RelativeRotation(start, target);
            Assert.AreEqual(0, result.X, 0.000001);
            Assert.AreEqual(Math.PI / 2, result.Y, 0.000001);
            Assert.AreEqual(0, result.Z, 0.000001);
        }

        [Test]
        public void TestRelativeRotationEquals()
        {
            var expected = Vector3.Zero;
            var actual = Geometry.RelativeRotation(new Quaternion(1, 2, 3, 4), new Quaternion(1, 2, 3, 4));
            Assert.AreEqual(expected, actual);
        }
    }
}