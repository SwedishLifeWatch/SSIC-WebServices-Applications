using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.Data.Test
{
    [TestClass]
    public class RedListCriteriaBuilderTest
    {
        private RedListCriteriaBuilder _redListCriteriaBuilder;

        public RedListCriteriaBuilderTest()
        {
            _redListCriteriaBuilder = null;
        }

        [TestMethod]
        public void AddCriteriaLevel1()
        {
            // Add one criteria.
            GetRedListCriteriaBuilder(true).AddCriteriaLevel1("A");
            Assert.AreEqual("A", GetRedListCriteriaBuilder().ToString());

            // Add more than one criteria.
            GetRedListCriteriaBuilder(true).AddCriteriaLevel1("A");
            GetRedListCriteriaBuilder().AddCriteriaLevel1("B");
            Assert.AreEqual("A; B", GetRedListCriteriaBuilder().ToString());
        }

        [TestMethod]
        public void AddCriteriaLevel2()
        {
            // Add one criteria.
            GetRedListCriteriaBuilder(true).AddCriteriaLevel1("A");
            GetRedListCriteriaBuilder().AddCriteriaLevel2(1);
            Assert.AreEqual("A1", GetRedListCriteriaBuilder().ToString());

            // Add more than one criteria.
            GetRedListCriteriaBuilder(true).AddCriteriaLevel1("A");
            GetRedListCriteriaBuilder().AddCriteriaLevel2(1);
            GetRedListCriteriaBuilder().AddCriteriaLevel2(2);
            Assert.AreEqual("A1+2", GetRedListCriteriaBuilder().ToString());

            // Add criteria to more than one criteria of level 1.
            GetRedListCriteriaBuilder(true).AddCriteriaLevel1("A");
            GetRedListCriteriaBuilder().AddCriteriaLevel2(1);
            GetRedListCriteriaBuilder().AddCriteriaLevel2(2);
            GetRedListCriteriaBuilder().AddCriteriaLevel1("B");
            GetRedListCriteriaBuilder().AddCriteriaLevel2(3);
            Assert.AreEqual("A1+2; B3", GetRedListCriteriaBuilder().ToString());

            // Add more than one criteria of level 2
            // to more than one criteria of level 1.
            GetRedListCriteriaBuilder(true).AddCriteriaLevel1("A");
            GetRedListCriteriaBuilder().AddCriteriaLevel2(1);
            GetRedListCriteriaBuilder().AddCriteriaLevel2(2);
            GetRedListCriteriaBuilder().AddCriteriaLevel1("B");
            GetRedListCriteriaBuilder().AddCriteriaLevel2(3);
            GetRedListCriteriaBuilder().AddCriteriaLevel2(5);
            Assert.AreEqual("A1+2; B3+5", GetRedListCriteriaBuilder().ToString());
        }

        [TestMethod]
        public void AddCriteriaLevel3()
        {
            // Add one criteria.
            GetRedListCriteriaBuilder(true).AddCriteriaLevel1("A");
            GetRedListCriteriaBuilder().AddCriteriaLevel2(1);
            GetRedListCriteriaBuilder().AddCriteriaLevel3("a");
            Assert.AreEqual("A1a", GetRedListCriteriaBuilder().ToString());

            // Add more than one criteria.
            GetRedListCriteriaBuilder(true).AddCriteriaLevel1("A");
            GetRedListCriteriaBuilder().AddCriteriaLevel2(1);
            GetRedListCriteriaBuilder().AddCriteriaLevel2(2);
            GetRedListCriteriaBuilder().AddCriteriaLevel3("a");
            GetRedListCriteriaBuilder().AddCriteriaLevel3("d");
            Assert.AreEqual("A1+2ad", GetRedListCriteriaBuilder().ToString());

            // Add criteria to more than one criteria of level 1.
            GetRedListCriteriaBuilder(true).AddCriteriaLevel1("A");
            GetRedListCriteriaBuilder().AddCriteriaLevel2(1);
            GetRedListCriteriaBuilder().AddCriteriaLevel3("a");
            GetRedListCriteriaBuilder().AddCriteriaLevel2(2);
            GetRedListCriteriaBuilder().AddCriteriaLevel1("B");
            GetRedListCriteriaBuilder().AddCriteriaLevel2(3);
            GetRedListCriteriaBuilder().AddCriteriaLevel3("c");
            Assert.AreEqual("A1a+2; B3c", GetRedListCriteriaBuilder().ToString());

            // Add more than one criteria of level 3
            // to more than one criteria of level 2.
            GetRedListCriteriaBuilder(true).AddCriteriaLevel1("A");
            GetRedListCriteriaBuilder().AddCriteriaLevel2(1);
            GetRedListCriteriaBuilder().AddCriteriaLevel3("a");
            GetRedListCriteriaBuilder().AddCriteriaLevel3("b");
            GetRedListCriteriaBuilder().AddCriteriaLevel2(2);
            GetRedListCriteriaBuilder().AddCriteriaLevel1("B");
            GetRedListCriteriaBuilder().AddCriteriaLevel2(3);
            GetRedListCriteriaBuilder().AddCriteriaLevel3("e");
            GetRedListCriteriaBuilder().AddCriteriaLevel2(5);
            GetRedListCriteriaBuilder().AddCriteriaLevel3("a");
            GetRedListCriteriaBuilder().AddCriteriaLevel3("b");
            Assert.AreEqual("A1ab+2; B3e+5ab", GetRedListCriteriaBuilder().ToString());
        }

        [TestMethod]
        public void AddCriteriaLevel4()
        {
            // Add one criteria.
            GetRedListCriteriaBuilder(true).AddCriteriaLevel1("A");
            GetRedListCriteriaBuilder().AddCriteriaLevel2(1);
            GetRedListCriteriaBuilder().AddCriteriaLevel3("a");
            GetRedListCriteriaBuilder().AddCriteriaLevel4(1);
            Assert.AreEqual("A1a(i)", GetRedListCriteriaBuilder().ToString());

            // Add more than one criteria.
            GetRedListCriteriaBuilder(true).AddCriteriaLevel1("A");
            GetRedListCriteriaBuilder().AddCriteriaLevel2(1);
            GetRedListCriteriaBuilder().AddCriteriaLevel2(2);
            GetRedListCriteriaBuilder().AddCriteriaLevel3("a");
            GetRedListCriteriaBuilder().AddCriteriaLevel4(1);
            GetRedListCriteriaBuilder().AddCriteriaLevel3("d");
            GetRedListCriteriaBuilder().AddCriteriaLevel4(3);
            GetRedListCriteriaBuilder().AddCriteriaLevel4(4);
            Assert.AreEqual("A1+2a(i)d(iii,iv)", GetRedListCriteriaBuilder().ToString());

            // Add criteria to more than one criteria of level 1.
            GetRedListCriteriaBuilder(true).AddCriteriaLevel1("A");
            GetRedListCriteriaBuilder().AddCriteriaLevel2(1);
            GetRedListCriteriaBuilder().AddCriteriaLevel3("a");
            GetRedListCriteriaBuilder().AddCriteriaLevel4(3);
            GetRedListCriteriaBuilder().AddCriteriaLevel4(4);
            GetRedListCriteriaBuilder().AddCriteriaLevel2(2);
            GetRedListCriteriaBuilder().AddCriteriaLevel1("B");
            GetRedListCriteriaBuilder().AddCriteriaLevel2(3);
            GetRedListCriteriaBuilder().AddCriteriaLevel3("c");
            GetRedListCriteriaBuilder().AddCriteriaLevel4(1);
            GetRedListCriteriaBuilder().AddCriteriaLevel4(4);
            Assert.AreEqual("A1a(iii,iv)+2; B3c(i,iv)", GetRedListCriteriaBuilder().ToString());

            // Add more than one criteria of level 4
            // to more than one criteria of level 3.
            GetRedListCriteriaBuilder(true).AddCriteriaLevel1("A");
            GetRedListCriteriaBuilder().AddCriteriaLevel2(1);
            GetRedListCriteriaBuilder().AddCriteriaLevel3("a");
            GetRedListCriteriaBuilder().AddCriteriaLevel4(4);
            GetRedListCriteriaBuilder().AddCriteriaLevel3("b");
            GetRedListCriteriaBuilder().AddCriteriaLevel4(1);
            GetRedListCriteriaBuilder().AddCriteriaLevel4(5);
            GetRedListCriteriaBuilder().AddCriteriaLevel2(2);
            GetRedListCriteriaBuilder().AddCriteriaLevel1("B");
            GetRedListCriteriaBuilder().AddCriteriaLevel2(3);
            GetRedListCriteriaBuilder().AddCriteriaLevel3("e");
            GetRedListCriteriaBuilder().AddCriteriaLevel4(2);
            GetRedListCriteriaBuilder().AddCriteriaLevel4(3);
            GetRedListCriteriaBuilder().AddCriteriaLevel2(5);
            GetRedListCriteriaBuilder().AddCriteriaLevel3("a");
            GetRedListCriteriaBuilder().AddCriteriaLevel3("b");
            Assert.AreEqual("A1a(iv)b(i,v)+2; B3e(ii,iii)+5ab", GetRedListCriteriaBuilder().ToString());
        }

        [TestMethod]
        public void Constructor()
        {
            Assert.IsNotNull(GetRedListCriteriaBuilder(true));
            Assert.IsTrue(GetRedListCriteriaBuilder().ToString().IsNotNull());
        }

        private RedListCriteriaBuilder GetRedListCriteriaBuilder(Boolean refresh = false)
        {
            if (_redListCriteriaBuilder.IsNull() || refresh)
            {
                _redListCriteriaBuilder = new RedListCriteriaBuilder();
            }

            return _redListCriteriaBuilder;
        }
    }
}
