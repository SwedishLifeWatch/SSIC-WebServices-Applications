using System.Runtime.Serialization;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataProviders.MapLayers;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataProviders
{
    [DataContract]
    public sealed class DataProviderSettings : SettingBase
    {
        [DataMember]
        public MapLayersSetting MapLayers { get; set; }

        [DataMember]
        public DataProvidersSetting DataProviders { get; set; }            
    }
}
