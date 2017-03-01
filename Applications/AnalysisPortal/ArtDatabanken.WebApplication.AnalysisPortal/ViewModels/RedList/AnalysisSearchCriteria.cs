using System.Collections.Generic;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.RedList
{
    /// <summary>
    /// Search criteria that are used during analysis.
    /// </summary>    
    public class AnalysisSearchCriteria
    {
        ///// <summary>
        ///// Swedish occurrence search criteria.
        ///// </summary>
        //public List<int> SwedishOccurrence;

        ///// <summary>
        ///// Biotope search criteria.
        ///// See class BiotopeSearchCriteria for more details.
        ///// A null value means that this search criteria
        ///// should not be used.
        ///// </summary>    
        //public BiotopeSearchCriteria Biotopes;

        ///// <summary>
        ///// County occurrence consists of
        ///// all child factors to factor with id = 775.
        ///// Possible values are the factor id for all 
        ///// child factors to factor with id = 775.
        ///// Only species fact with value = 4 (resident) are cached.
        ///// A null value means that this search criteria
        ///// should not be used.
        ///// An empty integer list means that no taxa should match.
        ///// </summary>    
        //public List<int> CountyOccurrence;

        ///// <summary>
        ///// Host search criteria.
        ///// See class HostSearchCriteria for more details.
        ///// A null value means that this search criteria
        ///// should not be used.
        ///// </summary>    
        //public HostSearchCriteria Host;

        ///// <summary>
        ///// Impact search criteria.
        ///// See class ImpactSearchCriteria for more details.
        ///// A null value means that this search criteria
        ///// should not be used.
        ///// </summary>    
        //public ImpactSearchCriteria Impact;

        ///// <summary>
        ///// Landscape type search criteria.
        ///// See class LandscapeTypeSearchCriteria for more details.
        ///// A null value means that this search criteria
        ///// should not be used.
        ///// </summary>    
        //public LandscapeTypeSearchCriteria LandscapeTypes;

        ///// <summary>
        ///// Life form search criteria.
        ///// Possible values are the factor id for all 
        ///// child leaf factors to factor with id = 1859.
        ///// Only species fact with value = True are cached.
        ///// A null value means that this search criteria
        ///// should not be used.
        ///// An empty integer list means that no taxa should match.
        ///// </summary>    
        //public List<int> LifeForms;

        ///// <summary>
        ///// Organism group 1.
        ///// Factor with id 656.
        ///// Possible values are the integer value for each
        ///// enum value that is defined for this factor.
        ///// A null value means that this search criteria
        ///// should not be used.
        ///// An empty integer list means that no taxa should match.
        ///// </summary>    
        //public List<int> OrganismGroups;

        /// <summary>
        /// Red list category search criteria.
        /// Factor with id 743.
        /// Possible values are
        /// 0 DD, Data Deficient.
        /// 1 RE, Regionally Extinct.
        /// 2 CR, Critically Endagered.
        /// 3 EN, Endagered.
        /// 4 VU, Vulnerable.
        /// 5 NT, Near Threatened.
        /// 6 LC, Least Concern.
        /// A null value means that this search criteria
        /// should not be used.
        /// An empty integer list means that no taxa should match.
        /// </summary>    
        public List<int> RedListCategories;

        ///// <summary>
        ///// Taxon category search criteria.
        ///// Taxon from species fact database with factor with id 655, 
        ///// using the following possible values are A, S or U, and 
        ///// taxa defined in Taxon database with taxon categories species 
        ///// or subspecies will also be included in the result.
        ///// A null value means that this search criteria
        ///// should not be used.
        ///// An empty string list means that no taxa should match.
        ///// </summary>    
        //public List<string> TaxonCategories;

        ///// <summary>
        ///// TaxonScope search criteria.
        ///// See class TaxonScopeSearchCriteria for more details.
        ///// A null value means that this search criteria
        ///// should not be used.
        ///// Taxon scope search criteria  sets taxonomic categories that are to be used is search, 
        ///// and a boolean indication if all defined additional categories are goining to be used in search.
        ///// An empty integer list, categories list, means that no taxa should match.
        ///// </summary>    
        //public TaxonScopeSearchCriteria TaxonScope;

        ///// <summary>
        ///// Substrate search criteria.
        ///// See class SubstrateSearchCriteria for more details.
        ///// A null value means that this search criteria
        ///// should not be used.
        ///// </summary>    
        //public SubstrateSearchCriteria Substrate;

        ///// <summary>
        ///// Thematic list search criteria.
        ///// See class ThematicListSearchCriteria for more details.
        ///// A null value means that this search criteria
        ///// should not be used.
        ///// </summary>
        //public ThematicListSearchCriteria ThematicLists;
    }
}
