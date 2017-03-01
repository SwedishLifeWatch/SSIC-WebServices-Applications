namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    public class SpeciesFactFieldValueModelHelper
    {
        public int QualityId { get; set; }
        public int ReferenceId { get; set; }
        public double FactorField1Value { get; set; }
        public SpeciesFactFieldType FactorField1Type { get; set; }
        public int FactorId { get; set; }
        public int HostId { get; set; }
        public int IndividualCategoryId { get; set; }
        public string StringValue4 { get; set; }
        public string StringValue5 { get; set; }
        public double FactorField2Value { get; set; }
        public bool FactorField2HasValue { get; set; }
        public SpeciesFactFieldType FactorFieldType2 { get; set; }
        public double FactorFieldValue3 { get; set; }
        public SpeciesFactFieldType FactorFieldType3 { get; set; }

        public int MainParentFactorId { get; set; }
    }

    public enum SpeciesFactFieldType
    {
        INTEGER = 0,
        DOUBLE = 1,
        ENUM = 3,
        STRING = 4,
        BOOLEAN = 5
    }
}
