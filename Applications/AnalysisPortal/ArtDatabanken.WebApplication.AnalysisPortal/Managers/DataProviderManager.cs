using System;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Managers
{
    public static class DataProviderManager
    {
        public static SpeciesObservationDataProviderList GetAllDataProviders(IUserContext userContext)
        {
            bool providersWithObservationDisabled;
            return GetAllDataProviders(userContext, out providersWithObservationDisabled);
        }

        public static SpeciesObservationDataProviderList GetAllDataProviders(IUserContext userContext, out bool providersWithObservationDisabled)
        {
            providersWithObservationDisabled = false;
            var dataProviders = CoreData.SpeciesObservationManager.GetSpeciesObservationDataProviders(userContext);
            var disabledProviderGuidsString = ConfigurationHelper.GetValue("DisableDataProviders", string.Empty);
            var disabledProviderGuids = new string[0];

            if (disabledProviderGuidsString != null)
            {
                disabledProviderGuids = disabledProviderGuidsString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray();
            }

            //Populate a new list since removing items from orginal list affect cached list
            var returnList = new SpeciesObservationDataProviderList();
            foreach (var dataProvider in dataProviders)
            {
                if (disabledProviderGuids.Count(dpg => dpg == dataProvider.Guid) == 0)
                {
                    returnList.Add(dataProvider);
                }
                else if (!providersWithObservationDisabled)
                {
                    //Set flag to true if any disabled provider has observations
                    providersWithObservationDisabled = dataProvider.SpeciesObservationCount != 0;
                }
            }
 
            return returnList;
        }
    }
}
