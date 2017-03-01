using System;
using System.Data;
using System.Collections.Generic;
using ArtDatabanken.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.WebService.ArtDatabankenService.Data;
using ArtDatabanken.WebService.ArtDatabankenService.Database;
using TaxonManager = ArtDatabanken.WebService.ArtDatabankenService.Data.TaxonManager;
using TaxonSearchScope = ArtDatabanken.WebService.ArtDatabankenService.Data.TaxonSearchScope;

namespace ArtDatabanken.WebService.ArtDatabankenService.Test.Data
{
    [TestClass]
    public class TaxonManagerTest : TestBase
    {
        public TaxonManagerTest()
            : base (false, 30)
        {
        }

        [TestMethod]
        public void AddUserSelectedTaxa()
        {
            foreach (UserSelectedTaxonUsage taxonUsage in Enum.GetValues(typeof(UserSelectedTaxonUsage)))
            {
                TaxonManager.AddUserSelectedTaxa(GetContext(), GetSomeTaxonIds(), taxonUsage);
                TaxonManager.DeleteUserSelectedTaxa(GetContext());
            }
        }

        [TestMethod]
        public void AddUserSelectedTaxonTypes()
        {
            foreach (UserSelectedTaxonTypeUsage taxonTypeUsage in Enum.GetValues(typeof(UserSelectedTaxonTypeUsage)))
            {
                TaxonManager.AddUserSelectedTaxonTypes(GetContext(),
                                                       GetTaxonTypeIds(),
                                                       taxonTypeUsage);
                TaxonManager.DeleteUserSelectedTaxonTypes(GetContext());
            }
        }

        [TestMethod]
        public void DeleteUserSelectedTaxa()
        {
            foreach (UserSelectedTaxonUsage taxonUsage in Enum.GetValues(typeof(UserSelectedTaxonUsage)))
            {
                TaxonManager.AddUserSelectedTaxa(GetContext(), GetSomeTaxonIds(), taxonUsage);
                TaxonManager.DeleteUserSelectedTaxa(GetContext());
            }
        }

        [TestMethod]
        public void DeleteUserSelectedTaxonTypes()
        {
            foreach (UserSelectedTaxonTypeUsage taxonTypeUsage in Enum.GetValues(typeof(UserSelectedTaxonTypeUsage)))
            {
                TaxonManager.AddUserSelectedTaxonTypes(GetContext(),
                                                       GetTaxonTypeIds(),
                                                       taxonTypeUsage);
                TaxonManager.DeleteUserSelectedTaxonTypes(GetContext());
            }
        }

        public static WebTaxonTreeNode GetBearTaxonTreeNode(WebServiceContext context)
        {
            List<Int32> taxonIds;
            List<WebTaxonTreeNode> taxonTrees;
            WebTaxonTreeSearchCriteria searchCriteria;

            searchCriteria = new WebTaxonTreeSearchCriteria();
            taxonIds = new List<Int32>();
            taxonIds.Add(BEAR_TAXON_ID);
            searchCriteria.RestrictSearchToTaxonIds = taxonIds;
            taxonTrees = TaxonManager.GetTaxonTreesBySearchCriteria(context, searchCriteria);
            return taxonTrees[0];
        }

        private WebDataQuery GetDataQuery(WebSpeciesFactCondition speciesFactCondition)
        {
            WebDataCondition dataCondition;
            WebDataQuery dataQuery;

            dataCondition = new WebDataCondition();
            dataCondition.SpeciesFactCondition = speciesFactCondition;
            dataQuery = new WebDataQuery();
            dataQuery.DataCondition = dataCondition;
            return dataQuery;
        }

        private WebDataQuery GetDataQuery(WebDataLogicCondition dataLogicCondition)
        {
            WebDataCondition dataCondition;
            WebDataQuery dataQuery;


            dataCondition = new WebDataCondition();
            dataCondition.DataLogicCondition = dataLogicCondition;
            dataQuery = new WebDataQuery();
            dataQuery.DataCondition = dataCondition;
            return dataQuery;
        }

