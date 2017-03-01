using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Data.Test
{
    /// <summary>
    /// Unit test for class WebMessageType
    /// </summary>
    [TestClass]
    public class WebMessageTypeTest
    {
        private WebMessageType _messageType;

        public WebMessageTypeTest()
        {
            _messageType = null;
        }

        [TestMethod]
        public void Constructor()
        {
            WebMessageType MessageType;

            MessageType = new WebMessageType();

            Assert.IsNotNull(MessageType);
        }

        private WebMessageType GetMessageType()
        {
            if (_messageType.IsNull())
            {
                _messageType = new WebMessageType();
            }
            return _messageType;
        }



        [TestMethod]
        public void Id()
        {

            GetMessageType().Id = 2;
            Assert.AreEqual(GetMessageType().Id, 2);

        }

        [TestMethod]
        public void NameStringId()
        {

            GetMessageType().NameStringId = 2;
            Assert.AreEqual(GetMessageType().NameStringId, 2);

        }

        [TestMethod]
        public void Name()
        {

            GetMessageType().Name = "No mail";
            Assert.AreEqual(GetMessageType().Name, "No mail");

        }




        #region Additional test attributes
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }


        #endregion


    }
}


