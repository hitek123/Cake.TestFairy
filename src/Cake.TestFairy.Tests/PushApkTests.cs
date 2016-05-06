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
    public class PushApkTests
    {
        [Test]
        public void ThrowsCakeException_WhenIpaDoesNotExist()
        {
            using (var mock = AutoMock.GetLoose())
            {
                //Arrange
                var apkFile = new FilePath("path");
                mock.Mock<IFileSystemProvider>()
                    .Setup(x => x.Exists(apkFile))
                    .Returns(false);
                var aliasesImpl = mock.Create<PushApkImpl>();

                //Act

                try
                {
                    aliasesImpl.PushApk(apkFile, new TestFairyUploadSettings("key"));
                    Assert.Fail("Expected exception");
                }
                catch (CakeException e)
                {
                    e.Source.Should().Be("ApkFileMissing");
                }
            }
        }

        [Test]
        public void ThrowsException_WhenVerifyUtilitiesFails()
        {
            using (var mock = AutoMock.GetLoose())
            {
                //Arrange
                mock.Mock<IFileSystemProvider>()
                    .Setup(x => x.Exists(It.IsAny<FilePath>()))
                    .Returns(false);
                mock.Mock<IVerificationProvider>()
                    .Setup(x => x.VerifyUtilities(It.IsAny<TestFairyUploadSettings>()))
                    .Throws(new CakeException {Source = "U"});
                var aliasesImpl = mock.Create<PushApkImpl>();

                //Act

                try
                {
                    aliasesImpl.PushApk(new FilePath("path"), new TestFairyUploadSettings("key"));
                    Assert.Fail("Expected exception");
                }
                catch (CakeException e)
                {
                    e.Source.Should().Be("U");
                }
            }
        }

        [Test]
        public void ThrowsException_WhenValidateKeyStoreFails()
        {
            using (var mock = AutoMock.GetLoose())
            {
                //Arrange
                mock.Mock<IFileSystemProvider>()
                    .Setup(x => x.Exists(It.IsAny<FilePath>()))
                    .Returns(false);
                mock.Mock<IVerificationProvider>()
                    .Setup(x => x.ValidateKeyStore(It.IsAny<KeyStore>()))
                    .Throws(new CakeException { Source = "K"});
                var aliasesImpl = mock.Create<PushApkImpl>();

                //Act
                try
                {
                    aliasesImpl.PushApk(new FilePath("path"), new TestFairyUploadSettings("key"));
                    Assert.Fail("Expected exception");
                }
                catch (CakeException e)
                {
                    e.Source.Should().Be("K");
                }
            }
        }

        [Test]
        public void ThrowsCakeException_WhenUploadFails()
        {
            using (var mock = AutoMock.GetLoose())
            {
                //Arrange
                var apk = new FilePath("apk");
                mock.Mock<IFileSystemProvider>()
                    .Setup(x => x.Exists(It.IsAny<FilePath>()))
                    .Returns(true);
                mock.Mock<ITestFairyFileTransfer>()
                    .Setup(x => x.Upload<TestFairyUploadResponse>(It.IsAny<string>(),
                        apk, It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<NameValueCollection>()))
                    .Returns(new TestFairyUploadResponse {Status = "fail"});
                var aliasesImpl = mock.Create<PushApkImpl>();

                //Act
                try
                {
                    aliasesImpl.PushApk(apk, new TestFairyUploadSettings("key"));
                    Assert.Fail("Expected exception");
                }
                catch (CakeException e)
                {
                    e.Source.Should().Be("ApkUpload");
                }
            }
        }

        [Test]
        public void ThrowsCakeException_WhenKeySignProcesssingFails()
        {
            using (var mock = AutoMock.GetLoose())
            {
                //Arrange
                var apk = new FilePath("apk");
                var tempFiles = new TempFiles
                {
                    InstrumentedApk = new FilePath("instrument"),
                    SignedApk = new FilePath("signed")
                };
                mock.Provide(tempFiles);
                mock.Mock<IFileSystemProvider>()
                    .Setup(x => x.Exists(It.IsAny<FilePath>()))
                    .Returns(true);
                mock.Mock<IVerificationProvider>()
                   .Setup(x => x.ValidateKeyStore(It.IsAny<KeyStore>()))
                   .Returns(new KeyStore(new FilePath("keystorepath"), "pswd", "alias"));
                mock.Mock<ITestFairyFileTransfer>()
                    .Setup(x => x.Upload<TestFairyUploadResponse>(It.IsAny<string>(),
                        apk, It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<NameValueCollection>()))
                    .Returns(new TestFairyUploadResponse {Status = "ok"});
                mock.Mock<IProcessUtils>()
                    .Setup(x => x.RunCommand(It.IsAny<string>(), It.IsAny<string>()))
                    .Throws(new CakeException {Source = "A"});
                var aliasesImpl = mock.Create<PushApkImpl>();

                //Act
                try
                {
                    aliasesImpl.PushApk(apk, new TestFairyUploadSettings("key"));
                    Assert.Fail("Expected exception");
                }
                catch (CakeException e)
                {
                    e.Source.Should().Be("A");
                }
            }
        }

        [Test]
        public void ThrowsCakeException_WhenKeySigningFails()
        {
            using (var mock = AutoMock.GetLoose())
            {
                //Arrange
                var apk = new FilePath("apk");
                var tempFiles = new TempFiles
                {
                    InstrumentedApk = new FilePath("instrument"),
                    SignedApk = new FilePath("signed")
                };
                mock.Provide(tempFiles);
                mock.Mock<IFileSystemProvider>()
                    .Setup(x => x.Exists(apk))
                    .Returns(true);
                mock.Mock<IFileSystemProvider>()
                    .Setup(x => x.Exists(tempFiles.SignedApk))
                    .Returns(false);
                mock.Mock<IVerificationProvider>()
                   .Setup(x => x.ValidateKeyStore(It.IsAny<KeyStore>()))
                   .Returns(new KeyStore(new FilePath("keystorepath"), "pswd", "alias"));
                mock.Mock<ITestFairyFileTransfer>()
                    .Setup(x => x.Upload<TestFairyUploadResponse>(It.IsAny<string>(),
                        apk, It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<NameValueCollection>()))
                    .Returns(new TestFairyUploadResponse {Status = "ok"});
                var aliasesImpl = mock.Create<PushApkImpl>();

                //Act
                try
                {
                    aliasesImpl.PushApk(apk, new TestFairyUploadSettings("key"));
                    Assert.Fail("Expected exception");
                }
                catch (CakeException e)
                {
                    e.Source.Should().Be("SigningError");
                }
            }
        }

        [Test]
        public void ThrowsCakeException_WhenSignedApkUploadFails()
        {
            using (var mock = AutoMock.GetLoose())
            {
                //Arrange
                var apk = new FilePath("apk");
                var tempFiles = new TempFiles
                {
                    InstrumentedApk = new FilePath("instrument"),
                    SignedApk = new FilePath("signed")
                };
                mock.Provide(tempFiles);
                mock.Mock<IFileSystemProvider>()
                    .Setup(x => x.Exists(apk))
                    .Returns(true);
                mock.Mock<IFileSystemProvider>()
                    .Setup(x => x.Exists(tempFiles.SignedApk))
                    .Returns(true);
                mock.Mock<IVerificationProvider>()
                   .Setup(x => x.ValidateKeyStore(It.IsAny<KeyStore>()))
                   .Returns(new KeyStore(new FilePath("keystorepath"), "pswd", "alias"));
                mock.Mock<ITestFairyFileTransfer>()
                    .Setup(x => x.Upload<TestFairyUploadResponse>(It.IsAny<string>(),
                        apk, It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<NameValueCollection>()))
                    .Returns(new TestFairyUploadResponse { Status = "ok" });
                mock.Mock<ITestFairyFileTransfer>()
                    .Setup(x => x.Upload<TestFairyUploadResponse>(It.IsAny<string>(),
                        tempFiles.SignedApk, It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<NameValueCollection>()))
                    .Returns(new TestFairyUploadResponse { Status = "fail" });
                var aliasesImpl = mock.Create<PushApkImpl>();

                //Act
                try
                {
                    aliasesImpl.PushApk(apk, new TestFairyUploadSettings("key"));
                    Assert.Fail("Expected exception");
                }
                catch (CakeException e)
                {
                    e.Source.Should().Be("SignedApkUpload");
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
                var apkFilePath = new FilePath("path");
                var tempFiles = new TempFiles
                {
                    InstrumentedApk = new FilePath("instrument"),
                    SignedApk = new FilePath("signed")
                };
                mock.Provide(tempFiles);
                mock.Mock<IFileSystemProvider>()
                    .Setup(x => x.Exists(It.IsAny<FilePath>()))
                    .Returns(true);
                mock.Mock<IVerificationProvider>()
                    .Setup(x => x.ValidateKeyStore(It.IsAny<KeyStore>()))
                    .Returns(new KeyStore(new FilePath("keystorepath"), "pswd", "alias" ));
                mock.Mock<ITestFairyFileTransfer>()
                    .Setup(x => x.Upload<TestFairyUploadResponse>(It.IsAny<string>(),
                        apkFilePath, It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<NameValueCollection>()))
                    .Returns(response);
                mock.Mock<ITestFairyFileTransfer>()
                    .Setup(x => x.Upload<TestFairyUploadResponse>(It.IsAny<string>(),
                        tempFiles.SignedApk, It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<NameValueCollection>()))
                    .Returns(response);
                var aliasesImpl = mock.Create<PushApkImpl>();

                //Act
                var result = aliasesImpl.PushApk(apkFilePath, new TestFairyUploadSettings("key"));

                //Assert
                result.ShouldBeEquivalentTo(response);
            }
        }
    }
}
