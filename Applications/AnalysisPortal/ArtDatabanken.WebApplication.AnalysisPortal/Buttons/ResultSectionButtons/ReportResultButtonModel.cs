using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons.ResultSectionButtons
{
    /// <summary>
    /// This class acts as a view model for the Filter Taxa state button
    /// </summary>
    public class ReportResultViewButtonModel : StateButtonModel
    {
         /// <summary>
        /// Initializes a new instance of the <see cref="ReportResultViewButtonModel"/> class.
        /// </summary>
        public ReportResultViewButtonModel()
        {            
            this.IsEnabled = true;
        }

        /// <summary>
        /// Gets the button identifier.
        /// This is used to identify a button, for example when we want to know which button was pressed.
        /// </summary>
        public override StateButtonIdentifier Identifier
        {
            get { return StateButtonIdentifier.ReportResultView; }
        }

        /// <summary>
        /// Gets the button title.
        /// </summary>
        public override string Title
        {
            get { return Resources.Resource.StateButtonReportResult; }
        }
        
        public override PageInfo StaticPageInfo
        {
            get
            {
                return PageInfoManager.GetPageInfo("Result", "Reports");
            }
        }

        public override string Tooltip
        {
            get { return Resources.Resource.StateButtonReportResultTooltip; }
        }

        public override bool IsChecked
        {
            get
            {
                return true;
            }
            set
            {
            }
        }

        // Todo: Check wich reports is possible to view (sync with format)
        public override bool HasSettings
        {
            get { return true; }
        }

        public override bool IsSettingsDefault
        {
            get { return true; }
        }
        
        protected List<ButtonModelBase> _buttons = new List<ButtonModelBase>();

        public override List<ButtonModelBase> Children
        {
            get
            {
                if (_buttons.IsEmpty())
                {
                    _buttons.Add(new ResultSummaryStatisticsButtonModel());
                    _buttons.Add(new ResultSpeciesObservationProvenanceButtonModel());
                    _buttons.Add(new ResultSettingsSummaryButtonModel());
                }

                return _buttons;
            }
        }
    }
}
