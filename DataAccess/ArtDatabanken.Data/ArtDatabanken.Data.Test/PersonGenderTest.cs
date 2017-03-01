using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.Data.Test
{
    [TestClass]
    public class PersonGenderTest
    {
        PersonGender _personGender;

        public PersonGenderTest()
        {
            _personGender = null;
        }

        [TestMethod]
        public void Constructor()
        {
            PersonGender personGender;
            personGender = new PersonGender(1, "Test", 1, DataContextTest.GetOneDataContext());
            Assert.IsNotNull(personGender);
        }

        [TestMethod]
        public void DataContext()
        {
            Assert.IsNotNull(GetPersonGender(true).DataContext);
        }

        private PersonGender GetPersonGender()
        {
            return GetPersonGender(false);
        }

        private PersonGender GetPersonGender(Boolean refresh)
        {
            if (_personGender.IsNull() || refresh)
            {
                _personGender = new PersonGender(1, "Test", 1, DataContextTest.GetOneDataContext());
            }
            return _personGender;
        }

        [TestMethod]
        public void Id()
        {
            Assert.AreNotEqual(GetPersonGender(true).Id, Int32.MinValue);
        }

        [TestMethod]
        public void Name()
        {
            Assert.IsTrue(GetPersonGender().Name.IsNotEmpty());
        }

        [TestMethod]
        public void NameStringId()
        {
            Assert.AreNotEqual(GetPersonGender(true).NameStringId, Int32.MinValue);
        }
    }
}