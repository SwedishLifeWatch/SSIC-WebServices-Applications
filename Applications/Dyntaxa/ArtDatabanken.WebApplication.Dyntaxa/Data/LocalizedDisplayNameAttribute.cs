using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using ArtDatabanken.Data;
using ArtDatabanken;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    public class LocalizedDisplayNameAttribute : DisplayNameAttribute
    {
        private PropertyInfo _nameProperty;
        private Type _resourceType;

        public LocalizedDisplayNameAttribute(string displayNameKey)
            : base(displayNameKey)
        {
        }

        public Type NameResourceType
        {
            get
            {
                return _resourceType;
            }
            set
            {
                _resourceType = value;
                //initialize nameProperty when type property is provided by setter
                _nameProperty = _resourceType.GetProperty(
                    base.DisplayName, BindingFlags.Static | BindingFlags.Public);
            }
        }

        public override string DisplayName
        {
            get
            {
                //check if nameProperty is null and return original display name value
                if (_nameProperty == null)
                {
                    return base.DisplayName;
                }

                return (string)_nameProperty.GetValue(_nameProperty.DeclaringType, null);
            }
        }
    }
}
