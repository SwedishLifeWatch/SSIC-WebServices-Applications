using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// List class for the Taxon class.
    /// </summary>
    [Serializable]
    public class TaxonList : DataIdList
    {
        /// <summary>
        /// Constructor for the TaxonList class.
        /// </summary>
        public TaxonList()
            : this(false)
        {
        }

        /// <summary>
        /// Constructor for the TaxonList class.
        /// </summary>
        /// <param name='optimize'>
        /// Indicates if speed optimization should be turned on.
        /// This optimization assumes that each id only occurs once
        /// in the list.
        /// </param>
        public TaxonList(Boolean optimize)
            : base(optimize)
        {
        }

        /// <summary>
        /// Get Taxon with specified id.
        /// </summary>
        /// <param name='taxonId'>Id of requested taxon.</param>
        /// <returns>Requested taxon.</returns>
        /// <exception cref="ArgumentException">Thrown if no data has the requested id.</exception>
        public Taxon Get(Int32 taxonId)
        {
            return (Taxon)(GetById(taxonId));
        }

        /// <summary>
        /// Get index for specified taxon.
        /// </summary>
        /// <param name='taxon'>Searched taxon.</param>
        /// <returns>Index for specified taxon.</returns>
        public Int32 GetIndex(Taxon taxon)
        {
            Int32 taxonIndex;

            if (this.IsNotEmpty())
            {
                for (taxonIndex = 0; taxonIndex < this.Count; taxonIndex++)
                {
                    if (this[taxonIndex].Id == taxon.Id)
                    {
                        return taxonIndex;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// Get/set Taxon by list index.
        /// </summary>
        public new Taxon this[Int32 index]
        {
            get
            {
                return (Taxon)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }



        /// <summary>
        /// Get a subset of this taxon list by search string
        /// </summary>
        /// <param name="searchString">Search string</param>
        /// <param name="comparisonType">Type of string comparison</param>
        /// <returns>A taxon list</returns>
        public TaxonList GetTaxaBySearchString(String searchString, StringComparison comparisonType)
        {
            TaxonList taxa = new TaxonList();
            var subset = from Taxon taxon in this
                         where taxon.ScientificName.StartsWith(searchString, comparisonType)
                         orderby taxon.ScientificName ascending
                         select taxon;
            taxa.AddRange(subset.ToArray());
            return taxa;
        }

        /// <summary>
        /// Writes one row to the xml format.
        /// </summary>
        /// <param name='xmlDoc'>The xml document that the row node will be written to.</param>
        /// <param name="tableNode">The table node in the Xml document that the row node should be appended to</param>
        protected override void WriteXmlRows(XmlDocument xmlDoc, XmlNode tableNode)
        {

            foreach (Taxon taxon in this)
            {
                XmlNode rowNode = xmlDoc.CreateNode(XmlNodeType.Element, "Row", "urn:schemas-microsoft-com:office:spreadsheet");

                XmlNode cellIdNode = xmlDoc.CreateNode(XmlNodeType.Element, "Cell", "urn:schemas-microsoft-com:office:spreadsheet");
                rowNode.AppendChild(cellIdNode);

                XmlNode dataIdNode = xmlDoc.CreateNode(XmlNodeType.Element, "Data", "urn:schemas-microsoft-com:office:spreadsheet");
                dataIdNode.Attributes.Append(xmlDoc.CreateAttribute("ss:Type", "urn:schemas-microsoft-com:office:spreadsheet"));
                dataIdNode.Attributes[0].Value = "Number";
                dataIdNode.InnerText = Convert.ToString(taxon.Id);
                cellIdNode.AppendChild(dataIdNode);

                XmlNode cellNameNode = xmlDoc.CreateNode(XmlNodeType.Element, "Cell", "urn:schemas-microsoft-com:office:spreadsheet");
                rowNode.AppendChild(cellNameNode);

                XmlNode dataNameNode = xmlDoc.CreateNode(XmlNodeType.Element, "Data", "urn:schemas-microsoft-com:office:spreadsheet");
                dataNameNode.Attributes.Append(xmlDoc.CreateAttribute("ss:Type", "urn:schemas-microsoft-com:office:spreadsheet"));
                dataNameNode.Attributes[0].Value = "String";
                dataNameNode.InnerText = taxon.Label;
                cellNameNode.AppendChild(dataNameNode);

                tableNode.AppendChild(rowNode);
                
            }
        }

        /// <summary>
        /// Reades one row from the xml format.
        /// </summary>
        /// <param name='xmlDoc'>The xml document that the row node will be read from.</param>
        /// <param name="nsmgr">Namespacemanager containing all namespaces used by the excel compatible xml format</param>
        protected override void ReadXmlRows(XmlDocument xmlDoc, XmlNamespaceManager nsmgr)
        {
            List<int> taxonIDs = new List<int>();

            XmlNodeList rowNodes = xmlDoc.SelectNodes("/ss:Workbook/ss:Worksheet/ss:Table/ss:Row", nsmgr);
            foreach (XmlNode rowNode in rowNodes)
            {
                XmlNode dataNode = rowNode.SelectSingleNode("ss:Cell/ss:Data", nsmgr);
                int taxonId = Convert.ToInt32(dataNode.InnerText);
                taxonIDs.Add(taxonId);
            }

            this.AddRange(TaxonManager.GetTaxa(taxonIDs, ArtDatabanken.Data.WebService.TaxonInformationType.Basic));
        }
    }
}
