using System;
using System.Linq;
using System.Xml.Linq;
using ArtDatabanken.GIS;
using ArtDatabanken.GIS.GeoJSON.Net.CoordinateReferenceSystem;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings;
using Newtonsoft.Json.Linq;

namespace AnalysisPortal.Controllers
{
    /// <summary>
    /// This Controller has Actions that is used to select data sources.
    /// </summary>
    public class WfsController : BaseController
    {
        public string Index()
        {
            var srsName = Request["srsName"];
            var typeName = Request["typeName"];

            if (string.IsNullOrEmpty(typeName) && Request.HttpMethod == "POST")
            {
                XDocument payload;

                try
                {
                    //Try to load xml from payload
                    using (var input = Request.InputStream)
                    {
                        payload = XDocument.Load(input);
                        input.Close();
                    }
                }
                catch
                {
                    return null;
                }

                if (payload == null || payload.Root == null)
                {
                    return null;
                }

                var outputFormatAttribute = payload.Root.Attribute("outputFormat");
                if (outputFormatAttribute == null || outputFormatAttribute.Value != "json")
                {
                    return null;
                }

                var wfsQuery =
                    payload.Root.Descendants(XName.Get("Query", payload.Root.Name.Namespace.NamespaceName))
                        .FirstOrDefault();

                if (wfsQuery == null)
                {
                    return null;
                }

                srsName = wfsQuery.Attribute("srsName").Value;
                typeName = wfsQuery.Attribute("typeName").Value;
            }

            typeName = Server.UrlDecode(typeName);

            var fileName = typeName.Substring(typeName.LastIndexOf(":", StringComparison.CurrentCulture) + 1);
            var requestedCoordinateSystemId = GisTools.GeoJsonUtils.FindCoordinateSystemId(new NamedCRS(srsName));

            return JObject.FromObject(MySettingsManager.GetMapDataFeatureCollection(GetCurrentUser(), fileName, requestedCoordinateSystemId)).ToString();
        }
    }
}
