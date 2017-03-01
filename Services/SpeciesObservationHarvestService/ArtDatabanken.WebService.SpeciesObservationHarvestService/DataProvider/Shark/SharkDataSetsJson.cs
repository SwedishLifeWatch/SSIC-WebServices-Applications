using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Shark
{
    public class SharkDataSetsJson
    {
        public int Page { get; set; }

        public int Pages { get; set; }

        public int Per_page { get; set; }

        public int Total { get; set; }

        public List<string> Header { get; set; }

        public List<List<string>> Rows { get; set; }
    }
}
