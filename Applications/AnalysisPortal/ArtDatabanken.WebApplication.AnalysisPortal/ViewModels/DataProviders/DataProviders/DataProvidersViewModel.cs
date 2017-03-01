using System.Collections.Generic;
using System.Linq;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.DataProviders.DataProviders
{
    /// <summary>
    /// This class is a view model for data providers.
    /// </summary>
    public class DataProvidersViewModel
    {
        public bool IsSettingsDefault { get; set; }        

        /// <summary>
        /// List with data providers.
        /// </summary>
        public List<DataProviderViewModel> DataProviders { get; set; }

        /// <summary>
        /// Get the sums of all data providers NumberOfObservations field.
        /// </summary>
        public long SpeciesObservationsSum
        {
            get
            {
                if (DataProviders == null)
                {
                    return 0;
                }

                return DataProviders.Sum(dataProvider => dataProvider.NumberOfObservations);
            }
        }

        /// <summary>
        /// Gets the species observations sum formatted.
        /// </summary>
        public string SpeciesObservationsSumFormatted
        {
            get
            {
                return string.Format("{0:n0}", SpeciesObservationsSum);   
            }
        }

        /// <summary>
        /// Get the sums of all data providers public NumberOfObservations field.
        /// </summary>
        public long PublicSpeciesObservationsSum
        {
            get
            {
                if (DataProviders == null)
                {
                    return 0;
                }

                return DataProviders.Sum(dataProvider => dataProvider.NumberOfPublicObservations);
            }
        }

        /// <summary>
        /// Gets the public species observations sum formatted.
        /// </summary>
        public string PublicSpeciesObservationsSumFormatted
        {
            get
            {
                return string.Format("{0:n0}", PublicSpeciesObservationsSum);
            }
        }
    }

    /// <summary>
    /// This class is a view model for a data provider.
    /// </summary>
    public class DataProviderViewModel
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Organization { get; set; }
        public string Description { get; set; }
        public long NumberOfObservations { get; set; }
        public long NumberOfPublicObservations { get; set; }
        public bool IsSelected { get; set; }

        /// <summary>
        /// Gets the name and organization concatenated.
        /// </summary>
        public string NameAndOrganization
        {
            get
            {
                if (string.IsNullOrEmpty(Organization))
                {
                    return Name;
                }

                return string.Format("{0} ({1})", Name, Organization);
            }
        }

        /// <summary>
        /// Gets the number of observations formatted.
        /// </summary>
        public string NumberOfObservationsFormatted
        {
            get { return string.Format("{0:n0}", NumberOfObservations); }
        }

        /// <summary>
        /// Gets the number of public observations formatted.
        /// </summary>
        public string NumberOfPublicObservationsFormatted
        {
            get { return string.Format("{0:n0}", NumberOfPublicObservations); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataProviderViewModel"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="guid">The Guid.</param>
        /// <param name="name">The name.</param>
        /// <param name="organization">The organization.</param>
        /// <param name="numberOfObservations">The number of observations.</param>
        /// <param name="numberOfPublicObservations"></param>
        /// <param name="description"></param>
        /// <param name="url"></param>
        public DataProviderViewModel(int id, string guid, string name, string organization, long numberOfObservations, long numberOfPublicObservations, string description, string url)
        {
            Id = id;
            Guid = guid;
            Name = name;
            Organization = organization;
            NumberOfObservations = numberOfObservations;
            NumberOfPublicObservations = numberOfPublicObservations;
            Description = description;
            Url = url;
        }
    }
}
