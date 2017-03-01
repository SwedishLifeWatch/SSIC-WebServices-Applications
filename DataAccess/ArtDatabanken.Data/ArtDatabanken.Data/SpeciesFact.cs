using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Class that handles species fact information.
    /// </summary>
    [Serializable]
    public class SpeciesFact : ISpeciesFact
    {
        /// <summary>
        /// Event that fires when species facts are updated.
        /// </summary>
        [field: NonSerialized]
        public event SpeciesFactUpdateEventHandler UpdateEvent;

        private Boolean _hasId;
        private Boolean _shouldBeSaved;
        private String _identifier;
        private ITaxon _taxon;
        private IIndividualCategory _individualCategory;
        private IFactor _factor;
        private ITaxon _host;
        private Boolean _hasHost;
        private Boolean _hasModifiedDate;
        private IPeriod _period;
        private Boolean _hasPeriod;
        private IReference _oldReference;
        private IReference _reference;
        private String _modifiedBy;
        private DateTime _modifiedDate;
        private ISpeciesFactQuality _oldQuality;
        private ISpeciesFactQuality _quality;
        private ISpeciesFactField[] _fieldArray;
        private SpeciesFactFieldList _fields;
        private SpeciesFactFieldList _substantialFields;
        private ISpeciesFactField _mainField;

        /// <summary>
        /// Creates a species fact instance with no data from web service.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="taxon">Taxon object of the species fact.</param>
        /// <param name="individualCategory">Individual category object of the species fact.</param>
        /// <param name="factor">Factor object of the species fact.</param>
        /// <param name="host">Host taxon object of the species fact.</param>
        /// <param name="period">Period object of the species fact.</param>
        public SpeciesFact(IUserContext userContext,
                           ITaxon taxon,
                           IIndividualCategory individualCategory,
                           IFactor factor,
                           ITaxon host,
                           IPeriod period)
        {
            Int32 fieldIndex;

            Id = -1;
            _fields = null;
            _substantialFields = null;
            _mainField = null;
            _hasId = false;

            _identifier = CoreData.SpeciesFactManager.GetSpeciesFactIdentifier(taxon,
                                                                               individualCategory,
                                                                               factor,
                                                                               host,
                                                                               period);
            _taxon = taxon;
            _individualCategory = individualCategory;
            _factor = factor;
            _host = host;
            _hasHost = _host.IsNotNull();
            _period = period;
            _hasPeriod = _period.IsNotNull();
            _quality = CoreData.SpeciesFactManager.GetDefaultSpeciesFactQuality(userContext);
            _oldQuality = _quality;
            _reference = null;
            _oldReference = null;
            _modifiedBy = null;
            _modifiedDate = DateTime.MinValue;
            _hasModifiedDate = false;

            if (!_factor.UpdateMode.IsHeader)
            {
                _fields = new SpeciesFactFieldList();
                _substantialFields = new SpeciesFactFieldList();
                _fieldArray = new ISpeciesFactField[CoreData.FactorManager.GetFactorFieldMaxCount()];
                for (fieldIndex = 0; fieldIndex < CoreData.FactorManager.GetFactorFieldMaxCount(); fieldIndex++)
                {
                    _fieldArray[fieldIndex] = null;
                }

                foreach (FactorField factorField in _factor.DataType.Fields)
                {
                    ISpeciesFactField field;

                    field = new SpeciesFactField(this, factorField, false, null);
                    _fields.Add(field);
                    _fieldArray[field.Index] = field;

                    if (factorField.IsSubstantial)
                    {
                        _substantialFields.Add(field);
                    }

                    if (factorField.IsMain)
                    {
                        _mainField = field;
                    }
                }
            }

            Fields.Sort();
            _shouldBeSaved = AllowUpdate;
        }

        /// <summary>
        /// Creates a species fact instance with data from web service.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="id">Id of the species fact.</param>
        /// <param name="taxonId">Taxon Id of the species fact.</param>
        /// <param name="individualCategoryId">Individual Category Id of the species fact.</param>
        /// <param name="factorId">Factor Id of the species fact.</param>
        /// <param name="hostId">Taxon Id of the host taxon associated with the species fact.</param>
        /// <param name="hasHost">Indicates if this species fact has a host.</param>
        /// <param name="periodId">Period Id of the species fact.</param>
        /// <param name="hasPeriod">Indicates if this species fact has a period.</param>
        /// <param name="fieldValue1">Field value of field 1 for the species fact.</param>
        /// <param name="hasFieldValue1">Indicates if field 1 has a value.</param>
        /// <param name="fieldValue2">Field value of field 2 for the species fact.</param>
        /// <param name="hasFieldValue2">Indicates if field 2 has a value.</param>
        /// <param name="fieldValue3">Field value of field 3 for the species fact.</param>
        /// <param name="hasFieldValue3">Indicates if field 3 has a value.</param>
        /// <param name="fieldValue4">Field value of field 4 for the species fact.</param>
        /// <param name="hasFieldValue4">Indicates if field 4 has a value.</param>
        /// <param name="fieldValue5">Field value of field 5 for the species fact.</param>
        /// <param name="hasFieldValue5">Indicates if field 5 has a value.</param>
        /// <param name="qualityId">Quality Id of the species fact.</param>
        /// <param name="referenceId">Reference id of the species fact.</param>
        /// <param name="modifiedBy">Full Name of the update user of the species fact.</param>
        /// <param name="modifiedDate">Update date of the species fact.</param>
        public SpeciesFact(IUserContext userContext,
                           Int32 id,
                           Int32 taxonId,
                           Int32 individualCategoryId,
                           Int32 factorId,
                           Int32 hostId,
                           Boolean hasHost,
                           Int32 periodId,
                           Boolean hasPeriod,
                           Double fieldValue1,
                           Boolean hasFieldValue1,
                           Double fieldValue2,
                           Boolean hasFieldValue2,
                           Double fieldValue3,
                           Boolean hasFieldValue3,
                           String fieldValue4,
                           Boolean hasFieldValue4,
                           String fieldValue5,
                           Boolean hasFieldValue5,
                           Int32 qualityId,
                           Int32 referenceId,
                           String modifiedBy,
                           DateTime modifiedDate)
        {
            Int32 fieldIndex;

            Id = id;
            _fields = null;
            _substantialFields = null;
            _mainField = null;
            _hasId = true;
            _identifier = CoreData.SpeciesFactManager.GetSpeciesFactIdentifier(taxonId,
                                                                               individualCategoryId,
                                                                               factorId,
                                                                               hasHost,
                                                                               hostId,
                                                                               hasPeriod,
                                                                               periodId);

            _taxon = CoreData.TaxonManager.GetTaxon(userContext, taxonId);
            _individualCategory = CoreData.FactorManager.GetIndividualCategory(userContext, individualCategoryId);
            _factor = CoreData.FactorManager.GetFactor(userContext, factorId);

            if (hasHost)
            {
                _host = CoreData.TaxonManager.GetTaxon(userContext, hostId);
                _hasHost = true;
            }
            else
            {
                if (_factor.IsTaxonomic)
                {
                    _host = CoreData.TaxonManager.GetTaxon(userContext, 0);
                }
                else
                {
                    _host = null;
                }

                _hasHost = _host.IsNotNull();
            }

            if (hasPeriod)
            {
                _period = CoreData.FactorManager.GetPeriod(userContext, periodId);
            }
            else
            {
                _period = null;
            }

            _hasPeriod = hasPeriod;

            _quality = CoreData.SpeciesFactManager.GetSpeciesFactQuality(userContext, qualityId);
            _oldQuality = _quality;
            _reference = CoreData.ReferenceManager.GetReference(userContext, referenceId);
            _oldReference = _reference;
            _modifiedBy = modifiedBy;
            _modifiedDate = modifiedDate;
            _hasModifiedDate = true;

            if (!_factor.UpdateMode.IsHeader)
            {
                _fields = new SpeciesFactFieldList();
                _substantialFields = new SpeciesFactFieldList();
                _fieldArray = new ISpeciesFactField[CoreData.FactorManager.GetFactorFieldMaxCount()];
                for (fieldIndex = 0; fieldIndex < CoreData.FactorManager.GetFactorFieldMaxCount(); fieldIndex++)
                {
                    _fieldArray[fieldIndex] = null;
                }

                foreach (FactorField factorField in _factor.DataType.Fields)
                {
                    Boolean hasFieldValue;
                    SpeciesFactField field;
                    Object fieldValue;

                    switch (factorField.Index)
                    {
                        case 0:
                            fieldValue = fieldValue1;
                            hasFieldValue = hasFieldValue1;
                            break;
                        case 1:
                            fieldValue = fieldValue2;
                            hasFieldValue = hasFieldValue2;
                            break;
                        case 2:
                            fieldValue = fieldValue3;
                            hasFieldValue = hasFieldValue3;
                            break;
                        case 3:
                            fieldValue = fieldValue4;
                            hasFieldValue = hasFieldValue4;
                            break;
                        case 4:
                            fieldValue = fieldValue5;
                            hasFieldValue = hasFieldValue5;
                            break;
                        default:
                            throw new Exception("Unknown data field!");
                    }

                    field = new SpeciesFactField(this, factorField, hasFieldValue, fieldValue);
                    _fields.Add(field);
                    _fieldArray[field.Index] = field;

                    if (factorField.IsSubstantial)
                    {
                        _substantialFields.Add(field);
                    }

                    if (factorField.IsMain)
                    {
                        _mainField = field;
                    }
                }
            }

            Fields.Sort();
            _shouldBeSaved = AllowUpdate;
        }

        /// <summary>
        /// Creates a species fact instance with data from web service.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="id">Id of the species fact.</param>
        /// <param name="taxon">Taxon of the species fact.</param>
        /// <param name="individualCategoryId">Individual Category Id of the species fact.</param>
        /// <param name="factorId">Factor id of the species fact.</param>
        /// <param name="host">Host taxon associated with the species fact.</param>
        /// <param name="hasHost">Indicates if this species fact has a host.</param>
        /// <param name="periodId">Period Id of the species fact.</param>
        /// <param name="hasPeriod">Indicates if this species fact has a period.</param>
        /// <param name="fieldValue1">Field value of field 1 for the species fact.</param>
        /// <param name="hasFieldValue1">Indicates if field 1 has a value.</param>
        /// <param name="fieldValue2">Field value of field 2 for the species fact.</param>
        /// <param name="hasFieldValue2">Indicates if field 2 has a value.</param>
        /// <param name="fieldValue3">Field value of field 3 for the species fact.</param>
        /// <param name="hasFieldValue3">Indicates if field 3 has a value.</param>
        /// <param name="fieldValue4">Field value of field 4 for the species fact.</param>
        /// <param name="hasFieldValue4">Indicates if field 4 has a value.</param>
        /// <param name="fieldValue5">Field value of field 5 for the species fact.</param>
        /// <param name="hasFieldValue5">Indicates if field 5 has a value.</param>
        /// <param name="qualityId">Quality Id of the species fact.</param>
        /// <param name="referenceId">Reference id of the species fact.</param>
        /// <param name="modifiedBy">Full name of the update user of the species fact.</param>
        /// <param name="modifiedDate">Update date of the species fact.</param>
        public SpeciesFact(IUserContext userContext,
                           Int32 id,
                           ITaxon taxon,
                           Int32 individualCategoryId,
                           Int32 factorId,
                           ITaxon host,
                           Boolean hasHost,
                           Int32 periodId,
                           Boolean hasPeriod,
                           Double fieldValue1,
                           Boolean hasFieldValue1,
                           Double fieldValue2,
                           Boolean hasFieldValue2,
                           Double fieldValue3,
                           Boolean hasFieldValue3,
                           String fieldValue4,
                           Boolean hasFieldValue4,
                           String fieldValue5,
                           Boolean hasFieldValue5,
                           Int32 qualityId,
                           Int32 referenceId,
                           String modifiedBy,
                           DateTime modifiedDate)
        {
            Int32 hostId;
            Int32 fieldIndex;

            Id = id;
            _fields = null;
            _substantialFields = null;
            _mainField = null;
            _hasId = true;
            hostId = 0;
            if (hasHost)
            {
                hostId = host.Id;
            }

            _identifier = CoreData.SpeciesFactManager.GetSpeciesFactIdentifier(taxon.Id,
                                                                               individualCategoryId,
                                                                               factorId,
                                                                               hasHost,
                                                                               hostId,
                                                                               hasPeriod,
                                                                               periodId);

            _taxon = taxon;
            _individualCategory = CoreData.FactorManager.GetIndividualCategory(userContext, individualCategoryId);
            _factor = CoreData.FactorManager.GetFactor(userContext, factorId);

            if (hasHost)
            {
                _host = host;
                _hasHost = true;
            }
            else
            {
                if (_factor.IsTaxonomic)
                {
                    _host = CoreData.TaxonManager.GetTaxon(userContext, 0);
                }
                else
                {
                    _host = null;
                }

                _hasHost = _host.IsNotNull();
            }

            if (hasPeriod)
            {
                _period = CoreData.FactorManager.GetPeriod(userContext, periodId);
            }
            else
            {
                _period = null;
            }

            _hasPeriod = hasPeriod;

            _quality = CoreData.SpeciesFactManager.GetSpeciesFactQuality(userContext, qualityId);
            _oldQuality = _quality;
            _reference = CoreData.ReferenceManager.GetReference(userContext, referenceId);
            _oldReference = _reference;
            _modifiedBy = modifiedBy;
            _modifiedDate = modifiedDate;
            _hasModifiedDate = true;

            if (!_factor.UpdateMode.IsHeader)
            {
                _fields = new SpeciesFactFieldList();
                _substantialFields = new SpeciesFactFieldList();
                _fieldArray = new ISpeciesFactField[CoreData.FactorManager.GetFactorFieldMaxCount()];
                for (fieldIndex = 0; fieldIndex < CoreData.FactorManager.GetFactorFieldMaxCount(); fieldIndex++)
                {
                    _fieldArray[fieldIndex] = null;
                }

                foreach (FactorField factorField in _factor.DataType.Fields)
                {
                    Boolean hasFieldValue;
                    SpeciesFactField field;
                    Object fieldValue;

                    switch (factorField.Index)
                    {
                        case 0:
                            fieldValue = fieldValue1;
                            hasFieldValue = hasFieldValue1;
                            break;
                        case 1:
                            fieldValue = fieldValue2;
                            hasFieldValue = hasFieldValue2;
                            break;
                        case 2:
                            fieldValue = fieldValue3;
                            hasFieldValue = hasFieldValue3;
                            break;
                        case 3:
                            fieldValue = fieldValue4;
                            hasFieldValue = hasFieldValue4;
                            break;
                        case 4:
                            fieldValue = fieldValue5;
                            hasFieldValue = hasFieldValue5;
                            break;
                        default:
                            throw new Exception("Unknown data field!");
                    }

                    field = new SpeciesFactField(this, factorField, hasFieldValue, fieldValue);
                    _fields.Add(field);
                    _fieldArray[field.Index] = field;

                    if (factorField.IsSubstantial)
                    {
                        _substantialFields.Add(field);
                    }

                    if (factorField.IsMain)
                    {
                        _mainField = field;
                    }
                }
            }

            Fields.Sort();
            _shouldBeSaved = AllowUpdate;
        }

        /// <summary>
        /// Creates a species fact instance with data from data source.
        /// </summary>
        /// <param name="id">Id of the species fact.</param>
        /// <param name="taxon">Taxon of the species fact.</param>
        /// <param name="individualCategory">Individual Category of the species fact.</param>
        /// <param name="factor">Factor of the species fact.</param>
        /// <param name="host">Host taxon associated with the species fact.</param>
        /// <param name="period">Period of the species fact.</param>
        /// <param name="fieldValue1">Field value of field 1 for the species fact.</param>
        /// <param name="hasFieldValue1">Indicates if field 1 has a value.</param>
        /// <param name="fieldValue2">Field value of field 2 for the species fact.</param>
        /// <param name="hasFieldValue2">Indicates if field 2 has a value.</param>
        /// <param name="fieldValue3">Field value of field 3 for the species fact.</param>
        /// <param name="hasFieldValue3">Indicates if field 3 has a value.</param>
        /// <param name="fieldValue4">Field value of field 4 for the species fact.</param>
        /// <param name="hasFieldValue4">Indicates if field 4 has a value.</param>
        /// <param name="fieldValue5">Field value of field 5 for the species fact.</param>
        /// <param name="hasFieldValue5">Indicates if field 5 has a value.</param>
        /// <param name="quality">Quality of the species fact.</param>
        /// <param name="reference">Reference of the species fact.</param>
        /// <param name="modifiedBy">Full name of the update user of the species fact.</param>
        /// <param name="modifiedDate">Update date of the species fact.</param>
        public SpeciesFact(Int32 id,
                           ITaxon taxon,
                           IIndividualCategory individualCategory,
                           IFactor factor,
                           ITaxon host,
                           IPeriod period,
                           Double fieldValue1,
                           Boolean hasFieldValue1,
                           Double fieldValue2,
                           Boolean hasFieldValue2,
                           Double fieldValue3,
                           Boolean hasFieldValue3,
                           String fieldValue4,
                           Boolean hasFieldValue4,
                           String fieldValue5,
                           Boolean hasFieldValue5,
                           ISpeciesFactQuality quality,
                           IReference reference,
                           String modifiedBy,
                           DateTime modifiedDate)
        {
            Int32 fieldIndex;

            Id = id;
            _fields = null;
            _substantialFields = null;
            _mainField = null;
            _hasId = true;
            _identifier = CoreData.SpeciesFactManager.GetSpeciesFactIdentifier(taxon,
                                                                               individualCategory,
                                                                               factor,
                                                                               host,
                                                                               period);

            _taxon = taxon;
            _individualCategory = individualCategory;
            _factor = factor;
            _host = host;
            _hasHost = host.IsNotNull();
            _period = period;
            _hasPeriod = period.IsNotNull();
            _quality = quality;
            _oldQuality = quality;
            _reference = reference;
            _oldReference = reference;
            _modifiedBy = modifiedBy;
            _modifiedDate = modifiedDate;
            _hasModifiedDate = true;

            if (!_factor.UpdateMode.IsHeader)
            {
                _fields = new SpeciesFactFieldList();
                _substantialFields = new SpeciesFactFieldList();
                _fieldArray = new ISpeciesFactField[CoreData.FactorManager.GetFactorFieldMaxCount()];
                for (fieldIndex = 0; fieldIndex < CoreData.FactorManager.GetFactorFieldMaxCount(); fieldIndex++)
                {
                    _fieldArray[fieldIndex] = null;
                }

                foreach (FactorField factorField in _factor.DataType.Fields)
                {
                    Boolean hasFieldValue;
                    SpeciesFactField field;
                    Object fieldValue;

                    switch (factorField.Index)
                    {
                        case 0:
                            fieldValue = fieldValue1;
                            hasFieldValue = hasFieldValue1;
                            break;
                        case 1:
                            fieldValue = fieldValue2;
                            hasFieldValue = hasFieldValue2;
                            break;
                        case 2:
                            fieldValue = fieldValue3;
                            hasFieldValue = hasFieldValue3;
                            break;
                        case 3:
                            fieldValue = fieldValue4;
                            hasFieldValue = hasFieldValue4;
                            break;
                        case 4:
                            fieldValue = fieldValue5;
                            hasFieldValue = hasFieldValue5;
                            break;
                        default:
                            throw new Exception("Unknown data field!");
                    }

                    field = new SpeciesFactField(this, factorField, hasFieldValue, fieldValue);
                    _fields.Add(field);
                    _fieldArray[field.Index] = field;

                    if (factorField.IsSubstantial)
                    {
                        _substantialFields.Add(field);
                    }

                    if (factorField.IsMain)
                    {
                        _mainField = field;
                    }
                }
            }

            Fields.Sort();
            _shouldBeSaved = AllowUpdate;
        }

        /// <summary>
        /// Test if automatic update is allowed.
        /// </summary>
        public Boolean AllowAutomaticUpdate
        {
            get
            {
                // Temporary check to avoid periodic data
                // where default IndividualCategory is not selected.
                // ArtFakta web application can not handle this
                // combination of data.
                if (_period.IsNotNull() &&
                    _individualCategory.Id != (Int32)(IndividualCategoryId.Default))
                {
                    return false;
                }

                if (_factor.IsPeriodic)
                {
                    return _period.AllowUpdate &&
                           _factor.UpdateMode.AllowAutomaticUpdate;
                }
                else
                {
                    return _factor.UpdateMode.AllowAutomaticUpdate;
                }
            }
        }

        /// <summary>
        /// Test if manual update is allowed.
        /// </summary>
        public Boolean AllowManualUpdate
        {
            get
            {
                // Temporary check to avoid periodic data
                // where default IndividualCategory is not selected.
                // ArtFakta web application can not handle this
                // combination of data.
                if (_period.IsNotNull() &&
                    _individualCategory.Id != (Int32)(IndividualCategoryId.Default))
                {
                    return false;
                }

                if (_factor.IsPeriodic)
                {
                    return _period.AllowUpdate &&
                           _factor.UpdateMode.AllowManualUpdate;
                }
                else
                {
                    return _factor.UpdateMode.AllowManualUpdate;
                }
            }
        }

        /// <summary>
        /// Test if update is allowed.
        /// </summary>
        public Boolean AllowUpdate
        {
            get
            {
                return AllowManualUpdate || AllowAutomaticUpdate;
            }
        }

        /// <summary>
        /// Check that automatic update is allowed.
        /// </summary>
        public void CheckAutomaticUpdate()
        {
            if (!AllowAutomaticUpdate)
            {
                throw new Exception("Automatic update of species fact with identifier=" +
                                    Identifier + " is not allowed");
            }
        }

        /// <summary>
        /// Check that manual update is allowed.
        /// </summary>
        public void CheckManualUpdate()
        {
            if (!AllowManualUpdate)
            {
                throw new Exception("Manual update of species fact with identifier=" +
                                    Identifier + " is not allowed");
            }
        }

        /// <summary>
        /// Check that update is allowed.
        /// </summary>
        public void CheckUpdate()
        {
            if (!AllowUpdate)
            {
                throw new Exception("Update of species fact with identifier=" +
                                    Identifier + " is not allowed");
            }
        }

        /// <summary>
        /// Get the factor of this species fact object.
        /// </summary>
        public IFactor Factor
        {
            get { return _factor; }
        }

        /// <summary>
        /// Get field 1 of this species fact object.
        /// </summary>
        public ISpeciesFactField Field1
        {
            get { return _fieldArray[0]; }
        }

        /// <summary>
        /// Get field 2 of this species fact object.
        /// </summary>
        public ISpeciesFactField Field2
        {
            get { return _fieldArray[1]; }
        }

        /// <summary>
        /// Get field 3 of this species fact object.
        /// </summary>
        public ISpeciesFactField Field3
        {
            get { return _fieldArray[2]; }
        }

        /// <summary>
        /// Get field 4 of this species fact object.
        /// </summary>
        public ISpeciesFactField Field4
        {
            get { return _fieldArray[3]; }
        }

        /// <summary>
        /// Get field 5 of this species fact object.
        /// </summary>
        public ISpeciesFactField Field5
        {
            get { return _fieldArray[4]; }
        }

        /// <summary>
        /// Get the fields of this species fact object.
        /// </summary>
        public SpeciesFactFieldList Fields
        {
            get { return _fields; }
        }

        /// <summary>
        /// Indicates whether or not this species fact has changed.
        /// </summary>
        public Boolean HasChanged
        {
            get
            {
                Boolean hasChanged;

                hasChanged = false;

                // Test if data has changed.
                if ((_oldReference.IsNull() != _reference.IsNull()) ||
                    (_oldReference.IsNotNull() &&
                     _reference.IsNotNull() &&
                     (_oldReference.Id != _reference.Id)))
                {
                    hasChanged = true;
                }

                if (_oldQuality.AreNotEqual(_quality))
                {
                    hasChanged = true;
                }

                foreach (SpeciesFactField speciesFactField in _fields)
                {
                    if (speciesFactField.HasChanged)
                    {
                        hasChanged = true;
                    }
                }

                if (hasChanged)
                {
                    // Test if no significant data has changed
                    // in a none existing (not saved to database)
                    // SpeciesFact.
                    if (!HasId &&
                        !MainField.HasValue &&
                        (!HasField1 || !Field1.HasValue) &&
                        (!HasField2 || !Field2.HasValue) &&
                        (!HasField3 || !Field3.HasValue))
                    {
                        hasChanged = false;
                    }
                }

                return hasChanged;
            }
        }

        /// <summary>
        /// Test if this species fact uses field 1.
        /// </summary>
        public Boolean HasField1
        {
            get { return Field1.IsNotNull(); }
        }

        /// <summary>
        /// Test if this species fact uses field 2.
        /// </summary>
        public Boolean HasField2
        {
            get { return Field2.IsNotNull(); }
        }

        /// <summary>
        /// Test if this species fact uses field 3.
        /// </summary>
        public Boolean HasField3
        {
            get { return Field3.IsNotNull(); }
        }

        /// <summary>
        /// Test if this species fact uses field 4.
        /// </summary>
        public Boolean HasField4
        {
            get { return Field4.IsNotNull(); }
        }

        /// <summary>
        /// Test if this species fact uses field 5.
        /// </summary>
        public Boolean HasField5
        {
            get { return Field5.IsNotNull(); }
        }

        /// <summary>
        /// Indicates whether or not an host object has
        /// been associated with this species fact.
        /// </summary>
        public Boolean HasHost
        {
            get { return _hasHost; }
        }

        /// <summary>
        /// Indicates whether or not this species fact has an id.
        /// </summary>
        public Boolean HasId
        {
            get { return _hasId; }
        }

        /// <summary>
        /// Indicates whether or not a update date has been
        /// associated with this species fact .
        /// </summary>
        public Boolean HasModifiedDate
        {
            get { return _hasModifiedDate; }
        }

        /// <summary>
        /// Indicates whether or not a period object has been
        /// associated with this species fact.
        /// </summary>
        public Boolean HasPeriod
        {
            get { return _hasPeriod; }
        }

        /// <summary>
        /// Indicates whether or not a reference id has
        /// been associated with this species fact.
        /// </summary>
        public Boolean HasReference
        {
            get { return _reference.IsNotNull(); }
        }

        /// <summary>
        /// Get the host of this species fact object.
        /// </summary>
        public ITaxon Host
        {
            get { return _host; }
        }

        /// <summary>
        /// Id for this species fact.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Get the unique identifier for this species fact.
        /// </summary>
        public String Identifier
        {
            get { return _identifier; }
        }

        /// <summary>
        /// Get the individual category of this species fact object.
        /// </summary>
        public IIndividualCategory IndividualCategory
        {
            get { return _individualCategory; }
        }

        /// <summary>
        /// Get the main field of this species fact object.
        /// </summary>
        public ISpeciesFactField MainField
        {
            get { return _mainField; }
        }

        /// <summary>
        /// Get the update date of this species fact object.
        /// </summary>
        public DateTime ModifiedDate
        {
            get { return _modifiedDate; }
        }

        /// <summary>
        /// Get the update user full name of this species fact object.
        /// </summary>
        public String ModifiedBy
        {
            get { return _modifiedBy; }
        }

        /// <summary>
        /// Get the Period of this species fact object.
        /// </summary>
        public IPeriod Period
        {
            get { return _period; }
        }

        /// <summary>
        /// Handle the species fact quality of this species fact.
        /// </summary>
        public ISpeciesFactQuality Quality
        {
            get
            {
                return _quality;
            }

            set
            {
                CheckUpdate();
                _quality = value;
                FireUpdateEvent();
            }
        }

        /// <summary>
        /// Handle the Reference of this species fact object.
        /// </summary>
        public IReference Reference
        {
            get
            {
                return _reference;
            }

            set
            {
                CheckUpdate();
                _reference = value;
                FireUpdateEvent();
            }
        }

        /// <summary>
        /// Indication if this species fact should be deleted
        /// from database.
        /// </summary>
        public Boolean ShouldBeDeleted
        {
            get
            {
                return (HasId &&
                        !MainField.HasValue &&
                        (!HasField1 || !Field1.HasValue) &&
                        (!HasField2 || !Field2.HasValue) &&
                        (!HasField3 || !Field3.HasValue));
            }
        }

        /// <summary>
        /// Get indication if this species fact should be saved to database.
        /// This property is used to handle the
        /// case when a user changes an existing (in database)
        /// species fact to default values and the species fact
        /// should be removed from the database to save space.
        /// </summary>
        public Boolean ShouldBeSaved
        {
            // Todo: Add code that test for defaults values
            // in existing species fact.
            get { return _shouldBeSaved; }
        }

        /// <summary>
        /// Get the substantial fields of this species fact object.
        /// </summary>
        public SpeciesFactFieldList SubstantialFields
        {
            get { return _substantialFields; }
        }

        /// <summary>
        /// Get the taxon of this species fact object.
        /// </summary>
        public ITaxon Taxon
        {
            get { return _taxon; }
        }

        /// <summary>
        /// Send information about species fact update.
        /// </summary>
        public void FireUpdateEvent()
        {
            if (UpdateEvent != null)
            {
                UpdateEvent.Invoke(this);
            }
        }

        /// <summary>
        /// Reset species fact that has been deleted from database.
        /// Set all values to default or null.
        /// </summary>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        public void Reset(IUserContext userContext)
        {
            Id = -1;
            _hasId = false;
            _quality = CoreData.SpeciesFactManager.GetDefaultSpeciesFactQuality(userContext);
            _oldQuality = _quality;
            this._reference = null;
            this._oldReference = null;
            _modifiedBy = null;
            _modifiedDate = DateTime.MinValue;
            _hasModifiedDate = false;
            foreach (ISpeciesFactField field in _fields)
            {
                field.Reset();
            }
        }

        /// <summary>
        /// Update species fact with new data from database.
        /// </summary>
        /// <param name="id">Id of the species fact.</param>
        /// <param name="reference">Reference of the species fact.</param>
        /// <param name="modifiedDate">Modified date of the species fact.</param>
        /// <param name="modifiedBy">Name of the update user of the species fact.</param>
        /// <param name="hasFieldValue1">Indicates if field 1 has a value.</param>
        /// <param name="fieldValue1">Field value of field 1 for the species fact.</param>
        /// <param name="hasFieldValue2">Indicates if field 2 has a value.</param>
        /// <param name="fieldValue2">Field value of field 2 for the species fact.</param>
        /// <param name="hasFieldValue3">Indicates if field 3 has a value.</param>
        /// <param name="fieldValue3">Field value of field 3 for the species fact.</param>
        /// <param name="hasFieldValue4">Indicates if field 4 has a value.</param>
        /// <param name="fieldValue4">Field value of field 4 for the species fact.</param>
        /// <param name="hasFieldValue5">Indicates if field 5 has a value.</param>
        /// <param name="fieldValue5">Field value of field 5 for the species fact.</param>
        /// <param name="quality">Quality of the species fact.</param>
        public void Update(Int32 id,
                           IReference reference,
                           DateTime modifiedDate,
                           String modifiedBy,
                           Boolean hasFieldValue1,
                           Double fieldValue1,
                           Boolean hasFieldValue2,
                           Double fieldValue2,
                           Boolean hasFieldValue3,
                           Double fieldValue3,
                           Boolean hasFieldValue4,
                           String fieldValue4,
                           Boolean hasFieldValue5,
                           String fieldValue5,
                           ISpeciesFactQuality quality)
        {
            Id = id;
            _hasId = true;
            _quality = quality;
            _oldQuality = _quality;
            _reference = reference;
            _oldReference = reference;
            _modifiedBy = modifiedBy;
            _modifiedDate = modifiedDate;
            _hasModifiedDate = true;

            foreach (ISpeciesFactField field in _fields)
            {
                switch (field.Index)
                {
                    case 0:
                        field.Update(hasFieldValue1, fieldValue1);
                        break;
                    case 1:
                        field.Update(hasFieldValue2, fieldValue2);
                        break;
                    case 2:
                        field.Update(hasFieldValue3, fieldValue3);
                        break;
                    case 3:
                        field.Update(hasFieldValue4, fieldValue4);
                        break;
                    case 4:
                        field.Update(hasFieldValue5, fieldValue5);
                        break;
                    default:
                        throw new Exception("Unknown data field!");
                }
            }
        }
    }
}