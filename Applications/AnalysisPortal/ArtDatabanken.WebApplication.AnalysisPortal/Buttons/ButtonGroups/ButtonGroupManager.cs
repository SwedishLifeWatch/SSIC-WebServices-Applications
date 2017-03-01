using System.Collections.Generic;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons.ButtonGroups
{
    /// <summary>
    /// This static class manages the button groups used primarily on the main page (they are also used elsewhere).
    /// The static constructor creates all button groups and stores them in application cache as long the application is running.
    /// </summary>
    public static class ButtonGroupManager
    {
        private static readonly Dictionary<ButtonGroupIdentifier, ButtonGroupModelBase> _buttonGroupDictionary;
        private static readonly DataProvidersButtonGroupModel _dataProvidersButtonGroupModel;
        private static readonly FilterButtonGroupModel _filterButtonGroupModel;
        private static readonly CalculationButtonGroupModel _calculationButtonGroupModel;
        private static readonly PresentationButtonGroupModel _presentationButtonGroupModel;
        private static readonly ResultButtonGroupModel _resultButtonGroupModel;
        private static readonly SettingsButtonGroupModel _settingsButtonGroupModel;

        public static DataProvidersButtonGroupModel DataProvidersGroup
        {
            get { return _dataProvidersButtonGroupModel; }
        }

        public static FilterButtonGroupModel FilterButtonGroup
        {
            get { return _filterButtonGroupModel; }
        }

        public static CalculationButtonGroupModel CalculationButtonGroup
        {
            get { return _calculationButtonGroupModel; }
        }
        public static SettingsButtonGroupModel SettingsButtonGroup
        {
            get { return _settingsButtonGroupModel; }
        }

        public static PresentationButtonGroupModel PresentationButtonGroup
        {
            get { return _presentationButtonGroupModel; }
        }

        public static ResultButtonGroupModel ResultButtonGroup
        {
            get { return _resultButtonGroupModel; }
        }

        /// <summary>
        /// Initializes the <see cref="ButtonGroupManager"/> class.
        /// Creates all button groups and stores them in application cache as long the application is running.
        /// </summary>
        static ButtonGroupManager()
        {
            _buttonGroupDictionary = new Dictionary<ButtonGroupIdentifier, ButtonGroupModelBase>();

            _dataProvidersButtonGroupModel = new DataProvidersButtonGroupModel();
            _buttonGroupDictionary.Add(ButtonGroupIdentifier.DataProviders, _dataProvidersButtonGroupModel);
            _filterButtonGroupModel = new FilterButtonGroupModel();
            _buttonGroupDictionary.Add(ButtonGroupIdentifier.Filter, FilterButtonGroup);
            _calculationButtonGroupModel = new CalculationButtonGroupModel();
            _buttonGroupDictionary.Add(ButtonGroupIdentifier.Calculation, CalculationButtonGroup);
            _presentationButtonGroupModel = new PresentationButtonGroupModel();
            _buttonGroupDictionary.Add(ButtonGroupIdentifier.Presentation, PresentationButtonGroup);
            _resultButtonGroupModel = new ResultButtonGroupModel();
            _buttonGroupDictionary.Add(ButtonGroupIdentifier.Result, ResultButtonGroup);
            _settingsButtonGroupModel = new SettingsButtonGroupModel();
            _buttonGroupDictionary.Add(ButtonGroupIdentifier.Settings, SettingsButtonGroup);
        }

        /// <summary>
        /// Gets the button group.
        /// </summary>
        /// <param name="buttonGroupIdentifier">The button group identifier.</param>
        /// <returns></returns>
        public static ButtonGroupModelBase GetButtonGroup(int buttonGroupIdentifier)
        {
            return GetButtonGroup((ButtonGroupIdentifier)buttonGroupIdentifier);
        }

        /// <summary>
        /// Gets the button group.
        /// </summary>
        /// <param name="buttonGroupIdentifier">The button group identifier.</param>
        /// <returns></returns>
        public static ButtonGroupModelBase GetButtonGroup(ButtonGroupIdentifier buttonGroupIdentifier)
        {
            return _buttonGroupDictionary[buttonGroupIdentifier];
        }
    }
}
