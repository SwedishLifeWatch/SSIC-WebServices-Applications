using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Data.Test
{
    /// <summary>
    /// Test all properties and constructor for WebSpeciesObservationFieldDescription class.
    /// </summary>
    [TestClass]
    public class WebSpeciesObservationFieldDescriptionTest : WebDomainTestBase<WebSpeciesObservationFieldDescription>
    {
        [TestMethod]
        public void Constructor()
        {
            WebSpeciesObservationFieldDescription webSpeciesObservationFieldDescription = new WebSpeciesObservationFieldDescription();
            Assert.IsNotNull(webSpeciesObservationFieldDescription);
        }

        [TestMethod]
        public void Id()
        {
            const Int32 id = 20889;
            GetObject(true).Id = id;
            Assert.AreEqual(GetObject().Id, id);
        }

        [TestMethod]
        public void Class()
        {
            const String testString = "DarwinCore";
            GetObject(true).Class = new WebSpeciesObservationClass();
            GetObject().Class.Identifier = testString;
            Assert.AreEqual(GetObject().Class.Identifier, testString);
        }

        [TestMethod]
        public void Name()
        {
            const String testString = "DarwinCore";
            GetObject(true).Name = testString;
            Assert.AreEqual(GetObject().Name, testString);
        }

        [TestMethod]
        public void Type()
        {
            const WebDataType dataType = WebDataType.Boolean;
            GetObject(true).Type = dataType;
            Assert.AreEqual(GetObject().Type, dataType);
        }

        [TestMethod]
        public void Definition()
        {
            const String testString = "This is DarwinCore term";
            GetObject(true).Definition = testString;
            Assert.AreEqual(GetObject().Definition, testString);
        }

        [TestMethod]
        public void DefinitionUrl()
        {
            const String testString = "http://DarwinCore";
            GetObject(true).DefinitionUrl = testString;
            Assert.AreEqual(GetObject().DefinitionUrl, testString);
        }

        public void Documentation()
        {
            const String testString = "This is DarwinCore term";
            GetObject(true).Documentation = testString;
            Assert.AreEqual(GetObject().Documentation, testString);
        }

        [TestMethod]
        public void DocumentationUrl()
        {
            const String testString = "http://DarwinCore";
            GetObject(true).DocumentationUrl = testString;
            Assert.AreEqual(GetObject().DocumentationUrl, testString);
        }

        [TestMethod]
        public void Label()
        {
            const String testString = "Darwin Core";
            GetObject(true).Label = testString;
            Assert.AreEqual(GetObject().Label, testString);
        }

        [TestMethod]
        public void Guid()
        {
            const String testString = "GUID";
            GetObject(true).Guid = testString;
            Assert.AreEqual(GetObject().Guid, testString);
        }

        [TestMethod]
        public void Uuid()
        {
            const String testString = "UUID";
            GetObject(true).Uuid = testString;
            Assert.AreEqual(GetObject().Uuid, testString);
        }

        [TestMethod]
        public void SortOrder()
        {
            const Int32 sortOrder = 1;
            GetObject(true).SortOrder = sortOrder;
            Assert.AreEqual(GetObject().SortOrder, sortOrder);
        }

        [TestMethod]
        public void Importance()
        {
            const Int32 importance = 1;
            GetObject(true).Importance = importance;
            Assert.AreEqual(GetObject().Importance, importance);
        }

        [TestMethod]
        public void Remarks()
        {
            const String remarks = "This is excellent";
            GetObject(true).Remarks = remarks;
            Assert.AreEqual(GetObject().Remarks, remarks);
        }

        [TestMethod]
        public void IsAcceptedByTdwg()
        {
            const Boolean testBool = true;
            GetObject(true).IsAcceptedByTdwg = testBool;
            Assert.AreEqual(GetObject().IsAcceptedByTdwg, testBool);
        }

        [TestMethod]
        public void IsImplemented()
        {
            const Boolean testBool = true;
            GetObject(true).IsImplemented = testBool;
            Assert.AreEqual(GetObject().IsImplemented, testBool);
        }

        [TestMethod]
        public void IsPlanned()
        {
            const Boolean testBool = true;
            GetObject(true).IsPlanned = testBool;
            Assert.AreEqual(GetObject().IsPlanned, testBool);
        }

        [TestMethod]
        public void IsMandatory()
        {
            const Boolean testBool = true;
            GetObject(true).IsMandatory = testBool;
            Assert.AreEqual(GetObject().IsMandatory, testBool);
        }

        [TestMethod]
        public void IsMandatoryFromProvider()
        {
            const Boolean testBool = true;
            GetObject(true).IsMandatoryFromProvider = testBool;
            Assert.AreEqual(GetObject().IsMandatoryFromProvider, testBool);
        }

        [TestMethod]
        public void IsObtainedFromProvider()
        {
            const Boolean testBool = true;
            GetObject(true).IsObtainedFromProvider = testBool;
            Assert.AreEqual(GetObject().IsObtainedFromProvider, testBool);
        }

        [TestMethod]
        public void IsClassName()
        {
            const Boolean testBool = true;
            GetObject(true).IsClass = testBool;
            Assert.AreEqual(GetObject().IsClass, testBool);
        }

        [TestMethod]
        public void Mappings()
        {
            List<WebSpeciesObservationFieldMapping> mappings = new List<WebSpeciesObservationFieldMapping>();
            GetObject(true).Mappings = mappings;
            Assert.AreEqual(GetObject().Mappings, mappings);
        }
    }
}
