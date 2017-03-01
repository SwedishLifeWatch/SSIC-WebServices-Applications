using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers.Presentation;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Details;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Table;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Observations
{
    /// <summary>
    /// This class is a view model for one species observation.
    /// </summary>
    public class SpeciesObservationViewModel
    {
        public string ObservationId { get; set; }
        public string AccessRights { get; set; }
        public string BasisOfRecord { get; set; }
        public string BibliographicCitation { get; set; }
        public string CollectionCode { get; set; }
        public string CollectionID { get; set; }
        public string ActionPlan { get; set; }
        public string ConservationRelevant { get; set; }
        public string Natura2000 { get; set; }
        public string ProtectedByLaw { get; set; }
        public string ProtectionLevel { get; set; }
        public string RedlistCategory { get; set; }
        public string SwedishImmigrationHistory { get; set; }
        public string SwedishOccurrence { get; set; }
        public string DataGeneralizations { get; set; }
        public string DatasetID { get; set; }
        public string DatasetName { get; set; }
        public string DynamicProperties { get; set; }
        public string Day { get; set; }
        public string End { get; set; }
        public string EndDayOfYear { get; set; }
        public string EventDate { get; set; }
        public string EventID { get; set; }
        public string EventRemarks { get; set; }
        public string EventTime { get; set; }
        public string FieldNotes { get; set; }
        public string FieldNumber { get; set; }
        public string Habitat { get; set; }
        public string Month { get; set; }
        public string SamplingEffort { get; set; }
        public string SamplingProtocol { get; set; }
        public string Start { get; set; }
        public string StartDayOfYear { get; set; }
        public string VerbatimEventDate { get; set; }
        public string Year { get; set; }
        public string Bed { get; set; }
        public string EarliestAgeOrLowestStage { get; set; }
        public string EarliestEonOrLowestEonothem { get; set; }
        public string EarliestEpochOrLowestSeries { get; set; }
        public string EarliestEraOrLowestErathem { get; set; }
        public string EarliestPeriodOrLowestSystem { get; set; }
        public string Formation { get; set; }
        public string GeologicalContextID { get; set; }
        public string Group { get; set; }
        public string HighestBiostratigraphicZone { get; set; }
        public string LatestAgeOrHighestStage { get; set; }
        public string LatestEonOrHighestEonothem { get; set; }
        public string LatestEpochOrHighestSeries { get; set; }
        public string LatestEraOrHighestErathem { get; set; }
        public string LatestPeriodOrHighestSystem { get; set; }
        public string LithostratigraphicTerms { get; set; }
        public string LowestBiostratigraphicZone { get; set; }
        public string Member { get; set; }
        public string DateIdentified { get; set; }
        public string Id { get; set; }
        public string IdentificationID { get; set; }
        public string IdentificationQualifier { get; set; }
        public string IdentificationReferences { get; set; }
        public string IdentificationRemarks { get; set; }
        public string IdentificationVerificationStatus { get; set; }
        public string IdentifiedBy { get; set; }
        public string TypeStatus { get; set; }
        public string UncertainDetermination { get; set; }
        public string InformationWithheld { get; set; }
        public string InstitutionCode { get; set; }
        public string InstitutionID { get; set; }
        public string Language { get; set; }
        public string Continent { get; set; }
        public string CoordinateM { get; set; }
        public string CoordinatePrecision { get; set; }
        public string CoordinateSystemWkt { get; set; }
        public string CoordinateUncertaintyInMeters { get; set; }
        public string CoordinateX { get; set; }
        public string CoordinateY { get; set; }
        public string CoordinateZ { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string County { get; set; }
        public string DecimalLatitude { get; set; }
        public string DecimalLongitude { get; set; }
        public string FootprintSpatialFit { get; set; }
        public string FootprintSRS { get; set; }
        public string FootprintWKT { get; set; }
        public string GeodeticDatum { get; set; }
        public string GeoreferencedBy { get; set; }
        public string GeoreferencedDate { get; set; }
        public string GeoreferenceProtocol { get; set; }
        public string GeoreferenceRemarks { get; set; }
        public string GeoreferenceSources { get; set; }
        public string GeoreferenceVerificationStatus { get; set; }
        public string HigherGeography { get; set; }
        public string HigherGeographyID { get; set; }
        public string Island { get; set; }
        public string IslandGroup { get; set; }
        public string Locality { get; set; }
        public string LocationAccordingTo { get; set; }
        public string LocationID { get; set; }
        public string LocationRemarks { get; set; }
        public string LocationURL { get; set; }
        public string MaximumDepthInMeters { get; set; }
        public string MaximumDistanceAboveSurfaceInMeters { get; set; }
        public string MaximumElevationInMeters { get; set; }
        public string MinimumDepthInMeters { get; set; }
        public string MinimumDistanceAboveSurfaceInMeters { get; set; }
        public string MinimumElevationInMeters { get; set; }
        public string Municipality { get; set; }
        public string Parish { get; set; }
        public string PointRadiusSpatialFit { get; set; }
        public string StateProvince { get; set; }
        public string VerbatimCoordinates { get; set; }
        public string VerbatimCoordinateSystem { get; set; }
        public string VerbatimDepth { get; set; }
        public string VerbatimElevation { get; set; }
        public string VerbatimLatitude { get; set; }
        public string VerbatimLocality { get; set; }
        public string VerbatimLongitude { get; set; }
        public string VerbatimSRS { get; set; }
        public string WaterBody { get; set; }
        public string MeasurementAccuracy { get; set; }
        public string MeasurementDeterminedBy { get; set; }
        public string MeasurementDeterminedDate { get; set; }
        public string MeasurementID { get; set; }
        public string MeasurementMethod { get; set; }
        public string MeasurementRemarks { get; set; }
        public string MeasurementType { get; set; }
        public string MeasurementUnit { get; set; }
        public string MeasurementValue { get; set; }
        public string Modified { get; set; }
        public string AssociatedMedia { get; set; }
        public string AssociatedOccurrences { get; set; }
        public string AssociatedReferences { get; set; }
        public string AssociatedSequences { get; set; }
        public string AssociatedTaxa { get; set; }
        public string Behavior { get; set; }
        public string CatalogNumber { get; set; }
        public string Disposition { get; set; }
        public string EstablishmentMeans { get; set; }
        public string IndividualCount { get; set; }
        public string IndividualID { get; set; }
        public string IsNaturalOccurrence { get; set; }
        public string IsNeverFoundObservation { get; set; }
        public string IsNotRediscoveredObservation { get; set; }
        public string IsPositiveObservation { get; set; }
        public string LifeStage { get; set; }
        public string OccurrenceID { get; set; }
        public string OccurrenceRemarks { get; set; }
        public string OccurrenceStatus { get; set; }
        public string OccurrenceURL { get; set; }
        public string OtherCatalogNumbers { get; set; }
        public string Preparations { get; set; }
        public string PreviousIdentifications { get; set; }
        public string Quantity { get; set; }
        public string QuantityUnit { get; set; }
        public string RecordedBy { get; set; }
        public string RecordNumber { get; set; }
        public string ReproductiveCondition { get; set; }
        public string Sex { get; set; }
        public string Substrate { get; set; }
        public string Owner { get; set; }
        public string OwnerInstitutionCode { get; set; }
        public string IsPublic { get; set; }
        public string ProjectCategory { get; set; }
        public string ProjectDescription { get; set; }
        public string ProjectEndDate { get; set; }
        public string ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string ProjectOwner { get; set; }
        public string ProjectStartDate { get; set; }
        public string ProjectURL { get; set; }
        public string SurveyMethod { get; set; }
        public string References { get; set; }
        public string ReportedBy { get; set; }
        public string RelatedResourceID { get; set; }
        public string RelationshipAccordingTo { get; set; }
        public string RelationshipEstablishedDate { get; set; }
        public string RelationshipOfResource { get; set; }
        public string RelationshipRemarks { get; set; }
        public string ResourceID { get; set; }
        public string ResourceRelationshipID { get; set; }
        public string Rights { get; set; }
        public string RightsHolder { get; set; }
        public string SpeciesObservationURL { get; set; }
        public string AcceptedNameUsage { get; set; }
        public string AcceptedNameUsageID { get; set; }
        public string Class { get; set; }
        public string DyntaxaTaxonID { get; set; }
        public string Family { get; set; }
        public string Genus { get; set; }
        public string HigherClassification { get; set; }
        public string InfraspecificEpithet { get; set; }
        public string Kingdom { get; set; }
        public string NameAccordingTo { get; set; }
        public string NameAccordingToID { get; set; }
        public string NamePublishedIn { get; set; }
        public string NamePublishedInID { get; set; }
        public string NamePublishedInYear { get; set; }
        public string NomenclaturalCode { get; set; }
        public string NomenclaturalStatus { get; set; }
        public string Order { get; set; }
        public string OrganismGroup { get; set; }
        public string OriginalNameUsage { get; set; }
        public string OriginalNameUsageID { get; set; }
        public string ParentNameUsage { get; set; }
        public string ParentNameUsageID { get; set; }
        public string Phylum { get; set; }
        public string ScientificName { get; set; }
        public string ScientificNameAuthorship { get; set; }
        public string ScientificNameID { get; set; }
        public string SpecificEpithet { get; set; }
        public string Subgenus { get; set; }
        public string TaxonConceptID { get; set; }
        public string TaxonConceptStatus { get; set; }
        public string TaxonID { get; set; }
        public string TaxonomicStatus { get; set; }
        public string TaxonRank { get; set; }
        public string TaxonRemarks { get; set; }
        public string TaxonSortOrder { get; set; }
        public string TaxonURL { get; set; }
        public string VerbatimScientificName { get; set; }
        public string VerbatimTaxonRank { get; set; }
        public string VernacularName { get; set; }
        public string Type { get; set; }
        public string ValidationStatus { get; set; }
        public List<ProjectViewModel> Projects { get; set; }
        public Dictionary<ProjectViewModel, List<Tuple<ProjectParameterObservationDetailFieldViewModel, string>>> ProjectParameterValuesDictionary
        { get; set; }

        private static List<ProjectViewModel> GetSampleProjects()
        {
            var projects = new List<ProjectViewModel>();
            var project1 = new ProjectViewModel
            {
                Name = "s projekt",
                ProjectParameters = new Dictionary<string, ProjectParameterObservationDetailFieldViewModel>                
                {
                    { "MyPropIdentifier1", new ProjectParameterObservationDetailFieldViewModel
                        {
                            PropertyIdentifier = "MyPropIdentifier1",
                            Name = "Parameter1",
                            Label = "Parameter1",
                            Value = "34"
                        }
                    },
                    { "MyPropIdentifier2", new ProjectParameterObservationDetailFieldViewModel
                        {
                            PropertyIdentifier = "MyPropIdentifier2",
                            Name = "Parameter2",
                            Label = "Parameter2",
                            Value = "Östra vägen"
                        }
                    }
                }
            };
            projects.Add(project1);

            var project2 = new ProjectViewModel
            {
                Name = "Test projekt",
                ProjectParameters = new Dictionary<string, ProjectParameterObservationDetailFieldViewModel>
                {
                    { "MyPropIdentifier5", new ProjectParameterObservationDetailFieldViewModel
                        {
                            PropertyIdentifier = "MyPropIdentifier5",
                            Name = "Parameter5",
                            Label = "Parameter5",
                            Value = "Ja"
                        }
                    }
                }
            };
            projects.Add(project2);

            return projects;
        }

        /// <summary>
        /// Creates a new object by copying data from an species observation object.
        /// </summary>
        /// <param name="obs">The species observation object.</param>
        /// <param name="speciesObservationFieldDescriptions">The species observation field descriptions.</param>
        /// <returns></returns>
        public static SpeciesObservationViewModel CreateFromSpeciesObservation(
            ISpeciesObservation obs,
            ISpeciesObservationFieldDescriptionsViewModel speciesObservationFieldDescriptions)
        {
            var model = new SpeciesObservationViewModel();           
            model.ObservationId = obs.Id.ToString(CultureInfo.InvariantCulture);
            model.CopyDataFromObservationToModel(obs, model, speciesObservationFieldDescriptions);           
            //model.Projects.AddRange(GetSampleProjects()); // used for testing purpose
            return model;
        }

        /// <summary>
        /// Gets a dictionary containing all properties of the ObservationDarwinCoreViewModel type.
        /// Key=PropertyInfo.Name
        /// </summary>
        private static Dictionary<string, PropertyInfo> ObservationDarwinCoreViewModelPropertiesDictionary
        {
            get
            {
                if (_observationDarwinCoreViewModelPropertiesDictionary == null)
                {
                    _observationDarwinCoreViewModelPropertiesDictionary = typeof(SpeciesObservationViewModel).GetProperties().ToDictionary(m => m.Name);
                }

                return _observationDarwinCoreViewModelPropertiesDictionary;
            }
        }
        private static Dictionary<string, PropertyInfo> _observationDarwinCoreViewModelPropertiesDictionary;

        /// <summary>
        /// Copies the data from observation to model.
        /// The observation object contains hierarchical data, while the model is flat.
        /// </summary>
        /// <param name="observation">The observation.</param>
        /// <param name="model">The model.</param>
        /// <param name="speciesObservationFieldDescriptions"></param>
        private void CopyDataFromObservationToModel(
            ISpeciesObservation observation, 
            SpeciesObservationViewModel model, 
            ISpeciesObservationFieldDescriptionsViewModel speciesObservationFieldDescriptions)
        {                                                
            // Create detalis list out of propery and resource string
            Type type = observation.GetType();
            PropertyInfo[] propInfos = type.GetProperties();
            for (int i = 0; i < propInfos.Length; i++)
            {
                PropertyInfo pi = (PropertyInfo)propInfos.GetValue(i);
                string propertyName = pi.Name;
                object propValue = pi.GetValue(observation, null);
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

                    CopyDataFromSubProperty(observation, subSpeciesPropertyInfos, propValue, model);
                }
                else
                {
                    //Get string value for property
                    string propertyValue = GetPropertyValueAsString(propValue);
                    
                    PropertyInfo modelPi;
                    if (ObservationDarwinCoreViewModelPropertiesDictionary.TryGetValue(propertyName, out modelPi))
                    {
                        modelPi.SetValue(model, propertyValue, null);
                    }                    
                }
            }

            CopyProjectParametersToModel(observation, model, speciesObservationFieldDescriptions);
        }

        /// <summary>
        /// Copies the project parameters to model.
        /// </summary>
        /// <param name="observation">The observation.</param>
        /// <param name="model">The model.</param>
        /// <param name="speciesObservationFieldDescriptions"></param>
        private void CopyProjectParametersToModel(
            ISpeciesObservation observation, 
            SpeciesObservationViewModel model, 
            ISpeciesObservationFieldDescriptionsViewModel speciesObservationFieldDescriptions)
        {
            model.Projects = new List<ProjectViewModel>();
            if (observation.Project == null || observation.Project.ProjectParameters == null)
            {
                return;
            }            

            var projectParametersDictionary = new Dictionary<int, List<ISpeciesObservationProjectParameter>>();
            foreach (var projectParameter in observation.Project.ProjectParameters)
            {
                if (!projectParametersDictionary.ContainsKey(projectParameter.ProjectId))
                {
                    projectParametersDictionary.Add(projectParameter.ProjectId, new List<ISpeciesObservationProjectParameter>());
                }

                projectParametersDictionary[projectParameter.ProjectId].Add(projectParameter);                    
            }

            // loop all projects
            foreach (var pair in projectParametersDictionary)
            {
                ProjectViewModel projectViewModel;
                if (!speciesObservationFieldDescriptions.ProjectsDictionary.TryGetValue(pair.Key, out projectViewModel))
                {
                    projectViewModel = CreateProjectViewModel(pair);                    
                }
                else
                {
                    projectViewModel = projectViewModel.Clone();
                    // map values
                    foreach (var projectParameter in pair.Value)
                    {
                        projectViewModel.ProjectParameters[projectParameter.PropertyIdentifier].Value = projectParameter.Value;
                    }
                }

                model.Projects.Add(projectViewModel);
            }
        }

        private static ProjectViewModel CreateProjectViewModel(KeyValuePair<int, List<ISpeciesObservationProjectParameter>> pair)
        {
            var projectViewModel = new ProjectViewModel();
            var firstProjectParameter = pair.Value.First();
            projectViewModel.Name = firstProjectParameter.ProjectName;
            projectViewModel.Id = firstProjectParameter.ProjectId;
            projectViewModel.Guid = firstProjectParameter.Guid;
            projectViewModel.ProjectParameters = new Dictionary<string, ProjectParameterObservationDetailFieldViewModel>();

            foreach (var projectParameter in pair.Value)
            {
                var field = ProjectParameterObservationDetailFieldViewModel.Create(projectParameter);
                projectViewModel.ProjectParameters.Add(field.PropertyIdentifier, field);
            }

            return projectViewModel;
        }

        /// <summary>
        /// Gets all string properties from the Resources.Resource file.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetAllStringPropertiesFromResourceFile()
        {
            IEnumerable<PropertyInfo> properties = typeof(Resources.Resource).GetProperties()
                                                    .Where(p => p.PropertyType == typeof(string));
            var dicStringProperties = new Dictionary<string, string>();

            foreach (PropertyInfo p in properties)
            {
                string strValue = (string)p.GetValue(null, null);
                dicStringProperties.Add(p.Name, strValue);
            }
            return dicStringProperties;
        }

        /// <summary>
        /// Performe property check
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
            else if (value is Int64)
            {
                propertyValue = Convert.ToString((Int64)value);
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

            return propertyValue;
        }

        private static void CopyDataFromSubProperty(
            ISpeciesObservation speciesObservation, 
            PropertyInfo[] propertyInfos,
            object value, 
            SpeciesObservationViewModel model)
        {            
            for (int j = 0; j < propertyInfos.Length; j++)
            {
                PropertyInfo speciesSubPropertyInfo = propertyInfos[j];                
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
                string propertyValue = GetPropertyValueAsString(propValue);                
                PropertyInfo modelPi;
                if (ObservationDarwinCoreViewModelPropertiesDictionary.TryGetValue(propertyName, out modelPi))
                {
                    modelPi.SetValue(model, propertyValue, null);
                }
            }
        }
    }
}
