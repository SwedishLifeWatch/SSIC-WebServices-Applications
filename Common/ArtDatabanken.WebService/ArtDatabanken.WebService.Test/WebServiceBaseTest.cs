using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ArtDatabanken.WebService.Test
{
    [TestClass]
    public class WebServiceBaseTest : TestBase
    {
        [TestMethod]
        public void GetResourceType()
        {
            List<LocaleId> localeIds;
            WebResourceType resourceType;

            localeIds = new List<LocaleId>();
            localeIds.Add(LocaleId.en_GB);
            localeIds.Add(LocaleId.sv_SE);
            foreach (ResourceTypeIdentifier resourceTypeIdentifier in Enum.GetValues(typeof(ResourceTypeIdentifier)))
            {
                foreach (LocaleId localeId in localeIds)
                {
                    resourceType = WebServiceBase.GetResourceType(resourceTypeIdentifier, (Int32) localeId);
                    Assert.IsNotNull(resourceType);
                }
            }
        }
    }
}
