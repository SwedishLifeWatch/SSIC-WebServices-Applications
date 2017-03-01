using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using System.Web.Caching;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.ArtDatabankenService.Data;
using ArtDatabanken.WebService.ArtDatabankenService.Database;
using ArtDatabanken.WebService.Data;
using ApplicationIdentifier = ArtDatabanken.WebService.ArtDatabankenService.Data.ApplicationIdentifier;

namespace ArtDatabanken.WebService.ArtDatabankenService
{
    /// <summary>
    /// WebServiceContext contains information related
    /// to a singel request from one user.
    /// WebServiceContext also handles caching of data
    /// and trace of web service usage.
    /// Example of information handled in WebServiceContext:
    ///     User information.
    ///     Cache in ASP.NET.
    ///     Database connections made in this request.
    ///     Request id.
    /// </summary>
    public class WebServiceContext : IDisposable
    {
        private const String LOCAL_IP_ADDRESS = "127.0.0.1";
        private const Int32 DEFAULT_TRANSACTION_TIMEOUT = 30; // 30 seconds.
        private const Int32 MAX_TRANSACTION_TIMEOUT = 120; // 2 minutes.

        private static Boolean _isTracing;
        private static Int32 _nextRequestId;
        private static Type _lockObject;
        private static Hashtable _traceUsers;
        private static Hashtable _transactionInformation;

        private DataServer[] _databases;
        private DateTime _traceStart;
        private Int32 _requestId;
        private Int32 _transactionTimeout;
        private Object _traceArgument1;
        private Object _traceArgument2;
        private Object _traceArgument3;
        private String _traceMethod;
        private WebClientToken _clientToken;
        private WebLocale _locale;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static WebServiceContext()
        {
            _isTracing = false;
            _lockObject = typeof(WebServiceContext);
            _traceUsers = new Hashtable();
            _transactionInformation = new Hashtable();

            switch (Environment.MachineName)
            {
                case "ARTDATA-OK179":
                    _nextRequestId = 10000000;
                    break;
                case "ARTDATA-BK024":
                    _nextRequestId = 20000000;
                    break;
                case "ARTDATA-MR224":
                    _nextRequestId = 30000000;
                    break;
                case "COLIAS":
                    _nextRequestId = 40000000;
                    break;
                default:
                    _nextRequestId = 0;
                    break;
            }
        }

        /// <summary>
        /// Create a WebServiceContext instance.
        /// </summary>
        /// <param name='clientToken'>Client information.</param>
        public WebServiceContext(String clientToken)
            : this(new WebClientToken(clientToken, WebServiceData.WebServiceManager.Key), true, null, null, null, null)
        {
        }

        /// <summary>
        /// Create a WebServiceContext instance.
        /// </summary>
        /// <param name='clientToken'>Token with information about current user and connection</param>
        /// <param name='checkUser'>Information about if user authority should be checked.
        ///                         It should only be set to false during login.</param>
        public WebServiceContext(WebClientToken clientToken,
                                 Boolean checkUser)
            : this(clientToken, checkUser, null, null, null, null)
        {
        }

        /// <summary>
        /// Create a WebServiceContext instance.
        /// </summary>
        /// <param name='clientToken'>Client information.</param>
        /// <param name='traceMethod'>Name of calling method. Used during tracing.</param>
        public WebServiceContext(String clientToken,
                                 String traceMethod)
            : this(new WebClientToken(clientToken, WebServiceData.WebServiceManager.Key), true, traceMethod, null, null, null)
        {
        }

        /// <summary>
        /// Create a WebServiceContext instance.
        /// </summary>
        /// <param name='clientToken'>Client information.</param>
        /// <param name='traceMethod'>Name of calling method. Used during tracing.</param>
        /// <param name='traceArgument1'>Argument 1 to the calling method. Used during tracing.</param>
        public WebServiceContext(String clientToken,
                                 String traceMethod,
                                 Object traceArgument1)
            : this(new WebClientToken(clientToken, WebServiceData.WebServiceManager.Key), true, traceMethod, traceArgument1, null, null)
        {
        }

