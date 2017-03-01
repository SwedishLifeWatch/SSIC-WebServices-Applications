using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ArtDatabanken.Data;
using Resources;
using Newtonsoft.Json;
using System.Web.Routing;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Reference
{
    /// <summary>
    /// View model for adding and removing reference relations
    /// </summary>
    public class ReferenceAddViewModel
    {
        public string Guid { get; set; }
        public List<ReferenceViewModel> SelectedReferences { get; set; }
        public string ReferenceTypesSelectBoxString { get; set; }

        public string ReturnController { get; set; }
        public string ReturnAction { get; set; }
        public string ReturnParameters { get; set; }
        public int? TaxonId { get; set; }
        //        public RouteValueDictionary LinkParams { get; set; }        

        public List<IReferenceRelationType> ReferenceTypes { get; set; }

        public string GetSelectedReferencesAsJSON()
        {
            string strJson = JsonConvert.SerializeObject(SelectedReferences);
            return strJson;
        }

        public string GetReferenceTypesJavascriptArray()
        {
            List<IReferenceRelationType> referenceTypes = this.ReferenceTypes;
            var sb = new StringBuilder();
            sb.Append("[");
            for (int i = 0; i < referenceTypes.Count; i++)
            {
                var referenceType = referenceTypes[i];
                sb.AppendFormat("{{ Id: {0}, Name: \"{1}\"}}", referenceType.Id, HttpUtility.HtmlEncode(referenceType.Description));
                if (i < referenceTypes.Count - 1)
                {
                    sb.Append(", ");
                }
            }
            sb.Append("]");            
            return sb.ToString();
        }

        public string GetReferenceTypesSelectBoxString(int usageTypeId)
        {
            List<IReferenceRelationType> referenceTypes = this.ReferenceTypes;
            bool foundUsageType = referenceTypes.Any(referenceType => referenceType.Id == usageTypeId);

            var sb = new StringBuilder();
            sb.Append("<select name=\"usageType\" style=\"width: 100%\">");
            sb.AppendFormat("<option value=-1 {1}>{0}</option>", HttpUtility.HtmlEncode(Resources.DyntaxaResource.ReferenceAddChooseType), foundUsageType ? "" : "selected=\"selected\"");
            foreach (var referenceRelationType in referenceTypes)
            {
                sb.AppendFormat(
                    "<option value=\"{0}\" {2}>{1}</option>", 
                    referenceRelationType.Id,
                    HttpUtility.HtmlEncode(referenceRelationType.Description), 
                    referenceRelationType.Id == usageTypeId ? "selected=\"selected\"" : "");
            }
            sb.Append("</select>");
            return sb.ToString();          
        }

        public ModelLabels Labels
        {
            get { return _labels; }
        }

        public RouteValueDictionary RouteValues { get; set; }
        public bool ShowReferenceApplyMode { get; set; }

        private readonly ModelLabels _labels = new ModelLabels();

        /// <summary>
        /// Localized labels
        /// </summary>
        public class ModelLabels
        {
            public string CreateNewReferenceLabel { get { return DyntaxaResource.ReferenceAddCreateNewReference; } }            
            public string TitleLabel { get { return DyntaxaResource.ReferenceAddTitle; } }

            public string SearchReferencesLabel { get { return DyntaxaResource.ReferenceAddSearchReferences; } }
            public string AddButtonLabel { get { return DyntaxaResource.ReferenceAddButtonAdd; } }

            public string ColumnTitleType { get { return DyntaxaResource.ReferenceAddType; } }
            public string ColumnTitleId { get { return DyntaxaResource.ReferenceAddId; } }
            public string ColumnTitleName { get { return DyntaxaResource.ReferenceAddName; } }
            public string ColumnTitleYear { get { return DyntaxaResource.ReferenceAddYear; } }
            public string ColumnTitleText { get { return DyntaxaResource.ReferenceAddText; } }
            public string ColumnTitleUsage { get { return DyntaxaResource.ReferenceAddUsage; } }
            public string ColumnTitleUsageTypeId { get { return DyntaxaResource.ReferenceAddUsageTypeId; } }

            public string SearchLabel { get { return DyntaxaResource.ReferenceAddSearchLabel; } }
            public string NumberOfFilteredElementsLabel { get { return DyntaxaResource.ReferenceAddNumberOfFilteredElementsLabel; } }
            public string NoDataAvailableLabel { get { return DyntaxaResource.ReferenceAddNoDataAvailableLabel; } }
            public string FilteringLabel { get { return DyntaxaResource.ReferenceAddFilteringLabel; } }
            public string NoRecordsLabel { get { return DyntaxaResource.ReferenceAddNoRecordsLabel; } }
            public string ReferencesLabel { get { return DyntaxaResource.ReferenceAddReferences; } }

            public string SharedDialogInformationHeader { get { return DyntaxaResource.SharedDialogInformationHeader; } }
        }
    }
}