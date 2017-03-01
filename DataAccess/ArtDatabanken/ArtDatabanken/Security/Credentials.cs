using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ArtDatabanken.Security
{
    /// <summary>
    /// A class that exposes windows loginprompt
    /// </summary>
    public class CredentialsUI
    {
        const int MAX_USER_NAME = 32;
        const int MAX_PASSWORD = 32;
        const int MAX_DOMAIN = 2;

        /// <summary>
        /// Get credentials from user.
        /// </summary>
        public static CredUIReturnCodes PromptForCredentials(ref CREDUI_INFO creditUI,
                                                             string targetName,
                                                             int netError,
                                                             ref string userName,
                                                             ref string password,
                                                             ref bool save,
                                                             CREDUI_FLAGS flags)
        {
            StringBuilder user = new StringBuilder(MAX_USER_NAME);
            StringBuilder pwd = new StringBuilder(MAX_PASSWORD);
            creditUI.cbSize = Marshal.SizeOf(creditUI);

            CredUIReturnCodes result = CredUIPromptForCredentials(ref creditUI,
                                                                  targetName,
                                                                  IntPtr.Zero,
                                                                  netError,
                                                                  user,
                                                                  MAX_USER_NAME,
                                                                  pwd,
                                                                  MAX_PASSWORD,
                                                                  ref save,
                                                                  flags);

            userName = user.ToString();
            password = pwd.ToString();

            return result;
        }

        [DllImport("credui")]
        private static extern CredUIReturnCodes CredUIPromptForCredentials(ref CREDUI_INFO creditUR,
                                                                           string targetName,
                                                                           IntPtr reserved1,
                                                                           int iError,
                                                                           StringBuilder userName,
                                                                           int maxUserName,
                                                                           StringBuilder password,
                                                                           int maxPassword,
                                                                           [MarshalAs(UnmanagedType.Bool)] ref bool pfSave,
                                                                           CREDUI_FLAGS flags);
    }

    /// <summary>
    /// Definition of flags when credentials is requested from user.
    /// </summary>
    [Flags]
    public enum CREDUI_FLAGS
    {
        /// <summary> Incorrect password. </summary>
        INCORRECT_PASSWORD = 0x1,
        /// <summary> Do not persist. </summary>
        DO_NOT_PERSIST = 0x2,
        /// <summary> Request administrator. </summary>
        REQUEST_ADMINISTRATOR = 0x4,
        /// <summary> Exclude certificates. </summary>
        EXCLUDE_CERTIFICATES = 0x8,
        /// <summary> Require certificate. </summary>
        REQUIRE_CERTIFICATE = 0x10,
        /// <summary> Show save check box. </summary>
        SHOW_SAVE_CHECK_BOX = 0x40,
        /// <summary> Always show ui. </summary>
        ALWAYS_SHOW_UI = 0x80,
        /// <summary> Require smartcard. </summary>
        REQUIRE_SMARTCARD = 0x100,
        /// <summary> Password only ok. </summary>
        PASSWORD_ONLY_OK = 0x200,
        /// <summary> Validate username. </summary>
        VALIDATE_USERNAME = 0x400,
        /// <summary> Complete username. </summary>
        COMPLETE_USERNAME = 0x800,
        /// <summary> Persist. </summary>
        PERSIST = 0x1000,
        /// <summary> Server credential. </summary>
        SERVER_CREDENTIAL = 0x4000,
        /// <summary> Expect confirmation. </summary>
        EXPECT_CONFIRMATION = 0x20000,
        /// <summary> Generic credentials. </summary>
        GENERIC_CREDENTIALS = 0x40000,
        /// <summary> Username target credentials. </summary>
        USERNAME_TARGET_CREDENTIALS = 0x80000,
        /// <summary> Keep username. </summary>
        KEEP_USERNAME = 0x100000,
    }

    /// <summary>
    /// Definition of credential information.
    /// </summary>
    public struct CREDUI_INFO
    {
        /// <summary> Size. </summary>
        public int cbSize;
        /// <summary> Parent windows. </summary>
        public IntPtr hwndParent;
        /// <summary> Message. </summary>
        public string pszMessageText;
        /// <summary> Titel. </summary>
        public string pszCaptionText;
        /// <summary> Banner. </summary>
        public IntPtr hbmBanner;
    }

    /// <summary>
    /// Return codes when retrieving credential information.
    /// </summary>
    public enum CredUIReturnCodes
    {
        /// <summary> No error. </summary>
        NO_ERROR = 0,
        /// <summary> Cancelled. </summary>
        ERROR_CANCELLED = 1223,
        /// <summary> No such logon session. </summary>
        ERROR_NO_SUCH_LOGON_SESSION = 1312,
        /// <summary> Not found. </summary>
        ERROR_NOT_FOUND = 1168,
        /// <summary> Invalid account name. </summary>
        ERROR_INVALID_ACCOUNT_NAME = 1315,
        /// <summary> Insufficient buffer. </summary>
        ERROR_INSUFFICIENT_BUFFER = 122,
        /// <summary> Invalid parameter. </summary>
        ERROR_INVALID_PARAMETER = 87,
        /// <summary> Invalid flags. </summary>
        ERROR_INVALID_FLAGS = 1004,
    }
}
