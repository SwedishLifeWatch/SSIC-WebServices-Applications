//// --------------------------------------------------------------------------------------------------------------------
//// <copyright file="MapController.cs" company="Artdatabanken SLU">
////   Copyright (c) 2009 Artdatabanken SLU. All rights reserved.
//// </copyright>
//// <summary>
////   The map controller.
//// </summary>
//// --------------------------------------------------------------------------------------------------------------------

//namespace Artportalen.UserInterface.Controllers
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Configuration;
//    using System.Diagnostics.CodeAnalysis;
//    using System.Linq;
//    using System.Web.Mvc;

//    using Artportalen.ApplicationServices;
//    using Artportalen.ApplicationServices.Authorization;
//    using Artportalen.ApplicationServices.GIS;
//    using Artportalen.ApplicationServices.Site;
//    using Artportalen.ObjectModel.Constants;
//    using Artportalen.ObjectModel.Domain;
//    using Artportalen.ObjectServices.Repositories;
//    using Artportalen.UserInterface.Helpers.Extensions;
//    using Artportalen.UserInterface.Helpers.SiteManipulation;

//    using Helpers.Filters;

//    /// <summary>
//    /// The map controller.
//    /// </summary>
//    public class MapController : BaseController
//    {
//        #region Constants and Fields

//        /// <summary>
//        /// The Area repository
//        /// </summary>
//        private readonly Func<IAreaRepository> areaRepository;

//        /// <summary>
//        /// The organization repository.
//        /// </summary>
//        private readonly Func<IOrganizationRepository> organizationRepository;

//        /// <summary>
//        /// The site service.
//        /// </summary>
//        private readonly Func<ISiteService> siteService;

//        /// <summary>
//        /// The Gis Services
//        /// </summary>
//        private readonly Func<IGisServices> gisService;

//        /// <summary>
//        /// The Map repository
//        /// </summary>
//        private readonly Func<IMapRepository> mapRepository;

//        /// <summary>
//        /// The site repository.
//        /// </summary>
//        private readonly Func<ISiteRepository> siteRepository;

//        /// <summary>
//        /// The authorization service delegate
//        /// </summary>
//        private readonly Func<IAuthorizationService> authorizationServiceDelegate;

//        /// <summary>
//        /// The authorization service
//        /// </summary>
//        private IAuthorizationService authorizationService;

//        #endregion

//        #region Constructors and Destructors

//        /// <summary>
//        /// Initializes a new instance of the <see cref="MapController"/> class.
//        /// </summary>
//        /// <param name="baseServiceContext">
//        /// The base Service Context.
//        /// </param>
//        /// <param name="siteRepository">
//        /// The site Repository.
//        /// </param>
//        /// <param name="organizationRepository">
//        /// The organization Repository.
//        /// </param>
//        /// <param name="siteService">
//        /// The site Service.
//        /// </param>
//        /// <param name="gisService">
//        /// the GisService
//        /// </param>
//        /// <param name="areaRepository">
//        /// The Area repository
//        /// </param>
//        /// <param name="authorizationService">
//        /// The AuthorizationService
//        /// </param>
//        /// <param name="mapRepository">
//        /// The mapRepository
//        /// </param>
//        public MapController(
//            Func<IBaseServiceContext> baseServiceContext,
//            Func<ISiteRepository> siteRepository,
//            Func<IOrganizationRepository> organizationRepository,
//            Func<ISiteService> siteService,
//            Func<IGisServices> gisService,
//            Func<IAreaRepository> areaRepository,
//            Func<IAuthorizationService> authorizationService,
//            Func<IMapRepository> mapRepository)
//            : base(baseServiceContext)
//        {
//            this.siteRepository = siteRepository;
//            this.organizationRepository = organizationRepository;
//            this.areaRepository = areaRepository;
//            this.siteService = siteService;
//            this.gisService = gisService;
//            this.authorizationServiceDelegate = authorizationService;
//            this.mapRepository = mapRepository;
//        }

//        #endregion

//        #region Public Methods

//        /// <summary>
//        /// Gets AuthorizationService.
//        /// </summary>
//        private IAuthorizationService AuthorizationService
//        {
//            get
//            {
//                return this.authorizationService ?? (this.authorizationService = this.authorizationServiceDelegate());
//            }
//        }

//        /// <summary>
//        /// The AddSiteInfo.
//        /// </summary>
//        /// <param name="site">
//        /// The site for the sighting.
//        /// </param>
//        /// <param name="geometry">
//        /// The geometry for the site.
//        /// </param>
//        /// <param name="parentId">
//        /// The parent id for the site
//        /// </param>
//        /// <param name="comment">
//        /// The comment for the site
//        /// </param>
//        /// <param name="coordSys">
//        /// The coord Sys.
//        /// </param>
//        /// <returns>
//        /// String with error message if any
//        /// </returns>
//        public ActionResult AddSiteInfo(Site site, string geometry, string parentId, string comment, int? coordSys)
//        {
//            try
//            {
//                int pId;
//                int.TryParse(parentId, out pId);
//                if (pId > 0)
//                {
//                    site.Parent = this.siteRepository().GetSiteById(pId);
//                }

