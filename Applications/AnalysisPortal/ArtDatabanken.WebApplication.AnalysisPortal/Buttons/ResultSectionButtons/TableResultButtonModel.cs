using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons.ResultSectionButtons
{
    /// <summary>
    /// This class acts as a view model for the Filter Taxa state button
    /// </summary>
    public class TableResultViewButtonModel : StateButtonModel
    {
         /// <summary>
        /// Initializes a new instance of the <see cref="TableResultViewButtonModel"/> class.
        /// </summary>
        public TableResultViewButtonModel()
        {            
            this.IsEnabled = true;
        }

        /// <summary>
        /// Gets the button identifier.
        /// This is used to identify a button, for example when we want to know which button was pressed.
        /// </summary>
        public override StateButtonIdentifier Identifier
        {
            get { return StateButtonIdentifier.TableResultView; }
        }

        /// <summary>
        /// Gets the button title.
        /// </summary>
        public override string Title
        {
            get { return Resources.Resource.StateButtonTableResult; }
        }
        
        public override PageInfo StaticPageInfo
        {
            get
            {
                return PageInfoManager.GetPageInfo("Result", "Tables");
            }
        }

        public override string Tooltip
        {
            get { return Resources.Resource.StateButtonTableResultTooltip; }
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

        // Todo: Check wich tables is possible to view (sync with format)
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
                    _buttons.Add(new ResultSpeciesObservationTableButtonModel());
                    _buttons.Add(new ResultSpeciesObservationTaxonTableButtonModel());
                    _buttons.Add(new ResultTaxonWithSpeciesObservationCountsTableButtonModel());
                    _buttons.Add(new ResultSpeciesObservationGridTableButtonModel());
                    _buttons.Add(new ResultTaxonGridTableButtonModel());
                    _buttons.Add(new ResultTimeSeriesOnSpeciesObservationCountsTableButtonModel());
                    _buttons.Add(new ResultSummaryStatisticsPerPolygonTableButtonModel());
                }

                return _buttons;
            }
        }
    }
}
