using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    ///  This class handles information about bird nest activities.
    /// </summary>
    [Serializable()]
    public class BirdNestActivity : DataId
    {
        private String _name;

        /// <summary>
        /// Create a BirdNestActivity instance.
        /// </summary>
        /// <param name='id'>Id for bird nest activity.</param>
        /// <param name='name'>Name for bird nest activity.</param>
        public BirdNestActivity(Int32 id, String name)
            : base(id)
        {
            _name = name;
        }

        /// <summary>
        /// Get name for bird nest activity.
        /// </summary>
        public String Name
        {
            get { return _name; }
        }
    }
}
