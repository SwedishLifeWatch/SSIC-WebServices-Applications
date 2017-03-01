using ArtDatabanken.WebApplication.AnalysisPortal.Result.CalculatedData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Test.Result.CalculatedData
{
    [TestClass]
    public class CalculatedDataItemTests
    {
        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void AddAndGetCalculatedDataItem_GenericLocale_DataIsReturned()
        {
            CalculatedDataItemCollection calculatedDataItemCollection = new CalculatedDataItemCollection();

            const string strData = "Testing";
            calculatedDataItemCollection.AddCalculatedDataItem(new CalculatedDataItemCacheKey(CalculatedDataItemType.GridCellTaxa, null), strData);
            CalculatedDataItem<string> calculatedDataItem = calculatedDataItemCollection.GetCalculatedDataItem<string>(new CalculatedDataItemCacheKey(CalculatedDataItemType.GridCellTaxa, null));

            Assert.AreEqual("Testing", calculatedDataItem.Data);
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void AddAndGetCalculatedDataItem_NullAndEmptyStringLocale_NullAndEmptyStringLocalesAreTreatedSame()
        {
            CalculatedDataItemCollection calculatedDataItemCollection = new CalculatedDataItemCollection();

            const string strData = "Testing";
            calculatedDataItemCollection.AddCalculatedDataItem(new CalculatedDataItemCacheKey(CalculatedDataItemType.GridCellTaxa, null), strData);
            CalculatedDataItem<string> calculatedDataItem = calculatedDataItemCollection.GetCalculatedDataItem<string>(new CalculatedDataItemCacheKey(CalculatedDataItemType.GridCellTaxa, ""));

            Assert.AreEqual("Testing", calculatedDataItem.Data);
        }

        [TestMethod]
        [TestCategory("UnitTestApp")]
        public void AddAndGetCalculatedDataItem_AddWithSwedishLocaleGetWithSwedishLocale_DataIsReturned()
        {
            const string strData = "Testing";
            const string addIsoCode = "sv";
            const string getIsoCode = "sv";
            CalculatedDataItemCollection calculatedDataItemCollection = new CalculatedDataItemCollection();
            
            calculatedDataItemCollection.AddCalculatedDataItem(CalculatedDataItemType.GridCellTaxa, addIsoCode, strData);            
            CalculatedDataItem<string> calculatedDataItem = calculatedDataItemCollection.GetCalculatedDataItem<string>(CalculatedDataItemType.GridCellTaxa, getIsoCode);

            Assert.AreEqual("Testing", calculatedDataItem.Data);
        }    

        [TestMethod]
        [TestCategory("NightlyTestApp")]
        public void AddAndGetCalculatedDataItem_AddWithSwedishLocaleGetWithEnglishLocale_DataIsEmpty()
        {
            const string strData = "Testing";
            const string addIsoCode = "sv";
            const string getIsoCode = "en";
            CalculatedDataItemCollection calculatedDataItemCollection = new CalculatedDataItemCollection();
            
            calculatedDataItemCollection.AddCalculatedDataItem(CalculatedDataItemType.GridCellTaxa, addIsoCode, strData);            
            CalculatedDataItem<string> calculatedDataItem = calculatedDataItemCollection.GetCalculatedDataItem<string>(CalculatedDataItemType.GridCellTaxa, getIsoCode);

            Assert.IsNull(calculatedDataItem.Data);
        }
    }
}