        /// <summary>
        /// Create a WebServiceContext instance.
        /// </summary>
        /// <param name='clientToken'>Client information.</param>
        /// <param name='traceMethod'>Name of calling method. Used during tracing.</param>
        /// <param name='traceArgument1'>Argument 1 to the calling method. Used during tracing.</param>
        /// <param name='traceArgument2'>Argument 2 to the calling method. Used during tracing.</param>
        public WebServiceContext(String clientToken,
                                 String traceMethod,
                                 Object traceArgument1,
                                 Object traceArgument2)
            : this(new WebClientToken(clientToken, WebServiceData.WebServiceManager.Key), true, traceMethod, traceArgument1, traceArgument2, null)
        {
        }

        /// <summary>
        /// Create a WebServiceContext instance.
        /// </summary>
        /// <param name='clientToken'>Client information.</param>
        /// <param name='traceMethod'>Name of calling method. Used during tracing.</param>
        /// <param name='traceArgument1'>Argument 1 to the calling method. Used during tracing.</param>
        /// <param name='traceArgument2'>Argument 2 to the calling method. Used during tracing.</param>
        /// <param name='traceArgument3'>Argument 3 to the calling method. Used during tracing.</param>
        public WebServiceContext(String clientToken,
                                 String traceMethod,
                                 Object traceArgument1,
                                 Object traceArgument2,
                                 Object traceArgument3)
            : this(new WebClientToken(clientToken, WebServiceData.WebServiceManager.Key), true, traceMethod, traceArgument1, traceArgument2, traceArgument3)
        {
        }

        /// <summary>
        /// Create a WebServiceContext instance.
        /// </summary>
        /// <param name='clientToken'>Token with information about current user and connection</param>
        /// <param name='checkUser'>Information about if user authority should be checked.
        ///                         It should only be set to false during login.</param>
        /// <param name='traceMethod'>Name of calling method. Used during tracing.</param>
        /// <param name='traceArgument1'>Argument 1 to the calling method. Used during tracing.</param>
        /// <param name='traceArgument2'>Argument 2 to the calling method. Used during tracing.</param>
        /// <param name='traceArgument3'>Argument 3 to the calling method. Used during tracing.</param>
        public WebServiceContext(WebClientToken clientToken,
                                 Boolean checkUser,
                                 String traceMethod,
                                 Object traceArgument1,
                                 Object traceArgument2,
                                 Object traceArgument3)
        {
            // Init context.
            _databases = new DataServer[Enum.GetNames(typeof(DataServer.DatabaseId)).Length];
            _clientToken = clientToken;
            _locale = null;
            _requestId = GetNextRequestId();
            _traceArgument1 = traceArgument1;
            _traceArgument2 = traceArgument2;
            _traceArgument3 = traceArgument3;
            _traceMethod = traceMethod;
            if (_isTracing)
            {
                _traceStart = DateTime.Now;
            }
            else
            {
                _traceStart = DateTime.MinValue;
            }
            _transactionTimeout = DEFAULT_TRANSACTION_TIMEOUT;

            // Check arguments.
            try
            {
                clientToken.CheckNotNull("clientToken");
                if (checkUser)
                {
                    clientToken.CheckData();
                    CheckUser();
                }
                CheckClientIPAddress();
                CheckHttpsProtocol();
            }
            catch (Exception exception)
            {
                LogManager.LogSecurityError(this, exception);
                throw exception;
            }
        }

        /// <summary>
        /// Get token for current client.
        /// </summary>
        public WebClientToken ClientToken
        {
            get { return _clientToken; }
        }

        /// <summary>
        /// Handle token for current client.
        /// </summary>
        public WebLocale Locale
        {
            get
            {
                if (_locale.IsNull())
                {
                    // Use the default locale.
                    _locale = new WebLocale();
                    _locale.Id = Settings.Default.DefaultLocaleId;
                    _locale.ISOCode = Settings.Default.DefaultLocaleISOCode;
                }
                return _locale;
            }
            set
            {
                // This web service only handles one locale.
            }
        }

