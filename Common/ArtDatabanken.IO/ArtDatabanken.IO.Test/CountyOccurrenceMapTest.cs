using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ArtDatabanken.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MigraDoc.DocumentObjectModel.Shapes;

namespace ArtDatabanken.IO.Test
{
    /// <summary>
    /// Summary description for SpeciesCountyMapWriter
    /// </summary>
    [TestClass]
    public class CountyOccurrenceMapTest : TestBase
    {
        private TestContext testContextInstance;

        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
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


        [TestMethod]        
        public void CountyMapFirstConstructorTest()
        {
            IUserContext userContext = LoginTestUser();            
            ITaxon taxon = CoreData.TaxonManager.GetTaxon(userContext, 101656);
            string shapeFileName = Path.Combine(AbsoluteResourceFolderPath, "Sverigekarta med län.shp");
            Console.WriteLine("Reading file: {0}", shapeFileName);
            CountyOccurrenceMap.InitializeMap(shapeFileName);

            CountyOccurrenceMap countyOccurenceMap = new CountyOccurrenceMap(userContext, taxon);

            string image1Path = Path.Combine(AbsoluteTempFolderPath, "testMapWithLegend.png");
            Console.WriteLine("Generating image: {0}", image1Path);
            countyOccurenceMap.Save(image1Path, System.Drawing.Imaging.ImageFormat.Png);

            countyOccurenceMap.Height = 200;
            string image2Path = Path.Combine(AbsoluteTempFolderPath, "testHeight200MapWithLegend.png");
            Console.WriteLine("Generating image: {0}", image2Path);
            countyOccurenceMap.Save(image2Path, System.Drawing.Imaging.ImageFormat.Png);
        }

        [TestMethod]
        public void CountyMapSecondConstructorTest()
        {            
            IUserContext userContext = LoginTestUser();
            IFactorSearchCriteria factorSearchCrieteria = new FactorSearchCriteria();
            List<Int32> countyIds = new List<Int32>();
            countyIds.Add((Int32)FactorId.CountyOccurrence);
            factorSearchCrieteria.RestrictSearchToFactorIds = countyIds;
            factorSearchCrieteria.RestrictReturnToScope = ArtDatabanken.Data.FactorSearchScope.LeafFactors;
            FactorList counties = CoreData.FactorManager.GetFactors(userContext, factorSearchCrieteria);
            ISpeciesFactSearchCriteria parameters = new SpeciesFactSearchCriteria();
            ITaxon taxon = CoreData.TaxonManager.GetTaxon(userContext, 101656);
            parameters.Taxa = new TaxonList { taxon };
            parameters.Factors = counties;
            parameters.IncludeNotValidHosts = true;
            parameters.IncludeNotValidTaxa = true;
            SpeciesFactList countyInformation = CoreData.SpeciesFactManager.GetSpeciesFacts(userContext, parameters);

            string shapeFileName = Path.Combine(AbsoluteResourceFolderPath, "Sverigekarta med län.shp");
            Console.WriteLine("Reading file: {0}", shapeFileName);
            CountyOccurrenceMap.InitializeMap(shapeFileName);
            CountyOccurrenceMap countyOccurenceMap = new CountyOccurrenceMap(countyInformation);

            string imagePath = Path.Combine(AbsoluteTempFolderPath, "testSpeciesFactMapWithLegend.png");
            Console.WriteLine("Generating image: {0}", imagePath);
            countyOccurenceMap.Save(imagePath, System.Drawing.Imaging.ImageFormat.Png);
        }


        [TestMethod]        
        public void CountyMapNoDataTest()
        {
            SpeciesFactList facts = new SpeciesFactList();
            CountyOccurrenceMap countyOccurenceMap = new CountyOccurrenceMap(facts);
            string shapeFileName = Path.Combine(AbsoluteResourceFolderPath, "Sverigekarta med län.shp");
            Console.WriteLine("Reading file: {0}", shapeFileName);
            CountyOccurrenceMap.InitializeMap(shapeFileName);

            string imagePath1 = Path.Combine(AbsoluteTempFolderPath, "testNoDataMapWithLegend.png");
            Console.WriteLine("Generating image: {0}", imagePath1);
            countyOccurenceMap.Save(imagePath1, System.Drawing.Imaging.ImageFormat.Png);            

            countyOccurenceMap.Height = 200;
            string imagePath2 = Path.Combine(AbsoluteTempFolderPath, "testHeight200MapWithLegend.png");
            Console.WriteLine("Generating image: {0}", imagePath2);
            countyOccurenceMap.Save(imagePath2, System.Drawing.Imaging.ImageFormat.Png);            
        }
    }
}
