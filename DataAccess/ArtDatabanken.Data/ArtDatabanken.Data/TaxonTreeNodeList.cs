using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// List class for the ITaxonTreeNode interface.
    /// </summary>
    [Serializable]
    public class TaxonTreeNodeList : DataId32List<ITaxonTreeNode>
    {
        /// <summary>
        /// Constructor for the TaxonTreeNodeList class.
        /// </summary>
        public TaxonTreeNodeList()
            : this(false)
        {
        }

        /// <summary>
        /// Constructor for the TaxonTreeNodeList class.
        /// </summary>
        /// <param name='optimize'>
        /// Indicates if speed optimization should be turned on.
        /// This optimization assumes that each id only occurs once
        /// in the list.
        /// </param>
        public TaxonTreeNodeList(Boolean optimize)
            : base(optimize)
        {
            
        }
    }
}
