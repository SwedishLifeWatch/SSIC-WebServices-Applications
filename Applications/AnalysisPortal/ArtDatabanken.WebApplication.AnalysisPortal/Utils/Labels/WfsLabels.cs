namespace ArtDatabanken.WebApplication.AnalysisPortal.Utils.Labels
{
    public sealed class WfsLabels
    {
        private static readonly WfsLabels instance = new WfsLabels();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static WfsLabels()
        {
        }

        private WfsLabels()
        {
        }

        public static WfsLabels Instance
        {
            get
            {
                return instance;
            }
        }

        public string GreaterThanOperator = "Greater than (>)";
        public string LessThanOperator = "Less than (<)";
        public string GreatorOrEqualToOperator = "Greater or equal to (>=)";
        public string LessOrEqualToOperator = "Less or equal to (<=)";
        public string NotEqualToOperator = "Not equal to (<>)";
        public string EqualToOperator = "EqualTo (=)";
        public string LikeOperator = "Like";
        public string IsNullOperator = "Is null";        
        public string LeftOperand = "Left operand";
        public string RightOperand = "Right operand";
        public string Operator = "Operator";
        public string Constant = "Constant";
        public string Field = "Field";
        public string Filter = "Filter";
    } 
}
