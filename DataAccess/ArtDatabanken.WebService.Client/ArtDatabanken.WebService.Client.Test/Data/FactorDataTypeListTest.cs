using System;
using ArtDatabanken.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class FactorDataTypeListTest : TestBase
    {
        private FactorDataTypeList _factorDataTypes;

        public FactorDataTypeListTest()
        {
            _factorDataTypes = null;
        }

        private FactorDataTypeList GetFactorDataTypes(Boolean refresh = false)
        {
            if (_factorDataTypes.IsNull() || refresh)
            {
                _factorDataTypes = CoreData.FactorManager.GetFactorDataTypes(GetUserContext());
            }

            return _factorDataTypes;
        }

        [TestMethod]
        public void Sort()
        {
            FactorDataTypeList factorDataTypes;
            Int32 index;

            factorDataTypes = GetFactorDataTypes(true);
            factorDataTypes.Sort();
            for (index = 0; index < (factorDataTypes.Count - 1); index++)
            {
                Assert.IsTrue(factorDataTypes[index].Id < factorDataTypes[index + 1].Id);
            }
        }
    }
}
