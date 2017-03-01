using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons
{
    /// <summary>
    /// This is the base class for all State buttons view models
    /// </summary>
    public abstract class StateButtonModel : ButtonModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StateButtonModel"/> class.
        /// </summary>
        public StateButtonModel()
        {
            ShowCheckbox = true;
        }

        /// <summary>
        /// Gets the button identifier.
        /// This is used to identify a button, for example when we want to know which button was pressed.
        /// </summary>
        public abstract StateButtonIdentifier Identifier { get; }
        
        /// <summary>
        /// Gets or sets a value indicating whether this button is checked.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is checked; otherwise, <c>false</c>.
        /// </value>
        public abstract bool IsChecked { get; set; }

        /// <summary>
        /// Gets a value indicating whether the user has made any settings in the Action page        
        /// </summary>
        /// <value>
        ///     <c>true</c> if the user hade made changes; otherwise, <c>false</c>.
        /// </value>
        public abstract bool HasSettings { get; }

        /// <summary>
        /// Gets a value indicating whether the related settings to this buttons is default settings.
        /// </summary>        
        public abstract bool IsSettingsDefault { get; }

        /* TODO: test - current/active state */

        /// <summary>
        /// Gets or sets value indicating wich button that is active
        /// </summary>
        /// <value>
        ///     <c>true</c> if the user is on the matching page; otherwise, <c>false</c>.
        /// </value>
        public override bool IsCurrent
        {
            get
            {
                PageInfo pageInfo = SessionHandler.CurrentPage;
                if (pageInfo.IsNotNull())
                {
                    return pageInfo.StateButton == this.Identifier;
                }
                return false;
            }
        }
        
        public abstract List<ButtonModelBase> Children { get; }        
        
        
        /// <summary>
        /// Indicates if a checkbox should be rendered or not
        /// </summary>      
        public bool ShowCheckbox { get; set; }
    }
}