//                site.Comment = comment;
//                site.IsPrivate = 1;
//                site.User = this.CurrentUser;

//                if (site.Id == 0)
//                {
//                    site.Id = -1;
//                }

//                site.InputString = geometry.Contains("POLYGON") ? "map;polygon" : "map;point";

//                // must have areas also
//                site.Areas = this.siteRepository().GetSiteAreasForPoint(site.XCoord, site.YCoord);
//                if (this.CurrentUser.Id == 0)
//                {
//                    // not logget in. Not allowed to save a site
//                    var errorResult = new { success = false, message = "Map_Not_Logged_In".Localize().ToString() };
//                    return this.Json(errorResult);
//                }
//                else if (this.AuthorizationService.IsAuthorizedPublicSiteAdmin())
//                {
//                    var adminfeatures = this.AuthorizationService.GetFeatureIdsForPublicSiteAdmin();
//                    var areas = site.Areas;
//                    bool autorizedToMove = false;
//                    foreach (var area in areas)
//                    {
//                        foreach (var featureId in adminfeatures)
//                        {
//                            if (area.FeatureId == featureId)
//                            {
//                                autorizedToMove = true;
//                                break;
//                            }
//                        }

//                        if (autorizedToMove)
//                        {
//                            break;
//                        }
//                    }

//                    if (!autorizedToMove)
//                    {
//                        var errorResult =
//                            new
//                            {
//                                error = true,
//                                success = false,
//                                message = "Map_Permission_Denied_Outside_Area".Localize().ToString()
//                            };
//                        return this.Json(errorResult);
//                    }

//                    site.IsPrivate = 0;
//                }
//                else if (this.AuthorizationService.IsAuthorizedOrganizationSiteAdmin())
//                {
//                    var organizationId = this.AuthorizationService.GetAuthorizedOrganizationIdForOrganizationSiteAdmin();
//                    if (!this.siteRepository().IsPointWithinAllowedArea(site.XCoord, site.YCoord))
//                    {
//                        var errorResult = new { success = false, message = "Map_Wrong_Country".Localize().ToString() };
//                        return this.Json(errorResult);
//                    }

//                    if (organizationId.HasValue)
//                    {
//                        Organization org = this.organizationRepository().GetByOrganizationId(organizationId.Value);
//                        site.ControlingOrganisation = org;
//                    }
//                }
//                else
//                {
//                    if (!this.siteRepository().IsPointWithinAllowedArea(site.XCoord, site.YCoord))
//                    {
//                        var errorResult = new { success = false, message = "Map_Wrong_Country".Localize().ToString() };
//                        return this.Json(errorResult);
//                    }

//                    site.ControlingUser = this.CurrentUser;
//                }
//            }
//            catch (Exception ex)
//            {
//                var errorResult = new { success = false, message = "Site not saved:" + ex.Message };

//                return this.Json(errorResult);
//            }

//            SiteGeometry siteGeometry = null;
//            if (geometry.Contains("POLYGON"))
//            {
//                try
//                {
//                    siteGeometry = new SiteGeometry(geometry)
//                    {
//                        User = this.CurrentUser,
//                        Site = site,
//                        ChangedByUser = this.CurrentUser,
//                        Areas = this.siteRepository().GetSiteAreasForGeometry(geometry)
//                    };

//                    // must have areas also
//                }
//                catch (Exception ex)
//                {
//                    var errorResult = new { success = false, message = "Site not saved:" + ex.Message };

//                    return this.Json(errorResult);
//                }
//            }

//            IList<Site> sites = new List<Site> { site };
//            IList<SiteGeometry> siteGeometries = new List<SiteGeometry>();
//            if (siteGeometry != null)
//            {
//                siteGeometries.Add(siteGeometry);
//            }

//            return this.GetGeoJsonSites(sites, siteGeometries, coordSys);
//        }

//        /// <summary>
//        /// Check if site is within the allowed area.
//        /// </summary>
//        /// <param name="siteCoordX">
//        /// The x-coordinate
//        /// </param>
//        /// <param name="siteCoordY">
//        /// The y-coordinate
//        /// </param>
//        /// <returns>
//        /// The result of the check.
//        /// </returns>
//        public JsonResult IsSiteWithinAllowedArea(int siteCoordX, int siteCoordY)
//        {
//            if (this.siteRepository().IsPointWithinAllowedArea(siteCoordX, siteCoordY))
//            {
//                return Json(new { status = "success", statusText = string.Empty });
//            }

//            return Json(new { status = "failure", statusText = "Site_NotificationMessage_NewSite_Failure_WrongCountry".Localize().ToString() });
//        }

//        /// <summary>
//        /// Entry point for getting users from a selector control.
//        /// </summary>
//        /// <param name="term">
//        /// The search term.
//        /// </param>
//        /// <returns>
//        /// Returns a JSON data object representation of a list of User model objects.
//        /// </returns>
//        [AcceptVerbs(HttpVerbs.Get)]
//        public ActionResult FindAreasByNameForAutocomplete(string term)
//        {
//            var areas = this.areaRepository().FindAreasByName(term, 10);

