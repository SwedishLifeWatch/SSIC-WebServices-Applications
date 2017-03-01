using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains information about an address.
    /// </summary>
    [Serializable]
    public class SpeciesActivity : ISpeciesActivity
    {
        /// <summary>
        /// Id for the species activity category that
        /// this species activity belongs to.
        /// </summary>
        public ISpeciesActivityCategory Category 
        { get; set; }

        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext 
        { get; set; }

        /// <summary>
        /// Id for the species activity category that
        /// this species activity belongs to.
        /// </summary>
       public Int32 CategoryId
        { get; set; }

        /// <summary>
        /// GUID for this species activity.
        /// </summary>
       public String Guid { get; set; }

        /// <summary>
        /// Id for this species activity.
        /// </summary>
       public Int32 Id
        { get; set; }

        /// <summary>
        /// Identifier for this species activity.
        /// </summary>
        public String Identifier
        { get; set; }

        /// <summary>
        /// Name for this species activity.
        /// </summary>
       public String Name
        { get; set; }

        /// <summary>
        /// This species activity may be used together
        /// with these specified taxa and their child taxa.
        /// If property TaxonIds is empty this means that
        /// this activity may be used for all taxa.
        /// </summary>
       public List<Int32> TaxonIds
        { get; set; }

        /// <summary>
        /// ToString method
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
