using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.IO.Test
{
    [TestClass]
    public class SpeciesnInformationDocumentWriterTest : TestBase
    {
        public SpeciesnInformationDocumentWriterTest()
        {
        }

        #region Additional test attributes
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #endregion

        [TestMethod]
        public void SaveFileTest()
        {
            //Int32 taxonId = 101656; //trumgräshoppa
            Int32 taxonId = 100046; // vitrygg
            SpeciesInformationDocument document = new SpeciesInformationDocument(GetUserContext(), taxonId);

            SpeciesInformationDocumentWriter writer = new SpeciesInformationDocumentWriter(document);
            Assert.IsNotNull(writer);

            ///The code below is recommended for debug only.
            /*                   
            String fileName = "TestPdfWriter.pdf";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            writer.SavePdf(fileName);
             
            Process.Start(fileName);
             * */
        }
    }
}
