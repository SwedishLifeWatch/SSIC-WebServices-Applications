using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using System.Web.Caching;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Database;

namespace ArtDatabanken.WebService
{
    /// <summary>
    /// Delegate for event handling of commit transaction.
    /// </summary>
    /// <param name="context">Web service context.</param>
    public delegate void CommitTransactionEventHandler(WebServiceContext context);

    /// <summary>
    /// WebServiceContext contains information related
    /// to a singel request from one user.
    /// Example of information handled in WebServiceContext:
    ///     User information.
    ///     Cache in ASP.NET.
    ///     Database connections made in this request.
    ///     Request id.
    ///     Trace of web service usage.
    /// </summary>
    public class WebServiceContext : IDisposable
    {
        private static Boolean _isTracing;
        private static Int32 _nextRequestId;
        private readonly static Type _lockObject;
        private readonly static Hashtable _traceUsers;
        private readonly static Hashtable _transactionInformation;

        private WebServiceDataServer _database;
        private readonly DateTime _traceStart;
        private readonly Int32 _requestId;
        private Int32 _transactionTimeout;
        private readonly WebClientToken _clientToken;
        private WebLocale _locale;
        private WebRole _currentRole;

        /// <summary>
        /// Event handling of commit transaction.
        /// </summary>
        public static event CommitTransactionEventHandler CommitTransactionEvent;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static WebServiceContext()
        {
            _isTracing = false;
            _lockObject = typeof(WebServiceContext);
            _nextRequestId = 0;
            _traceUsers = new Hashtable();
            _transactionInformation = new Hashtable();
        }

        /// <summary>
        /// Create a WebServiceContext instance.
        /// </summary>
        /// <param name='clientInformation'>Client information.</param>
        public WebServiceContext(WebClientInformation clientInformation)
        {
            // Init context.
            _database = null;
            _clientToken = new WebClientToken(clientInformation.Token,
                                              WebServiceData.WebServiceManager.Key);
            _locale = clientInformation.Locale;
            if (_locale.IsNull())
            {
                _locale = GetDefaultLocale();
            }
            _currentRole = clientInformation.Role;
            _requestId = GetNextRequestId();
            if (_isTracing)
            {
                _traceStart = DateTime.Now;
            }
            _transactionTimeout = Settings.Default.TransactionDefaultTimeout; // Unit is seconds.

            // Check arguments.
            try
            {
                clientInformation.CheckNotNull("clientInformation");
                CheckUser();
                CheckClientIpAddress();
                CheckHttpsProtocol();
                CheckWebServiceName();
                CheckCurrentRole();
            }
            catch (Exception exception)
            {
                WebServiceData.LogManager.LogSecurityError(this, exception);
                throw;
            }
        }

        /// <summary>
        /// Create a WebServiceContext instance.
        /// This contructor should only be used during login.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="applicationIdentifier">
        /// Identifier of the application that the user uses.
        /// </param>
        public WebServiceContext(String userName,
                                 String applicationIdentifier)
        {
            // Init object.
            _database = null;
            _clientToken = new WebClientToken(userName,
                                              applicationIdentifier,
                                              WebServiceData.WebServiceManager.Key);
            _requestId = GetNextRequestId();
            if (_isTracing)
            {
                _traceStart = DateTime.Now;
            }
            _transactionTimeout = Settings.Default.TransactionDefaultTimeout; // Unit is seconds.

            // Check arguments.
            try
            {
                CheckHttpsProtocol();
            }
            catch (Exception exception)
            {
                WebServiceData.LogManager.LogSecurityError(this, exception);
                throw;
            }
            _currentRole = null;

            // This is only a temporary value.
            // Real value is set by UserManager when user has logged in.
            _locale = GetDefaultLocale();
        }

        /// <summary>
        /// Get token for current client.
        /// </summary>
        public WebClientToken ClientToken
        {
            get { return _clientToken; }
        }

        /// <summary>
        /// Get role selected by user.
        /// If null, the user has not specified any current role.
        /// </summary>
        public WebRole CurrentRole
        {
            get { return _currentRole; }
        }

        /// <summary>
        /// Get user roles.
        /// If property CurrentRole is set then CurrentRoles returns
        /// CurrentRole otherwise CurrentRoles returnes all roles
        /// that the user has for the currently used application.
        /// </summary>
        public List<WebRole> CurrentRoles
        {
            get
            {
                List<WebRole> currentRoles;

                if (CurrentRole.IsNull())
                {
                    currentRoles = GetRoles();
                }
                else
                {
                    currentRoles = new List<WebRole>();
                    currentRoles.Add(CurrentRole);
                }
                return currentRoles;
            }
        }

