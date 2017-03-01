using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.WebApplication.Dyntaxa
{
    /// <summary>
    /// Class for factor quality data
    /// </summary>
    public class DyntaxaFactorQuality
    {
        private string quality = string.Empty;

        private int qualityId = 0;

        private IList<KeyValuePair<int, string>> qualities = null;

        //private string referenceName = string.Empty;
        //private int referenceId = 0;

        public DyntaxaFactorQuality(int qualityId, string qualityName, IList<KeyValuePair<int, string>> qualities)
        {
            this.qualityId = qualityId;
            this.quality = qualityName;
            this.qualities = qualities;
            //this.referenceName = referenceName;
            //this.referenceId = referenceId;
        }

        public string Quality
        { get { return quality; } }

        public int QualityId
        { get { return qualityId; } }

        public IList<KeyValuePair<int, string>> Qualities
        {
            get { return qualities; }
        }

        //public string ReferenceName
        //{ get { return referenceName; } }

        //public int ReferenceId
        //{ get { return referenceId; } }
    }
}
