using System.Runtime.Serialization;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataProviders
{
    [DataContract]
    public sealed class SpeciesObservationDataSourceSetting : SettingBase
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int NumberOfObservations { get; set; }

        [DataMember]
        public bool IsActive { get; set; }
    }
}
