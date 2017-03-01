using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    ///  This class represents a province or a part of a province.
    /// </summary>
    [Serializable()]
    public class Province : DataId
    {
        private Boolean _isProvincePart;
        private Int32 _partOfProvinceId;
        private String _identifier;
        private String _name;

        /// <summary>
        /// Create a Province instance.
        /// </summary>
        /// <param name='id'>Id for province.</param>
        /// <param name='name'>Name for province.</param>
        /// <param name='identifier'>Identifier for province.</param>
        /// <param name='isProvincePart'>Indicates if this province is part of another province.</param>
        /// <param name='partOfProvinceId'>Id for province that this province is part of.</param>
        public Province(Int32 id,
                        String name,
                        String identifier,
                        Boolean isProvincePart,
                        Int32 partOfProvinceId)
            : base(id)
        {
            _name = name;
            _identifier = identifier;
            _isProvincePart = isProvincePart;
            _partOfProvinceId = partOfProvinceId;
        }

        /// <summary>
        /// Get identifier for this province.
        /// </summary>
        public String Identifier
        {
            get { return _identifier; }
        }

        /// <summary>
        /// Test if this province is a part of another province.
        /// </summary>
        public Boolean IsProvincePart
        {
            get { return _isProvincePart; }
        }

        /// <summary>
        /// Get name for this province.
        /// </summary>
        public String Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Get id for province that this province is part of.
        /// </summary>
        public Int32 PartOfProvinceId
        {
            get { return _partOfProvinceId; }
        }

        /// <summary>
        /// Get province that this province is part of.
        /// </summary>
        public Province PartOfProvince
        {
            get
            {
                return GeographicManager.GetProvince(_partOfProvinceId);
            }
        }
    }
}
