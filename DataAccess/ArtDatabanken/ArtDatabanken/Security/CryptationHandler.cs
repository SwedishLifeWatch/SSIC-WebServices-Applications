using System;
using System.Text;
using System.Security.Cryptography;

namespace ArtDatabanken.Security
{
    /// <summary>
    /// This class generates hashes.
    /// </summary>
    public class CryptationHandler
    {

        /// <summary>
        /// Returnerar en SHA1 hashning på 40 tecken
        /// </summary>
        /// <param name="value">Lösenordet som ska krypteras</param>
        public static string GetSHA1Hash(string value)
        {
            Byte[] clearBytes;
            clearBytes = new UTF8Encoding().GetBytes(value);
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();

            return BitConverter.ToString(sha1.ComputeHash(clearBytes)).Replace("-", ""); ;
        }
    }
}
