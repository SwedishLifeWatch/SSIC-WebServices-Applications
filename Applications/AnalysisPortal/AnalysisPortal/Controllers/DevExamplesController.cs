using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AnalysisPortal.Helpers;
using AnalysisPortal.Helpers.DevExamplesModels;
using ArtDatabanken;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Enums;
using ArtDatabanken.WebApplication.AnalysisPortal.Json;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Localization;
using Newtonsoft.Json;

namespace AnalysisPortal.Controllers
{
#if DEBUG
    public class UploadFilesResult
    {
        public string Name { get; set; }
        public int Length { get; set; }
        public string Type { get; set; }
    }

    /// <summary>
    /// This Controller has Actions that is examples how we can or
    /// we should implement common things.
    /// </summary>
    public class DevExamplesController : Controller
    {
        [HttpGet]
        public ActionResult UploadFileUsingPostSample()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadFileUsingPostSample(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                ////file.SaveAs(path);
            }

            return RedirectToAction("UploadFileUsingPostSample");
        }

        public ActionResult UploadFileSample()
        {
            return View();
        }

        [HttpPost]
        public ContentResult UploadFiles()
        {
            var r = new List<UploadFilesResult>();

            foreach (string file in Request.Files)
            {
                HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                if (hpf.ContentLength == 0)
                {
                    continue;
                }

                string savedFileName = Path.Combine(Server.MapPath("~/App_Data"), Path.GetFileName(hpf.FileName));
                ////hpf.SaveAs(savedFileName); // Save the file

                r.Add(new UploadFilesResult()
                {
                    Name = hpf.FileName,
                    Length = hpf.ContentLength,
                    Type = hpf.ContentType
                });
            }
            // Returns json
            return Content("{\"name\":\"" + r[0].Name + "\",\"type\":\"" + r[0].Type + "\",\"size\":\"" + string.Format("{0} bytes", r[0].Length) + "\"}", "application/json");
        }

        public ActionResult SelectTextInExtJsGrid()
        {
            return View();
        }

        public ActionResult MapGeoJsonString()
        {
            ViewBag.GeoJsonAsString = GeoJsonCreator.CreateSampleJsonString();
            return View();
        }

        public ActionResult MapAP()
        {
            return View();
        }

        public ActionResult MapGoogle()
        {
            return View();
        }

        public ActionResult MapBasic()
        {
            return View();
        }

        public ActionResult SessionTimeout()
        {
            return View();
        }

        public ActionResult SessionTimeout2()
        {
            return View();
        }

        public ActionResult SessionTimeout3()
        {
            return View();
        }

        public ActionResult MapGoogleGeoJson()
        {
            ViewBag.geoJson = "hej";
            //JsonNetResult result = GetObservations();
            return View();
        }

        public ActionResult MapGoogleGeoJsonPlainCss()
        {
            return View();
        }

        public ActionResult LongRunningAjax()
        {
            return View();
        }

        //public Task<ActionResult> MyLongRunningProcess()
        //{
        //    return View();
        //}

        //public ActionResult Gizmos()
        //{
        //    ViewBag.SyncOrAsync = "Synchronous";
        //    var gizmoService = new GizmoService();
        //    return View("Gizmos", gizmoService.GetGizmos());
        //}

        public static int? NullableTryParseInt32(string text)
        {
            int value;
            return int.TryParse(text, out value) ? (int?)value : null;
        } 

        private object GetObservationsObject(IList<ISpeciesObservation> observationsList)
        {            
            var geoJsonSite = 
                from observation in observationsList                              
                select new
                    {
                        type = "Feature",                        
                        id = observation.DatasetID,
                        geometry = new { type = "Point", coordinates = new[] { observation.Location.CoordinateX, observation.Location.CoordinateY } },
                        properties = new
                            {
                                siteName = observation.Location.Locality,                                
                                coordSystemName = observation.Location.GeodeticDatum,
                                accuracy = NullableTryParseInt32(observation.Location.CoordinateUncertaintyInMeters),
                                county = observation.Location.Country,
                                municipality = observation.Location.Municipality,
                                parish = observation.Location.Parish,
                                stateProvince = observation.Location.StateProvince,
                                locality = observation.Location.Locality,
                                recordedBy = observation.Occurrence.RecordedBy,
                                occurrenceId = observation.Occurrence.OccurrenceID,
                                taxonId = observation.Taxon.TaxonID,
                                scientificName = observation.Taxon.ScientificName,
                                organismGroup = observation.Taxon.OrganismGroup,
                                vernacularName = observation.Taxon.VernacularName,
                                siteType = 2                                
                            }
                    };

