using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ArtDatabanken.Data;
using DataType = System.ComponentModel.DataAnnotations.DataType;

// ReSharper disable CheckNamespace
namespace ArtDatabanken.WebApplication.Dyntaxa.Data
// ReSharper restore CheckNamespace
{
    public class TaxonSwedishOccuranceBaseViewModel : BaseViewModel 
    {
        [LocalizedDisplayName("TaxonAddExcludeFromReportingSystem", NameResourceType = typeof(Resources.DyntaxaResource))]
        public bool ExcludeFromReportingSystem { get; set; }

        [LocalizedDisplayName("TaxonAddBlockedForReporting", NameResourceType = typeof(Resources.DyntaxaResource))]
        public bool BlockedForReporting { get; set; }

        /// <summary>
        /// Description of the taxon quality.
        /// </summary>
        // [StringLength(250, ErrorMessageResourceType = typeof(Resources.DyntaxaResource), ErrorMessageResourceName = "SharedErrorStringToLong250")]
        [DataType(DataType.Text)]
        [LocalizedDisplayName("SharedTaxonConceptDescription", NameResourceType = typeof(Resources.DyntaxaResource))]
        [AllowHtml]
        public string TaxonQualityDescription { get; set; }

        /// <summary>
        /// Quality status of the taxon.
        /// </summary>
        [LocalizedDisplayName("TaxonEditQualityLabel", NameResourceType = typeof(Resources.DyntaxaResource))]
        public int TaxonQualityId { get; set; }

        /// <summary>
        /// List of all avaliable quality values for a taxon
        /// </summary>
        public IList<TaxonDropDownModelHelper> TaxonQualityList { get; set; }
        
        /// <summary>
        /// Selected swedish occurance status
        /// </summary>
        [LocalizedDisplayName("TaxonEditStatusOfSwedishOccurrence", NameResourceType = typeof(Resources.DyntaxaResource))]
        public int SwedishOccurrenceStatusId { get; set; }

        /// <summary>
        /// A list of all national occurrence and reproduction status quality.
        /// </summary>
        public IList<TaxonDropDownModelHelper> SwedishOccurrenceStatusList { get; set; }

        /// <summary>
        /// Selected swedish occurance quality id
        /// </summary>
        [LocalizedDisplayName("TaxonEditQualityOfSwedishOccurrence", NameResourceType = typeof(Resources.DyntaxaResource))]
        public int SwedishOccurrenceQualityId { get; set; }

        /// <summary>
        /// A list of all swedish occurance quality.
        /// </summary>
        public IList<TaxonDropDownModelHelper> SwedishOccurrenceQualityList { get; set; }

        /// <summary>
        /// Selected swedish occurance reference id
        /// </summary>
        [LocalizedDisplayName("TaxonEditReferenceOfSwedishOccurrence", NameResourceType = typeof(Resources.DyntaxaResource))]
        public int SwedishOccurrenceReferenceId { get; set; }

        /// <summary>
        /// A list of all swedish occurance references.
        /// </summary>
        public IList<TaxonDropDownModelHelper> SwedishOccurrenceReferenceList { get; set; }

        /// <summary>
        /// The national immigration  status description.
        /// </summary>
        [DataType(DataType.Text)]
        [LocalizedDisplayName("TaxonEditSwedishOccurrenceDescriptionLabel", NameResourceType = typeof(Resources.DyntaxaResource))]
        [AllowHtml]
        public string SwedishOccurrenceDescription { get; set; }

        /// <summary>
        /// The national immigration status id.
        /// </summary>
        [LocalizedDisplayName("TaxonEditStatusOfSwedishImmigrationHistory", NameResourceType = typeof(Resources.DyntaxaResource))]
        public int SwedishImmigrationHistoryStatusId { get; set; }

        /// <summary>
        /// A list of all national immigration status.
        /// </summary>
        public IList<TaxonDropDownModelHelper> SwedishImmigrationHistoryStatusList { get; set; }

        /// <summary>
        /// The national immigration quality id.
        /// </summary>
        [LocalizedDisplayName("TaxonEditQualityOfSwedishImmigrationHistory", NameResourceType = typeof(Resources.DyntaxaResource))]
        public int SwedishImmigrationHistoryQualityId { get; set; }

        /// <summary>
        /// A list of all national immigration qualities.
        /// </summary>
        public IList<TaxonDropDownModelHelper> SwedishImmigrationHistoryQualityList { get; set; }

        /// <summary>
        /// Selected national immigration reference id.
        /// </summary>
        [LocalizedDisplayName("TaxonEditReferenceOfSwedishImmigrationHistory", NameResourceType = typeof(Resources.DyntaxaResource))]
        public int SwedishImmigrationHistoryReferenceId { get; set; }

        /// <summary>
        /// A list of all national immigration reference list as strings to be displayed.
        /// </summary>
        public IList<TaxonDropDownModelHelper> SwedishImmigrationHistoryReferenceList { get; set; }

        /// <summary>
        /// The national immigration description.
        /// </summary>
        [DataType(DataType.Text)]
        [LocalizedDisplayName("TaxonEditSwedishImmigrationHistoryDescriptionLabel", NameResourceType = typeof(Resources.DyntaxaResource))]
        [AllowHtml]
        public string SwedishImmigrationHistoryDescription { get; set; }

        /// <summary>
        /// List of all taxon referenses
        /// </summary>
        [LocalizedDisplayName("SharedReferences", NameResourceType = typeof(Resources.DyntaxaResource))]
        public IList<IReference> TaxonReferencesList { get; set; }
    }
}