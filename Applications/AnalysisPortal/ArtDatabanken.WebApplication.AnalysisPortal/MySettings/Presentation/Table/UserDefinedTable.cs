using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.Presentation.Table
{
    [DataContract]
    public class UserDefinedTable
    {
        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public List<int> FieldIds { get; set; }
    }
}
