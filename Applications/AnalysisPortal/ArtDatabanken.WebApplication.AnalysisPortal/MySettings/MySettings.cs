using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Calculation;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataProviders;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Filter;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings
{
    /// <summary>
    /// This class stores all the settings the user has made. The settings can be serialized
    /// to a file and later be deserialized.
    /// </summary>
    /// <remarks>
    /// Info about versioning in WCF DataContract: http://msdn.microsoft.com/en-us/library/ms733832.aspx
    /// Some rules about versioning:
    /// 1. You shouldn't change the Name of a property.
    /// 2. You shouldn't change the Namespace of a class.
    /// </remarks>
    [DataContract]
    public sealed class MySettings : SettingBase
    {
        /// <summary>
        /// Gets or sets the FilterSettings.
        /// </summary>
        [DataMember]        
        public FilterSettings Filter { get; set; }

        /// <summary>
        /// Gets or sets the DataProviderSettings.
        /// </summary>
        [DataMember]
        public DataProviderSettings DataProvider { get; set; }

        /// <summary>
        /// Gets or sets the ViewSettings.
        /// </summary>
        [DataMember]
        public PresentationSettings Presentation { get; set; }

        /// <summary>
        /// Gets or sets the CalculationSettings.
        /// </summary>
        [DataMember]
        public CalculationSettings Calculation { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MySettings"/> class.
        /// </summary>
        public MySettings()
        {         
            Initialize();            
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            EnsureInitialized();
            ResultCacheNeedsRefresh = true;
            Initialized = true;
            if (SessionHandler.Results != null)
            {
                SessionHandler.Results.Clear();
            }
        }

        /// <summary>
        /// Make sure saved providers still exists
        /// </summary>
        /// <param name="currentUser"></param>
        public void EnsureDataProviders(IUserContext currentUser)
        {
            var myDataProviders = DataProvider;
            if (myDataProviders != null && myDataProviders.DataProviders != null && myDataProviders.DataProviders.DataProvidersGuids != null)
            {
                var removedProviders = new Collection<string>();
                var dataProviders = DataProviderManager.GetAllDataProviders(currentUser);
                foreach (var providerGuid in myDataProviders.DataProviders.DataProvidersGuids)
                {
                    if (dataProviders.Count(p => p.Guid == providerGuid) == 0)
                    {
                        removedProviders.Add(providerGuid);
                    }
                }

                foreach (var removedProvider in removedProviders)
                {
                    myDataProviders.DataProviders.DataProvidersGuids.Remove(removedProvider);
                }    
            }
        }
    }
}
