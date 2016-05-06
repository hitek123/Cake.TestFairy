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
    public class VerificationProviderTest
    {
        [Test]
        public void ValidateKeyStore_ThrowsCakeException_WhenKeyStoreIsNull()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var provider = mock.Create<VerificationProvider>();

                try
                {
                    provider.ValidateKeyStore(null);
                    Assert.Fail("Expected exception");
                }
                catch (CakeException e)
                {
                    e.Source.Should().Be("ValidateKeyStoreParameterNull");
                }
            }
        }

        [Test]
        public void ValidateKeyStore_ThrowsCakeException_WhenKeyStoreIsIncomplete()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var provider = mock.Create<VerificationProvider>();

                try
                {
                    provider.ValidateKeyStore(new KeyStore());
                    Assert.Fail("Expected exception");
                }
                catch (CakeException e)
                {
                    e.Source.Should().Be("ValidateKeyStoreParameterError");
                }
            }
        }

        [Test]
        public void ValidateKeyStore_ThrowsCakeException_WhenKeyStoreIsInvalid()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var keyStore = new KeyStore(new FilePath("keystore"), "pswd", "alias");
                mock.Mock<IProcessUtils>()
                    .Setup(x => x.RunCommand("keytool.exe", It.IsAny<string>()))
                    .Throws(new CakeException() { Source = "P" });
                var provider = mock.Create<VerificationProvider>();

                try
                {
                    provider.ValidateKeyStore(keyStore);
                    Assert.Fail("Expected exception");
                }
                catch (CakeException e)
                {
                    e.Source.Should().Be("P");
                }
            }
        }

        [Test]
        public void ValidateKeyStore_ReturnsKeyStore_WhenKeyStoreIsValid()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var keyStore = new KeyStore(new FilePath("keystore"), "pswd", "alias");
                var provider = mock.Create<VerificationProvider>();

                KeyStore result = provider.ValidateKeyStore(keyStore);

                result.Should().Be(keyStore);
            }
        }
    }
}