using Cake.Core;
using Cake.Core.IO;
using Cake.TestFairy.Internal.Interfaces;

namespace Cake.TestFairy.Internal
{
    internal class PushIpaImpl
    {
        private readonly IFileSystemProvider _fileSystemProvider;
        private readonly ITestFairyFileTransfer _fileTransferService;
        private readonly IDataMapper _dataMapper;

        public PushIpaImpl(IFileSystemProvider fileSystemProvider, ITestFairyFileTransfer fileTransferService,
            IDataMapper dataMapper)
        {
            _fileSystemProvider = fileSystemProvider;
            _fileTransferService = fileTransferService;
            _dataMapper = dataMapper;
        }

        internal TestFairyUploadResponse PushIpa(FilePath ipaFilePath, TestFairyUploadSettings settings)
        {
            var t = _fileSystemProvider.Exists(ipaFilePath);
            if (!_fileSystemProvider.Exists(ipaFilePath))
                throw new CakeException($"IPA file not found: {ipaFilePath}") {Source = "IpaFileMissing"};

            var formData = _dataMapper.MapSettings(settings);

            var response = _fileTransferService.Upload<TestFairyUploadResponse>
                ($"{settings.ServerEndpoint}/api/upload", ipaFilePath.FullPath, "file", "octet-stream", formData);
            if (response.Success)
                return response;
            throw new CakeException($"{response.Message}; Code: {response.Code}") {Source = "IpaUpload"};
        }
    }
}