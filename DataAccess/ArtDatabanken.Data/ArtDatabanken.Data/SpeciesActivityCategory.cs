using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains information about an address.
    /// </summary>
    [Serializable]
    public class SpeciesActivityCategory : ISpeciesActivityCategory
    {
        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext 
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
    }
}
