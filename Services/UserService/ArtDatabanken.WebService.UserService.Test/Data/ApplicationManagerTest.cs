using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.UserService.Test.Data
{
    [TestClass]
    public class ApplicationManagerTest : TestBase
    {
        public ApplicationManagerTest()
            : base(true, 50)
        {
        }

        [TestMethod]
        public void AddAuthorityDataTypeToApplication()
        {
            Int32 authorityDataTypeId;
            Int32 applicationId;

            UseTransaction = true;
            authorityDataTypeId = 1;
            applicationId = 1;
            UserService.Data.ApplicationManager.AddAuthorityDataTypeToApplication(GetContext(), authorityDataTypeId, applicationId);
        }

        [TestMethod]
        public void CreateApplication()
        {
            WebApplication application;

            // Get existing application.
            UseTransaction = true;
            application = UserService.Data.ApplicationManager.GetApplicationById(GetContext(), Settings.Default.TestApplicationId);
            application.Name = application.Name + "123";
            application.ShortName = application.ShortName + "123";
            application.ContactPersonId = 1;
            WebApplication newApplication;
            newApplication = UserService.Data.ApplicationManager.CreateApplication(GetContext(), application);
            Assert.IsNotNull(newApplication);
            Assert.AreEqual(application.Name, newApplication.Name);
            Assert.IsTrue(newApplication.Id > Settings.Default.TestApplicationId);
            Assert.IsNotNull(newApplication.Description);
            Assert.AreEqual(newApplication.ShortName, application.ShortName);
            Assert.AreEqual(newApplication.ContactPersonId, 1);
        }

        [TestMethod]
        public void CreateApplicationAction()
        {
            WebApplicationAction applicationAction;
            WebApplicationAction newApplicationAction;

            // Get existing application.
            UseTransaction = true;
            applicationAction = UserService.Data.ApplicationManager.GetApplicationAction(GetContext(), 3);
            applicationAction.Name = applicationAction.Name + "123";
            newApplicationAction = UserService.Data.ApplicationManager.CreateApplicationAction(GetContext(), applicationAction);
            Assert.IsNotNull(newApplicationAction);
            Assert.AreEqual(newApplicationAction.Identifier, applicationAction.Identifier);
            Assert.IsTrue(newApplicationAction.Id > 3);
            Assert.IsFalse(newApplicationAction.IsAdministrationRoleIdSpecified);
            Assert.IsNotNull(newApplicationAction.Description);
        }

        [TestMethod]
        public void CreateApplicationVersion()
        {
            WebApplicationVersion applicationVersion;

            // Get existing application.
            UseTransaction = true;
            applicationVersion = UserService.Data.ApplicationManager.GetApplicationVersion(GetContext(), 1);
            WebApplicationVersion newApplicationVersion;
            newApplicationVersion = UserService.Data.ApplicationManager.CreateApplicationVersion(GetContext(), applicationVersion);
            Assert.IsNotNull(newApplicationVersion);
            Assert.AreEqual(newApplicationVersion.Version, applicationVersion.Version);
            Assert.IsNotNull(newApplicationVersion.Description);
            Assert.AreEqual(newApplicationVersion.IsRecommended, applicationVersion.IsRecommended);
            Assert.AreEqual(newApplicationVersion.IsValid, applicationVersion.IsValid);
        }       
        
        [TestMethod]
        public void DeleteApplication()
        {
            WebApplication application, applicationToDelete;

            UseTransaction = true;
            application = GetNewApplication();
            applicationToDelete = UserService.Data.ApplicationManager.CreateApplication(GetContext(), application);
            UserService.Data.ApplicationManager.DeleteApplication(GetContext(), applicationToDelete);
        }
     
        [TestMethod]
        public void GetApplicationById()
        {
            WebApplication application;

            // Get existing application.
            UseTransaction = true;
            application = UserService.Data.ApplicationManager.GetApplicationById(GetContext(), Settings.Default.TestApplicationId);
            Assert.IsNotNull(application);
            Assert.AreEqual(Settings.Default.TestApplicationName, application.Name);
            Assert.AreEqual(Settings.Default.TestApplicationId, application.Id);
            Assert.AreEqual("UserAdmin", application.ShortName);
            Assert.IsNotNull(application.Description);
            Assert.IsNotNull(application.GUID);
            Assert.IsNotNull(application.CreatedBy);
            Assert.IsNotNull(application.CreatedDate);
            Assert.IsNotNull(application.ModifiedBy);
            Assert.IsNotNull(application.ModifiedDate);
            Assert.IsNotNull(application.ValidFromDate);
            Assert.IsNotNull(application.ValidToDate);
            Assert.AreEqual(application.Identifier, Settings.Default.TestApplicationIdentifier);

            UseTransaction = false;
            application = UserService.Data.ApplicationManager.GetApplicationById(GetContext(), Settings.Default.TestApplicationId);
            Assert.IsNotNull(application);
            Assert.AreEqual(Settings.Default.TestApplicationName, application.Name);
            Assert.AreEqual(Settings.Default.TestApplicationId, application.Id);
            Assert.AreEqual("UserAdmin", application.ShortName);
            Assert.IsNotNull(application.Description);
            Assert.IsNotNull(application.GUID);
            Assert.IsNotNull(application.CreatedBy);
            Assert.IsNotNull(application.CreatedDate);
            Assert.IsNotNull(application.ModifiedBy);
            Assert.IsNotNull(application.ModifiedDate);
            Assert.IsNotNull(application.ValidFromDate);
            Assert.IsNotNull(application.ValidToDate);
            Assert.AreEqual(application.Identifier, Settings.Default.TestApplicationIdentifier);
        }

        [TestMethod]
        public void GetApplicationByIdentifier()
        {
            WebApplication application;

            // Get existing application.
            UseTransaction = true;
            application = UserService.Data.ApplicationManager.GetApplicationByIdentifier(GetContext(), ApplicationIdentifier.UserService.ToString());
            Assert.IsNotNull(application);
            Assert.AreEqual(ApplicationIdentifier.UserService.ToString(), application.ShortName);
            Assert.IsNotNull(application.Description);
            Assert.IsNotNull(application.GUID);
            Assert.IsNotNull(application.CreatedBy);
            Assert.IsNotNull(application.CreatedDate);
            Assert.IsNotNull(application.ModifiedBy);
            Assert.IsNotNull(application.ModifiedDate);
            Assert.IsNotNull(application.ValidFromDate);
            Assert.IsNotNull(application.ValidToDate);
            Assert.AreEqual(application.Identifier, ApplicationIdentifier.UserService.ToString());

            UseTransaction = false;
            application = UserService.Data.ApplicationManager.GetApplicationByIdentifier(GetContext(), ApplicationIdentifier.UserService.ToString());
            Assert.IsNotNull(application);
            Assert.AreEqual(ApplicationIdentifier.UserService.ToString(), application.ShortName);
            Assert.IsNotNull(application.Description);
            Assert.IsNotNull(application.GUID);
            Assert.IsNotNull(application.CreatedBy);
            Assert.IsNotNull(application.CreatedDate);
            Assert.IsNotNull(application.ModifiedBy);
            Assert.IsNotNull(application.ModifiedDate);
            Assert.IsNotNull(application.ValidFromDate);
            Assert.IsNotNull(application.ValidToDate);
            Assert.AreEqual(application.Identifier, ApplicationIdentifier.UserService.ToString());
        }

        [TestMethod]
        public void GetApplicationAction()
        {
            WebApplicationAction applicationAction;

            UseTransaction = true;
            applicationAction = UserService.Data.ApplicationManager.GetApplicationAction(GetContext(), 1);
            Assert.IsNotNull(applicationAction);
            Assert.IsTrue(applicationAction.Description.Length > 10);
            Assert.IsNotNull(applicationAction.ValidFromDate);
            Assert.IsNotNull(applicationAction.ValidToDate);

            UseTransaction = false;
            applicationAction = UserService.Data.ApplicationManager.GetApplicationAction(GetContext(), 1);
            Assert.IsNotNull(applicationAction);
            Assert.IsTrue(applicationAction.Description.Length > 10);
            Assert.IsNotNull(applicationAction.ValidFromDate);
            Assert.IsNotNull(applicationAction.ValidToDate);
        }

        [TestMethod]
        public void GetApplicationActions()
        {
            List<WebApplicationAction> applicationActions;

            UseTransaction = true;
            applicationActions = UserService.Data.ApplicationManager.GetApplicationActionsByApplicationId(GetContext(), Settings.Default.TestApplicationId);
            Assert.IsNotNull(applicationActions);
            Assert.IsTrue(applicationActions.Count > 0);

            UseTransaction = false;
            applicationActions = UserService.Data.ApplicationManager.GetApplicationActionsByApplicationId(GetContext(), Settings.Default.TestApplicationId);
            Assert.IsNotNull(applicationActions);
            Assert.IsTrue(applicationActions.Count > 0);
        }

        [TestMethod]
        public void GetApplicationActionsByGuids()
        {
            List<WebApplicationAction> applicationActions;
            List<String> applicationActionGuids;

            UseTransaction = true;
            applicationActionGuids = new List<String>();
            applicationActionGuids.Add("3");
            applicationActionGuids.Add("4");
            applicationActions = UserService.Data.ApplicationManager.GetApplicationActionsByGuids(GetContext(), applicationActionGuids);
            Assert.IsNotNull(applicationActions);
            Assert.AreEqual(2, applicationActions.Count);
            Assert.IsNotNull(applicationActions[0].Description);
            Assert.IsNotNull(applicationActions[0].Identifier);
            Assert.IsNotNull(applicationActions[1].Description);

            UseTransaction = false;
            applicationActionGuids = new List<String>();
            applicationActionGuids.Add("3");
            applicationActionGuids.Add("4");
            applicationActions = UserService.Data.ApplicationManager.GetApplicationActionsByGuids(GetContext(), applicationActionGuids);
            Assert.IsNotNull(applicationActions);
            Assert.AreEqual(2, applicationActions.Count);
            Assert.IsNotNull(applicationActions[0].Description);
            Assert.IsNotNull(applicationActions[0].Identifier);
            Assert.IsNotNull(applicationActions[1].Description);
        }

        [TestMethod]
        public void GetApplicationActionsByIds()
        {
            List<WebApplicationAction> applicationActions;
            List<Int32> applicationActionIdList;

            UseTransaction = true;
            applicationActionIdList = new List<Int32>();
            applicationActionIdList.Add(3);
            applicationActionIdList.Add(4);
            applicationActions = UserService.Data.ApplicationManager.GetApplicationActionsByIds(GetContext(), applicationActionIdList);
            Assert.IsNotNull(applicationActions);
            Assert.AreEqual(2, applicationActions.Count);
            Assert.IsNotNull(applicationActions[0].Description);
            Assert.IsNotNull(applicationActions[0].Identifier);
            Assert.IsNotNull(applicationActions[1].Description);

            UseTransaction = false;
            applicationActionIdList = new List<Int32>();
            applicationActionIdList.Add(3);
            applicationActionIdList.Add(4);
            applicationActions = UserService.Data.ApplicationManager.GetApplicationActionsByIds(GetContext(), applicationActionIdList);
            Assert.IsNotNull(applicationActions);
            Assert.AreEqual(2, applicationActions.Count);
            Assert.IsNotNull(applicationActions[0].Description);
            Assert.IsNotNull(applicationActions[0].Identifier);
            Assert.IsNotNull(applicationActions[1].Description);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetApplicationNonExistingIdErrorTransaction()
        {
            WebApplication application;
            // Set testdata
            const Int32 applicationId = -1;

            // Try to get non-existing application.
            UseTransaction = true;
            application = UserService.Data.ApplicationManager.GetApplicationById(GetContext(), applicationId);
            Assert.IsNull(application);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetApplicationNonExistingIdErrorNoTransaction()
        {
            WebApplication application;
            // Set testdata
            const Int32 applicationId = -1;

            // Try to get non-existing application.
            UseTransaction = false;
            application = UserService.Data.ApplicationManager.GetApplicationById(GetContext(), applicationId);
            Assert.IsNull(application);
        }

        [TestMethod]
        public void GetApplications()
        {
            List<WebApplication> applications;

            UseTransaction = true;
            applications = UserService.Data.ApplicationManager.GetApplications(GetContext());
            Assert.IsNotNull(applications);
            Assert.AreEqual(1, applications[0].Id);
            Assert.AreEqual(2, applications[1].Id);
            Assert.AreEqual(3, applications[2].Id);

            UseTransaction = false;
            applications = UserService.Data.ApplicationManager.GetApplications(GetContext());
            Assert.IsNotNull(applications);
            Assert.AreEqual(1, applications[0].Id);
            Assert.AreEqual(2, applications[1].Id);
            Assert.AreEqual(3, applications[2].Id);
        }

        [TestMethod]
        public void GetApplicationsInSoa()
        {
            List<WebApplication> applications;

            UseTransaction = true;
            applications = UserService.Data.ApplicationManager.GetApplicationsInSoa(GetContext());
            Assert.IsTrue(applications.IsNotEmpty());
            Assert.AreEqual(11, applications.Count);
            foreach (WebApplication application in applications)
            {
                Assert.IsTrue(((application.Identifier == ApplicationIdentifier.AnalysisService.ToString()) ||
                               (application.Identifier == ApplicationIdentifier.ArtDatabankenService.ToString()) ||
                               (application.Identifier == ApplicationIdentifier.GeoReferenceService.ToString()) ||
                               (application.Identifier == ApplicationIdentifier.PictureService.ToString()) ||
                               (application.Identifier == ApplicationIdentifier.ReferenceService.ToString()) ||
                               (application.Identifier == ApplicationIdentifier.SpeciesObservationHarvestService.ToString()) ||
                               (application.Identifier == ApplicationIdentifier.SwedishSpeciesObservationService.ToString()) ||
                               (application.Identifier == ApplicationIdentifier.SwedishSpeciesObservationSOAPService.ToString()) ||
                               (application.Identifier == ApplicationIdentifier.TaxonService.ToString()) ||
                               (application.Identifier == ApplicationIdentifier.TaxonAttributeService.ToString()) ||
                               (application.Identifier == ApplicationIdentifier.UserService.ToString())));
            }

            UseTransaction = false;
            applications = UserService.Data.ApplicationManager.GetApplicationsInSoa(GetContext());
            Assert.IsTrue(applications.IsNotEmpty());
            Assert.AreEqual(11, applications.Count);
            foreach (WebApplication application in applications)
            {
                Assert.IsTrue(((application.Identifier == ApplicationIdentifier.AnalysisService.ToString()) ||
                               (application.Identifier == ApplicationIdentifier.ArtDatabankenService.ToString()) ||
                               (application.Identifier == ApplicationIdentifier.GeoReferenceService.ToString()) ||
                               (application.Identifier == ApplicationIdentifier.PictureService.ToString()) ||
                               (application.Identifier == ApplicationIdentifier.ReferenceService.ToString()) ||
                               (application.Identifier == ApplicationIdentifier.SpeciesObservationHarvestService.ToString()) ||
                               (application.Identifier == ApplicationIdentifier.SwedishSpeciesObservationService.ToString()) ||
                               (application.Identifier == ApplicationIdentifier.SwedishSpeciesObservationSOAPService.ToString()) ||
                               (application.Identifier == ApplicationIdentifier.TaxonService.ToString()) ||
                               (application.Identifier == ApplicationIdentifier.TaxonAttributeService.ToString()) ||
                               (application.Identifier == ApplicationIdentifier.UserService.ToString())));
            }
        }

        [TestMethod]
        public void GetApplicationVersion()
        {
            WebApplicationVersion applicationVersion;

            UseTransaction = true;
            applicationVersion = UserService.Data.ApplicationManager.GetApplicationVersion(GetContext(), 1);
            Assert.IsNotNull(applicationVersion);
            Assert.AreEqual("1.0", applicationVersion.Version);
            Assert.IsTrue(applicationVersion.IsRecommended);
            Assert.IsTrue(applicationVersion.IsValid);
            Assert.IsTrue(applicationVersion.Description.Length > 5);
            Assert.IsNotNull(applicationVersion.ValidFromDate);
            Assert.IsNotNull(applicationVersion.ValidToDate);

            UseTransaction = false;
            applicationVersion = UserService.Data.ApplicationManager.GetApplicationVersion(GetContext(), 1);
            Assert.IsNotNull(applicationVersion);
            Assert.AreEqual("1.0", applicationVersion.Version);
            Assert.IsTrue(applicationVersion.IsRecommended);
            Assert.IsTrue(applicationVersion.IsValid);
            Assert.IsTrue(applicationVersion.Description.Length > 5);
            Assert.IsNotNull(applicationVersion.ValidFromDate);
            Assert.IsNotNull(applicationVersion.ValidToDate);
        }

        [TestMethod]
        public void GetApplicationVersions()
        {
            List<WebApplicationVersion> applicationVersions;

            UseTransaction = true;
            applicationVersions = UserService.Data.ApplicationManager.GetApplicationVersionsByApplicationId(GetContext(), Settings.Default.TestApplicationId);
            Assert.IsNotNull(applicationVersions);
            Assert.AreEqual("1.0", applicationVersions[0].Version);
            Assert.IsTrue(applicationVersions[0].IsRecommended);
            Assert.IsTrue(applicationVersions[0].IsValid);

            UseTransaction = false;
            applicationVersions = UserService.Data.ApplicationManager.GetApplicationVersionsByApplicationId(GetContext(), Settings.Default.TestApplicationId);
            Assert.IsNotNull(applicationVersions);
            Assert.AreEqual("1.0", applicationVersions[0].Version);
            Assert.IsTrue(applicationVersions[0].IsRecommended);
            Assert.IsTrue(applicationVersions[0].IsValid);
        }

        [TestMethod]
        public void GetAuthorityDataTypesByApplicationId()
        {
            List<WebAuthorityDataType> authorityDataTypes;

            UseTransaction = true;
            authorityDataTypes = UserService.Data.ApplicationManager.GetAuthorityDataTypesByApplicationId(GetContext(), Settings.Default.TestApplicationId);
            Assert.IsTrue(authorityDataTypes.IsNotEmpty());
            // In TestDB (moneses there are two AuthorityDataTypes assigned to TestApplication
            Assert.IsTrue(authorityDataTypes.Count == 2);

            UseTransaction = false;
            authorityDataTypes = UserService.Data.ApplicationManager.GetAuthorityDataTypesByApplicationId(GetContext(), Settings.Default.TestApplicationId);
            Assert.IsTrue(authorityDataTypes.IsNotEmpty());
            // In TestDB (moneses there are two AuthorityDataTypes assigned to TestApplication
            Assert.IsTrue(authorityDataTypes.Count == 2);
        }

        private WebApplication GetNewApplication()
        {
            WebApplication newApplication;
            newApplication = new WebApplication();
            newApplication.Identifier = @"NewApplication";
            newApplication.Name = @"NewApplication123";
            newApplication.ShortName = @"NewApplicationShort123";
            newApplication.Description = @"testdescription";
            return newApplication;
        }

        [TestMethod]
        public void IsApplicationVersionValid()
        {
            WebApplicationVersion applicationVersion;

            UseTransaction = true;

            // Valid version.
            applicationVersion = UserService.Data.ApplicationManager.IsApplicationVersionValid(GetContext(), Settings.Default.TestApplicationIdentifier, "1.0");
            Assert.IsNotNull(applicationVersion);
            Assert.IsTrue(applicationVersion.Version.Length > 1);
            Assert.IsTrue(applicationVersion.IsRecommended);
            Assert.IsTrue(applicationVersion.IsValid);
            Assert.IsTrue(applicationVersion.Description.Length > 20);
            Assert.IsNotNull(applicationVersion.ValidFromDate);
            Assert.IsNotNull(applicationVersion.ValidToDate);

            // Not valid version.
            applicationVersion = UserService.Data.ApplicationManager.IsApplicationVersionValid(GetContext(), Settings.Default.TestApplicationIdentifier, "Version 0.8");
            Assert.IsNotNull(applicationVersion);
            Assert.AreEqual("Version 0.8", applicationVersion.Version);
            Assert.IsFalse(applicationVersion.IsRecommended);
            Assert.IsFalse(applicationVersion.IsValid);
            Assert.IsTrue(applicationVersion.Description.Length < 2);
            Assert.IsNotNull(applicationVersion.ValidFromDate);
            Assert.IsNotNull(applicationVersion.ValidToDate);

            // None existing version.
            applicationVersion = UserService.Data.ApplicationManager.IsApplicationVersionValid(GetContext(), "NotAnApplicationIdenity", "NoneExistingVersion");
            Assert.IsNotNull(applicationVersion);
            Assert.AreEqual("NoneExistingVersion", applicationVersion.Version);
            Assert.IsFalse(applicationVersion.IsRecommended);
            Assert.IsFalse(applicationVersion.IsValid);
            Assert.IsTrue(applicationVersion.Description.Length < 2);
            Assert.IsNotNull(applicationVersion.ValidFromDate);
            Assert.IsNotNull(applicationVersion.ValidToDate);

            UseTransaction = false;

            // Valid version.
            applicationVersion = UserService.Data.ApplicationManager.IsApplicationVersionValid(GetContext(), Settings.Default.TestApplicationIdentifier, "1.0");
            Assert.IsNotNull(applicationVersion);
            Assert.IsTrue(applicationVersion.Version.Length > 1);
            Assert.IsTrue(applicationVersion.IsRecommended);
            Assert.IsTrue(applicationVersion.IsValid);
            Assert.IsTrue(applicationVersion.Description.Length > 20);
            Assert.IsNotNull(applicationVersion.ValidFromDate);
            Assert.IsNotNull(applicationVersion.ValidToDate);

            // Not valid version.
            applicationVersion = UserService.Data.ApplicationManager.IsApplicationVersionValid(GetContext(), Settings.Default.TestApplicationIdentifier, "Version 0.8");
            Assert.IsNotNull(applicationVersion);
            Assert.AreEqual("Version 0.8", applicationVersion.Version);
            Assert.IsFalse(applicationVersion.IsRecommended);
            Assert.IsFalse(applicationVersion.IsValid);
            Assert.IsTrue(applicationVersion.Description.Length < 2);
            Assert.IsNotNull(applicationVersion.ValidFromDate);
            Assert.IsNotNull(applicationVersion.ValidToDate);

            // None existing version.
            applicationVersion = UserService.Data.ApplicationManager.IsApplicationVersionValid(GetContext(), "NotAnApplicationIdenity", "NoneExistingVersion");
            Assert.IsNotNull(applicationVersion);
            Assert.AreEqual("NoneExistingVersion", applicationVersion.Version);
            Assert.IsFalse(applicationVersion.IsRecommended);
            Assert.IsFalse(applicationVersion.IsValid);
            Assert.IsTrue(applicationVersion.Description.Length < 2);
            Assert.IsNotNull(applicationVersion.ValidFromDate);
            Assert.IsNotNull(applicationVersion.ValidToDate);

            // Test with character '.
            applicationVersion = UserService.Data.ApplicationManager.IsApplicationVersionValid(GetContext(), "Not'AnApplicationIdenity", "None'ExistingVersion");
            Assert.IsNotNull(applicationVersion);
            Assert.IsFalse(applicationVersion.IsRecommended);
            Assert.IsFalse(applicationVersion.IsValid);
        }

        [TestMethod]
        public void RemoveAuthorityDataTypeFromApplication()
        {
            Int32 authorityDataTypeId;
            Int32 applicationId;

            UseTransaction = true;
            authorityDataTypeId = 1;
            applicationId = 1;
            UserService.Data.ApplicationManager.RemoveAuthorityDataTypeFromApplication(GetContext(), authorityDataTypeId, applicationId);
        }

        [TestMethod]
        public void UpdateApplication()
        {
            WebApplication application;

            // Get existing application.
            UseTransaction = true;
            application = UserService.Data.ApplicationManager.GetApplicationById(GetContext(), Settings.Default.TestApplicationId);
            application.Name = "TestNameUpdate123";
            application.ShortName = "TestShortNameUpdate123";
            application.Identifier = "TestAppIdUpdate";
            application.ModifiedBy = Settings.Default.TestUserId;
            application.AdministrationRoleId = Settings.Default.TestUserId;
            application.Description = "Testdescription update";
            application.ValidFromDate = DateTime.Now;
            application.ValidToDate = DateTime.Today.AddYears(100);
            WebApplication updatedApplication;
            updatedApplication = UserService.Data.ApplicationManager.UpdateApplication(GetContext(), application);
            Assert.IsNotNull(updatedApplication);
            Assert.AreEqual(Settings.Default.TestApplicationId, updatedApplication.Id);
            Assert.AreEqual(updatedApplication.Description, application.Description);
            Assert.AreEqual(updatedApplication.ModifiedBy, Settings.Default.TestUserId);
            Assert.AreEqual(updatedApplication.Name, application.Name);
            Assert.AreEqual(updatedApplication.Identifier, application.Identifier);
            Assert.IsNotNull(updatedApplication.ValidFromDate);
            Assert.IsNotNull(updatedApplication.ValidToDate);
        }

        [TestMethod]
        public void UpdateApplicationAction()
        {
            WebApplicationAction applicationAction;

            // Get existing applicationActionAction.
            UseTransaction = true;
            applicationAction = UserService.Data.ApplicationManager.GetApplicationAction(GetContext(), 3);
            applicationAction.Identifier = "UpdateAction";
            applicationAction.Name = "UpdateName";
            applicationAction.AdministrationRoleId = 1;
            applicationAction.ModifiedBy = Settings.Default.TestUserId;
            applicationAction.Description = "Testdescription update";
            applicationAction.ValidFromDate = DateTime.Now;
            applicationAction.ValidToDate = DateTime.Today.AddYears(100);
            WebApplicationAction updatedApplicationAction;
            updatedApplicationAction = UserService.Data.ApplicationManager.UpdateApplicationAction(GetContext(), applicationAction);
            Assert.IsNotNull(updatedApplicationAction);
            Assert.AreEqual(3, updatedApplicationAction.Id);
            Assert.AreEqual("UpdateAction", updatedApplicationAction.Identifier);
            Assert.AreEqual("UpdateName", updatedApplicationAction.Name);
            Assert.AreEqual(updatedApplicationAction.Description, applicationAction.Description);
            Assert.AreEqual(updatedApplicationAction.ModifiedBy, Settings.Default.TestUserId);
            Assert.IsNotNull(updatedApplicationAction.ValidFromDate);
            Assert.IsNotNull(updatedApplicationAction.ValidToDate);
        }

        [TestMethod]
        public void UpdateApplicationVersion()
        {
            WebApplicationVersion applicationVersion;

            // Get existing applicationVersionVersion.
            UseTransaction = true;
            applicationVersion = UserService.Data.ApplicationManager.GetApplicationVersion(GetContext(), 1);
            applicationVersion.Version = "2.0-0";
            applicationVersion.IsRecommended = true;
            applicationVersion.IsValid = true;
            applicationVersion.ModifiedBy = Settings.Default.TestUserId;
            applicationVersion.Description = "Testdescription update";
            applicationVersion.ValidFromDate = DateTime.Now;
            applicationVersion.ValidToDate = DateTime.Today.AddYears(100);
            WebApplicationVersion updatedApplicationVersion;
            updatedApplicationVersion = UserService.Data.ApplicationManager.UpdateApplicationVersion(GetContext(), applicationVersion);
            Assert.IsNotNull(updatedApplicationVersion);
            Assert.AreEqual(1, updatedApplicationVersion.Id);
            Assert.AreEqual("2.0-0", updatedApplicationVersion.Version);
            Assert.IsTrue(updatedApplicationVersion.IsRecommended);
            Assert.IsTrue(updatedApplicationVersion.IsValid);
            Assert.AreEqual(updatedApplicationVersion.Description, applicationVersion.Description);
            Assert.AreEqual(updatedApplicationVersion.ModifiedBy, Settings.Default.TestUserId);
            Assert.IsNotNull(updatedApplicationVersion.ValidFromDate);
            Assert.IsNotNull(updatedApplicationVersion.ValidToDate);
        }
    }
}
