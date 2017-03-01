using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.TaxonName
{
    public class TaxonNameInfoViewModel
    {        
        public string GUID { get; set; }
        public string Comment { get; set; }
        public string Name { get; set; }
        public string NameCategory { get; set; }
        public string NameUsage { get; set; }
        public string NameStatus { get; set; }
        public bool IsUnique { get; set; }
        public bool IsRecommended { get; set; }
        public bool IsOriginal { get; set; }
        public bool IsOkForSpeciesObservation { get; set; }
        public string Author { get; set; }
        public string Modified { get; set; }

        public string ReferenceLabel
        {
            get { return Resources.DyntaxaResource.TaxonNameSharedReferences; }
        }

        public string AuthorLabel
        {
            get { return Resources.DyntaxaResource.TaxonNameInfoAuthorLabel; }
        }

        public string GUIDLabel
        {
            get { return Resources.DyntaxaResource.TaxonNameInfoGUIDLabel; }
        }

        public string ModifiedLabel
        {
            get { return Resources.DyntaxaResource.TaxonNameInfoModifiedLabel; }
        }

        public string NameLabel
        {
            get { return Resources.DyntaxaResource.TaxonNameInfoNameLabel; }
        }

        public string NameCategoryLabel
        {
            get { return Resources.DyntaxaResource.TaxonNameInfoNameCategoryLabel; }
        }

        public string CommentLabel
        {
            get { return Resources.DyntaxaResource.TaxonNameInfoCommentLabel; }
        }

        public string NameNameUsageLabel
        {
            get { return Resources.DyntaxaResource.TaxonNameSharedNameUsage; }
        }

        public string NameNomenclatureLabel
        {
            get { return Resources.DyntaxaResource.TaxonNameSharedNomenclature; }
        }

        public string IsUniqueLabel
        {
            get { return Resources.DyntaxaResource.TaxonNameInfoIsUniqueLabel; }
        }

        public string OkForObsSystemLabel
        {
            get { return Resources.DyntaxaResource.TaxonNameInfoOkForObsSystemLabel; }
        }
        
        public string IsRecommendedLabel
        {
            get { return Resources.DyntaxaResource.TaxonNameInfoIsRecommendedLabel; }
        }

        public string IsOriginalLabel
        {
            get { return Resources.DyntaxaResource.TaxonNameInfoIsOriginalLabel; }
        }        
    }
}
