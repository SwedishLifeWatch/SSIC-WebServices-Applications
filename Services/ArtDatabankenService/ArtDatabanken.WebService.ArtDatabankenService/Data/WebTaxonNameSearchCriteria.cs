using System;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.ArtDatabankenService;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// This class holds taxa name filter information.
    /// </summary>
    [DataContract]
    public class WebTaxonNameSearchCriteria : WebData
    {
        /// <summary>
        /// Create a WebTaxonNameSearchCriteria instance.
        /// </summary>
        public WebTaxonNameSearchCriteria()
            : base()
        {
            NameSearchMethod = SearchStringComparisonMethod.Like;
            NameSearchString = null;
        }

        /// <summary>
        /// Name search method.
        /// </summary>
        [DataMember]
        public SearchStringComparisonMethod NameSearchMethod
        { get; set; }

        /// <summary>
        /// The name search string.
        /// </summary>
        [DataMember]
        public String NameSearchString
        { get; set; }

        /// <summary>
        /// Check that data is valid.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if name search string is empty.</exception>
        public override void CheckData()
        {
            base.CheckData();
            NameSearchString.CheckNotEmpty("NameSearchString");
            NameSearchString = NameSearchString.CheckSqlInjection();
        }
    }
}
