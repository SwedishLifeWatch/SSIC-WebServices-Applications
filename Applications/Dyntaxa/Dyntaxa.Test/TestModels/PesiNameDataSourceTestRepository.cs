using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using Dyntaxa.Helpers;

namespace Dyntaxa.Test.TestModels
{
    public class PesiNameDataSourceTestRepository : IPesiNameDataSource

    {
        public string GetPesiGuidByVernacularName(string name)
        {
            return "PESIRecords";
        }

        public string GetPesiGuidByScientificName(string scientificName)
        {
            return "PESIRecords";
        }
    }
}
