using System.Runtime.Serialization;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Calculation
{
    [DataContract]
    public sealed class CalculationSettings : SettingBase
    {
        [DataMember]
        public GridStatisticsSetting GridStatistics { get; set; }

        [DataMember]
        public SummaryStatisticsSetting SummaryStatistics { get; set; }

        [DataMember]
        public TimeSeriesSetting TimeSeries { get; set; }
    }
}
