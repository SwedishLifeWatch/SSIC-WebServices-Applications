using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    ///  Enum that contains taxon type ids for taxon types that
    ///  are specifically used by web service clients.
    /// </summary>
    public enum TaxonTypeId
    {
        /// <summary>
        ///  Id for kingdom taxon type.
        /// </summary>
        Kingdom = 1,
        /// <summary>
        ///  Id for phylum taxon type.
        /// </summary>
        Phylum = 2,
        /// <summary>
        ///  Id for class taxon type.
        /// </summary>
        Class = 5,
        /// <summary>
        ///  Id for order taxon type.
        /// </summary>
        Order = 8,
        /// <summary>
        ///  Id for family taxon type.
        /// </summary>
        Family = 11,
        /// <summary>
        ///  Id for species taxon type.
        /// </summary>
        Species = 17
    }

    /// <summary>
    ///  This class represents a taxon type.
    /// </summary>
    [Serializable()]
    public class TaxonType : DataSortOrder, IListableItem
    {
        private String _name;

        /// <summary>
        /// Create a TaxonType instance.
        /// </summary>
        /// <param name='id'>Id for taxon type.</param>
        /// <param name='name'>Name for taxon type.</param>
        /// <param name='sortOrder'>Sort order among taxon types.</param>
        public TaxonType(Int32 id, String name, Int32 sortOrder)
            : base(id, sortOrder)
        {
            _name = name;
        }

        /// <summary>
        /// Get name for this taxon type.
        /// </summary>
        public String Name
        {
            get { return _name; }
        }

        /// <summary>
        ///  Get name on definite form.
        /// </summary>
        public String NameDefinite
        {
            // TODO: The data used in NameDefinite should 
            // probably be moved to the database.
            get
            {
                String name = String.Empty;

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

        #region IListableItem Members

        /// <summary>
        /// Get label for this taxon type.
        /// </summary>
        public string Label
        {
            get { return _name; }
        }

        #endregion
    }
}
