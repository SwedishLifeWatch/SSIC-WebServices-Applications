// -----------------------------------------------------------------------
// <copyright file="WebRevisionEventTest.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Data.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [TestClass]
    public class WebRevisionEventTest : WebDomainTestBase<WebTaxonRevisionEvent>
    {
        [TestMethod]
        public void Id()
        {
            const int value = 1;
            GetObject(true).Id = value;
            Assert.AreEqual(GetObject().Id, value);
        }

        [TestMethod]
        public void Revision()
        {
            var value = 1;
            GetObject(true).RevisionId = value;
            Assert.AreEqual(GetObject().RevisionId, value);
        }

        //// [DataMember]
        //// public WebEventType EventType { get; set; }

        [TestMethod]
        public void CreatedDate()
        {
            var value = DateTime.Now;
            GetObject(true).CreatedDate = value;
            Assert.AreEqual(GetObject().CreatedDate, value);            
        }

        [TestMethod]
        public void CreatedBy()
        {
            var value = 1;
            GetObject(true).CreatedBy = value;
            Assert.AreEqual(GetObject().CreatedBy, value);
        }
    }
}