        /// <summary>
        /// Get the unique id for this request.
        /// </summary>
        public Int32 RequestId
        {
            get { return _requestId; }
        }

        /// <summary>
        /// Get the unique id for this session.
        /// </summary>
        public Int32 SessionId
        {
            get { return _clientToken.SessionId; }
        }

        /// <summary>
        /// Get key used to store transaction information in HashTable.
        /// </summary>
        private String TransactionKey
        {
            get
            {
                return ClientToken.Token;
            }
        }

        /// <summary>
        /// Get  key used to store transaction
        /// id information in HashTable.
        /// </summary>
        private String TransactionIdKey
        {
            get
            {
                return ClientToken.Token + "TransactionId";
            }
        }

        /// <summary>
        /// Add object to the ASP.NET cache.
        /// </summary>
        /// <param name='cacheKey'>Key used when accessing this object.</param>
        /// <param name='cacheObject'>The object to cache.</param>
        /// <param name='absoluteExpiration'>Time when the object is removed from the cache.</param>
        /// <param name='priority'>Priority used when deciding which object that should be removed from cache.</param>
        public void AddCachedObject(String cacheKey,
                                    Object cacheObject,
                                    DateTime absoluteExpiration,
                                    CacheItemPriority priority)
        {
            AddCachedObject(cacheKey,
                            cacheObject,
                            absoluteExpiration,
                            Cache.NoSlidingExpiration,
                            priority);
        }

        /// <summary>
        /// Add object to the ASP.NET cache.
        /// </summary>
        /// <param name='cacheKey'>Key used when accessing this object.</param>
        /// <param name='cacheObject'>The object to cache.</param>
        /// <param name='slidingExpiration'>Information about how much longer from now that the object should be keept in cache.</param>
        /// <param name='priority'>Priority used when deciding which object that should be removed from cache.</param>
        public void AddCachedObject(String cacheKey,
                                    Object cacheObject,
                                    TimeSpan slidingExpiration,
                                    CacheItemPriority priority)
        {
            AddCachedObject(cacheKey,
                            cacheObject,
                            Cache.NoAbsoluteExpiration,
                            slidingExpiration,
                            priority);
        }

        /// <summary>
        /// Add object to the ASP.NET cache.
        /// </summary>
        /// <param name='cacheKey'>Key used when accessing this object.</param>
        /// <param name='cacheObject'>The object to cache.</param>
        /// <param name='absoluteExpiration'>Time when the object is removed from the cache.</param>
        /// <param name='slidingExpiration'>Information about how much longer from now that the object should be keept in cache.</param>
        /// <param name='priority'>Priority used when deciding which object that should be removed from cache.</param>
        protected virtual void AddCachedObject(String cacheKey,
                                               Object cacheObject,
                                               DateTime absoluteExpiration,
                                               TimeSpan slidingExpiration,
                                               CacheItemPriority priority)
        {
            if (HttpContext.Current.IsNotNull() &&
                HttpContext.Current.Cache.IsNotNull())
            {
                lock (HttpContext.Current.Cache)
                {
                    if (HttpContext.Current.Cache.Get(cacheKey).IsNull())
                    {
                        HttpContext.Current.Cache.Add(cacheKey,
                                                      cacheObject,
                                                      null,
                                                      absoluteExpiration,
                                                      slidingExpiration,
                                                      priority,
                                                      null);
                    }
                    else
                    {
                        HttpContext.Current.Cache.Insert(cacheKey,
                                                         cacheObject,
                                                         null,
                                                         absoluteExpiration,
                                                         slidingExpiration,
                                                         priority,
                                                         null);
                    }
                }
            }
        }

        /// <summary>
        /// Check that the clients IP address is the same in
        /// the client token as in the http request.
        /// This is done in order to limit the possibility
        /// of theft and replay of WebClientToken by other users.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if IP address is not the same as expected.</exception>
        private void CheckClientIPAddress()
        {
            if (GetClientIPAddress() != _clientToken.ClientIPAddress)
            {
                throw new ArgumentException("Invalid client IP address. " +
                                            "Current IP adress = " + GetClientIPAddress() + ". " +
                                            "ClientToken IP adress = " + _clientToken.ClientIPAddress + ".");
            }
        }

