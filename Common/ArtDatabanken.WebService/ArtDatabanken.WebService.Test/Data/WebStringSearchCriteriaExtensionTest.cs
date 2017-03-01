using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Test.Data
{
    [TestClass]
    public class WebStringSearchCriteriaExtensionTest
    {
        private WebStringSearchCriteria _stringSearchCriteria;

        public WebStringSearchCriteriaExtensionTest()
        {
            _stringSearchCriteria = null;
        }

        public WebStringSearchCriteria GetStringSearchCriteria(Boolean refresh = false)
        {
            if (_stringSearchCriteria.IsNull() || refresh)
            {
                _stringSearchCriteria = new WebStringSearchCriteria();
            }

            return _stringSearchCriteria;
        }

        [TestMethod]
        public void WebToString()
        {
            String stringSearchCriteria;

            _stringSearchCriteria = null;
            stringSearchCriteria = _stringSearchCriteria.WebToString();
            Assert.IsTrue(stringSearchCriteria.IsEmpty());

            GetStringSearchCriteria(true).CompareOperators = new List<StringCompareOperator>();
            GetStringSearchCriteria().SearchString = "hej hopp%";
            stringSearchCriteria = GetStringSearchCriteria().WebToString();
            Assert.IsTrue(stringSearchCriteria.IsNotEmpty());

            GetStringSearchCriteria().CompareOperators.Add(StringCompareOperator.Equal);
            stringSearchCriteria = GetStringSearchCriteria().WebToString();
            Assert.IsTrue(stringSearchCriteria.IsNotEmpty());

            GetStringSearchCriteria().CompareOperators.Add(StringCompareOperator.BeginsWith);
            stringSearchCriteria = GetStringSearchCriteria().WebToString();
            Assert.IsTrue(stringSearchCriteria.IsNotEmpty());
        }
    }
}
