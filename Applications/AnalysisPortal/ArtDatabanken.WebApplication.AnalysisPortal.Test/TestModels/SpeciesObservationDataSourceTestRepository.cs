using System;
using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Test.TestModels
{
    class SpeciesObservationDataSourceTestRepository : ISpeciesObservationDataSource
    {
        public static IEnumerable<int> AllTaxonIds = Enumerable.Range(1, 1000);

        public IDataSourceInformation GetDataSourceInformation()
        {
            throw new NotImplementedException();
        }

        public SpeciesActivityList GetBirdNestActivities(IUserContext userContext)
        {
            throw new NotImplementedException();
        }

        public RegionList GetCountyRegions(IUserContext userContext)
        {
            throw new NotImplementedException();
        }

        public DarwinCoreList GetDarwinCore(IUserContext userContext, List<long> speciesObservationIds,
                                                 ICoordinateSystem coordinateSystem)
        {
            throw new NotImplementedException();
        }

        public DarwinCoreList GetDarwinCore(IUserContext userContext, 
                                                            ISpeciesObservationSearchCriteria searchCriteria,
                                                            ICoordinateSystem coordinateSystem,
                                                            SpeciesObservationFieldSortOrderList sortOrder)
        {
            throw new NotImplementedException();
        }

        public DarwinCoreList GetDarwinCore(IUserContext userContext,
                                                                ISpeciesObservationSearchCriteria searchCriteria,
                                                                ICoordinateSystem coordinateSystem,
                                                                ISpeciesObservationPageSpecification pageSpecification)
        {
            throw new NotImplementedException();
        }

        public SpeciesObservationList GetSpeciesObservationBySearchCriteriaPage(IUserContext userContext,
                                                                ISpeciesObservationSearchCriteria searchCriteria,
                                                                ICoordinateSystem coordinateSystem,
                                                                ISpeciesObservationPageSpecification pageSpecification)
        {
            SpeciesObservationList speciesObservationList = new SpeciesObservationList();
            IEnumerable<int> taxonIds;
            if (searchCriteria.TaxonIds != null && searchCriteria.TaxonIds.Count > 0)
                taxonIds = searchCriteria.TaxonIds;
            else
                taxonIds = AllTaxonIds;

            foreach (int taxonId in taxonIds)
            {
                SpeciesObservation speciesObservation = CreateSpeciesObservation(taxonId);
                speciesObservationList.Add(speciesObservation);
            }

            return speciesObservationList;
        }

        public DarwinCoreChange GetDarwinCoreChange(IUserContext userContext, DateTime changedFrom, DateTime changedTo,
                                                    CoordinateSystem coordinateSystem)
        {
            throw new NotImplementedException();
        }

        public IDarwinCoreChange GetDarwinCoreChange(IUserContext userContext, DateTime? changedFrom, DateTime? changedTo, long? changeId, long maxReturnedChanges, ISpeciesObservationSearchCriteria searchCriteria, ICoordinateSystem coordinateSystem)
        {
            throw new NotImplementedException();
        }

        public bool GetProtectedSpeciesObservationIndication(IUserContext userContext, ISpeciesObservationSearchCriteria searchCriteria,
                                                             ICoordinateSystem coordinateSystem)
        {
            throw new NotImplementedException();
        }

        public RegionList GetProvinceRegions(IUserContext userContext)
        {
            throw new NotImplementedException();
        }

        public SpeciesActivityList GetSpeciesActivities(IUserContext userContext)
        {
            throw new NotImplementedException();
        }

        public SpeciesActivityCategoryList GetSpeciesActivityCategories(IUserContext userContext)
        {
            throw new NotImplementedException();
        }

        public ISpeciesActivityCategory GetSpeciesActivityCategory(IUserContext userContext, int speciesActivityCategoryId)
        {
            throw new NotImplementedException();
        }

        public ISpeciesObservationChange GetSpeciesObservationChange(IUserContext userContext,
                                                                     DateTime? changedFrom,
                                                                     DateTime? changedTo,
                                                                     long? changeId,
                                                                     long maxReturnedChanges,
                                                                     ISpeciesObservationSearchCriteria searchCriteria,
                                                                     ICoordinateSystem coordinateSystem,
                                                                     ISpeciesObservationSpecification speciesObservationSpecification)
        {
            throw new NotImplementedException();
        }

        public SpeciesObservationList GetSpeciesObservations(IUserContext userContext,
                                                             List<long> speciesObservationIds,
                                                             ICoordinateSystem coordinateSystem,
                                                             ISpeciesObservationSpecification speciesObservationSpecification)
        {
            throw new NotImplementedException();
        }

        public SpeciesObservationList GetSpeciesObservations(IUserContext userContext,
                                                             ISpeciesObservationSearchCriteria searchCriteria,
                                                             ICoordinateSystem coordinateSystem,
                                                             ISpeciesObservationSpecification speciesObservationSpecification,
                                                             SpeciesObservationFieldSortOrderList sortOrder)
        {
            throw new NotImplementedException();
        }

        public Int64 GetSpeciesObservationCount(IUserContext userContext,
                                                                ISpeciesObservationSearchCriteria searchCriteria,
                                                                ICoordinateSystem coordinateSystem)
        {
            throw new NotImplementedException();
        }

        public SpeciesObservationList GetSpeciesObservations(IUserContext userContext,
                                                             ISpeciesObservationSearchCriteria searchCriteria,
                                                             ICoordinateSystem coordinateSystem,
                                                             ISpeciesObservationPageSpecification pageSpecification,
                                                             ISpeciesObservationSpecification speciesObservationSpecification)
        {
            SpeciesObservationList speciesObservationList = new SpeciesObservationList();
            IEnumerable<int> taxonIds;
            if (searchCriteria.TaxonIds != null && searchCriteria.TaxonIds.Count > 0)
                taxonIds = searchCriteria.TaxonIds;
            else
                taxonIds = AllTaxonIds;

            foreach (int taxonId in taxonIds)
            {
                SpeciesObservation speciesObservation = CreateSpeciesObservation(taxonId);
                speciesObservationList.Add(speciesObservation);
            }

            return speciesObservationList;
        }

        public SpeciesObservationList GetSpeciesObservationsByIds(IUserContext userContext, List<long> speciesObservationIds,
                                                                  ICoordinateSystem coordinateSystem)
        {
            throw new NotImplementedException();
        }

        public SpeciesObservationList GetSpeciesObservationsBySearchCriteria(IUserContext userContext,
                                                                             ISpeciesObservationSearchCriteria searchCriteria,
                                                                             ICoordinateSystem coordinateSystem,
                                                                             SpeciesObservationFieldList fieldList)
        {
            SpeciesObservationList speciesObservationList = new SpeciesObservationList();
            IEnumerable<int> taxonIds;
            if (searchCriteria.TaxonIds != null && searchCriteria.TaxonIds.Count > 0)
                taxonIds = searchCriteria.TaxonIds;
            else
                taxonIds = AllTaxonIds;
            
            foreach (int taxonId in taxonIds)
            {
                SpeciesObservation speciesObservation = CreateSpeciesObservation(taxonId);
                speciesObservationList.Add(speciesObservation);
            }
                        
            return speciesObservationList;
        }

        

        private static SpeciesObservation CreateSpeciesObservation(int taxonId)
        {
            SpeciesObservation speciesObservation = new SpeciesObservation();
            speciesObservation.Taxon = new SpeciesObservationTaxon();
            speciesObservation.Taxon.TaxonID = taxonId.ToString();
            return speciesObservation;            
        }

        public SpeciesObservationDataProviderList GetSpeciesObservationDataProviders(IUserContext userContext)
        {
            throw new NotImplementedException();
        }

        public SpeciesObservationFieldDescriptionList GetSpeciesObservationFieldDescriptions(IUserContext userContext)
        {
            SpeciesObservationFieldDescriptionList list = new SpeciesObservationFieldDescriptionList();
            SpeciesObservationFieldDescription desc1 = new SpeciesObservationFieldDescription();            
            list.Add(desc1);
            return list;
        }
    }
}
