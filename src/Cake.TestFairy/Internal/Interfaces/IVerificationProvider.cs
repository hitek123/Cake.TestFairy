namespace Cake.TestFairy.Internal.Interfaces
{
    internal interface IVerificationProvider
    {
        KeyStore ValidateKeyStore(KeyStore keyStore);
        void VerifyUtilities(TestFairyUploadSettings testFairyUploadSettings);
    }
}