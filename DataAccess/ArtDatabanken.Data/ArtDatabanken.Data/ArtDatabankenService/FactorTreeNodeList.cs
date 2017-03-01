using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// List class for the FactorTreeNode class.
    /// </summary>
    [Serializable]
    public class FactorTreeNodeList : DataIdList
    {
        /// <summary>
        /// Constructor for the FactorTreeNodeList class.
        /// </summary>
        public FactorTreeNodeList()
            : base(true)
        {
        }

        /// <summary>
        /// Get Factor with specified id.
        /// </summary>
        /// <param name='factorId'>Id of factor belonging to requested factor tree node.</param>
        /// <returns>Requested factor tree node.</returns>
        /// <exception cref="ArgumentException">Thrown if no data has the requested id.</exception>
        public FactorTreeNode Get(Int32 factorId)
        {
            return (FactorTreeNode)(GetById(factorId));
        }

        /// <summary>
        /// Get/set FactorTreeNode by list index.
        /// </summary>
        public new FactorTreeNode this[Int32 index]
        {
            get
            {
                return (FactorTreeNode)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }
    }
}
