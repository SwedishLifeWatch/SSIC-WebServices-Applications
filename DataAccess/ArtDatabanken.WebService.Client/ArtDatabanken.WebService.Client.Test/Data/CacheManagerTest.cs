using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class CacheManagerTest : TestBase
    {
        Boolean _isRefreshCacheFired;

        public CacheManagerTest()
        {
            _isRefreshCacheFired = false;
        }

        [TestMethod]
        public void FireRefreshCache()
        {
            _isRefreshCacheFired = false;
            CacheManager.FireRefreshCache(GetUserContext());
            Assert.IsFalse(_isRefreshCacheFired);

            _isRefreshCacheFired = false;
            CacheManager.RefreshCacheEvent += RefreshCache;
            CacheManager.FireRefreshCache(GetUserContext());
            Assert.IsTrue(_isRefreshCacheFired);

            _isRefreshCacheFired = false;
            CacheManager.RefreshCacheEvent -= RefreshCache;
            CacheManager.FireRefreshCache(GetUserContext());
            Assert.IsFalse(_isRefreshCacheFired);
        }

        /// <summary>
        /// Refresh cached data.
        /// </summary>
        protected virtual void RefreshCache(IUserContext userContext)
        {
            _isRefreshCacheFired = true;
        }
    }
}
