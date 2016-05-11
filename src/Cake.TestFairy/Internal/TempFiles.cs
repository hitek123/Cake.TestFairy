using Cake.Core.IO;

namespace Cake.TestFairy.Internal
{
    internal class TempFiles
    {
        public FilePath InstrumentedApk { get; set; }
        public FilePath SignedApk { get; set; }
    }
}