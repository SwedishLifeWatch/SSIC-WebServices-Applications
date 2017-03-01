//using System;
//using System.Text;
//using System.Collections.Generic;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using ArtDatabanken.Data;
//using ArtDatabanken.WebApplication.Dyntaxa.Data;

//namespace ArtDatabanken.WebApplication.Dyntaxa.Test.Data
//{
//    [TestClass]
//    public class MatchItemTest : TestBase
//    {

//        public MatchItemTest()
//        {
            
//        }

//        [TestMethod]
//        public void ConstructorTest()
//        {
//            MatchItem item;
            
//            item = new MatchItem(null);
//            Assert.AreEqual(item.Status, MatchStatus.NoMatch);

//            item = new MatchItem("");
//            Assert.AreEqual(item.Status, MatchStatus.NoMatch);

//            item = new MatchItem("", null);
//            Assert.AreEqual(item.Status, MatchStatus.NoMatch);

//            item = new MatchItem("", "");
//            Assert.AreEqual(item.Status, MatchStatus.NoMatch);

//            item = new MatchItem("", "Hej");
//            Assert.AreEqual(item.Status, MatchStatus.NoMatch);

//            item = new MatchItem("Hej");
//            Assert.AreEqual(item.Status, MatchStatus.Undone);

//            item = new MatchItem("Hej", "");
//            Assert.AreEqual(item.Status, MatchStatus.Undone);

//            item = new MatchItem("Hej", "Hej");
//            Assert.AreEqual(item.Status, MatchStatus.Undone);
            
//        }

//        [TestMethod]
//        public void ProvidedTextTest()
//        {
//            MatchItem item;

//            item = new MatchItem("Hej");
//            Assert.AreEqual(item.ProvidedText, "Hej");

//            item = new MatchItem("Hej", null);
//            Assert.AreEqual(item.ProvidedText, "Hej");

//            item = new MatchItem("Hej", "");
//            Assert.AreEqual(item.ProvidedText, "Hej");

//            item = new MatchItem("Hej", "Hej");
//            Assert.AreEqual(item.ProvidedText, "Hej Hej");
//        }

//        [TestMethod]
//        public void NameStringTest()
//        {
//            MatchItem item;

//            item = new MatchItem("   Psophus   stridulus ");
//            Assert.AreEqual("Psophus stridulus", item.NameString);

//            item = new MatchItem("Psophus   stridulus ", "(Linnaeus, 1758)");
//            Assert.AreEqual("Psophus stridulus", item.NameString);
//        }

//        [TestMethod]
//        public void AuthorStringTest()
//        {
//            MatchItem item;

//            item = new MatchItem("Psophus stridulus");
//            Assert.IsNull(item.AuthorString);

//            item = new MatchItem("Psophus stridulus", "(Linnaeus,    1758)");
//            Assert.AreEqual("(Linnaeus, 1758)", item.AuthorString);
//        }
//    }
//}
