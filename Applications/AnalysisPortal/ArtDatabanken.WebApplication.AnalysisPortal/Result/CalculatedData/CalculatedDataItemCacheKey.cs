namespace ArtDatabanken.WebApplication.AnalysisPortal.Result.CalculatedData
{
    public class CalculatedDataItemCacheKey
    {
        private string _localeIsoCode;
        public CalculatedDataItemType CalculatedDataItemType { get; set; }
        public string LocaleIsoCode
        {
            get
            {
                return _localeIsoCode ?? "";
            }

            set
            {
                _localeIsoCode = value;
            }
        }

        protected bool Equals(CalculatedDataItemCacheKey other)
        {
            return CalculatedDataItemType == other.CalculatedDataItemType && string.Equals(LocaleIsoCode, other.LocaleIsoCode);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((CalculatedDataItemCacheKey)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int)CalculatedDataItemType * 397) ^ (LocaleIsoCode != null ? LocaleIsoCode.GetHashCode() : 0);
            }
        }

        public static bool operator ==(CalculatedDataItemCacheKey left, CalculatedDataItemCacheKey right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CalculatedDataItemCacheKey left, CalculatedDataItemCacheKey right)
        {
            return !Equals(left, right);
        }

        public CalculatedDataItemCacheKey(CalculatedDataItemType calculatedDataItemType)
        {
            CalculatedDataItemType = calculatedDataItemType;
        }

        public CalculatedDataItemCacheKey(CalculatedDataItemType calculatedDataItemType, string localeIsoCode)
        {
            CalculatedDataItemType = calculatedDataItemType;
            LocaleIsoCode = localeIsoCode;
        }
    }
}