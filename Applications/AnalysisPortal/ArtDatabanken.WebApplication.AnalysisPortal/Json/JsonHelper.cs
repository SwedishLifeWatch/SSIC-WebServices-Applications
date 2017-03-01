using ArtDatabanken.GIS.GeoJSON.Net;
using Newtonsoft.Json;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Json
{
    /// <summary>
    /// This static class contains helper methods for JSON data
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// Creates a JSON string.
        /// </summary>
        /// <param name="jsonModel">The json model.</param>
        /// <returns></returns>
        public static string CreateJsonString(JsonModel jsonModel)
        {
            return JsonConvert.SerializeObject(jsonModel);            
        }

        public static JsonSerializerSettings GetDefaultJsonSerializerSettings()
        {
            return new JsonSerializerSettings { ContractResolver = new CustomPropertySortContractResolver() };
        }

        //public static JsonNetResult CreateJsonResult(JsonModel jsonModel)
        //{
        //    var jsonResult = new JsonResult();
        //    jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        //    jsonResult.Data = CreateJson(jsonModel);
        //    return jsonResult;
        //}
    }
}
