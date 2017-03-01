namespace ArtDatabanken.WebApplication.AnalysisPortal.Utils.Labels
{
    /// <summary>
    /// This class is a singleton class for all shared labels
    /// </summary>
    /// <remarks>
    /// Use in a view model like this:
    /// 
    /// public SharedLabels SharedLabels
    /// {
    ///     get { return SharedLabels.Instance; }
    /// }
    /// </remarks>
    public sealed class SharedLabels
    {
        private static readonly SharedLabels instance = new SharedLabels();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static SharedLabels()
        {
        }

        private SharedLabels()
        {
        }

        public static SharedLabels Instance
        {
            get
            {
                return instance;
            }
        }

        public string AboutAnalysisPortal { get { return Resources.Resource.SharedAboutAnalysisPortal; } }
        public string AboutAnalysisPortalText { get { return Resources.Resource.SharedAboutAnalysisPortalText; } }
        public string AboutCookiesHeader { get { return Resources.Resource.SharedAboutCookiesHeader; } }
        public string AboutCookiesSubHeader { get { return Resources.Resource.SharedAboutCookiesSubHeader; } }
        public string AddButtonText { get { return Resources.Resource.SharedAddButtonText; } }
        public string AdditionalErrorInformation { get { return Resources.Resource.SharedAdditionalErrorInformation; } }
        public string AnalysisPortalMainTitle { get { return Resources.Resource.SharedAnalysisPortalMainTitle; } }
        public string ArtDatabankenText { get { return Resources.Resource.SharedArtDatabankenText; } }
        public string AvaliableLabel { get { return Resources.Resource.SharedAvaliableLabel; } }
        public string BoolFalseText { get { return Resources.Resource.SharedBoolFalseText; } }
        public string BoolTrueText { get { return Resources.Resource.SharedBoolTrueText; } }
        public string ButtonCheckToolTip { get { return Resources.Resource.SharedButtonCheckToolTip; } }
        public string ButtonGroupHelp { get { return Resources.Resource.SharedButtonGroupHelp; } }
        public string ButtonSettingToolTip { get { return Resources.Resource.SharedButtonSettingToolTip; } }
        public string ButtonUnCheckToolTip { get { return Resources.Resource.SharedButtonUnCheckToolTip; } }
        public string CancelButtonText { get { return Resources.Resource.SharedCancelButtonText; } }
        public string ChangeLanguageLabel { get { return Resources.Resource.SharedChangeLanguageLabel; } }
        public string Clear { get { return Resources.Resource.SharedClear; } }
        public string Clipboard { get { return Resources.Resource.SharedClipboard; } }
        public string ContactMailLinkText { get { return Resources.Resource.SharedContactMailLinkText; } }
        public string CookiesReadMoreLink { get { return Resources.Resource.SharedCookiesReadMoreLink; } }
        public string CookiesReadMoreText { get { return Resources.Resource.SharedCookiesReadMoreText; } }
        public string Creating { get { return Resources.Resource.SharedCreating; } }
        public string CurrentServiceStatusLinkText { get { return Resources.Resource.SharedCurrentServiceStatusLinkText; } }
        public string DateText { get { return Resources.Resource.SharedDateText; } }
        public string DeleteButtonText { get { return Resources.Resource.SharedDeleteButtonText; } }
        public string Deleting { get { return Resources.Resource.SharedDeleting; } }
        public string DialogButtonTextNo { get { return Resources.Resource.SharedDialogButtonTextNo; } }
        public string DialogButtonTextYes { get { return Resources.Resource.SharedDialogButtonTextYes; } }
        public string DialogConfirmationHeader { get { return Resources.Resource.SharedDialogConfirmationHeader; } }
        public string DialogConfirmationText { get { return Resources.Resource.SharedDialogConfirmationText; } }
        public string DialogDeleteHeader { get { return Resources.Resource.SharedDialogDeleteHeader; } }
        public string DialogInformationHeader { get { return Resources.Resource.SharedDialogInformationHeader; } }
        public string DoYouWantToContinue { get { return Resources.Resource.SharedDoYouWantToContinue; } }
        public string DropDownAny { get { return Resources.Resource.SharedDropDownAny; } }
        public string DropDownFalse { get { return Resources.Resource.SharedDropDownFalse; } }
        public string DropDownTrue { get { return Resources.Resource.SharedDropDownTrue; } }
        public string DyntaxaFullVersionLinkText { get { return Resources.Resource.SharedDyntaxaFullVersionLinkText; } }
        public string DyntaxaLinkText { get { return Resources.Resource.SharedDyntaxaLinkText; } }
        public string EditButtonText { get { return Resources.Resource.SharedEditButtonText; } }
        public string Error { get { return Resources.Resource.SharedError; } }
        public string ErrorHeader { get { return Resources.Resource.SharedErrorHeader; } }
        public string ErrorOccurred { get { return Resources.Resource.SharedErrorOccurred; } }
        public string ErrorOccurredInPartialView { get { return Resources.Resource.SharedErrorOccurredInPartialView; } }
        public string ErrorRequestTimeout { get { return Resources.Resource.SharedErrorRequestTimeout; } }
        public string ErrorStringToLong250 { get { return Resources.Resource.SharedErrorStringToLong250; } }
        public string ExeptionNullOrEmpty { get { return Resources.Resource.SharedExeptionNullOrEmpty; } }
        public string FieldsetLegendEditCreateText { get { return Resources.Resource.SharedFieldsetLegendEditCreateText; } }
        public string Finalizing { get { return Resources.Resource.SharedFinalizing; } }
        public string FooterInfoHeader { get { return Resources.Resource.SharedFooterInfoHeader; } }
        public string FooterLinkHeader { get { return Resources.Resource.SharedFooterLinkHeader; } }
        public string GeneratingExcelFile { get { return Resources.Resource.SharedGeneratingExcelFile; } }
        public string GoToReferenceViewQuestion { get { return Resources.Resource.SharedGoToReferenceViewQuestion; } }
        public string Info { get { return Resources.Resource.SharedInfo; } }
        public string InvalidApplicationUserContext { get { return Resources.Resource.SharedInvalidApplicationUserContext; } }
        public string InvalidUserContext { get { return Resources.Resource.SharedInvalidUserContext; } }
        public string LoadButtonText { get { return Resources.Resource.SharedLoadButtonText; } }
        public string Loading { get { return Resources.Resource.SharedLoading; } }
        public string LoadingData { get { return Resources.Resource.SharedLoadingData; } }
        public string LogoArtprojektetSmallImg { get { return Resources.Resource.SharedLogoArtprojektetSmallImg; } }
        public string LogoLifewatch_SmallImg { get { return Resources.Resource.SharedLogoLifewatch_SmallImg; } }
        public string ManageReferences { get { return Resources.Resource.SharedManageReferences; } }
        public string MissingValue { get { return Resources.Resource.SharedMissingValue; } }
        public string NotPossibleToLoginArtDatabankenServiceError { get { return Resources.Resource.SharedNotPossibleToLoginArtDatabankenServiceError; } }
        public string NotPossibleToReadSpeciesFactError { get { return Resources.Resource.SharedNotPossibleToReadSpeciesFactError; } }
        public string NotPossibleToUpdateSpeciesFactError { get { return Resources.Resource.SharedNotPossibleToUpdateSpeciesFactError; } }
        public string NotSpecifiedText { get { return Resources.Resource.SharedNotSpecifiedText; } }
        public string NoValidReferenceErrorInfoText { get { return Resources.Resource.SharedNoValidReferenceErrorInfoText; } }
        public string NoValidReferenceErrorText { get { return Resources.Resource.SharedNoValidReferenceErrorText; } }
        public string NoValidRevisionErrorText { get { return Resources.Resource.SharedNoValidRevisionErrorText; } }
        public string NoValidTaxonErrorText { get { return Resources.Resource.SharedNoValidTaxonErrorText; } }
        public string OkButtonText { get { return Resources.Resource.SharedOkButtonText; } }
        public string Overview { get { return Resources.Resource.SharedOverview; } }
        public string PasswordLabel { get { return Resources.Resource.SharedPasswordLabel; } }
        public string PickReferenceText { get { return Resources.Resource.SharedPickReferenceText; } }
        public string ReadTaxonInformationNationalOccurrenceLabel { get { return Resources.Resource.SharedReadTaxonInformationNationalOccurrenceLabel; } }
        public string ReadTaxonInformationTaxonStatusLabel { get { return Resources.Resource.SharedReadTaxonInformationTaxonStatusLabel; } }
        public string ReferenceLabel { get { return Resources.Resource.SharedReferenceLabel; } }
        public string ReferenceNoReferencesAvaliable { get { return Resources.Resource.SharedReferenceNoReferencesAvaliable; } }
        public string References { get { return Resources.Resource.SharedReferences; } }
        public string ReferencesHeaderText { get { return Resources.Resource.SharedReferencesHeaderText; } }
        public string RemoveAll { get { return Resources.Resource.SharedRemoveAll; } }
        public string Reset { get { return Resources.Resource.SharedReset; } }
        public string ResetButtonText { get { return Resources.Resource.SharedResetButtonText; } }
        public string ReturnToListText { get { return Resources.Resource.SharedReturnToListText; } }
        public string RowDelimeter { get { return Resources.Resource.SharedRowDelimeter; } }
        public string RowDelimiterReturnLinefeed { get { return Resources.Resource.SharedRowDelimiterReturnLinefeed; } }
        public string RowDelimiterSemicolon { get { return Resources.Resource.SharedRowDelimiterSemicolon; } }
        public string RowDelimiterTab { get { return Resources.Resource.SharedRowDelimiterTab; } }
        public string RowDelimiterVerticalBar { get { return Resources.Resource.SharedRowDelimiterVerticalBar; } }
        public string SaveButtonText { get { return Resources.Resource.SharedSaveButtonText; } }
        public string Saving { get { return Resources.Resource.SharedSaving; } }
        public string Search { get { return Resources.Resource.SharedSearch; } }
        public string Searching { get { return Resources.Resource.SharedSearching; } }
        public string Select { get { return Resources.Resource.SharedSelect; } }
        public string SelectedLabel { get { return Resources.Resource.SharedSelectedLabel; } }
        public string SLUText { get { return Resources.Resource.SharedSLUText; } }
        public string SpeciesFactError { get { return Resources.Resource.SharedSpeciesFactError; } }        
        public string SubmitButtonText { get { return Resources.Resource.SharedSubmitButtonText; } }
        public string SupportHeaderText { get { return Resources.Resource.SharedSupportHeaderText; } }
        public string SwedishLifeWatchText { get { return Resources.Resource.SharedSwedishLifeWatchText; } }
        public string SvenskaArtprojektetText { get { return Resources.Resource.SharedSvenskaArtprojektetText; } }
        public string TaxonConceptDescription { get { return Resources.Resource.SharedTaxonConceptDescription; } }
        public string TaxonInvalidCategoryText { get { return Resources.Resource.SharedTaxonInvalidCategoryText; } }
        public string TaxonNameSearchResultListHeader { get { return Resources.Resource.SharedTaxonNameSearchResultListHeader; } }
        public string TaxonPickCategoryText { get { return Resources.Resource.SharedTaxonPickCategoryText; } }
        public string TaxonSelecteCategoryDropDownListText { get { return Resources.Resource.SharedTaxonSelecteCategoryDropDownListText; } }
        public string TaxonSelecteReferenceDropDownListText { get { return Resources.Resource.SharedTaxonSelecteReferenceDropDownListText; } }
        public string TermsOfUseText { get { return Resources.Resource.SharedTermsOfUseText; } }
        public string TimeoutError { get { return Resources.Resource.SharedTimeoutError; } }
        public string UpdateButtonText { get { return Resources.Resource.SharedUpdateButtonText; } }
        public string UserNameLabel { get { return Resources.Resource.SharedUserNameLabel; } }
        public string WebGridFirstPage { get { return Resources.Resource.SharedWebGridFirstPage; } }
        public string WebGridLastPage { get { return Resources.Resource.SharedWebGridLastPage; } }
        public string VersionNumberText { get { return Resources.Resource.SharedVersionNumberText; } }
        public string VersionText { get { return Resources.Resource.SharedVersionText; } }
        public string ViewLabel { get { return Resources.Resource.SharedViewLabel; } }
        public string Color { get { return Resources.Resource.SharedColor; } }
        public string Name { get { return Resources.Resource.SharedName; } }
        public string Type { get { return Resources.Resource.SharedType; } }
        public string Wait { get { return Resources.Resource.SharedWait; } }
        public string Region { get { return Resources.Resource.SharedRegion; } }
        public string Regions { get { return Resources.Resource.SharedRegions; } }
        public string SelectedRegions { get { return Resources.Resource.SharedSelectedRegions; } }
        public string RemoveRegion { get { return Resources.Resource.SharedRemoveRegion; } }
        public string RemoveRegionTooltip { get { return Resources.Resource.SharedRemoveRegionTooltip; } }
        public string NoSelectedRegions { get { return Resources.Resource.SharedNoSelectedRegions; } }
        public string Polygons { get { return Resources.Resource.SharedPolygons; } }
        public string Back { get { return Resources.Resource.SharedBack; } }
        public string Map { get { return Resources.Resource.SharedMap; } }
        public string Media { get { return Resources.Resource.SharedMedia; } }
    }
}
