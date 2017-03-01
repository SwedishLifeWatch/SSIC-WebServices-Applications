using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.GIS.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.GIS.Test.Helpers
{
    [TestClass]
    public class CollectionExtensionsTests
    {

        [TestMethod]
        public void SequenceEqualEx_ListOneAndTwoAreNull_ReturnTrue()
        {
            List<int> first = null;
            List<int> second = null;

            Assert.IsTrue(first.SequenceEqualEx(second));
            Assert.IsTrue(second.SequenceEqualEx(first));
        }

        [TestMethod]
        public void SequenceEqualEx_ListOneIsNullListTwoHasZeroElements_ReturnTrue()
        {
            List<int> first = null;
            List<int> second = new List<int>();

            Assert.IsTrue(first.SequenceEqualEx(second));
            Assert.IsTrue(second.SequenceEqualEx(first));
        }

        [TestMethod]
        public void SequenceEqualEx_ListOneIsNullListTwoHasOneElement_ReturnFalse()
        {
            List<int> first = null;
            List<int> second = new List<int> {5};

            Assert.IsFalse(first.SequenceEqualEx(second));
            Assert.IsFalse(second.SequenceEqualEx(first));
        }

        [TestMethod]
        public void SequenceEqualEx_ListOneHasOneElementListTwoHasEqualElement_ReturnTrue()
        {
            List<int> first = new List<int> { 5 };
            List<int> second = new List<int> { 5 };

            Assert.IsTrue(first.SequenceEqualEx(second));
            Assert.IsTrue(second.SequenceEqualEx(first));
        }

    }
}
