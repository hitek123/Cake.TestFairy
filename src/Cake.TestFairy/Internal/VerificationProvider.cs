using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Cake.Core;
using Cake.TestFairy.Internal.Interfaces;

namespace Cake.TestFairy.Internal
{
    class VerificationProvider : IVerificationProvider
    {
        private readonly IProcessUtils _processUtils;

        public VerificationProvider(IProcessUtils processUtils)
        {
            _processUtils = processUtils;
        }

        public KeyStore ValidateKeyStore(KeyStore keyStore)
        {
            if (keyStore == null)
                throw new CakeException(
                    "Please update KeyStoreFilePath, KeyStorePassword and KeyStoreAlias with your jar signing credentials")
                {Source = "ValidateKeyStoreParameterNull"};

            if (keyStore.KeyStoreFilePath != null && !string.IsNullOrWhiteSpace(keyStore.KeyStoreAlias) &&
                !string.IsNullOrWhiteSpace(keyStore.KeyStorePassword))
            {
                _processUtils.RunCommand("keytool.exe",
                    $"-list -keystore {keyStore.KeyStoreFilePath} -storepass {keyStore.KeyStorePassword} -alias {keyStore.KeyStoreAlias}");
                return keyStore;
            }
            else
                throw new CakeException(
                    "Please update KeyStoreFilePath, KeyStorePassword and KeyStoreAlias with your jar signing credentials")
                { Source = "ValidateKeyStoreParameterError" };
        }

        public void VerifyUtilities(TestFairyUploadSettings testFairyUploadSettings)
        {
            var missingUtils = new List<string>();
            if (!_processUtils.CanExecute(testFairyUploadSettings.Jarsigner))
                missingUtils.Add(testFairyUploadSettings.Jarsigner);
            if (!_processUtils.CanExecute(testFairyUploadSettings.KeyTool))
                missingUtils.Add(testFairyUploadSettings.KeyTool);
            if (!_processUtils.CanExecute(testFairyUploadSettings.ZipAlign))
                missingUtils.Add(testFairyUploadSettings.ZipAlign);
            if (!_processUtils.CanExecute(testFairyUploadSettings.ZipUtility))
                missingUtils.Add(testFairyUploadSettings.ZipUtility);

            if (missingUtils.Any())
                throw new CakeException(
                    $"Unable to locate the following utilies:{Environment.NewLine}{string.Join(Environment.NewLine, missingUtils)}")
                {
                    Source = "VerifyUtilities"
                };
        }
    }
}