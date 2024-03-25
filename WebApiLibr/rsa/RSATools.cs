using System.Security.Cryptography;

namespace WebApiLibr.rsa
{
    public static class RSATools
    {
        public static RSA GetPrivateKey()
        {
            return GetRSAKey(@"..\WebApiLibr\rsa\private_key.pem");
        }

        public static RSA GetPublicKey()
        {
            return GetRSAKey(@"..\WebApiLibr\rsa\public_key.pem");
        }

        private static RSA GetRSAKey(string path)
        {
            var f = File.ReadAllText(path);

            var rsa = RSA.Create();
            rsa.ImportFromPem(f);
            return rsa;
        }
    }
}