        /// <summary>
        /// Handle token for current client.
        /// </summary>
        public WebLocale Locale
        {
            get { return _locale; }
            set { _locale = value; }
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
        private void CheckClientIpAddress()
        {
            if (GetClientIpAddress() != _clientToken.ClientIpAddress)
            {
                throw new ArgumentException("Invalid client IP address. " +
                                            "Current IP adress = " + GetClientIpAddress() + ". " +
                                            "ClientToken IP adress = " + _clientToken.ClientIpAddress + ".");
            }
        }

        /// <summary>
        /// If current role has been specified check that
        /// user has this role in the specified context.
        /// Authorities for specified role is replaced
        /// with information from UserService for
        /// security reasons.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if user does not have the specified role in this context.</exception>
        private void CheckCurrentRole()
        {
            WebRole verifiedRole;

            if (CurrentRole.IsNotNull())
            {
                // Don't trust client information. Verify role information
                // and get authorities from UserService.
                verifiedRole = null;
                foreach (WebRole role in WebServiceData.UserManager.GetRoles(this))
                {
                    if (role.Id == CurrentRole.Id)
                    {
                        verifiedRole = role;
                        break;
                    }
                }
                if (verifiedRole.IsNull())
                {
                    // User does not have specified role.
                    throw new ArgumentException("User " + GetUser().UserName + " is not in role name:" + CurrentRole.Name + " id:" + CurrentRole.Id);
                }
                else
                {
                    _currentRole = verifiedRole;
                }
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
                if (!IsInTransaction())
                {
                    throw new ApplicationException("Required transaction is missing!");
                }
            }
        }

        /// <summary>
        /// Check if transaction has timed out and should be rollbacked.
        /// </summary>
        private void CheckTransactionTimeOut()
        {
            WebServiceDataServer transactionDatabase = null;
            Int32 transactionId;
            Int32 transactionTimeout;

            // Get transaction information.
            transactionTimeout = _transactionTimeout;
            if (transactionTimeout > Settings.Default.TransactionMaxTimeout)
            {
                // Unit is seconds.
                transactionTimeout = Settings.Default.TransactionMaxTimeout;
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
                        if (IsInTransaction())
                        {
                            // Undo transaction.
                            if (_transactionInformation.ContainsKey(TransactionKey))
                            {
                                transactionDatabase = (WebServiceDataServer)(_transactionInformation[TransactionKey]);
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
        /// Check that the token belongs to this web service.
        /// </summary>
        private void CheckWebServiceName()
        {
            if (ClientToken.WebServiceName != WebServiceData.WebServiceManager.Name)
            {
                throw new ApplicationException("Token belongs to web service:" +
                                               ClientToken.WebServiceName +
                                               " but call was made to web service:" +
                                               WebServiceData.WebServiceManager.Name);
            }
        }

        /// <summary>
        /// Clear ASP.NET cache in web service.
        /// </summary>
        private void ClearAspDotNetCache()
        {
            IDictionaryEnumerator cacheEnum;
            List<String> cacheKeys;

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

                WebServiceData.LogManager.Log(this, "ASP.NET cache has been emptied.", LogType.Information, null);
            }
        }

        /// <summary>
        /// Clear data cache in web service.
        /// </summary>
        /// <param name='checkAccessRight'>
        /// Indicates if user access rights should be checked or not.
        /// Requests to clear cache that are made from Internet should always
        /// check access rights.
        /// </param>
        public virtual void ClearCache(Boolean checkAccessRight = true)
        {
            // Check access rights.
            if (checkAccessRight)
            {
                WebServiceData.AuthorizationManager.CheckAuthorization(this, AuthorityIdentifier.WebServiceAdministrator);
            }

            // Clear cached data.
            ClearAspDotNetCache();
            WebServiceDataServer.ClearCache();
        }

        /// <summary>
        /// Commit a transaction.
        /// </summary>
        public void CommitTransaction()
        {
            WebServiceDataServer transactionDatabase;

            lock (_transactionInformation)
            {
                if (IsInTransaction())
                {
                    transactionDatabase = (WebServiceDataServer)(_transactionInformation[TransactionKey]);
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

            // Fire commit transaction event.
            if (CommitTransactionEvent.IsNotNull())
            {
                CommitTransactionEvent(this);
            }
        }

        /// <summary>
        /// Implementation of the IDisposable interface.
        /// Releases all resources related to this request.
        /// </summary>
        public void Dispose()
        {
            Int32 index;
            String method;
            TimeSpan duration;

            if (_isTracing)
            {
                if (_traceUsers.IsEmpty() ||
                    _traceUsers.ContainsKey(ClientToken.UserName))
                {
                    duration = DateTime.Now - _traceStart;
                    method = Environment.StackTrace;
                    index = method.IndexOf(":line"); // English.
                    if (index < 0)
                    {
                        index = method.IndexOf(":rad"); // Swedish.
                    }

                    if (0 <= index)
                    {
                        method = method.Substring(index);
                        method = method.Substring(method.IndexOf("."));
                        method = method.Substring(1, method.IndexOf("(") - 1) + "()";
                    }
                    WebServiceData.LogManager.Log(this, method + ", duration = " + duration.Milliseconds + " milliseconds.", LogType.Trace, null);
                }
            }

            if (_database.IsNotNull())
            {
                try
                {
                    _database.Disconnect();
                }
                catch
                {
                }
                _database = null;
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

        /// <summary>
        /// Get web service client IP address.
        /// </summary>
        /// <returns>IP address.</returns>
        public static String GetClientIpAddress()
        {
            String clientIpAddress;

            if (HttpContext.Current.IsNull() ||
                HttpContext.Current.Request.IsNull() ||
                HttpContext.Current.Request.UserHostAddress.IsEmpty())
            {
                // Local ip address is used during test.
                clientIpAddress = Settings.Default.LocalIpAddress;
            }
            else
            {
                if (Configuration.InstallationType == InstallationType.Production)
                {
                    clientIpAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (clientIpAddress.IsEmpty() ||
                        (clientIpAddress.ToLower() == "unknown"))
                    {
                        // This occurs for web services that have a machine dependent web address. 
                        clientIpAddress = HttpContext.Current.Request.UserHostAddress;
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

        /// <summary>
        /// Get default locale.
        /// </summary>
        /// <returns>Default locale.</returns>
        public WebLocale GetDefaultLocale()
        {
            WebLocale defaultLocale;

            defaultLocale = new WebLocale();
            defaultLocale.Id = Settings.Default.DefaultLocaleId;
            defaultLocale.ISOCode = Settings.Default.DefaultLocaleISOCode;
            return defaultLocale;
        }

        /// <summary>
        /// Get database instance.
        /// </summary>
        /// <returns>Requested database instance.</returns>
        public WebServiceDataServer GetDatabase()
        {
            if (IsInTransaction())
            {
                return GetTransactionDatabase();
            }

            if (_database.IsNull())
            {
                _database = WebServiceData.DatabaseManager.GetDatabase(this);
            }

            return _database;
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
        public WebServiceDataServer GetTransactionDatabase()
        {
            WebServiceDataServer database = null;

            lock (_transactionInformation)
            {
                if (_transactionInformation.Contains(TransactionKey))
                {
                    database = (WebServiceDataServer)(_transactionInformation[TransactionKey]);
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
        public WebUser GetUser()
        {
            return WebServiceData.UserManager.GetUser(this);
        }

        /// <summary>
        /// Test if this user is in a transaction.
        /// </summary>
        /// <returns>
        /// True, if this user is in a transaction.
        /// </returns>
        public virtual Boolean IsInTransaction()
        {
            return _transactionInformation.Contains(TransactionKey);
        }

        /// <summary>
        /// Remove object from the ASP.NET cache.
        /// </summary>
        /// <param name='cacheKey'>Key used when accessing this object.</param>
        public virtual void RemoveCachedObject(String cacheKey)
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
            WebServiceDataServer transactionDatabase = null;

            lock (_transactionInformation)
            {
                // Undo transaction.
                if (_transactionInformation.ContainsKey(TransactionKey))
                {
                    transactionDatabase = (WebServiceDataServer)(_transactionInformation[TransactionKey]);
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

                // Cache is cleared since data changes that
                // is rollbacked could have been cached.
                ClearCache(false);
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
            WebServiceData.AuthorizationManager.CheckAuthorization(this, AuthorityIdentifier.WebServiceAdministrator);

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
            WebServiceDataServer transactionDatabase = null;
            Thread transactionThread;

            _transactionTimeout = timeout;
            lock (_transactionInformation)
            {
                if (IsInTransaction())
                {
                    // StartTransaction should only be called once
                    // to start the transaction.
                    abortTransaction = true;

                    // Undo transaction.
                    if (_transactionInformation.ContainsKey(TransactionKey))
                    {
                        transactionDatabase = (WebServiceDataServer)(_transactionInformation[TransactionKey]);
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
                    transactionDatabase = WebServiceData.DatabaseManager.GetDatabase(this);
                    transactionDatabase.CommandTimeout = _transactionTimeout;
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
            transactionThread = new Thread(CheckTransactionTimeOut);
            transactionThread.Priority = ThreadPriority.Highest;
            transactionThread.Start();
        }

        /// <summary>
        /// Stop tracing usage of web service.
        /// </summary>
        public void StopTrace()
        {
            // Check access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(this, AuthorityIdentifier.WebServiceAdministrator);

            _isTracing = false;
            _traceUsers.Clear();
        }
    }
}
