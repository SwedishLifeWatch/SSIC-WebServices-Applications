using System;
using System.Collections.Generic;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.DataProvider.Gbif
{
    public class GbifDataSetsJson
    {
        public Int64 Offset { get; set; }

        public Int64 Limit { get; set; }

        public bool EndOfRecords { get; set; }

        public Int64 Count { get; set; }

        public List<Dictionary<string, object>> Results { get; set; }
    }
}
