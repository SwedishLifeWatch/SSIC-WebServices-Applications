using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    ///  Enum that contains period type ids.
    /// </summary>
    public enum PeriodTypeId
    {
        /// <summary>Swedish Redlists</summary>
        SwedishRedlist = 1,
        /// <summary>HELCOM Redlists</summary>
        HELCOMRedlist = 2
    }

    /// <summary>
    /// This class represents a Period type.
    /// </summary>
    sealed public class PeriodType : DataId, IListableItem, IComparable
    {
        private String _name;
        private String _description;

        /// <summary>
        /// Constructor of a period type object.
        /// </summary>
        /// <param name="id">Id of the period type.</param>
        /// <param name="name">Name of the period type.</param>
        /// <param name="description">description of the period type.</param>
        public PeriodType(Int32 id, String name, String description)
            : base(id)
        {
            _name = name;
            _description = description;
        }

        /// <summary>
        /// Get a description for this period type. 
        /// </summary>
        public String Description
        {
            get { return _description; }
        }

        /// <summary>
        /// Get name for this period type.
        /// </summary>
        public String Name
        {
            get { return _name; }
        }


        #region IListableItem Members

        /// <summary>
        /// A string usable as a display name
        /// </summary>
        public string Label
        {
            get { return this._name; }
        }

        #endregion

        #region IComparable Members

        int IComparable.CompareTo(object obj)
        {
            PeriodType otherPeriodType = (PeriodType)obj;
            return this.Name.CompareTo(otherPeriodType.Name);
        }

        #endregion
    }
}
