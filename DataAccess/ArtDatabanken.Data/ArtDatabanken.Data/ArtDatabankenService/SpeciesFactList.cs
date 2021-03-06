﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// List class for the species fact class.
    /// </summary>
    [Serializable]
    public class SpeciesFactList : DataIdList
    {
        private Boolean _optimize;
        private Hashtable _identifierHashTable;

        /// <summary>
        /// Constructor for the SpeciesFactList class.
        /// </summary>
        public SpeciesFactList()
            : this(true)
        {
        }

        /// <summary>
        /// Constructor for the SpeciesFactList class.
        /// </summary>
        /// <param name='optimize'>
        /// Indicates if speed optimization should be turned on.
        /// This optimization assumes that each identfier only
        /// occurs once in the list.
        /// </param>
        public SpeciesFactList(Boolean optimize)
        {
            _optimize = optimize;
            if (_optimize)
            {
                _identifierHashTable = new Hashtable();
            }
        }


        /// <summary>
        /// Add SpeciesFact to list.
        /// </summary>
        /// <param name='speciesFact'>SpeciesFact to add</param>
        public void Add(SpeciesFact speciesFact)
        {
            if (speciesFact.IsNotNull())
            {
                if (_optimize)
                {
                    _identifierHashTable.Add(speciesFact.Identifier, speciesFact);
                }
                base.Add(speciesFact);
            }
        }

        /// <summary>
        /// Hide base class implementation of Add.
        /// This list class only supports specific data types.
        /// Generic data types are not supported.
        /// </summary>
        /// <param name='value'>A value.</param>
        public override int Add(object value)
        {
            throw new NotSupportedException("SpeciesFactList does not support generic data.");
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
                    if (value.IsNotNull() && (value is SpeciesFact))
                    {
                        Add(((SpeciesFact)value));
                    }
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
                _identifierHashTable.Clear();
            }
            base.Clear();
        }

        /// <summary>
        /// Test if species fact with a specified identifier is in the list.
        /// This unique identifier is a string which can be generated by the method GetSpeciesFactIdentifier.
        /// </summary>
        /// <param name="identifier">Identifier for requested species fact.</param>
        /// <returns>True if species fact is in the list.</returns>
        public Boolean Exists(String identifier)
        {
            if (_optimize)
            {
                return _identifierHashTable[identifier].IsNotNull();
            }
            else
            {
                foreach (SpeciesFact speciesFact in this)
                {
                    if (speciesFact.Identifier == identifier)
                    {
                        // Data found. Return it.
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Get species fact with specified species fact id.
        /// </summary>
        /// <param name='speciesFactId'>Id of requested species fact.</param>
        /// <returns>Requested species fact.</returns>
        /// <exception cref="ArgumentException">Thrown if no data has the requested id.</exception>
        public SpeciesFact Get(Int32 speciesFactId)
        {
            return (SpeciesFact)(GetById(speciesFactId));
        }

        /// <summary>
        /// Get/set species fact by list index.
        /// </summary>
        public new SpeciesFact this[Int32 index]
        {
            get
            {
                return (SpeciesFact)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }

        /// <summary>
        /// Get species fact with a specified identifier.
        /// This unique identifier is a string which can be generated by the method GetSpeciesFactIdentifier.
        /// </summary>
        /// <param name="identifier">Identifier for requested species fact.</param>
        /// <returns>Requested species fact</returns>
        public SpeciesFact Get(String identifier)
        {
            Object value;

            if (_optimize)
            {
                value = _identifierHashTable[identifier];
                if (value.IsNotNull())
                {
                    return (SpeciesFact)value;
                }
            }
            else
            {
                foreach (SpeciesFact speciesFact in this)
                {
                    if (speciesFact.Identifier == identifier)
                    {
                        // Data found. Return it.
                        return speciesFact;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Get species fact with a unique combination of parameters
        /// </summary>
        /// <param name="taxon">Taxon for requested species fact.</param>
        /// <param name="individualCategory">Individual Category for requested species fact.</param>
        /// <param name="factor">Factor for requested species fact.</param>
        /// <param name="host">Host for the requested species fact</param>
        /// <param name="period">Period for the requested species fact</param>
        /// <returns>Requested species fact</returns>
        public SpeciesFact Get(
            Taxon taxon,
            IndividualCategory individualCategory,
            Factor factor,
            Taxon host,
            Period period)
        {
            String identifier;

            identifier = SpeciesFactManager.GetSpeciesFactIdentifier(taxon,
                                                                     individualCategory,
                                                                     factor,
                                                                     host,
                                                                     period);
            return Get(identifier);
        }

        /// <summary>
        /// Get the most resent update time for the
        /// SpeciesFacts in this list.
        /// </summary>
        /// <returns>Most resent update time.</returns>
        public DateTime GetLastUpdate()
        {
            DateTime lastUpdate;

            lastUpdate = DateTime.MinValue;
            if (this.IsNotEmpty())
            {
                foreach (SpeciesFact speciesFact in this)
                {
                    if (speciesFact.UpdateDate > lastUpdate)
                    {
                        lastUpdate = speciesFact.UpdateDate;
                    }
                }
            }
            return lastUpdate;
        }

        /// <summary>
        /// Get a subset of this species fact list based on parameters
        /// </summary>
        /// <param name="taxon">The taxon for the requested species facts.</param>
        /// <returns>A species fact list</returns>
        public SpeciesFactList GetSpeciesFactsByParameters(Taxon taxon)
        {
            SpeciesFactList speciesFacts = new SpeciesFactList();
            var subset = from SpeciesFact speciesFact in this
                         where speciesFact.Taxon.Id == taxon.Id
                         //where speciesFact.Taxon == taxon //denna kod fungera lika bra som rad ovan
                         orderby speciesFact.Factor.SortOrder ascending
                         select speciesFact;
            speciesFacts.AddRange(subset.ToArray());
            return speciesFacts;
        }

        /// <summary>
        /// Get a subset of this species fact list based on parameters
        /// </summary>
        /// <param name="factor">The factor for the requested species facts.</param>
        /// <returns>A species fact list</returns>
        public SpeciesFactList GetSpeciesFactsByParameters(Factor factor)
        {
            SpeciesFactList speciesFacts = new SpeciesFactList();
            var subset = from SpeciesFact speciesFact in this
                         where speciesFact.Factor.Id == factor.Id
                         orderby speciesFact.Taxon.SortOrder ascending
                         select speciesFact;
            speciesFacts.AddRange(subset.ToArray());
            return speciesFacts;
        }

        /// <summary>
        /// Get a subset of this species fact list based on parameters
        /// </summary>
        /// <param name="individualCategory">The individual category for the requested species facts.</param>
        /// <returns>A species fact list</returns>
        public SpeciesFactList GetSpeciesFactsByParameters(IndividualCategory individualCategory)
        {
            SpeciesFactList speciesFacts = new SpeciesFactList();
            var subset = from SpeciesFact speciesFact in this
                         where speciesFact.IndividualCategory.Id == individualCategory.Id
                         select speciesFact;
            speciesFacts.AddRange(subset.ToArray());
            return speciesFacts;
        }

        /// <summary>
        /// Get a subset of this species fact list based on parameters
        /// </summary>
        /// <param name="period">The period for the requested species facts.</param>
        /// <returns>A species fact list</returns>
        public SpeciesFactList GetSpeciesFactsByParameters(Period period)
        {
            SpeciesFactList speciesFacts = new SpeciesFactList();
            var subset = from SpeciesFact speciesFact in this
                         where speciesFact.Period == period
                         select speciesFact;
            speciesFacts.AddRange(subset.ToArray());
            return speciesFacts;
        }

        /// <summary>
        /// Get a subset of this species fact list based on parameters
        /// </summary>
        /// <param name="taxon">The taxon for the requested species facts.</param>
        /// <param name="individualCategory">The individidual category for the requested species facts.</param>
        /// <param name="factor">The factor for the requested species facts.</param>
        /// <param name="period">The period for the requested species facts.</param>
        /// <returns></returns>
        public SpeciesFactList GetSpeciesFactsByParameters(
            Taxon taxon,
            IndividualCategory individualCategory,
            Factor factor,
            Period period)
        {
            SpeciesFactList speciesFacts = new SpeciesFactList();
            var subset = from SpeciesFact speciesFact in this
                         where speciesFact.Taxon.Id == taxon.Id
                         && speciesFact.IndividualCategory.Id == individualCategory.Id
                         && speciesFact.Factor.Id == factor.Id
                         && ((speciesFact.Period == period) || (speciesFact.Period == null))
                         select speciesFact;
            speciesFacts.AddRange(subset.ToArray());
            return speciesFacts;
        }

        /// <summary>
        /// Get a subset of this species fact list by quality
        /// </summary>
        /// <param name="quality">The quality for the requested species facts.</param>
        /// <returns>A species fact list</returns>
        public SpeciesFactList GetSpeciesFactsByQuality(SpeciesFactQuality quality)
        {
            SpeciesFactList speciesFacts = new SpeciesFactList();
            var subset = from SpeciesFact speciesFact in this
                         where speciesFact.Quality == quality
                         select speciesFact;
            speciesFacts.AddRange(subset.ToArray());
            return speciesFacts;
        }

        /// <summary>
        /// Method that counts the number of species facts in the list that has changed.
        /// </summary>
        /// <returns></returns>
        public Int32 CountChanges()
        {
            Int32 count = 0;

            foreach (SpeciesFact speciesFact in this)
            {
                if (speciesFact.HasChanged)
                {
                    ++count; 
                }
            }

            return count;
        }

        #region Looping Filter Functions

        /// <summary>
        /// Method that returns a filtered version of this species fact list. The filtering is done on several parameters.
        /// </summary>
        /// <param name="individualCategories"></param>
        /// <param name="periods"></param>
        /// <param name="hosts"></param>
        /// <param name="taxa"></param>
        /// <param name="factors"></param>
        /// <returns>A filtered SpeciesFactList</returns>
        public SpeciesFactList FilterSpeciesFacts(
            IndividualCategoryList individualCategories,
            PeriodList periods,
            TaxonList hosts,
            TaxonList taxa,
            FactorList factors)
        {
            SpeciesFactList filteredList = new SpeciesFactList();

            foreach (SpeciesFact fact in this)
            {
                bool go = true;

                if (fact.IndividualCategory != null)
                {
                    if ((individualCategories != null) && (individualCategories.Count > 0))
                    {
                        if (!individualCategories.Exists(fact.IndividualCategory))
                            go = false;
                    }
                }

                if (go)
                {
                    if (fact.Period != null)
                    {
                        if ((periods != null) && (periods.Count > 0))
                        {
                            if (!periods.Exists(fact.Period))
                                go = false;
                        }
                    }
                }
                if (go)
                {
                    if (fact.Host != null)
                    {
                        // For the time being we only accept species facts that dont have a host.

                        go = false;

                        //if ((hosts != null) && (hosts.Count > 0))
                        //{
                        //    if (!hosts.Exists(fact.Host))
                        //        go = false;
                        //}
                    }
                }
                if (go)
                {
                    if (fact.Taxon != null)
                    {
                        if ((taxa != null) && (taxa.Count > 0))
                        {
                            if (!taxa.Exists(fact.Taxon))
                                go = false;
                        }
                    }
                }
                if (go)
                {
                    if (fact.Factor != null)
                    {
                        if ((factors != null) && (factors.Count > 0))
                        {
                            if (!factors.Exists(fact.Factor))
                                go = false;
                        }
                    }
                }

                if (go)
                {
                    filteredList.Add(fact);
                }
            }

            return filteredList;
        }

        /// <summary>
        /// Method that returns a filtered version of this species fact list. The filtering is done on Individual Categories
        /// </summary>
        /// <param name="individualCategories"></param>
        /// <returns>A filtered SpeciesFactList</returns>
        public SpeciesFactList FilterSpeciesFacts(IndividualCategoryList individualCategories)
        {
            if (individualCategories == null)
            {
                throw new ArgumentException("CategoryList is null", "individualCategories");
            }

            SpeciesFactList filteredList = new SpeciesFactList();            
            {
                foreach (SpeciesFact fact in this)
                {
                    foreach (IndividualCategory category in individualCategories)
                    {
                        if (fact.IndividualCategory.Id == category.Id)
                        {
                            filteredList.Add(fact);
                            break;
                        }
                    }
                }
            }
            return filteredList;
        }

        /// <summary>
        /// Method that returns a filtered version of this species fact list. The filtering is done on Taxa
        /// </summary>
        /// <param name="taxa"></param>
        /// <returns>A filtered SpeciesFactList</returns>
        public SpeciesFactList FilterSpeciesFacts(TaxonList taxa)
        {
            
            if (taxa == null)
            {
                throw new ArgumentException("TaxonList is null", "taxa");
            }

            SpeciesFactList filteredList = new SpeciesFactList();
            //if (originalList.Count > taxa.Count)
            {
                foreach (SpeciesFact fact in this)
                {
                    foreach (Taxon taxon in taxa)
                    {
                        if (fact.Taxon.Id == taxon.Id)
                        {
                            filteredList.Add(fact);
                            break;
                        }
                    }
                }
            }
            return filteredList;
        }
        
        /// <summary>
        /// Method that returns a filtered version of this species fact list. The filtering is done on Periods
        /// </summary>
        /// <param name="periods"></param>
        /// <returns>A filtered SpeciesFactList</returns>
        public SpeciesFactList FilterSpeciesFacts(PeriodList periods)
        {
            
            if (periods == null)
            {
                throw new ArgumentException("PeriodList is null", "periods");
            }

            SpeciesFactList filteredList = new SpeciesFactList();
            //if (originalList.Count > periods.Count)
            {
                foreach (SpeciesFact fact in this)
                {
                    foreach (Period period in periods)
                    {
                        if (fact.Period.Id == period.Id)
                        {
                            filteredList.Add(fact);
                            break;
                        }
                    }
                }
            }
            
            return filteredList;
        }

        /// <summary>
        /// Method that returns a filtered version of this species fact list. The filtering is done on factors
        /// </summary>
        /// <param name="factors"></param>
        /// <returns>A filtered SpeciesFactList</returns>
        public SpeciesFactList FilterSpeciesFacts(FactorList factors)
        {
            
            if (factors == null)
            {
                throw new ArgumentException("FactorList is null", "factors");
            }

            SpeciesFactList filteredList = new SpeciesFactList();
            //if (originalList.Count > factors.Count)
            {
                foreach (SpeciesFact fact in this)
                {
                    foreach (Factor factor in factors)
                    {
                        if (fact.Factor.Id == factor.Id)
                        {
                            filteredList.Add(fact);
                            break;
                        }
                    }
                }
            }
            
            return filteredList;
        }

        /// <summary>
        /// Method that returns a filtered version of this species fact list. The filtering is done on hosts
        /// </summary>
        /// <param name="hosts"></param>
        /// <returns>A filtered SpeciesFactList</returns>
        public SpeciesFactList FilterSpeciesFactsHosts(TaxonList hosts)
        {
            if (hosts == null)
            {
                throw new ArgumentException("TaxonList is null", "hosts");
            }

            SpeciesFactList filteredList = new SpeciesFactList();
            //if (originalList.Count > hosts.Count)
            {
                foreach (SpeciesFact fact in this)
                {
                    foreach (Taxon host in hosts)
                    {
                        if (fact.Host.Id == host.Id)
                        {
                            filteredList.Add(fact);
                            break;
                        }
                    }
                }
            }
            
            return filteredList;
        }


        #endregion

/*
        protected override void WriteXmlRows(XmlDocument xmlDoc, XmlNode tableNode)
        {

            foreach (SpeciesFact fact in this)
            {
                //Add Row and Id node
                XmlNode rowNode = xmlDoc.CreateNode(XmlNodeType.Element, "Row", "urn:schemas-microsoft-com:office:spreadsheet");

                XmlNode cellIdNode = xmlDoc.CreateNode(XmlNodeType.Element, "Cell", "urn:schemas-microsoft-com:office:spreadsheet");
                cellIdNode.Attributes.Append(xmlDoc.CreateAttribute("DataType", ""));
                cellIdNode.Attributes[0].Value = "Id";
                rowNode.AppendChild(cellIdNode);

                XmlNode dataIdNode = xmlDoc.CreateNode(XmlNodeType.Element, "Data", "urn:schemas-microsoft-com:office:spreadsheet");
                dataIdNode.Attributes.Append(xmlDoc.CreateAttribute("ss:Type", "urn:schemas-microsoft-com:office:spreadsheet"));
                dataIdNode.Attributes[0].Value = "String";
                dataIdNode.InnerText = Convert.ToString(Convert.ToString(fact.Id));
                cellIdNode.AppendChild(dataIdNode);

                SpeciesFactFieldList fieldList = new SpeciesFactFieldList();
                fieldList.Add(fact.Field1);
                fieldList.Add(fact.Field2);
                fieldList.Add(fact.Field3);
                fieldList.Add(fact.Field4);
                fieldList.Add(fact.Field5);

                int fieldNo = 1;

                foreach (SpeciesFactField factField in fieldList)
                {
                    
                    XmlNode cellNode = xmlDoc.CreateNode(XmlNodeType.Element, "Cell", "urn:schemas-microsoft-com:office:spreadsheet");
                    cellNode.Attributes.Append(xmlDoc.CreateAttribute("DataType", ""));
                    cellNode.Attributes[0].Value = "Field" + fieldNo;
                    rowNode.AppendChild(cellNode);

                    XmlNode dataNode = xmlDoc.CreateNode(XmlNodeType.Element, "Data", "urn:schemas-microsoft-com:office:spreadsheet");
                    dataNode.Attributes.Append(xmlDoc.CreateAttribute("ss:Type", "urn:schemas-microsoft-com:office:spreadsheet"));
                    dataNode.Attributes[0].Value = "String";
                    if (factField.IsNotNull())
                    {
                        switch (factField.FactorField.Type.Id)
                        {
                            case (Int32)FactorFieldDataTypeId.Enum:
                                break;
                            case (Int32)FactorFieldDataTypeId.Double:
                                if ((!Double.IsNaN(Convert.ToDouble(factField.Value))) && (!Double.IsInfinity(Convert.ToDouble(factField.Value))) && (Double.MinValue != Convert.ToDouble(factField.Value)))
                                    dataNode.InnerText = Convert.ToString(Convert.ToString(factField.Value));
                                break;
                            default:
                                dataNode.InnerText = Convert.ToString(Convert.ToString(factField.Value));
                                break;

                        }
                    
                    }
                    cellNode.AppendChild(dataNode);

                    XmlNode oldDataNode = xmlDoc.CreateNode(XmlNodeType. Element, "OldData", "urn:schemas-microsoft-com:office:spreadsheet");
                    if (factField.IsNotNull())
                        oldDataNode.InnerText = Convert.ToString(Convert.ToString(factField.OldValue));
                    cellNode.AppendChild(oldDataNode);

                    fieldNo++;
                }
                

                tableNode.AppendChild(rowNode);
            }
        }

        protected override void ReadXmlRows(XmlDocument xmlDoc, XmlNamespaceManager nsmgr)
        {
            
            XmlNodeList rowNodes = xmlDoc.SelectNodes("/ss:Workbook/ss:Worksheet/ss:Table/ss:Row", nsmgr);
            foreach (XmlNode rowNode in rowNodes)
            {
                XmlNode idNode = rowNode.SelectSingleNode("ss:Cell[@DataType = 'Id']", nsmgr);

                List<Int32> idList = new List<Int32>();
                idList.Add(Convert.ToInt32(idNode.InnerText));
                SpeciesFactList factList = SpeciesFactManager.GetSpeciesFacts(idList);
                SpeciesFact fact = factList[0];

                SpeciesFactFieldList fieldList = new SpeciesFactFieldList();
                fieldList.Add(fact.Field1);
                fieldList.Add(fact.Field2);
                fieldList.Add(fact.Field3);
                fieldList.Add(fact.Field4);
                fieldList.Add(fact.Field5);

                for (int i = 1; i <= 5; i++)
                {
                    foreach (XmlNode cellNode in rowNode.SelectNodes("ss:Cell[@DataType = 'Field" + i + "']", nsmgr))
                    {
                        XmlNode dataNode = cellNode.SelectSingleNode("ss:Data", nsmgr);
                        if ((dataNode.InnerText.Length > 0) && (fact.AllowManualUpdate))
                            fieldList[i-1].Value = dataNode.InnerText;

                        //XmlNode oldDataNode = cellNode.SelectSingleNode("ss:OldData", nsmgr);
                    }
                }

                //Quality and Ref


                this.Add(fact);

            }

            
        }
        */
    }
}
