using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    ///  Enum that contains period ids.
    /// </summary>
    public enum PeriodId
    {
        /// <summary>Year 2000</summary>
        Year2000 = 1,
        /// <summary>Year 2005</summary>
        Year2005 = 2,
        /// <summary>Year 2010</summary>
        Year2010 = 3,
        /// <summary>Year 2015</summary>
        Year2015 = 4,
        /// <summary>HELCOM Year2013</summary>
        HelcomYear2013 = 5
    }

    /// <summary>
    ///  This class represents a period.
    /// </summary>
    [Serializable]
    public sealed class Period : DataId, IListableItem, IComparable
    {
        private String _name;
        private String _information;
        private DateTime _stopUpdate;
        private Int32 _periodTypeId;
        private Int32 _year;


        /// <summary>
        /// Constructor of a period object.
        /// </summary>
        /// <param name="id">Id of the period.</param>
        /// <param name="year">Periodic publication year</param>
        /// <param name="name">Name of the period.</param>
        /// <param name="information">Descriptive text about the period.</param>
        /// <param name="stopUpdate">Date when editing should stop.</param>
        /// <param name="periodTypeId">Id of period type.</param>
        public Period(Int32 id, Int32 year, String name, String information, DateTime stopUpdate, Int32 periodTypeId)
            : base(id)
        {
            _name = name;
            _year = year;
            _information = information;
            _stopUpdate = stopUpdate;
            _periodTypeId = periodTypeId;
        }

        /// <summary>
        /// Test if update of species facts belonging to
        /// this period is allowed. 
        /// </summary>
        public Boolean AllowUpdate
        {
            get
            {
                return DateTime.Today <= _stopUpdate;
            }
        }

        /// <summary>
        /// Get information for this period. 
        /// </summary>
        public String Information
        {
            get { return _information; }
        }

        /// <summary>
        /// Get name for this period.
        /// </summary>
        public  String Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Type of period.
        /// </summary>
        public PeriodType PeriodType
        {
            get { return PeriodManager.GetPeriodType(_periodTypeId); }
        }

        /// <summary>
        /// Get date for end of period updating.
        /// </summary>
        public DateTime StopUpdate
        {
            get { return _stopUpdate; }
        }

        /// <summary>
        /// Year representing the periodic publication.
        /// </summary>
        public Int32 Year
        {
            get { return _year; }
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
            Period otherPeriod = (Period)obj;
            return this.Name.CompareTo(otherPeriod.Name);
        }

        #endregion
    }
}
