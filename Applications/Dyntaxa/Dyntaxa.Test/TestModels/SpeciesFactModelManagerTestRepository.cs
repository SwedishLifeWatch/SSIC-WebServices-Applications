using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using ArtDatabanken.WebApplication.Dyntaxa.Data;

namespace Dyntaxa.Test.TestModels
{
    public class SpeciesFactModelManagerTestRepository : ISpeciesFactModelManager
    {
        public void UpdatedSpeciecFacts(IList<SpeciesFactFieldValueModelHelper> newValuesInList, bool newCategory)
        {
            return;
        }

        public void UpdatedSpeciecFacts(IList<SpeciesFactFieldValueModelHelper> newValuesInList, bool newCategory, bool updateFieldValue2)
        {
            throw new NotImplementedException();
        }

        public ITaxon Taxon { set; private get; }
    }
}