//            if (areas.Count == 0)
//            {
//                var noresult = new[] { new { value = "Inte treff", subvalue = "Ingen", bbox = "none" } };

//                return this.Json(noresult, JsonRequestBehavior.AllowGet);
//            }

//            var result = from a in areas
//                         select
//                             new { value = a.Name.Trim(), subvalue = a.AreaDataset.Name.Trim(), bbox = a.Bbox.Trim() };

//            return this.Json(result, JsonRequestBehavior.AllowGet);
//        }

//        /// <summary>
//        /// Gets a GEOJSon string of sites
//        /// </summary>
//        /// <param name="zoomLevel">
//        /// The current Zoom level from Openlayers
//        /// </param>
//        /// <param name="bbox">
//        /// the BBOX to retrieve sites within
//        /// </param>
//        /// <param name="userId">
//        /// UserID of the request - for private sites
//        /// </param>
//        /// <param name="projectId">
//        /// Optional ProjectID - to get project related sites
//        /// </param>
//        /// <param name="isInAdmin">
//        /// The is In Admin.
//        /// </param>
//        /// <param name="coordSys">
//        /// The coord Sys.
//        /// </param>
//        /// <returns>
//        /// Featurecollection on GeoJSON format
//        /// </returns>
//        /// <example>
//        /// { "type": "FeatureCollection",
//        /// "features": [ 
//        /// { "type": "Feature",
//        /// "id": 123 ,
//        /// "geometry": { "type": "Point", "coordinates": [654321 , 6543212] } ,
//        /// "properties": { "sitename": "aSite" , "isParent": "false", "isPublic": "false" } }   
//        /// ] }
//        /// </example>
//        [PublicAuthorization]
//        public ActionResult GetSitesGeoJson(
//            int zoomLevel, string bbox, int userId, int projectId, bool isInAdmin, int? coordSys)
//        {
//            if (userId > 0 && this.CurrentUser.Id != userId)
//            {
//                return this.Content(string.Empty);
//            }

//            // todo: Check if user is administrator for sites before accepting _isInAdmin
//            ////if (isInAdmin && User.IsInRole("Ap2Admin"))
//            ////{

//            ////}
//            //// 
//            if (zoomLevel < 6)
//            {
//                return this.Content(string.Empty);
//            }

//            var extent = MapExtent.FromBboxString(bbox);

//            var sites = this.siteRepository().GetSites(
//                zoomLevel, 500, extent.MinX, extent.MinY, extent.MaxX, extent.MaxY, userId, projectId, isInAdmin);
//            var siteGeometries = this.siteRepository().GetSiteGeometries(
//                zoomLevel, 500, extent.MinX, extent.MinY, extent.MaxX, extent.MaxY, userId, projectId, isInAdmin);

//            if (sites.Count <= 0)
//            {
//                return this.Content(string.Empty);
//            }

//            return this.GetGeoJsonSites(sites, siteGeometries, coordSys);
//        }

//        /// <summary>
//        /// Gets a GEOJSon string of sites
//        /// </summary>
//        /// <param name="zoomLevel">
//        /// The current Zoom level from Openlayers
//        /// </param>
//        /// <param name="bbox">
//        /// the BBOX to retrieve sites within
//        /// </param>
//        /// <param name="userId">
//        /// UserID of the request - for private sites
//        /// </param>
//        /// <param name="projectId">
//        /// ProjectId of the project to get sites for
//        /// </param>
//        /// <returns>
//        /// Featurecollection on GeoJSON format
//        /// </returns>
//        /// <example>
//        /// { "type": "FeatureCollection",
//        /// "features": [ 
//        /// { "type": "Feature",
//        /// "id": 123 ,
//        /// "geometry": { "type": "Point", "coordinates": [654321 , 6543212] } ,
//        /// "properties": { "sitename": "aSite" , "isParent": "false", "isPublic": "false" } }   
//        /// ] }
//        /// </example>
//        [PublicAuthorization]
//        public ActionResult GetEditableSitesGeoJson(int zoomLevel, string bbox, int userId, int projectId)
//        {
//            if ((userId > 0 && this.CurrentUser.Id != userId) || zoomLevel < 6)
//            {
//                return this.Content(string.Empty);
//            }

