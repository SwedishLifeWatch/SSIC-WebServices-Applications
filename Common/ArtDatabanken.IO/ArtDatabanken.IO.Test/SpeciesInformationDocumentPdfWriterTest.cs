using System;
using System.IO;
using System.Diagnostics;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data.ArtDatabankenService;
using ArtDatabanken.IO;

namespace ArtDatabanken.IO.Test
{
    [TestClass]
    public class SpeciesInformationDocumentPdfWriterTest : TestBase
    {
        [TestMethod]
        public void TestMethod1()
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
        public void ConstructorTest()
        {
            Int32 taxonId = 100046; // vitrygg
            Data.ArtDatabankenService.SpeciesInformationDocument document = new Data.ArtDatabankenService.SpeciesInformationDocument(taxonId);
            String templateFileName = @"C:\Dev\ArtDatabanken\ArtDatabanken.IO\Resources\Artfaktablad.dotx";

            //Retrieve county map image based on URL:
            WebRequest requestPic5 = WebRequest.Create("http://www.artfakta.se/Bilder/Lansforekomst/Dendrocopos_Leucotos_100046.png");
            requestPic5.Timeout = 5000;
            WebResponse responsePic5 = null;
            System.Drawing.Image mapImage = null;

            if (requestPic5 != null)
            {
                responsePic5 = requestPic5.GetResponse();
                if (responsePic5 != null)
                {
                    mapImage = System.Drawing.Image.FromStream(responsePic5.GetResponseStream());
                }
            }
            
            String mapImageFileName = @"C:\Users\oskark\Desktop\testMap.png";
            mapImage.Save(@"C:\Users\oskark\Desktop\testMap.png");

            //Retrieve foto based on URL:
            requestPic5 = null;
            requestPic5 = WebRequest.Create("http://nordmyran.se/wp-content/uploads/2008/11/vitrygg-fredrik-wilde.jpg");
            requestPic5.Timeout = 5000;
            responsePic5 = null;
            System.Drawing.Image taxonImage = null;

            if (requestPic5 != null)
            {
                responsePic5 = requestPic5.GetResponse();
                if (responsePic5 != null)
                {
                    taxonImage = System.Drawing.Image.FromStream(responsePic5.GetResponseStream());
                }
            }
            requestPic5 = null;
            String taxonImageFileName = @"C:\Users\oskark\Desktop\testPicture.jpg";
            taxonImage.Save(@"C:\Users\oskark\Desktop\testPicture.jpg");
            
            SpeciesInformationDocumentPdfWriter writer = new SpeciesInformationDocumentPdfWriter(document, templateFileName, mapImageFileName, taxonImageFileName, "");
            Assert.IsNotNull(writer);

            ///The code below is recommended for debug only.
            
            // Other users change path eg. @"C:\temp\testMap.png";
     /*
            String fileName = @"C:\Users\oskark\Desktop\TestPdfWriter.pdf";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            writer.SavePdf(fileName);

            Process.Start(fileName);*/
  
        }

        [TestMethod]
        public void ConstructorWithImagesTest()
        {
            Int32 taxonId = 100046; // vitrygg
            Data.ArtDatabankenService.SpeciesInformationDocument document = new Data.ArtDatabankenService.SpeciesInformationDocument(taxonId);
            String templateFileName = @"C:\Dev\ArtDatabanken\ArtDatabanken.IO\Resources\Artfaktablad.dotx";

            //Retrieve county map image based on URL:
            WebRequest requestPic5 = WebRequest.Create("http://www.artfakta.se/Bilder/Lansforekomst/Dendrocopos_Leucotos_100046.png");
            requestPic5.Timeout = 5000;
            WebResponse responsePic5 = null;
            System.Drawing.Image mapImage = null;

            if (requestPic5 != null)
            {
                responsePic5 = requestPic5.GetResponse();
                if (responsePic5 != null)
                {
                    mapImage = System.Drawing.Image.FromStream(responsePic5.GetResponseStream());
                }
            }
            

            //Retrieve foto based on URL:
            requestPic5 = null;
            requestPic5 = WebRequest.Create("http://nordmyran.se/wp-content/uploads/2008/11/vitrygg-fredrik-wilde.jpg");
            requestPic5.Timeout = 5000;
            responsePic5 = null;
            System.Drawing.Image taxonImage = null;

            if (requestPic5 != null)
            {
                responsePic5 = requestPic5.GetResponse();
                if (responsePic5 != null)
                {
                    taxonImage = System.Drawing.Image.FromStream(responsePic5.GetResponseStream());
                }
            }
            requestPic5 = null;

            SpeciesInformationDocumentPdfWriter writer = new SpeciesInformationDocumentPdfWriter(document, templateFileName, mapImage, taxonImage, "");
            Assert.IsNotNull(writer);

            ///The code below is recommended for debug only.
            /*
            String fileName = @"C:\Users\oskark\Desktop\TestPdfWriter.pdf";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            writer.SavePdf(fileName);

            Process.Start(fileName);*/
        }

        [TestMethod]
        public void ConstructorNoImagesTest()
        {
            //Int32 taxonId = 101656; // trummgräshoppa
            Int32 taxonId = 696; //gentiana
            Data.ArtDatabankenService.SpeciesInformationDocument document = new Data.ArtDatabankenService.SpeciesInformationDocument(taxonId);
            String templateFileName = @"C:\Dev\ArtDatabanken\ArtDatabanken.IO\Resources\Artfaktablad.dotx";

            SpeciesInformationDocumentPdfWriter writer = new SpeciesInformationDocumentPdfWriter(document, templateFileName, "", "", "");
            Assert.IsNotNull(writer);

            ///The code below is recommended for debug only.
            /*
            String fileName = @"C:\Users\oskark\Desktop\TestPdfWriter.pdf";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            writer.SavePdf(fileName);

            Process.Start(fileName);
             * */
        }

        [TestMethod]
        public void ConstructorLavarTest()
        {
            Int32 taxonId = 188; //långt broktagel
            Data.ArtDatabankenService.SpeciesInformationDocument document = new Data.ArtDatabankenService.SpeciesInformationDocument(taxonId);
            String templateFileName = @"C:\Dev\ArtDatabanken\ArtDatabanken.IO\Resources\Artfaktablad.dotx";

            SpeciesInformationDocumentPdfWriter writer = new SpeciesInformationDocumentPdfWriter(document, templateFileName, "", "", "");
            Assert.IsNotNull(writer);

            ///The code below is recommended for debug only.
            /*
            String fileName = @"C:\Users\oskark\Desktop\TestPdfWriter.pdf";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            writer.SavePdf(fileName);

            Process.Start(fileName);*/
 
        }
    }
}
