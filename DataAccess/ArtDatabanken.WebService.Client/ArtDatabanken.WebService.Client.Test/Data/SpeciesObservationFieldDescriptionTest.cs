using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class SpeciesObservationFieldDescriptionTest : TestBase
    {
        SpeciesObservationFieldDescription _Description;

        public SpeciesObservationFieldDescriptionTest()
        {
            _Description = null;
        }

        [TestMethod]
        public void Constructor()
        {
            SpeciesObservationFieldDescription Description;

            Description = new SpeciesObservationFieldDescription();
            Assert.IsNotNull(Description);
        }

        public static SpeciesObservationFieldDescription GetOneDescription()
        {
            return new SpeciesObservationFieldDescription();
        }

        private SpeciesObservationFieldDescription GetDescription()
        {
            return GetDescription(false);
        }

        private SpeciesObservationFieldDescription GetDescription(Boolean refresh)
        {
            if (_Description.IsNull() || refresh)
            {
                _Description = new SpeciesObservationFieldDescription();
            }
            return _Description;
        }

        [TestMethod]
        public void Id()
        {
            const Int32 id = 20889;
            GetDescription(true).Id = id;
            Assert.AreEqual(GetDescription().Id, id);
        }

        [TestMethod]
        public void Class()
        {
            const String testString = "DarwinCore";
            GetDescription(true).Class = new SpeciesObservationClass();
            GetDescription().Class.Id = SpeciesObservationClassId.None;
            GetDescription().Class.Identifier = testString;
            Assert.AreEqual(GetDescription().Class.GetName(), testString);
        }

        [TestMethod]
        public void Name()
        {
            const String testString = "DarwinCore";
            GetDescription(true).Name = testString;
            Assert.AreEqual(GetDescription().Name, testString);
        }

        [TestMethod]
        public void Type()
        {
            const DataType dataType = DataType.Boolean;
            GetDescription(true).Type = dataType;
            Assert.AreEqual(GetDescription().Type, dataType);
        }

        [TestMethod]
        public void Definition()
        {
            const String testString = "This is DarwinCore term";
            GetDescription(true).Definition = testString;
            Assert.AreEqual(GetDescription().Definition, testString);
        }

        [TestMethod]
        public void DefinitionUrl()
        {
            const String testString = "http://DarwinCore";
            GetDescription(true).DefinitionUrl = testString;
            Assert.AreEqual(GetDescription().DefinitionUrl, testString);
        }

        public void Documentation()
        {
            const String testString = "This is DarwinCore term";
            GetDescription(true).Documentation = testString;
            Assert.AreEqual(GetDescription().Documentation, testString);
        }

        [TestMethod]
        public void DocumentationUrl()
        {
            const String testString = "http://DarwinCore";
            GetDescription(true).DocumentationUrl = testString;
            Assert.AreEqual(GetDescription().DocumentationUrl, testString);
        }

        [TestMethod]
        public void Label()
        {
            const String testString = "Darwin Core";
            GetDescription(true).Label = testString;
            Assert.AreEqual(GetDescription().Label, testString);
        }

        [TestMethod]
        public void Guid()
        {
            const String testString = "GUID";
            GetDescription(true).Guid = testString;
            Assert.AreEqual(GetDescription().Guid, testString);
        }

        [TestMethod]
        public void Uuid()
        {
            const String testString = "UUID";
            GetDescription(true).Uuid = testString;
            Assert.AreEqual(GetDescription().Uuid, testString);
        }

        [TestMethod]
        public void SortOrder()
        {
            const Int32 sortOrder = 1;
            GetDescription(true).SortOrder = sortOrder;
            Assert.AreEqual(GetDescription().SortOrder, sortOrder);
        }

        [TestMethod]
        public void Importance()
        {
            const Int32 importance = 1;
            GetDescription(true).Importance = importance;
            Assert.AreEqual(GetDescription().Importance, importance);
        }

        [TestMethod]
        public void Remarks()
        {
            const String remarks = "This is excellent";
            GetDescription(true).Remarks = remarks;
            Assert.AreEqual(GetDescription().Remarks, remarks);
        }

        [TestMethod]
        public void IsAcceptedByTdwg()
        {
            const Boolean testBool = true;
            GetDescription(true).IsAcceptedByTdwg = testBool;
            Assert.AreEqual(GetDescription().IsAcceptedByTdwg, testBool);
        }

        [TestMethod]
        public void IsImplemented()
        {
            const Boolean testBool = true;
            GetDescription(true).IsImplemented = testBool;
            Assert.AreEqual(GetDescription().IsImplemented, testBool);
        }

        [TestMethod]
        public void IsPlanned()
        {
            const Boolean testBool = true;
            GetDescription(true).IsPlanned = testBool;
            Assert.AreEqual(GetDescription().IsPlanned, testBool);
        }

        [TestMethod]
        public void IsMandatory()
        {
            const Boolean testBool = true;
            GetDescription(true).IsMandatory = testBool;
            Assert.AreEqual(GetDescription().IsMandatory, testBool);
        }

        [TestMethod]
        public void IsMandatoryFromProvider()
        {
            const Boolean testBool = true;
            GetDescription(true).IsMandatoryFromProvider = testBool;
            Assert.AreEqual(GetDescription().IsMandatoryFromProvider, testBool);
        }

        [TestMethod]
        public void IsObtainedFromProvider()
        {
            const Boolean testBool = true;
            GetDescription(true).IsObtainedFromProvider = testBool;
            Assert.AreEqual(GetDescription().IsObtainedFromProvider, testBool);
        }

        [TestMethod]
        public void IsClassName()
        {
            const Boolean testBool = true;
            GetDescription(true).IsClass = testBool;
            Assert.AreEqual(GetDescription().IsClass, testBool);
        }

        [TestMethod]
        public void Mappings()
        {
            SpeciesObservationFieldMappingList mappings = new SpeciesObservationFieldMappingList();
            GetDescription(true).Mappings = mappings;
            Assert.AreEqual(GetDescription().Mappings, mappings);
        }
    }
}
