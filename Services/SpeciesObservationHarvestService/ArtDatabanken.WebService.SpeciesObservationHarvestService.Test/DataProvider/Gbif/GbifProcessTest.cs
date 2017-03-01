using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Gbif;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Test.DataProvider.Gbif
{
    [TestClass]
    public class GbifProcessTest : TestBase
    {
        [TestMethod]
        public void GetDyntaxaTaxonId()
        {
            Dictionary<string, WebDataField> dictionaryWebData = new Dictionary<string, WebDataField>();
            WebDataField dataField = new WebDataField();
            dataField.Name = "scientificname";
            dataField.Type = WebDataType.String;
            dataField.Value = "Timmia bavarica";
            dictionaryWebData[dataField.Name] = dataField;

            GbifProcess gbifProcess = new GbifProcess();
            string taxonId = gbifProcess.GetTaxonId(dictionaryWebData, GetContext());
            Assert.IsTrue(taxonId.IsNotEmpty());
        }
    }
}