        public static WebTaxonTreeNode GetHawkBirdsTaxonTree(WebServiceContext context)
        {
            List<Int32> taxonIds;
            List<WebTaxonTreeNode> taxonTrees;
            WebTaxonTreeSearchCriteria searchCriteria;

            searchCriteria = new WebTaxonTreeSearchCriteria();
            taxonIds = new List<Int32>();
            taxonIds.Add(HAWK_BIRDS_TAXON_ID);
            searchCriteria.RestrictSearchToTaxonIds = taxonIds;
            taxonTrees = TaxonManager.GetTaxonTreesBySearchCriteria(context, searchCriteria);
            return taxonTrees[0];
        }

        public static WebTaxon GetOneTaxon(WebServiceContext context)
        {
            Int32 taxonId;

            taxonId = BEAR_TAXON_ID;
            return TaxonManager.GetTaxon(context, taxonId, TaxonInformationType.Basic);
        }

        public static Int32 GetOneTaxonId()
        {
            return BEAR_TAXON_ID;
        }

        public static List<Int32> GetSomeTaxonIds()
        {
            return GetSomeTaxonIds(4);
        }

        public static List<Int32> GetSomeTaxonIds(Int32 taxonIdCount)
        {
            List<Int32> taxonIds;

            taxonIds = new List<Int32>();
            if (taxonIdCount > 0)
            {
                taxonIds.Add(BADGER_TAXON_ID);
            }
            if (taxonIdCount > 1)
            {
                taxonIds.Add(BEAVER_TAXON_ID);
            }
            if (taxonIdCount > 2)
            {
                taxonIds.Add(HEDGEHOG_TAXON_ID);
            }
            if (taxonIdCount > 3)
            {
                taxonIds.Add(FALLOW_DEER_TAXON_ID);
            }
            return taxonIds;
        }

        [TestMethod]
        public void GetTaxaById()
        {
            Boolean taxonFound;
            List<Int32> taxonIds;
            List<WebTaxon> taxa;
            List<WebTaxonName> taxonNames;
            WebTaxonNameSearchCriteria searchCriteria;

            foreach (TaxonInformationType taxonInformationType in Enum.GetValues(typeof(TaxonInformationType)))
            {
                taxa = TaxonManager.GetTaxaById(GetContext(), GetSomeTaxonIds(), taxonInformationType);
                Assert.IsNotNull(taxa);
                Assert.AreEqual(taxa.Count, GetSomeTaxonIds().Count);

                foreach (WebTaxon taxon in taxa)
                {
                    Assert.AreEqual(taxon.TaxonInformationType, taxonInformationType);

                    taxonFound = false;
                    foreach (Int32 taxonId in GetSomeTaxonIds())
                    {
                        if (taxonId == taxon.Id)
                        {
                            taxonFound = true;
                            break;
                        }
                    }
                    Assert.IsTrue(taxonFound);
                }
            }

            // Test that only valid relations in dt_hier are used.
            // Test taxon is 1097, Oedipodium Griffithianum which
            // has moved in the taxa tree more than once.

            taxonIds = new List<Int32>();
            taxonIds.Add(1097);
            taxa = TaxonManager.GetTaxaById(GetContext(), taxonIds, TaxonInformationType.PrintObs);
            Assert.IsNotNull(taxa);
            Assert.AreEqual(taxa.Count, taxonIds.Count);

            // Test problem with searching for taxon names equal to 'mossa'.
            searchCriteria = new WebTaxonNameSearchCriteria();
            searchCriteria.NameSearchString = "mossa";
            searchCriteria.NameSearchMethod = ArtDatabankenService.Data.SearchStringComparisonMethod.Iterative;
            taxonNames = TaxonManager.GetTaxonNamesBySearchCriteria(GetContext(), searchCriteria);
            taxonIds = new List<Int32>();
            foreach (WebTaxonName taxonName in taxonNames)
            {
                if (!taxonIds.Contains(taxonName.Taxon.Id))
                {
                    taxonIds.Add(taxonName.Taxon.Id);
                }
            }
            taxa = TaxonManager.GetTaxaById(GetContext(), taxonIds, TaxonInformationType.Basic);
            Assert.IsTrue(taxa.IsNotEmpty());

            // Test with invalid taxa.
            taxonIds = new List<Int32>();
            taxonIds.Add(190);
            taxonIds.Add(1637);
            taxonIds.Add(1638);
            taxa = TaxonManager.GetTaxaById(GetContext(), taxonIds, TaxonInformationType.Basic);
            Assert.IsTrue(taxa.IsNotEmpty());
            Assert.AreEqual(taxonIds.Count, taxa.Count);
        }

