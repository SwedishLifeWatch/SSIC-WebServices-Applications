using System;
using ArtDatabanken.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.Test.Security
{
    [TestClass]
    public class CipherStringTest
    {
        [TestMethod]
        public void DecryptText()
        {
            CipherString cipher;
            String key, textIn, textOut;

            cipher = new CipherString();
            key = @"lakflödfaökl3948+0ikaoja23";

            textIn = "Hej hopp i lingonskogen!";
            textOut = cipher.EncryptText(textIn, key);
            textOut = cipher.DecryptText(textOut, key);
            Assert.AreEqual(textIn, textOut);

            textIn = "Hej hopp i lingonskogen! Det här är en lång text";
            textOut = cipher.EncryptText(textIn, key);
            textOut = cipher.DecryptText(textOut, key);
            Assert.AreEqual(textIn, textOut);
        }
    }
}
