using System.Runtime.CompilerServices;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;
using Cake.TestFairy.Internal;
using Cake.TestFairy.Internal.Interfaces;

[assembly:InternalsVisibleTo("Cake.TestFairy.Tests")]
[assembly:InternalsVisibleTo("TestConsole")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace Cake.TestFairy
{
    [CakeAliasCategory("TestFairy")]
    public static class TestFairyAliases
    {
        [CakeAliasCategory("Deployment")]
        [CakeMethodAlias]
        public static TestFairyUploadResponse PushIpa(ICakeContext context, FilePath ipaFilePath,
            TestFairyUploadSettings settings)
        {
            var impl = new PushIpaImpl(new CakeContextFileSystemProvider(context), new TestFairyFileTransfer(), new DataMapper());
            return impl.PushIpa(ipaFilePath, settings);
        }

        [CakeAliasCategory("Deployment")]
        [CakeMethodAlias]
        public static TestFairyUploadResponse PushApk(this ICakeContext context, FilePath apkFile,
            TestFairyUploadSettings settings)
        {
            IProcessUtils processUtils = new ProcessUtils();
            var impl = new PushApkImpl(new CakeContextFileSystemProvider(context), new TestFairyFileTransfer(),
                new VerificationProvider(processUtils), processUtils, new DataMapper(),
                TempFiles);
            return impl.PushApk(apkFile, settings);
        }

        private static TempFiles TempFiles => new TempFiles
        {
            InstrumentedApk = "temp.Instrumented.apk",
            SignedApk = "temp.ZipAligned.apk"
        };
    }
}

