using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.ViewResult
{
    public class ViewTableField
    {
        public string Title { get; set; }
        public string DataField { get; set; }
        public int Width { get; set; }
        public bool Sortable { get; set; }

        public ViewTableField(string title, string dataField, int width, bool sortable)
        {
            Title = title;
            DataField = dataField;
            Width = width;
            Sortable = sortable;
        }

        public ViewTableField(string title, string dataField)
        {
            Title = title;
            DataField = dataField;

            Sortable = true;
            Width = 100;
        }

        public string GetHeader()
        {
            if (SessionHandler.MySettings.Presentation.Table.SpeciesObservationTable.UseLabelAsColumnHeader)
            {
                return Title;
            }

            return DataField;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return DataField;
        }
    }
}
