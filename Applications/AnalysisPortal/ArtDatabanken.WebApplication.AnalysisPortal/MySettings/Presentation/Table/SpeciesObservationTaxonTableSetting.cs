using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation.Table
{
    /// <summary>
    /// This class stores view settings
    /// </summary>
    [DataContract]
    public sealed class SpeciesObservationTaxonTableSetting : SettingBase
    {
        /// <summary>
        /// Gets or sets whether PresentationViewSetting is active or not.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is checked; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public bool UseLabelAsColumnHeader { get; set; }

        /// <summary>
        /// Determines whether any settings has been done.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance has settings; otherwise, <c>false</c>.
        ///   </returns>
        public override bool HasSettings
        {
            get { return false; }
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SpeciesObservationTaxonTableSetting"/> class.
        /// </summary>
        public SpeciesObservationTaxonTableSetting()
        {            
            ResetSettings();
        }
        
        public void ResetSettings()
        {
            UseLabelAsColumnHeader = true;
        }

        public override bool IsSettingsDefault()
        {
            if (UseLabelAsColumnHeader == true)
            {
                return true;
            }
            return false;
        }
    }
}
