// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonpResult.cs" company="Artdatabanken SLU">
//   Copyright (c) 2009 Artdatabanken SLU. All rights reserved.
// </copyright>
// <summary>
//   Defines the JsonpResult type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Dyntaxa.Helpers
{
    /// <summary>
    /// Renders result as JSON and also wraps the JSON in a call
    /// to the callback function specified in "JsonpResult.Callback".
    /// </summary>
    public class JsonpResult : JsonResult
    {
        /// <summary>
        /// Gets or sets the javascript callback function that is
        /// to be invoked in the resulting script output.
        /// </summary>
        /// <value>The callback function name.</value>
        public string Callback { get; set; }

        /// <summary>
        /// Enables processing of the result of an action method by a
        /// custom type that inherits from <see cref="T:System.Web.Mvc.ActionResult"/>.
        /// </summary>
        /// <param name="context">The context within which the
        /// result is executed.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;
            if (!String.IsNullOrEmpty(ContentType))
            {
                response.ContentType = ContentType;
            }
            else
            {
                response.ContentType = "application/javascript";
            }

            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }

            if (string.IsNullOrEmpty(Callback))
            {
                Callback = context.HttpContext.Request.QueryString["callback"];
            }

            if (Data != null)
            {
                // The JavaScriptSerializer type was marked as obsolete
                // prior to .NET Framework 3.5 SP1 
#pragma warning disable 0618
                var serializer = new JavaScriptSerializer();
                string ser = serializer.Serialize(Data);
                response.Write(Callback + "(" + ser + ");");
#pragma warning restore 0618
            }
        }
    }
}
