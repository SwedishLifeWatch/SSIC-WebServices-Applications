using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.DataVisualization.Charting;
using ArtDatabanken;
using ArtDatabanken.IO;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Shared;
using Dyntaxa.Helpers;

namespace Dyntaxa.Controllers
{
    /// <summary>
    /// An API Controller.
    /// It contains a number of methods for retrieving taxon information. 
    /// </summary>
    public class ApiController : DyntaxaBaseController
    {
        /// <summary>
        /// Returns taxon information summary as json.
        /// </summary>
        /// <param name="taxonId">Id of requested taxon.</param>
        /// <returns></returns>
        [JsonpFilter]
        public JsonResult TaxonInformationSummary(int taxonId)
        {
            var model = TaxonSummaryViewModel.Create(GetCurrentUser(), taxonId, this.RevisionId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Returns the recommended common name as json.
        /// </summary>
        /// <param name="taxonId">Id of requested taxon.</param>
        /// <returns></returns>
        [JsonpFilter]
        public JsonResult CommonName(int taxonId)
        {
            var model = TaxonSummaryViewModel.Create(GetCurrentUser(), taxonId, this.RevisionId);
            return Json(model.CommonName, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Returns the list of parent taxa as json.
        /// </summary>
        /// <param name="taxonId">Id of requested taxon.</param>
        /// <returns></returns>
        [JsonpFilter]
        public JsonResult ParentTaxa(int taxonId)
        {
            var model = TaxonSummaryViewModel.Create(GetCurrentUser(), taxonId, this.RevisionId);
            return Json(model.Classification, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Returns the recommended scientific name as json.
        /// </summary>
        /// <param name="taxonId">Id of requested taxon.</param>
        /// <returns>A name object.</returns>
        [JsonpFilter]
        public JsonResult ScientificName(int taxonId)
        {
            var model = TaxonSummaryViewModel.Create(GetCurrentUser(), taxonId, this.RevisionId);
            return Json(model.ScientificName, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Returns the list of synonyms as json.
        /// </summary>
        /// <param name="taxonId">Id of requested taxon.</param>
        /// <returns>A list of synonyms.</returns>
        [JsonpFilter]
        public JsonResult Synonyms(int taxonId)
        {
            var model = TaxonSummaryViewModel.Create(GetCurrentUser(), taxonId, this.RevisionId);
            return Json(model.Synonyms, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Returns the taxon id as json.
        /// </summary>
        /// <param name="taxonId">Any identifier associated with the taxon. For example scientific name or GUID.</param>
        /// <returns>The taxon id.</returns>
        [JsonpFilter]
        public JsonResult TaxonId(string taxonId)
        {
            TaxonSearchResult searchResult = this.TaxonSearchManager.GetTaxon(taxonId);
            if (searchResult.NumberOfMatches != 1)
            {
                return Json("No or ambigous result", JsonRequestBehavior.AllowGet);
            }

            ITaxon taxon = searchResult.Taxon;
            this.TaxonIdentifier = TaxonIdTuple.Create(taxonId, searchResult.Taxon.Id);

            return Json(this.TaxonIdentifier.Id, JsonRequestBehavior.AllowGet);
        }
    }
}
