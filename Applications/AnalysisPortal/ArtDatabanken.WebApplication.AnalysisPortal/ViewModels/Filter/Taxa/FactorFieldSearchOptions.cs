using System;
using System.Web.Mvc;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Filter.Taxa
{
    /// <summary>
    /// This class contains search settings to use in Taxa search.
    /// </summary>
    public class FactorFieldSearchOptions
    {        
        public String UniqueId { get; set; }

        public StringCompareOperator? StringValueCompareOperator { get; set; }
        
        public NumberCompareOperator? NumberValueCompareOperator { get; set; }
        
        public SelectList CreateStringValueCompareOperatorSelectlist(StringCompareOperator? op)
        {
            const StringCompareOperator defaultOperator = StringCompareOperator.BeginsWith;

            return new SelectList(
                new[] 
                {
                    new SelectListItem() { Text = Labels.CompareOpBeginsWithLabel, Value = StringCompareOperator.BeginsWith.ToString() },
                    new SelectListItem() { Text = Labels.CompareOpContainsLabel, Value = StringCompareOperator.Contains.ToString() },
                    new SelectListItem() { Text = Labels.CompareOpEndsWithLabel, Value = StringCompareOperator.EndsWith.ToString() },
                    new SelectListItem() { Text = Labels.CompareOpEqualLabel, Value = StringCompareOperator.Equal.ToString() },
                    new SelectListItem() { Text = Labels.CompareOpLikeLabel, Value = StringCompareOperator.Like.ToString() }
                }, 
                "Value", 
                "Text", 
                op.HasValue ? op.Value.ToString() : defaultOperator.ToString());
        }

        public SelectList CreateNumberValueCompareOperatorSelectlist(NumberCompareOperator? op)
        {
            const NumberCompareOperator defaultOperator = NumberCompareOperator.Equal;

            return new SelectList(
                new[] 
                {
                    new SelectListItem() { Text = Labels.CompareOpGreaterLabel, Value = NumberCompareOperator.Greater.ToString() },
                    new SelectListItem() { Text = Labels.CompareOpGreaterOrEqualLabel, Value = NumberCompareOperator.GreaterOrEqual.ToString() },
                    new SelectListItem() { Text = Labels.CompareOpLessLabel, Value = NumberCompareOperator.Less.ToString() },
                    new SelectListItem() { Text = Labels.CompareOpLessOrEqualLabel, Value = NumberCompareOperator.LessOrEqual.ToString() },
                    new SelectListItem() { Text = Labels.CompareOpNotEqualLabel, Value = NumberCompareOperator.NotEqual.ToString() },
                    new SelectListItem() { Text = Labels.CompareOpEqualLabel, Value = NumberCompareOperator.Equal.ToString() }
                }, 
                "Value", 
                "Text", 
                op.HasValue ? op.Value.ToString() : defaultOperator.ToString());
        }

        private readonly ModelLabels _labels = new ModelLabels();

        /// <summary>
        /// All localized labels
        /// </summary>
        public ModelLabels Labels
        {
            get { return _labels; }
        }

        public class ModelLabels
        {
            public string CompareOpBeginsWithLabel { get { return Resources.Resource.TaxonSearchCompareOpBeginsWith; } }
            public string CompareOpContainsLabel { get { return Resources.Resource.TaxonSearchCompareOpContains; } }
            public string CompareOpEndsWithLabel { get { return Resources.Resource.TaxonSearchCompareOpEndsWith; } }
            public string CompareOpEqualLabel { get { return Resources.Resource.TaxonSearchCompareOpEqual; } }
            public string CompareOpIterativeLabel { get { return Resources.Resource.TaxonSearchCompareOpIterative; } }
            public string CompareOpLikeLabel { get { return Resources.Resource.TaxonSearchCompareOpLike; } }
            public string CompareOpNotEqualLabel { get { return Resources.Resource.TaxonSearchCompareOpNotEqual; } }

            public string CompareOpGreaterLabel { get { return Resources.Resource.TaxonSearchCompareOpGreater; } }
            public string CompareOpGreaterOrEqualLabel { get { return Resources.Resource.TaxonSearchCompareOpGreaterOrEqual; } }
            public string CompareOpLessLabel { get { return Resources.Resource.TaxonSearchCompareOpLess; } }
            public string CompareOpLessOrEqualLabel { get { return Resources.Resource.TaxonSearchCompareOpLessOrEqual; } }
        }
    }
}
