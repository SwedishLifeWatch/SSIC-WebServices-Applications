// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActionInfo.cs" company="Artdatabanken SLU">
//   Copyright (c) 2009 Artdatabanken SLU. All rights reserved.
// </copyright>
// <summary>
//   Defines the ActionInfo type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Dyntaxa.Helpers
{
    /// <summary>
    /// This class contains information about an actions name and a controllers name for use in an url.
    /// </summary>
    public class ActionInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionInfo"/> class.
        /// </summary>
        /// <param name="controllerName">The controller name.</param>
        /// <param name="actionName">The action name.</param>
        public ActionInfo(string controllerName, string actionName)
        {
            this.ControllerName = controllerName;
            this.ActionName = actionName;
        }

        /// <summary>
        /// Gets ActionName.
        /// </summary>
        public string ActionName { get; private set; }

        /// <summary>
        /// Gets ControllerName.
        /// </summary>
        public string ControllerName { get; private set; }
    }
}