        /// <summary>
        /// Check that the web service is accessed through the
        /// https protocol.
        /// </summary>
        /// <exception cref="Exception">Thrown if https is not used.</exception>
        private void CheckHttpsProtocol()
        {
            if (HttpContext.Current.IsNotNull() &&
                HttpContext.Current.Request.IsNotNull() &&
                !HttpContext.Current.Request.IsSecureConnection &&
                !HttpContext.Current.Request.IsLocal)
            {
                throw new ApplicationException("Web service should only be accessed through protocol https!");
            }
        }

        /// <summary>
        /// Check that the client has an active transaction.
        /// </summary>
        public void CheckTransaction()
        {
            lock (_transactionInformation)
            {
                if (!HasTransaction())
                {
                    throw new ApplicationException("Required transaction is missing!");
                }
            }
        }

        /// <summary>
        /// Check if transaction has timed out and should be roll backed.
        /// </summary>
        private void CheckTransactionTimeOut()
        {
            DataServer transactionDatabase = null;
            Int32 transactionId;
            Int32 transactionTimeout;

            // Get transaction information.
            transactionTimeout = _transactionTimeout;
            if (transactionTimeout > MAX_TRANSACTION_TIMEOUT)
            {
                transactionTimeout = MAX_TRANSACTION_TIMEOUT;
            }
            lock (_transactionInformation)
            {
                if (_transactionInformation.ContainsKey(TransactionIdKey))
                {
                    transactionId = (Int32)(_transactionInformation[TransactionIdKey]);
                }
                else
                {
                    // Transaction has already been commited or aborted.
                    return;
                }
            }

            // Wait for timeout.
            Thread.Sleep(transactionTimeout * 1000);

            // Check if we have a transaction to abort.
            lock (_transactionInformation)
            {
                if (_transactionInformation.ContainsKey(TransactionIdKey))
                {
                    if (transactionId == ((Int32)(_transactionInformation[TransactionIdKey])))
                    {
                        if (HasTransaction())
                        {
                            // Undo transaction.
                            if (_transactionInformation.ContainsKey(TransactionKey))
                            {
                                transactionDatabase = (DataServer)(_transactionInformation[TransactionKey]);
                                _transactionInformation.Remove(TransactionKey);
                            }
                            if (_transactionInformation.ContainsKey(TransactionIdKey))
                            {
                                _transactionInformation.Remove(TransactionIdKey);
                            }
                        }
                        else
                        {
                            _transactionInformation.Remove(TransactionIdKey);
                        }
                    }
                    // else: A new transaction has started.
                }
                // else: Transaction id has been removed by another
                //       transaction call from the same client.
            }

            if (transactionDatabase.IsNotNull())
            {
                lock (transactionDatabase)
                {
                    transactionDatabase.RollbackTransaction();
                    transactionDatabase.Disconnect();
                }
            }
        }

        /// <summary>
        /// Authenticate user.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if user could not be authenticated.</exception>
        private void CheckUser()
        {
            if (GetUser().IsNull())
            {
                throw new ArgumentException("Invalid user information!");
            }
        }

        /// <summary>
        /// Clear data cache in webb service.
        /// </summary>
        public void ClearCache()
        {
            IDictionaryEnumerator cacheEnum;
            List<String> cacheKeys;

            // Check access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(this, ApplicationIdentifier.WebAdministration, AuthorityIdentifier.WebServiceAdministrator);

            if (HttpContext.Current.IsNotNull() &&
                HttpContext.Current.Cache.IsNotNull())
            {
                lock (HttpContext.Current.Cache)
                {
                    // Empty cache in ASP.NET.
                    cacheEnum = HttpContext.Current.Cache.GetEnumerator();
                    cacheKeys = new List<String>();
                    while (cacheEnum.MoveNext())
                    {
                        cacheKeys.Add((String)(cacheEnum.Key));
                    }
                    foreach (String cacheKey in cacheKeys)
                    {
                        HttpContext.Current.Cache.Remove(cacheKey);
                    }
                }

                LogManager.Log(this,
                               "ASP.NET cache has been emptied.",
                               ArtDatabanken.WebService.ArtDatabankenService.Data.LogType.Information,
                               null);
            }
        }

