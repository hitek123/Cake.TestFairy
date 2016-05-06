using System;
using Cake.Core;
using Cake.TestFairy.Internal;
using Cake.TestFairy.Internal.Interfaces;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Cake.TestFairy.Tests
{
    public class VerifyUtilitiesTests
    {
        private Mock<IProcessUtils> _processUtilsMock;
        private IVerificationProvider _provider;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _processUtilsMock = new Mock<IProcessUtils>();

            _processUtilsMock
                .Setup(x => x.CanExecute("fail"))
                .Returns(false);
            _processUtilsMock
                .Setup(x => x.CanExecute("pass"))
                .Returns(true);
            _provider = new VerificationProvider(_processUtilsMock.Object);

        }

        [Test]
        public void VerifyUtilities_NoException_WhenAllUtilitiesCanExecute()
        {
            //Arrange
            var settings = new TestFairyUploadSettings("apiKey")
            {
                Jarsigner = "pass",
                KeyTool = "pass",
                ZipAlign = "pass",
                ZipUtility = "pass"
            };

            //Act
            _provider.VerifyUtilities(settings);
        }

        [Test]
        public void VerifyUtilities_ThrowsCakeException_WhenJarsignerCanNotExecute()
        {
            //Arrange
            var settings = new TestFairyUploadSettings("apiKey")
            {
                Jarsigner = "fail",
                KeyTool = "pass",
                ZipAlign = "pass",
                ZipUtility = "pass"
            };

            //Act
            try
            {
                _provider.VerifyUtilities(settings);
                Assert.Fail("Expected exception");
            }
            catch (CakeException e)
            {
                e.Source.Should().Be("VerifyUtilities");
            }
        }

        [Test]
        public void VerifyUtilities_ThrowsCakeException_WhenKeytoolCanNotExecute()
        {
            //Arrange
            var settings = new TestFairyUploadSettings("apiKey")
            {
                Jarsigner = "pass",
                KeyTool = "fail",
                ZipAlign = "pass",
                ZipUtility = "pass"
            };

            //Act
            try
            {
                _provider.VerifyUtilities(settings);
                Assert.Fail("Expected exception");
            }
            catch (CakeException e)
            {
                e.Source.Should().Be("VerifyUtilities");
            }
        }

        [Test]
        public void VerifyUtilities_ThrowsCakeException_WhenZipAlignCanNotExecute()
        {
            //Arrange
            var settings = new TestFairyUploadSettings("apiKey")
            {
                Jarsigner = "pass",
                KeyTool = "pass",
                ZipAlign = "fail",
                ZipUtility = "pass"
            };

            //Act
            try
            {
                _provider.VerifyUtilities(settings);
                Assert.Fail("Expected exception");
            }
            catch (CakeException e)
            {
                e.Source.Should().Be("VerifyUtilities");
            }
        }

        [Test]
        public void VerifyUtilities_ThrowsCakeException_WhenZipUtilityCanNotExecute()
        {
            //Arrange
            var settings = new TestFairyUploadSettings("apiKey")
            {
                Jarsigner = "pass",
                KeyTool = "pass",
                ZipAlign = "pass",
                ZipUtility = "fail"
            };

            //Act
            try
            {
                _provider.VerifyUtilities(settings);
                Assert.Fail("Expected exception");
            }
            catch (CakeException e)
            {
                e.Source.Should().Be("VerifyUtilities");
            }
        }

    }
}