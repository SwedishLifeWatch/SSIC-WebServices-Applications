using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface handles information about an address.
    /// </summary>
    public interface ISpeciesActivity : IDataId32
    {
        /// <summary>
        /// Id for the species activity category that
        /// this species activity belongs to.
        /// </summary>
        ISpeciesActivityCategory Category
        { get; set; }
        
        /// <summary>
        /// Data context.
        /// </summary>
        IDataContext DataContext
        { get; set; }

        /// <summary>
        /// GUID for this species activity.
        /// </summary>
        String Guid { get; set; }

        /// <summary>
        /// Identifier for this species activity.
        /// </summary>
        String Identifier
        { get; set; }

        /// <summary>
        /// Name for this species activity.
        /// </summary>
        String Name
        { get; set; }

        /// <summary>
        /// This species activity may be used together
        /// with these specified taxa and their child taxa.
        /// If property TaxonIds is empty this means that
        /// this activity may be used for all taxa.
        /// </summary>
        List<Int32> TaxonIds
        { get; set; }

        

       
    }
}
