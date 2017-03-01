using System;
using ArtDatabanken.Security;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class holds information about one client connection.
    /// </summary>
    public class WebClientToken
    {
        private static Int32 mNextSessionId;
        private static readonly Type LockObject = typeof(WebClientToken);

        private readonly DateTime mCreatedDate;
        private readonly Int32 mSessionId;
        private String mApplicationIdentifier;
        private String mClientIpAddress;
        private readonly String mToken;
        private String mUserName;
        private String mWebServiceName;

        /// <summary>
        /// Constructor that should only be used after a successful login.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="applicationIdentifier">
        /// Identifier of the application that the user uses.
        /// </param>
        /// <param name="key">
        /// Encryption key that is used in production.
        /// </param>
        public WebClientToken(String userName,
                              String applicationIdentifier,
                              String key)
        {
            mApplicationIdentifier = applicationIdentifier;
            mClientIpAddress = WebServiceContext.GetClientIpAddress();
            mCreatedDate = DateTime.Now;
            mSessionId = GetNextSessionId();
            mUserName = userName;
            mWebServiceName = WebServiceData.WebServiceManager.Name;

            mToken = GetRandomText() + Settings.Default.ClientTokenDelimiter +
                     mCreatedDate.WebToString() + Settings.Default.ClientTokenDelimiter +
                     mUserName + Settings.Default.ClientTokenDelimiter +
                     mApplicationIdentifier + Settings.Default.ClientTokenDelimiter +
                     mSessionId + Settings.Default.ClientTokenDelimiter +
                     mClientIpAddress + Settings.Default.ClientTokenDelimiter +
                     mWebServiceName + Settings.Default.ClientTokenDelimiter +
                     GetRandomText();
            mToken = EncryptText(mToken, key);
        }

        /// <summary>
        /// Normaly used constructor.
        /// </summary>
        /// <param name="token">Token with client information.</param>
        /// <param name="key">
        /// Decryption key that is used in production.
        /// </param>
        /// <exception cref="ArgumentException">Thrown if token is not in the correct format.</exception>
        public WebClientToken(String token,
                              String key)
        {
            String decryptedToken;
            String[] split;

            token.CheckNotEmpty("token");
            mToken = token;
            decryptedToken = DecryptText(mToken, key);
            split = decryptedToken.Split(new [] { Settings.Default.ClientTokenDelimiter });
            switch (split.Length)
            {
                case 5:
                    // Old token format.
                    // TODO: Delete this code when all
                    // TODO: web services has been updated.
                    mCreatedDate = new DateTime(1900, 1, 1);
                    mUserName = split[0];
                    mApplicationIdentifier = split[1];
                    mSessionId = Int32.Parse(split[2]);
                    mClientIpAddress = split[3];
                    mWebServiceName = split[4];
                    break;

                case 6:
                    // New token format.
                    mCreatedDate = split[0].WebParseDateTime();
                    mUserName = split[1];
                    mApplicationIdentifier = split[2];
                    mSessionId = Int32.Parse(split[3]);
                    mClientIpAddress = split[4];
                    mWebServiceName = split[5];
                    break;


                case 8:
                    // Newest token format.
                    mCreatedDate = split[1].WebParseDateTime();
                    mUserName = split[2];
                    mApplicationIdentifier = split[3];
                    mSessionId = Int32.Parse(split[4]);
                    mClientIpAddress = split[5];
                    mWebServiceName = split[6];
                    break;

                default:
                    throw new ArgumentException("Wrong format in client token.");
            }
        }

        /// <summary>
        /// Get identifier of the application that the user uses.
        /// </summary>
        public String ApplicationIdentifier
        {
            get { return mApplicationIdentifier; }
        }

        /// <summary>
        /// Get client IP address.
        /// </summary>
        public String ClientIpAddress
        {
            get { return mClientIpAddress; }
        }

        /// <summary>
        /// Get date and time when the token was created.
        /// </summary>
        public DateTime CreatedDate
        {
            get { return mCreatedDate; }
        }

        /// <summary>
        /// Get unique session id.
        /// </summary>
        public Int32 SessionId
        {
            get { return mSessionId; }
        }

        /// <summary>
        /// Web user token. The sum of all information in this class.
        /// It is this member that is transfered between client
        /// and the web service.
        /// </summary>
        public String Token
        {
            get { return mToken; }
        }

        /// <summary>
        /// Get user name.
        /// </summary>
        public String UserName
        {
            get { return mUserName; }
        }

        /// <summary>
        /// Get web service name.
        /// </summary>
        public String WebServiceName
        {
            get { return mWebServiceName; }
        }

        /// <summary>
        /// Check that data is valid.
        /// </summary>
        public void CheckData()
        {
            mApplicationIdentifier = mApplicationIdentifier.CheckInjection();
            mClientIpAddress = mClientIpAddress.CheckInjection();
            mClientIpAddress.CheckLength(Settings.Default.IPv6MaxStringLength);
            mUserName = mUserName.CheckInjection();
            mWebServiceName = mWebServiceName.CheckInjection();
        }

        /// <summary>
        /// Decrypt text.
        /// </summary>
        /// <param name="text">Text that should be decrypted.</param>
        /// <param name="key">
        /// Decryption key that is used in production.
        /// </param>
        /// <returns>Decrypted text.</returns>
        private String DecryptText(String text, String key)
        {
            CipherString cipherString;
            String decryptedText;
            String[] split;

            cipherString = new CipherString();
            if (key.IsEmpty())
            {
                return cipherString.DecryptText(text);
            }
            else
            {
                try
                {
                    decryptedText = cipherString.DecryptText(text, key);
                    split = decryptedText.Split(new[] { Settings.Default.ClientTokenDelimiter });
                    // TODO: Delete alternative 5 and 6 when all
                    // TODO: web services has been updated.
                    if ((split.Length == 5) || (split.Length == 6) || (split.Length == 8))
                    {
                        return decryptedText;
                    }
                    else
                    {
                        // Maybe old token is used.
                        return cipherString.DecryptText(text);
                    }
                }
                catch 
                {
                    // Maybe old token is used.
                    return cipherString.DecryptText(text);
                }
            }
        }

        /// <summary>
        /// Encrypt text.
        /// </summary>
        /// <param name="text">Text that should be encrypted.</param>
        /// <param name="key">
        /// Encryption key that is used in production.
        /// </param>
        /// <returns>Encrypted text.</returns>
        private String EncryptText(String text, String key)
        {
            CipherString cipherString;

            cipherString = new CipherString();
            if (key.IsEmpty())
            {
                return cipherString.EncryptText(text);
            }
            else
            {
                return cipherString.EncryptText(text, key);
            }
        }

        /// <summary>
        /// Get a unique id that can identifiy this session.
        /// </summary>
        /// <returns>Session id</returns>
        private static Int32 GetNextSessionId()
        {
            Int32 nextSessionId;

            lock (LockObject)
            {
                nextSessionId = mNextSessionId++;
                if (mNextSessionId > 1000000000)
                {
                    // Avoid overflow.
                    mNextSessionId = 0;
                }
            }
            return nextSessionId;
        }


        /// <summary>
        /// Get random text to be used as salt in encrypted information.
        /// </summary>
        /// <returns>Random text to be used as salt in encrypted information.</returns>
        public static String GetRandomText()
        {
            PasswordGenerator passwordGenerator;

            passwordGenerator = new PasswordGenerator();
            passwordGenerator.Minimum = (DateTime.Now.Millisecond % Settings.Default.SaltMaxLength) + 1;
            passwordGenerator.Maximum = passwordGenerator.Minimum + 1;
            passwordGenerator.Exclusions = Settings.Default.ClientTokenDelimiter.ToString();
            return passwordGenerator.Generate();
        }
    }
}