//            var extent = MapExtent.FromBboxString(bbox);
//            IList<Site> sites = null;
//            IList<SiteGeometry> siteGeometries = null;
//            if (this.CurrentUser.Id == 0)
//            {
//                return this.Content(string.Empty);
//            }
//            else if (this.AuthorizationService.IsAuthorizedPublicSiteAdmin())
//            {
//                var features = this.AuthorizationService.GetFeatureIdsForPublicSiteAdmin();
//                sites = this.siteRepository().GetEditableSitesForAdmin(
//                    zoomLevel, 500, extent.MinX, extent.MinY, extent.MaxX, extent.MaxY, features);
//                siteGeometries = this.siteRepository().GetEditableSiteGeometriesForAdmin(
//                    zoomLevel, 500, extent.MinX, extent.MinY, extent.MaxX, extent.MaxY, features);
//            }
//            else if (this.AuthorizationService.IsAuthorizedOrganizationSiteAdmin())
//            {
//                int? organizationId = this.AuthorizationService.GetAuthorizedOrganizationIdForOrganizationSiteAdmin();
//                if (organizationId.HasValue)
//                {
//                    sites = this.siteRepository().GetEditableSitesForOrganization(
//                        zoomLevel, 500, extent.MinX, extent.MinY, extent.MaxX, extent.MaxY, organizationId.Value);
//                    siteGeometries = this.siteRepository().GetEditableSiteGeometriesForOrganization(
//                        zoomLevel, 500, extent.MinX, extent.MinY, extent.MaxX, extent.MaxY, organizationId.Value);
//                }
//            }
//            else
//            {
//                sites = this.siteRepository().GetEditableSitesForUser(
//                        zoomLevel, 500, extent.MinX, extent.MinY, extent.MaxX, extent.MaxY, userId, projectId);
//                siteGeometries = this.siteRepository().GetEditableSiteGeometriesForUser(
//                    zoomLevel, 500, extent.MinX, extent.MinY, extent.MaxX, extent.MaxY, userId, projectId);
//            }

//            if (sites == null || sites.Count <= 0)
//            {
//                return this.Content(string.Empty);
//            }

//            return this.GetGeoJsonSites(sites, siteGeometries, null);
//        }

//        /// <summary>
//        /// Gets a GEOJSon string of sites
//        /// </summary>
//        /// <param name="zoomLevel">
//        /// The current Zoom level from Openlayers
//        /// </param>
//        /// <param name="bbox">
//        /// the BBOX to retrieve sites within
//        /// </param>
//        /// <param name="userId">
//        /// UserID of the request - for private sites
//        /// </param>
//        /// <param name="projectId">
//        /// ProjectId for project (only sites for project is returned)
//        /// </param>
//        /// <returns>
//        /// Featurecollection on GeoJSON format
//        /// </returns>
//        /// <example>
//        /// { "type": "FeatureCollection",
//        /// "features": [ 
//        /// { "type": "Feature",
//        /// "id": 123 ,
//        /// "geometry": { "type": "Point", "coordinates": [654321 , 6543212] } ,
//        /// "properties": { "sitename": "aSite" , "isParent": "false", "isPublic": "false" } }   
//        /// ] }
//        /// </example>
//        [PublicAuthorization]
//        public ActionResult GetNonEditableSitesGeoJson(int zoomLevel, string bbox, int userId, int projectId)
//        {
//            if ((userId > 0 && this.CurrentUser.Id != userId) || zoomLevel < 6)
//            {
//                return this.Content(string.Empty);
//            }

//            var extent = MapExtent.FromBboxString(bbox);
//            IList<Site> sites = null;
//            IList<SiteGeometry> siteGeometries = null;
//            if (this.CurrentUser.Id == 0)
//            {
//                return this.GetSitesGeoJson(zoomLevel, bbox, userId, projectId, false, null);
//            }
//            else if (this.AuthorizationService.IsAuthorizedPublicSiteAdmin())
//            {
//                var features = this.AuthorizationService.GetFeatureIdsForPublicSiteAdmin();
//                sites = this.siteRepository().GetNonEditableSitesForAdmin(
//                    zoomLevel, 500, extent.MinX, extent.MinY, extent.MaxX, extent.MaxY, userId, features);
//                siteGeometries = this.siteRepository().GetNonEditableSiteGeometriesForAdmin(
//                    zoomLevel, 500, extent.MinX, extent.MinY, extent.MaxX, extent.MaxY, userId, features);
//            }
//            else if (this.AuthorizationService.IsAuthorizedOrganizationSiteAdmin())
//            {
//                var organizationId = this.AuthorizationService.GetAuthorizedOrganizationIdForOrganizationSiteAdmin();
//                if (organizationId.HasValue)
//                {
//                    sites = this.siteRepository().GetNonEditableSitesForOrganization(
//                        zoomLevel, 500, extent.MinX, extent.MinY, extent.MaxX, extent.MaxY, userId, organizationId.Value);
//                    siteGeometries = this.siteRepository().GetNonEditableSiteGeometriesForOrganization(
//                        zoomLevel, 500, extent.MinX, extent.MinY, extent.MaxX, extent.MaxY, organizationId.Value);
//                }
//            }
//            else
//            {
//                sites = this.siteRepository().GetNonEditableSitesForUser(
//                        zoomLevel, 500, extent.MinX, extent.MinY, extent.MaxX, extent.MaxY, userId, projectId);
//                siteGeometries = this.siteRepository().GetNonEditableSiteGeometriesForUser(
//                    zoomLevel, 500, extent.MinX, extent.MinY, extent.MaxX, extent.MaxY, userId, projectId);
//            }

//            if (sites == null || sites.Count <= 0)
//            {
//                return this.Content(string.Empty);
//            }

