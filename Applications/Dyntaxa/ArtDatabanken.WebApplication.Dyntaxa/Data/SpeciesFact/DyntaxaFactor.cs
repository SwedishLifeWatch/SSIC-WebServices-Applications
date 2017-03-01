using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.WebApplication.Dyntaxa.Data;

namespace ArtDatabanken.WebApplication.Dyntaxa
{
    public class DyntaxaFactor
    {
        private DyntaxaFactorOrigin factorOrigin;

        private DyntaxaFactorUpdateMode factorUpdateMode;

        private bool isLeaf = true;

        private bool isTaxonomic = true;

        private bool isPublic = false;

        private bool isPeriodic = false;

        private string id = string.Empty;

        private int sortOrder = 0;

        private string label = string.Empty;
        private int factorId = 0;

        public DyntaxaFactor(
            string id, 
            string label, 
            bool isLeaf, 
            bool isPeriodic, 
            int sortOrder, 
            bool isPublic, 
            bool isTaxonomic,
            DyntaxaFactorOrigin factorOrigin, 
            DyntaxaFactorUpdateMode factorUpdateMode, 
            int factorId)
        {
            this.factorOrigin = factorOrigin;
            this.factorUpdateMode = factorUpdateMode;
            this.isLeaf = isLeaf;
            // if (Convert.ToInt32(id) == 12011)
            // {
            //    int i = 0;
            // }
            this.id = id;
            this.label = label;
            this.isPeriodic = isPeriodic;
            this.sortOrder = sortOrder;
            this.isPublic = isPublic;
            this.isTaxonomic = isTaxonomic;
            this.factorId = factorId;
        }

        //public DyntaxaFactor(string id, string label)
        //{
          
        //    this.id = id;
        //    this.label = label;

        //}

        protected DyntaxaFactor()
        {
        }

        public bool IsLeaf
        {
            get
            {
                return isLeaf;
            }
            set
            {
                isLeaf = value;
            }
        }

        public bool IsPeriodic
        {
            get
            {
                return isPeriodic;
            }
            set
            {
                isPeriodic = value;
            }
        }

        public bool IsPublic
        {
            get
            {
                return isPublic;
            }
            set
            {
                isPublic = value;
            }
        }

        public bool IsTaxonomic
        {
            get
            {
                return isTaxonomic;
            }
            set
            {
                isTaxonomic = value;
            }
        }

        public string Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        public int FactorId
        {
            get
            {
                return factorId;
            }
            set
            {
                factorId = value;
            }
        }

        public int SortOrder
        {
            get
            {
                return sortOrder;
            }
            set
            {
                sortOrder = value;
            }
        }

        public string Label
        {
            get
            {
                return label;
            }
            set
            {
                label = value;
            }
        }

        public ArtDatabanken.WebApplication.Dyntaxa.Data.DyntaxaFactorUpdateMode FactorUpdateMode
        {
            get
            {
                return factorUpdateMode;
            }
        }

        public DyntaxaFactorOrigin FactorOrigin
        {
            get
            {
                return factorOrigin;
            }
        }

       public bool IsHost { get; set; }

        public int HostId { get; set; }
    }

    /// <summary>
    ///  Enum that contains factor header type ids.
    /// </summary>
    public enum DyntaxaFactorHeaderType
    {
        /// <summary>
        /// MainHeader
        /// </summary>
        Main = 0,

        /// <summary>
        /// Level 1 header
        /// </summary>
        Level1 = 1,

        /// <summary>
        /// Level 3 header
        /// </summary>
        Level2 = 2,

        /// <summary>
        /// Level 3 header
        /// </summary>
        Level3 = 3,

        /// <summary>
        /// Level 4 header
        /// </summary>
        Level4 = 4
    }
}
