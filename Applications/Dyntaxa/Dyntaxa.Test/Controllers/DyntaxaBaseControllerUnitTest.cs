using System.Web;
using System.Web.Routing;
using Dyntaxa.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using System.Collections.Specialized;

namespace Dyntaxa.Test
{
     
    ///<summary>
    ///This is a test class for DyntaxaBaseControllerTest and is intended
    ///to contain all DyntaxaBaseControllerTest Unit Tests
    /// </summary>
    [TestClass()]
    public class DyntaxaBaseControllerTest
    {


        /// <summary>
        ///A test for ConstructQueryString
        ///</summary>        
        [TestMethod()]
        [TestCategory("UnitTestApp")]
        public void EncodeDecodeQueryStringTest()
        {
            DyntaxaBaseController controller = new DyntaxaBaseController();
            RouteValueDictionary dic = new RouteValueDictionary();
            dic.Add("mode", "full");
            dic.Add("search", "hästar och fåglar");
            string encodedQueryString = controller.EncodeRouteQueryString(dic);

            RouteValueDictionary dicDecoded = controller.DecodeRouteQueryString(encodedQueryString);
            Assert.AreEqual("full", dicDecoded["mode"]);
            Assert.AreEqual("hästar och fåglar", dicDecoded["search"]);
        }

    }
}
