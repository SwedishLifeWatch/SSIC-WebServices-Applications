using System.Text;
using Resources;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.DataProviders.DataProviders;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.DataProviders;

namespace ArtDatabanken.WebApplication.AnalysisPortal.IO
{
    /// <summary>
    /// A class that can be used for downloads of statistcs about data providers of species observations.
    /// </summary>
    public class DataProviderListExcelXml : ExcelXmlBase
    {
        /// <summary>
        /// Constructor of an excel xml file with statistics on data providers.
        /// </summary>
        /// <param name="currentUser">Current user context</param>
        public DataProviderListExcelXml(IUserContext currentUser)
            : base()
        {
            var viewManager = new DataProvidersViewManager(currentUser, SessionHandler.MySettings);
            var data = viewManager.CreateDataProvidersViewModel();

            _xmlBuilder = new StringBuilder();

            //Add file definitions and basic format settings
            _xmlBuilder.AppendLine(base.GetInitialSection());

            //Specify column and row counts
            _xmlBuilder.AppendLine(base.GetColumnInitialSection(4, data.DataProviders.Count));

            //Specify column widths
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(300));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(270));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(140));
            _xmlBuilder.AppendLine(base.GetColumnWidthLine(140));

            //Add row with column headers
            _xmlBuilder.AppendLine(base.GetRowStart());
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine(Resource.DataProvidersDataProvidersDataProvider));
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine("Organisation"));
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine(Resource.DataProvidersDataProvidersNumberOfObservations));
            _xmlBuilder.AppendLine(base.GetColumnNameRowLine(Resource.DataProvidersDataProvidersNumberOfPublicObservations));
            _xmlBuilder.AppendLine(base.GetRowEnd());

            //Data values
            foreach (DataProviderViewModel row in data.DataProviders)
            {
                _xmlBuilder.AppendLine(base.GetRowStart());
                _xmlBuilder.AppendLine(base.GetDataRowLine("String", row.Name));
                _xmlBuilder.AppendLine(base.GetDataRowLine("String", row.Organization));
                _xmlBuilder.AppendLine(base.GetDataRowLine("Number", row.NumberOfObservations.ToString()));
                _xmlBuilder.AppendLine(base.GetDataRowLine("Number", row.NumberOfPublicObservations.ToString()));
                _xmlBuilder.AppendLine(base.GetRowEnd());
            }

            //Add final section of the xml document.
            _xmlBuilder.AppendLine(base.GetFinalSection());
        }
    }
}
