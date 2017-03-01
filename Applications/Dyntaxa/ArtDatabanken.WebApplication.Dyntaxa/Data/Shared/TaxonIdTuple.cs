using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Shared
{
    public class TaxonIdTuple
    {   
        /// <summary>
        /// The Taxon Id
        /// example: "mammalia"
        ///       or "4000107"
        /// </summary>
        public string TaxonId { get; set; }

        /// <summary>
        /// The Taxon Id integer
        /// example: 400107
        /// </summary>
        public int? Id { get; set; }

        protected TaxonIdTuple()
        {
        }   
        
        public static TaxonIdTuple Create(string taxonId, int id)
        {
            return new TaxonIdTuple { TaxonId = taxonId, Id = id };
        }

        public static TaxonIdTuple CreateFromString(string taxonId)
        {
            return new TaxonIdTuple { TaxonId = taxonId };
        }

        public static TaxonIdTuple CreateFromId(int id)
        {
            return new TaxonIdTuple { Id = id };
        }             

        public static TaxonIdTuple Create()
        {
            return new TaxonIdTuple();
        }
    }
}
