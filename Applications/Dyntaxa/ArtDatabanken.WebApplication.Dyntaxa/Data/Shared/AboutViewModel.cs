using System.Web.Mvc;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Reflection;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AboutViewModel
    {
        private readonly AboutViewModelLabels labels = new AboutViewModelLabels();

        /// <summary>
        /// Gets or sets the CreationDate.
        /// </summary>
        public string CreationDate { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// All localized labels
        /// </summary>
        public AboutViewModelLabels Labels
        {
            get { return labels; }
        }

        /// <summary>
        /// The server name.
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// Localized labels used in about view
        /// </summary>
        public class AboutViewModelLabels
        {
            public string SharedAboutDyntaxaText
            {
                get { return Resources.DyntaxaResource.SharedAboutDyntaxaText; }
            }

            public string SharedDialogInformationHeader
            {
                get { return Resources.DyntaxaResource.SharedDialogInformationHeader; }
            }

            public string SharedVersionText
            {
                get { return Resources.DyntaxaResource.SharedVersionText; }
            }

            public string SharedDateText
            {
                get { return Resources.DyntaxaResource.SharedDateText; }
            }

            public string DyntaxaUserName
            {
                get { return Resources.DyntaxaSettings.Default.DyntaxaUserName; }
            }
        }
    }
}
