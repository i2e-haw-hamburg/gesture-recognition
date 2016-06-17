using System.Collections.Generic;
using GestureRecognition.Utility;
using Leap;
using NUnit;
using NUnit.Framework;

namespace GestureRecognitionTest.Utility
{
    [TestFixture]
    public class GeometryTest
    {
        [Test]
        public void TestDirectTo()
        {
            Assert.IsTrue(Geometry.DirectTo(Vector.Zero, Vector.Right, new Vector(2, 0, 0)));
            Assert.IsFalse(Geometry.DirectTo(Vector.Zero, Vector.Left, new Vector(2, 0, 0)));
            Assert.IsTrue(Geometry.DirectTo(Vector.Zero, Vector.Right, new Vector(4, 1, 0), 45));
            Assert.IsFalse(Geometry.DirectTo(Vector.Zero, Vector.Right, new Vector(3, 1, 0)));

            Assert.IsTrue(Geometry.DirectTo(new Vector(123.5262f, 432.3945f, 133.2612f), new Vector(-0.3469545f, -0.5146277f, -0.7840797f), new Vector(11.2197f, 261.1236f, -58.43291f), 35));
            Assert.IsTrue(Geometry.DirectTo(new Vector(11.2197f, 261.1236f, -58.43291f), new Vector(0.704173f, 0.105439f, 0.7021537f), new Vector(123.5262f, 432.3945f, 133.2612f), 35));
        }
    }
}