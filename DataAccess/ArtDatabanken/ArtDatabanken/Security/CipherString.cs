using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;

namespace ArtDatabanken.Security
{
    /// <summary>
    /// This class cipher strings.
    /// The cipher is computer dependent.
    /// An encrypted string must be
    /// decrypted on the same computer.
    /// </summary>
    public class CipherString
    {
        /// <summary>
        /// Decimal value for the ascii character A.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        private const Byte CharacterAAsciiValue = 65;

        /// <summary>
        /// Entropy for use in cryptography.
        /// </summary>
        private readonly Byte[] cryptographyEntropy = { 54, 73, 12, 91, 237, 159, 172 };

        /// <summary>
        /// Add entropy to cipher data.
        /// </summary>
        /// <param name='cipherData'>Cipher data.</param>
        /// <returns>Cipher data with entropy.</returns>
        private Byte[] AddEntropy(Byte[] cipherData)
        {
            Byte[] cipherEntropyData;
            Int32 cipherEntropyIndex, index;

            cipherEntropyData = new byte[cipherData.Length + cryptographyEntropy.Length];
            cipherEntropyIndex = 0;
            for (index = 0; index < cryptographyEntropy.Length; index++)
            {
                cipherEntropyData[cipherEntropyIndex] = cryptographyEntropy[index];
                cipherEntropyIndex++;
            }

            for (index = 0; index < cipherData.Length; index++)
            {
                cipherEntropyData[cipherEntropyIndex] = cipherData[index];
                cipherEntropyIndex++;
            }

            return cipherEntropyData;
        }

        /// <summary>
        /// Decrypt text.
        /// </summary>
        /// <param name='cipherText'>Text to decrypt.</param>
        /// <returns>Decrypted text.</returns>
        public String DecryptText(String cipherText)
        {
            byte[] cipherData;
            String text = null;

            if (cipherText.IsNotEmpty())
            {
                cipherData = UnicodeEncoding.ASCII.GetBytes(cipherText);
                cipherData = GetBytesFromAsciiFriendlyBytes(cipherData);
                cipherData = ProtectedData.Unprotect(cipherData, this.cryptographyEntropy, DataProtectionScope.LocalMachine);
                text = UnicodeEncoding.UTF8.GetString(cipherData);
            }

            return text;
        }

        /// <summary>
        /// Decrypt text.
        /// </summary>
        /// <param name='cipherText'>Text to decrypt.</param>
        /// <param name="key">Decryption key.</param>
        /// <returns>Decrypted text.</returns>
        public String DecryptText(String cipherText, String key)
        {
            byte[] cipherData, cipherKey;
            Int32 dataIndex, keyIndex;
            String text;

            text = null;
            if (cipherText.IsNotEmpty())
            {
                cipherData = UnicodeEncoding.ASCII.GetBytes(cipherText);
                cipherData = GetBytesFromAsciiFriendlyBytes(cipherData);
                cipherKey = UnicodeEncoding.UTF8.GetBytes(key);
                keyIndex = 0;
                for (dataIndex = 0; dataIndex < cipherData.Length; dataIndex++)
                {
                    cipherData[dataIndex] = (byte)(cipherData[dataIndex] ^ cipherKey[keyIndex]);
                    keyIndex++;
                    if (keyIndex >= cipherKey.Length)
                    {
                        keyIndex = 0;
                    }
                }

                cipherData = RemoveEntropy(cipherData);
                text = UnicodeEncoding.UTF8.GetString(cipherData);
            }

            return text;
        }

        /// <summary>
        /// Encrypt text.
        /// </summary>
        /// <param name='text'>Text to encrypt.</param>
        /// <returns>Encrypted text.</returns>
        public String EncryptText(String text)
        {
            byte[] cipherData;
            String cipherText = null;

            if (text.IsNotEmpty())
            {
                cipherData = UnicodeEncoding.UTF8.GetBytes(text);
                cipherData = ProtectedData.Protect(cipherData, this.cryptographyEntropy, DataProtectionScope.LocalMachine);
                cipherData = GetAsciiFriendlyBytes(cipherData);
                cipherText = UnicodeEncoding.ASCII.GetString(cipherData);
            }

            return cipherText;
        }

