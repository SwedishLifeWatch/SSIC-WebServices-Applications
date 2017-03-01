using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons.ButtonGroups
{
    /// <summary>
    /// This is the base class for button group view models
    /// </summary>
    public abstract class ButtonGroupModelBase
    {
        /// <summary>
        /// Gets the button group title.
        /// </summary>
        public abstract string Title { get; }

        public abstract PageInfo MainPageInfo { get; }

        public abstract string ImageUrl { get; }

        public abstract string IconClass { get; }

        /// <summary>
        /// Gets the button group identifier.        
        /// </summary>
        public abstract ButtonGroupIdentifier Identifier { get; }

        /// <summary>
        /// List with the buttons that is part of the button group.
        /// </summary>
        public List<StateButtonModel> Buttons
        {
            get
            {
                if (_buttons.IsNull())
                {
                    _buttons = new List<StateButtonModel>();
                }                    
                return _buttons;
            }            
        }
        private List<StateButtonModel> _buttons;

        /// <summary>
        /// Gets or sets value indicating if this button group is the current one
        /// </summary>
        /// <value>
        ///     <c>true</c> if the user is on the matching page; otherwise, <c>false</c>.
        /// </value>
        public bool IsCurrent
        {
            get
            {
                PageInfo pageInfo = SessionHandler.CurrentPage;
                if (pageInfo.IsNotNull())
                {
                    return pageInfo.ButtonGroup == Identifier;                  
                }
                return false;
            }
        }
    }
}