            var result = 
                new
                {
                    points = new { type = "FeatureCollection", features = geoJsonSite },                    
                };

            return result;
        }

        public JsonNetResult GetObservations()
        {
            JsonModel jsonModel;
            try
            {
                IUserContext user = CoreData.UserManager.GetCurrentUser();
                var obsSearchCriteria = new SpeciesObservationSearchCriteria
                                            { TaxonIds = new List<int> { 100573 }, IncludePositiveObservations = true };
                var coordinateSystem = new CoordinateSystem { Id = CoordinateSystemId.GoogleMercator };    
                //CoordinateSystemId.SWEREF99

                user.CurrentRole = user.CurrentRoles[0];
                var obsList = new SpeciesObservationList();
                SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();
                if (obsSearchCriteria.TaxonIds.Count > 0)
                {
                    obsList = CoreData.SpeciesObservationManager.GetSpeciesObservations(user, obsSearchCriteria, coordinateSystem);
                }

                IList<ISpeciesObservation> resultList = obsList.GetGenericList();

                var observationObject = GetObservationsObject(resultList);

                jsonModel = JsonModel.CreateFromObject(observationObject);                
            }
            catch (Exception ex)
            {
                jsonModel = JsonModel.CreateFailure(ex.Message);
            }
            return new JsonNetResult(jsonModel);
        }

        public ActionResult MapBing()
        {
            return View();
        }

        public ActionResult ExtJsCustomTooltip()
        {
            return View();
        }

        public ActionResult ExtJsBasicTooltip()
        {
            return View();
        }

        public ActionResult BootstrapToolTip()
        {
            return View();
        }

        public ActionResult BootstrapPopOver()
        {
            return View();
        }

        public ActionResult BootstrapButtonStates()
        {
            return View();
        }

        public ActionResult WFSMap()
        {
            return View();
        }

        public ActionResult ExtJsLayout()
        {
            return View();
        }

        public ActionResult ExtJSGrid()
        {
            ViewBag.StockListAsJSON = JsonConvert.SerializeObject(Stock.CreateSampleData());
            return View();
        }

        public ActionResult ExtJSGridJSON()
        {            
            return View();
        }

        [HttpGet]
        public ActionResult ExtJSFormSubmit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ExtJSFormSubmit(string name, int age, int? rating)
        {
            if (Request.IsAjaxRequest())
            {
                if (age > 50)
                {
                    return Json(new { success = false, data = new { name = name, age = age } });    
                }
                else
                {
                    return Json(new { success = true, data = new { name = name, age = age } });    
                }                                
            }
            string msg = string.Format("Name: {0}, Age {1}", name ?? "-", age);
            return Content(msg);
        }

        public JsonResult GetStocks()
        {
            return Json(Stock.CreateSampleData(), JsonRequestBehavior.AllowGet);
        }

        public JsonNetResult GetAllRowDelimiters()
        {            
            var rowDelimiters = from RowDelimiter rd in Enum.GetValues(typeof(RowDelimiter))
                                select new { Value = (int)rd, Text = rd.GetLocalizedDescription() };
            var jsonModel = JsonModel.Create(rowDelimiters.ToList());
            return new JsonNetResult(jsonModel);
        }

    #region Generate Properties

