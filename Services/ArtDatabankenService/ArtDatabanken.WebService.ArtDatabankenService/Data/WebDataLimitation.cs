using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Contains limitations on which data that a DataQuery encompass.
    /// Both Data and DataQueries can be set.
    /// </summary>
    [DataContract]
    public class WebDataLimitation : WebData
    {
        /// <summary>
        /// Create a WebDataLimitation instance.
        /// </summary>
        public WebDataLimitation()
        {
            Data = null;
            DataQueries = null;
        }

        /// <summary>
        /// Data that constitutes the limitations
        /// on which data that a DataQuery encompass.
        /// </summary>
        [DataMember]
        public List<WebDataIdentifier> Data
        { get; set; }

        /// <summary>
        /// Data queries that result in data that is used to add
        /// limitations on which data that another DataQuery encompass.
        /// </summary>
        [DataMember]
        public List<WebDataQuery> DataQueries
        { get; set; }

        /// <summary>
        /// Check that data is valid.
        /// </summary>
        public override void CheckData()
        {
            base.CheckData();
            if (Data.IsNotEmpty())
            {
                foreach (WebDataIdentifier dataIdentifier in Data)
                {
                    dataIdentifier.CheckData();
                }
            }

            if (DataQueries.IsNotEmpty())
            {
                foreach (WebDataQuery dataQuery in DataQueries)
                {
                    dataQuery.CheckData();
                }
            }
        }
    }
}
