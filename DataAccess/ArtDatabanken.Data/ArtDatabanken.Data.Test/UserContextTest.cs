using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.Data.Test
{
    [TestClass]
    public class UserContextTest
    {
        UserContext _userContext;

        public UserContextTest()
        {
            _userContext = null;
        }

        [TestMethod]
        public void Constructor()
        {
            UserContext userContext;

            userContext = new UserContext();
            Assert.IsNotNull(userContext);
        }

        private UserContext GetUserContext()
        {
            return GetUserContext(false);
        }

        private UserContext GetUserContext(Boolean refresh)
        {
            if (_userContext.IsNull() || refresh)
            {
                _userContext = new UserContext();
            }
            return _userContext;
        }

        public static UserContext GetOneUserContext()
        {
            UserContext userContext;

            userContext = new UserContext();
            return userContext;
        }

        [TestMethod]
        public void Locale()
        {
            Locale locale;
            locale = new Locale(Settings.Default.SwedishLocaleId,
                                Settings.Default.SwedishLocaleISOCode,
                                Settings.Default.SwedishLocaleName,
                                Settings.Default.SwedishLocaleNativeName,
                                new DataContext(new DataSourceInformation(), null));
            Assert.IsNotNull(locale);
            GetUserContext(true).Locale = locale;
            Assert.AreEqual(locale, GetUserContext().Locale);
        }

        [TestMethod]
        public void Properties()
        {
            String key, locale;

            Assert.IsNotNull(GetUserContext(true).Properties);

            key = "Key";
            locale = Settings.Default.SwedishLocaleISOCode;
            GetUserContext().Properties[key] = locale;
            Assert.AreEqual(locale, GetUserContext().Properties[key]);
        }

        [TestMethod]
        public void Transaction()
        {
            Assert.IsNull(GetUserContext(true).Transaction);
        }

        [TestMethod]
        public void User()
        {
            Assert.IsNull(GetUserContext(true).User);
        }

        private Locale GetOneLocale()
        {
            return new Locale(
                Settings.Default.SwedishLocaleId,
                Settings.Default.SwedishLocaleISOCode,
                Settings.Default.SwedishLocaleName,
                Settings.Default.SwedishLocaleNativeName,
                new DataContext(new DataSourceInformation(), null));
        }

        [TestMethod]
        public void Serialize()
        {
            UserContext context = new UserContext();
            context = GetOneUserContext();
            context.Locale = GetOneLocale();
            Stream stream = null;
            //XmlSerializer serializer = new XmlSerializer(typeof(UserContext));
            String fileName = "c:\\apa.txt";
            try 
            {                
                IFormatter formatter = new BinaryFormatter();

                /*
                using (stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    serializer.Serialize(stream, context);
                    stream.Flush();
                }
                 */
                stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, context);            
               
            } 
            catch 
            {                
                // do nothing, just ignore any possible errors            
            } 
            finally 
            {                
                if (null != stream)                    
                    stream.Close();            
            }
            /*
            try 
            {            
                IFormatter formatter = new BinaryFormatter(); 
                stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
                context = (UserContext)formatter.Deserialize(stream);
            } 
            catch 
            {            
                // do nothing, just ignore any possible errors        
            } 
            finally 
            {            
                if (null != stream)                
                    stream.Close();        
            } 
             */
        }

    }
}
