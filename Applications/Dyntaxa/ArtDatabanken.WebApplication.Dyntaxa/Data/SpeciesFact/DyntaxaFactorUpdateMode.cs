namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    /// <summary>
    /// This class represents a FactorUpdateMode for a dyntaxa factor.
    /// </summary>
    public class DyntaxaFactorUpdateMode
    {
        private bool isHeader = false;
        private bool okToUpdate = true;

        public DyntaxaFactorUpdateMode(bool isHeader, bool okToUpdate)
        {
            this.isHeader = isHeader;
            this.okToUpdate = okToUpdate;
        }

        public bool IsHeader
        {
            get
            {
                return isHeader;
            }
        }
        public bool OkToUpdate
        {
            get
            {
                return okToUpdate;
            }
        }
    }
}