        /// <summary>
        /// Commit a transaction.
        /// </summary>
        public void CommitTransaction()
        {
            DataServer transactionDatabase = null;

            lock (_transactionInformation)
            {
                if (HasTransaction())
                {
                    transactionDatabase = (DataServer)(_transactionInformation[TransactionKey]);
                    _transactionInformation.Remove(TransactionKey);
                    if (_transactionInformation.ContainsKey(TransactionIdKey))
                    {
                        _transactionInformation.Remove(TransactionIdKey);
                    }
                }
                else
                {
                    throw new ApplicationException("No transaction to commit!");
                }
            }

            if (transactionDatabase.IsNotNull())
            {
                lock (transactionDatabase)
                {
                    transactionDatabase.CommitTransaction();
                    transactionDatabase.Disconnect();
                }
            }
        }

        /// <summary>
        /// Implementation of the IDisposable interface.
        /// Releases all resources related to this request.
        /// </summary>
        public void Dispose()
        {
            Int32 databaseIndex;
            String method;
            TimeSpan duration;

            if (_isTracing)
            {
                if (_traceUsers.IsEmpty() ||
                    _traceUsers.ContainsKey(ClientToken.UserName))
                {
                    duration = DateTime.Now - _traceStart;
                    method = Environment.StackTrace;
                    method = method.Substring(method.IndexOf(":line"));
                    method = method.Substring(method.IndexOf("."));
                    method = method.Substring(1, method.IndexOf("(") - 1) + "()";
                    LogManager.Log(this,
                                   method + ", duration = " + duration.Milliseconds + " milliseconds.",
                                   ArtDatabanken.WebService.ArtDatabankenService.Data.LogType.Trace,
                                   null);
                }
            }

            for (databaseIndex = 0; databaseIndex < _databases.Length; databaseIndex++)
            {
                if (_databases[databaseIndex].IsNotNull())
                {
                    try
                    {
                        _databases[databaseIndex].Disconnect();
                    }
                    catch
                    {
                    }
                    _databases[databaseIndex] = null;
                }
            }
        }

        /// <summary>
        /// Get object from the ASP.NET cache.
        /// </summary>
        /// <param name='cacheKey'>Key used when accessing this object.</param>
        /// <returns>Cached object or null if no object was found.</returns>
        public virtual Object GetCachedObject(String cacheKey)
        {
            Object cachedObject = null;

            if (HttpContext.Current.IsNotNull() &&
                HttpContext.Current.Cache.IsNotNull())
            {
                cachedObject = HttpContext.Current.Cache.Get(cacheKey);
            }
            return cachedObject;
        }

#if OLD_WEB_SERVICE_ADDRESS
        /// <summary>
        /// Get web service client IP address.
        /// </summary>
        /// <returns>IP address.</returns>
        public static String GetClientIPAddress()
        {
            String clientIPAddress;

            if (HttpContext.Current.IsNull() ||
                HttpContext.Current.Request.IsNull() ||
                HttpContext.Current.Request.UserHostAddress.IsEmpty())
            {
                // Local ip address used during test.
                clientIPAddress = LOCAL_IP_ADDRESS;
            }
            else
            {
                clientIPAddress = HttpContext.Current.Request.UserHostAddress;
            }

            return clientIPAddress;
        }
#else
        /// <summary>
        /// Get web service client IP address.
        /// </summary>
        /// <returns>IP address.</returns>
        public static String GetClientIPAddress()
        {
            String clientIpAddress;

            if (HttpContext.Current.IsNull() ||
                HttpContext.Current.Request.IsNull() ||
                HttpContext.Current.Request.UserHostAddress.IsEmpty())
            {
                // Local ip address is used during test.
                clientIpAddress = LOCAL_IP_ADDRESS;
            }
            else
            {
                if (Configuration.InstallationType == InstallationType.Production)
                {
                    clientIpAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (clientIpAddress.IsEmpty() ||
                        (clientIpAddress.ToLower() == "unknown"))
                    {
                        throw new ApplicationException("Can not retrive client IP address.");
                    }
                }
                else
                {
                    clientIpAddress = HttpContext.Current.Request.UserHostAddress;
                }
            }

            //if (HttpContext.Current.IsNotNull() &&
            //    HttpContext.Current.Request.IsNotNull() &&
            //    HttpContext.Current.Request.UserHostAddress.IsNotEmpty() &&
            //    HttpContext.Current.Request.ServerVariables.IsNotNull())
            //{
            //WebServiceData.LogManager.Log(this,
            //                              "HTTP_CLIENT_IP = " + HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"],
            //                              LogType.Information,
            //                              null);
            //WebServiceData.LogManager.Log(this,
            //                              "HTTP_X_FORWARDED_FOR = " + HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"],
            //                              LogType.Information,
            //                              null);
            //WebServiceData.LogManager.Log(this,
            //                              "REMOTE_ADDR = " + HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"],
            //                              LogType.Information,
            //                              null);
            //}

            return clientIpAddress;
        }
#endif

