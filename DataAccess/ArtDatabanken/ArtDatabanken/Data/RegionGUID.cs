using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// GUID that uniquely identifies a region.
    /// </summary>
    public class RegionGUID : LSID
    {
        /// <summary>
        /// Create a RegionGUID instance.
        /// </summary>
        /// <param name="GUID">GUID.</param>
        public RegionGUID(String GUID)
            : base(GUID)
        {
            Int32 categoryId;
            String categoryIdString;

            // Check arguments.
            if (ObjectID.Length < (Settings.Default.RegionCategoryId_LSID.Length +
                                   Settings.Default.RegionNativeId_LSID.Length +
                                   2))
            {
                throw new ArgumentException("Wrong format in region GUID : " + GUID);
            }
            if (ObjectID.Substring(0, Settings.Default.RegionCategoryId_LSID.Length).ToUpper() !=
                Settings.Default.RegionCategoryId_LSID.ToUpper())
            {
                throw new ArgumentException("Wrong format in region GUID : " + GUID);
            }
            categoryIdString = GetRegionCategoryId();
            if (categoryIdString.IsEmpty() ||
                !Int32.TryParse(categoryIdString, out categoryId))
            {
                throw new ArgumentException("Wrong format in region GUID : " + GUID);
            }
            if (NativeId.IsEmpty())
            {
                throw new ArgumentException("Wrong format in region GUID : " + GUID);
            }
        }

        /// <summary>
        /// Create a RegionGUID instance.
        /// </summary>
        /// <param name="categoryId">Region category id.</param>
        /// <param name="nativeId">Native id.</param>
        public RegionGUID(Int32 categoryId, String nativeId)
            : base(RegionAuthority,
                   Settings.Default.RegionNameSpace_LSID,
                   Settings.Default.RegionCategoryId_LSID + categoryId.ToString() +
                   Settings.Default.RegionNativeId_LSID + nativeId.ToString())
        {
            // Check arguments.
            nativeId.CheckNotEmpty("nativeId");
        }

        /// <summary>
        /// Region category id.
        /// </summary>
        public Int32 CategoryId
        {
            get
            {
                return Int32.Parse(GetRegionCategoryId());
            }
        }

        /// <summary>
        /// Native id.
        /// </summary>
        public String NativeId
        {
            get
            {
                return ObjectID.Substring(ObjectID.IndexOf(Settings.Default.RegionNativeId_LSID) +
                                          Settings.Default.RegionNativeId_LSID.Length);
            }
        }

        /// <summary>
        /// Get LSID authority that is used for regions.
        /// </summary>
        private static String RegionAuthority
        {
            get
            {
                return Settings.Default.SwedenSpeciesGatewayAuthority_LSID;
            }
        }

        /// <summary>
        /// Get region category id as string.
        /// </summary>
        /// <returns>Region category id as string.</returns>       
        private String GetRegionCategoryId()
        {
            return ObjectID.Substring(Settings.Default.RegionCategoryId_LSID.Length,
                                      ObjectID.IndexOf(Settings.Default.RegionNativeId_LSID) -
                                      Settings.Default.RegionCategoryId_LSID.Length);
        }
    }
}
