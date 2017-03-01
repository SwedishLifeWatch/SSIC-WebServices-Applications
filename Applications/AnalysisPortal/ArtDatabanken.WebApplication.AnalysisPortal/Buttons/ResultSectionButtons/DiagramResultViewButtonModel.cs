using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons.ResultSectionButtons
{
    public class DiagramResultViewButtonModel : StateButtonModel
    {
         /// <summary>
        /// Initializes a new instance of the <see cref="DiagramResultViewButtonModel"/> class.
        /// </summary>
        public DiagramResultViewButtonModel()
        {            
            this.IsEnabled = true;
        }

        /// <summary>
        /// Gets the button identifier.
        /// This is used to identify a button, for example when we want to know which button was pressed.
        /// </summary>
        public override StateButtonIdentifier Identifier
        {
            get { return StateButtonIdentifier.DiagramResultView; }
        }

        /// <summary>
        /// Gets the button title.
        /// </summary>
        public override string Title
        {
            get { return Resources.Resource.StateButtonDiagramResult; }
        }
        
        public override PageInfo StaticPageInfo
        {
            get
            {
                return PageInfoManager.GetPageInfo("Result", "Diagrams");
            }
        }

        public override string Tooltip
        {
            get { return Resources.Resource.StateButtonDiagramResultTooltip; }
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
                    _buttons.Add(new ResultSpeciesObservationDiagramButtonModel());
                    _buttons.Add(new ResultSpeciesObservationAbundanceIndexDiagramButtonModel());
                }

                return _buttons;
            }
        }
    }
}
