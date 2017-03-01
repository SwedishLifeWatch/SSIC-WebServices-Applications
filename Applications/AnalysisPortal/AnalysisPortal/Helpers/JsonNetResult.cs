using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using AnalysisPortal.Helpers.ActionFilters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AnalysisPortal.Helpers
{
    /// <summary>
    /// This class represents a JSON ActionResult which uses Json.Net for serializing.
    /// </summary>
    public class JsonNetResult : ActionResult
    {
        public Encoding ContentEncoding { get; set; }
        public string ContentType { get; set; }
        public object Data { get; set; }

        public JsonSerializerSettings SerializerSettings { get; set; }
        public Formatting Formatting { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonNetResult"/> class.
        /// </summary>
        public JsonNetResult()
        {
            SerializerSettings = new JsonSerializerSettings();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonNetResult"/> class.
        /// </summary>
        /// <param name="data">The data to be serialized as JSON.</param>
        public JsonNetResult(object data)
        {
            this.Data = data;
            SerializerSettings = new JsonSerializerSettings();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonNetResult"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="settings">The settings.</param>
        public JsonNetResult(object data, JsonSerializerSettings settings)
        {
            this.Data = data;
            SerializerSettings = settings;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonNetResult"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="contractResolver">The contract resolver.</param>
        public JsonNetResult(object data, IContractResolver contractResolver)
        {
            this.Data = data;
            SerializerSettings = new JsonSerializerSettings { ContractResolver = contractResolver };
        }

        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult"/> class.
        /// </summary>
        /// <param name="context">The context in which the result is executed. The context information includes the controller, HTTP content, request context, and route data.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;

            response.ContentType = !string.IsNullOrEmpty(ContentType)
              ? ContentType
              : "application/json";

            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }

            if (Data != null)
            {
                JsonTextWriter writer = new JsonTextWriter(response.Output) { Formatting = Formatting };

                JsonSerializer serializer = JsonSerializer.Create(SerializerSettings);
                serializer.Serialize(writer, Data);

                writer.Flush();                
                CompressFilterAttribute.AddCompression(context.HttpContext);
            }
        }
    }
}