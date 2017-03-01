using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// List class for the ITaxonName interface.
    /// </summary>
    public class TaxonNameList : DataId32List<ITaxonName>
    {
        /// <summary>
        /// Return TaxonName by version
        /// </summary>
        /// <param name="version"></param>
        /// <returns>TaxonName object with requested version number</returns>
        public ITaxonName GetByVersion(Int32 version)
        {
            foreach (ITaxonName name in this)
            {
                if (name.Version == version)
                {
                    // Data found. Return it.
                    return name;
                }
            }

            // No data found with requested version
            throw new ArgumentException("No data with id (version) " + version + "!");
        }

        /// <summary>
        /// Holds taxon names for names of type other recommended.
        /// </summary>
       public List<ITaxonName> OtherRecommendedNames { get; set; }

       /// <summary>
       /// Holds taxon names for names of type other synonymes.
       /// </summary>
        public List<ITaxonName> OtherSynonyms { get; set; }

        /// <summary>
        /// Holds taxon names for names of type other names.
        /// </summary>
        public List<ITaxonName> OtherNames { get; set; }

        /// <summary>
        /// Holds taxon names for names of type secientific synonymes.
        /// </summary>
        public List<ITaxonName> ScientificSynonyms { get; set; }

        /// <summary>
        /// Holds taxon names for names of type common synonymes.
        /// </summary>
        public List<ITaxonName> CommonSynonyms { get; set; }

        /// <summary>
        /// Holds taxon names for names of identifies.
        /// </summary>
        public List<ITaxonName> Identifiers { get; set; }

        /// <summary>
        /// Holds information on original name
        /// </summary>
        public string OriginalName { get; set; }

       /// <summary>
       /// Holds information on original author
       /// </summary>
        public string OriginalAuthor { get; set; }

        /// <summary>
        /// Holds information on anamorph name
        /// </summary>
        public string AnamorphName { get; set; }

        /// <summary>
        ///  Holds information on anamorph author
        /// </summary>
        public string AnamorphAuthor { get; set; }


        /// <summary>
        /// Method that group ie sort taxon names into different categories.
        /// </summary>
        public void GroupTaxonNameByCategories()
        {
            OtherRecommendedNames = new List<ITaxonName>();
            OtherSynonyms = new List<ITaxonName>();
            OtherNames = new List<ITaxonName>();
            ScientificSynonyms = new List<ITaxonName>();
            CommonSynonyms = new List<ITaxonName>();
            Identifiers = new List<ITaxonName>();

            foreach (ITaxonName taxonName in this)
            {
            

                    if (taxonName.Status.Id != (Int32)(TaxonNameStatusId.ApprovedNaming))
                    {
                        this.OtherNames.Add(taxonName);
                    }

                    switch ((TaxonNameCategoryId)(taxonName.Category.Id))
                    {
                        case TaxonNameCategoryId.ScientificName:
                            if (!taxonName.IsRecommended && taxonName.Status.Id == (Int32)(TaxonNameStatusId.ApprovedNaming))
                            {
                                this.ScientificSynonyms.Add(taxonName);
                            }

                            break;

                        case TaxonNameCategoryId.SwedishName:
                            if (!taxonName.IsRecommended && taxonName.Status.Id == (Int32)(TaxonNameStatusId.ApprovedNaming))
                            {
                                this.CommonSynonyms.Add(taxonName);
                            }

                            break;

                        case TaxonNameCategoryId.EnglishName:
                            if (taxonName.IsRecommended)
                            {
                                this.OtherRecommendedNames.Add(taxonName);
                            }
                            else
                            {
                                if (taxonName.Status.Id == (Int32)(TaxonNameStatusId.ApprovedNaming))
                                {
                                    this.OtherSynonyms.Add(taxonName);
                                }
                            }
                            break;

                        case TaxonNameCategoryId.DanishName:
                            if (taxonName.IsRecommended)
                            {
                                this.OtherRecommendedNames.Add(taxonName);
                            }
                            else
                            {
                                if (taxonName.Status.Id == (Int32)(TaxonNameStatusId.ApprovedNaming))
                                {
                                    this.OtherSynonyms.Add(taxonName);
                                }
                            }
                            break;

                        case TaxonNameCategoryId.NorwegianName:
                            if (taxonName.IsRecommended)
                            {
                                this.OtherRecommendedNames.Add(taxonName);
                            }
                            else
                            {
                                if (taxonName.Status.Id == (Int32)(TaxonNameStatusId.ApprovedNaming))
                                {
                                    this.OtherSynonyms.Add(taxonName);
                                }
                            }
                            break;

                        case TaxonNameCategoryId.FinnishName:
                            if (taxonName.IsRecommended)
                            {
                                this.OtherRecommendedNames.Add(taxonName);
                            }
                            else
                            {
                                if (taxonName.Status.Id == (Int32)(TaxonNameStatusId.ApprovedNaming))
                                {
                                    this.OtherSynonyms.Add(taxonName);
                                }
                            }
                            break;

                        case TaxonNameCategoryId.IcelandicName:
                            if (taxonName.IsRecommended)
                            {
                                this.OtherRecommendedNames.Add(taxonName);
                            }
                            else
                            {
                                if (taxonName.Status.Id == (Int32)(TaxonNameStatusId.ApprovedNaming))
                                {
                                    this.OtherSynonyms.Add(taxonName);
                                }
                            }
                            break;

                        case TaxonNameCategoryId.AmericanEnglishName:
                            if (taxonName.IsRecommended)
                            {
                                this.OtherRecommendedNames.Add(taxonName);
                            }
                            else
                            {
                                if (taxonName.Status.Id == (Int32)(TaxonNameStatusId.ApprovedNaming))
                                {
                                    this.OtherSynonyms.Add(taxonName);
                                }
                            }
                            break;

                        case TaxonNameCategoryId.NnCode:
                            if (taxonName.IsRecommended)
                            {
                                this.Identifiers.Add(taxonName);
                            }
                            break;

                        case TaxonNameCategoryId.ItisName:
                            if (taxonName.IsRecommended)
                            {
                                this.Identifiers.Add(taxonName);
                            }
                            break;

                        case TaxonNameCategoryId.ItisNumber:
                            if (taxonName.IsRecommended)
                            {
                                this.Identifiers.Add(taxonName);

                            }
                            break;

                        case TaxonNameCategoryId.ErmsName:
                            if (taxonName.IsRecommended)
                            {
                                this.Identifiers.Add(taxonName);
                            }
                            break;

                        case TaxonNameCategoryId.GermanName:
                            if (taxonName.IsRecommended)
                            {
                                this.OtherRecommendedNames.Add(taxonName);
                            }
                            else
                            {
                                if (taxonName.Status.Id == (Int32)(TaxonNameStatusId.ApprovedNaming))
                                {
                                    this.OtherSynonyms.Add(taxonName);
                                }
                            }
                            break;

                        case TaxonNameCategoryId.OriginalName:
                            this.OriginalName = taxonName.Name;
                            this.OriginalAuthor = taxonName.Author;
                            break;

                        case TaxonNameCategoryId.FaeroeName:
                            if (taxonName.IsRecommended)
                            {
                                this.OtherRecommendedNames.Add(taxonName);
                            }
                            else
                            {
                                if (taxonName.Status.Id == (Int32)(TaxonNameStatusId.ApprovedNaming))
                                {
                                    this.OtherSynonyms.Add(taxonName);
                                }
                            }
                            break;

                        case TaxonNameCategoryId.AnamorphName:
                            this.AnamorphName = taxonName.Name;
                            this.AnamorphAuthor = taxonName.Author;
                            break;

                        case TaxonNameCategoryId.Guid:
                            if (taxonName.IsRecommended)
                            {
                                this.Identifiers.Add(taxonName);


                            }
                            break;
                        default:
                            throw new NotImplementedException("No valid taxon name category for taxon: " + taxonName.Taxon.Id + " and name: " + taxonName);
                    }
                
            }
        }
    }
   
}
