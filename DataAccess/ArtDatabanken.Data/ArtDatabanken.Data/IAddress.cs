using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface handles information about an address.
    /// </summary>
    public interface IAddress : IDataId32
    {
        /// <summary>
        /// City. Can be empty, but should generraly be considered as mandatory.
        /// </summary>
        String City
        { get; set; }

        /// <summary>
        /// Country. Mandatory.
        /// </summary>
        ICountry Country
        { get; set; }

        /// <summary>
        /// Data context.
        /// </summary>
        IDataContext DataContext
        { get; set; }

        /// <summary>
        /// First line in the postal address. Can be empty, but should generraly be considered as mandatory.
        /// </summary>
        String PostalAddress1
        { get; set; }

        /// <summary>
        /// Second line in the postal address. Optinal.
        /// </summary>
        String PostalAddress2
        { get; set; }

        /// <summary>
        /// Type of address. Not NULL.
        /// </summary>
        IAddressType Type
        { get; set; }

        /// <summary>
        /// Zip code. Can be empty, but should generraly be considered as mandatory.
        /// </summary>
        String ZipCode
        { get; set; }
    }
}
