using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Menu
{    
    public class MenuItem
    {
        public string Text { get; set; }                
        public string Action { get; set; }
        public string Controller { get; set; }        
        public RouteValueDictionary Parameters { get; set; }
        public bool Enabled { get; set; }
        public bool Current { get; set; }
        public List<MenuItem> ChildItems { get; set; }        

        public MenuItem()
        {
        }

        public MenuItem(string text)
        {
            Text = text;
            Enabled = true;
        }

        public MenuItem(string text, string action, string controller)
            : this(text, action, controller, true, null as RouteValueDictionary)
        {
        }

        public MenuItem(string text, string action, string controller, bool enabled)
            : this(text, action, controller, enabled, null as RouteValueDictionary)
        {
        }

        public MenuItem(string text, string action, string controller, object parameters) 
            : this(text, action, controller, new RouteValueDictionary(parameters))
        {            
        }

        public MenuItem(string text, string action, string controller, bool enabled, object parameters)
            : this(text, action, controller, enabled, new RouteValueDictionary(parameters))
        {
        }

        public MenuItem(string text, string action, string controller, params IDictionary<string, object>[] parameters)
            : this(text, action, controller, new RouteValueDictionary(parameters))
        {
        }

        public MenuItem(string text, string action, string controller, bool enabled, params IDictionary<string, object>[] parameters)
            : this(text, action, controller, enabled, new RouteValueDictionary(parameters))
        {
        }

        public MenuItem(string text, string action, string controller, RouteValueDictionary parameters)
            : this(text, action, controller, true, parameters)
        {
        }

        public MenuItem(string text, string action, string controller, bool enabled, RouteValueDictionary parameters)
        {
            Text = text;
            Action = action;
            Controller = controller;
            Parameters = parameters;
            Enabled = enabled;
            ChildItems = new List<MenuItem>();
        }
    }
}
