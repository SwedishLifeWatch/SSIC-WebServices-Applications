using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservation.Data;
using ArtDatabanken.WebService.SpeciesObservationHarvestService.Database;
using LogType = ArtDatabanken.WebService.Data.LogType;
using WebTaxon = ArtDatabanken.WebService.Data.WebTaxon;
using WebTaxonName = ArtDatabanken.WebService.Data.WebTaxonName;
using WebTaxonNameSearchCriteria = ArtDatabanken.WebService.Data.WebTaxonNameSearchCriteria;
using WebTaxonTreeNode = ArtDatabanken.WebService.Data.WebTaxonTreeNode;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Data
{
    /// <summary>
    /// The harvest manager.
    /// </summary>
    public partial class HarvestManager
    {

        /// <summary>
        /// Update taxon information in database.
        /// New information is read from web service TaxonService.
        /// </summary>
        /// <param name="context">
        /// User context.
        /// </param>
        /// <param name="taxons">
        /// The taxon.
        /// </param>
        /// <param name="taxonTrees">
        /// Contains information about all valid taxon trees.
        /// </param>
        /// <param name="taxonRemarks">
        /// The taxon remarks.
        /// </param>
        private static void UpdateTempTaxonInformation(
            WebServiceContext context,
            List<WebTaxon> taxons,
            IEnumerable<WebTaxonTreeNode> taxonTrees,
            Dictionary<Int32, String> taxonRemarks)
        {
            Dictionary<Int32, TaxonInformation> taxonInformations;
            TaxonInformation taxonInformation;
            
            ResetTaxaCache();
            foreach (var taxonTreeNode in taxonTrees)
            {
                AddTaxaToCache(taxonTreeNode);
            }

            //load taxon tree with only main parents
            var mainParentsTaxonTree = WebServiceData.TaxonManager.GetTaxonTreesBySearchCriteria(context, new WebTaxonTreeSearchCriteria { IsMainRelationRequired = true, IsValidRequired = true });
            var dictionaryTaxonTreeNode = ConvertTreeNodeStructure(mainParentsTaxonTree);
            taxonInformations = GetSpeciesFactInformation(context);

            // Create worktable.
            using (var taxaTable = CreateTaxaTable())
            {
                // Add information into table (only once per taxonId)
                var taxonIds = new DataIdInt32List(true);
                foreach (var taxon in taxons)
                {
                    if (taxonIds.Contains(taxon.Id))
                    {
                        continue;
                    }
                    else
                    {
                        taxonIds.Add(taxon.Id);
                    }

                    if (taxonInformations.ContainsKey(taxon.Id))
                    {
                        taxonInformation = taxonInformations[taxon.Id];
                    }
                    else
                    {
                        taxonInformation = new TaxonInformation();
                        taxonInformation.ActionPlanId = 0;
                        taxonInformation.ConservationRelevant = false;
                        taxonInformation.DisturbanceRadius = 0;
                        taxonInformation.DyntaxaTaxonId = taxon.Id;
                        taxonInformation.Natura2000 = false;
                        taxonInformation.ProtectedByLaw = false;
                        taxonInformation.ProtectionLevel = 0;
                        taxonInformation.RedlistCategory = String.Empty;
                        taxonInformation.SwedishImmigrationHistory = String.Empty;
                        taxonInformation.SwedishOccurrence = String.Empty;
                    }

                    var row = taxaTable.NewRow();
                    row[0] = taxon.Id; // dyntaxaTaxonId
                    if (!taxon.CategoryId.Equals(CATEGORY_HYBRID))
                    {
                        try
                        {
                            // infraspecificEpithet    
                            row[1] = GetPartOfScientificName(taxon.ScientificName, INFRA_SPECIFIC_EPITHET);
                        }
                        catch (Exception)
                        {
                            WebServiceData.LogManager.Log(
                                context,
                                "Problems with taxon id = " + taxon.Id,
                                LogType.Error,
                                null);
                            throw;
                        }
                    }
                    else
                    {
                        row[1] = null;
                    }

                    row[2] = String.Empty; // nameAccordingTo isplanned=0
                    row[3] = String.Empty; // nameAccordingToId isplanned=0
                    row[4] = String.Empty; // namePublishedIn isplanned=0
                    row[5] = String.Empty; // namePublishedInId isplanned=0
                    row[6] = String.Empty; // namePublishedInYear isplanned=0
                    row[7] = String.Empty; // nomenclaturalCode isplanned=0
                    row[8] = String.Empty; // nomenclaturalStatus - Metod som hämtar namn från taxonobjektet
                    row[9] = GetTaxaOriginalNameFromCache(taxon.Id); // originalNameUsage 
                    row[10] = String.Empty; // originalNameUsageId 
                    row[11] = taxon.ScientificName; // scientificName 
                    row[12] = taxon.Author; // scientificNameAuthorship 
                    row[13] = GetTaxonNameGuidFromCache(taxon.Id); // scientificNameId 
                    if (!taxon.CategoryId.Equals(CATEGORY_HYBRID))
                    {
                        // specificEpithet
                        row[14] = GetPartOfScientificName(taxon.ScientificName, SPECIFIC_EPITHET);
                    }
                    else
                    {
                        row[14] = null;
                    }

                    row[15] = taxon.Guid; // taxonConceptId 
                    row[16] = String.Empty; // taxonConceptStatus
                    row[17] = String.Empty; // taxonomicStatus 
                    row[18] = GetTaxonCategoryFromCache(taxon.CategoryId); // taxonRank 
                    row[19] = taxon.SortOrder; // taxonSortOrder 
                    row[20] = TAXON_URL_PREFIX + taxon.Id.WebToString(); // taxonURL 
                    row[21] = taxon.CommonName; // vernacularName 
                    row[22] = taxon.CategoryId;

                    //row[23] = 0; // actionplan
                    //row[24] = 0; // conservationRelevant
                    //row[25] = 0; // natura2000
                    //row[26] = 0; // protectedByLaw
                    //row[27] = 0; // protectionLevel
                    //row[28] = String.Empty; // redlistCategory
                    //row[29] = String.Empty; // swedishImmigrationHistory
                    //row[30] = String.Empty; // swedishOccurrence
                    //row[31] = String.Empty; // organismGroup
                    //row[32] = 0; // disturbanceRadius

                    row[23] = taxonInformation.ActionPlanId; // actionplan
                    row[24] = taxonInformation.ConservationRelevant ? 1 : 0; // conservationRelevant
                    row[25] = taxonInformation.Natura2000 ? 1 : 0; // natura2000
                    row[26] = taxonInformation.ProtectedByLaw ? 1 : 0; // protectedByLaw
                    row[27] = taxonInformation.ProtectionLevel; // protectionLevel
                    row[28] = taxonInformation.RedlistCategory; // redlistCategory
                    row[29] = taxonInformation.SwedishImmigrationHistory; // swedishImmigrationHistory
                    row[30] = taxonInformation.SwedishOccurrence; // swedishOccurrence
                    row[31] = taxonInformation.OrganismGroup; // organismGroup
                    row[32] = taxonInformation.DisturbanceRadius; // disturbanceRadius

                    row[33] = String.Empty;
                    row[34] = String.Empty;
                    row[35] = String.Empty;
                    row[36] = String.Empty;
                    row[37] = String.Empty;
                    row[38] = String.Empty;
                    row[39] = String.Empty;
                    row[40] = String.Empty;

                    if (taxon.IsValid)
                    {
                        // Load TempTaxonTreeNodes into dictionary.
                        //     Dictionary<Int32, MyTaxonTreeNode> dictionaryTaxonTreeNode =  GetTaxonTreeNodes(context);
                        Int32 currentTaxonId = taxon.Id;
                        String higherClassificationList = String.Empty;
                        String sign = String.Empty;

                        //loop through the tree structure and set the corresponding values
                        if (dictionaryTaxonTreeNode.ContainsKey(currentTaxonId))
                        {
                            while (dictionaryTaxonTreeNode[currentTaxonId].Parents.IsNotEmpty())
                            {
                                currentTaxonId = dictionaryTaxonTreeNode[currentTaxonId].Parents[0].TaxonId;

                                if (dictionaryTaxonTreeNode[currentTaxonId].Parents.IsNotEmpty())
                                {
                                    higherClassificationList =
                                        dictionaryTaxonTreeNode[currentTaxonId].Parents[0].Information
                                            .ScientificName + sign + higherClassificationList;
                                    sign = ";";
                                }

                                switch (dictionaryTaxonTreeNode[currentTaxonId].Information.TaxonCategoryId)
                                {
                                    case (Int32)TaxonCategoryId.Class:
                                        // row[34] = String.Empty; //class
                                        row[34] = dictionaryTaxonTreeNode[currentTaxonId].Information.ScientificName;
                                        break;

                                    case (Int32)TaxonCategoryId.Family:
                                        // row[35] = String.Empty; //family
                                        row[35] = dictionaryTaxonTreeNode[currentTaxonId].Information.ScientificName;
                                        break;

                                    case (Int32)TaxonCategoryId.Genus:
                                        // row[36] = String.Empty; //genus
                                        row[36] = dictionaryTaxonTreeNode[currentTaxonId].Information.ScientificName;
                                        break;

                                    case (Int32)TaxonCategoryId.Kingdom:
                                        // row[37] = String.Empty; //kingdom
                                        row[37] = dictionaryTaxonTreeNode[currentTaxonId].Information.ScientificName;
                                        break;

                                    case (Int32)TaxonCategoryId.Order:
                                        // row[38] = String.Empty; //order
                                        row[38] = dictionaryTaxonTreeNode[currentTaxonId].Information.ScientificName;
                                        break;

                                    case (Int32)TaxonCategoryId.Phylum:
                                        // row[39] = String.Empty; //phylum
                                        row[39] = dictionaryTaxonTreeNode[currentTaxonId].Information.ScientificName;
                                        break;

                                    case (Int32)TaxonCategoryId.Subgenus:
                                        // row[40] = String.Empty; //subgenus
                                        row[40] = dictionaryTaxonTreeNode[currentTaxonId].Information.ScientificName;
                                        break;
                                }
                            }
                            row[33] = higherClassificationList;
                        }
                    }

                    row[41] = taxon.IsValid; // isValid

                    if (taxon.IsValid)
                    {
                        row[42] = DBNull.Value;
                    }
                    else
                    {
                        row[42] = TaxonManager.GetNewTaxonId(context, taxon.Id); // newDyntaxaTaxonId 
                    }

                    if (taxonRemarks.ContainsKey(taxon.Id))
                    {
                        row[43] = taxonRemarks[taxon.Id];
                    }
                    else
                    {
                        row[43] = "-";
                    }

                    if (taxon.IsValid)
                    {
                        row[44] = taxon.Id;
                    }
                    else
                    {
                        row[44] = TaxonManager.GetCurrentTaxonId(context, taxon.Id); // currentDyntaxaTaxonId
                    }

                    taxaTable.Rows.Add(row);
                }

                // Copy data into database.
                context.GetSpeciesObservationDatabase().AddTableData(context, taxaTable);
            }
        }

        private static DataTable CreateTaxaTable()
        {
            var taxaTable = new DataTable(TaxonData.TABLE_UPDATE_NAME);

            DataColumn column = new DataColumn(TaxonData.DYNTAXA_TAXON_ID, typeof(Int32));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.INFRASPECIFIC_EPITHET, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.NAME_ACCORDING_TO, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.NAME_ACCORDING_TO_ID, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.NAME_PUBLISHED_IN, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.NAME_PUBLISHED_IN_ID, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.NAME_PUBLISHED_IN_YEAR, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.NOMENCLATURAL_CODE, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.NOMENCLATURAL_STATUS, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.ORIGINAL_NAME_USAGE, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.ORIGINAL_NAME_USAGE_ID, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.SCIENTIFIC_NAME, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.SCIENTIFIC_NAME_AUTHORSHIP, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.SCIENTIFIC_NAME_ID, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.SPECIFIC_EPITHET, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.TAXON_CONCEPT_ID, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.TAXON_CONCEPT_STATUS, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.TAXONOMIC_STATUS, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.TAXON_RANK, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.TAXON_SORT_ORDER, typeof(Int32));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.TAXON_URL, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.VERNACULAR_NAME, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.CATEGORY_ID, typeof(Int32));
            taxaTable.Columns.Add(column);

            // ArtFakta kolumner
            column = new DataColumn(ArtFaktaData.ACTION_PLAN, typeof(Byte));
            taxaTable.Columns.Add(column);
            column = new DataColumn(ArtFaktaData.CONSERVATION_RELEVANT, typeof(Byte));
            taxaTable.Columns.Add(column);
            column = new DataColumn(ArtFaktaData.NATURA_2000, typeof(Byte));
            taxaTable.Columns.Add(column);
            column = new DataColumn(ArtFaktaData.PROTECTED_BY_LAW, typeof(Byte));
            taxaTable.Columns.Add(column);
            column = new DataColumn(ArtFaktaData.PROTECTION_LEVEL, typeof(Int32));
            taxaTable.Columns.Add(column);
            column = new DataColumn(ArtFaktaData.REDLIST_CATEGORY, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(ArtFaktaData.SWEDISH_IMMIGRATION_HISTORY, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(ArtFaktaData.SWEDISH_OCCURRENCE, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(ArtFaktaData.ORGANISM_GROUP, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(ArtFaktaData.DISTURBANCE_RADIUS, typeof(Int32));
            taxaTable.Columns.Add(column);

            // TaxonTree Columns
            column = new DataColumn(TaxonTreeInformationData.HIGHER_CLASSIFICATION, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonTreeInformationData.CLASS, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonTreeInformationData.FAMILY, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonTreeInformationData.GENUS, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonTreeInformationData.KINGDOM, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonTreeInformationData.ORDER, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonTreeInformationData.PHYLUM, typeof(String));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonTreeInformationData.SUBGENUS, typeof(String));
            taxaTable.Columns.Add(column);

            // TaxonValidityAndLumpSplitInfo
            column = new DataColumn(TaxonData.ISVALID, typeof(Byte));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.NEW_DYNTAXA_TAXONID, typeof(Int32));
            taxaTable.Columns.Add(column);
            column = new DataColumn(TaxonData.TAXON_REMARK, typeof(String));
            taxaTable.Columns.Add(column);

            column = new DataColumn(TaxonData.CURRENT_DYNTAXA_TAXONID, typeof(Int32));
            taxaTable.Columns.Add(column);

            // Set TaxonId as worktables PK
            // taxaTable.PrimaryKey = new[] { taxaTable.Columns[TaxonData.DYNTAXA_TAXON_ID] };

            return taxaTable;
        }



        /// <summary>
        /// Converts a tree node structure from the proxy type to the local type, keeping the parent-child relation
        /// !! Only a subset of fields are copied so do not use this without examining it thoroughly!!
        /// </summary>
        /// <param name="webTaxonTreeNodes"></param>
        /// <returns></returns>
        private static Dictionary<Int32, MyTaxonTreeNode> ConvertTreeNodeStructure(List<WebTaxonTreeNode> webTaxonTreeNodes)
        {
            var dictionary = new Dictionary<Int32, MyTaxonTreeNode>();

            foreach (var webTaxonTreeNode in webTaxonTreeNodes)
            {
                innerConvertTreeNodeStructure(dictionary, null, webTaxonTreeNode);
            }
            return dictionary;
        }

        private static void innerConvertTreeNodeStructure(Dictionary<Int32, MyTaxonTreeNode> dictionary, MyTaxonTreeNode parentNode, WebTaxonTreeNode childNode)
        {
            if (dictionary.ContainsKey(childNode.Taxon.Id))
            {
                if (parentNode.IsNotNull())
                {
                    var thisNode = dictionary[childNode.Taxon.Id];
                    if (thisNode.Parents.All(item => item.TaxonId != parentNode.TaxonId))
                    {
                        thisNode.Parents.Add(parentNode);
                    }
                }
            }
            else
            {
                var thisNode = new MyTaxonTreeNode
                {
                    Childes = new List<MyTaxonTreeNode>(),
                    Information = new TaxonInformation { DyntaxaTaxonId = childNode.Taxon.Id, TaxonCategoryId = childNode.Taxon.CategoryId, ScientificName = childNode.Taxon.ScientificName },
                    Parents = parentNode == null ? new List<MyTaxonTreeNode>() : new List<MyTaxonTreeNode> { parentNode },
                    TaxonId = childNode.Taxon.Id
                };

                dictionary.Add(childNode.Taxon.Id, thisNode);

                if (childNode.Children != null)
                {
                    foreach (var webTaxonTreeNode in childNode.Children)
                    {
                        innerConvertTreeNodeStructure(dictionary, thisNode, webTaxonTreeNode);
                        thisNode.Childes.Add(dictionary[webTaxonTreeNode.Taxon.Id]);
                    }
                }
            }
        }

        /// <summary>
        /// Update taxon information in database.
        /// New information is read from web service TaxonService.
        /// </summary>
        /// <param name="taxonTree">Contains information about current taxon tree.</param>
        /// <param name="isRootTaxonTree">True if taxon tree is a root taxon tree node.</param>
        /// <param name="taxonTreeTable">Contains current taxon tree table.</param>
        private static void UpdateTaxonTreeInformation(WebTaxonTreeNode taxonTree,
                                                       Boolean isRootTaxonTree,
                                                       DataTable taxonTreeTable)
        {
            // Add information about current taxon tree node.
            DataRow row = taxonTreeTable.NewRow();
            row[0] = taxonTree.Taxon.Id;
            row[1] = taxonTree.Taxon.Id;
            if (isRootTaxonTree)
            {
                row[2] = 0;
            }
            else
            {
                row[2] = 1;
            }

            taxonTreeTable.Rows.Add(row);

            if (taxonTree.Children.IsNotEmpty())
            {
                // Add information about nearest child taxa relations
                foreach (WebTaxonTreeNode nearestChild in taxonTree.Children)
                {
                    row = taxonTreeTable.NewRow();
                    row[0] = taxonTree.Taxon.Id;
                    row[1] = nearestChild.Taxon.Id;
                    row[2] = 2;
                    taxonTreeTable.Rows.Add(row);
                }

                // Add information about child taxa relations.
                WebTaxonList childTaxa = new WebTaxonList();
                foreach (WebTaxonTreeNode nearestChild in taxonTree.Children)
                {
                    childTaxa.Merge(GetChildTaxa(nearestChild));
                }

                foreach (WebTaxon childTaxon in childTaxa)
                {
                    row = taxonTreeTable.NewRow();
                    row[0] = taxonTree.Taxon.Id;
                    row[1] = childTaxon.Id;
                    row[2] = 3;
                    taxonTreeTable.Rows.Add(row);
                }

                // Add information about child taxon trees.
                foreach (WebTaxonTreeNode nearestChild in taxonTree.Children)
                {
                    UpdateTaxonTreeInformation(nearestChild, false, taxonTreeTable);
                }
            }
        }

        /// <summary>
        /// Update taxon information in database.
        /// New information is read from web service TaxonService.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="taxonTrees">Contains information about all valid taxon trees.</param>
        private static void UpdateTempTaxonTreeInformation(WebServiceContext context, IEnumerable<WebTaxonTreeNode> taxonTrees)
        {
            // Create table.
            using (DataTable taxonTreeTable = new DataTable(TaxonTreeData.TABLE_UPDATE_NAME))
            {
                DataColumn column = new DataColumn(TaxonTreeData.PARENT_TAXON_ID, typeof(Int32));
                taxonTreeTable.Columns.Add(column);
                column = new DataColumn(TaxonTreeData.CHILD_TAXON_ID, typeof(Int32));
                taxonTreeTable.Columns.Add(column);
                column = new DataColumn(TaxonTreeData.PARENT_CHILD_RELATION_ID, typeof(Int32));
                taxonTreeTable.Columns.Add(column);

                // Add information into table.
                foreach (WebTaxonTreeNode taxonTree in taxonTrees)
                {
                    UpdateTaxonTreeInformation(taxonTree, true, taxonTreeTable);
                }

                // Copy data into database.
                context.GetSpeciesObservationDatabase().AddTableData(context, taxonTreeTable);
            }
        }

        /// <summary>
        /// The get species facts.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        private static IEnumerable<ArtDatabanken.WebService.Data.WebSpeciesFact> GetSpeciesFacts(WebServiceContext webContext)
        {
            WebSpeciesFactSearchCriteria webUserParameterSelection = new WebSpeciesFactSearchCriteria();
            webUserParameterSelection.IncludeNotValidHosts = true;
            webUserParameterSelection.IncludeNotValidTaxa = true;
            webUserParameterSelection.FactorIds = new List<Int32>();

            // ActionPlan
            webUserParameterSelection.FactorIds.Add((Int32)ArtDatabanken.Data.FactorId.ActionPlan);

            // Redlist
            webUserParameterSelection.FactorIds.Add((Int32)ArtDatabanken.Data.FactorId.RedlistCategory);

            // Natura 2000
            webUserParameterSelection.FactorIds.Add((Int32)ArtDatabanken.Data.FactorId.Natura2000BirdsDirective);
            webUserParameterSelection.FactorIds.Add((Int32)ArtDatabanken.Data.FactorId.Natura2000HabitatsDirectiveArticle2);
            webUserParameterSelection.FactorIds.Add((Int32)ArtDatabanken.Data.FactorId.Natura2000HabitatsDirectiveArticle4);
            webUserParameterSelection.FactorIds.Add((Int32)ArtDatabanken.Data.FactorId.Natura2000HabitatsDirectiveArticle5);

            // ProtectionLevel
            webUserParameterSelection.FactorIds.Add((Int32)ArtDatabanken.Data.FactorId.ProtectionLevel);

            // ProtectedByLaw
            webUserParameterSelection.FactorIds.Add((Int32)ArtDatabanken.Data.FactorId.ProtectedByLaw);

            // SwedishHistory
            webUserParameterSelection.FactorIds.Add((Int32)ArtDatabanken.Data.FactorId.SwedishHistory);

            // SwedishOccurrence
            webUserParameterSelection.FactorIds.Add((Int32)ArtDatabanken.Data.FactorId.SwedishOccurrence);

            // Redlist_OrganismLabel1
            webUserParameterSelection.FactorIds.Add((Int32)ArtDatabanken.Data.FactorId.Redlist_OrganismLabel1);

            // DisturbanceRadius
            webUserParameterSelection.FactorIds.Add((Int32)ArtDatabanken.Data.FactorId.DisturbanceRadius);

            webUserParameterSelection.IndividualCategoryIds = new List<Int32>();
            WebIndividualCategory individualcategory = WebServiceData.FactorManager.GetDefaultIndividualCategory(webContext);
            webUserParameterSelection.IndividualCategoryIds.Add(individualcategory.Id);
            webUserParameterSelection.PeriodIds = new List<Int32>();
            //            webUserParameterSelection.PeriodIds.Add(WebServiceData.FactorManager.GetCurrentPublicPeriod(webContext).Id);
            webUserParameterSelection.PeriodIds.Add((Int32)(PeriodId.Year2015));

            // Get data from web service.
            List<ArtDatabanken.WebService.Data.WebSpeciesFact> webSpeciesFacts = WebServiceData.SpeciesFactManager.GetSpeciesFactsBySearchCriteria(webContext, webUserParameterSelection);

            return webSpeciesFacts;
        }

        /// <summary>
        /// Get species fact information.
        /// This information is stored in instances of class TaxonInformation
        /// but only species fact related properties has values in these instances.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Species fact information.</returns>
        private static Dictionary<Int32, TaxonInformation> GetSpeciesFactInformation(WebServiceContext context)
        {
            Dictionary<Int32, TaxonInformation> taxonInformations;
            IEnumerable<WebSpeciesFact> webSpeciesFacts;
            TaxonInformation taxonInformation;

            GetOrganismGroups(context);
            GetSwedishHistoryEnums(context);
            GetSwedishOccurrenceEnums(context);

            taxonInformations = new Dictionary<Int32, TaxonInformation>();
            webSpeciesFacts = GetSpeciesFacts(context);
            foreach (WebSpeciesFact speciesFact in webSpeciesFacts)
            {
                if (taxonInformations.ContainsKey(speciesFact.TaxonId))
                {
                    taxonInformation = taxonInformations[speciesFact.TaxonId];
                }
                else
                {
                    taxonInformation = new TaxonInformation();
                    taxonInformation.ActionPlanId = 0;
                    taxonInformation.ConservationRelevant = false;
                    taxonInformation.DisturbanceRadius = 0;
                    taxonInformation.DyntaxaTaxonId = speciesFact.TaxonId;
                    taxonInformation.Natura2000 = false;
                    taxonInformation.ProtectedByLaw = false;
                    taxonInformation.ProtectionLevel = 0;
                    taxonInformation.RedlistCategory = String.Empty;
                    taxonInformation.SwedishImmigrationHistory = String.Empty;
                    taxonInformation.SwedishOccurrence = String.Empty;
                    taxonInformations[speciesFact.TaxonId] = taxonInformation;
                }

                // Update record in worktable with data from SpeciesFact.
                switch (speciesFact.FactorId)
                {
                    case (Int32)FactorId.Natura2000HabitatsDirectiveArticle2:
                    case (Int32)FactorId.Natura2000HabitatsDirectiveArticle4:
                    case (Int32)FactorId.Natura2000HabitatsDirectiveArticle5:
                    case (Int32)FactorId.Natura2000BirdsDirective:
                        if (speciesFact.IsFieldValue1Specified)
                        {
                            taxonInformation.Natura2000 = ((Int32)(speciesFact.FieldValue1)) == 1;
                        }

                        break;

                    case (Int32)FactorId.ProtectionLevel:
                        if (speciesFact.IsFieldValue1Specified)
                        {
                            taxonInformation.ProtectionLevel = (Int32)(speciesFact.FieldValue1);
                        }

                        break;

                    case (Int32)FactorId.ProtectedByLaw:
                        if (speciesFact.IsFieldValue1Specified)
                        {
                            taxonInformation.ProtectedByLaw = ((Int32)(speciesFact.FieldValue1)) == 1;
                        }

                        break;

                    case (Int32)FactorId.RedlistCategory:
                        if (speciesFact.IsFieldValue1Specified && speciesFact.IsFieldValue4Specified)
                        {
                            if ((speciesFact.FieldValue1 >= -0.5) && (speciesFact.FieldValue1 <= 5.5))
                            {
                                taxonInformation.RedlistCategory = speciesFact.FieldValue4;
                            }
                        }

                        break;

                    case (Int32)FactorId.ActionPlan:
                        if (speciesFact.IsFieldValue1Specified)
                        {
                            taxonInformation.ActionPlanId = (Int32)(speciesFact.FieldValue1);
                        }

                        break;

                    case (Int32)FactorId.SwedishOccurrence:
                        if (speciesFact.IsFieldValue1Specified)
                        {
                            taxonInformation.SwedishOccurrence = GetSwedishOccurrenceFromCache((int)speciesFact.FieldValue1);
                        }

                        break;

                    case (Int32)FactorId.SwedishHistory:
                        if (speciesFact.IsFieldValue1Specified)
                        {
                            taxonInformation.SwedishImmigrationHistory = GetSwedishHistoryFromCache((int)speciesFact.FieldValue1);
                        }

                        break;

                    case (Int32)FactorId.Redlist_OrganismLabel1:
                        if (speciesFact.IsFieldValue1Specified)
                        {
                            taxonInformation.OrganismGroup = GetOrganismGroupFromCache((int)speciesFact.FieldValue1);
                        }

                        break;

                    case (Int32)FactorId.DisturbanceRadius:
                        if (speciesFact.IsFieldValue1Specified)
                        {
                            taxonInformation.DisturbanceRadius = (Int32)(speciesFact.FieldValue1);
                        }

                        break;
                }
            }

            return taxonInformations;
        }

        /// <summary>
        /// Get organism groups from Taxon attribute service and store in cache organismGroupCache.
        /// </summary>
        /// <param name="context">The context WebServiceContext.</param>
        private static void GetOrganismGroups(WebServiceContext context)
        {
            _organismGroupCache = new Hashtable();

            try
            {
                // WebServiceData.LogManager.Log(context, "Call to GetFactorFieldEnums.", LogType.Information, null);
                List<ArtDatabanken.WebService.Data.WebFactorFieldEnum> webFactorFieldEnums = WebServiceData.FactorManager.GetFactorFieldEnums(context);

                foreach (ArtDatabanken.WebService.Data.WebFactorFieldEnum webFactorFieldEnum in webFactorFieldEnums)
                {
                    if (webFactorFieldEnum.Id.Equals(KLASSDEFINITION_KLASS))
                    {
                        foreach (ArtDatabanken.WebService.Data.WebFactorFieldEnumValue webFactorFieldEnumValue in webFactorFieldEnum.Values)
                        {
                            _organismGroupCache[webFactorFieldEnumValue.KeyInteger] = webFactorFieldEnumValue.Label;
                        }

                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                WebServiceData.LogManager.LogError(context, ex);
                throw new ApplicationException(ex.Message);
            }
        }

        /// <summary>
        /// Get organism group name from cached list of all organism groups.
        /// </summary>
        /// <param name="id">Id for organism group.</param>
        /// <returns>The Organism group.</returns>
        private static String GetOrganismGroupFromCache(Int32 id)
        {
            String organismGroupName = String.Empty;
            if (_organismGroupCache.ContainsKey(id))
            {
                organismGroupName = (String)_organismGroupCache[id];
            }

            return organismGroupName;
        }

        /// <summary>
        /// Get swedish history enums from Taxon attribute service and store in cache swedishHistoryCache.
        /// </summary>
        /// <param name="context">The context WebServiceContext.</param>
        private static void GetSwedishHistoryEnums(WebServiceContext context)
        {
            _swedishHistoryCache = new Hashtable();

            try
            {
                // WebServiceData.LogManager.Log(context, "Call to GetFactorFieldEnums.", LogType.Information, null);
                List<ArtDatabanken.WebService.Data.WebFactorFieldEnum> webFactorFieldEnums = WebServiceData.FactorManager.GetFactorFieldEnums(context);

                foreach (ArtDatabanken.WebService.Data.WebFactorFieldEnum webFactorFieldEnum in webFactorFieldEnums)
                {
                    if (webFactorFieldEnum.Id.Equals(CLASS_DEFINITION_SWEDISH_HISTORY))
                    {
                        foreach (ArtDatabanken.WebService.Data.WebFactorFieldEnumValue webFactorFieldEnumValue in webFactorFieldEnum.Values)
                        {
                            _swedishHistoryCache[webFactorFieldEnumValue.KeyInteger] = webFactorFieldEnumValue.Label;
                        }

                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                WebServiceData.LogManager.LogError(context, ex);
                throw new ApplicationException(ex.Message);
            }
        }

        /// <summary>
        /// Get swedish history label from cached list of all swedish history enum values.
        /// </summary>
        /// <param name="id">Id for swedish history enum value.</param>
        /// <returns>The swedish history label.</returns>
        private static String GetSwedishHistoryFromCache(Int32 id)
        {
            String swedishHistoryLabel = String.Empty;
            if (_swedishHistoryCache.ContainsKey(id))
            {
                swedishHistoryLabel = (String)_swedishHistoryCache[id];
            }

            return swedishHistoryLabel;
        }

        /// <summary>
        /// Get swedish occurrence enums from Taxon attribute service and store in cache swedishOccurrenceCache.
        /// </summary>
        /// <param name="context">The context WebServiceContext.</param>
        private static void GetSwedishOccurrenceEnums(WebServiceContext context)
        {
            _swedishOccurrenceCache = new Hashtable();

            try
            {
                // WebServiceData.LogManager.Log(context, "Call to GetFactorFieldEnums.", LogType.Information, null);
                List<ArtDatabanken.WebService.Data.WebFactorFieldEnum> webFactorFieldEnums = WebServiceData.FactorManager.GetFactorFieldEnums(context);

                foreach (ArtDatabanken.WebService.Data.WebFactorFieldEnum webFactorFieldEnum in webFactorFieldEnums)
                {
                    if (webFactorFieldEnum.Id.Equals(CLASS_DEFINITION_SWEDISH_OCCURRENCE))
                    {
                        foreach (ArtDatabanken.WebService.Data.WebFactorFieldEnumValue webFactorFieldEnumValue in webFactorFieldEnum.Values)
                        {
                            _swedishOccurrenceCache[webFactorFieldEnumValue.KeyInteger] = webFactorFieldEnumValue.Label;
                        }

                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                WebServiceData.LogManager.LogError(context, ex);
                throw new ApplicationException(ex.Message);
            }
        }

        /// <summary>
        /// Get swedish occurrence label from cached list of all swedish occurrence enum values.
        /// </summary>
        /// <param name="id">Id for swedish occurrence enum value.</param>
        /// <returns>The swedish occurrence label.</returns>
        private static String GetSwedishOccurrenceFromCache(Int32 id)
        {
            String swedishOccurrenceLabel = String.Empty;
            if (_swedishOccurrenceCache.ContainsKey(id))
            {
                swedishOccurrenceLabel = (String)_swedishOccurrenceCache[id];
            }

            return swedishOccurrenceLabel;
        }

        /// <summary>
        /// Get taxa original name and store in cache.
        /// </summary>
        /// <param name="context">The WebServiceContext.</param>
        private static void GetTaxaOriginalNames(WebServiceContext context)
        {
            _taxonOriginalNameCache = new Hashtable();

            try
            {
                // WebServiceData.LogManager.Log(context, "Get taxa original names.", LogType.Information, null);
                // Get taxa original names
                WebTaxonNameSearchCriteria taxonNameSearchCriteria = new WebTaxonNameSearchCriteria();
                taxonNameSearchCriteria.IsOriginalName = true;
                taxonNameSearchCriteria.IsIsOriginalNameSpecified = true;

                List<WebTaxonName> taxaOriginalNames = WebServiceData.TaxonManager.GetTaxonNamesBySearchCriteria(
                    context, taxonNameSearchCriteria);
                foreach (WebTaxonName taxonName in taxaOriginalNames)
                {
                    _taxonOriginalNameCache[taxonName.Taxon.Id] = taxonName;
                }
            }
            catch (Exception ex)
            {
                WebServiceData.LogManager.LogError(context, ex);
                throw new ApplicationException(ex.Message);
            }
        }

        /// <summary>
        /// Get taxon original name from cached list of all taxa original names.
        /// </summary>
        /// <param name="taxonId">Id for taxon.</param>
        /// <returns>The Original Taxon Name.</returns>
        private static String GetTaxaOriginalNameFromCache(Int32 taxonId)
        {
            String originalTaxonName = String.Empty;
            if (_taxonOriginalNameCache.ContainsKey(taxonId))
            {
                WebTaxonName taxonName = (WebTaxonName)_taxonOriginalNameCache[taxonId];
                originalTaxonName = taxonName.Name + " " + taxonName.Author;
            }

            return originalTaxonName;
        }

        /// <summary>
        /// Get taxon original name from cached list of all taxa original names.
        /// </summary>
        /// <param name="taxonId">Id for taxon.</param>
        /// <returns>The Original taxon GUID.</returns>
        private static String GetTaxonNameGuidFromCache(Int32 taxonId)
        {
            String originalTaxonGuid = String.Empty;
            if (_taxonOriginalNameCache.ContainsKey(taxonId))
            {
                WebTaxonName taxonName = (WebTaxonName)_taxonOriginalNameCache[taxonId];
                originalTaxonGuid = taxonName.Guid;
            }

            return originalTaxonGuid;
        }

        /// <summary>
        /// Get taxon categories and store in cache.
        /// </summary>
        /// <param name="context">The WebServiceContext.</param>
        private static void GetTaxonCategories(WebServiceContext context)
        {
            _taxonCategoryCache = new Hashtable();

            try
            {
                // WebServiceData.LogManager.Log(context, "Get taxoncategories.", LogType.Information, null);
                List<WebTaxonCategory> taxonCategories = WebServiceData.TaxonManager.GetTaxonCategories(context);
                foreach (WebTaxonCategory taxonCategory in taxonCategories)
                {
                    _taxonCategoryCache[taxonCategory.Id] = taxonCategory;
                }
            }
            catch (Exception ex)
            {
                WebServiceData.LogManager.LogError(context, ex);
                throw new ApplicationException(ex.Message);
            }
        }

        /// <summary>
        /// Get taxon category from cached list of all taxon categories.
        /// </summary>
        /// <param name="id">Id for taxon category.</param>
        /// <returns>The taxon category.</returns>
        private static String GetTaxonCategoryFromCache(Int32 id)
        {
            String taxonCategoryName = String.Empty;
            if (_taxonCategoryCache.ContainsKey(id))
            {
                WebTaxonCategory taxonCategory = (WebTaxonCategory)_taxonCategoryCache[id];
                taxonCategoryName = taxonCategory.Name;
            }

            return taxonCategoryName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taxonId"></param>
        /// <param name="taxonTreeNodeList"></param>
        /// <returns></returns>
        ////private static String GetHigherClassificationList(Int32 taxonId, TaxonTreeNodeList taxonTreeNodeList)
        ////{
        ////    Int32 currentTaxonId = taxonId;
        ////    String higherClassificationList = String.Empty;
        ////    String sign = String.Empty;

        ////    while (taxonTreeNodeList[currentTaxonId].Parents.IsNotEmpty())
        ////    {
        ////        higherClassificationList = taxonTreeNodeList[currentTaxonId].Parents[0].Taxon.ScientificName + sign + higherClassificationList;
        ////        currentTaxonId = taxonTreeNodeList[currentTaxonId].Parents[0].Taxon.Id;
        ////        sign = ";";
        ////    }

        ////    return higherClassificationList;
        ////}

        /// <summary>
        /// The my taxon tree node.
        /// </summary>
        public class MyTaxonTreeNode
        {
            /// <summary>
            /// The parents.
            /// </summary>
            public List<MyTaxonTreeNode> Parents { get; set; }

            /// <summary>
            /// The childes.
            /// </summary>
            public List<MyTaxonTreeNode> Childes { get; set; }

            /// <summary>
            /// The taxon id.
            /// </summary>
            public Int32 TaxonId { get; set; }

            /// <summary>
            /// The information.
            /// </summary>
            public TaxonInformation Information { get; set; }
        }

        /// <summary>
        /// Get parts of taxons scientific name. 
        /// Used for specificEpithet and infraspecificEpithet.
        /// 
        /// The infraspecific Epithet should only be the terminal name part - the part of the name with the lowest or most specific rank. 
        /// Thus, given the scientificName "Carex viridula subsp. brachyrrhyncha var. elatior", the atomized Taxon terms for this name would be: 
        /// specificEpithet: viridula 
        /// infraspecificEpithet: elatior.
        /// </summary>
        /// <param name="scientificName">
        /// Taxon scientific name.
        /// </param>
        /// <param name="part">
        /// Indicates which part of the name that should be returned.
        /// </param>
        /// <returns>
        /// Part of taxon scientific name.
        /// </returns>
        private static String GetPartOfScientificName(String scientificName, Int32 part)
        {
            String partOfScienticName = null;
            char[] delimiters = new[] { ' ' };

            String[] scientificNameParts = scientificName.Split(delimiters);
            if (scientificNameParts.Length > 1)
            {
                if (part.Equals(SPECIFIC_EPITHET))
                {
                    // The second part of the sci.name
                    partOfScienticName = scientificNameParts[1];
                }
                else if (part.Equals(INFRA_SPECIFIC_EPITHET) && scientificNameParts.Length > 2)
                {
                    // The last part of the sci.name
                    partOfScienticName = scientificNameParts[scientificNameParts.Length - 1];
                }
            }

            return partOfScienticName;
        }
    }
}
