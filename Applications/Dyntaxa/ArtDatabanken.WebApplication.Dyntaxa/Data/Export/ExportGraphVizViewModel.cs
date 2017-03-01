using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Export
{
    /// <summary>
    /// Export GraphViz view model.
    /// </summary>
    public class ExportGraphVizViewModel
    {
        /// <summary>
        /// Type of the Column delimiter
        /// </summary>
        [LocalizedDisplayName("MatchOptionsInputRowDelimeterLabel", NameResourceType = typeof(Resources.DyntaxaResource))]
        public MatchTaxonRowDelimiter RowDelimiter { get; set; }

        /// <summary>
        /// String pasted into textarea
        /// </summary>
        [LocalizedDisplayName("ExportDatabaseClipboard", NameResourceType = typeof(Resources.DyntaxaResource))]
        public string ClipBoard { get; set; }

        /// <summary>
        /// The input taxon ids.
        /// </summary>
        public List<int> InputTaxonIds { get; set; }

        /// <summary>
        /// The tree iteration mode.
        /// </summary>
        public TaxonRelationsTreeIterationMode TreeIterationMode { get; set; }

        /// <summary>
        /// The relation type mode.
        /// </summary>
        public TaxonRelationsTreeRelationTypeMode RelationTypeMode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether lumps and splits should be included in graph.
        /// </summary>        
        public bool IncludeLumpSplits { get; set; }        

        /// <summary>
        /// Gets or sets a value indicating whether relation identifier should be visible.
        /// </summary>        
        public bool ShowRelationId { get; set; }

        //public bool IncludeChildrenSecondaryParents { get; set; }

        /// <summary>
        /// Gets the taxon ids from string.
        /// </summary>
        /// <returns>List of taxon ids.</returns>
        public List<int> GetTaxonIdsFromString()
        {
            string[] stringArray = null;
            switch (this.RowDelimiter)
            {
                case MatchTaxonRowDelimiter.ReturnLinefeed:
                    stringArray = this.ClipBoard.Split('\n');
                    break;

                case MatchTaxonRowDelimiter.Semicolon:
                    stringArray = this.ClipBoard.Split(';');
                    break;

                case MatchTaxonRowDelimiter.Tab:
                    stringArray = this.ClipBoard.Split('\t');
                    break;

                case MatchTaxonRowDelimiter.VerticalBar:
                    stringArray = this.ClipBoard.Split('|');
                    break;

                default:
                    stringArray = this.ClipBoard.Split('\n');
                    break;
            }
            var taxonIds = new List<int>();
            foreach (string str in stringArray)
            {
                int number;
                if (int.TryParse(str, out number))
                {
                    taxonIds.Add(number);
                }
            }
            return taxonIds;
        }

        /// <summary>
        /// Gets the taxa.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns></returns>
        public List<ITaxon> GetTaxa(IUserContext userContext)
        {
            List<int> taxonIds = GetTaxonIdsFromString();
            TaxonList taxonList = CoreData.TaxonManager.GetTaxa(userContext, taxonIds);
            List<ITaxon> taxa = taxonList.Cast<ITaxon>().ToList();
            return taxa;
        }        
    }
}