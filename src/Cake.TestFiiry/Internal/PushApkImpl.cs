using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.TestFairy.Internal.Interfaces;

namespace Cake.TestFairy.Internal
{
    internal class PushApkImpl
    {
        private readonly IFileSystemProvider _fileSystemProvider;
        private readonly ITestFairyFileTransfer _fileTransferService;
        private readonly IVerificationProvider _verificationProvider;
        private readonly IProcessUtils _processUtils;
        private readonly IDataMapper _dataMapper;
        private readonly TempFiles _tempFiles;

        public PushApkImpl(IFileSystemProvider fileSystemProvider, ITestFairyFileTransfer fileTransferService,
            IVerificationProvider verificationProvider, IProcessUtils processUtils,
            IDataMapper dataMapper, TempFiles tempFiles)
        {
            _fileSystemProvider = fileSystemProvider;
            _fileTransferService = fileTransferService;
            _verificationProvider = verificationProvider;
            _processUtils = processUtils;
            _dataMapper = dataMapper;
            _tempFiles = tempFiles;
        }

        internal TestFairyUploadResponse PushApk(FilePath apkFilePath,
            TestFairyUploadSettings settings)
        {
            {
                Console.WriteLine("Validating request...");
                _verificationProvider.VerifyUtilities(settings);
                KeyStore keyStore = _verificationProvider.ValidateKeyStore(settings.KeyStore);

                if (!_fileSystemProvider.Exists(apkFilePath))
                    throw new CakeException($"APK file not found: {apkFilePath}") { Source = "ApkFileMissing" };

                var formData = _dataMapper.MapSettings(settings);

                Console.WriteLine($"Uploading APK: {apkFilePath}...");
                var response = _fileTransferService.Upload<TestFairyUploadResponse>
                    ($"{settings.ServerEndpoint}/api/upload", apkFilePath, "apk_file", "octet-stream", formData);
                if (!response.Success)
                    throw new CakeException($"{response.Message}; Code: {response.Code}") {Source = "ApkUpload"};

                Console.WriteLine($"Downloading instrumented APK: {response.Instrumented_url}...");
                _fileTransferService.Download(response.Instrumented_url, _tempFiles.InstrumentedApk);

                Console.WriteLine($"Re-signing APK file...");
                var quotedApkPath = $"\"{_tempFiles.InstrumentedApk}\"";
                var quotedAlignedApkPath = $"\"{_tempFiles.SignedApk.FullPath}\"";
                var quotedKeyStorePath = $"\"{keyStore.KeyStoreFilePath.FullPath}\"";
                _processUtils.RunCommand(settings.ZipUtility, $"-qd {quotedApkPath} META-INF/*");
                _processUtils.RunCommand(settings.Jarsigner,
                    $"-keystore {quotedKeyStorePath} -storepass {keyStore.KeyStorePassword} -digestalg SHA1 -sigalg MD5withRSA {quotedApkPath} {settings.KeyStore.KeyStoreAlias}");
                _processUtils.RunCommand(settings.Jarsigner, $"-verify {quotedApkPath}");
                _processUtils.RunCommand(settings.ZipAlign, $"-f 4 {quotedApkPath} {quotedAlignedApkPath}");

                if (!_fileSystemProvider.Exists(_tempFiles.SignedApk))
                    throw new CakeException($"Signed APK file not found: {_tempFiles.SignedApk.FullPath}")
                    { Source = "SigningError" };

                Console.WriteLine("Uploading signed APK File to TestFairy...");
                response = _fileTransferService.Upload<TestFairyUploadResponse>
                    ($"{settings.ServerEndpoint}/api/upload-signed", _tempFiles.SignedApk, "apk_file", "octet-stream", formData);
                if (response.Success)
                    return response;
                throw new CakeException($"{response.Message}; Code: {response.Code}; apk_file {_tempFiles.SignedApk.FullPath}")
                { Source = "SignedApkUpload" };
            }

        }
    }
}