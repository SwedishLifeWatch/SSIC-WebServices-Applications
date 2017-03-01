using System;
using System.Collections.Generic;
using System.IO;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Extensions;
using Resources;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    public class SpeciesFactDyntaxaListManager
    {
        /// <summary>
        /// Constructor of a List manager.
        /// </summary>
        public SpeciesFactDyntaxaListManager()
        {
        }
 
        /// <summary>
        /// Creates a memory stream of all factors for a taxon in a excel file
        /// </summary>
        /// <param name="user"></param>
        /// <param name="taxon"></param>
        /// <param name="dyntaxaFactors"></param>
        /// <param name="fileFormat"></param>
        /// <param name="showNonPublicData"></param>
        /// <returns></returns>
        public MemoryStream CreateSpeciesFactExcelFile(IUserContext user, ITaxon taxon, DyntaxaAllFactorData dyntaxaFactors, ExcelFileFormat fileFormat, bool showNonPublicData)
        {
            var excelFile = new SpeciesFactDataList();
            return excelFile.CreateSpeciesFactExcelFile(fileFormat, user, dyntaxaFactors, taxon, showNonPublicData);
        }

        /// <summary>
        /// Creates a view model ie view data of all factors for a taxon.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="taxon"></param>
        /// <param name="dyntaxaFactors"></param>
        /// <param name="showNonPublicData"></param>
        /// <returns></returns>
        public SpeciesFactViewModel CreateSpeciesFactViewData(IUserContext user, ITaxon taxon, DyntaxaAllFactorData dyntaxaFactors, bool showNonPublicData, SpeciesFactViewModel model, bool useAllFactors)
        {
            var viewData = new SpeciesFactDataList();
            return viewData.CreateSpeciesFactViewData(user, dyntaxaFactors, taxon, showNonPublicData, model, useAllFactors);
        }

        /// <summary>
        /// Creates view model for Editing factor data ie EditFactorItem.ascx
        /// </summary>
        /// <param name="user"></param>
        /// <param name="taxon"></param>
        /// <param name="factorData"></param>
        /// <param name="factorDataType"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public FactorViewModel CreateFactorViewData(IUserContext user, ITaxon taxon, DyntaxaSpeciesFact factorData, int factorDataType, int dataTypeId, FactorViewModel model, IList<DyntaxaIndividualCategory> allIndividualCategories, string referenceId, bool isHost)
        {
            model.FactorDataTypeId = factorDataType;
            model.DataTypeId = dataTypeId;
            model.FactorName = factorData.Label;
            if (isHost)
            {
                model.FactorName = factorData.HostName;
            }
           
            model.IndividualCategoryName = factorData.IndividualCatgory.Label;
            model.QualityId = factorData.Quality.QualityId;
            model.QualityValueList = new List<SpeciesFactDropDownModelHelper>(); 
            foreach (KeyValuePair<int, string> quality in factorData.Quality.Qualities)
            {
                model.QualityValueList.Add(new SpeciesFactDropDownModelHelper(quality.Key, quality.Value)); 
            }

            model.FactorFieldComment = factorData.Comments;
             model.FactorFieldEnumLabel = factorData.FactorEnumLabel;
            model.FactorFieldEnumValueList = new List<SpeciesFactDropDownModelHelper>();
            foreach (KeyValuePair<int, string> factorFieldEnum in factorData.FactorEnumValueList)
            {
               // Only add 0 and positiv values for Substrate and Biotope.
                if ((factorFieldEnum.Key >= 0 && factorDataType == (int)DyntaxaFactorDataType.AF_BIOTOPE)
                    || (factorFieldEnum.Key >= 0 && factorDataType == (int)DyntaxaFactorDataType.AF_SUBSTRATE)
                    || factorDataType == (int)DyntaxaFactorDataType.AF_INFLUENCE)
                {
                    model.FactorFieldEnumValueList.Add(new SpeciesFactDropDownModelHelper(factorFieldEnum.Key, factorFieldEnum.Value));
                }
            }

            model.FactorFieldEnumValue = factorData.FactorEnumValue;           
            model.FactorFieldEnumValueList2 = new List<SpeciesFactDropDownModelHelper>();
            foreach (KeyValuePair<int, string> factorFieldEnum2 in factorData.FactorEnumValueList2)
            {
                // Only add 0 and positiv values for Substrate and Biotope.
                if (factorDataType == (int)DyntaxaFactorDataType.AF_SUBSTRATE || factorDataType == (int)DyntaxaFactorDataType.AF_INFLUENCE)
                {
                    model.FactorFieldEnumValueList2.Add(new SpeciesFactDropDownModelHelper(factorFieldEnum2.Key, factorFieldEnum2.Value));
                }
            }

            model.FactorFieldEnumValueList2.Add(new SpeciesFactDropDownModelHelper(SpeciesFactModelManager.SpeciesFactNoValueSet, DyntaxaResource.SpeciesFactNoValueSet));
            model.FactorFieldEnumValue2 = factorData.FactorEnumValue2;          
            model.FactorFieldEnumLabel2 = factorData.FactorEnumLabel2;
            model.FaktorReferenceList = new List<SpeciesFactDropDownModelHelper>();
            bool isReferenceSet = false;
            int dyntaxaReferenceId = 0;
            int userReferenceId = 0;
            if (referenceId.IsNotEmpty())
            {
                isReferenceSet = true;
               
                var referenceList = ReferenceHelper.GetReferenceList(user);
                foreach (IReference reference in referenceList)
                {
                    if (reference.Id == Convert.ToInt32(referenceId))
                    {
                        model.FaktorReferenceList.Add(new SpeciesFactDropDownModelHelper(reference.Id, reference.Name + " " + reference.Year));
                        userReferenceId = reference.Id;
                        model.FactorReferenceId = reference.Id;
                    }
                }

                if (factorData.ReferenceName.IsNotEmpty())
                {
                    model.FactorReferenceOld = factorData.ReferenceName;
                }
                else
                {
                    model.FactorReferenceOld = "-";
                }
            }

            if (factorData.DyntaxaFactorReference.IsNotNull())
            {
               if (!isReferenceSet)
               {
                   isReferenceSet = true;
                   model.FactorReferenceId = factorData.DyntaxaFactorReference.GetReference(user).Id;
               }
                
                dyntaxaReferenceId = factorData.DyntaxaFactorReference.GetReference(user).Id;
                IReference reference = factorData.DyntaxaFactorReference.GetReference(user);
                Int32 year = reference.Year.HasValue ? reference.Year.Value : -1;
                model.FaktorReferenceList.Add(new SpeciesFactDropDownModelHelper(reference.Id, reference.Name + " " + year));
            }
           
            // Here we get the dyntaxa references...
            foreach (IReferenceRelation referenceRelation in taxon.GetReferenceRelations(user))
            {
                IReference reference = referenceRelation.GetReference(user);
                Int32 year = reference.Year.HasValue ? reference.Year.Value : -1;
                if (reference.Id != dyntaxaReferenceId && reference.Id != userReferenceId)
                {
                    model.FaktorReferenceList.Add(
                        new SpeciesFactDropDownModelHelper(
                            reference.Id,
                            reference.Name + " " + year));
                }

                if (!isReferenceSet)
                {
                    isReferenceSet = true;
                    model.FactorReferenceId = reference.Id;
                }
            }

            model.IndividualCategoryId = factorData.IndividualCatgory.Id;
            model.IndividualCategoryName = factorData.IndividualCatgory.Label;
            model.IndividualCategoryList = new List<SpeciesFactDropDownModelHelper>();

            // Add all individual categories that exist
            List<DyntaxaIndividualCategory> exitingCategories = factorData.IndividualCategoryList as List<DyntaxaIndividualCategory>;
            foreach (DyntaxaIndividualCategory category in allIndividualCategories)
            {
               model.IndividualCategoryList.Add(new SpeciesFactDropDownModelHelper(category.Id, category.Label));
            }
           
            model.ExistingEvaluations = factorData.ExistingEvaluations;
            string message = Resources.DyntaxaResource.SpeciesFactLatestUpdateByText.Replace("[UpdateDate]", factorData.UpdateDate.ToShortDateString());
            message = message.Replace("[UpdateUserFullName]", factorData.UpdateUserFullName);
            model.UpdateUserData = message;
           
            return model;
        }

        public FactorViewModel CreateNewFactorViewData(IUserContext user, ITaxon taxon, DyntaxaSpeciesFact factorData, int factorDataType, int dataTypeId, FactorViewModel model, IList<DyntaxaIndividualCategory> allIndividualCategories, string referenceId, bool isHost)
        {
            model.FactorDataTypeId = factorDataType;
            model.DataTypeId = dataTypeId; 
            model.FactorName = factorData.Label;
            if (isHost && !model.FactorName.Contains(model.FactorName))
            {
                model.FactorName = factorData.HostName + " (" + model.FactorName + ")";
            }

            model.QualityValueList = new List<SpeciesFactDropDownModelHelper>();
            // Quality reference value == Godtagbar
            model.QualityId = factorData.Quality.QualityId;
            foreach (KeyValuePair<int, string> quality in factorData.Quality.Qualities)
            {
                model.QualityValueList.Add(new SpeciesFactDropDownModelHelper(quality.Key, quality.Value));
                if (quality.Value == "Godtagbar")
                {
                    model.QualityId = quality.Key;
                }
            }
           
            model.FactorFieldComment = "";
            model.FactorFieldEnumValue = factorData.FactorEnumValue;
            model.FactorFieldEnumLabel = factorData.FactorEnumLabel;
            model.FactorFieldEnumValueList = new List<SpeciesFactDropDownModelHelper>();
            foreach (KeyValuePair<int, string> factorFieldEnum in factorData.FactorEnumValueList)
            {
                // Only add 0 and positiv values for Substrate and Biotope.
                if ((factorFieldEnum.Key >= 0 && factorDataType == (int)DyntaxaFactorDataType.AF_BIOTOPE)
                    || (factorFieldEnum.Key >= 0 && factorDataType == (int)DyntaxaFactorDataType.AF_SUBSTRATE)
                    || factorDataType == (int)DyntaxaFactorDataType.AF_INFLUENCE)
                {
                    model.FactorFieldEnumValueList.Add(new SpeciesFactDropDownModelHelper(factorFieldEnum.Key, factorFieldEnum.Value));
                }

                if (factorFieldEnum.Key == 1)
                {
                    model.FactorFieldEnumValue = factorFieldEnum.Key;
                }
            }

            model.FactorFieldEnumLabel2 = factorData.FactorEnumLabel2;
            if (factorData.FactorEnumValue2.IsNull())
            {
                model.FactorFieldEnumValue2 = SpeciesFactModelManager.SpeciesFactNoValueSet;
            }
            else
            {
                model.FactorFieldEnumValue2 = factorData.FactorEnumValue2;
            }

            model.FactorFieldEnumValueList2 = new List<SpeciesFactDropDownModelHelper>();
            foreach (KeyValuePair<int, string> factorFieldEnum2 in factorData.FactorEnumValueList2)
            {
                // Only add values for Substrate and influence.
                if (factorDataType == (int)DyntaxaFactorDataType.AF_SUBSTRATE || factorDataType == (int)DyntaxaFactorDataType.AF_INFLUENCE)
                {
                    model.FactorFieldEnumValueList2.Add(new SpeciesFactDropDownModelHelper(factorFieldEnum2.Key, factorFieldEnum2.Value));
                }
            }

            model.FactorFieldEnumValueList2.Add(new SpeciesFactDropDownModelHelper(SpeciesFactModelManager.SpeciesFactNoValueSet, DyntaxaResource.SpeciesFactNoValueSet));
            model.FaktorReferenceList = new List<SpeciesFactDropDownModelHelper>();
            bool isReferenceSet = false;
            int factorReferenceId = 0;
            int userReferenceId = 0;
            if (referenceId.IsNotEmpty())
            {
                isReferenceSet = true;
                List<IReference> referenceList = ReferenceHelper.GetReferenceList(user);
                foreach (ArtDatabanken.Data.IReference reference in referenceList)
                {
                    if (reference.Id == Convert.ToInt32(referenceId))
                    {
                        model.FaktorReferenceList.Add(new SpeciesFactDropDownModelHelper(reference.Id, reference.Name + " " + reference.Year));
                        userReferenceId = reference.Id;
                        model.FactorReferenceId = reference.Id;
                    }
                }
            } 

            if (factorData.DyntaxaFactorReference.IsNotNull())
            {
                IReference reference = factorData.DyntaxaFactorReference.GetReference(user);
                Int32 year = reference.Year.HasValue ? reference.Year.Value : -1;
                if (!isReferenceSet)
                {
                    isReferenceSet = true;
                    model.FactorReferenceId = factorData.DyntaxaFactorReference.GetReference(user).Id;
                }

                factorReferenceId = reference.Id;
                model.FaktorReferenceList.Add(
                   new SpeciesFactDropDownModelHelper(
                       reference.Id,
                       reference.Name + " " + year));
            }
          
            // Here we get the dyntaxa references...
            foreach (IReferenceRelation referenceRelation in taxon.GetReferenceRelations(user))
            {
                IReference reference = referenceRelation.GetReference(user);
                Int32 year = reference.Year.HasValue ? reference.Year.Value : -1;
                if (reference.Id != factorReferenceId && reference.Id != userReferenceId)
                {
                    model.FaktorReferenceList.Add(
                        new SpeciesFactDropDownModelHelper(
                            reference.Id,
                            reference.Name + " " + year));
                }

                if (!isReferenceSet)
                {
                    isReferenceSet = true;
                    model.FactorReferenceId = reference.Id;
                }
            }

            model.IndividualCategoryId = factorData.IndividualCatgory.Id;
            model.IndividualCategoryName = factorData.IndividualCatgory.Label;
            model.IndividualCategoryList = new List<SpeciesFactDropDownModelHelper>();
            
            // Add all individual categories that exist
            List<DyntaxaIndividualCategory> exitingCategories = factorData.IndividualCategoryList as List<DyntaxaIndividualCategory>;
            foreach (DyntaxaIndividualCategory category in allIndividualCategories)
            {
                model.IndividualCategoryList.Add(new SpeciesFactDropDownModelHelper(category.Id, category.Label));
            }

            model.ExistingEvaluations = factorData.ExistingEvaluations;
            string message = "";
            model.UpdateUserData = message;

            return model;
        }

        public FactorViewModel CreateFactorAndHostViewData(IUserContext user, ITaxon taxon, IList<DyntaxaSpeciesFact> factorData, int factorDataType, int dataTypeId, FactorViewModel model, string referenceId, bool isHost)
        {
            IList<DyntaxaIndividualCategory> allIndividualCategories = new List<DyntaxaIndividualCategory>();
            foreach (DyntaxaSpeciesFact dyntaxaSpeciesFact in factorData)
            {
                if (!allIndividualCategories.Contains(dyntaxaSpeciesFact.IndividualCatgory))
                {
                    allIndividualCategories.Add(dyntaxaSpeciesFact.IndividualCatgory);
                }
            }
           
           // Set up data for first factor.. Use data from the first factor
            FactorViewModel tempModel = CreateFactorViewData(user, taxon, factorData[0], factorDataType, dataTypeId, model, allIndividualCategories, referenceId, isHost);
            // This is setting the header in View
            string headerName = string.Empty;
            foreach (DyntaxaSpeciesFact dyntaxaSpeciesFact in factorData)
            {
               string name = dyntaxaSpeciesFact.HostName;
               headerName = headerName + name + " \n";
            }

            tempModel.HostTaxaHeader = "Valda värdtaxa:";
            tempModel.HostTaxaText = headerName;
            tempModel.FactorName = "Värdtaxa";
            bool isFirst = true;
            foreach (DyntaxaIndividualCategory dyntaxaIndividualCategory in allIndividualCategories)
            {
                if (isFirst)
                {
                    model.IndividualCategoryName = dyntaxaIndividualCategory.Label;
                    isFirst = false;
                }
                else
                {
                    model.IndividualCategoryName = model.IndividualCategoryName + ", " + dyntaxaIndividualCategory.Label;
                }
            }

           return tempModel;
        }
    }
}
