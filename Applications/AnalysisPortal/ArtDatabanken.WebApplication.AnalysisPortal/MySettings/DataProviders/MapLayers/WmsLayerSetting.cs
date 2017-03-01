using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataProviders.MapLayers
{
    [DataContract]
    public class WmsLayerSetting
    {
        /// <summary>
        /// Gets or sets the unique id.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the server URL.
        /// </summary>
        [DataMember]
        public string ServerUrl { get; set; }

        /// <summary>
        /// Gets or sets the Layers.
        /// </summary>        
        [DataMember]
        public List<string> Layers { get; set; }

        /// <summary>
        /// Gets or sets the supported coordinate systems.
        /// </summary>

        /// <value>
        /// The supported coordinate systems for this WMS layer.
        /// </value>
        [DataMember]
        public List<string> SupportedCoordinateSystems { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is a base layer.
        /// </summary>        
        [DataMember]
        public bool IsBaseLayer { get; set; }
    }
}
