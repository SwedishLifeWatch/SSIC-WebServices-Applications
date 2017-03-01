using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Life Science Identifiers (LSID) are a way to name and locate
    /// pieces of information on the web. Essentially, an LSID is a
    /// unique identifier for some data, and the LSID protocol specifies
    /// a standard way to locate the data (as well as a standard way of
    /// describing that data).
    /// An LSID is represented as a Uniform Resource Name (URN) with
    /// the following format.
    /// URN:LSID:Authority:Namespace:ObjectID[:Version]
    /// </summary>
    public class LSID
    {
        /// <summary>
        /// Create a LSID instance.
        /// </summary>
        /// <param name="GUID">GUID.</param>
        public LSID(String GUID)
        {
            String[] split;

            // Check argument.
            GUID.CheckNotEmpty("GUID");
            split = GUID.Split(new Char[] { Settings.Default.GUID_Delimiter });
            if ((split.Length < 5) || (split.Length > 6))
            {
                throw new ArgumentException("Wrong format in LSID : " + GUID);
            }
            if (split[0].ToUpper() != Settings.Default.UniformResourceName.ToUpper())
            {
                throw new ArgumentException("Wrong format in LSID : " + GUID);
            }
            if (split[1].ToUpper() != Settings.Default.NameSpace_LSID.ToUpper())
            {
                throw new ArgumentException("Wrong format in LSID : " + GUID);
            }
            if (split[2].IsEmpty() ||
                split[3].IsEmpty() ||
                split[4].IsEmpty())
            {
                throw new ArgumentException("Wrong format in LSID : " + GUID);
            }

            // Set properties.
            this.GUID = GUID;
            Authority = split[2];
            NameSpace = split[3];
            ObjectID = split[4];
            if ((split.Length == 6) && split[5].IsNotEmpty())
            {
                Version = split[5];
            }
            else
            {
                Version = null;
            }
        }

        /// <summary>
        /// Create a LSID instance.
        /// </summary>
        /// <param name="authority">Authority.</param>
        /// <param name="nameSpace">Name space.</param>
        /// <param name="objectID">Object ID.</param>
        public LSID(String authority,
                    String nameSpace,
                    String objectID)
            : this(authority, nameSpace, objectID, null)
        {
        }

        /// <summary>
        /// Create a LSID instance.
        /// </summary>
        /// <param name="authority">Authority.</param>
        /// <param name="nameSpace">Name space.</param>
        /// <param name="objectID">Object ID.</param>
        /// <param name="version">Version.</param>
        public LSID(String authority,
                    String nameSpace,
                    String objectID,
                    String version)
        {
            // Check arguments.
            authority.CheckNotEmpty("authority");
            nameSpace.CheckNotEmpty("nameSpace");
            objectID.CheckNotEmpty("objectID");

            Authority = authority;
            NameSpace = nameSpace;
            ObjectID = objectID;
            Version = version;
            GUID = Settings.Default.UniformResourceName + Settings.Default.GUID_Delimiter +
                   Settings.Default.NameSpace_LSID + Settings.Default.GUID_Delimiter +
                   Authority + Settings.Default.GUID_Delimiter +
                   NameSpace + Settings.Default.GUID_Delimiter +
                   ObjectID;
            if (Version.IsNotEmpty())
            {
                GUID += Settings.Default.GUID_Delimiter + Version;
            }
        }

        /// <summary>
        /// Authority.
        /// </summary>
        public String Authority
        { get; private set; }

        /// <summary>
        /// GUID.
        /// </summary>
        public String GUID
        { get; private set; }

        /// <summary>
        /// Name space.
        /// </summary>
        public String NameSpace
        { get; private set; }

        /// <summary>
        /// Object ID.
        /// </summary>
        public String ObjectID
        { get; private set; }

        /// <summary>
        /// Version.
        /// </summary>
        public String Version
        { get; private set; }
    }
}
