using System;
using System.Collections.Generic;
using System.ServiceModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using ArtDatabanken.WebService.Client.UserService;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class ApplicationManagerTest : TestBase
    {

        private ApplicationManager _applicationManager;

        public ApplicationManagerTest()
        {
            _applicationManager = null;
        }

        [TestMethod]
        public void Constructor()
        {
            ApplicationManager applicationManager;

            applicationManager = new ApplicationManager();
            Assert.IsNotNull(applicationManager);
        }

        [TestMethod]
        public void AddAuthorityDataTypeToApplication()
        {
            Int32 applicationId = 1;
            Int32 authorityDataTypeId = 2;
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                GetApplicationManager(true).AddAuthorityDataTypeToApplication(GetUserContext(), authorityDataTypeId, applicationId);
            }
        }

        [TestMethod]
        public void CreateApplication()
        {
            DateTime validFromDate, validToDate;
            Int32 administrationRoleId, contactUserId;
            IApplication application;
            String applicationIdentity, name, description,
                   shortName;

            // Test data that is not set in the client.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                application = GetNewApplication();
                GetApplicationManager(true).CreateApplication(GetUserContext(), application);
                Assert.IsNotNull(application);

                // Test created by user.
                Assert.AreNotEqual(Int32.MinValue, application.UpdateInformation.CreatedBy);
                Assert.AreEqual(application.UpdateInformation.ModifiedBy, application.UpdateInformation.CreatedBy);

                // Test created date.
                Assert.IsTrue((DateTime.Now - application.UpdateInformation.CreatedDate) < new TimeSpan(0, 0, Settings.Default.ComputerTimeDifference));

                // Test DataContext.
                Assert.IsNotNull(application.DataContext);

                // Test GUID.
                Assert.IsTrue(application.GUID.IsNotEmpty());

                // Test id.
                Assert.AreNotEqual(Int32.MinValue, application.Id);

                // Test modified by user.
                Assert.AreNotEqual(Int32.MinValue, application.UpdateInformation.ModifiedBy);
                Assert.AreEqual(application.UpdateInformation.ModifiedBy, application.UpdateInformation.CreatedBy);

                // Test modified date.
                Assert.IsTrue((DateTime.Now - application.UpdateInformation.ModifiedDate) < new TimeSpan(0, 0, Settings.Default.ComputerTimeDifference));
            }


            // Test administration role id.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                application = GetNewApplication();
                administrationRoleId = 42;
                application.AdministrationRoleId = administrationRoleId; ;
                GetApplicationManager(true).CreateApplication(GetUserContext(), application);
                Assert.IsNotNull(application);
                Assert.AreEqual(administrationRoleId, application.AdministrationRoleId.Value);
            }


            // Test applicationIdentity
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                applicationIdentity = @"ApplicationIdentity";
                application = GetNewApplication();
                application.Identifier = applicationIdentity;
                GetApplicationManager(true).CreateApplication(GetUserContext(), application);
                Assert.IsNotNull(application);
                Assert.AreEqual(applicationIdentity, application.Identifier);
            }

            // Test name.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                name = @"ApplicationName";
                application = GetNewApplication();
                application.Name = name;
                GetApplicationManager(true).CreateApplication(GetUserContext(), application);
                Assert.IsNotNull(application);
                Assert.AreEqual(name, application.Name);
            }

            // Test description
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                description = @"Hej hopp i lingonskogen räksmörgås RÄKSMÖRGÅS";
                application = GetNewApplication();
                application.Description = description;
                GetApplicationManager(true).CreateApplication(GetUserContext(), application);
                Assert.IsNotNull(application);
                Assert.AreEqual(description, application.Description);
            }

            // Test short name
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                shortName = @"ShortName";
                application = GetNewApplication();
                application.ShortName = shortName;
                GetApplicationManager(true).CreateApplication(GetUserContext(), application);
                Assert.IsNotNull(application);
                Assert.AreEqual(shortName, application.ShortName);
            }

            // Test ContactUserId
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                contactUserId = 1;
                application = GetNewApplication();
                application.ContactPersonId = contactUserId;
                GetApplicationManager(true).CreateApplication(GetUserContext(), application);
                Assert.IsNotNull(application);
                Assert.AreEqual(contactUserId, application.ContactPersonId);
            }


            // Test valid from date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validFromDate = new DateTime(2000, 6, 5);
                application = GetNewApplication();
                application.ValidFromDate = validFromDate;
                GetApplicationManager(true).CreateApplication(GetUserContext(), application);
                Assert.IsNotNull(application);
                Assert.AreEqual(validFromDate, application.ValidFromDate);
            }

            // Test valid to date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validToDate = new DateTime(2000, 6, 5);
                application = GetNewApplication();
                application.ValidToDate = validToDate;
                GetApplicationManager(true).CreateApplication(GetUserContext(), application);
                Assert.IsNotNull(application);
                Assert.AreEqual(validToDate, application.ValidToDate);
            }

        }

        [TestMethod]
        public void CreateApplicationAction()
        {
            DateTime validFromDate, validToDate;
            Int32 applicationId, administrationRoleId;
            IApplicationAction applicationAction;
            String actionIdentity, description, name;


            // Test data that is not set in the client.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                applicationAction = GetNewApplicationAction();
                GetApplicationManager(true).CreateApplicationAction(GetUserContext(), applicationAction);
                Assert.IsNotNull(applicationAction);

                // Test created by user.
                Assert.AreNotEqual(Int32.MinValue, applicationAction.UpdateInformation.CreatedBy);
                Assert.AreEqual(applicationAction.UpdateInformation.ModifiedBy, applicationAction.UpdateInformation.CreatedBy);

                // Test created date.
                Assert.IsTrue((DateTime.Now - applicationAction.UpdateInformation.CreatedDate) < new TimeSpan(0, 0, Settings.Default.ComputerTimeDifference));

                // Test DataContext.
                Assert.IsNotNull(applicationAction.DataContext);

                // Test id.
                Assert.AreNotEqual(Int32.MinValue, applicationAction.Id);

                // Test GUID.
                Assert.IsTrue(applicationAction.GUID.IsNotEmpty());

                // Test modified by user.
                Assert.AreNotEqual(Int32.MinValue, applicationAction.UpdateInformation.ModifiedBy);
                Assert.AreEqual(applicationAction.UpdateInformation.ModifiedBy, applicationAction.UpdateInformation.CreatedBy);

                // Test modified date.
                Assert.IsTrue((DateTime.Now - applicationAction.UpdateInformation.ModifiedDate) < new TimeSpan(0, 0, Settings.Default.ComputerTimeDifference));
            }

            // Test actionIdentity
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                actionIdentity = @"ActionIdentity";
                applicationAction = GetNewApplicationAction();
                applicationAction.Identifier = actionIdentity;
                GetApplicationManager(true).CreateApplicationAction(GetUserContext(), applicationAction);
                Assert.IsNotNull(applicationAction);
                Assert.AreEqual(actionIdentity, applicationAction.Identifier);
            }

            // Test administration role id.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                applicationAction = GetNewApplicationAction();
                administrationRoleId = 1;
                applicationAction.AdministrationRoleId = administrationRoleId;
                GetApplicationManager(true).CreateApplicationAction(GetUserContext(), applicationAction);
                Assert.IsNotNull(applicationAction);
                Assert.AreEqual(administrationRoleId, applicationAction.AdministrationRoleId.Value);
            }

            // Test applicationId.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                applicationAction = GetNewApplicationAction();
                applicationId = 3;
                applicationAction.ApplicationId = applicationId; ;
                GetApplicationManager(true).CreateApplicationAction(GetUserContext(), applicationAction);
                Assert.IsNotNull(applicationAction);
                Assert.AreEqual(applicationId, applicationAction.ApplicationId);
            }

            // Test description
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                description = @"Hej hopp i lingonskogen räksmörgås RÄKSMÖRGÅS";
                applicationAction = GetNewApplicationAction();
                applicationAction.Description = description;
                GetApplicationManager(true).CreateApplicationAction(GetUserContext(), applicationAction);
                Assert.IsNotNull(applicationAction);
                Assert.AreEqual(description, applicationAction.Description);
            }

            // Test name
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                name = @"ActionName";
                applicationAction = GetNewApplicationAction();
                applicationAction.Name = name;
                GetApplicationManager(true).CreateApplicationAction(GetUserContext(), applicationAction);
                Assert.IsNotNull(applicationAction);
                Assert.AreEqual(name, applicationAction.Name);
            }

            // Test valid from date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validFromDate = new DateTime(2000, 6, 5);
                applicationAction = GetNewApplicationAction();
                applicationAction.ValidFromDate = validFromDate;
                GetApplicationManager(true).CreateApplicationAction(GetUserContext(), applicationAction);
                Assert.IsNotNull(applicationAction);
                Assert.AreEqual(validFromDate, applicationAction.ValidFromDate);
            }

            // Test valid to date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validToDate = new DateTime(2000, 6, 5);
                applicationAction = GetNewApplicationAction();
                applicationAction.ValidToDate = validToDate;
                GetApplicationManager(true).CreateApplicationAction(GetUserContext(), applicationAction);
                Assert.IsNotNull(applicationAction);
                Assert.AreEqual(validToDate, applicationAction.ValidToDate);
            }

        }

        [TestMethod]
        public void CreateApplicationVersion()
        {
            Boolean isValid, isRecommended;
            DateTime validFromDate, validToDate;
            Int32 applicationId;
            IApplicationVersion applicationVersion;
            String version, description;


            // Test data that is not set in the client.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                applicationVersion = GetNewApplicationVersion();
                GetApplicationManager(true).CreateApplicationVersion(GetUserContext(), applicationVersion);
                Assert.IsNotNull(applicationVersion);

                // Test created by user.
                Assert.AreNotEqual(Int32.MinValue, applicationVersion.UpdateInformation.CreatedBy);
                Assert.AreEqual(applicationVersion.UpdateInformation.ModifiedBy, applicationVersion.UpdateInformation.CreatedBy);

                // Test created date.
                Assert.IsTrue((DateTime.Now - applicationVersion.UpdateInformation.CreatedDate) < new TimeSpan(0, 0, Settings.Default.ComputerTimeDifference));

                // Test DataContext.
                Assert.IsNotNull(applicationVersion.DataContext);

                // Test id.
                Assert.AreNotEqual(Int32.MinValue, applicationVersion.Id);

                // Test modified by user.
                Assert.AreNotEqual(Int32.MinValue, applicationVersion.UpdateInformation.ModifiedBy);
                Assert.AreEqual(applicationVersion.UpdateInformation.ModifiedBy, applicationVersion.UpdateInformation.CreatedBy);

                // Test modified date.
                Assert.IsTrue((DateTime.Now - applicationVersion.UpdateInformation.ModifiedDate) < new TimeSpan(0, 0, Settings.Default.ComputerTimeDifference));
            }


            // Test applicationId.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                applicationVersion = GetNewApplicationVersion();
                applicationId = 3;
                applicationVersion.ApplicationId = applicationId; ;
                GetApplicationManager(true).CreateApplicationVersion(GetUserContext(), applicationVersion);
                Assert.IsNotNull(applicationVersion);
                Assert.AreEqual(applicationId, applicationVersion.ApplicationId);
            }


            // Test isValid
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                isValid = true;
                applicationVersion = GetNewApplicationVersion();
                applicationVersion.IsValid = isValid;
                GetApplicationManager(true).CreateApplicationVersion(GetUserContext(), applicationVersion);
                Assert.IsNotNull(applicationVersion);
                Assert.AreEqual(isValid, applicationVersion.IsValid);
            }

            // Test isRecommended
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                isRecommended = true;
                applicationVersion = GetNewApplicationVersion();
                applicationVersion.IsRecommended = isRecommended;
                GetApplicationManager(true).CreateApplicationVersion(GetUserContext(), applicationVersion);
                Assert.IsNotNull(applicationVersion);
                Assert.AreEqual(isRecommended, applicationVersion.IsRecommended);
            }

            // Test version.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                version = @"ApplicationVersionName";
                applicationVersion = GetNewApplicationVersion();
                applicationVersion.Version = version;
                GetApplicationManager(true).CreateApplicationVersion(GetUserContext(), applicationVersion);
                Assert.IsNotNull(applicationVersion);
                Assert.AreEqual(version, applicationVersion.Version);
            }

            // Test description
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                description = @"Hej hopp i lingonskogen räksmörgås RÄKSMÖRGÅS";
                applicationVersion = GetNewApplicationVersion();
                applicationVersion.Description = description;
                GetApplicationManager(true).CreateApplicationVersion(GetUserContext(), applicationVersion);
                Assert.IsNotNull(applicationVersion);
                Assert.AreEqual(description, applicationVersion.Description);
            }


            // Test valid from date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validFromDate = new DateTime(2000, 6, 5);
                applicationVersion = GetNewApplicationVersion();
                applicationVersion.ValidFromDate = validFromDate;
                GetApplicationManager(true).CreateApplicationVersion(GetUserContext(), applicationVersion);
                Assert.IsNotNull(applicationVersion);
                Assert.AreEqual(validFromDate, applicationVersion.ValidFromDate);
            }

            // Test valid to date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validToDate = new DateTime(2000, 6, 5);
                applicationVersion = GetNewApplicationVersion();
                applicationVersion.ValidToDate = validToDate;
                GetApplicationManager(true).CreateApplicationVersion(GetUserContext(), applicationVersion);
                Assert.IsNotNull(applicationVersion);
                Assert.AreEqual(validToDate, applicationVersion.ValidToDate);
            }
        }

   
        [TestMethod]
        public void DataSource()
        {
            IApplicationDataSource dataSource;

            dataSource = null;
            GetApplicationManager(true).DataSource = dataSource;
            Assert.AreEqual(dataSource, GetApplicationManager().DataSource);

            dataSource = new ApplicationDataSource();
            GetApplicationManager().DataSource = dataSource;
            Assert.AreEqual(dataSource, GetApplicationManager().DataSource);
        }

        
        [TestMethod]
        public void DeleteApplication()
        {
            IApplication application;
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                application = GetNewApplication();
                GetApplicationManager(true).CreateApplication(GetUserContext(), application);
                GetApplicationManager().DeleteApplication(GetUserContext(), application);
            }
        }


        [TestMethod]
        public void GetDataSourceInformation()
        {
            Assert.IsNotNull(GetApplicationManager(true).GetDataSourceInformation());
        }

        private IApplication GetNewApplication()
        {
            IApplication newApplication;
            newApplication = new Application(GetUserContext());
            newApplication.Identifier = @"NewApplication";
            newApplication.Name = @"NewApplication";
            newApplication.Description = @"testdescription";
            return newApplication;
        }

        private IApplicationAction GetNewApplicationAction()
        {
            IApplicationAction newApplicationAction;
            newApplicationAction = new ApplicationAction(GetUserContext());
            newApplicationAction.Identifier = @"ActionIdentity";
            newApplicationAction.ApplicationId = 3;
            newApplicationAction.Name = @"ActionName";
            newApplicationAction.Description = @"testdescription";
            return newApplicationAction;
        }

        private IApplicationVersion GetNewApplicationVersion()
        {
            IApplicationVersion newApplicationVersion;
            newApplicationVersion = new ApplicationVersion(GetUserContext());
            newApplicationVersion.ApplicationId = 3;
            newApplicationVersion.Version = "3.1";
            newApplicationVersion.Description = @"testdescription";
            return newApplicationVersion;
        }

        private IApplication GetOneApplication()
        {
            IApplication application;

            // It is assumed that this method is called
            // inside a transaction.
            application = GetNewApplication();
            GetApplicationManager(true).CreateApplication(GetUserContext(), application);
            return application;
        }

        private IApplicationAction GetOneApplicationAction()
        {
            IApplicationAction applicationAction;

            // It is assumed that this method is called
            // inside a transaction.
            applicationAction = GetNewApplicationAction();
            GetApplicationManager(true).CreateApplicationAction(GetUserContext(), applicationAction);
            return applicationAction;
        }

        private IApplicationVersion GetOneApplicationVersion()
        {
            IApplicationVersion applicationVersion;

            // It is assumed that this method is called
            // inside a transaction.
            applicationVersion = GetNewApplicationVersion();
            GetApplicationManager(true).CreateApplicationVersion(GetUserContext(), applicationVersion);
            return applicationVersion;
        }

        [TestMethod]
        public void GetApplication()
        {
            IApplication application1, application2;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                application1 = GetNewApplication();
                GetApplicationManager(true).CreateApplication(GetUserContext(), application1);
                application2 = GetApplicationManager().GetApplication(GetUserContext(), application1.Id);
                Assert.AreEqual(application1.Id, application2.Id);
            }

            foreach (ApplicationIdentifier applicationIdentifier in Enum.GetValues(typeof(ApplicationIdentifier)))
            {
                application1 = GetApplicationManager().GetApplication(GetUserContext(), applicationIdentifier);
                Assert.IsNotNull(application1);
            }
        }

        [TestMethod]
        public void GetApplicationAction()
        {
            IApplicationAction applicationAction1, applicationAction2;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                applicationAction1 = GetNewApplicationAction();
                GetApplicationManager(true).CreateApplicationAction(GetUserContext(), applicationAction1);
                applicationAction2 = GetApplicationManager(true).GetApplicationAction(GetUserContext(), applicationAction1.Id);
                Assert.AreEqual(applicationAction1.Id, applicationAction2.Id);
            }
        }

        [TestMethod]
        public void GetApplicationActions()
        {
            ApplicationActionList ApplicationActionList;

            ApplicationActionList = GetApplicationManager(true).GetApplicationActions(GetUserContext(), Settings.Default.TestApplicationId);
            Assert.IsTrue(ApplicationActionList.IsNotEmpty());
            Assert.IsInstanceOfType(ApplicationActionList[0], typeof(ApplicationAction));
            Assert.IsTrue(ApplicationActionList.Count > 0);
        }

        [TestMethod]
        public void GetApplicationActionsByGUIDs()
        {
            List<String> applicationActionGUIDs = new List<String>();
            applicationActionGUIDs.Add("3");
            applicationActionGUIDs.Add("4");

            ApplicationActionList ApplicationActionList = GetApplicationManager(true).GetApplicationActionsByGUIDs(GetUserContext(), applicationActionGUIDs);
            Assert.IsTrue(ApplicationActionList.IsNotEmpty());
            Assert.IsInstanceOfType(ApplicationActionList[0], typeof(ApplicationAction));
            Assert.IsTrue(ApplicationActionList.Count > 1);
        }

        [TestMethod]
        public void GetApplicationActionsByIdList()
        {
            ApplicationActionList ApplicationActionList;
            List<Int32> applicationActionIdList = new List<Int32>();
            applicationActionIdList.Add(3);
            applicationActionIdList.Add(4);

            ApplicationActionList = GetApplicationManager(true).GetApplicationActionsByIds(GetUserContext(), applicationActionIdList);
            Assert.IsTrue(ApplicationActionList.IsNotEmpty());
            Assert.IsInstanceOfType(ApplicationActionList[0], typeof(ApplicationAction));
            Assert.IsTrue(ApplicationActionList.Count > 1);
        }

        [TestMethod]
        public void GetApplications()
        {
            ApplicationList applicationList;

            applicationList = GetApplicationManager(true).GetApplications(GetUserContext());
            Assert.IsTrue(applicationList.IsNotEmpty());
            Assert.IsInstanceOfType(applicationList[0], typeof(Application));
            Assert.IsTrue(applicationList.Count > 2);
        }

        [TestMethod]
        public void GetApplicationVersion()
        {
            IApplicationVersion applicationVersion1, applicationVersion2;

            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                applicationVersion1 = GetNewApplicationVersion();
                GetApplicationManager(true).CreateApplicationVersion(GetUserContext(), applicationVersion1);
                applicationVersion2 = GetApplicationManager().GetApplicationVersion(GetUserContext(), applicationVersion1.Id);
                Assert.AreEqual(applicationVersion1.Id, applicationVersion2.Id);
            }
        }

        [TestMethod]
        public void GetApplicationVersionList()
        {
            ApplicationVersionList ApplicationVersionList;

            ApplicationVersionList = GetApplicationManager(true).GetApplicationVersions(GetUserContext(), Settings.Default.TestApplicationId);
            Assert.IsTrue(ApplicationVersionList.IsNotEmpty());
            Assert.IsInstanceOfType(ApplicationVersionList[0], typeof(ApplicationVersion));
            Assert.IsTrue(ApplicationVersionList.Count > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException<ExceptionDetail>))]
        public void GetApplicationIdError()
        {
            IApplication application;
            application = GetApplicationManager(true).GetApplication(GetUserContext(), Int32.MinValue);
            Assert.IsNull(application);
        }


        private ApplicationManager GetApplicationManager()
        {
            return GetApplicationManager(false);
        }

        private ApplicationManager GetApplicationManager(Boolean refresh)
        {
            if (_applicationManager.IsNull() || refresh)
            {
                _applicationManager = new ApplicationManager();
                _applicationManager.DataSource = new ApplicationDataSource();
            }
            return _applicationManager;
        }

        [TestMethod]
        public void GetApplicationVersionByIdentifierAndVersion()
        {
            IApplicationVersion version;
            String applicationIdentifier, applicationVersion;

            applicationIdentifier = "No application";
            applicationVersion = "No version";
            Assert.IsNull(GetApplicationManager(true).GetApplicationVersion(GetUserContext(), applicationIdentifier, applicationVersion));

            applicationIdentifier = ApplicationIdentifier.PrintObs.ToString();
            applicationVersion = "No version";
            Assert.IsNull(GetApplicationManager(true).GetApplicationVersion(GetUserContext(), applicationIdentifier, applicationVersion));

            applicationIdentifier = ApplicationIdentifier.PrintObs.ToString();
            applicationVersion = "0.3.4163.17509";
            version = GetApplicationManager().GetApplicationVersion(GetUserContext(), applicationIdentifier, applicationVersion);
            Assert.IsNotNull(version);
            Assert.AreEqual(version.Version, applicationVersion);
        }

        [TestMethod]
        public void RemoveAuthorityDataTypeFromApplication()
        {
            Int32 applicationId = 1;
            Int32 authorityDataTypeId = 2;
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                GetApplicationManager(true).RemoveAuthorityDataTypeFromApplication(GetUserContext(), authorityDataTypeId, applicationId);
            }
        }

        [TestMethod]
        public void UpdateApplication()
        {
            DateTime validFromDate, validToDate;
            Int32 administrationRoleId, contactUserId;
            IApplication application;
            String applicationIdentity, name, description,
                   shortName;

            // Test administration role id.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                application = GetOneApplication();
                administrationRoleId = 42;
                application.AdministrationRoleId = administrationRoleId; ;
                GetApplicationManager(true).UpdateApplication(GetUserContext(), application);
                Assert.IsNotNull(application);
                Assert.AreEqual(administrationRoleId, application.AdministrationRoleId.Value);
            }

            // Test email address.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                application = GetOneApplication();
                shortName = @"MyShortName";
                application.ShortName = shortName;
                GetApplicationManager(true).UpdateApplication(GetUserContext(), application);
                Assert.IsNotNull(application);
                Assert.AreEqual(shortName, application.ShortName);
            }

            // Test name.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                name = @"ApplicationName";
                application = GetOneApplication();
                application.Name = name;
                GetApplicationManager(true).UpdateApplication(GetUserContext(), application);
                Assert.IsNotNull(application);
                Assert.AreEqual(name, application.Name);
            }


            // Test applicationIdentity.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                applicationIdentity = @"ApplicationIdentity";
                application = GetOneApplication();
                application.Identifier = applicationIdentity;
                GetApplicationManager(true).UpdateApplication(GetUserContext(), application);
                Assert.IsNotNull(application);
                Assert.AreEqual(applicationIdentity, application.Identifier);
            }

            // Test description.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                description = @"Hej hopp i lingonskogen";
                application = GetOneApplication();
                application.Description = description;
                GetApplicationManager(true).UpdateApplication(GetUserContext(), application);
                Assert.IsNotNull(application);
                Assert.AreEqual(description, application.Description);
            }

            // Test ContactUserId
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                contactUserId = 1;
                application = GetOneApplication();
                application.ContactPersonId = contactUserId;
                GetApplicationManager(true).UpdateApplication(GetUserContext(), application);
                Assert.IsNotNull(application);
                Assert.AreEqual(contactUserId, application.ContactPersonId);
            }

            // Test valid from date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validFromDate = new DateTime(2010, 6, 5);
                application = GetOneApplication();
                application.ValidFromDate = validFromDate;
                GetApplicationManager(true).UpdateApplication(GetUserContext(), application);
                Assert.IsNotNull(application);
                Assert.AreEqual(validFromDate, application.ValidFromDate);
            }

            // Test valid to date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validToDate = new DateTime(2010, 6, 5);
                application = GetOneApplication();
                application.ValidToDate = validToDate;
                GetApplicationManager(true).UpdateApplication(GetUserContext(), application);
                Assert.IsNotNull(application);
                Assert.AreEqual(validToDate, application.ValidToDate);
            }


        }

        [TestMethod]
        public void UpdateApplicationAction()
        {
            DateTime validFromDate, validToDate;
            Int32 applicationId, administrationRoleId;
            IApplicationAction applicationAction;
            String actionIdentity, description, name;

            // Test actionIdentity
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                actionIdentity = @"ActionIdentity";
                applicationAction = GetOneApplicationAction();
                applicationAction.Name = actionIdentity;
                GetApplicationManager(true).UpdateApplicationAction(GetUserContext(), applicationAction);
                Assert.IsNotNull(applicationAction);
                Assert.AreEqual(actionIdentity, applicationAction.Name);
            }

            // Test administration role id.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                applicationAction = GetOneApplicationAction();
                administrationRoleId = 42;
                applicationAction.AdministrationRoleId = administrationRoleId;
                GetApplicationManager(true).UpdateApplicationAction(GetUserContext(), applicationAction);
                Assert.IsNotNull(applicationAction);
                Assert.AreEqual(administrationRoleId, applicationAction.AdministrationRoleId);
            }

            // Test application id.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                applicationAction = GetOneApplicationAction();
                applicationId = 3;
                applicationAction.ApplicationId = applicationId;
                GetApplicationManager(true).UpdateApplicationAction(GetUserContext(), applicationAction);
                Assert.IsNotNull(applicationAction);
                Assert.AreEqual(applicationId, applicationAction.ApplicationId);
            }


            // Test description.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                description = @"Hej hopp i lingonskogen";
                applicationAction = GetOneApplicationAction();
                applicationAction.Description = description;
                GetApplicationManager(true).UpdateApplicationAction(GetUserContext(), applicationAction);
                Assert.IsNotNull(applicationAction);
                Assert.AreEqual(description, applicationAction.Description);
            }

            // Test name.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                name = @"ActionName";
                applicationAction = GetOneApplicationAction();
                applicationAction.Name = name;
                GetApplicationManager(true).UpdateApplicationAction(GetUserContext(), applicationAction);
                Assert.IsNotNull(applicationAction);
                Assert.AreEqual(name, applicationAction.Name);
            }

            // Test valid from date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validFromDate = new DateTime(2010, 6, 5);
                applicationAction = GetOneApplicationAction();
                applicationAction.ValidFromDate = validFromDate;
                GetApplicationManager(true).UpdateApplicationAction(GetUserContext(), applicationAction);
                Assert.IsNotNull(applicationAction);
                Assert.AreEqual(validFromDate, applicationAction.ValidFromDate);
            }

            // Test valid to date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validToDate = new DateTime(2010, 6, 5);
                applicationAction = GetOneApplicationAction();
                applicationAction.ValidToDate = validToDate;
                GetApplicationManager(true).UpdateApplicationAction(GetUserContext(), applicationAction);
                Assert.IsNotNull(applicationAction);
                Assert.AreEqual(validToDate, applicationAction.ValidToDate);
            }

        }

        [TestMethod]
        public void UpdateApplicationVersion()
        {
            DateTime validFromDate, validToDate;
            Int32 applicationId;
            String version, description;
            Boolean isValid, isRecommended;

            IApplicationVersion applicationVersion;

            // Test administration role id.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                applicationVersion = GetOneApplicationVersion();
                applicationId = 3;
                applicationVersion.ApplicationId = applicationId;
                GetApplicationManager(true).UpdateApplicationVersion(GetUserContext(), applicationVersion);
                Assert.IsNotNull(applicationVersion);
                Assert.AreEqual(applicationId, applicationVersion.ApplicationId);
            }

            // Test description.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                description = @"Hej hopp i lingonskogen";
                applicationVersion = GetOneApplicationVersion();
                applicationVersion.Description = description;
                GetApplicationManager(true).UpdateApplicationVersion(GetUserContext(), applicationVersion);
                Assert.IsNotNull(applicationVersion);
                Assert.AreEqual(description, applicationVersion.Description);
            }

            // Test IsValid
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                isValid = true;
                applicationVersion = GetOneApplicationVersion();
                applicationVersion.IsValid = isValid;
                GetApplicationManager(true).UpdateApplicationVersion(GetUserContext(), applicationVersion);
                Assert.IsNotNull(applicationVersion);
                Assert.AreEqual(isValid, applicationVersion.IsValid);
            }

            // Test IsRecommended
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                isRecommended = true;
                applicationVersion = GetOneApplicationVersion();
                applicationVersion.IsRecommended = isRecommended;
                GetApplicationManager(true).UpdateApplicationVersion(GetUserContext(), applicationVersion);
                Assert.IsNotNull(applicationVersion);
                Assert.AreEqual(isRecommended, applicationVersion.IsRecommended);
            }

            // Test valid from date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validFromDate = new DateTime(2010, 6, 5);
                applicationVersion = GetOneApplicationVersion();
                applicationVersion.ValidFromDate = validFromDate;
                GetApplicationManager(true).UpdateApplicationVersion(GetUserContext(), applicationVersion);
                Assert.IsNotNull(applicationVersion);
                Assert.AreEqual(validFromDate, applicationVersion.ValidFromDate);
            }

            // Test valid to date.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                validToDate = new DateTime(2010, 6, 5);
                applicationVersion = GetOneApplicationVersion();
                applicationVersion.ValidToDate = validToDate;
                GetApplicationManager(true).UpdateApplicationVersion(GetUserContext(), applicationVersion);
                Assert.IsNotNull(applicationVersion);
                Assert.AreEqual(validToDate, applicationVersion.ValidToDate);
            }

            // Test version.
            using (ITransaction transaction = GetUserContext().StartTransaction())
            {
                version = "3.5";
                applicationVersion = GetOneApplicationVersion();
                applicationVersion.Version = version;
                GetApplicationManager(true).UpdateApplicationVersion(GetUserContext(), applicationVersion);
                Assert.IsNotNull(applicationVersion);
                Assert.AreEqual(version, applicationVersion.Version);
            }


        }

    }
}
