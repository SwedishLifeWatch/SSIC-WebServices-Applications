using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.UserService;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class PersonGenderListTest : TestBase
    {
        private PersonGenderList _personGenders;

        public PersonGenderListTest()
        {
            _personGenders = null;
        }

        [TestMethod]
        public void Constructor()
        {
            PersonGenderList personGenders;

            personGenders = new PersonGenderList();
            Assert.IsNotNull(personGenders);
        }

        [TestMethod]
        public void Get()
        {
            IPersonGender personGender;

            GetPersonGenders(true);
            foreach (PersonGenderId personGenderId in Enum.GetValues(typeof(PersonGenderId)))
            {
                personGender = GetPersonGenders().Get(personGenderId);
                Assert.IsNotNull(personGender);
            }

            foreach (IPersonGender tempPersonGender in GetPersonGenders())
            {
                Assert.AreEqual(tempPersonGender, GetPersonGenders().Get(tempPersonGender.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 personGenderId;

            personGenderId = Int32.MinValue;
            GetPersonGenders(true).Get(personGenderId);
        }

        private PersonGenderList GetPersonGenders()
        {
            return GetPersonGenders(false);
        }

        private PersonGenderList GetPersonGenders(Boolean refresh)
        {
            if (_personGenders.IsNull() || refresh)
            {
                _personGenders = CoreData.UserManager.GetPersonGenders(GetUserContext());
            }
            return _personGenders;
        }

        [TestMethod]
        public void SquareBracketOperator()
        {
            PersonGenderList newPersonGenderList, oldPersonGenderList;
            Int32 personGenderIndex;

            oldPersonGenderList = GetPersonGenders(true);
            newPersonGenderList = new PersonGenderList();
            for (personGenderIndex = 0; personGenderIndex < oldPersonGenderList.Count; personGenderIndex++)
            {
                newPersonGenderList.Add(oldPersonGenderList[oldPersonGenderList.Count - personGenderIndex - 1]);
            }
            for (personGenderIndex = 0; personGenderIndex < oldPersonGenderList.Count; personGenderIndex++)
            {
                Assert.AreEqual(newPersonGenderList[personGenderIndex], oldPersonGenderList[oldPersonGenderList.Count - personGenderIndex - 1]);
            }
        }
    }
}