        /// <summary>
        /// Generate species observation properties to a text file.
        /// </summary>
        /// <returns></returns>
        public ActionResult GenerateObservationProperties()
        {
            string filePath = string.Empty;
            try
            {
                IUserContext userContext = CoreData.UserManager.GetApplicationContext();
                // Serach griffelblomfluga to extract properties from
                var obsSearchCriteria = new SpeciesObservationSearchCriteria { TaxonIds = new List<int> { 100573 }, IncludePositiveObservations = true };
                var coordinateSystem = new CoordinateSystem { Id = CoordinateSystemId.WGS84 };

                userContext.CurrentRole = userContext.CurrentRoles[0];
                var obsList = new SpeciesObservationList();
                SpeciesObservationFieldList fieldList = new SpeciesObservationFieldList();
                if (obsSearchCriteria.TaxonIds.Count > 0)
                {
                    obsList = CoreData.SpeciesObservationManager.GetSpeciesObservations(userContext, obsSearchCriteria, coordinateSystem);
                }

                ISpeciesObservation observation = obsList[0];

                Dictionary<string, string> tableData = new Dictionary<string, string>();
                tableData = ExtractProperties(observation);
                filePath = GetFilePath("SpeciesObservationProperties" + System.DateTime.Now.ToShortDateString());
                SaveFileToDisk(filePath, tableData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            ViewData["FilePath"] = filePath;
            return View();
        }

        /// <summary>
        /// Get properties and add them to a list.
        /// </summary>
        /// <param name="obs"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ExtractProperties(ISpeciesObservation obs)
        {
            Dictionary<string, string> propertyList = new Dictionary<string, string>();
            string propertyValueAsString = null;
            Type type = obs.GetType();
            PropertyInfo[] propInfos = type.GetProperties();
            for (int i = 0; i < propInfos.Length; i++)
            {
                PropertyInfo pi = (PropertyInfo)propInfos.GetValue(i);
                string propName = pi.Name;
                object propValue = pi.GetValue(obs, null);
                if (propValue is ISpeciesObservationConservation || propValue is ISpeciesObservationEvent ||
                    propValue is ISpeciesObservationGeologicalContext ||
                    propValue is ISpeciesObservationIdentification || propValue is ISpeciesObservationLocation ||
                    propValue is ISpeciesObservationMeasurementOrFact ||
                    propValue is ISpeciesObservationOccurrence || propValue is ISpeciesObservationProject ||
                    propValue is ISpeciesObservationResourceRelationship ||
                    propValue is ISpeciesObservationTaxon)
                {
                    PropertyInfo[] subSpeciesPropertyInfos = null;
                    if (propValue is ISpeciesObservationConservation)
                    {
                        subSpeciesPropertyInfos = typeof(ISpeciesObservationConservation).GetProperties();
                    }
                    else if (propValue is ISpeciesObservationEvent)
                    {
                        subSpeciesPropertyInfos = typeof(ISpeciesObservationEvent).GetProperties();
                    }
                    else if (propValue is ISpeciesObservationGeologicalContext)
                    {
                        subSpeciesPropertyInfos = typeof(ISpeciesObservationGeologicalContext).GetProperties();
                    }
                    else if (propValue is ISpeciesObservationIdentification)
                    {
                        subSpeciesPropertyInfos = typeof(ISpeciesObservationIdentification).GetProperties();
                    }
                    else if (propValue is ISpeciesObservationLocation)
                    {
                        subSpeciesPropertyInfos = typeof(ISpeciesObservationLocation).GetProperties();
                    }
                    else if (propValue is ISpeciesObservationMeasurementOrFact)
                    {
                        subSpeciesPropertyInfos = typeof(ISpeciesObservationMeasurementOrFact).GetProperties();
                    }
                    else if (propValue is ISpeciesObservationOccurrence)
                    {
                        subSpeciesPropertyInfos = typeof(ISpeciesObservationOccurrence).GetProperties();
                    }
                    else if (propValue is ISpeciesObservationProject)
                    {
                        subSpeciesPropertyInfos = typeof(ISpeciesObservationProject).GetProperties();
                    }
                    else if (propValue is ISpeciesObservationResourceRelationship)
                    {
                        subSpeciesPropertyInfos = typeof(ISpeciesObservationResourceRelationship).GetProperties();
                    }
                    else if (propValue is ISpeciesObservationTaxon)
                    {
                        subSpeciesPropertyInfos = typeof(ISpeciesObservationTaxon).GetProperties();
                    }
                    else
                    {
                        subSpeciesPropertyInfos = new PropertyInfo[] { };
                    }

                    //Add property
                    AddClassPropertiesToList(obs, subSpeciesPropertyInfos, propValue, propertyList);
                }
                else
                {
                    propertyValueAsString = GetPropertyValueAsString(propValue);
                    propertyList.Add(propName, propertyValueAsString);
                }
            }
            return propertyList;
        }

        /// <summary>
        /// Get property value out of class and add properties to a list.
        /// </summary>
        /// <param name="speciesObservation"></param>
        /// <param name="propertyInfos"></param>
        /// <param name="value"></param>
        /// <param name="propertyList"> </param>
        private static void AddClassPropertiesToList(ISpeciesObservation speciesObservation, PropertyInfo[] propertyInfos, object value, Dictionary<string, string> propertyList)
        {
            for (int j = 0; j < propertyInfos.Length; j++)
            {
                PropertyInfo speciesSubPropertyInfo = propertyInfos[j];
                //Console.WriteLine(propertyInfo.Name + "-" + subPropInfo.Name);
                ISpeciesObservation speciesClass = speciesObservation;
                object propValue = null;
                string propertyName = null;
                propertyName = speciesSubPropertyInfo.Name;
                // We must know what tye of class the sub property orginates from to get correct value.
                if (value is ISpeciesObservationConservation)
                {
                    propValue = speciesSubPropertyInfo.GetValue(speciesClass.Conservation, null);
                }
                else if (value is ISpeciesObservationEvent)
                {
                    propValue = speciesSubPropertyInfo.GetValue(speciesClass.Event, null);
                }
                else if (value is ISpeciesObservationGeologicalContext)
                {
                    propValue = speciesSubPropertyInfo.GetValue(speciesClass.GeologicalContext, null);
                }
                else if (value is ISpeciesObservationIdentification)
                {
                    propValue = speciesSubPropertyInfo.GetValue(speciesClass.Identification, null);
                }
                else if (value is ISpeciesObservationLocation)
                {
                    propValue = speciesSubPropertyInfo.GetValue(speciesClass.Location, null);
                }
                else if (value is ISpeciesObservationMeasurementOrFact)
                {
                    propValue = speciesSubPropertyInfo.GetValue(speciesClass.MeasurementOrFact, null);
                }
                else if (value is ISpeciesObservationOccurrence)
                {
                    propValue = speciesSubPropertyInfo.GetValue(speciesClass.Occurrence, null);
                }
                else if (value is ISpeciesObservationProject)
                {
                    propValue = speciesSubPropertyInfo.GetValue(speciesClass.Project, null);
                }
                else if (value is ISpeciesObservationResourceRelationship)
                {
                    propValue = speciesSubPropertyInfo.GetValue(speciesClass.ResourceRelationship, null);
                }
                else if (value is ISpeciesObservationTaxon)
                {
                    propValue = speciesSubPropertyInfo.GetValue(speciesClass.Taxon, null);
                }
                else
                {
                    propValue = "New value: not implemeted in code";
                }
                // Add property to list
                string propertyValueAsString = GetPropertyValueAsString(propValue);
                propertyList.Add(propertyName, propertyValueAsString);
                Console.WriteLine("Name: " + propertyName + " Value: " + propertyValueAsString);
            }
        }

        /// <summary>
        /// Get property as a string value
        /// </summary>
        /// <param name="value"></param>
        private static string GetPropertyValueAsString(object value)
        {
            string propertyValue = null;
            if (value is String)
            {
                propertyValue = value.ToString();
            }
            else if (value is Int32)
            {
                propertyValue = Convert.ToString((int)value);
            }
            else if (value is Boolean)
            {
                propertyValue = Convert.ToString((bool)value);
            }
            else if (value is DateTime)
            {
                propertyValue = ((DateTime)value).ToShortDateString();
            }
            else if (value is Double)
            {
                propertyValue = Convert.ToString((double)value);
            }
            if (propertyValue.IsNull())
            {
                propertyValue = string.Empty;
            }

            return propertyValue;
        }

        /// <summary>
        /// Gets the full file name including the path.
        /// </summary>
        /// <param name="fileName">The name of the file</param>
        /// <returns></returns>
        private static string GetFilePath(string fileName)
        {
            string fileExtension = ".txt";
            fileName = fileName.Trim().RemoveDuplicateBlanks();
            String tempDirectory = Resources.AppSettings.Default.PathToTempDirectory;
            fileName = Path.ChangeExtension(fileName, fileExtension);
            string filePath = System.Web.HttpContext.Current.Server.MapPath(Path.Combine(tempDirectory, fileName));
            return filePath;
        }

        /// <summary>
        /// Saves file to disk.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        private static void SaveFileToDisk(string filePath, Dictionary<string, string> tableData)
        {
            EnsureFolder(filePath);

            // Create the file and write to it. 
            // DANGER: System.IO.File.Create will overwrite the file 
            // if it already exists. 

            using (System.IO.StreamWriter file = new StreamWriter(System.IO.File.Create(filePath)))
            {
                file.WriteLine("Species Observatoin Properties n=" + Convert.ToString(tableData.Count));
                file.WriteLine(string.Empty);
                foreach (var row in tableData)
                {
                    string line = "public string " + row.Key + " { get; set; }";
                    file.WriteLine(line);
                }
                file.WriteLine(string.Empty);
                file.WriteLine(string.Empty);
                file.WriteLine(string.Empty);
                file.WriteLine("Species Observation Table Headers");
                file.WriteLine(string.Empty);
                foreach (var row in tableData)
                {
                    string line = "SharedSpeciesObservation" + row.Key + "Label\t" + row.Key;
                    file.WriteLine(line);
                }
                file.WriteLine(string.Empty);
                file.WriteLine(string.Empty);
                file.WriteLine(string.Empty);
                file.WriteLine("Species Observation Table Data");
                file.WriteLine(string.Empty);
                foreach (var row in tableData)
                {
                    string line = "{ text: AnalysisPortal.Resources.SharedSpeciesObservation" + row.Key + "Label, width: 100, dataIndex: '" + row.Key + "', sortable: true },";
                    file.WriteLine(line);
                }

               // { name: 'ObservationId', type: 'string' },
                 file.WriteLine(string.Empty);
                file.WriteLine(string.Empty);
                file.WriteLine(string.Empty);
                file.WriteLine("Ext js Table Data");
                file.WriteLine(string.Empty);
                foreach (var row in tableData)
                {
                    string line = "{ name: '" + row.Key + "', type: 'string'},";
                    file.WriteLine(line);
                }
            }
        }

        /// <summary>
        /// Ensures that the folder on disk exists.
        /// If it doesn't exist, the folder will be created
        /// </summary>
        /// <param name="path">The path.</param>
        private static void EnsureFolder(string path)
        {
            string directoryName = Path.GetDirectoryName(path);
            if (directoryName != null && ((directoryName.Length > 0) && (!Directory.Exists(directoryName))))
            {
                Directory.CreateDirectory(directoryName);
            }
        }

    #endregion

    }

    public class Stock
    {
        public string Company { get; set; }
        public double Price { get; set; }
        public double Change { get; set; }
        public double PctChange { get; set; }
        public DateTime LastChange { get; set; }
        public int NumberEmployees { get; set; }
        public string CEO { get; set; }

        public Stock(string company, double price, double change, double pctChange, DateTime lastChange, int numberEmployees, string ceo)
        {
            Company = company;
            Price = price;
            Change = change;
            PctChange = pctChange;
            LastChange = lastChange;
            NumberEmployees = numberEmployees;
            CEO = ceo;
        }

        public static List<Stock> CreateSampleData()
        {
            var list = new List<Stock>();
            list.Add(new Stock("3m Co", 71.72, 0.02, 0.03, DateTime.Now, 55000, "Gordon Brown"));
            list.Add(new Stock("Alcoa Inc", 29.01, 0.42, 1.47, DateTime.Now, 12500, "James Fox"));
            list.Add(new Stock("Altria Group Inc", 83.81, 0.28, 0.34, DateTime.Now, 34500, "Lance Johansen"));
            list.Add(new Stock("American Express Company", 52.55, 0.01, 0.02, DateTime.Now, 43000, "Bill Walker"));
            list.Add(new Stock("American International Group, Inc.", 64.13, 0.31, 0.49, DateTime.Now, 28000, "John Andersen"));
            return list;
        }
    }

#endif
}