//            return this.GetGeoJsonSites(sites, siteGeometries, null);
//        }

//        /// <summary>
//        /// Gets a GEOJSon string of sites
//        /// </summary>
//        /// <param name="siteIds">
//        /// The Site Ids.
//        /// </param>
//        /// <param name="coordSys">
//        /// The coord Sys.
//        /// </param>
//        /// <returns>
//        /// Featurecollection on GeoJSON format
//        /// </returns>
//        /// <example>
//        /// { "type": "FeatureCollection",
//        /// "features": [ 
//        /// { "type": "Feature",
//        /// "id": 123 ,
//        /// "geometry": { "type": "Point", "coordinates": [654321 , 6543212] } ,
//        /// "properties": { "sitename": "aSite" , "isParent": "false", "isPublic": "false" } }   
//        /// ] }
//        /// </example>
//        [PublicAuthorization]
//        public ActionResult GetSpecificSitesGeoJson(int[] siteIds, int coordSys)
//        {
//            if (siteIds == null)
//            {
//                return this.Content(string.Empty);
//            }

//            if (siteIds.Count() == 0)
//            {
//                return this.Content(string.Empty);
//            }

//            var sites = this.siteRepository().GetSites(siteIds);
//            var siteGeometries = this.siteRepository().GetSiteGeometries(siteIds);

//            return sites.Count <= 0 ? this.Content(string.Empty) : this.GetGeoJsonSites(sites, siteGeometries, coordSys);
//        }

//        /////// <summary>
//        /////// dasf �lk df�als dfl�j 
//        /////// </summary>
//        /////// <param name="siteIds">
//        /////// The site ids.
//        /////// </param>
//        /////// <param name="bbox">
//        /////// The bbox.
//        /////// </param>
//        /////// <param name="coordSys">
//        /////// The coord Sys.
//        /////// </param>
//        /////// <returns>
//        /////// somethingjfd �laf 
//        /////// </returns>
//        ////[PublicAuthorization]
//        ////public ActionResult GetSpecificSitesGeoJsonConstrainedByMap(int[] siteIds, string bbox, int? coordSys)
//        ////{
//        ////    if (siteIds == null)
//        ////    {
//        ////        return this.Content(string.Empty);
//        ////    }

//        ////    if (siteIds.Count() == 0)
//        ////    {
//        ////        return this.Content(string.Empty);
//        ////    }

//        ////    var sites = this.siteRepository().GetSites(siteIds);

//        ////    var extent = MapExtent.FromBboxString(bbox);

//        ////    var sitesToReturn = new List<Site>();
//        ////    List<int> sitesToReturnIds = new List<int>();

//        ////    foreach (Site site in sites)
//        ////    {
//        ////        if (site.XCoord > extent.MinX && site.XCoord < extent.MaxX && site.YCoord > extent.MinY && site.YCoord < extent.MaxY)
//        ////        {
//        ////            sitesToReturn.Add(site);
//        ////            sitesToReturnIds.Add(site.Id);
//        ////        }
//        ////    }

//        ////    var siteGeomtriesToReturn = this.siteRepository().GetSiteGeometries(sitesToReturnIds.ToArray());

//        ////    return sitesToReturn.Count <= 0 ? this.Content(string.Empty) : this.GetGeoJsonSites(sitesToReturn, siteGeomtriesToReturn, coordSys);
//        ////}

//        /// <summary>
//        /// Gets a static map image
//        /// </summary>
//        /// <param name="version">
//        /// Request version.
//        /// </param>
//        /// <param name="request">
//        /// Request name.
//        /// </param>
//        /// <param name="bbox">
//        /// The bbox extent of request
//        /// </param>
//        /// <param name="width">
//        /// The width.
//        /// </param>
//        /// <param name="height">
//        /// The height.
//        /// </param>
//        /// <param name="layers">
//        /// Comma-separated list of one or more map layers.
//        /// </param>
//        /// <param name="siteIds">
//        /// Optional list of site Ids
//        /// </param>
//        /// <returns>
//        /// An image stream
//        /// </returns>
//        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope",
//            Justification = "Try catch finnaly would be heavy overhead here - no reason why it should fail...")]
//        [PublicAuthorization]
//        public ActionResult GetStaticMap(
//            string version, string request, string bbox, int width, int height, string[] layers, string siteIds)
//        {
//            // only support GetMap
//            if (request.ToLowerInvariant() != "getmap")
//            {
//                throw new InvalidOperationException("Method only supports the GetMap request; request=GetMap");
//            }

//            // method throw error if invalid
//            var mapExtent = MapExtent.FromBboxString(bbox);

//            if (width == 0)
//            {
//                throw new ArgumentOutOfRangeException("width", width, "width must be greater than 0");
//            }

//            if (height == 0)
//            {
//                throw new ArgumentOutOfRangeException("height", height, "height must be greater than 0");
//            }

//            if (layers.Count() == 0)
//            {
//                throw new ArgumentException("No layers selected");
//            }

