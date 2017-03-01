using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;


namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// List class for the IndividualCategory class.
    /// </summary>
    [Serializable]
    public class IndividualCategoryList : DataIdList
    {
        /// <summary>
        /// Constructor for the IndividualCategoryList class.
        /// </summary>
        public IndividualCategoryList()
            : this(false)
        {
        }

        /// <summary>
        /// Constructor for the IndividualCategoryList class.
        /// </summary>
        /// <param name='optimize'>
        /// Indicates if speed optimization should be turned on.
        /// This optimization assumes that each id only occurs once
        /// in the list.
        /// </param>
        public IndividualCategoryList(Boolean optimize)
            : base(optimize)
        {
        }

        /// <summary>
        /// Get IndividualCategory using specified id.
        /// </summary>
        /// <returns>Requested individual categories.</returns>
        /// <exception cref="ArgumentException">Thrown if no data has the requested id.</exception>
        public IndividualCategory Get(Int32 individualCategoryId)
        {
            return (IndividualCategory)(GetById(individualCategoryId));
        }

        /// <summary>
        /// Get/set IndividualCategory by list index.
        /// </summary>
        public new IndividualCategory this[Int32 index]
        {
            get
            {
                return (IndividualCategory)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }

        /// <summary>
        /// Writes one row to the xml format.
        /// </summary>
        /// <param name='xmlDoc'>The xml document that the row node will be written to.</param>
        /// <param name="tableNode">The table node in the Xml document that the row node should be appended to</param>
        protected override void WriteXmlRows(XmlDocument xmlDoc, XmlNode tableNode)
        {

            foreach (IndividualCategory category in this)
            {
                XmlNode rowNode = xmlDoc.CreateNode(XmlNodeType.Element, "Row", "urn:schemas-microsoft-com:office:spreadsheet");

                XmlNode cellIdNode = xmlDoc.CreateNode(XmlNodeType.Element, "Cell", "urn:schemas-microsoft-com:office:spreadsheet");
                rowNode.AppendChild(cellIdNode);

                XmlNode dataIdNode = xmlDoc.CreateNode(XmlNodeType.Element, "Data", "urn:schemas-microsoft-com:office:spreadsheet");
                dataIdNode.Attributes.Append(xmlDoc.CreateAttribute("ss:Type", "urn:schemas-microsoft-com:office:spreadsheet"));
                dataIdNode.Attributes[0].Value = "Number";
                dataIdNode.InnerText = Convert.ToString(category.Id);
                cellIdNode.AppendChild(dataIdNode);

                XmlNode cellNameNode = xmlDoc.CreateNode(XmlNodeType.Element, "Cell", "urn:schemas-microsoft-com:office:spreadsheet");
                rowNode.AppendChild(cellNameNode);

                XmlNode dataNameNode = xmlDoc.CreateNode(XmlNodeType.Element, "Data", "urn:schemas-microsoft-com:office:spreadsheet");
                dataNameNode.Attributes.Append(xmlDoc.CreateAttribute("ss:Type", "urn:schemas-microsoft-com:office:spreadsheet"));
                dataNameNode.Attributes[0].Value = "String";
                dataNameNode.InnerText = category.Label;
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
                this.Add(IndividualCategoryManager.GetIndividualCategory(Convert.ToInt32(dataNode.InnerText)));
            }
        }
    }
}
