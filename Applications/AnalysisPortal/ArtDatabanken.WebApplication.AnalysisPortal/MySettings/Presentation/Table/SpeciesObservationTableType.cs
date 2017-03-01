using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation.Table
{
    public struct SpeciesObservationTableColumnsSetId
    {        
        public SpeciesObservationTableColumnsSetId(bool useUserDefinedTableType, int tableId)
        {
            UseUserDefinedTableType = useUserDefinedTableType;
            TableId = tableId;
        }

        public bool UseUserDefinedTableType { get; set; }
        
        public int TableId { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return String.Format("UseUserDefinedTableType: {0}, TableId: {1}", UseUserDefinedTableType, TableId);
        }
    }
}
