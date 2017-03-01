using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Navigation
{
    /// <summary>
    /// Class for storing information about navigation. 
    /// Used to know to which view a user will see when 
    /// navigating with the help of the taxon tree.
    /// </summary>
    public class NavigateData
    {
        public string Controller { get; set; }
        public string Action { get; set; }                

        public NavigateData(string controller, string action)
        {
            Controller = controller;
            Action = action;            
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != typeof(NavigateData))
            {
                return false;
            }

            return Equals((NavigateData)obj);
        }

        public bool Equals(NavigateData other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(other.Controller.ToLower(), Controller.ToLower()) && Equals(other.Action.ToLower(), Action.ToLower());
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Controller != null ? Controller.GetHashCode() : 0) * 397) ^ (Action != null ? Action.GetHashCode() : 0);
            }
        }

        public static bool operator ==(NavigateData left, NavigateData right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(NavigateData left, NavigateData right)
        {
            return !Equals(left, right);
        }
    }
}
