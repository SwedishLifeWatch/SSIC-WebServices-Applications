using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Filter
{
    [DataContract]
    public class TemporalDateSetting : SettingBase
    {
        private DateTime _startDate;
        private DateTime _endDate;
        private bool _useSetting;
        private bool _annually;

        [DataMember]
        public DateTime StartDate
        {
            get
            {
                return _startDate;
            }

            set
            {
                _startDate = value;
                ResultCacheNeedsRefresh = true;
            }
        }

        [DataMember]
        public DateTime EndDate
        {
            get
            {
                return _endDate;
            }

            set
            {
                _endDate = value;
                ResultCacheNeedsRefresh = true;
            }
        }

        [DataMember]
        public bool UseSetting
        {
            get
            {
                return _useSetting;
            }

            set
            {
                _useSetting = value;
                ResultCacheNeedsRefresh = true;
            }
        }

        [DataMember]
        public bool Annually
        {
            get
            {
                return _annually;
            }

            set
            {
                _annually = value;
                ResultCacheNeedsRefresh = true;
            }
        }

        public override bool HasSettings
        {
            get { return UseSetting; }
        }

        public TemporalDateSetting()
        {
            DateTime today = DateTime.Now;
            //StartDate = today.AddYears(-1);
            StartDate = DateTime.Parse("01/01/" + today.Year);
            EndDate = today;
        }

        public override bool IsSettingsDefault()
        {
            if (UseSetting == false && Annually == false)
            {
                return true;
            }

            return false;
        }

        public void ResetSettings()
        {
            StartDate = DateTime.Now.AddYears(-1);
            EndDate = DateTime.Now;
            UseSetting = false;
            Annually = false;
        }
    }
}
