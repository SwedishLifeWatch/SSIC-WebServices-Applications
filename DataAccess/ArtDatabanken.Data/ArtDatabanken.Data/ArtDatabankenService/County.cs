using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    ///  This class represents a county or a part of a county.
    /// </summary>
    [Serializable()]
    public class County : DataId
    {
        private Boolean _hasNumber;
        private Boolean _isCountyPart;
        private Int32 _number;
        private Int32 _partOfCountyId;
        private String _identifier;
        private String _name;

        /// <summary>
        /// Create a county instance.
        /// </summary>
        /// <param name='id'>Id for county.</param>
        /// <param name='name'>Name for county.</param>
        /// <param name='identifier'>Identifier for county.</param>
        /// <param name='hasNumber'>Indicates if this county has a number.</param>
        /// <param name='number'>Number for county.</param>
        /// <param name='isCountyPart'>Indicates if this county is part of another county.</param>
        /// <param name='partOfCountyId'>Id for county that this county is part of.</param>
        public County(Int32 id,
                      String name,
                      String identifier,
                      Boolean hasNumber,
                      Int32 number,
                      Boolean isCountyPart,
                      Int32 partOfCountyId)
            : base(id)
        {
            _name = name;
            _identifier = identifier;
            _hasNumber = hasNumber;
            _number = number;
            _isCountyPart = isCountyPart;
            _partOfCountyId = partOfCountyId;
        }

        /// <summary>
        /// Test if this county has a number.
        /// </summary>
        public Boolean HasNumber
        {
            get { return _hasNumber; }
        }

        /// <summary>
        /// Get identifier for this county.
        /// </summary>
        public String Identifier
        {
            get { return _identifier; }
        }

        /// <summary>
        /// Test if this county is a part of another county.
        /// </summary>
        public Boolean IsCountyPart
        {
            get { return _isCountyPart; }
        }

        /// <summary>
        /// Get name for this county.
        /// </summary>
        public String Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Get number for this county.
        /// </summary>
        public Int32 Number
        {
            get { return _number; }
        }

        /// <summary>
        /// Get county that this county is part of.
        /// </summary>
        public County PartOfCounty
        {
            get
            {
                return GeographicManager.GetCounty(_partOfCountyId);
            }
        }

        /// <summary>
        /// Get id for county that this county is part of.
        /// </summary>
        public Int32 PartOfCountyId
        {
            get { return _partOfCountyId; }
        }
    }
}
