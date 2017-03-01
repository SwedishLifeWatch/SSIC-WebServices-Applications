using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// List class for the DataId class.
    /// </summary>
    [Serializable()]
    public class DataIdList : ArrayList
    {
        private Boolean _optimize;
        private Hashtable _idHashTable;

        /// <summary>
        /// Constructor for the DataIdList class.
        /// </summary>
        public DataIdList()
            : this(false)
        {
        }

        /// <summary>
        /// Constructor for the DataIdList class.
        /// </summary>
        /// <param name='optimize'>
        /// Indicates if speed optimization should be turned on.
        /// This optimization assumes that each id only occurs once
        /// in the list.
        /// </param>
        public DataIdList(Boolean optimize)
        {
            _optimize = optimize;
            if (_optimize)
            {
                _idHashTable = new Hashtable();
            }
        }

        /// <summary>
        /// Add data to list.
        /// </summary>
        /// <param name='value'>Data to add</param>
        /// <returns>The list index at which the value has been added.</returns>
        public override int Add(object value)
        {
            DataId data;

            if (value.IsNotNull() && (value is DataId))
            {
                if (_optimize)
                {
                    data = (DataId)value;
                    if (!_idHashTable.Contains(data.Id))
                    {
                        _idHashTable.Add(data.Id, data);
                    }
                }
                return base.Add(value);
            }
            return -1;
        }

        /// <summary>
        /// Add a collection of data objects to the list.
        /// Override method in base class.
        /// </summary>
        /// <param name='collection'>The collection to add.</param>
        public override void AddRange(ICollection collection)
        {
            if (collection.IsNotEmpty())
            {
                foreach (Object value in collection)
                {
                    Add(value);
                }
            }
        }

        /// <summary>
        /// Clear the collection of data objects.
        /// Override method in base class.
        /// </summary>
        public override void Clear()
        {
            if (_optimize)
            {
                _idHashTable.Clear();
            }
            base.Clear();
        }

        /// <summary>
        /// Checks if data is in the list.
        /// </summary>
        /// <param name='dataId'>Object to be checked.</param>
        /// <returns>Boolean value indicating if the data exists in the list or not.</returns>
        public Boolean Exists(DataId dataId)
        {
            Object value;

            if (_optimize)
            {
                value = _idHashTable[dataId.Id];
                return ((value != null) && (value is DataId));
            }
            else
            {
                foreach (DataId dataIdInList in this)
                {
                    if (dataIdInList.Id == dataId.Id)
                    {
                        // Data found.
                        return true;
                    }
                }

                // Data not found.
                return false;
            }
        }

        /// <summary>
        /// Checks if data is in the list.
        /// </summary>
        /// <param name="itemID">int with the ID to search for</param>
        /// <returns>Boolean value indicating if the data exists in the list or not.</returns>
        public Boolean Exists(Int32 itemID)
        {
            return Find(itemID).IsNotNull();
        }

        /// <summary>
        /// Returns the first Item in the list wich has an Id corresponding to the parameter itemID
        /// </summary>
        /// <param name="itemID">int with the ID to search for</param>
        /// <returns>a DataId with the correct ID or null if none were found.</returns>
        public virtual DataId Find(Int32 itemID)
        {
            Object value;

            if (_optimize)
            {
                value = _idHashTable[itemID];
                if ((value != null) && (value is DataId))
                {
                    return (DataId)value;
                }
            }
            else
            {
                foreach (DataId dataIdInList in this)
                {
                    if (dataIdInList.Id == itemID)
                    {
                        // Item found.
                        return dataIdInList;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Makes a list of all item ids in this list.
        /// </summary>
        /// <returns>a typed list of all item ids in this list.</returns>
        public List<int> GetIds()
        {
             
            if (this.Count > 0)
            {
                List<int> itemIds = new List<int>();
                foreach (DataId itemId in this)
                {
                    itemIds.Add(itemId.Id);
                }
                return itemIds;
            }
            return null;
        }

        /// <summary>
        /// Get data with specified id.
        /// </summary>
        /// <param name='id'>Id of requested data.</param>
        /// <exception cref="ArgumentException">Thrown if no data has the requested id.</exception>
        /// <returns>Requested taxon type.</returns>
        protected DataId GetById(Int32 id)
        {
            Object value;

            if (_optimize)
            {
                value = _idHashTable[id];
                if ((value != null) && (value is DataId))
                {
                    return (DataId)value;
                }
            }
            else
            {
                foreach (DataId dataId in this)
                {
                    if (dataId.Id == id)
                    {
                        // Data found. Return it.
                        return dataId;
                    }
                }

            }

            // No data found with requested id.
            throw new ArgumentException("No data with id " + id + "!");
        }

        /// <summary>
        /// Merge data object with this list.
        /// Only objects that are not already in the list
        /// are added to the list.
        /// </summary>
        /// <param name='data'>The data to merge.</param>
        public void Merge(DataId data)
        {
            if (data.IsNotNull() && !Exists(data))
            {
                Add(data);
            }
        }

        /// <summary>
        /// Merge a collection of data objects with this list.
        /// Only objects that are not already in the list
        /// are added to the list.
        /// </summary>
        /// <param name='collection'>The collection to merge.</param>
        public void Merge(ICollection collection)
        {
            if (collection.IsNotEmpty())
            {
                foreach (Object value in collection)
                {
                    if (value.IsNotNull() &&
                        (value is DataId) &&
                        !Exists((DataId)value))
                    {
                        Add(value);
                    }
                }
            }
        }

        /// <summary>
        /// Remove data from list.
        /// </summary>
        /// <param name='value'>Data to remove</param>
        public override void Remove(object value)
        {
            DataId data;

            if (value.IsNotNull() && (value is DataId))
            {
                if (_optimize)
                {
                    data = (DataId)value;
                    _idHashTable.Remove(data.Id);
                }
                base.Remove(value);
            }
        }

        /// <summary>
        /// Removes the element at the specified index of the list.
        /// </summary>
        /// <param name='index'>The zero-based index of the element to remove.</param>
        public override void RemoveAt(int index)
        {
            DataId data;

            if (_optimize)
            {
                data = (DataId)(this[index]);
                _idHashTable.Remove(data.Id);
            }
            base.RemoveAt(index);
        }

        /// <summary>
        /// Get the subset of two lists.
        /// All data in "this" list that is not contained
        /// in parameter "data" is removed from this list.
        /// </summary>
        /// <param name='data'>The data list to compare with.</param>
        public virtual void Subset(DataIdList data)
        {
            Int32 index;

            if (this.IsNotEmpty())
            {
                for (index = this.Count - 1; index >= 0; index--)
                {
                    if (data.IsEmpty() || !data.Exists(((DataId)(this[index])).Id))
                    {
                        this.RemoveAt(index);
                    }
                }
            }
        }

        /// <summary>
        /// Get the Union of two collections.
        /// The result is returned in the first collection.
        /// </summary>
        /// <param name='collection'>The collection to union with.</param>
        public void Union(ICollection collection)
        {
            Merge(collection);
        }

        /// <summary>
        /// Converts the list to an excel compatible xml format
        /// </summary>
        public string ToXml()
        {
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
            tableNode.Attributes[1].Value = Convert.ToString(this.Count);
            tableNode.Attributes.Append(xmlDoc.CreateAttribute("x:FullColumns", "urn:schemas-microsoft-com:office:excel"));
            tableNode.Attributes[2].Value = "1";
            tableNode.Attributes.Append(xmlDoc.CreateAttribute("x:FullRows", "urn:schemas-microsoft-com:office:excel"));
            tableNode.Attributes[3].Value = "1";

            /*
            XmlNode columnNode = xmlDoc.CreateNode(XmlNodeType.Element, "Column", "urn:schemas-microsoft-com:office:spreadsheet");
            columnNode.Attributes.Append(xmlDoc.CreateAttribute("ss:Index", "urn:schemas-microsoft-com:office:spreadsheet"));
            columnNode.Attributes[0].Value = "2";
            columnNode.Attributes.Append(xmlDoc.CreateAttribute("ss:AutoFitWidth", "urn:schemas-microsoft-com:office:spreadsheet"));
            columnNode.Attributes[1].Value = "0";
            columnNode.Attributes.Append(xmlDoc.CreateAttribute("ss:Width", "urn:schemas-microsoft-com:office:spreadsheet"));
            columnNode.Attributes[2].Value = "160.0";
            tableNode.AppendChild(columnNode);
            */

            WriteXmlRows(xmlDoc, tableNode);

            workSheetNode.AppendChild(tableNode);
            return xmlDoc.InnerXml;
        }

        /// <summary>
        /// Builds a new list from an xml structure
        /// </summary>
        public void FromXml(String xml)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsmgr.AddNamespace("", "urn:schemas-microsoft-com:office:spreadsheet");
            nsmgr.AddNamespace("html", "http://www.w3.org/TR/REC-html40");
            nsmgr.AddNamespace("o", "urn:schemas-microsoft-com:office:office");
            nsmgr.AddNamespace("ss", "urn:schemas-microsoft-com:office:spreadsheet");
            nsmgr.AddNamespace("x", "urn:schemas-microsoft-com:office:excel");


            ReadXmlRows(xmlDoc, nsmgr);

        }

        /// <summary>
        /// Writes one row to the xml format. Overridden by each class that inherits DataIdList and wishes
        /// to write some data to a row.
        /// </summary>
        /// <param name='xmlDoc'>The xml document that the row node will be written to.</param>
        /// <param name="tableNode">The table node in the Xml document that the row node should be appended to</param>
        protected virtual void WriteXmlRows(XmlDocument xmlDoc, XmlNode tableNode)
        { }

        /// <summary>
        /// Reades one row from the xml format. Overridden by each class that inherits DataIdList and wishes
        /// to read some data from the row.
        /// </summary>
        /// <param name='xmlDoc'>The xml document that the row node will be read from.</param>
        /// <param name="nsmgr">Namespacemanager containing all namespaces used by the excel compatible xml format</param>
        protected virtual void ReadXmlRows(XmlDocument xmlDoc, XmlNamespaceManager nsmgr)
        { }
    }
}