        /// <summary>
        /// Encrypt text.
        /// </summary>
        /// <param name='text'>Text to encrypt.</param>
        /// <param name="key">Encryption key.</param>
        /// <returns>Encrypted text.</returns>
        public String EncryptText(String text, String key)
        {
            byte[] cipherData, chiperKey;
            Int32 dataIndex, keyIndex;
            String cipherText;

            cipherText = null;
            if (text.IsNotEmpty())
            {
                cipherData = UnicodeEncoding.UTF8.GetBytes(text);
                cipherData = AddEntropy(cipherData);
                chiperKey = UnicodeEncoding.UTF8.GetBytes(key);
                keyIndex = 0;
                for (dataIndex = 0; dataIndex < cipherData.Length; dataIndex++)
                {
                    cipherData[dataIndex] = (byte)(cipherData[dataIndex] ^ chiperKey[keyIndex]);
                    keyIndex++;
                    if (keyIndex >= chiperKey.Length)
                    {
                        keyIndex = 0;
                    }
                }

                cipherData = GetAsciiFriendlyBytes(cipherData);
                cipherText = UnicodeEncoding.ASCII.GetString(cipherData);
            }

            return cipherText;
        }

        /// <summary>
        /// Convert data to format that is possible
        /// to read as simple text.
        /// </summary>
        /// <param name='chiperDataInput'>Data that should be converted.</param>
        /// <returns>Data as text.</returns>
        private byte[] GetAsciiFriendlyBytes(byte[] chiperDataInput)
        {
            byte[] cipherDataOutput = null;
            int byteIndex = 0;

            if (chiperDataInput != null)
            {
                cipherDataOutput = new byte[chiperDataInput.Length * 2];
                foreach (byte chiperByte in chiperDataInput)
                {
                    cipherDataOutput[byteIndex++] += (byte)(CharacterAAsciiValue + ((chiperByte & 0xF0) >> 4));
                    cipherDataOutput[byteIndex++] += (byte)(CharacterAAsciiValue + (chiperByte & 0x0F));
                }
            }

            return cipherDataOutput;
        }

        /// <summary>
        /// Convert data as text to byte format.
        /// </summary>
        /// <param name='chiperDataInput'>Data that should be converted.</param>
        /// <returns>Data as bytes.</returns>
        private byte[] GetBytesFromAsciiFriendlyBytes(byte[] chiperDataInput)
        {
            byte cipherData;
            byte[] cipherDataOutput = null;
            int inputByteIndex = 0, outputByteIndex;

            if (chiperDataInput != null)
            {
                cipherDataOutput = new byte[chiperDataInput.Length / 2];
                for (outputByteIndex = 0; outputByteIndex < cipherDataOutput.Length; outputByteIndex++)
                {
                    cipherData = (byte)((chiperDataInput[inputByteIndex++] - CharacterAAsciiValue) << 4);
                    cipherData += (byte)(chiperDataInput[inputByteIndex++] - CharacterAAsciiValue);
                    cipherDataOutput[outputByteIndex] = cipherData;
                }
            }

            return cipherDataOutput;
        }

        /// <summary>
        /// Remove entropy from cipher data.
        /// </summary>
        /// <param name='cipherEntropyData'>Cipher data with entropy.</param>
        /// <returns>Cipher data without entropy.</returns>
        private Byte[] RemoveEntropy(Byte[] cipherEntropyData)
        {
            Byte[] cipherData;
            Int32 cipherEntropyIndex, index;

            cipherData = new byte[cipherEntropyData.Length - cryptographyEntropy.Length];
            cipherEntropyIndex = cryptographyEntropy.Length;
            for (index = 0; index < cipherData.Length; index++)
            {
                cipherData[index] = cipherEntropyData[cipherEntropyIndex];
                cipherEntropyIndex++;
            }

            return cipherData;
        }
    }
}
