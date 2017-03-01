﻿using System.Collections.Generic;
using ArtDatabanken.WebApplication.AnalysisPortal.Managers.PageInfo;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Filter;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Buttons.FilterSectionButtons
{
    // Not yet fully implemented
    public class FilterOccurrenceButtonModel : StateButtonModel
    {
        private OccurrenceSetting OccurrenceSetting
        {
            get { return SessionHandler.MySettings.Filter.Occurrence; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterOccurrenceButtonModel"/> class.
        /// </summary>
        public FilterOccurrenceButtonModel()
        {            
            this.IsEnabled = true;
        }

        /// <summary>
        /// Gets the button identifier.
        /// This is used to identify a button, for example when we want to know which button was pressed.
        /// </summary>
        public override StateButtonIdentifier Identifier
        {
            get { return StateButtonIdentifier.FilterOccurrence; }
        }

        /// <summary>
        /// Gets the button title.
        /// </summary>
        public override string Title
        {
            get { return Resources.Resource.StateButtonFilterOccurrence; }
        }

        public override PageInfo StaticPageInfo
        {
            get { return PageInfoManager.GetPageInfo("Filter", "Occurrence"); }
        }

        public override string Tooltip
        {
            get { return Resources.Resource.StateButtonFilterOccurrenceTooltip; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this button is checked.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is checked; otherwise, <c>false</c>.
        /// </value>
        public override bool IsChecked
        {
            get { return OccurrenceSetting.IsActive; }
            set { OccurrenceSetting.IsActive = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the user has made any settings in the Action page
        /// </summary>
        /// <value>
        ///     <c>true</c> if the user hade made changes; otherwise, <c>false</c>.
        /// </value>
        public override bool HasSettings
        {
            get { return OccurrenceSetting.HasSettings; }
        }

        public override bool IsSettingsDefault
        {
            get { return OccurrenceSetting.IsSettingsDefault(); }
        }

        public override List<ButtonModelBase> Children { get { return null; } }
    }
}