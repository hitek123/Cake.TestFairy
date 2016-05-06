using System.Collections.Specialized;
using Autofac.Extras.Moq;
using Cake.Core;
using Cake.Core.IO;
using Cake.TestFairy.Internal;
using Cake.TestFairy.Internal.Interfaces;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Cake.TestFairy.Tests
{
    public class PushIpaTests
    {
        [Test]
        public void ThrowsCakeException_WhenIpaDoesNotExist()
        {
            using (var mock = AutoMock.GetLoose())
            {
                //Arrange
                mock.Mock<IFileSystemProvider>()
                    .Setup(x => x.Exists(It.IsAny<FilePath>()))
                    .Returns(false);
                var aliasesImpl = mock.Create<PushIpaImpl>();

                //Act
                Assert.Throws<CakeException>(() =>
                    aliasesImpl.PushIpa(new FilePath("path"), new TestFairyUploadSettings("key")));

                try
                {
                    aliasesImpl.PushIpa(new FilePath("path"), new TestFairyUploadSettings("key"));
                    Assert.Fail("Expected exception");
                }
                catch (CakeException e)
                {
                    e.Source.Should().Be("IpaFileMissing");
                }
            }
        }

        [Test]
        public void ThrowsCakeException_WhenUploadFails()
        {
            using (var mock = AutoMock.GetLoose())
            {
                //Arrange
                mock.Mock<IFileSystemProvider>()
                    .Setup(x => x.Exists(It.IsAny<FilePath>()))
                    .Returns(true);
                mock.Mock<ITestFairyFileTransfer>()
                    .Setup(x => x.Upload<TestFairyUploadResponse>(It.IsAny<string>(),
                        It.IsAny<FilePath>(), It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<NameValueCollection>()))
                    .Returns(new TestFairyUploadResponse {Status = "fail"});
                var aliasesImpl = mock.Create<PushIpaImpl>();

                //Act

                try
                {
                    aliasesImpl.PushIpa(new FilePath("path"), new TestFairyUploadSettings("key"));
                    Assert.Fail("Expected exception");
                }
                catch (CakeException e)
                {
                    e.Source.Should().Be("IpaUpload");
                }
            }
        }

        [Test]
        public void ReturnsResponse_WhenUploadSucceeds()
        {
            using (var mock = AutoMock.GetLoose())
            {
                //Arrange
                var response = new TestFairyUploadResponse
                {
                    Status = "ok",
                    App_name = "app_name"
                };
                mock.Mock<IFileSystemProvider>()
                    .Setup(x => x.Exists(It.IsAny<FilePath>()))
                    .Returns(true);
                mock.Mock<ITestFairyFileTransfer>()
                    .Setup(x => x.Upload<TestFairyUploadResponse>(It.IsAny<string>(),
                        It.IsAny<FilePath>(), It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<NameValueCollection>()))
                    .Returns(response);
                var aliasesImpl = mock.Create<PushIpaImpl>();

                //Act
                var result = aliasesImpl.PushIpa(new FilePath("path"), new TestFairyUploadSettings("key"));

                //Assert
                result.ShouldBeEquivalentTo(response);
            }
        }
    }
}
