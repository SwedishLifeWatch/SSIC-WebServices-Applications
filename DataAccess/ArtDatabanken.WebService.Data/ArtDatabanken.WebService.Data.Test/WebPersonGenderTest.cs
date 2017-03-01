using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Data.Test
{
    /// <summary>
    /// Unit test for class WebPersonGender
    /// </summary>
    [TestClass]
    public class WebPersonGenderTest 
    {
        private WebPersonGender _personGender;

        public WebPersonGenderTest()
        {
            _personGender = null;
        }

        [TestMethod]
        public void Constructor()
        {
            WebPersonGender personGender;

            personGender = new WebPersonGender();

            Assert.IsNotNull(personGender);
        }

        private WebPersonGender GetPersonGender()
        {
            if (_personGender.IsNull())
            {
                _personGender = new WebPersonGender();
            }
            return _personGender;
        }



        [TestMethod]
        public void Id()
        {

            GetPersonGender().Id = 2;
            Assert.AreEqual(GetPersonGender().Id, 2);

        }

        [TestMethod]
        public void NameStringId()
        {

            GetPersonGender().NameStringId = 2;
            Assert.AreEqual(GetPersonGender().NameStringId, 2);

        }

        [TestMethod]
        public void Name()
        {

            GetPersonGender().Name = "Kvinna";
            Assert.AreEqual(GetPersonGender().Name, "Kvinna");

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


