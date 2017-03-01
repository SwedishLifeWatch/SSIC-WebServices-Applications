using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains information about a taxon category.
    /// </summary>
    [Serializable]
    public class TaxonCategory : ITaxonCategory
    {
        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Id of taxon category.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Test if taxon category is a main category.
        /// </summary>
        public Boolean IsMainCategory { get; set; }

        /// <summary>
        /// Test if taxon category is taxonomic.
        /// </summary>
        public Boolean IsTaxonomic { get; set; }

        /// <summary>
        /// Name of the taxon category.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Get name on definite form.
        /// Current implementation only support the swedish language.
        /// </summary>
        public String NameDefinite
        {
            // TODO: The data used in NameDefinite should 
            // probably be moved to the database.
            get
            {
                String name;

                name = Name.ToLower();
                switch (Id)
                {
                    case 14:
                        name = name + "t";
                        break;
                    case 1:
                    case 15:
                        name = name + "et";
                        break;
                    case 2:
                    case 3:
                        name = name + "men";
                        break;
                    case 24:
                        name = "gruppen av familjer";
                        break;
                    case 28:
                        name = "det svårbestämda artparet";
                        break;
                    case 27:
                        name = "arten";
                        break;
                    default:
                        name = name + "en";
                        break;
                }
                return name;
            }
        }

        /// <summary>
        /// Id of parent taxon category.
        /// </summary>
        public Int32 ParentId { get; set; }

        /// <summary>
        /// Sort order for this taxon category.
        /// </summary>
        public Int32 SortOrder { get; set; }

        /// <summary>
        /// Get parent taxon category.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>Parent taxon category.</returns>
        public virtual ITaxonCategory GetParent(IUserContext userContext)
        {
            return CoreData.TaxonManager.GetTaxonCategory(userContext, ParentId);
        }
    }
}
