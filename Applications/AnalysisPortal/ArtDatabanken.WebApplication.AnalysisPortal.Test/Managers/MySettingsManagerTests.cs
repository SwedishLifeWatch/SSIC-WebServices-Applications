using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media.Animation;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings;
using ArtDatabanken.WebApplication.AnalysisPortal.Test.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Test.Managers
{
    using System.IO;
    using System.IO.Fakes;
    using System.Web.Hosting.Fakes;

    using Microsoft.QualityTools.Testing.Fakes;

    //todo Kunna välja att MySettings-filerna ska zippas.
    
    /// <summary>
    ///This is a test class for MySettingsManagerTest and is intended
    ///to contain all MySettingsManagerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MySettingsManagerTests
    {

        private static IUserContext GetTestUserContext()
        {
            IUserContext userContext;
            userContext = new UserContext();
            userContext.User = new User(userContext) { UserName = "testuser" };
            return userContext;
        }

        private static AnalysisPortal.MySettings.MySettings CreateMySettingsObjectWithTaxonFilter(params int[] taxonIds)
        {
            AnalysisPortal.MySettings.MySettings mySettings;
            mySettings = new AnalysisPortal.MySettings.MySettings();
            mySettings.Filter.Taxa.TaxonIds = new ObservableCollection<int>(taxonIds);
            return mySettings;
        }


        [TestMethod]
        // This test expects that it can write to "C:\Temp\myTempFile.txt"    
        public void SaveAndGetLastUsedSettings_CreateMySettingsWithTaxa_LastSettingsSavedToFileAndReturnedOnLoad()
        {
            using (ShimsContext.Create())
            {
                ShimFilePath();

                IUserContext userContext;
                AnalysisPortal.MySettings.MySettings mySettings;
                AnalysisPortal.MySettings.MySettings loadedSettings;

                // Arrange
                userContext = GetTestUserContext();                
                mySettings = CreateMySettingsObjectWithTaxonFilter(1,2,5);

                // Act
                MySettingsManager.SaveLastSettings(userContext, mySettings);                
                loadedSettings = MySettingsManager.LoadLastSettings(userContext);
                
                // Assert
                CollectionAssert.AreEqual(new List<int> { 1, 2, 5 }, loadedSettings.Filter.Taxa.TaxonIds);
            }
        }        

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void DoesLastSettingsExist_SettingsDontExist_ReturnFalse()
        {
            using (ShimsContext.Create())
            {
                ShimFilePath();
                IUserContext userContext;
                Boolean doesLastSettingsExist;

                // Arrange
                userContext = GetTestUserContext();                
                
                // Act
                MySettingsManager.DeleteLastSettingsFile(userContext);
                doesLastSettingsExist = MySettingsManager.DoesLastSettingsExist(userContext);

                // Assert
                Assert.IsFalse(doesLastSettingsExist);
            }
        }

        [TestMethod]
        // This test expects that it can write to "C:\Temp\myTempFile.txt".
        public void DoesLastSettingsExist_SettingsExists_ReturnTrue()
        {
            using (ShimsContext.Create())
            {
                ShimFilePath();
                IUserContext userContext;
                Boolean doesLastSettingsExist;
                AnalysisPortal.MySettings.MySettings mySettings;

                // Arrange
                userContext = GetTestUserContext();
                mySettings = CreateMySettingsObjectWithTaxonFilter(1, 2, 5);

                // Act
                MySettingsManager.SaveLastSettings(userContext, mySettings);
                doesLastSettingsExist = MySettingsManager.DoesLastSettingsExist(userContext);

                // Assert
                Assert.IsTrue(doesLastSettingsExist);
            }
        }

        [TestMethod]
        // This test expects that it can write to "C:\Temp\myTempFile.txt" 
        public void SaveLastSettings_SaveTwoTimes_LastSavedSettingsIsOverwritten()
        {
            using (ShimsContext.Create())
            {
                ShimFilePath();
                IUserContext userContext;
                AnalysisPortal.MySettings.MySettings mySettings;
                AnalysisPortal.MySettings.MySettings loadedSettings;

                // Arrange
                userContext = GetTestUserContext();
                mySettings = CreateMySettingsObjectWithTaxonFilter(1, 2, 5);

                // Act
                MySettingsManager.SaveLastSettings(userContext, mySettings);
                loadedSettings = MySettingsManager.LoadLastSettings(userContext);

                // Assert
                CollectionAssert.AreEqual(new List<int> { 1, 2, 5 }, loadedSettings.Filter.Taxa.TaxonIds);


                // Arrange                
                mySettings = CreateMySettingsObjectWithTaxonFilter(10,12,22);

                // Act
                MySettingsManager.SaveLastSettings(userContext, mySettings);
                loadedSettings = MySettingsManager.LoadLastSettings(userContext);

                // Assert
                CollectionAssert.AreEqual(new List<int> { 10, 12, 22 }, loadedSettings.Filter.Taxa.TaxonIds);

            }            
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void GetLastSettingsSaveTime_LastSettingsDontExists_ReturnNull()
        {
            using (ShimsContext.Create())
            {
                ShimFilePath();
                IUserContext userContext;
                DateTime? lastSettingsSaveTime;

                // Arrange
                userContext = GetTestUserContext();

                // Act
                MySettingsManager.DeleteLastSettingsFile(userContext);
                lastSettingsSaveTime = MySettingsManager.GetLastSettingsSaveTime(userContext);

                // Assert
                Assert.IsNull(lastSettingsSaveTime);
            }
        }

        [TestMethod]
        // This test expects that it can write to "C:\Temp\myTempFile.txt"    
        public void GetLastSettingsSaveTime_LastSettingsExists_ReturnDateTimeNow()
        {
            using (ShimsContext.Create())
            {
                ShimFilePath();
                IUserContext userContext;
                AnalysisPortal.MySettings.MySettings mySettings;
                DateTime? lastSettingsSaveTime;

                // Arrange
                userContext = GetTestUserContext();
                mySettings = CreateMySettingsObjectWithTaxonFilter(1, 2, 5);

                // Act
                MySettingsManager.SaveLastSettings(userContext,mySettings);
                lastSettingsSaveTime = MySettingsManager.GetLastSettingsSaveTime(userContext);                
                TimeSpan timeSpan = DateTime.Now - lastSettingsSaveTime.Value;

                // Assert
                Assert.IsTrue(timeSpan.TotalMilliseconds < 1000);
            }
        }


        [TestMethod()]
        [TestCategory("UnitTestApp")]
        public void SerializeAndDeserializeMySettings()
        {
            using (ShimsContext.Create())
            {
                ShimFilePath();
                AnalysisPortal.MySettings.MySettings mySettings;
                AnalysisPortal.MySettings.MySettings loadedSettings;
                const string settingsName = "test";
                
                // Arrange
                mySettings = CreateMySettingsObjectWithTaxonFilter(1, 2, 5);

                // Act
                MySettingsManager.SaveToDisk(settingsName, mySettings);                
                loadedSettings = MySettingsManager.LoadFromDisk(settingsName);

                // Assert
                CollectionAssert.AreEqual(new[] {1,2,5}, loadedSettings.Filter.Taxa.TaxonIds);
            }
        }

        [TestMethod()]
        // This test expects that it can write to "C:\Temp\myTempFile.txt"   
        public void SerializeAndDeserializeMySettingsWithUserContext()
        {
            using (ShimsContext.Create())
            {
                ShimFilePath();
                AnalysisPortal.MySettings.MySettings mySettings;
                AnalysisPortal.MySettings.MySettings loadedMySettings;
                IUserContext userContext;
                const string settingsName = "test";

                // Arrange
                userContext = GetTestUserContext();                
                mySettings = CreateMySettingsObjectWithTaxonFilter(1, 2, 5);
                
                // Act
                MySettingsManager.SaveToDisk(userContext, settingsName, mySettings);
                loadedMySettings = MySettingsManager.LoadFromDisk(userContext, settingsName);

                // Assert
                Assert.IsNotNull(loadedMySettings);
                CollectionAssert.AreEqual(new[] { 1, 2, 5 }, loadedMySettings.Filter.Taxa.TaxonIds);                                
            }
        }

        [TestMethod()]
        [TestCategory("UnitTestApp")]
        public void Test_ResultCacheNeedsRefresh()
        {
            AnalysisPortal.MySettings.MySettings mySettings = new ArtDatabanken.WebApplication.AnalysisPortal.MySettings.MySettings();
            mySettings.ResultCacheNeedsRefresh = true;
            Assert.IsTrue(mySettings.ResultCacheNeedsRefresh);
            Assert.IsTrue(mySettings.Filter.ResultCacheNeedsRefresh);
            Assert.IsTrue(mySettings.Filter.Taxa.ResultCacheNeedsRefresh);
            
            mySettings.ResultCacheNeedsRefresh = false;
            Assert.IsFalse(mySettings.ResultCacheNeedsRefresh);
            Assert.IsFalse(mySettings.Filter.ResultCacheNeedsRefresh);
            Assert.IsFalse(mySettings.Filter.Taxa.ResultCacheNeedsRefresh);

            mySettings.Filter.Taxa.TaxonIds.Add(25);
            Assert.IsTrue(mySettings.ResultCacheNeedsRefresh);
            Assert.IsTrue(mySettings.Filter.ResultCacheNeedsRefresh);
            Assert.IsTrue(mySettings.Filter.Taxa.ResultCacheNeedsRefresh);

            mySettings.ResultCacheNeedsRefresh = false;
            Assert.IsFalse(mySettings.ResultCacheNeedsRefresh);
            Assert.IsFalse(mySettings.Filter.ResultCacheNeedsRefresh);
            Assert.IsFalse(mySettings.Filter.Taxa.ResultCacheNeedsRefresh);

        }

        public static void ShimFilePath()
        {
             string fullPath = @"C:\Temp\myTempFile.txt";

            ShimFile.CreateString = (filePath) => { return new FileStream(fullPath, FileMode.Create); };
            ShimHostingEnvironment.MapPathString = (path) => { return fullPath; };
        }

    }
}
