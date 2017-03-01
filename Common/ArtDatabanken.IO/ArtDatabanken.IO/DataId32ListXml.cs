using System;
using System.Xml;
using ArtDatabanken.Data;

namespace ArtDatabanken.IO
{
    /// <summary>
    /// Handle conversion to and from XML.
    /// </summary>
    public abstract class DataId32ListXml
    {
        /// <summary>
        /// List with data.
        /// </summary>
        protected DataId32List<IDataId32> DataId32List { get; set; }

        /// <summary>
        /// Builds a new list from an excel xml structure.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="xml">Excel xml input.</param>
        public void FromExcelXml(IUserContext userContext, String xml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsmgr.AddNamespace("", "urn:schemas-microsoft-com:office:spreadsheet");
            nsmgr.AddNamespace("html", "http://www.w3.org/TR/REC-html40");
            nsmgr.AddNamespace("o", "urn:schemas-microsoft-com:office:office");
            nsmgr.AddNamespace("ss", "urn:schemas-microsoft-com:office:spreadsheet");
            nsmgr.AddNamespace("x", "urn:schemas-microsoft-com:office:excel");

            ReadExcelXmlRows(userContext, xmlDoc, nsmgr);
        }

        /// <summary>
        /// Read rows from the excel xml format.
        /// Overridden by each class that inherits this class.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name='xmlDoc'>The xml document that the row node will be read from.</param>
        /// <param name="nsmgr">Namespacemanager containing all namespaces used by the excel compatible xml format</param>
        protected abstract void ReadExcelXmlRows(IUserContext userContext,
                                                 XmlDocument xmlDoc,
                                                 XmlNamespaceManager nsmgr);

        /// <summary>
        /// Converts the list to an excel compatible xml format.
        /// </summary>
        /// <returns>List in an excel compatible xml format.</returns>       
        public String ToExcelXml()
        {
            Int32 count;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.InnerXml = "<?xml version=\"1.0\"?><?mso-application progid=\"Excel.Sheet\"?><Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:x=\"urn:schemas-microsoft-com:office:excel\" xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\" xmlns:html=\"http://www.w3.org/TR/REC-html40\"><Worksheet ss:Name=\"Blad1\"></Worksheet></Workbook>";

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsmgr.AddNamespace("", "urn:schemas-microsoft-com:office:spreadsheet");
            nsmgr.AddNamespace("html", "http://www.w3.org/TR/REC-html40");
            nsmgr.AddNamespace("o", "urn:schemas-microsoft-com:office:office");
            nsmgr.AddNamespace("ss", "urn:schemas-microsoft-com:office:spreadsheet");
            nsmgr.AddNamespace("x", "urn:schemas-microsoft-com:office:excel");

            XmlNode workBookNode = xmlDoc.SelectSingleNode("/ss:Workbook", nsmgr);
            XmlNode versionNode = xmlDoc.CreateNode(XmlNodeType.Element, "EVAVersion", "");
            versionNode.InnerText = "1.0";
            workBookNode.AppendChild(versionNode);

            XmlNode workSheetNode = xmlDoc.SelectSingleNode("/ss:Workbook/ss:Worksheet", nsmgr);
            XmlNode tableNode = xmlDoc.CreateNode(XmlNodeType.Element, "Table", "urn:schemas-microsoft-com:office:spreadsheet");
            tableNode.Attributes.Append(xmlDoc.CreateAttribute("ss:ExpandedColumnCount", "urn:schemas-microsoft-com:office:spreadsheet"));
            tableNode.Attributes[0].Value = "20";
            tableNode.Attributes.Append(xmlDoc.CreateAttribute("ss:ExpandedRowCount", "urn:schemas-microsoft-com:office:spreadsheet"));
            count = 0;
            if (DataId32List.IsNotEmpty())
            {
                count = DataId32List.Count;
            }

            tableNode.Attributes[1].Value = Convert.ToString(count);
            tableNode.Attributes.Append(xmlDoc.CreateAttribute("x:FullColumns", "urn:schemas-microsoft-com:office:excel"));
            tableNode.Attributes[2].Value = "1";
            tableNode.Attributes.Append(xmlDoc.CreateAttribute("x:FullRows", "urn:schemas-microsoft-com:office:excel"));
            tableNode.Attributes[3].Value = "1";

            WriteExcelXmlRows(xmlDoc, tableNode);

            workSheetNode.AppendChild(tableNode);
            return xmlDoc.InnerXml;
        }

        /// <summary>
        /// Writes rows to the xml format.
        /// Overridden by each class that inherits this class.
        /// </summary>
        /// <param name='xmlDoc'>The xml document that the row node will be written to.</param>
        /// <param name="tableNode">The table node in the Xml document that the row node should be appended to</param>
        protected abstract void WriteExcelXmlRows(XmlDocument xmlDoc,
                                                  XmlNode tableNode);
    }
}
