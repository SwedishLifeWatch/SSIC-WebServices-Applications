using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// List class for the Factor class.
    /// </summary>
    [Serializable]
    public class FactorList : DataIdList
    {
        /// <summary>
        /// Constructor for the FactorList class.
        /// </summary>
        public FactorList()
            : this(true)
        {
        }

        /// <summary>
        /// Constructor for the FactorList class.
        /// </summary>
        /// <param name='optimize'>
        /// Indicates if speed optimization should be turned on.
        /// This optimization assumes that each id only occurs once
        /// in the list.
        /// </param>
        public FactorList(Boolean optimize)
            : base(optimize)
        {
        }

        /// <summary>
        /// Get Factor with specified factor id.
        /// </summary>
        /// <param name='factorId'>Id of requested factor.</param>
        /// <returns>Requested factor.</returns>
        /// <exception cref="ArgumentException">Thrown if no data has the requested id.</exception>
        public Factor Get(Int32 factorId)
        {
            return (Factor)(GetById(factorId));
        }

        /// <summary>
        /// Get/set Factor by list index.
        /// </summary>
        public new Factor this[Int32 index]
        {
            get
            {
                return (Factor)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }

        /// <summary>
        /// Get a subset of this factor list by search string
        /// </summary>
        /// <param name="searchString">Search string</param>
        /// <param name="comparisonType">Type of string comparison</param>
        /// <returns>A factor list</returns>
        public FactorList GetFactorsBySearchString(String searchString, StringComparison comparisonType)
        {
            FactorList factors = new FactorList();
            var subset = from Factor factor in this
                         where factor.Label.StartsWith(searchString, comparisonType)
                         orderby factor.Label ascending
                         select factor;
            factors.AddRange(subset.ToArray());
            return factors;
        }

        /// <summary>
        /// Writes one row to the xml format.
        /// </summary>
        /// <param name='xmlDoc'>The xml document that the row node will be written to.</param>
        /// <param name="tableNode">The table node in the Xml document that the row node should be appended to</param>
        protected override void WriteXmlRows(XmlDocument xmlDoc, XmlNode tableNode)
        {

            foreach (Factor factor in this)
            {
                XmlNode rowNode = xmlDoc.CreateNode(XmlNodeType.Element, "Row", "urn:schemas-microsoft-com:office:spreadsheet");

                XmlNode cellIdNode = xmlDoc.CreateNode(XmlNodeType.Element, "Cell", "urn:schemas-microsoft-com:office:spreadsheet");
                rowNode.AppendChild(cellIdNode);

                XmlNode dataIdNode = xmlDoc.CreateNode(XmlNodeType.Element, "Data", "urn:schemas-microsoft-com:office:spreadsheet");
                dataIdNode.Attributes.Append(xmlDoc.CreateAttribute("ss:Type", "urn:schemas-microsoft-com:office:spreadsheet"));
                dataIdNode.Attributes[0].Value = "Number";
                dataIdNode.InnerText = Convert.ToString(factor.Id);
                cellIdNode.AppendChild(dataIdNode);

                XmlNode cellNameNode = xmlDoc.CreateNode(XmlNodeType.Element, "Cell", "urn:schemas-microsoft-com:office:spreadsheet");
                rowNode.AppendChild(cellNameNode);

                XmlNode dataNameNode = xmlDoc.CreateNode(XmlNodeType.Element, "Data", "urn:schemas-microsoft-com:office:spreadsheet");
                dataNameNode.Attributes.Append(xmlDoc.CreateAttribute("ss:Type", "urn:schemas-microsoft-com:office:spreadsheet"));
                dataNameNode.Attributes[0].Value = "String";
                dataNameNode.InnerText = factor.Label;
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

            XmlNodeList rowNodes = xmlDoc.SelectNodes("/ss:Workbook/ss:Worksheet/ss:Table/ss:Row", nsmgr);
            foreach (XmlNode rowNode in rowNodes)
            {
                XmlNode dataNode = rowNode.SelectSingleNode("ss:Cell/ss:Data", nsmgr);
                this.Add(FactorManager.GetFactor(Convert.ToInt32(dataNode.InnerText)));
            }
        }
    }
}
