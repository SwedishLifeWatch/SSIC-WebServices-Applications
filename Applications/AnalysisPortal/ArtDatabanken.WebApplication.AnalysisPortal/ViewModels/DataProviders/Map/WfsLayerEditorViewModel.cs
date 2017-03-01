using System.Collections.Generic;
using ArtDatabanken.GIS.WFS.Capabilities;
using ArtDatabanken.GIS.WFS.DescribeFeature;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings.DataProviders.MapLayers;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Labels;
using Newtonsoft.Json;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.DataProviders.Map
{
    /// <summary>
    /// Used by DataProviders/WfsLayerEditor.
    /// Decides if the view is in "create new" mode or "edit existing layer" mode
    /// </summary>
    public enum WfsLayerEditorMode
    {
        New = 0,
        Edit = 1
    }

    public enum WmsLayerEditorMode
    {
        New = 0,
        Edit = 1
    }

    public class WmsLayerEditorViewModel
    {
        public WmsLayerEditorMode Mode { get; set; }
        public int? Id { get; set; }
        public WmsLayerViewModel WmsLayerViewModel { get; set; }        
    }

    /// <summary>
    /// This class is a view model for the DataProviders/WfsLayerEditor action
    /// </summary>
    public class WfsLayerEditorViewModel
    {
        public WFSCapabilities WfsCapabilities { get; set; }
        public WfsFeatureType FeatureType { get; set; }
        public string ServerUrl { get; set; }        
        public WfsLayerEditorMode Mode { get; set; }
        public int? Id { get; set; }
        public WfsLayerSetting WfsLayerSetting { get; set; }

        /// <summary>
        /// Gets all layer fields except the geometry field as a JSON string.
        /// The data type is set to ""
        /// </summary>
        /// <returns></returns>
        public string GetLayerFieldsAsJsonString()
        {
            if (FeatureType == null || FeatureType.DescribeFeatureType == null || FeatureType.DescribeFeatureType.Fields == null)
            {
                return JsonConvert.SerializeObject(string.Empty);
            }

            var list = new List<KeyValuePair<string, string>>();
            
            foreach (Field field in FeatureType.DescribeFeatureType.Fields)
            {
                if (field != FeatureType.DescribeFeatureType.GeometryField)
                {
                    list.Add(new KeyValuePair<string, string>(field.Name, ""));
                }
            }

            return JsonConvert.SerializeObject(list);           
        }

        public SharedLabels SharedLabels
        {
            get { return SharedLabels.Instance; }
        }

        /// <summary>
        /// Gets the model labels.
        /// </summary>
        public ModelLabels Labels
        {
            get
            {
                if (_labels == null)
                {
                    _labels = new ModelLabels();
                }

                return _labels;
            }
        }
        private ModelLabels _labels;

        /// <summary>
        /// This class holds localized labels for the TaxaSetting class.
        /// </summary>
        public class ModelLabels
        {
            public string Title { get { return Resources.Resource.DataSourcesWfsLayerEditorTitle; } }
            public string HeaderName { get { return Resources.Resource.DataSourcesWfsLayerEditorHeaderName; } }
            public string HeaderColor { get { return Resources.Resource.DataSourcesWfsLayerEditorHeaderColor; } }
            public string HeaderTitle { get { return Resources.Resource.DataSourcesWfsLayerEditorHeaderTitle; } }
            public string HeaderServerUrl { get { return Resources.Resource.DataSourcesWfsLayerEditorHeaderServerUrl; } }
            public string HeaderAbstract { get { return Resources.Resource.DataSourcesWfsLayerEditorHeaderAbstract; } }
            public string HeaderKeywords { get { return Resources.Resource.DataSourcesWfsLayerEditorHeaderKeywords; } }
            public string HeaderMetadataUrl { get { return Resources.Resource.DataSourcesWfsLayerEditorHeaderMetadataUrl; } }
            public string HeaderSrs { get { return Resources.Resource.DataSourcesWfsLayerEditorHeaderSrs; } }
            public string HeaderFormats { get { return Resources.Resource.DataSourcesWfsLayerEditorHeaderFormats; } }
            public string Filter { get { return Resources.Resource.DataSourcesWfsLayerEditorFilter; } }
            public string Operator { get { return Resources.Resource.DataSourcesWfsLayerEditorOperator; } }
            public string GreaterThanOperator { get { return Resources.Resource.DataSourcesWfsLayerEditorGreaterThanOperator; } }
            public string LessThanOperator { get { return Resources.Resource.DataSourcesWfsLayerEditorLessThanOperator; } }
            public string GreatorOrEqualToOperator { get { return Resources.Resource.DataSourcesWfsLayerEditorGreatorOrEqualToOperator; } }
            public string LessOrEqualToOperator { get { return Resources.Resource.DataSourcesWfsLayerEditorLessOrEqualToOperator; } }
            public string NotEqualToOperator { get { return Resources.Resource.DataSourcesWfsLayerEditorNotEqualToOperator; } }
            public string EqualToOperator { get { return Resources.Resource.DataSourcesWfsLayerEditorEqualToOperator; } }
            public string LikeOperator { get { return Resources.Resource.DataSourcesWfsLayerEditorLikeOperator; } }
            public string IsNullOperator { get { return Resources.Resource.DataSourcesWfsLayerEditorIsNullOperator; } }
            public string Constant { get { return Resources.Resource.DataSourcesWfsLayerEditorConstant; } }
            public string LeftOperand { get { return Resources.Resource.DataSourcesWfsLayerEditorLeftOperand; } }
            public string RightOperand { get { return Resources.Resource.DataSourcesWfsLayerEditorRightOperand; } }
            public string Field { get { return Resources.Resource.DataSourcesWfsLayerEditorField; } }
            public string AddOperation { get { return Resources.Resource.DataSourcesWfsLayerEditorAddOperation; } }
            public string AndOperation { get { return Resources.Resource.DataSourcesWfsLayerEditorAndOperation; } }
            public string OrOperation { get { return Resources.Resource.DataSourcesWfsLayerEditorOrOperation; } }
            public string NotOperation { get { return Resources.Resource.DataSourcesWfsLayerEditorNotOperation; } }
            public string Undo { get { return Resources.Resource.DataSourcesWfsLayerEditorUndo; } }
            public string Reset { get { return Resources.Resource.DataSourcesWfsLayerEditorReset; } }
            public string MaxNumberOfFeatures { get { return Resources.Resource.DataSourcesWfsLayerEditorMaxNumberOfFeatures; } }
            public string RunQuery { get { return Resources.Resource.DataSourcesWfsLayerEditorRunQuery; } }
            public string NumberOfFeaturedFeatures { get { return Resources.Resource.DataSourcesWfsLayerEditorNumberOfFeaturedFeatures; } }
            public string TotalNumberOfFeatures { get { return Resources.Resource.DataSourcesWfsLayerEditorTotalNumberOfFeatures; } }
            public string Back { get { return Resources.Resource.DataSourcesWfsLayerEditorBack; } }
            public string SaveChanges { get { return Resources.Resource.DataSourcesWfsLayerEditorSaveChanges; } }
            public string CreateNewLayer { get { return Resources.Resource.DataSourcesWfsLayerEditorCreateNewLayer; } }
            public string EnterConstant { get { return Resources.Resource.DataSourcesWfsLayerEditorEnterConstant; } }
            public string BasicInformation { get { return Resources.Resource.DataSourcesWfsLayerEditorBasicInformation; } }
            public string Result { get { return Resources.Resource.DataSourcesWfsLayerEditorResult; } }
            public string LayerData { get { return Resources.Resource.DataSourcesWfsLayerEditorLayerData; } }
            public string ErrorEnterLayerName { get { return Resources.Resource.DataSourcesWfsLayerEditorErrorEnterLayerName; } }

            //public string Title = "WFS layer editor";
            //public string HeaderName = "Name";
            //public string HeaderColor = "Color";
            //public string HeaderTitle = "Title";
            //public string HeaderServerUrl = "Server URL";
            //public string HeaderAbstract="Abstract";
            //public string HeaderKeywords = "Keywords";
            //public string HeaderMetadataUrl = "Metadata URL";
            //public string HeaderSrs = "Srs";
            //public string HeaderFormats = "Formats";
            //public string Filter="Filter";
            //public string Operator = "Operator";
            //public string GreaterThanOperator="Greater than (>)";
            //public string LessThanOperator = "Less than (<)";
            //public string GreatorOrEqualToOperator = "Greater or equal to (>=)";
            //public string LessOrEqualToOperator = "Less or equal to (<=)";
            //public string NotEqualToOperator = "Not equal to (<>)";
            //public string EqualToOperator = "EqualTo (=)";
            //public string LikeOperator = "Like";
            //public string IsNullOperator = "Is null";
            //public string Constant = "Constant";
            //public string LeftOperand = "Left operand";
            //public string RightOperand = "Right operand";
            //public string Field= "Field";
            //public string AddOperation="Add";
            //public string AndOperation = "And";
            //public string OrOperation = "Or";
            //public string NotOperation="Not";
            //public string Undo = "Undo";
            //public string Reset = "Reset";
            //public string MaxNumberOfFeatures = "Max number of features";
            //public string RunQuery = "Run query";
            //public string NumberOfFeaturedFeatures = "Number of filtered features";
            //public string TotalNumberOfFeatures = "Total number of features";
            //public string Back = "Back";
            //public string SaveChanges = "Save changes";
            //public string CreateNewLayer = "Create new layer";
            //public string EnterConstant = "Enter constant";
            //public string BasicInformation = "Basic information";
            //public string Result = "Result";
            //public string LayerData = "Layer data";
            //public string ErrorEnterLayerName = "You must enter a layer name";
        }
    }
}
