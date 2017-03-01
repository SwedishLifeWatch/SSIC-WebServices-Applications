using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.WebApplication.Dyntaxa
{
    /// <summary>
    /// Class for factor origin data
    /// </summary>
    public class DyntaxaFactorOrigin
    {
        private string originName = string.Empty;
        private int originId = -1;

        public DyntaxaFactorOrigin(int originId, string originName)
        {
            this.originId = originId;
            this.originName = originName;
        }

        public int OriginId
        {
            get
            {
                return originId;
            }
            set
            {
                originId = value;
            }
        }

        public string OriginName
        {
            get
            {
                return originName;
            }
            set
            {
                originName = value;
            }
        }
    }
}
