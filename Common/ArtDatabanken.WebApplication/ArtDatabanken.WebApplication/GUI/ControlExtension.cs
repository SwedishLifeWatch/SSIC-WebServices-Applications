using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ArtDatabanken.WebApplication.GUI
{
    /// <summary>
    /// Contains extension methods to the System.Web.UI.Control class.
    /// </summary>
    public static class ControlExtension
    {
        /// <summary>
        /// Add a html attribute to a control.
        /// </summary>
        /// <param name="control">Attribute should be added to this control.</param>
        /// <param name="key">Attribute key.</param>
        /// <param name="value">Attribute value.</param>
        public static void AddAttribute(this Control control, String key, String value)
        {
            if (control is HtmlControl)
            {
                ((HtmlControl)control).Attributes.Add(key, value);
            }
            else if (control is WebControl)
            {
                ((WebControl)control).Attributes.Add(key, value);
            }
            else
            {
                // Exception is thrown if control is not of type HtmlControl or WebControl.
                throw new ArgumentException("Control should be derived from either System.Web.UI.HtmlControls.HtmlControl or System.Web.UI.WebControls.WebControl", "control");
            }
        }
    }
}