        /// <summary>
        /// Get database instance.
        /// </summary>
        /// <param name='databaseId'>Id of the database to get.</param>
        /// <returns>Requested database instance.</returns>
        public DataServer GetDatabase(DataServer.DatabaseId databaseId)
        {
            if (HasTransaction() &&
                (databaseId == GetTransactionDatabase().GetDatabaseId()))
            {
                return GetTransactionDatabase();
            }

            if (_databases[(int)databaseId].IsNull())
            {
                _databases[(int)databaseId] = new DataServer(databaseId);
            }
            return _databases[(int)databaseId];
        }

        /// <summary>
        /// Get a unique id that can identifiy this request.
        /// </summary>
        /// <returns>Request id</returns>
        private static Int32 GetNextRequestId()
        {
            Int32 nextRequestId;

            lock (_lockObject)
            {
                nextRequestId = _nextRequestId++;
                if (_nextRequestId > 1000000000)
                {
                    // Avoid overflow
                    _nextRequestId = 0;
                }
            }
            return nextRequestId;
        }

        /// <summary>
        /// Get role with specified id.
        /// Only roles that are relevant in
        /// the given context may be returned.
        /// </summary>
        /// <param name='roleId'>Role id.</param>
        /// <returns>Role with specified id.</returns>
        public WebRole GetRole(Int32 roleId)
        {
            foreach (WebRole role in GetRoles())
            {
                if (role.Id == roleId)
                {
                    return role;
                }
            }

            return null;
        }


        /// <summary>
        /// Get roles for current web service user.
        /// </summary>
        /// <returns>Roles for current web service user. </returns>
        public List<WebRole> GetRoles()
        {
            return WebServiceData.UserManager.GetRoles(this);
        }

        /// <summary>
        /// Get database instance that is used in a transaction.
        /// </summary>
        /// <returns>The database instance.</returns>
        public DataServer GetTransactionDatabase()
        {
            DataServer database = null;

            lock (_transactionInformation)
            {
                if (_transactionInformation.Contains(TransactionKey))
                {
                    database = (DataServer)(_transactionInformation[TransactionKey]);
                }
            }

            return database;
        }

        /// <summary>
        /// Get information about current web service user.
        /// </summary>
        /// <returns>
        /// Returns user information or null if 
        /// user information is not valid.
        /// It is only during login that the
        /// user information can be invalid.
        /// </returns>
        public ArtDatabanken.WebService.Data.WebUser GetUser()
        {
            return WebServiceData.UserManager.GetUser(this);
        }

        /// <summary>
        /// Test if we have a database instance that
        /// is used in a transaction.
        /// </summary>
        /// <returns>
        /// True if we have a database instance that
        /// is used in a transaction.
        /// </returns>
        private Boolean HasTransaction()
        {
            return _transactionInformation.Contains(TransactionKey);
        }

