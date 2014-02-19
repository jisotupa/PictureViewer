using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace PictureViewer.Tests
{
    [TestFixture]
    public class PicturesViewModelTests
    {
        private PicturesViewModel mTarget;

        [SetUp]
        public void Setup()
        {
            mTarget = new PicturesViewModel(new[] { "c:\\file1.jpg", "c:\\file2.png" });
        }

        [Test]
        public void Ctor_2_Files_CreatesViewModels()
        {
            Assert.AreEqual(2, mTarget.Pictures.Count);

            Assert.AreEqual("file:///c:/file1.jpg", mTarget.Pictures[0].Uri.ToString());
            Assert.AreEqual("file:///c:/file2.png", mTarget.Pictures[1].Uri.ToString());
        }

        [Test]
        public void Ctor_2_Files_SetsCurrentToFirstPicture()
        {
            Assert.AreEqual("file:///c:/file1.jpg", mTarget.Current.Uri.ToString());
        }

        [Test]
        public void Next_MultipleImages_OnFirst_AdvancesCurrentToSecond()
        {
            mTarget.Next();

            Assert.AreEqual("file:///c:/file2.png", mTarget.Current.Uri.ToString());
        }

        [Test]
        public void Next_MultipleImages_OnLast_AdvancesCurrentToFirst()
        {
            mTarget.Next();
            mTarget.Next();

            Assert.AreEqual("file:///c:/file1.jpg", mTarget.Current.Uri.ToString());
        }

        [Test]
        public void Next_RaisesPropertyChanged()
        {
            var called = false;

            mTarget.PropertyChanged += (sender, e) => called = e.PropertyName == "Current";

            mTarget.Next();

            Assert.IsTrue(called);
        }

        [Test]
        public void Previous_MultipleImages_OnFirst_MovesCurrentToLast()
        {
            mTarget.Previous();

            Assert.AreEqual("file:///c:/file2.png", mTarget.Current.Uri.ToString());
        }

        [Test]
        public void Previous_MultipleImages_OnLast_MovesToPrevious()
        {
            mTarget.Next();
            mTarget.Previous();

            Assert.AreEqual("file:///c:/file1.jpg", mTarget.Current.Uri.ToString());
        }

        [Test]
        public void Previous_RaisesPropertyChanged()
        {
            var called = false;

            mTarget.PropertyChanged += (sender, e) => called = e.PropertyName == "Current";

            mTarget.Previous();

            Assert.IsTrue(called);
        }

        [Test]
        public void SerializeToJson_ReturnsJson()
        {
            var result = mTarget.SerializeToJson();

            Assert.AreEqual(
                @"[{""Uri"":""file:///c:/file1.jpg"",""Angle"":180},{""Uri"":""file:///c:/file2.png"",""Angle"":180}]",
                result);
        }

        [Test]
        public void ImportJson_SetsAnglesFromJson()
        {
            var json = @"[{""Uri"":""file:///c:/file1.jpg"",""Angle"":270},{""Uri"":""file:///c:/file2.png"",""Angle"":90}]";

            mTarget.ImportJson(json);

            Assert.AreEqual(270, mTarget.Pictures[0].Angle);
            Assert.AreEqual(90, mTarget.Pictures[1].Angle);
        }
    }
}
