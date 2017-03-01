using System;
using System.Text;
using ArtDatabanken.Data;

namespace ArtDatabanken.IO
{
    /// <summary>
    /// This class contains common functionality for all
    /// IO classes that handles information about a taxon.
    /// </summary>
    public class TaxonInformationFile
    {
        /// <summary>
        /// Get file name for species information document.
        /// No extension is appended to the file name.
        /// File name is based on scientific name and taxon id.
        /// </summary>
        /// <param name="taxon">Get file name for this taxon.</param>
        /// <returns>File name for specified taxon.</returns>
        public static String GetFileName(ITaxon taxon)
        {
            String[] nameParts;
            StringBuilder scientificName, fileName;

            // Remove all strange characters from scientific name.
            scientificName = new StringBuilder();
            foreach (Char character in taxon.ScientificName)
            {
                if ((('a' <= character) && (character <= 'z')) ||
                    (('A' <= character) && (character <= 'Z')) ||
                    (' ' == character))
                {
                    scientificName.Append(character);
                }
                else
                {
                    switch (character)
                    {
                        case 'ë':
                        case 'Ë':
                            scientificName.Append('e');
                            break;
                        case 'å':
                        case 'Å':
                        case 'ä':
                        case 'Ä':
                            scientificName.Append('a');
                            break;
                        case 'ö':
                        case 'Ö':
                            scientificName.Append('o');
                            break;
                        default:
                            scientificName.Append(' ');
                            break;
                    }
                }
            }

            // Split scientific name into it parts.
            nameParts = scientificName.ToString().Split(' ');
            fileName = new StringBuilder();
            foreach (String namePart in nameParts)
            {
                if (namePart.IsNotEmpty())
                {
                    // Add scientific name parts to the url.
                    // First letter is in upper case
                    // and the rest are in lower case.
                    if (fileName.ToString().IsNotEmpty())
                    {
                        fileName.Append("_");
                    }

                    fileName.Append(namePart.Substring(0, 1).ToUpper());
                    fileName.Append(namePart.Substring(1).ToLower());
                }
            }
            fileName.Append("_");
            fileName.Append(taxon.Id);
            return fileName.ToString();
        }
    }
}
