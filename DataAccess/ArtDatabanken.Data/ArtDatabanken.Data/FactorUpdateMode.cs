using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Class that handles factor update mode.
    /// </summary>
    public class FactorUpdateMode : IFactorUpdateMode
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Definition for this factor update mode.
        /// </summary>
        public String Definition { get; set; }

        /// <summary>
        /// Id for this factor update mode.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Indicates if factor is of type header.
        /// </summary>
        public Boolean IsHeader
        {
            get
            {
                return Type == FactorUpdateModeType.Header;
            }
        }

        /// <summary>
        /// Name for this factor update mode.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Type for this factor update mode.
        /// </summary>
        public FactorUpdateModeType Type { get; set; }

        /// <summary>
        /// Manual update allowed for this factor update mode.
        /// </summary>
        public Boolean AllowManualUpdate
        {
            get
            {
                return Type == FactorUpdateModeType.ManualUpdate;
            }
        }

        /// <summary>
        /// Automatic update allowed for this factor update mode.
        /// </summary>
        public Boolean AllowAutomaticUpdate
        {
            get
            {
                return Type == FactorUpdateModeType.AutomaticUpdate;
            }
        }

        /// <summary>
        /// Update allowed for this factor update mode.
        /// </summary>
        public Boolean AllowUpdate { get; set; }
    }
}