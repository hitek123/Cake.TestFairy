using Cake.Core.IO;

namespace Cake.TestFairy.Internal.Interfaces
{
    public interface IFileSystemProvider
    {
        bool Exists(FilePath filePath);
    }
}