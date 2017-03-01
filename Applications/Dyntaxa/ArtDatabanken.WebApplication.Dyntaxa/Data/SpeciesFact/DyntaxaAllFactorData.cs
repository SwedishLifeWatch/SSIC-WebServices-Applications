using System.Collections.Generic;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    /// <summary>
    /// This class holds all required data from artdatabanken service for a taxon
    /// </summary>
    public class DyntaxaAllFactorData
    {
        private IList<DyntaxaFactor> dyntaxaAllFactors = new List<DyntaxaFactor>();
        private IList<DyntaxaHost> hostList = new List<DyntaxaHost>();
        private IList<DyntaxaHost> completeHostList = new List<DyntaxaHost>();
        private IList<DyntaxaIndividualCategory> individualCategoryList = new List<DyntaxaIndividualCategory>();
        private IList<DyntaxaPeriod> periodList = new List<DyntaxaPeriod>();
        private IList<DyntaxaSpeciesFact> dyntaxaAllSpeciesFactsForATaxon = new List<DyntaxaSpeciesFact>();
        private int taxonId = -1;
        private string swedishOccuranceInfo = string.Empty;
        private string swedishHistoryInfo = string.Empty;
        
        public IList<DyntaxaFactor> DyntaxaAllFactors
        {
            get { return dyntaxaAllFactors; }
            set { dyntaxaAllFactors = value; }
        }

        public IList<DyntaxaHost> HostList
        {
            get { return hostList; }
        }

        public IList<DyntaxaHost> CompleteHostList
        {
            get { return completeHostList; }
        }

        public IList<DyntaxaIndividualCategory> IndividualCategoryList
        {
            get { return individualCategoryList; }
        }

        public IList<DyntaxaPeriod> PeriodList
        {
            get { return periodList; }
        }

        public IList<DyntaxaSpeciesFact> DyntaxaAllSpeciesFactsForATaxon
        {
            get { return dyntaxaAllSpeciesFactsForATaxon; }
        }

        public string SwedishOccuranceInfo
        {
            get { return swedishOccuranceInfo; }
        }

        public string SwedishHistoryInfo
        {
            get { return swedishHistoryInfo; }
        }

        public int TaxonId
        {
            get { return taxonId; }
        }

        public DyntaxaAllFactorData(IList<DyntaxaFactor> dyntaxaAllFactors, IList<DyntaxaHost> hostList, IList<DyntaxaIndividualCategory> individualCategoryList, IList<DyntaxaPeriod> periodList, IList<DyntaxaSpeciesFact> dyntaxaAllSpeciesFactsForATaxon, int taxonId, string swedishOccuranceInfo, string swedishHistoryInfo, IList<DyntaxaHost> completeHostList)
        {
            this.dyntaxaAllFactors = dyntaxaAllFactors;
            this.hostList = hostList;
            this.individualCategoryList = individualCategoryList;
            this.periodList = periodList;
            this.dyntaxaAllSpeciesFactsForATaxon = dyntaxaAllSpeciesFactsForATaxon;
            this.taxonId = taxonId;
            this.swedishHistoryInfo = swedishHistoryInfo;
            this.swedishOccuranceInfo = swedishOccuranceInfo;
            this.completeHostList = completeHostList;
        }
    }
}