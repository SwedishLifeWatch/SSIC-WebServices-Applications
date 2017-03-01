using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data
{
    public enum PictureLifeCycleStatusIdentifier
    {
        /// <summary>
        /// Identifier for picture life cycle status type Preliminary.
        /// </summary>
        Preliminary,

        /// <summary>
        /// Identifier for picture life cycle status type Created.
        /// </summary>
        Created,

        /// <summary>
        /// Identifier for picture life cycle status type NeedsConfirmation.
        /// </summary>
        NeedsConfirmation,

        /// <summary>
        /// Identifier for picture life cycle status type ValidatorContacted.
        /// </summary>
        ValidatorContacted,

        /// <summary>
        /// Identifier for picture life cycle status type NeedsEditing.
        /// </summary>
        NeedsEditing,

        /// <summary>
        /// Identifier for picture life cycle status type Publishable.
        /// </summary>
        Publishable,

        /// <summary>
        /// Identifier for picture life cycle status type Discarded.
        /// </summary>
        Discarded
    }
}
