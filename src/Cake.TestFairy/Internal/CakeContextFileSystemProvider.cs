using Cake.Core;
using Cake.Core.IO;
using Cake.TestFairy.Internal.Interfaces;

namespace Cake.TestFairy.Internal
{
    /// <summary>
    /// Wrapper for neede functionality in IContext, which was hard to Mock
    /// </summary>
    /// <seealso cref="IFileSystemProvider" />
    class CakeContextFileSystemProvider : IFileSystemProvider
    {
        /// <summary>
        /// The _context
        /// </summary>
        private readonly ICakeContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeContextFileSystemProvider"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public CakeContextFileSystemProvider(ICakeContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Existses the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public bool Exists(FilePath filePath)
        {
            return _context.FileSystem.Exist(filePath);
        }
    }
}