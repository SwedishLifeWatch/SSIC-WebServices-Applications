using System;
using ArtDatabanken.Security;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// This class holds information about one client connection.
    /// </summary>
    public class WebClientToken
    {
        private static Int32 _nextSessionId = 0;
        private static Type _lockObject = typeof(WebClientToken);

        private Int32 _sessionId;
        private String _applicationIdentifier;
        private String _clientIPAddress;
        private String _token;
        private String _userName;
        private String _webServiceName;

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
        public WebClientToken(String userName, String applicationIdentifier, String key)
        {
            _applicationIdentifier = applicationIdentifier;
            _clientIPAddress = WebServiceContext.GetClientIPAddress();
            _sessionId = GetNextSessionId();
            _userName = userName;
            _webServiceName = WebServiceData.WebServiceManager.Name;

            _token = _userName + Settings.Default.ClientTokenDelimiter +
                     _applicationIdentifier + Settings.Default.ClientTokenDelimiter +
                     _sessionId + Settings.Default.ClientTokenDelimiter +
                     _clientIPAddress + Settings.Default.ClientTokenDelimiter +
                     _webServiceName;
            _token = EncryptText(_token, key);
        }

        /// <summary>
        /// Normally used constructor.
        /// </summary>
        /// <param name="token">Token with client information.</param>
        /// <param name="key">
        /// Encryption key that is used in production.
        /// </param>
        /// <exception cref="ArgumentException">Thrown if token is not in the correct format.</exception>
        public WebClientToken(String token, String key)
        {
            String decryptedToken;
            String[] split;

            token.CheckNotNull("token");
            _token = token;
            decryptedToken = DecryptText(_token, key);
            split = decryptedToken.Split(new Char[] { Settings.Default.ClientTokenDelimiter });
            if (split.Length == 5)
            {
                _userName = split[0];
                _applicationIdentifier = split[1];
                _sessionId = Int32.Parse(split[2]);
                _clientIPAddress = split[3];
                _webServiceName = split[4];
            }
            else
            {
                throw new ArgumentException("Wrong format in client token.");
            }
        }

        /// <summary>
        /// Get identifier of the application that the user uses.
        /// </summary>
        public String ApplicationIdentifier
        {
            get { return _applicationIdentifier; }
        }

        /// <summary>
        /// Get client IP address.
        /// </summary>
        public String ClientIPAddress
        {
            get { return _clientIPAddress; }
        }

        /// <summary>
        /// Get unique session id.
        /// </summary>
        public Int32 SessionId
        {
            get { return _sessionId; }
        }

        /// <summary>
        /// Web user token. The sum of all information in this class.
        /// It is this member that is transfered between client
        /// and the web service.
        /// </summary>
        public String Token
        {
            get { return _token; }
        }

        /// <summary>
        /// Get user name.
        /// </summary>
        public String UserName
        {
            get { return _userName; }
        }

        /// <summary>
        /// Get web service name.
        /// </summary>
        public String WebServiceName
        {
            get { return _webServiceName; }
        }

        /// <summary>
        /// Check that data is valid.
        /// </summary>
        public void CheckData()
        {
            _applicationIdentifier = _applicationIdentifier.CheckInjection();
            _clientIPAddress = _clientIPAddress.CheckInjection();
            _clientIPAddress.CheckLength(Settings.Default.IPv6MaxStringLength);
            _userName = _userName.CheckInjection();
            _webServiceName = _webServiceName.CheckInjection();
        }

        /// <summary>
        /// Get max string length for client IP address.
        /// </summary>
        /// <returns>Max string length for client IP address.</returns>
        public static Int32 GetClientIPAddressMaxLength()
        {
            return Settings.Default.IPv6MaxStringLength;
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
                    if (split.Length == 5)
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

            lock (_lockObject)
            {
                nextSessionId = _nextSessionId++;
                if (_nextSessionId > 1000000000)
                {
                    // Avoid overflow
                    _nextSessionId = 0;
                }
            }
            return nextSessionId;
        }
    }
}
