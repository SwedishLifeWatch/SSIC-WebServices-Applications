using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Client.UserService;

namespace ArtDatabanken.WebService.Client.Test.Data
{
    [TestClass]
    public class PersonListTest : TestBase
    {
        private PersonList _persons;

        public PersonListTest()
        {
            _persons = null;
        }

        [TestMethod]
        public void Constructor()
        {
            PersonList persons;

            persons = new PersonList();
            Assert.IsNotNull(persons);
            persons = new PersonList(true);
            Assert.IsNotNull(persons);
            persons = new PersonList(false);
            Assert.IsNotNull(persons);
        }

        [TestMethod]
        public void Get()
        {
            foreach (IPerson person in GetPersons(true))
            {
                Assert.AreEqual(person, GetPersons().Get(person.Id));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetIdError()
        {
            Int32 personId;

            personId = Int32.MaxValue;
            GetPersons(true).Get(personId);
        }

        private PersonList GetPersons()
        {
            return GetPersons(false);
        }

        private PersonList GetPersons(Boolean refresh)
        {
            if (_persons.IsNull() || refresh)
            {
                _persons = new PersonList();
                _persons.Add(PersonTest.GetOnePerson(GetUserContext()));
            }
            return _persons;
        }

        [TestMethod]
        public void SquareBracketOperator()
        {
            PersonList newPersonList, oldPersonList;
            Int32 personIndex;

            oldPersonList = GetPersons(true);
            newPersonList = new PersonList();
            for (personIndex = 0; personIndex < oldPersonList.Count; personIndex++)
            {
                newPersonList.Add(oldPersonList[oldPersonList.Count - personIndex - 1]);
            }
            for (personIndex = 0; personIndex < oldPersonList.Count; personIndex++)
            {
                Assert.AreEqual(newPersonList[personIndex], oldPersonList[oldPersonList.Count - personIndex - 1]);
            }
        }
    }
}
