using System;
using System.Collections;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.WebService.Client.TaxonAttributeService
{
    /// <summary>
    /// This class is used to handle factor related information.
    /// </summary>
    public class FactorDataSource : TaxonAttributeDataSource, IFactorDataSource
    {
        /// <summary>
        /// Convert a WebFactorDataType instance into
        /// an IFactorDataType instance.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webFactorDataType">A WebFactorDataType instance.</param>
        /// <param name="factorFieldEnumerations">List of factor field enumerations.</param>
        /// <returns>An IFactorDataType instance.</returns>
        private IFactorDataType GetFactorDataType(IUserContext userContext,
                                                  WebFactorDataType webFactorDataType,
                                                  FactorFieldEnumList factorFieldEnumerations)
        {
            IFactorDataType factorDataType = new FactorDataType
            {
                DataContext = GetDataContext(userContext),
                Definition = webFactorDataType.Definition,
                Id = webFactorDataType.Id,
                Name = webFactorDataType.Name
            };

            FactorFieldList fields = new FactorFieldList();

            foreach (WebFactorField webFactorField in webFactorDataType.Fields)
            {
                fields.Add(GetFactorField(userContext, factorDataType, webFactorField, factorFieldEnumerations));
            }

            factorDataType.Fields = fields;

            int factorFieldMaxCount = CoreData.FactorManager.GetFactorFieldMaxCount();

            factorDataType.FieldArray = new IFactorField[factorFieldMaxCount];
            for (int i = 0; i < factorFieldMaxCount; i++)
            {
                factorDataType.FieldArray[i] = null;
            }

            foreach (IFactorField field in factorDataType.Fields)
            {
                factorDataType.FieldArray[field.Index] = field;
            }

            return factorDataType;
        }

        /// <summary>
        /// Get all factor data types.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All factor data types.</returns>
        public virtual FactorDataTypeList GetFactorDataTypes(IUserContext userContext)
        {
            List<WebFactorDataType> webFactorDataTypes;

            CheckTransaction(userContext);
            webFactorDataTypes = WebServiceProxy.TaxonAttributeService.GetFactorDataTypes(GetClientInformation(userContext));
            return GetFactorDataTypes(userContext, webFactorDataTypes);
        }

        /// <summary>
        /// Convert a list of WebFactorDataType instances
        /// to a FactorDataTypeList.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webFactorDataTypes">List of WebFactorDataType instances.</param>
        /// <returns>Factor data types.</returns>
        private FactorDataTypeList GetFactorDataTypes(IUserContext userContext,
                                                      List<WebFactorDataType> webFactorDataTypes)
        {
            FactorDataTypeList factorDataTypes;
            FactorFieldEnumList factorFieldEnums = CoreData.FactorManager.GetFactorFieldEnums(userContext);

            factorDataTypes = null;
            if (webFactorDataTypes.IsNotEmpty())
            {
                factorDataTypes = new FactorDataTypeList();
                foreach (WebFactorDataType webFactorDataType in webFactorDataTypes)
                {
                    factorDataTypes.Add(GetFactorDataType(userContext, webFactorDataType, factorFieldEnums));
                }

                factorDataTypes.Sort();
            }

            return factorDataTypes;
        }

        /// <summary>
        /// Convert a WebFactorField into an IFactorField instance.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="factorDataType">Factor data type that this factor field belongs to.</param>
        /// <param name="webFactorField">A WebFactorField instance.</param>
        /// <param name="factorFieldEnums">List of factor field enumerations.</param>
        /// <returns>An IFactorField instance.</returns>
        private IFactorField GetFactorField(IUserContext userContext,
                                            IFactorDataType factorDataType,
                                            WebFactorField webFactorField,
                                            FactorFieldEnumList factorFieldEnums)
        {
            Int32 factorFieldIndex;

            switch (webFactorField.DatabaseFieldName)
            {
                case "tal1":
                    factorFieldIndex = 0;
                    break;
                case "tal2":
                    factorFieldIndex = 1;
                    break;
                case "tal3":
                    factorFieldIndex = 2;
                    break;
                case "text1":
                    factorFieldIndex = 3;
                    break;
                case "text2":
                    factorFieldIndex = 4;
                    break;
                default:
                    throw new ApplicationException("Unknown database field name = " + webFactorField.DatabaseFieldName);
            }

            IFactorField factorField = new FactorField
            {
                DataContext = GetDataContext(userContext),
                DatabaseFieldName = webFactorField.DatabaseFieldName,
                FactorDataType = factorDataType,
                Id = webFactorField.Id,
                Index = factorFieldIndex,
                Information = webFactorField.Information,
                IsMain = webFactorField.IsMain,
                IsSubstantial = webFactorField.IsSubstantial,
                Label = webFactorField.Label,
                Size = webFactorField.Size,
                Type = CoreData.FactorManager.GetFactorFieldType(userContext, webFactorField.TypeId),
                Unit = webFactorField.Unit
            };

            if (webFactorField.IsEnumField)
            {
                factorField.Enum = factorFieldEnums.Get(webFactorField.EnumId);
            }

            return factorField;
        }

        /// <summary>
        /// Convert a web factor field enumeration instance into
        /// a factor field enumeration instance.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webFactorFieldEnum">A web factor field enumeration instance.</param>
        /// <returns>A factor field enumeration instance.</returns>
        private IFactorFieldEnum GetFactorFieldEnum(IUserContext userContext,
                                                    WebFactorFieldEnum webFactorFieldEnum)
        {
            IFactorFieldEnum factorFieldEnum;
            FactorFieldEnumValueList values;

            factorFieldEnum = new FactorFieldEnum();
            factorFieldEnum.DataContext = GetDataContext(userContext);
            factorFieldEnum.Id = webFactorFieldEnum.Id;
            values = new FactorFieldEnumValueList();
            foreach (WebFactorFieldEnumValue webFactorFieldEnumValue in webFactorFieldEnum.Values)
            {
                values.Add(GetFactorFieldEnumValue(userContext, factorFieldEnum, webFactorFieldEnumValue));
            }

            factorFieldEnum.Values = values;
            return factorFieldEnum;
        }

        /// <summary>
        /// Convert a web factor field enumeration into
        /// an factor field enumeration instance.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="factorFieldEnum">The enumeration value belongs to this enumeration.</param>
        /// <param name="webFactorFieldEnumValue">A web factor field enumeration instance.</param>
        /// <returns>A factor field enumeration instance.</returns>
        private IFactorFieldEnumValue GetFactorFieldEnumValue(IUserContext userContext,
                                                              IFactorFieldEnum factorFieldEnum,
                                                              WebFactorFieldEnumValue webFactorFieldEnumValue)
        {
            IFactorFieldEnumValue factorFieldEnumValue;

            factorFieldEnumValue = new FactorFieldEnumValue();
            factorFieldEnumValue.DataContext = GetDataContext(userContext);
            factorFieldEnumValue.Enum = factorFieldEnum;
            factorFieldEnumValue.Id = webFactorFieldEnumValue.Id;
            factorFieldEnumValue.Information = webFactorFieldEnumValue.Information;
            if (webFactorFieldEnumValue.IsKeyIntegerSpecified)
            {
                factorFieldEnumValue.KeyInt = webFactorFieldEnumValue.KeyInteger;
            }

            factorFieldEnumValue.KeyText = webFactorFieldEnumValue.KeyText;
            factorFieldEnumValue.Label = (webFactorFieldEnumValue.IsKeyIntegerSpecified
                                              ? "[" + webFactorFieldEnumValue.KeyInteger + "] "
                                              : "[" + webFactorFieldEnumValue.KeyText + "] ")
                                         + webFactorFieldEnumValue.Label;
            factorFieldEnumValue.OriginalLabel = webFactorFieldEnumValue.Label;
            factorFieldEnumValue.ShouldBeSaved = webFactorFieldEnumValue.ShouldBeSaved;
            factorFieldEnumValue.SortOrder = webFactorFieldEnumValue.SortOrder;
            return factorFieldEnumValue;
        }

        /// <summary>
        /// Get all factor field enumerations.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All factor field enumerations.</returns>
        public virtual FactorFieldEnumList GetFactorFieldEnums(IUserContext userContext)
        {
            List<WebFactorFieldEnum> webFactorFieldEnums;

            CheckTransaction(userContext);
            webFactorFieldEnums = WebServiceProxy.TaxonAttributeService.GetFactorFieldEnums(GetClientInformation(userContext));
            return GetFactorFieldEnums(userContext, webFactorFieldEnums);
        }

        /// <summary>
        /// Convert a list of web factor field enumeration instances
        /// to a list of factor field enumeration instances.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webFactorFieldEnums">List of factor field enumeration instances.</param>
        /// <returns>Factor field enumerations.</returns>
        private FactorFieldEnumList GetFactorFieldEnums(IUserContext userContext,
                                                        List<WebFactorFieldEnum> webFactorFieldEnums)
        {
            FactorFieldEnumList factorFieldEnums;

            factorFieldEnums = null;
            if (webFactorFieldEnums.IsNotEmpty())
            {
                factorFieldEnums = new FactorFieldEnumList();
                foreach (WebFactorFieldEnum webFactorFieldEnum in webFactorFieldEnums)
                {
                    factorFieldEnums.Add(GetFactorFieldEnum(userContext, webFactorFieldEnum));
                }

                factorFieldEnums.Sort();
                foreach (IFactorFieldEnum factorFieldEnum in factorFieldEnums)
                {
                    factorFieldEnum.Values.Sort();
                }
            }

            return factorFieldEnums;
        }

        /// <summary>
        /// Convert a WebFactorFieldType instance into
        /// an IFactorFieldType instance.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webFactorFieldType">A WebFactorFieldType instance.</param>
        /// <returns>An IFactorFieldType instance.</returns>
        private IFactorFieldType GetFactorFieldType(IUserContext userContext, WebFactorFieldType webFactorFieldType)
        {
            IFactorFieldType factorFieldType;

            factorFieldType = new FactorFieldType();
            factorFieldType.DataContext = GetDataContext(userContext);
            factorFieldType.Definition = webFactorFieldType.Definition;
            factorFieldType.Id = webFactorFieldType.Id;
            factorFieldType.Name = webFactorFieldType.Name;
            return factorFieldType;
        }

        /// <summary>
        /// Get all factor field types.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All factor field types.</returns>
        public virtual FactorFieldTypeList GetFactorFieldTypes(IUserContext userContext)
        {
            List<WebFactorFieldType> webFactorFieldTypes;

            CheckTransaction(userContext);
            webFactorFieldTypes = WebServiceProxy.TaxonAttributeService.GetFactorFieldTypes(GetClientInformation(userContext));
            return GetFactorFieldTypes(userContext, webFactorFieldTypes);
        }

        /// <summary>
        /// Convert a list of WebFactorFieldType instances
        /// to a FactorUpdateFieldTypesList.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webFactorFieldTypes">List of WebFactorFieldType instances.</param>
        /// <returns>Factor field types.</returns>
        private FactorFieldTypeList GetFactorFieldTypes(IUserContext userContext,
                                                        List<WebFactorFieldType> webFactorFieldTypes)
        {
            FactorFieldTypeList factorFieldTypes;

            factorFieldTypes = null;
            if (webFactorFieldTypes.IsNotEmpty())
            {
                factorFieldTypes = new FactorFieldTypeList();
                foreach (WebFactorFieldType webFactorFieldType in webFactorFieldTypes)
                {
                    factorFieldTypes.Add(GetFactorFieldType(userContext, webFactorFieldType));
                }
            }

            return factorFieldTypes;
        }

        /// <summary>
        /// Convert a WebFactorOrigin instance into
        /// an IFactorOrigin instance.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webFactorOrigin">A WebFactorOrigin instance.</param>
        /// <returns>An IFactorOrigin instance.</returns>
        private IFactorOrigin GetFactorOrigin(IUserContext userContext,
                                              WebFactorOrigin webFactorOrigin)
        {
            IFactorOrigin factorOrigin;

            factorOrigin = new FactorOrigin();
            factorOrigin.DataContext = GetDataContext(userContext);
            factorOrigin.Definition = webFactorOrigin.Definition;
            factorOrigin.Id = webFactorOrigin.Id;
            factorOrigin.Name = webFactorOrigin.Name;
            factorOrigin.SortOrder = webFactorOrigin.SortOrder;
            return factorOrigin;
        }

        /// <summary>
        /// Get all factor origins.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All factor origins.</returns>
        public virtual FactorOriginList GetFactorOrigins(IUserContext userContext)
        {
            List<WebFactorOrigin> webFactorOrigins;

            CheckTransaction(userContext);
            webFactorOrigins = WebServiceProxy.TaxonAttributeService.GetFactorOrigins(GetClientInformation(userContext));
            return GetFactorOrigins(userContext, webFactorOrigins);
        }

        /// <summary>
        /// Convert a list of WebFactorOrigin instances
        /// to a FactorOriginList.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webFactorOrigins">List of WebFactorOrigin instances.</param>
        /// <returns>Factor origins.</returns>
        private FactorOriginList GetFactorOrigins(IUserContext userContext,
                                                  List<WebFactorOrigin> webFactorOrigins)
        {
            FactorOriginList factorOrigins;

            factorOrigins = null;
            if (webFactorOrigins.IsNotEmpty())
            {
                factorOrigins = new FactorOriginList();
                foreach (WebFactorOrigin webFactorOrigin in webFactorOrigins)
                {
                    factorOrigins.Add(GetFactorOrigin(userContext, webFactorOrigin));
                }

                factorOrigins.Sort();
            }

            return factorOrigins;
        }

        /// <summary>
        /// Convert a WebFactorUpdateMode instance into
        /// an IFactorUpdateMode instance.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webFactorUpdateMode">A WebFactorUpdateMode instance.</param>
        /// <returns>An IFactorUpdateMode instance.</returns>
        private IFactorUpdateMode GetFactorUpdateMode(IUserContext userContext, WebFactorUpdateMode webFactorUpdateMode)
        {
            IFactorUpdateMode factorUpdateMode;

            factorUpdateMode = new FactorUpdateMode();
            factorUpdateMode.DataContext = GetDataContext(userContext);
            factorUpdateMode.Definition = webFactorUpdateMode.Definition;
            factorUpdateMode.Id = webFactorUpdateMode.Id;
            factorUpdateMode.AllowUpdate = webFactorUpdateMode.IsUpdateAllowed;
            factorUpdateMode.Name = webFactorUpdateMode.Name;
            factorUpdateMode.Type = webFactorUpdateMode.Type;
            return factorUpdateMode;
        }

        /// <summary>
        /// Get all factor update modes.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All factor update modes.</returns>
        public virtual FactorUpdateModeList GetFactorUpdateModes(IUserContext userContext)
        {
            List<WebFactorUpdateMode> webFactorUpdateModes;

            CheckTransaction(userContext);
            webFactorUpdateModes = WebServiceProxy.TaxonAttributeService.GetFactorUpdateModes(GetClientInformation(userContext));
            return GetFactorUpdateModes(userContext, webFactorUpdateModes);
        }

        /// <summary>
        /// Convert a list of WebFactorUpdateMode instances
        /// to a FactorUpdateModesList.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webFactorUpdateModes">List of WebFactorUpdateMode instances.</param>
        /// <returns>Factor update modes.</returns>
        private FactorUpdateModeList GetFactorUpdateModes(IUserContext userContext,
                                                          List<WebFactorUpdateMode> webFactorUpdateModes)
        {
            FactorUpdateModeList factorUpdateModes;

            factorUpdateModes = null;
            if (webFactorUpdateModes.IsNotEmpty())
            {
                factorUpdateModes = new FactorUpdateModeList();
                foreach (WebFactorUpdateMode webFactorUpdateMode in webFactorUpdateModes)
                {
                    factorUpdateModes.Add(GetFactorUpdateMode(userContext, webFactorUpdateMode));
                }

                factorUpdateModes.Sort();
            }

            return factorUpdateModes;
        }

        /// <summary>
        /// Convert a WebPeriodType instance into
        /// an IPeriodType instance.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webPeriodType">A WebPeriodType instance.</param>
        /// <returns>An IPeriodType instance.</returns>
        private IPeriodType GetPeriodType(IUserContext userContext, WebPeriodType webPeriodType)
        {
            IPeriodType periodType;

            periodType = new PeriodType();
            periodType.DataContext = GetDataContext(userContext);
            periodType.Description = webPeriodType.Description;
            periodType.Id = webPeriodType.Id;
            periodType.Name = webPeriodType.Name;
            return periodType;
        }

        /// <summary>
        /// Get all period types.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All period types.</returns>
        public virtual PeriodTypeList GetPeriodTypes(IUserContext userContext)
        {
            List<WebPeriodType> webPeriodTypes;

            CheckTransaction(userContext);
            webPeriodTypes = WebServiceProxy.TaxonAttributeService.GetPeriodTypes(GetClientInformation(userContext));
            return GetPeriodTypes(userContext, webPeriodTypes);
        }

        /// <summary>
        /// Convert a list of WebPeriodType instances
        /// to a PeriodTypesList.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webPeriodTypes">List of WebPeriodType instances.</param>
        /// <returns>Period types.</returns>
        private PeriodTypeList GetPeriodTypes(IUserContext userContext,
                                              List<WebPeriodType> webPeriodTypes)
        {
            PeriodTypeList periodTypes;

            periodTypes = null;
            if (webPeriodTypes.IsNotEmpty())
            {
                periodTypes = new PeriodTypeList();
                foreach (WebPeriodType webPeriodType in webPeriodTypes)
                {
                    periodTypes.Add(GetPeriodType(userContext, webPeriodType));
                }
            }

            return periodTypes;
        }

        /// <summary>
        /// Convert a WebPeriod instance into
        /// an IPeriod instance.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webPeriod">A WebPeriod instance.</param>
        /// <returns>An IPeriod instance.</returns>
        private IPeriod GetPeriod(IUserContext userContext, WebPeriod webPeriod)
        {
            IPeriod period;

            period = new Period();
            period.DataContext = GetDataContext(userContext);
            period.Description = webPeriod.Description;
            period.Id = webPeriod.Id;
            period.Name = webPeriod.Name;
            period.StopUpdates = webPeriod.StopUpdates;
            period.Year = webPeriod.Year;
            period.Type = CoreData.FactorManager.GetPeriodType(userContext,
                                                               webPeriod.TypeId);
            return period;
        }

        /// <summary>
        /// Get all periods.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All periods.</returns>
        public virtual PeriodList GetPeriods(IUserContext userContext)
        {
            List<WebPeriod> webPeriods;

            CheckTransaction(userContext);
            webPeriods = WebServiceProxy.TaxonAttributeService.GetPeriods(GetClientInformation(userContext));
            return GetPeriods(userContext, webPeriods);
        }

        /// <summary>
        /// Convert a list of WebPeriod instances
        /// to a PeriodList.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webPeriods">List of WebPeriod instances.</param>
        /// <returns>An instances of PeriodList.</returns>
        private PeriodList GetPeriods(IUserContext userContext,
                                      List<WebPeriod> webPeriods)
        {
            PeriodList periods;

            periods = null;
            if (webPeriods.IsNotEmpty())
            {
                periods = new PeriodList();
                foreach (WebPeriod webPeriod in webPeriods)
                {
                    periods.Add(GetPeriod(userContext, webPeriod));
                }

                periods.Sort();
            }

            return periods;
        }

        /// <summary>
        /// Convert a WebIndividualCategory instance into
        /// an IIndividualCategory instance.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webIndividualCategory">A WebIndividualCategory instance.</param>
        /// <returns>An IIndividualCategory instance.</returns>
        private IIndividualCategory GetIndividualCategory(IUserContext userContext, WebIndividualCategory webIndividualCategory)
        {
            IIndividualCategory individualCategory;

            individualCategory = new IndividualCategory();
            individualCategory.DataContext = GetDataContext(userContext);
            individualCategory.Definition = webIndividualCategory.Definition;
            individualCategory.Id = webIndividualCategory.Id;
            individualCategory.Name = webIndividualCategory.Name;
            return individualCategory;
        }

        /// <summary>
        /// Get all individual categories.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All individual categories.</returns>
        public virtual IndividualCategoryList GetIndividualCategories(IUserContext userContext)
        {
            List<WebIndividualCategory> webIndividualCategories;

            CheckTransaction(userContext);
            webIndividualCategories = WebServiceProxy.TaxonAttributeService.GetIndividualCategories(GetClientInformation(userContext));
            return GetIndividualCategories(userContext, webIndividualCategories);
        }

        /// <summary>
        /// Convert a list of WebIndividualCategory instances
        /// to a IndividualCategoryList.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webIndividualCategories">List of WebIndividualCategory instances.</param>
        /// <returns>Individual categories.</returns>
        private IndividualCategoryList GetIndividualCategories(IUserContext userContext,
                                                               List<WebIndividualCategory> webIndividualCategories)
        {
            IndividualCategoryList individualCategories;

            individualCategories = null;
            if (webIndividualCategories.IsNotEmpty())
            {
                individualCategories = new IndividualCategoryList();
                foreach (WebIndividualCategory webIndividualCategory in webIndividualCategories)
                {
                    individualCategories.Add(GetIndividualCategory(userContext, webIndividualCategory));
                }

                individualCategories.Sort();
            }

            return individualCategories;
        }

        /// <summary>
        /// Convert a WebFactor instance into
        /// an IFactor instance.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webFactor">A WebFactor instance.</param>
        /// <param name="factorDataTypes">List of factor data types.</param>
        /// <param name="factorOrigins">List of factor origins.</param>
        /// <param name="factorUpdateModes">List of factor update modes.</param>
        /// <returns>An IFactor instance.</returns>
        private IFactor GetFactor(IUserContext userContext, WebFactor webFactor, FactorDataTypeList factorDataTypes, FactorOriginList factorOrigins, FactorUpdateModeList factorUpdateModes)
        {
            IFactor factor = new Factor
                                 {
                                     DataContext = GetDataContext(userContext),
                                     Id = webFactor.Id,
                                     DefaultHostParentTaxonId = webFactor.DefaultHostParentTaxonId,
                                     DataType = webFactor.IsDataTypeIdSpecified ? factorDataTypes.Get(webFactor.DataTypeId) : null,
                                     Origin = factorOrigins.Get(webFactor.OriginId),
                                     UpdateMode = factorUpdateModes.Get(webFactor.UpdateModeId),
                                     HostLabel = webFactor.HostLabel,
                                     Information = webFactor.Information,
                                     IsLeaf = webFactor.IsLeaf,
                                     IsPeriodic = webFactor.IsPeriodic,
                                     IsPublic = webFactor.IsPublic,
                                     IsTaxonomic = webFactor.IsTaxonomic,
                                     Label = webFactor.Label,
                                     Name = webFactor.Name,
                                     SortOrder = webFactor.SortOrder
                                 };

            return factor;
        }

        /// <summary>
        /// Get all factors.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All factors.</returns>
        public virtual FactorList GetFactors(IUserContext userContext)
        {
            List<WebFactor> webFactors;

            CheckTransaction(userContext);
            webFactors = WebServiceProxy.TaxonAttributeService.GetFactors(GetClientInformation(userContext));
            return GetFactors(userContext, webFactors);
        }

        /// <summary>
        /// Convert a list of WebFactor instances
        /// to a FactorList.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webFactors">List of WebFactor instances.</param>
        /// <returns>Factors.</returns>
        private FactorList GetFactors(IUserContext userContext, List<WebFactor> webFactors)
        {
            FactorList factors;
            FactorDataTypeList factorDataTypes = CoreData.FactorManager.GetFactorDataTypes(userContext);
            FactorOriginList factorOrigins = CoreData.FactorManager.GetFactorOrigins(userContext);
            FactorUpdateModeList factorUpdateModes = CoreData.FactorManager.GetFactorUpdateModes(userContext);

            factors = null;
            if (webFactors.IsNotEmpty())
            {
                factors = new FactorList();
                foreach (WebFactor webFactor in webFactors)
                {
                    factors.Add(GetFactor(userContext, webFactor, factorDataTypes, factorOrigins, factorUpdateModes));
                }

                factors.Sort();
            }

            return factors;
        }

        /// <summary>
        /// Get web factor search criteria.
        /// </summary>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Web search criteria.</returns>
        private WebFactorSearchCriteria GetFactorSearchCriteria(IFactorSearchCriteria searchCriteria)
        {
            WebFactorSearchCriteria webFactorSearchCriteria = new WebFactorSearchCriteria
            {
                IsIdInNameSearchString = searchCriteria.IsIdInNameSearchString,
                NameSearchString = GetStringSearchCriteria(searchCriteria.NameSearchString),
                RestrictReturnToScope = searchCriteria.RestrictReturnToScope,
                RestrictSearchToFactorIds = searchCriteria.RestrictSearchToFactorIds,
                RestrictSearchToScope = searchCriteria.RestrictSearchToScope
            };

            return webFactorSearchCriteria;
        }

        /// <summary>
        /// Get factors that matches search criteria.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="searchCriteria">The factor search criteria.</param>
        /// <returns>Taxa that matches search criteria.</returns>
        public virtual FactorList GetFactors(IUserContext userContext,
                                             IFactorSearchCriteria searchCriteria)
        {
            List<WebFactor> webFactors;

            CheckTransaction(userContext);
            webFactors = WebServiceProxy.TaxonAttributeService.GetFactorsBySearchCriteria(GetClientInformation(userContext), GetFactorSearchCriteria(searchCriteria));
            return GetFactors(userContext, webFactors);
        }

        /// <summary>
        /// Convert a WebFactor instance into
        /// an IFactor instance.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webFactorTree">A WebFactorTreeNode instance.</param>
        /// <param name="factorTreeNodes">
        /// All factor tree nodes that have been created so far.
        /// This parameter is used to avoid duplication of
        /// factor tree nodes (factor tree nodes with same factor attached to it)
        /// if the same factor appears more than once in the tree.
        /// </param>
        /// <param name="factorDataTypes">List of factor data types.</param>
        /// <param name="factorOrigins">List of factor origins.</param>
        /// <param name="factorUpdateModes">List of factor update modes.</param>
        /// <returns>An IFactorTreeNode instance.</returns>
        private IFactorTreeNode GetFactorTree(IUserContext userContext, WebFactorTreeNode webFactorTree, Hashtable factorTreeNodes, FactorDataTypeList factorDataTypes, FactorOriginList factorOrigins, FactorUpdateModeList factorUpdateModes)
        {
            IFactorTreeNode child, factorTree;

            if (factorTreeNodes.ContainsKey(webFactorTree.Factor.Id))
            {
                factorTree = (IFactorTreeNode)factorTreeNodes[webFactorTree.Factor.Id];
            }
            else
            {
                factorTree = new FactorTreeNode
                                 {
                                     DataContext = GetDataContext(userContext),
                                     Factor = GetFactor(userContext, webFactorTree.Factor, factorDataTypes, factorOrigins, factorUpdateModes),
                                     Id = webFactorTree.Id
                                 };
                if (webFactorTree.Children.IsNotEmpty())
                {
                    factorTree.Children = new FactorTreeNodeList();
                    foreach (WebFactorTreeNode webChild in webFactorTree.Children)
                    {
                        child = GetFactorTree(userContext, webChild, factorTreeNodes, factorDataTypes, factorOrigins, factorUpdateModes);
                        if (child.Parents.IsNull())
                        {
                            child.Parents = new FactorTreeNodeList();
                        }

                        child.Parents.Add(factorTree);
                        factorTree.Children.Add(child);
                    }
                }

                factorTreeNodes.Add(webFactorTree.Factor.Id, factorTree);
            }

            return factorTree;
        }

        /// <summary>
        /// Get all factor trees.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <returns>All factor trees.</returns>
        public virtual FactorTreeNodeList GetFactorTrees(IUserContext userContext)
        {
            List<WebFactorTreeNode> webFactorTrees;

            CheckTransaction(userContext);
            webFactorTrees = WebServiceProxy.TaxonAttributeService.GetFactorTrees(GetClientInformation(userContext));
            return GetFactorTrees(userContext, webFactorTrees);
        }

        /// <summary>
        /// Get WebFactorTreeSearchCriteria form IFactorTreeSearchCriteria.
        /// </summary>
        /// <param name="searchCriteria">A WebFactorTreeSearchCriteria instance.</param>
        /// <returns></returns>
        private WebFactorTreeSearchCriteria GetFactorTreeSearchCriteria(IFactorTreeSearchCriteria searchCriteria)
        {
            WebFactorTreeSearchCriteria webFactorTreeSearchCriteria = new WebFactorTreeSearchCriteria { FactorIds = searchCriteria.FactorIds };

            return webFactorTreeSearchCriteria;
        }

        /// <summary>
        /// Get all factor trees that match search criteria.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="searchCriteria">The factor search criteria.</param>
        /// <returns>All factor trees.</returns>
        public virtual FactorTreeNodeList GetFactorTrees(IUserContext userContext,
                                                         IFactorTreeSearchCriteria searchCriteria)
        {
            List<WebFactorTreeNode> webFactorTrees;
            WebFactorTreeSearchCriteria webSearchCriteria;

            // Check arguments.
            CheckTransaction(userContext);
            searchCriteria.CheckNotNull("searchCriteria");

            webSearchCriteria = GetFactorTreeSearchCriteria(searchCriteria);
            webFactorTrees = WebServiceProxy.TaxonAttributeService.GetFactorTreesBySearchCriteria(GetClientInformation(userContext),
                                                                                                  webSearchCriteria);
            return GetFactorTrees(userContext, webFactorTrees);
        }

        /// <summary>
        /// Convert a list of WebFactorTreeNode instances
        /// to a list of IFactorTreeNode instances.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="webFactorTrees">A list of WebFactorTreeNode instances.</param>
        /// <returns>A list of IFactorTreeNode instances.</returns>
        private FactorTreeNodeList GetFactorTrees(IUserContext userContext,
                                                  List<WebFactorTreeNode> webFactorTrees)
        {
            FactorDataTypeList factorDataTypes;
            FactorOriginList factorOrigins;
            FactorTreeNodeList factorTrees;
            FactorUpdateModeList factorUpdateModes;
            Hashtable factorTreeNodes;

            factorDataTypes = CoreData.FactorManager.GetFactorDataTypes(userContext);
            factorOrigins = CoreData.FactorManager.GetFactorOrigins(userContext);
            factorUpdateModes = CoreData.FactorManager.GetFactorUpdateModes(userContext);
            factorTrees = new FactorTreeNodeList();
            factorTreeNodes = new Hashtable();
            if (webFactorTrees.IsNotEmpty())
            {
                foreach (WebFactorTreeNode webFactorTree in webFactorTrees)
                {
                    factorTrees.Add(GetFactorTree(userContext, webFactorTree, factorTreeNodes, factorDataTypes, factorOrigins, factorUpdateModes));
                }
            }

            if (factorTrees.IsNotEmpty())
            {
                factorTrees.Sort();
            }

            return factorTrees;
        }
    }
}