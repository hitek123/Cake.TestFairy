using System.Collections.Specialized;
using Cake.Core.IO;

namespace Cake.TestFairy.Internal.Interfaces
{
    internal interface ITestFairyFileTransfer
    {
        T Upload<T>(string url, FilePath filePath, string fileParameterName,
            string fileContentType, NameValueCollection nameValueCollection);

        void Download(string uri, FilePath filePath);
    }
}