//            if ((layers.Count() > 1 && !layers.Contains("sites")) || (layers.Count() == 1 && layers[0] != "sites"))
//            {
//                throw new ArgumentException("only 'sites' as of today)", "layers");
//            }

//            IList<Site> sites = null;
//            if (!string.IsNullOrEmpty(siteIds))
//            {
//                var ids = Array.ConvertAll(
//                    siteIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries), Convert.ToInt32);
//                sites = this.siteRepository().GetByIds(ids);
//            }

//            var memStream = this.gisService().GetMapImageAsMemStream(sites, height, width, mapExtent);
//            return new FileContentResult(memStream.ToArray(), "image/jpeg");
//        }

//        /// <summary>
//        /// The save site.
//        /// </summary>
//        /// <param name="site">
//        /// The site to save or update
//        /// </param>
//        /// <param name="geometry">
//        /// The polygon to assosiate if any
//        /// </param>
//        /// <param name="parentId">
//        /// The parent id for the site
//        /// </param>
//        /// <param name="comment">
//        /// The comment.
//        /// </param>
//        /// <param name="coordSys">
//        /// The coord Sys.
//        /// </param>
//        /// <returns>
//        /// String with error message if any
//        /// </returns>
//        public ActionResult SaveSite(Site site, string geometry, string parentId, string comment, int? coordSys)
//        {
//            try
//            {
//                int pId;
//                int.TryParse(parentId, out pId);
//                if (pId > 0)
//                {
//                    site.Parent = this.siteRepository().GetSiteById(pId);
//                }

//                site.Comment = comment;
//                site.IsPrivate = 1;
//                site.User = this.CurrentUser;
//                site.InputString = geometry.Contains("POLYGON") ? "map;polygon" : "map;point";
//                if (this.CurrentUser.Id == 0)
//                {
//                    // not logget in. Not allowed to save a site
//                    var errorResult = new { success = false, message = "Map_Not_Logged_In".Localize().ToString() };
//                    return this.Json(errorResult);
//                }
//                else if (this.AuthorizationService.IsAuthorizedPublicSiteAdmin())
//                {
//                    var adminfeatures = this.AuthorizationService.GetFeatureIdsForPublicSiteAdmin();
//                    var areas = this.siteRepository().GetSiteAreasForPoint(site.XCoord, site.YCoord);
//                    bool autorizedToMove = false;
//                    foreach (var area in areas)
//                    {
//                        foreach (var featureId in adminfeatures)
//                        {
//                            if (area.FeatureId == featureId)
//                            {
//                                autorizedToMove = true;
//                                break;
//                            }
//                        }

//                        if (autorizedToMove)
//                        {
//                            break;
//                        }
//                    }

//                    if (!autorizedToMove)
//                    {
//                        var errorResult =
//                            new
//                            {
//                                error = true,
//                                success = false,
//                                message = "Map_Permission_Denied_Outside_Area".Localize().ToString()
//                            };
//                        return this.Json(errorResult);
//                    }

//                    site.IsPrivate = 0;
//                }
//                else if (this.AuthorizationService.IsAuthorizedOrganizationSiteAdmin())
//                {
//                    var organizationId = this.AuthorizationService.GetAuthorizedOrganizationIdForOrganizationSiteAdmin();
//                    if (!this.siteRepository().IsPointWithinAllowedArea(site.XCoord, site.YCoord))
//                    {
//                        var errorResult = new { success = false, message = "Map_Wrong_Country".Localize().ToString() };
//                        return this.Json(errorResult);
//                    }

//                    if (organizationId.HasValue)
//                    {
//                        Organization org = this.organizationRepository().GetByOrganizationId(organizationId.Value);
//                        site.ControlingOrganisation = org;
//                    }
//                }
//                else
//                {
//                    if (!this.siteRepository().IsPointWithinAllowedArea(site.XCoord, site.YCoord))
//                    {
//                        var errorResult = new { success = false, message = "Map_Wrong_Country".Localize().ToString() };
//                        return this.Json(errorResult);
//                    }

//                    site.ControlingUser = this.CurrentUser;
//                }

//                this.siteRepository().SaveSite(site);
//            }
//            catch (Exception ex)
//            {
//                var errorResult = new { success = false, message = "Site not saved:" + ex.Message };

//                return this.Json(errorResult);
//            }

//            SiteGeometry siteGeometry = null;
//            if (geometry.Contains("POLYGON"))
//            {
//                try
//                {
//                    siteGeometry = new SiteGeometry(geometry)
//                    {
//                        User = this.CurrentUser,
//                        Site = site,
//                        ChangedByUser = this.CurrentUser
//                    };
//                    this.siteRepository().SaveSiteGeometry(siteGeometry);
//                }
//                catch (Exception ex)
//                {
//                    var errorResult = new { success = false, message = "Site not saved:" + ex.Message };

//                    return this.Json(errorResult);
//                }
//            }

//            IList<Site> sites = new List<Site> { site };
//            IList<SiteGeometry> siteGeometries = new List<SiteGeometry>();
//            if (siteGeometry != null)
//            {
//                siteGeometries.Add(siteGeometry);
//            }