        /// <summary>
        /// Remove object from the ASP.NET cache.
        /// </summary>
        /// <param name='cacheKey'>Key used when accessing this object.</param>
        public void RemoveCachedObject(String cacheKey)
        {
            if (HttpContext.Current.IsNotNull() &&
                HttpContext.Current.Cache.IsNotNull())
            {
                lock (HttpContext.Current.Cache)
                {
                    HttpContext.Current.Cache.Remove(cacheKey);
                }
            }
        }

        /// <summary>
        /// Rollback a transaction.
        /// </summary>
        public void RollbackTransaction()
        {
            DataServer transactionDatabase = null;

            lock (_transactionInformation)
            {
                // Undo transaction.
                if (_transactionInformation.ContainsKey(TransactionKey))
                {
                    transactionDatabase = (DataServer)(_transactionInformation[TransactionKey]);
                    _transactionInformation.Remove(TransactionKey);
                }
                if (_transactionInformation.ContainsKey(TransactionIdKey))
                {
                    _transactionInformation.Remove(TransactionIdKey);
                }
            }

            if (transactionDatabase.IsNotNull())
            {
                lock (transactionDatabase)
                {
                    transactionDatabase.RollbackTransaction();
                    transactionDatabase.Disconnect();
                }
            }
        }

        /// <summary>
        /// Start trace usage of web service for specified user.
        /// If no user is specified then all usage of web service
        /// is traced.
        /// Note: Tracing has negativ impact on web service performance.
        /// </summary>
        /// <param name="userName">User name.</param>
        public void StartTrace(String userName)
        {
            // Check access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(this, ApplicationIdentifier.WebAdministration, AuthorityIdentifier.WebServiceAdministrator);

            _isTracing = true;
            if (userName.IsNotEmpty())
            {
                _traceUsers.Add(userName, _isTracing);
            }
        }

        /// <summary>
        /// Start a transaction.
        /// </summary>
        /// <param name="timeout">
        /// Time before transaction timeouts if has not already finished.
        /// Unit is seconds.
        /// </param>
        public void StartTransaction(Int32 timeout)
        {
            Boolean abortTransaction = false;
            DataServer transactionDatabase = null;
            Thread transactionThread;

            _transactionTimeout = timeout;
            lock (_transactionInformation)
            {
                if (HasTransaction())
                {
                    // StartTransaction should only be called once
                    // to start the transaction.
                    abortTransaction = true;

                    // Undo transaction.
                    if (_transactionInformation.ContainsKey(TransactionKey))
                    {
                        transactionDatabase = (DataServer)(_transactionInformation[TransactionKey]);
                        _transactionInformation.Remove(TransactionKey);
                    }
                    if (_transactionInformation.ContainsKey(TransactionIdKey))
                    {
                        _transactionInformation.Remove(TransactionIdKey);
                    }
                }
                else
                {
                    // Start transaction.
                    transactionDatabase = new DataServer(DataServer.DatabaseId.SpeciesFact);
                    transactionDatabase.BeginTransaction();
                    _transactionInformation.Add(TransactionKey, transactionDatabase);
                    _transactionInformation.Add(TransactionIdKey, RequestId);
                }
            }

            if (abortTransaction)
            {
                if (transactionDatabase.IsNotNull())
                {
                    lock (transactionDatabase)
                    {
                        transactionDatabase.RollbackTransaction();
                        transactionDatabase.Disconnect();
                    }
                }
                throw new ApplicationException("Transaction has already been started!");
            }

            // Start thread that control time out of transaction.
            transactionThread = new Thread(new ThreadStart(CheckTransactionTimeOut));
            transactionThread.Priority = ThreadPriority.Highest;
            transactionThread.Start();
        }

        /// <summary>
        /// Stop tracing usage of web service.
        /// </summary>
        public void StopTrace()
        {
            // Check access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(this, ApplicationIdentifier.WebAdministration, AuthorityIdentifier.WebServiceAdministrator);

            _isTracing = false;
            _traceUsers.Clear();
        }
    }
}
