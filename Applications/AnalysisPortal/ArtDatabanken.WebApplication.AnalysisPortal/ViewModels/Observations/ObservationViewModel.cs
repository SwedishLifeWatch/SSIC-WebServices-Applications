using System;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Observations
{
    public class ObservationViewModel
    {
        public String ObservationId { get; set; }
        public String OrganismGroup { get; set; }
        public bool ShowOrganismGroup { get; set; }
        public String ScientificName { get; set; }
        public String CommonName { get; set; }
        public String TaxonConceptStatus { get; set; }
        public String RedlistCategory { get; set; }
        public String StartDate { get; set; }
        public String EndDate { get; set; }
        public String Locality { get; set; }
        public String Parish { get; set; }
        public String Municipality { get; set; }
        public String County { get; set; }
        public String StateProvince { get; set; }
        public String CoordinateNorth { get; set; }
        public String CoordinateEast { get; set; }
        public String Accurancy { get; set; }
        public String RecordedBy { get; set; }
        public String Owner { get; set; }
        public String Quantity { get; set; }
        public String QuantityUnit { get; set; }
        public String LifeStage { get; set; }
        public String Behaviour { get; set; }
        public String Substrate { get; set; }
        public String OcurranceRemarks { get; set; }
        public String DetConf { get; set; }
        public String CollectionCode { get; set; }
        public String IsNeverFoundObservarion { get; set; }
        public String IsNotRediscoveredObservation { get; set; }
        public String DatasetID { get; set; }
        public String Database { get; set; }
        public String TaxonSortOrder { get; set; }
        public String TaxonId { get; set; }
        public String GroupSorting { get; set; }
        public String ProtectionLevel { get; set; }

        public static ObservationViewModel Create(ISpeciesObservation obs)
        {
            var model = new ObservationViewModel();
            model.ObservationId = Convert.ToString(obs.Id);
            model.OrganismGroup = obs.Taxon.OrganismGroup;
           
            model.ShowOrganismGroup = true;
            model.ScientificName = obs.Taxon.ScientificName;
            model.CommonName = obs.Taxon.VernacularName;
            model.TaxonConceptStatus = obs.Taxon.TaxonConceptStatus;
            model.RedlistCategory = obs.Conservation.RedlistCategory;
            string startTime = string.Empty;
            if (obs.Event.Start != null)
            {
                startTime = obs.Event.Start.Value.ToShortDateString();
            }
            model.StartDate = startTime;
            string endTime = string.Empty;
            if (obs.Event.End != null)
            {
                endTime = obs.Event.End.Value.ToShortDateString();
            }
            model.EndDate = endTime;
            model.Locality = obs.Location.Locality;
            model.Parish = obs.Location.Parish;
            model.Municipality = obs.Location.Municipality;
            model.County = obs.Location.County;
            model.StateProvince = obs.Location.StateProvince;
            string coordinateY = string.Empty;
            if (obs.Location.CoordinateY.IsNotNull())
            {
                coordinateY = Convert.ToString(obs.Location.CoordinateY);
            }
            model.CoordinateNorth = coordinateY;
            string coordinateX = string.Empty;
            if (obs.Location.CoordinateX.IsNotNull())
            {
                coordinateX = Convert.ToString(obs.Location.CoordinateX);
            }
            model.CoordinateEast = coordinateX;
            model.Accurancy = obs.Location.CoordinateUncertaintyInMeters;
            model.RecordedBy = obs.Occurrence.RecordedBy;
            model.Owner = obs.Owner;
            model.Quantity = obs.Occurrence.Quantity;
            model.QuantityUnit = obs.Occurrence.QuantityUnit;
            model.LifeStage = obs.Occurrence.LifeStage;
            model.Behaviour = obs.Occurrence.Behavior;
            model.Substrate = ""; //obs.Occurrence.Substrate;

            model.OcurranceRemarks = obs.Occurrence.OccurrenceRemarks;
            model.DetConf = "";
            model.CollectionCode = obs.CollectionCode;
            string isNeverFound = string.Empty;
            if (obs.Occurrence.IsNeverFoundObservation.IsNotNull())
            {
                isNeverFound = Convert.ToString(obs.Occurrence.IsNeverFoundObservation);
            }
            model.IsNeverFoundObservarion = isNeverFound;

            string isNotRediscovered = string.Empty;
            if (obs.Occurrence.IsNotRediscoveredObservation.IsNotNull())
            {
                isNotRediscovered = Convert.ToString(obs.Occurrence.IsNotRediscoveredObservation);
            }
            model.IsNotRediscoveredObservation = isNotRediscovered;
            model.DatasetID = obs.DatasetID;
            model.Database = obs.DatasetName;
            string taxonSortOrder = string.Empty;
            if (obs.Taxon.TaxonSortOrder.IsNotNull())
            {
                taxonSortOrder = Convert.ToString(obs.Taxon.TaxonSortOrder);
            }
            model.TaxonSortOrder = taxonSortOrder;
            model.TaxonId = obs.Taxon.TaxonID;
            model.GroupSorting = "";
            string protectionLevel = string.Empty;
            if (obs.Conservation.ProtectionLevel.IsNotNull())
            {
                protectionLevel = Convert.ToString(obs.Conservation.ProtectionLevel);
            }
            model.ProtectionLevel = protectionLevel;
           
            return model;
        }

        ///// <summary>
        ///// Returns a path to an icon representing the taxon status
        ///// </summary>        
        //public string GetStatusImageUrl()
        //{
        //    switch (TaxonStatus)
        //    {
        //        case TaxonAlertLevel.Green:
        //            return "~/Images/info_green.png";
        //        case TaxonAlertLevel.Yellow:
        //            return "~/Images/info_yellow.png";
        //        case TaxonAlertLevel.Red:
        //            return "~/Images/info_red.png";
        //        default:
        //            throw new ArgumentOutOfRangeException();
        //    }
        //}
    }
}