//            return this.GetGeoJsonSites(sites, siteGeometries, coordSys);
//        }

//        /// <summary>
//        /// The move site.
//        /// </summary>
//        /// <param name="siteId">
//        /// The site to save or update
//        /// </param>
//        /// <param name="geometry">
//        /// The polygon to assosiate if any
//        /// </param>
//        /// <param name="coordX">
//        /// The centroid x
//        /// </param>
//        /// <param name="coordY">
//        /// The centroid y
//        /// </param>
//        /// <param name="coordSys">
//        /// The coord Sys.
//        /// </param>
//        /// <returns>
//        /// String with error message if any
//        /// </returns>
//        public ActionResult MoveSite(int siteId, string geometry, int coordX, int coordY, int? coordSys)
//        {
//            Site site = null;
//            try
//            {
//                if (siteId <= 0)
//                {
//                    var errorResult = new { success = false, message = "Map_Not_Valid_SiteId".Localize().ToString() };
//                    return this.Json(errorResult);
//                }

//                site = this.siteService().GetSite(siteId);

//                if (this.CurrentUser.Id == 0)
//                {
//                    // not logget in. Not allowed to move a site
//                    var errorResult = new { success = false, message = "Map_Not_Logged_In".Localize().ToString() };
//                    return this.Json(errorResult);
//                }
//                else if (this.AuthorizationService.IsAuthorizedPublicSiteAdmin())
//                {
//                    var adminfeatures = this.AuthorizationService.GetFeatureIdsForPublicSiteAdmin();
//                    var areas = this.siteRepository().GetSiteAreasForPoint(coordX, coordY);
//                    bool autorizedToMove = false;
//                    foreach (var area in areas)
//                    {
//                        foreach (var featureId in adminfeatures)
//                        {
//                            if (area.FeatureId == featureId)
//                            {
//                                autorizedToMove = true;
//                                break;
//                            }
//                        }

//                        if (autorizedToMove)
//                        {
//                            break;
//                        }
//                    }

//                    if (!autorizedToMove)
//                    {
//                        var errorResult =
//                            new
//                            {
//                                error = true,
//                                success = false,
//                                message = "Map_Permission_Denied_Outside_Area".Localize().ToString()
//                            };
//                        return this.Json(errorResult);
//                    }
//                }
//                else if (this.AuthorizationService.IsAuthorizedOrganizationSiteAdmin())
//                {
//                    var organizationId = this.AuthorizationService.GetAuthorizedOrganizationIdForOrganizationSiteAdmin();
//                    if (organizationId.HasValue)
//                    {
//                        if (organizationId.Value != site.ControlingOrganisation.OrganizationId)
//                        {
//                            var errorResult = new { success = false, message = "Map_Permission_Denied_Not_Authorized".Localize().ToString() };
//                            return this.Json(errorResult);
//                        }
//                    }

//                    if (!this.siteRepository().IsPointWithinAllowedArea(coordX, coordY))
//                    {
//                        var errorResult = new { success = false, message = "Map_Wrong_Country".Localize().ToString() };
//                        return this.Json(errorResult);
//                    }
//                }
//                else
//                {
//                    if (!this.siteRepository().IsPointWithinAllowedArea(coordX, coordY))
//                    {
//                        var errorResult = new { success = false, message = "Map_Wrong_Country".Localize().ToString() };
//                        return this.Json(errorResult);
//                    }

//                    if (site.ControlingUser != this.CurrentUser)
//                    {
//                        var errorResult = new { success = false, message = "Map_Permission_Denied_Not_Authorized".Localize().ToString() };
//                        return this.Json(errorResult);
//                    }
//                }

//                site.InputString = geometry.Contains("POLYGON") ? "map;polygon" : "map;point";
//                site.XCoord = coordX;
//                site.YCoord = coordY;
//                site.ChangedByUser = this.CurrentUser;
//                this.siteRepository().SaveSite(site);
//            }
//            catch (Exception ex)
//            {
//                var errorResult = new { success = false, message = "Site not updated:" + ex.Message };

//                return this.Json(errorResult);
//            }

//            SiteGeometry siteGeometry = null;
//            if (geometry.Contains("POLYGON"))
//            {
//                try
//                {
//                    siteGeometry = this.siteRepository().GetSiteGeometryBySiteId(siteId);
//                    siteGeometry.ChangedByUser = this.CurrentUser;
//                    siteGeometry.Geometry = geometry;
//                    this.siteRepository().SaveSiteGeometry(siteGeometry);
//                }
//                catch (Exception ex)
//                {
//                    var errorResult = new { success = false, message = "Site geometry not updated:" + ex.Message };

//                    return this.Json(errorResult);
//                }
//            }

//            IList<Site> sites = new List<Site> { site };
//            IList<SiteGeometry> siteGeometries = new List<SiteGeometry>();
//            if (siteGeometry != null)
//            {
//                siteGeometries.Add(siteGeometry);
//            }

//            return this.GetGeoJsonSites(sites, siteGeometries, coordSys);
//        }

//        #endregion

//        #region Methods

