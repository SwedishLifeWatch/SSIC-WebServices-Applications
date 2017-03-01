using System;
using System.Collections.Generic;

// ReSharper disable CheckNamespace
namespace ArtDatabanken.WebApplication.Dyntaxa.Data
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Holds information to be needed for displaying a list of users.
    /// </summary>
    public class RevisionUserItemModelHelper
    {
        /// <summary>
        /// User id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User unique name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// User person name
        /// </summary>
        public string PersonName { get; set; }
    }
}
