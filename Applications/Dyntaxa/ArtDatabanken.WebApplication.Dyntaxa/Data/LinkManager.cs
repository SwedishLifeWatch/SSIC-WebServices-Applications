using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    /// <summary>
    /// A manager class which generates Url to various sources of taxon information.
    /// </summary>
    public class LinkManager
    {
        private bool _addLinks = false;

        public LinkManager()
        {
        }

        public LinkManager(bool addLinks)
        {
            this._addLinks = addLinks;
        }

        /// <summary>
        /// Method that generates the url to the Redlist information provided by ArtDatabanken.
        /// </summary>
        /// <param name="taxonId">Id of the taxon.</param>
        /// <returns>A string representing an Url.</returns>
        public string GetUrlToRedlist(string taxonId)
        {
            string url = "";

            if (taxonId != "")
            {
                url = Resources.DyntaxaSettings.Default.UrlToGetArtDatabankenRedlistInformation.Replace("[TaxonId]", taxonId);

                if (this._addLinks)
                {
                    url = Resources.DyntaxaSettings.Default.UrlToUBioLinkitService.Replace("[Url]", url);
                }
            }
            return url;
        }

        /// <summary>
        /// Method that generates the url to the photo gallery of Artportalen.
        /// </summary>
        /// <param name="taxonId">Id of the taxon.</param>
        /// <returns>A string representing an Url.</returns>
        public string GetUrlToMediaAP(string taxonId)
        {
            string url = "";

            if (taxonId != "")
            {
                url = Resources.DyntaxaSettings.Default.UrlToGetSpeciesGatewayPhotos.Replace("[TaxonId]", taxonId);

                if (this._addLinks)
                {
                    url = Resources.DyntaxaSettings.Default.UrlToUBioLinkitService.Replace("[Url]", url);
                }
            }
            return url;
        }

        /// <summary>
        /// Method that generates the url to taxon information provided by Wikipedia.
        /// </summary>
        /// <param name="taxonName">Recommended scientific name.</param>
        /// <returns>A string representing an Url.</returns>
        public string GetUrlToWikipedia(string taxonName)
        {
            string url = "";

            if (taxonName != "")
            {
                url = Resources.DyntaxaResource.UrlToGetWikipediaTaxonInformation.Replace("[TaxonName]", taxonName);

                if (this._addLinks)
                {
                    url = Resources.DyntaxaResource.UrlToGetWikipediaTaxonInformation.Replace("[Url]", url);
                }
            }
            return url;
        }

        /// <summary>
        /// Method that generates the url to taxon information provided by Naturforskaren.
        /// </summary>
        /// <param name="taxonName">Recommended scientific name.</param>
        /// <returns>A string representing an Url.</returns>
        public string GetUrlToNaturforskaren(string taxonName)
        {
            string url = "";

            if (taxonName != "")
            {
                url = Resources.DyntaxaResource.NaturforskarenLink.Replace("[TaxonName]", taxonName);

                if (this._addLinks)
                {
                    url = Resources.DyntaxaResource.NaturforskarenLink.Replace("[Url]", url);
                }
            }
            return url;
        }

        /// <summary>
        /// Method that generates the url to Biodiversity Heritage Library.
        /// </summary>
        /// <param name="taxonName">Recommended scientific name.</param>
        /// <returns>A string representing an Url.</returns>
        public string GetUrlToBiodiversityHeritageLibrary(string taxonName)
        {
            string url = "";

            if (taxonName != "")
            {
                url = Resources.DyntaxaSettings.Default.UrlToBiodiversityHeritageLibrary.Replace("[TaxonName]", taxonName); 

                if (this._addLinks)
                {
                    url = Resources.DyntaxaResource.UrlToGetWikipediaTaxonInformation.Replace("[Url]", url);
                }
            }
            return url;
        }

        /// <summary>
        /// Method that generates the url to taxon information provided by Encyclopedia of life (EOL).
        /// </summary>
        /// <param name="taxonName">Recommended scientific name.</param>
        /// <returns>A string representing an Url.</returns>
        public string GetUrlToEoL(string taxonName)
        {
            string url = "";

            if (taxonName != "")
            {
                url = Resources.DyntaxaSettings.Default.UrlToGetEncyclopediaOfLifeTaxonInformation.Replace("[TaxonName]", taxonName);

                if (this._addLinks)
                {
                    url = Resources.DyntaxaSettings.Default.UrlToUBioLinkitService.Replace("[Url]", url);
                }
            }
            return url;
        }

        /// <summary>
        /// Method that generates the url to Google search results.
        /// </summary>
        /// <param name="taxonName">Any taxon name or identifier.</param>
        /// <returns>A string representing an Url.</returns>
        public string GetUrlToGoogleSearchResults(string taxonName)
        {
            string url = "";
            if (taxonName != "")
            {
                url = Resources.DyntaxaSettings.Default.UrlToGetGoogleSearchResults.Replace("[TaxonName]", taxonName);
            }
            return url;
        }

        /// <summary>
        /// Method that generates the url to PESI taxon information.
        /// </summary>
        /// <param name="GUID">GUID according to PESI</param>
        /// <returns>A string representing an Url.</returns>
        public string GetUrlToPESITaxonInformation(string GUID)
        {
            string url = "";
            if (GUID != "")
            {
                url = Resources.DyntaxaSettings.Default.UrlToGetPESITaxonInformation.Replace("[GUID]", GUID);
            }
            return url;
        }

        /// <summary>
        /// Method that generates the url to MarBEF (ERMS) taxon information.
        /// </summary>
        /// <param name="GUID">GUID</param>
        /// <returns>A string representing an Url.</returns>
        public string GetUrlToWormsTaxonInformation(string GUID)
        {
            string url = "";
            if (GUID != "")
            {
                try
                {
                    LSID lsid = new LSID(GUID);
                    if (lsid.Authority == "marinespecies.org")
                    {
                        url = Resources.DyntaxaSettings.Default.UrlToWormsTaxonInformation.Replace("[Id]", lsid.ObjectID);
                    }
                }
                catch (Exception) { }
            }
            return url;
        }

        /// <summary>
        /// Method that generates the url to Fauna Europea taxon information.
        /// </summary>
        /// <param name="GUID">GUID</param>
        /// <returns>A string representing an Url.</returns>
        public string GetUrlToFaunaEuropeaTaxonInformation(string GUID)
        {
            string url = "";
            if (GUID != "")
            {
                try
                {
                    LSID lsid = new LSID(GUID);
                    if (lsid.Authority == "faunaeur.org")
                    {
                        url = Resources.DyntaxaSettings.Default.UrlToGetFaunaEuropeaTaxonInformation.Replace("[Id]", lsid.ObjectID);
                    }
                }
                catch (Exception) { }
            }

            return url;
        }

        ///// <summary>
        ///// Method that generates the url to Algaebase taxon information.
        ///// </summary>
        ///// <param name="GUID">GUID</param>
        ///// <returns>A string representing an Url.</returns>
        //public string GetUrlToAlgaebaseTaxonInformation(string GUID)
        //{
        //    string url = "";
        //    if (GUID != "")
        //    {
        //        try
        //        {
        //            LSID lsid = new LSID(GUID);
        //            if (lsid.Authority == "algaebase.org")
        //            {
        //                url = Resources.DyntaxaSettings.Default.UrlToAlgaebaseTaxonInformation.Replace("[Id]", lsid.ObjectID);
        //            }
        //        }
        //        catch (Exception) { }
        //    }
        //    return url;
        //}

        /// <summary>
        /// Method that generates the url to Nordic Microalgae (SMHI) Taxon information.
        /// </summary>
        /// <param name="taxonName">Recommended scientific name</param>
        /// <returns>A string representing an Url.</returns>
        public string GetUrlToNordicMicroalgaeTaxonInformation(string taxonName)
        {
            string url = "";
            if (taxonName != "")
            {
                url = Resources.DyntaxaSettings.Default.UrlToNordicMicroalgaeTaxonInformation.Replace("[TaxonName]", taxonName);
            }
            return url;
        }

        /// <summary>
        /// Method that generates the url to the taxon information page provided by GBIF.
        /// </summary>
        /// <param name="taxonName">Recommended scientific name.</param>
        /// <returns>A string representing an Url.</returns>
        public string GetUrlToGBIF(string taxonName)
        {
            string url = "";
            string objText = "";
            if (taxonName != null)
            {
                string id = "";
                if (taxonName != "")
                {
                    try
                    {
                        url = Resources.DyntaxaSettings.Default.UrlToGetGBIFTaxonIdByName.Replace("[TaxonName]", taxonName);
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                        using (var twitpicResponse = (HttpWebResponse)request.GetResponse())
                        {
                            using (var reader = new StreamReader(twitpicResponse.GetResponseStream()))
                            {
                                //System.Web.Serialization.JavaScriptSerializer js = new System.Web.Serialization.JavaScriptSerializer();
                                objText = reader.ReadToEnd();
                                if (objText.IndexOf("usageKey") > 0)
                                {
                                    //"{\"usageKey\":1713389,\"scientificName\":\"Psophus"
                                    int fromIndex = objText.IndexOf(":");
                                    int toIndex = objText.IndexOf(",");
                                    objText = objText.Substring(fromIndex + 1, toIndex - fromIndex - 1);
                                }
                            }
                        }
                    }
                    catch { }
                }
                int i = -1;
                if (Int32.TryParse(objText, out i))
                {
                    id = objText;
                }

                if (id != "")
                {
                    url = Resources.DyntaxaSettings.Default.UrlToGetGBIFTaxonInformation.Replace("[TaxonId]", id);
                }
                else
                {
                    url = "";
                }
            }
            return url;
        }

        /// <summary>
        /// Method that generates the url to Nobanis info.
        /// </summary>
        /// <param name="taxonName">Any taxon name or identifier.</param>
        /// <returns>A string representing an Url.</returns>
        public string GetUrlToNobanis(string taxonName)
        {
            string url = "";
            if (taxonName != "")
            {
                //Sökning går bara att göra på taxon på art-nivå. Acceptera 0 träffar eller länka till söksida på annat sätt istället???
                url = Resources.DyntaxaSettings.Default.UrlToGetNobanisInformation.Replace("[TaxonName]", taxonName);
            }
            return url;
        }

        /// <summary>
        /// Method that generates the url to Artfakta info.
        /// </summary>
        /// <param name="taxonId">Taxon identifier.</param>
        /// <returns>A string representing an Url.</returns>
        public string GetUrlToArtfakta(string taxonId)
        {
            string url = "";
            if (taxonId != "")
            {
                url = Resources.DyntaxaSettings.Default.UrlToGetArtfaktaInformation.Replace("[TaxonId]", taxonId);
            }
            return url;
        }

        /// <summary>
        /// Method that generates the url to Skud info.
        /// </summary>
        /// <returns>A string representing an Url.</returns>
        public string GetUrlToSkud()
        {
            string url = "";
            url = Resources.DyntaxaSettings.Default.UrlToGetSkudInformation;

            return url;
        }
    }
}