        [TestMethod]
        public void GetTaxaByOrganismOrRedlist()
        {
            List<WebTaxon> taxa;
            Int32 organismGroupId, endangeredListId, redlistCategoryId;

            foreach (TaxonInformationType taxonInformationType in Enum.GetValues(typeof(TaxonInformationType)))
            {
                organismGroupId = 5;
                endangeredListId = 1;
                redlistCategoryId = 0;
                taxa = TaxonManager.GetTaxaByOrganismOrRedlist(GetContext(),
                                                               true,
                                                               organismGroupId,
                                                               true,
                                                               endangeredListId,
                                                               true,
                                                               redlistCategoryId,
                                                               taxonInformationType);
                Assert.IsNotNull(taxa);
                Assert.IsTrue(taxa.IsNotEmpty());
            }
        }

        [TestMethod]
        public void GetTaxaByQuery()
        {
            Int32 factorIndex;
            List<WebFactor> factors;
            List<WebTaxon> taxa;
            WebDataLogicCondition dataLogicCondition;
            WebSpeciesFactCondition speciesFactCondition;

            // Test one condition and one factor.
            speciesFactCondition = new WebSpeciesFactCondition();
            speciesFactCondition.Factors = new WebFactor[1];
            speciesFactCondition.Factors[0] = FactorManagerTest.GetOneFactor(GetContext());
            taxa = TaxonManager.GetTaxaByQuery(GetContext(), GetDataQuery(speciesFactCondition), TaxonInformationType.Basic);
            Assert.IsTrue(taxa.IsNotEmpty());

            // Test one condition and several factors.
            speciesFactCondition = new WebSpeciesFactCondition();
            factors = FactorManagerTest.GetSomeFactors(GetContext());
            speciesFactCondition.Factors = new WebFactor[factors.Count];
            for (factorIndex = 0; factorIndex < speciesFactCondition.Factors.Length; factorIndex++)
            {
                speciesFactCondition.Factors[factorIndex] = factors[factorIndex];
            }
            taxa = TaxonManager.GetTaxaByQuery(GetContext(), GetDataQuery(speciesFactCondition), TaxonInformationType.Basic);
            Assert.IsTrue(taxa.IsNotEmpty());

            // Test several conditions and one factor.
            dataLogicCondition = new WebDataLogicCondition();
            dataLogicCondition.DataQueries = new List<WebDataQuery>();
            dataLogicCondition.Operator = DataLogicConditionOperatorId.And;
            factors = FactorManagerTest.GetSomeFactors(GetContext());
            for (factorIndex = 0; factorIndex < factors.Count; factorIndex++)
            {
                speciesFactCondition = new WebSpeciesFactCondition();
                speciesFactCondition.Factors = new WebFactor[1];
                speciesFactCondition.Factors[0] = factors[factorIndex];
                dataLogicCondition.DataQueries.Add(GetDataQuery(speciesFactCondition));
            }
            taxa = TaxonManager.GetTaxaByQuery(GetContext(), GetDataQuery(dataLogicCondition), TaxonInformationType.Basic);
            Assert.IsTrue(taxa.IsNotEmpty());

            // Test several conditions and several factors.
            dataLogicCondition = new WebDataLogicCondition();
            dataLogicCondition.DataQueries = new List<WebDataQuery>();
            dataLogicCondition.Operator = DataLogicConditionOperatorId.And;
            factors = FactorManagerTest.GetSomeFactors(GetContext());
            for (factorIndex = 0; factorIndex < factors.Count; factorIndex++)
            {
                speciesFactCondition = new WebSpeciesFactCondition();
                speciesFactCondition.Factors = new WebFactor[2];
                speciesFactCondition.Factors[0] = factors[factorIndex++];
                speciesFactCondition.Factors[1] = factors[factorIndex];
                dataLogicCondition.DataQueries.Add(GetDataQuery(speciesFactCondition));
            }
            taxa = TaxonManager.GetTaxaByQuery(GetContext(), GetDataQuery(dataLogicCondition), TaxonInformationType.Basic);
            Assert.IsTrue(taxa.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxaBySearchCriteria()
        {
            List<WebTaxon> taxa;
            List<Int32> taxonIds;
            List<Int32> taxonTypeIds;
            WebTaxonSearchCriteria searchCriteria;

            foreach (TaxonInformationType taxonInformationType in Enum.GetValues(typeof(TaxonInformationType)))
            {
                for (TaxonSearchScope taxonReturnScope = TaxonSearchScope.NoScope; taxonReturnScope <= TaxonSearchScope.AllChildTaxa; taxonReturnScope++)
                {
                    searchCriteria = new WebTaxonSearchCriteria();
                    searchCriteria.TaxonInformationType = taxonInformationType;
                    searchCriteria.RestrictReturnToScope = taxonReturnScope;

                    searchCriteria.RestrictSearchToTaxonIds = null;
                    searchCriteria.RestrictSearchToTaxonTypeIds = null;
                    searchCriteria.TaxonNameSearchString = "björn";
                    searchCriteria.RestrictSearchToSwedishSpecies = true;
                    taxa = TaxonManager.GetTaxaBySearchCriteria(GetContext(), searchCriteria);
                    Assert.IsTrue(taxa.IsNotEmpty());
                    Assert.AreEqual(taxa.Count, 1);

                    searchCriteria.RestrictSearchToTaxonIds = null;
                    searchCriteria.RestrictSearchToTaxonTypeIds = null;
                    searchCriteria.TaxonNameSearchString = "björn%";
                    searchCriteria.RestrictSearchToSwedishSpecies = false;
                    taxa = TaxonManager.GetTaxaBySearchCriteria(GetContext(), searchCriteria);
                    Assert.IsNotNull(taxa);
                    Assert.IsTrue(taxa.IsNotEmpty());
                    Assert.IsTrue(taxa.Count > 1);

                    searchCriteria.RestrictSearchToTaxonIds = GetSomeTaxonIds();
                    searchCriteria.RestrictSearchToTaxonTypeIds = null;
                    searchCriteria.TaxonNameSearchString = "bäver%";
                    searchCriteria.RestrictSearchToSwedishSpecies = true;
                    taxa = TaxonManager.GetTaxaBySearchCriteria(GetContext(), searchCriteria);
                    Assert.IsNotNull(taxa);
                    Assert.IsTrue(taxa.IsNotEmpty());
                    Assert.AreEqual(taxa.Count, 1);

                    searchCriteria.RestrictSearchToTaxonIds = null;
                    searchCriteria.RestrictSearchToTaxonTypeIds = null;
                    searchCriteria.TaxonNameSearchString = "blåvingad sandgräshoppa";
                    searchCriteria.RestrictReturnToSwedishSpecies = true;
                    taxa = TaxonManager.GetTaxaBySearchCriteria(GetContext(), searchCriteria);
                    Assert.IsTrue(taxa.IsEmpty());

                    searchCriteria.RestrictSearchToTaxonIds = null;
                    searchCriteria.RestrictSearchToTaxonTypeIds = GetTaxonTypeIds();
                    searchCriteria.TaxonNameSearchString = "%björn%";
                    searchCriteria.RestrictSearchToSwedishSpecies = true;
                    taxa = TaxonManager.GetTaxaBySearchCriteria(GetContext(), searchCriteria);
                    Assert.IsNotNull(taxa);
                    Assert.IsTrue(taxa.IsNotEmpty());
                    Assert.IsTrue(taxa.Count > 1);
                }
            }
            
            // Test bugg were not valid taxa
            // (not valid today but has been valid before)
            // is included in the result.
            searchCriteria = new WebTaxonSearchCriteria();
            searchCriteria.RestrictReturnToScope = TaxonSearchScope.AllChildTaxa;
            taxonIds = new List<Int32>();
            taxonIds.Add(217177); // Vägbjörnmossa
            searchCriteria.RestrictSearchToTaxonIds = taxonIds;
            taxonTypeIds = new List<Int32>();
            taxonTypeIds.Add(SPECIES_TAXON_TYPE_ID);
            searchCriteria.RestrictReturnToTaxonTypeIds = taxonTypeIds;
            taxa = TaxonManager.GetTaxaBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsNotNull(taxa);
            Assert.IsTrue(taxa.IsEmpty());

            // Test getting parent taxa.
            searchCriteria = new WebTaxonSearchCriteria();
            searchCriteria.TaxonInformationType = TaxonInformationType.Basic;
            searchCriteria.RestrictReturnToScope = TaxonSearchScope.AllParentTaxa;
            searchCriteria.TaxonNameSearchString = "björn";
            taxa = TaxonManager.GetTaxaBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(taxa.IsNotEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetTaxaBySearchCriteriaNullError()
        {
            List<WebTaxon> taxa;

            taxa = TaxonManager.GetTaxaBySearchCriteria(GetContext(), null);
            Assert.IsTrue(taxa.IsNotEmpty());
        }

        public static DataTable GetUserSelectedTaxa(WebServiceContext context)
        {
            return GetUserSelectedTaxa(context, GetSomeTaxonIds(2));
        }

        public static DataTable GetUserSelectedTaxa(WebServiceContext context,
                                                    List<Int32> taxaIds)
        {
            DataColumn column;
            DataRow row;
            DataTable taxonTable;

            taxonTable = new DataTable(UserSelectedTaxaData.TABLE_NAME);
            column = new DataColumn(UserSelectedTaxaData.REQUEST_ID, typeof(Int32));
            taxonTable.Columns.Add(column);
            column = new DataColumn(UserSelectedTaxaData.TAXON_ID, typeof(Int32));
            taxonTable.Columns.Add(column);
            column = new DataColumn(UserSelectedTaxaData.TAXON_USAGE, typeof(String));
            taxonTable.Columns.Add(column);
            foreach (Int32 taxonId in taxaIds)
            {
                row = taxonTable.NewRow();
                row[0] = context.RequestId;
                row[1] = taxonId;
                row[2] = UserSelectedTaxonUsage.Input.ToString();
                taxonTable.Rows.Add(row);
            }
            return taxonTable;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetTaxaByIdTaxonIdsEmptyError()
        {
            List<WebTaxon> taxa;
            List<Int32> taxonIds;

            taxonIds = new List<Int32>();
            taxa = TaxonManager.GetTaxaById(GetContext(), taxonIds, TaxonInformationType.Basic);
            Assert.IsTrue(taxa.IsNotEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetTaxaByIdTaxonIdsNullError()
        {
            List<WebTaxon> taxa;

            taxa = TaxonManager.GetTaxaById(GetContext(), null, TaxonInformationType.Basic);
            Assert.IsTrue(taxa.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxon()
        {
            Int32 taxonId;
            WebTaxon taxon;

            taxonId = (Int32)(TaxonId.Bear);
            foreach (TaxonInformationType taxonInformationType in Enum.GetValues(typeof(TaxonInformationType)))
            {
                taxon = TaxonManager.GetTaxon(GetContext(), taxonId, taxonInformationType);
                Assert.IsNotNull(taxon);
                Assert.AreEqual(taxon.Id, taxonId);
                Assert.AreEqual(taxon.TaxonInformationType, taxonInformationType);
            }

            // Test error where a valid taxon is not returned as result.
            // Taxon is 3000176, Orthoptera, hopprätvingar.
            taxonId = 3000176;
            taxon = TaxonManager.GetTaxon(GetContext(), taxonId, TaxonInformationType.PrintObs);
            Assert.IsNotNull(taxon);
            Assert.AreEqual(taxon.Id, taxonId);

            // Test problem with dummy taxon (Id = 0)
            taxonId = 0;
            taxon = TaxonManager.GetTaxon(GetContext(), taxonId, TaxonInformationType.PrintObs);
            Assert.IsNotNull(taxon);
            Assert.AreEqual(taxon.Id, taxonId);

            // Get invalid taxon.
            taxonId = 190;
            taxon = TaxonManager.GetTaxon(GetContext(), taxonId, TaxonInformationType.Basic);
            Assert.IsNotNull(taxon);
            Assert.AreEqual(taxon.Id, taxonId);
        }

        public static WebTaxonName GetTaxonName(WebServiceContext context)
        {
            List<WebTaxonName> taxonNames;
            WebTaxonNameSearchCriteria searchCriteria;

            searchCriteria = new WebTaxonNameSearchCriteria();
            searchCriteria.NameSearchString = "björn";
            taxonNames = TaxonManager.GetTaxonNamesBySearchCriteria(context, searchCriteria);
            return taxonNames[0];
        }

        [TestMethod]
        public void GetTaxonNames()
        {
            Int32 taxonId;
            List<WebTaxonName> taxonNames;

            taxonId = GetOneTaxonId();
            taxonNames = TaxonManager.GetTaxonNames(GetContext(), taxonId);
            Assert.IsTrue(taxonNames.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxonNamesBySearchCriteria()
        {
            List<WebTaxonName> taxonNames;
            WebTaxonNameSearchCriteria searchCriteria;

            foreach (ArtDatabankenService.Data.SearchStringComparisonMethod nameSearchMethod in Enum.GetValues(typeof(ArtDatabankenService.Data.SearchStringComparisonMethod)))
            {
                searchCriteria = new WebTaxonNameSearchCriteria();
                searchCriteria.NameSearchString = "björn";
                searchCriteria.NameSearchMethod = nameSearchMethod;
                taxonNames = TaxonManager.GetTaxonNamesBySearchCriteria(GetContext(), searchCriteria);
                Assert.IsTrue(taxonNames.IsNotEmpty());
            }

            // Test retrieval of name for not valid taxon.
            // The taxon name occurs twice, with relation
            // to both valid and invalid taxon.
            searchCriteria = new WebTaxonNameSearchCriteria();
            searchCriteria.NameSearchString = "fläckig björnspinnare";
            taxonNames = TaxonManager.GetTaxonNamesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsTrue(taxonNames.IsNotEmpty());
            Assert.AreEqual(1, taxonNames.Count);
            Assert.AreEqual(searchCriteria.NameSearchString, taxonNames[0].Name);
        }

        public static WebTaxonNameType GetTaxonNameType(WebServiceContext context)
        {
            return TaxonManager.GetTaxonNameTypes(context)[0];
        }

        [TestMethod]
        public void GetTaxonNameTypes()
        {
            List<WebTaxonNameType> taxonNameTypes;

            taxonNameTypes = TaxonManager.GetTaxonNameTypes(GetContext());
            Assert.IsNotNull(taxonNameTypes);
            Assert.IsTrue(taxonNameTypes.IsNotEmpty());
        }

        public static WebTaxonNameUseType GetTaxonNameUseType(WebServiceContext context)
        {
            return TaxonManager.GetTaxonNameUseTypes(context)[0];
        }

        [TestMethod]
        public void GetTaxonNameUseTypes()
        {
            List<WebTaxonNameUseType> taxonNameUseTypes;

            taxonNameUseTypes = TaxonManager.GetTaxonNameUseTypes(GetContext());
            Assert.IsNotNull(taxonNameUseTypes);
            Assert.IsTrue(taxonNameUseTypes.IsNotEmpty());
        }

        [TestMethod]
        public void GetTaxonTreesBySearchCriteria()
        {
            List<Int32> taxonIds;
            List<WebTaxonTreeNode> taxonTrees;
            TaxonInformationType taxonInformationType;
            WebTaxonTreeSearchCriteria searchCriteria;

            taxonIds = new List<Int32>();
            taxonIds.Add(HAWK_BIRDS_TAXON_ID);
            taxonIds.Add((Int32)(TaxonId.Bears));
            taxonInformationType = TaxonInformationType.Basic;

            searchCriteria = new WebTaxonTreeSearchCriteria();
            searchCriteria.TaxonInformationType = taxonInformationType;

            taxonTrees = TaxonManager.GetTaxonTreesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsNotNull(taxonTrees);
            Assert.IsTrue(taxonTrees.IsNotEmpty());

            searchCriteria.RestrictSearchToTaxonTypeIds = GetTaxonTypeIds();
            taxonTrees = TaxonManager.GetTaxonTreesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsNotNull(taxonTrees);
            Assert.IsTrue(taxonTrees.IsNotEmpty());
            searchCriteria.RestrictSearchToTaxonTypeIds = null;

            searchCriteria.RestrictSearchToTaxonIds = taxonIds;
            taxonTrees = TaxonManager.GetTaxonTreesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsNotNull(taxonTrees);
            Assert.AreEqual(taxonTrees.Count, taxonIds.Count);

            searchCriteria.RestrictSearchToTaxonTypeIds = GetTaxonTypeIds();
            taxonTrees = TaxonManager.GetTaxonTreesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsNotNull(taxonTrees);
            Assert.IsTrue(taxonTrees.IsNotEmpty());

            // Test with root taxon.
            searchCriteria = new WebTaxonTreeSearchCriteria();
            searchCriteria.TaxonInformationType = TaxonInformationType.Basic;
            taxonIds = new List<Int32>();
            taxonIds.Add(ROOT_TAXON_ID);
            searchCriteria.RestrictSearchToTaxonIds = taxonIds;
            taxonTrees = TaxonManager.GetTaxonTreesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsNotNull(taxonTrees);

            // Test with more than one root taxon.
            taxonIds = new List<Int32>();
            taxonIds.Add(HAWK_BIRDS_TAXON_ID);
            taxonIds.Add((Int32)(ArtDatabanken.Data.TaxonId.Mammals));
            taxonInformationType = TaxonInformationType.Basic;
            searchCriteria = new WebTaxonTreeSearchCriteria();
            searchCriteria.RestrictSearchToTaxonIds = taxonIds;
            searchCriteria.TaxonInformationType = taxonInformationType;
            taxonTrees = TaxonManager.GetTaxonTreesBySearchCriteria(GetContext(), searchCriteria);
            Assert.IsNotNull(taxonTrees);
            Assert.AreEqual(taxonIds.Count, taxonTrees.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetTaxonTreesBySearchCriteriaNullError()
        {
            List<WebTaxonTreeNode> taxonTrees;

            taxonTrees = TaxonManager.GetTaxonTreesBySearchCriteria(GetContext(), null);
            Assert.IsTrue(taxonTrees.IsNotEmpty());
        }

        public static WebTaxonType GetTaxonType(WebServiceContext context)
        {
            return TaxonManager.GetTaxonTypes(context)[3];
        }

        public static List<Int32> GetTaxonTypeIds()
        {
            List<Int32> taxonTypeIds;

            taxonTypeIds = new List<Int32>();
            taxonTypeIds.Add(SPECIES_TAXON_TYPE_ID);
            taxonTypeIds.Add(GENUS_TAXON_TYPE_ID);
            taxonTypeIds.Add(FAMILY_TAXON_TYPE_ID);
            return taxonTypeIds;
        }

        [TestMethod]
        public void GetTaxonTypes()
        {
            List<WebTaxonType> taxonTypes;

            taxonTypes = TaxonManager.GetTaxonTypes(GetContext());
            Assert.IsNotNull(taxonTypes);
            Assert.IsTrue(taxonTypes.IsNotEmpty());
        }

        [TestMethod]
        public void GetHostTaxa()
        {
            List<WebTaxon> taxa;
            Int32 factorId;

            foreach (TaxonInformationType taxonInformationType in Enum.GetValues(typeof(TaxonInformationType)))
            { 
                factorId = 1142;
                taxa = TaxonManager.GetHostTaxa(GetContext(), factorId, taxonInformationType);
                Assert.IsNotNull(taxa);
                Assert.IsTrue(taxa.Count > 10);
          
            }
        }

        [TestMethod]
        public void GetHostTaxaByTaxonId()
        {
            List<WebTaxon> taxa;
            Int32 taxonId;

            foreach (TaxonInformationType taxonInformationType in Enum.GetValues(typeof(TaxonInformationType)))
            {
                taxonId = 101656; //Trummgräshoppa
                taxa = TaxonManager.GetHostTaxaByTaxonId(GetContext(), taxonId, taxonInformationType);
                Assert.IsNotNull(taxa);
                Assert.IsTrue(taxa.Count > 5);

            }
        }

        [TestMethod]
        public void GetTaxaByHostTaxonId()
        {
            List<WebTaxon> taxa;
            Int32 hostTaxonId;

            foreach (TaxonInformationType taxonInformationType in Enum.GetValues(typeof(TaxonInformationType)))
            {
                hostTaxonId = 1006592; //Salix
                taxa = TaxonManager.GetTaxaByHostTaxonId(GetContext(), hostTaxonId, taxonInformationType);
                Assert.IsNotNull(taxa);
                Assert.IsTrue(taxa.Count > 25);

            }
        }

        public static DataTable GetUserSelectedTaxonTypes(WebServiceContext context)
        {
            DataColumn column;
            DataRow row;
            DataTable taxonTypesTable;

            taxonTypesTable = new DataTable(UserSelectedTaxonTypesData.TABLE_NAME);
            column = new DataColumn(UserSelectedTaxonTypesData.REQUEST_ID, typeof(Int32));
            taxonTypesTable.Columns.Add(column);
            column = new DataColumn(UserSelectedTaxonTypesData.TAXON_TYPE_ID, typeof(Int32));
            taxonTypesTable.Columns.Add(column);
            column = new DataColumn(UserSelectedTaxonTypesData.TAXON_TYPE_USAGE, typeof(String));
            taxonTypesTable.Columns.Add(column);
            row = taxonTypesTable.NewRow();
            row[0] = context.RequestId;
            row[1] = SPECIES_TAXON_TYPE_ID;
            row[2] = UserSelectedTaxonTypeUsage.Input.ToString();
            taxonTypesTable.Rows.Add(row);
            row = taxonTypesTable.NewRow();
            row[0] = context.RequestId;
            row[1] = GENUS_TAXON_TYPE_ID;
            row[2] = UserSelectedTaxonTypeUsage.Input.ToString();
            taxonTypesTable.Rows.Add(row);
            return taxonTypesTable;
        }

        [TestMethod]
        public void UpdateTaxonInformation()
        {
            // This test takes about 15 minutes to run on moneses-dev.
            if (Configuration.IsAllTestsRun)
            {
                TaxonManager.UpdateTaxonInformation();
            }
        }
    }
}
