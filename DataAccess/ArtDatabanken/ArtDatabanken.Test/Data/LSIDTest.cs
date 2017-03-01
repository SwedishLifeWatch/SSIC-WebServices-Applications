using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ArtDatabanken.Data;

namespace ArtDatabanken.Test.Data
{
    [TestClass]
    public class LSIDTest
    {
        [TestMethod]
        public void Authority()
        {
            LSID lsid;
            String authority, GUID, nameSpace, objectID;

            GUID = "URN:LSID:artportalen.se:area:DataSet1Feature2";
            lsid = new LSID(GUID);
            Assert.AreEqual(lsid.Authority, "artportalen.se");

            authority = "artportalen.se";
            nameSpace = "area";
            objectID = "DataSet1Feature2";
            lsid = new LSID(authority, nameSpace, objectID);
            Assert.AreEqual(lsid.Authority, authority);
        }

        [TestMethod]
        public void Constructor()
        {
            LSID lsid;
            String authority, GUID, nameSpace, objectID, version;

            GUID = "URN:LSID:artportalen.se:area:DataSet1Feature2";
            lsid = new LSID(GUID);
            Assert.IsNotNull(lsid);
            Assert.AreEqual(lsid.GUID, GUID);
            Assert.AreEqual(lsid.Authority, "artportalen.se");
            Assert.AreEqual(lsid.NameSpace, "area");
            Assert.AreEqual(lsid.ObjectID, "DataSet1Feature2");
            Assert.IsTrue(lsid.Version.IsEmpty());

            GUID = "URN:LSID:artportalen.se:area:DataSet1Feature2:5";
            lsid = new LSID(GUID);
            Assert.IsNotNull(lsid);
            Assert.AreEqual(lsid.GUID, GUID);
            Assert.AreEqual(lsid.Authority, "artportalen.se");
            Assert.AreEqual(lsid.NameSpace, "area");
            Assert.AreEqual(lsid.ObjectID, "DataSet1Feature2");
            Assert.AreEqual(lsid.Version, "5");

            GUID = "URN:LSID:artportalen.se:area:DataSet1Feature2";
            authority = "artportalen.se";
            nameSpace = "area";
            objectID = "DataSet1Feature2";
            lsid = new LSID(authority, nameSpace, objectID);
            Assert.IsNotNull(lsid);
            Assert.AreEqual(lsid.GUID, GUID);
            Assert.AreEqual(lsid.Authority, authority);
            Assert.AreEqual(lsid.NameSpace, nameSpace);
            Assert.AreEqual(lsid.ObjectID, objectID);
            Assert.IsTrue(lsid.Version.IsEmpty());

            GUID = "URN:LSID:artportalen.se:area:DataSet1Feature2:5";
            authority = "artportalen.se";
            nameSpace = "area";
            objectID = "DataSet1Feature2";
            version = "5";
            lsid = new LSID(authority, nameSpace, objectID, version);
            Assert.IsNotNull(lsid);
            Assert.AreEqual(lsid.GUID, GUID);
            Assert.AreEqual(lsid.Authority, authority);
            Assert.AreEqual(lsid.NameSpace, nameSpace);
            Assert.AreEqual(lsid.ObjectID, objectID);
            Assert.AreEqual(lsid.Version, version);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorEmptyGUIDError()
        {
            LSID lsid;
            String GUID;

            GUID = String.Empty;
            lsid = new LSID(GUID);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorNoAuthorityError()
        {
            LSID lsid;
            String authority, GUID, nameSpace, objectID;

            GUID = "URN:LSID:artportalen.se:area:DataSet1Feature2";
            authority = null;
            nameSpace = "area";
            objectID = "DataSet1Feature2";
            lsid = new LSID(authority, nameSpace, objectID);
            Assert.IsNotNull(lsid);
            Assert.AreEqual(lsid.GUID, GUID);
            Assert.AreEqual(lsid.Authority, authority);
            Assert.AreEqual(lsid.NameSpace, nameSpace);
            Assert.AreEqual(lsid.ObjectID, objectID);
            Assert.IsTrue(lsid.Version.IsEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorNoNameSpaceError()
        {
            LSID lsid;
            String authority, GUID, nameSpace, objectID;

            GUID = "URN:LSID:artportalen.se:area:DataSet1Feature2";
            authority = "artportalen.se:area";
            nameSpace = string.Empty;
            objectID = "DataSet1Feature2";
            lsid = new LSID(authority, nameSpace, objectID);
            Assert.IsNotNull(lsid);
            Assert.AreEqual(lsid.GUID, GUID);
            Assert.AreEqual(lsid.Authority, authority);
            Assert.AreEqual(lsid.NameSpace, nameSpace);
            Assert.AreEqual(lsid.ObjectID, objectID);
            Assert.IsTrue(lsid.Version.IsEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorNoObjectIDError()
        {
            LSID lsid;
            String authority, GUID, nameSpace, objectID;

            GUID = "URN:LSID:artportalen.se:area:DataSet1Feature2";
            authority = "artportalen.se:area";
            nameSpace = "area";
            objectID = string.Empty;
            lsid = new LSID(authority, nameSpace, objectID);
            Assert.IsNotNull(lsid);
            Assert.AreEqual(lsid.GUID, GUID);
            Assert.AreEqual(lsid.Authority, authority);
            Assert.AreEqual(lsid.NameSpace, nameSpace);
            Assert.AreEqual(lsid.ObjectID, objectID);
            Assert.IsTrue(lsid.Version.IsEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorToFewPartsError()
        {
            LSID lsid;
            String GUID;

            GUID = "URN:LSID:artportalen.se:area";
            lsid = new LSID(GUID);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorToManyPartsError()
        {
            LSID lsid;
            String GUID;

            GUID = "URN:LSID:artportalen.se:area:DataSet1Feature2:5:42";
            lsid = new LSID(GUID);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorWrongLsidNameSpaceError()
        {
            LSID lsid;
            String GUID;

            GUID = "URN:LSID2:artportalen.se:area:DataSet1Feature2:5:42";
            lsid = new LSID(GUID);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorWrongUrnError()
        {
            LSID lsid;
            String GUID;

            GUID = "URN2:LSID:artportalen.se:area:DataSet1Feature2:5:42";
            lsid = new LSID(GUID);
        }

        [TestMethod]
        public void GUID()
        {
            LSID lsid;
            String authority, GUID, nameSpace, objectID;

            GUID = "URN:LSID:artportalen.se:area:DataSet1Feature2";
            lsid = new LSID(GUID);
            Assert.AreEqual(lsid.GUID, GUID);

            authority = "artportalen.se";
            nameSpace = "area";
            objectID = "DataSet1Feature2";
            lsid = new LSID(authority, nameSpace, objectID);
            Assert.AreEqual(lsid.GUID, "URN:LSID:artportalen.se:area:DataSet1Feature2");
        }

        [TestMethod]
        public void NameSpace()
        {
            LSID lsid;
            String authority, GUID, nameSpace, objectID;

            GUID = "URN:LSID:artportalen.se:area:DataSet1Feature2";
            lsid = new LSID(GUID);
            Assert.AreEqual(lsid.NameSpace, "area");

            authority = "artportalen.se";
            nameSpace = "area";
            objectID = "DataSet1Feature2";
            lsid = new LSID(authority, nameSpace, objectID);
            Assert.AreEqual(lsid.NameSpace, nameSpace);
        }

        [TestMethod]
        public void ObjectID()
        {
            LSID lsid;
            String authority, GUID, nameSpace, objectID;

            GUID = "URN:LSID:artportalen.se:area:DataSet1Feature2";
            lsid = new LSID(GUID);
            Assert.AreEqual(lsid.ObjectID, "DataSet1Feature2");

            authority = "artportalen.se";
            nameSpace = "area";
            objectID = "DataSet1Feature2";
            lsid = new LSID(authority, nameSpace, objectID);
            Assert.AreEqual(lsid.ObjectID, objectID);
        }

        [TestMethod]
        public void Version()
        {
            LSID lsid;
            String authority, GUID, nameSpace, objectID, version;

            GUID = "URN:LSID:artportalen.se:area:DataSet1Feature2:2";
            lsid = new LSID(GUID);
            Assert.AreEqual(lsid.Version, "2");

            GUID = "URN:LSID:artportalen.se:area:DataSet1Feature2";
            lsid = new LSID(GUID);
            Assert.IsTrue(lsid.Version.IsEmpty());

            authority = "artportalen.se";
            nameSpace = "area";
            objectID = "DataSet1Feature2";
            version = "2";
            lsid = new LSID(authority, nameSpace, objectID, version);
            Assert.AreEqual(lsid.Version, version);
        }
    }
}
