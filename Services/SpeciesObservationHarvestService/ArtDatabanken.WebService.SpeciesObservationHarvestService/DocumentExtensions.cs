using System.Xml;
using System.Xml.Linq;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService
{
    /// <summary>
    /// Document extensions used in harvest.
    /// </summary>
    public static class DocumentExtensions
    {
        /// <summary>
        /// Method for converting XmlDocument to XDocument.
        /// </summary>
        /// <param name="xmlDocument">
        /// The xml document.
        /// </param>
        /// <returns>
        /// The x (xDocument) xml document.<see cref="XDocument"/>.
        /// </returns>
        public static XDocument ToXDocument(this XmlDocument xmlDocument)
        {
            using (var nodeReader = new XmlNodeReader(xmlDocument))
            {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader);
            }
        }
    }
}
