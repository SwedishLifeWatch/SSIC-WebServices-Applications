using System;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    ///  This class represents a factor update mode.
    /// </summary>
    [Serializable]
    public class FactorUpdateMode : DataSortOrder
    {
        private Boolean _allowUpdate;
        private String _name;
        private String _definition;
        private WebService.FactorUpdateModeType _type;

        /// <summary>
        /// Create a FactorUpdateMode instance.
        /// </summary>
        /// <param name="id">Id for factor update mode.</param>
        /// <param name="name">Name for faktor update mode.</param>
        /// <param name="type">Type for faktor update mode.</param>
        /// <param name="definition">Definition for faktor update mode.</param>
        /// <param name="allowUpdate">Allow updates for faktor update mode.</param>
        public FactorUpdateMode(Int32 id,
                                String name,
                                WebService.FactorUpdateModeType type,
                                String definition,
                                Boolean allowUpdate)
            : base(id, id)
        {
            _name = name;
            _type = type;
            _definition = definition;
            _allowUpdate = allowUpdate;
        }

        /// <summary>
        /// Test if automatic update is allowed.
        /// </summary>
        public Boolean AllowAutomaticUpdate
        {
            get { return _type == WebService.FactorUpdateModeType.AutomaticUpdate; }
        }

        /// <summary>
        /// Test if manual update is allowed.
        /// </summary>
        public Boolean AllowManualUpdate
        {
            get { return _type == WebService.FactorUpdateModeType.ManualUpdate; }
        }

        /// <summary>
        /// Test if update is allowed.
        /// </summary>
        public Boolean AllowUpdate
        {
            get { return _allowUpdate; }
        }

        /// <summary>
        /// Get definition for this factor update mode.
        /// </summary>
        public String Definition
        {
            get { return _definition; }
        }

        /// <summary>
        /// Get IsHeader for this factor update mode.
        /// </summary>
        public Boolean IsHeader
        {
            get { return _type == WebService.FactorUpdateModeType.Header; }
        }

        /// <summary>
        /// Get name for this factor update mode.
        /// </summary>
        public String Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Get type for this factor update mode.
        /// </summary>
        public WebService.FactorUpdateModeType Type
        {
            get { return _type; }
        }
    }
}

