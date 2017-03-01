// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlResult.cs" company="Artdatabanken SLU">
//   Copyright (c) 2009 Artdatabanken SLU. All rights reserved.
// </copyright>
// <summary>
//   Defines the XmlResult type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Web.Mvc;

namespace Dyntaxa.Helpers
{
    /// <summary>
    /// Return data as XML.
    /// </summary>
    public class XmlResult : ActionResult
    {
        /// <summary>
        /// Gets or sets Data.
        /// </summary>
        public object Data
        {
            get;
            set;
        }

        /// <summary> 
        /// Serialises the object that was passed into the constructor to XML and writes the corresponding XML to the result stream. 
        /// </summary> 
        /// <param name="context">The controller context for the current request.</param> 
        public override void ExecuteResult(ControllerContext context)
        {
            if (this.Data != null)
            {
                context.HttpContext.Response.Clear();
                var xs = new System.Xml.Serialization.XmlSerializer(this.Data.GetType());
                context.HttpContext.Response.ContentType = "text/xml";
                xs.Serialize(context.HttpContext.Response.Output, this.Data);
            }
        }
    }
}