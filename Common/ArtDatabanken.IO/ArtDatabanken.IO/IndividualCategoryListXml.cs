using System.Xml;
using ArtDatabanken.Data;

namespace ArtDatabanken.IO
{
    /// <summary>
    /// Handle conversion to and from XML for individual categories.
    /// </summary>
    public class IndividualCategoryListXml : DataId32ListXml
    {
        private IndividualCategoryList _individualCategories;

        /// <summary>
        /// List with individual categories.
        /// </summary>
        public IndividualCategoryList IndividualCategories
        {
            get
            {
                return _individualCategories;
            }

            set
            {
                _individualCategories = value;
                if (value.IsNull())
                {
                    DataId32List = null;
                }
                else
                {
                    DataId32List = value.GetDataId32List();
                }
            }
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
        protected override void ReadExcelXmlRows(IUserContext userContext,
                                                 XmlDocument xmlDoc,
                                                 XmlNamespaceManager nsmgr)
        {
            IndividualCategoryList individualCategories;
            XmlNode dataNode;
            XmlNodeList rowNodes;

            rowNodes = xmlDoc.SelectNodes("/ss:Workbook/ss:Worksheet/ss:Table/ss:Row", nsmgr);
            individualCategories = new IndividualCategoryList();
            foreach (XmlNode rowNode in rowNodes)
            {
                dataNode = rowNode.SelectSingleNode("ss:Cell/ss:Data", nsmgr);
                individualCategories.Add(CoreData.FactorManager.GetIndividualCategory(userContext, dataNode.InnerText.WebParseInt32()));
            }

            IndividualCategories = individualCategories;
        }

        /// <summary>
        /// Writes one row to the xml format.
        /// </summary>
        /// <param name='xmlDoc'>The xml document that the row node will be written to.</param>
        /// <param name="tableNode">The table node in the Xml document that the row node should be appended to</param>
        protected override void WriteExcelXmlRows(XmlDocument xmlDoc, XmlNode tableNode)
        {
            if (IndividualCategories.IsNotEmpty())
            {
                foreach (IIndividualCategory individualCategory in IndividualCategories)
                {
                    XmlNode rowNode = xmlDoc.CreateNode(XmlNodeType.Element, "Row", "urn:schemas-microsoft-com:office:spreadsheet");

                    XmlNode cellIdNode = xmlDoc.CreateNode(XmlNodeType.Element, "Cell", "urn:schemas-microsoft-com:office:spreadsheet");
                    rowNode.AppendChild(cellIdNode);

                    XmlNode dataIdNode = xmlDoc.CreateNode(XmlNodeType.Element, "Data", "urn:schemas-microsoft-com:office:spreadsheet");
                    dataIdNode.Attributes.Append(xmlDoc.CreateAttribute("ss:Type", "urn:schemas-microsoft-com:office:spreadsheet"));
                    dataIdNode.Attributes[0].Value = "Number";
                    dataIdNode.InnerText = individualCategory.Id.WebToString();
                    cellIdNode.AppendChild(dataIdNode);

                    XmlNode cellNameNode = xmlDoc.CreateNode(XmlNodeType.Element, "Cell", "urn:schemas-microsoft-com:office:spreadsheet");
                    rowNode.AppendChild(cellNameNode);

                    XmlNode dataNameNode = xmlDoc.CreateNode(XmlNodeType.Element, "Data", "urn:schemas-microsoft-com:office:spreadsheet");
                    dataNameNode.Attributes.Append(xmlDoc.CreateAttribute("ss:Type", "urn:schemas-microsoft-com:office:spreadsheet"));
                    dataNameNode.Attributes[0].Value = "String";
                    dataNameNode.InnerText = individualCategory.Name;
                    cellNameNode.AppendChild(dataNameNode);

                    tableNode.AppendChild(rowNode);
                }
            }
        }
    }
}
