using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService
{
    internal static class WebDataFieldExtension
    {
        /// <summary>
        /// Load data into WebDataField.
        /// </summary>
        /// <param name='webDataField'>Data field object.</param>
        /// <param name='webSpeciesObservationField'>A WramService WebSpeciesObservationField object.</param>
        public static void LoadData(this WebDataField webDataField,
                                    Proxy.MvmService.WebSpeciesObservationField webSpeciesObservationField)
        {
            webDataField.Information = webSpeciesObservationField.Information;
            webDataField.Name = webSpeciesObservationField.Property.Id.ToString();
            webDataField.Type = (WebDataType)webSpeciesObservationField.Type;
            webDataField.Unit = webSpeciesObservationField.Unit;
            webDataField.Value = webSpeciesObservationField.Value.CheckInjection();
        }

        /// <summary>
        /// Load data into WebDataField.
        /// </summary>
        /// <param name='webDataField'>Data field object.</param>
        /// <param name='webSpeciesObservationField'>A WramService WebSpeciesObservationField object.</param>
        public static void LoadData(this WebDataField webDataField,
                                    Proxy.WramService.WebSpeciesObservationField webSpeciesObservationField)
        {
            webDataField.Information = webSpeciesObservationField.Information;
            webDataField.Name = webSpeciesObservationField.Property.Id.ToString();
            webDataField.Type = (WebDataType)webSpeciesObservationField.Type;
            webDataField.Unit = webSpeciesObservationField.Unit;
            webDataField.Value = webSpeciesObservationField.Value.CheckInjection();
        }
    }
}
