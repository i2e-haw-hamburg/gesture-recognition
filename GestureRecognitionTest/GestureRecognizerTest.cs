using System;
using System.Collections.Generic;
using GestureRecognition.Implementation.Pipeline.Interpreted;
using GestureRecognition.Implementation.Pipeline.Interpreted.Blank;
using GestureRecognition.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TrameSkeleton.Math;

namespace GestureRecognitionTest
{
    [TestClass]
    public class GestureRecognizerTest
    {
        private IRecognizer _recognizer;

        [TestInitialize]
        public void TestSetup()
        {
            _recognizer = new ThreeDGestureRecognizer();
        }

        [TestMethod]
        public void TestNormalizeSingle()
        {

        }
        
    }
}
