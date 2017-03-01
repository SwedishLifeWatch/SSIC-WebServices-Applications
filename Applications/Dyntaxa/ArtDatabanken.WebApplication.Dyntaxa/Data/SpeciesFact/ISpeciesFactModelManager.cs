using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    public interface ISpeciesFactModelManager
    {
        void UpdatedSpeciecFacts(IList<SpeciesFactFieldValueModelHelper> newValuesInList, bool newCategory, bool updateFieldValue2);

        ArtDatabanken.Data.ITaxon Taxon { set; }
    }
}
