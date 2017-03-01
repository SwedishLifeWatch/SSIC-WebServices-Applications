// -----------------------------------------------------------------------
// <copyright file="WebReferenceRelationTest.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace ArtDatabanken.WebService.Data.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [TestClass]
    public class WebReferenceRelationTest : WebDomainTestBase<WebReferenceRelation>
    {
        [TestMethod]
        public void Id()
        {
            const Int32 id = 20889;
            GetObject(true).Id = id;
            Assert.AreEqual(GetObject().Id, id);
        }

        [TestMethod]
        public void RelatedObjectGuid()
        {
            const string value = "GUIDValue";
            GetObject(true).RelatedObjectGuid = value;
            Assert.AreEqual(GetObject().RelatedObjectGuid, value);
        }

        [TestMethod]
        public void ReferenceId()
        {
            const Int32 value = 20889;
            GetObject(true).ReferenceId = value;
            Assert.AreEqual(GetObject().ReferenceId, value);
        }
    }
}
