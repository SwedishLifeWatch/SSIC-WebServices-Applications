using System;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Json
{
    /// <summary>
    /// This classed is used to create a JSON model that can:
    /// - Signal if the Ajax request was successful or not.
    /// - Return an error message so the user know why the request wasn't successful.
    /// - Return the Data that can be used if the request was successful
    /// </summary>
    /// <remarks>
    /// The JSON data will look like this:
    /// {
    ///    "total": 100,
    ///    "success": true,
    ///    "msg": "my message",
    ///    "data": [
    ///        {
    ///            "id": 1,
    ///            "name": "Ed Spencer",
    ///            "email": "ed@sencha.com"
    ///        }
    ///    ],
    ///    "extra": null
    /// }
    /// </remarks>
    public class JsonModel
    {
        [JsonProperty(PropertyName = "total")]
        public long Total { get; set; }

        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        [JsonProperty(PropertyName = "data")]
        public object Data { get; set; }

        [JsonProperty(PropertyName = "msg")]
        public string Msg { get; set; }
        
        [JsonProperty(PropertyName = "status", NullValueHandling = NullValueHandling.Ignore)]
        public object Status { get; set; }

        /// <summary>
        /// Metadata information about the result (Data property)
        ///
        /// eg if we do a search, the result will end up in the Data property 
        /// and additional information about the search will be in the Extra property.
        /// </summary>
        [JsonProperty(PropertyName = "extra")]
        public object Extra { get; set; }

        /// <summary>
        /// Creates a JsonModel containing the specified list and a success flag.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="extra">Metadata information about the information in Data.</param>
        /// <returns></returns>
        public static JsonModel Create(IList list, object extra = null)
        {            
            var model = new JsonModel();
            model.Total = list.Count;
            model.Data = list;
            model.Extra = extra;
            model.Success = true;            
            return model;
        }

        /// <summary>
        /// Creates a JsonModel that contains the specified object and a success flag.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="extra">Metadata information about the information in Data.</param>
        /// <returns></returns>
        public static JsonModel CreateFromObject(object obj, object extra = null)
        {
            var model = new JsonModel();
            model.Data = obj;
            model.Extra = extra;
            model.Success = true;
            return model;
        }

        /// <summary>
        /// Creates a JsonModel that contains a success message and success flag
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="extra">Metadata information about the information in Data.</param>
        /// <returns></returns>
        public static JsonModel CreateSuccess(string message, object extra = null)
        {
            var model = new JsonModel();
            model.Success = true;
            model.Extra = extra;
            model.Msg = message;
            return model;
        }

        /// <summary>
        /// Creates a JsonModel that contains a failure message and failure flag
        /// </summary>
        /// <param name="message">The MSG.</param>
        /// <param name="extra">Metadata information about the information in Data.</param>
        /// <returns></returns>
        public static JsonModel CreateFailure(string message, object extra = null)
        {
            var model = new JsonModel();
            model.Success = false;
            model.Extra = extra;
            model.Msg = message;
            return model;
        }        
    }

    public class FailureJsonModel : JsonModel
    {
        [JsonProperty(PropertyName = "failurecode")]
        public int FailureCode { get; set; }
    }
}
