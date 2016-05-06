using Cake.Core.IO;

namespace Cake.TestFairy
{
    public class KeyStore
    {

        public KeyStore()
        {
            
        }

        public KeyStore(FilePath keyStoreFilePath, string keyStorePassword, string keyStoreAliasa)
        {
            KeyStoreAlias = keyStoreAliasa;
            KeyStoreFilePath = keyStoreFilePath;
            KeyStorePassword = keyStorePassword;
        }

        public FilePath KeyStoreFilePath { get; set; }
        public string KeyStorePassword { get; set; }
        public string KeyStoreAlias { get; set; }

        public override string ToString()
        {
            return $"KeyStoreFilePath: {KeyStoreFilePath}, KeyStorePassword: {KeyStorePassword}, KeyStoreAlias: {KeyStoreAlias}";
        }
    }
}