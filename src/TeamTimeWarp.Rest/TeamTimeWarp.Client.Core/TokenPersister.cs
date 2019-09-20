using System.IO;
using System.IO.IsolatedStorage;

namespace TeamTimeWarp.Client.Core
{
    public class TokenPersister : ITokenPersister
    {
        //warning throws.
        private const string FileName =  @"login.dat";

        public bool TokenExists()
        {
            using (var isolatedFile = GetIsolatedStorageFile())
            {
                using (isolatedFile)
                {
                    return isolatedFile.FileExists(FileName);
                }
            }
        }

        public void RemoveToken()
        {
            using (var isolatedFile = GetIsolatedStorageFile())
            {
                isolatedFile.DeleteFile(FileName);
            }
        }

        public LoginToken GetToken()
        {
            using (var isolatedFile = GetIsolatedStorageFile())
            {
                using (var isolatedStorage = new IsolatedStorageFileStream(FileName, FileMode.Open, isolatedFile))
                {
                    using (var writer = new StreamReader(isolatedStorage))
                    {
                        return new LoginToken(writer.ReadToEnd());
                    }
                }
            }
        }

        private static IsolatedStorageFile GetIsolatedStorageFile()
        {
            IsolatedStorageFile isolatedFile = IsolatedStorageFile.GetMachineStoreForAssembly();
            return isolatedFile;
        }

        public void PersistToken(LoginToken tokenStore)
        {
            using (var isolatedFile = GetIsolatedStorageFile())
            {
                using (var isolatedStorage = new IsolatedStorageFileStream(FileName, FileMode.CreateNew, isolatedFile))
                {
                    using (var writer = new StreamWriter(isolatedStorage))
                    {
                        writer.WriteLine(tokenStore.TokenStr);
                        writer.Close();
                    }
                }
            }
        }
    }
}