using System;
using NUnit.Framework;

namespace PictureViewer.Tests
{
    [TestFixture]
    public class PictureViewModelTests
    {
        private PictureViewModel mTarget;

        [SetUp]
        public void Setup()
        {
            mTarget = new PictureViewModel("c:\\file.jpg");
        }

        [Test]
        public void Ctor_SetsAngleTo180ByDefault()
        {
            Assert.AreEqual(180, mTarget.Angle);
        }

        [TestCase(0, Result = 0)]
        [TestCase(90, Result = 90)]
        [TestCase(180, Result = 180)]
        [TestCase(270, Result = 270)]
        [TestCase(360, ExpectedException = typeof(ArgumentOutOfRangeException))]
        [TestCase(-1, ExpectedException = typeof(ArgumentOutOfRangeException))]
        public int Angle_Changes(int validAngle)
        {
            mTarget.Angle = validAngle;

            return mTarget.Angle;
        }

        [Test]
        public void Angle_NoChange_DoesNotRaisePropertyChanged()
        {
            var called = false;
            mTarget.Angle = 0;

            mTarget.PropertyChanged += (sender, e) => called = e.PropertyName == "Angle";

            mTarget.Angle = 0;

            Assert.IsFalse(called);
        }

        [Test]
        public void Angle_Change_RaisesPropertyChanged()
        {
            var called = false;
            mTarget.Angle = 0;

            mTarget.PropertyChanged += (sender, e) => called = e.PropertyName == "Angle";

            mTarget.Angle = 90;

            Assert.IsTrue(called);
        }

        [TestCase(0, Result = 90)]
        [TestCase(90, Result = 180)]
        [TestCase(180, Result = 270)]
        [TestCase(270, Result = 0)]
        public int RotateClockwise_IncreasesAngleBy90(int initialAngle)
        {
            mTarget.Angle = initialAngle;

            mTarget.RotateClockwise();

            return mTarget.Angle;
        }

        [Test]
        public void RotateClockwise_RaisesPropertyChanged()
        {
            var called = false;

            mTarget.PropertyChanged += (sender, e) => called = e.PropertyName == "Angle";

            mTarget.RotateClockwise();

            Assert.IsTrue(called);
        }

        [TestCase(0, Result = 270)]
        [TestCase(90, Result = 0)]
        [TestCase(180, Result = 90)]
        [TestCase(270, Result = 180)]
        public int RotateCounterClockwise_DecreasesAngleBy90(int initialAngle)
        {
            mTarget.Angle = initialAngle;

            mTarget.RotateCounterClockwise();

            return mTarget.Angle;
        }

        [Test]
        public void RotateCounterClockwise_RaisesPropertyChanged()
        {
            var called = false;

            mTarget.PropertyChanged += (sender, e) => called = e.PropertyName == "Angle";

            mTarget.RotateCounterClockwise();

            Assert.IsTrue(called);
        }
    }
}
