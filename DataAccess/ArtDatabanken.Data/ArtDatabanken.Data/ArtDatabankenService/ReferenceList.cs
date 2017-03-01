using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// List class for the Reference class.
    /// </summary>
    [Serializable]
    public class ReferenceList : DataIdList
    {
        /// <summary>
        /// Constructor for the ReferenceList class.
        /// </summary>
        public ReferenceList()
            : this(false)
        {
        }

        /// <summary>
        /// Constructor for the ReferenceList class.
        /// </summary>
        /// <param name='optimize'>
        /// Indicates if speed optimization should be turned on.
        /// This optimization assumes that each id only occurs once
        /// in the list.
        /// </param>
        public ReferenceList(Boolean optimize)
            : base(optimize)
        {
        }

        /// <summary>
        /// Get Reference with specified id.
        /// </summary>
        /// <returns>Requested references or not-a-valid-ref reference object if reference doesn't exist.</returns>
        public Reference Get(Int32 referenceId )
        {
            Reference reference;
            try
            {
                reference = (Reference)(GetById(referenceId));
            }
            catch (Exception)
            {
                // reference not found in artfakta
                reference = new Reference(referenceId, "Not a valid reference.", 0, "");
            }
            return reference;
        }

        /// <summary>
        /// Checks if a Reference is in the list
        /// </summary>
        /// <param name='reference'>Object to be checked.</param>
        /// <returns>Boolean value indicating if the Reference exists in the list or not.</returns>
        public Boolean Exists(Reference reference)
        {
            return base.Exists(reference);
        }

        /// <summary>
        /// Get/set Reference by list index.
        /// </summary>
        public new Reference this[Int32 index]
        {
            get
            {
                return (Reference)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }

        /// <summary>
        /// Writes rows to XML
        /// </summary>
        /// <param name='tableNode'>Nod.</param>
        /// <param name='xmlDoc'>Dokument.</param>
        protected override void WriteXmlRows(XmlDocument xmlDoc, XmlNode tableNode)
        {

            foreach (Reference reference in this)
            {
                XmlNode rowNode = xmlDoc.CreateNode(XmlNodeType.Element, "Row", "urn:schemas-microsoft-com:office:spreadsheet");

                XmlNode cellIdNode = xmlDoc.CreateNode(XmlNodeType.Element, "Cell", "urn:schemas-microsoft-com:office:spreadsheet");
                rowNode.AppendChild(cellIdNode);

                XmlNode dataIdNode = xmlDoc.CreateNode(XmlNodeType.Element, "Data", "urn:schemas-microsoft-com:office:spreadsheet");
                dataIdNode.Attributes.Append(xmlDoc.CreateAttribute("ss:Type", "urn:schemas-microsoft-com:office:spreadsheet"));
                dataIdNode.Attributes[0].Value = "Number";
                dataIdNode.InnerText = Convert.ToString(reference.Id);
                cellIdNode.AppendChild(dataIdNode);

                XmlNode cellNameNode = xmlDoc.CreateNode(XmlNodeType.Element, "Cell", "urn:schemas-microsoft-com:office:spreadsheet");
                rowNode.AppendChild(cellNameNode);

                XmlNode dataNameNode = xmlDoc.CreateNode(XmlNodeType.Element, "Data", "urn:schemas-microsoft-com:office:spreadsheet");
                dataNameNode.Attributes.Append(xmlDoc.CreateAttribute("ss:Type", "urn:schemas-microsoft-com:office:spreadsheet"));
                dataNameNode.Attributes[0].Value = "String";
                dataNameNode.InnerText = reference.Label;
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
                this.Add(ReferenceManager.GetReference(Convert.ToInt32(dataNode.InnerText)));
            }
        }
    }
}