//        /// <summary>
//        /// Sets sitetype for gui presentation
//        /// </summary>
//        /// <param name="site">
//        /// The site to inspect.
//        /// </param>
//        /// <returns>
//        /// returns a number like 0, 1, 2
//        /// </returns>
//        internal static int GetSiteType(Site site)
//        {
//            if (site.IsPrivate == 1)
//            {
//                return 0;
//            }

//            return site.Parent != null ? 2 : 1;
//        }

//        /// <summary>
//        /// The coordinates of a site
//        /// </summary>
//        /// <param name="site">
//        /// The site.
//        /// </param>
//        /// <returns>
//        /// optionally transformed coordinates
//        /// </returns>
//        internal static int[] SiteCoordinates(Site site)
//        {
//            return new[] { site.XCoord, site.YCoord };
//        }

//        /// <summary>
//        /// The coordinates of a site polygon
//        /// </summary>
//        /// <param name="siteG">
//        /// The site geometry.
//        /// </param>
//        /// <returns>
//        /// optionally transformed coordinates
//        /// </returns>
//        internal static IEnumerable<double[]>[] SitePolygonCoordinates(SiteGeometry siteG)
//        {
//            return new[] { from c in siteG.GetGeometryPoints select new[] { c.X, c.Y } };
//        }

//        /// <summary>
//        /// Gets a GeoJsonSitesresult
//        /// </summary>
//        /// <param name="sites">
//        /// The sites.
//        /// </param>
//        /// <param name="siteGeometries">
//        /// The site geometries.
//        /// </param>
//        /// <param name="coordSys">
//        /// The requested coordinatesystem 
//        /// </param>
//        /// <returns>
//        /// GeoJeson thing
//        /// </returns>
//        private ActionResult GetGeoJsonSites(IList<Site> sites, IList<SiteGeometry> siteGeometries, int? coordSys)
//        {
//            var userCoordinatesystem = this.CurrentUser.CoordinateSystem
//                                       ??
//                                       this.mapRepository().GetCoordinateSystem(
//                                           int.Parse(ConfigurationManager.AppSettings["DefaultCoordinatesystemId"]));
//            var userCoordinateSystemNotation = this.CurrentUser.CoordinateSystemNotation ?? userCoordinatesystem.CoordinateSystemGrouping.CoordinateSystemNotations.Where(x => x.NotationId == int.Parse(ConfigurationManager.AppSettings["DefaultCoordinatesystemNotationId"])).FirstOrDefault();

//            var siteIDs = (from sg in siteGeometries select sg.Site.Id).ToArray();
//            var geoJsonSite = from site in sites
//                              where !siteIDs.Contains(site.Id)
//                              select
//                                  new
//                                  {
//                                      type = "Feature",
//                                      id = site.Id,
//                                      geometry = new { type = "Point", coordinates = SiteCoordinates(site) },
//                                      properties =
//                              new
//                              {
//                                  siteName = site.Name,
//                                  siteAreaDescription = "Kommun",
//                                  //// siteAreaName = "NoAreasForSpead", 
//                                  siteAreaName =
//                          this.siteService().GetAreaName(site.Areas, AreaDatasetId.MunicipalityIds),
//                                  coordSystemName = this.siteService().GetDatabaseCoordinateSystemName(),
//                                  accuracy = site.Accuracy,
//                                  parentId = site.Parent != null ? site.Parent.Id : 0,
//                                  siteType = GetSiteType(site),
//                                  coordString =
//                          this.gisService().SiteCoordinateStringPresentation(site, userCoordinatesystem, userCoordinateSystemNotation)
//                              }
//                                  };
//            var geoJsonSiteGeometries = from siteG in siteGeometries
//                                        select
//                                            new
//                                            {
//                                                type = "Feature",
//                                                id = siteG.Site.Id,
//                                                geometry =
//                                        new
//                                        {
//                                            type = "Polygon",
//                                            coordinates = SitePolygonCoordinates(siteG)
//                                        },
//                                                properties =
//                                        new
//                                        {
//                                            siteName = siteG.Site.Name,

//                                            // todo: @ Stein. Remove silly hardcoded description
//                                            siteAreaDescription = "Kommun",
//                                            siteAreaName =
//                                    this.siteService().GetAreaName(siteG.Site.Areas, AreaDatasetId.MunicipalityIds),
//                                            coordSystemName = this.siteService().GetDatabaseCoordinateSystemName(),
//                                            accuracy = siteG.Site.Accuracy,
//                                            parentId = siteG.Site.Parent != null ? siteG.Site.Parent.Id : 0,
//                                            siteType = GetSiteType(siteG.Site),
//                                            coordString = this.gisService().SiteCoordinateStringPresentation(siteG.Site, userCoordinatesystem, userCoordinateSystemNotation)
//                                        }
//                                            };
//            var result =
//                new
//                {
//                    points = new { type = "FeatureCollection", features = geoJsonSite },
//                    polygons = new { type = "FeatureCollection", features = geoJsonSiteGeometries }
//                };
//            return this.Json(result);
//        }
//        #endregion
//    }
//}
