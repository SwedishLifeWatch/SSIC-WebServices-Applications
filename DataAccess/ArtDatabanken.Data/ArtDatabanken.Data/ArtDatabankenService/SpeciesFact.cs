using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// Delegate for species fact update event.
    /// </summary>
    public delegate void SpeciesFactUpdateEventHandler(SpeciesFact speciesFact);

    /// <summary>
    /// This class represents a species fact
    /// </summary>
    [Serializable]
    public class SpeciesFact : DataSortOrder
    {
        /// <summary>
        /// Event that fires when species facts are updated.
        /// </summary>
        [field: NonSerialized]
        public event SpeciesFactUpdateEventHandler UpdateEvent;

        private Boolean _hasId;
        private Boolean _shouldBeSaved;
        private String _identifier;
        private Taxon _taxon;
        private IndividualCategory _individualCategory;
        private Factor _factor;
        private Taxon _host;
        private Boolean _hasHost;
        private Period _period;
        private Boolean _hasPeriod;
        private Reference _oldReference;
        private Reference _reference;
        private String _updateUserFullName;
        private DateTime _updateDate;
        private Boolean _hasUpdateDate;
        private SpeciesFactQuality _oldQuality;
        private SpeciesFactQuality _quality;
        private SpeciesFactField[] _fieldArray;
        private SpeciesFactFieldList _fields;
        private SpeciesFactFieldList _substantialFields;
        private SpeciesFactField _mainField;

        /// <summary>
        /// Creates a species fact instance with no data from web service.
        /// </summary>
        /// <param name="taxon">Taxon object of the species fact</param>
        /// <param name="individualCategory">Individual category object of the species fact</param>
        /// <param name="factor">Factor object of the species fact</param>
        /// <param name="host">Host taxon object of the species fact</param>
        /// <param name="period">Period object of the species fact</param>
        public SpeciesFact(
            Taxon taxon,
            IndividualCategory individualCategory,
            Factor factor,
            Taxon host,
            Period period)
            : base(-1, -1)
        {
            Int32 fieldIndex;

            _fields = null;
            _substantialFields = null;
            _mainField = null;
            _hasId = false;

            _identifier = SpeciesFactManager.GetSpeciesFactIdentifier(
                taxon,
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
            _quality = SpeciesFactManager.GetDefaultSpeciesFactQuality();
            _oldQuality = _quality;
            _reference = null;
            _oldReference = null;
            _updateUserFullName = null;
            _updateDate = DateTime.MinValue;
            _hasUpdateDate = false;

            if (!_factor.FactorUpdateMode.IsHeader)
            {
                _fields = new SpeciesFactFieldList();
                _substantialFields = new SpeciesFactFieldList();
                _fieldArray = new SpeciesFactField[FactorManager.GetFactorFieldMaxCount()];
                for (fieldIndex = 0; fieldIndex < FactorManager.GetFactorFieldMaxCount(); fieldIndex++)
                {
                    _fieldArray[fieldIndex] = null;
                }

                foreach (FactorField factorField in _factor.FactorDataType.Fields)
                {
                    SpeciesFactField field;

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
            _shouldBeSaved = AllowUpdate;
        }

        /// <summary>
        /// Creates a species fact instance with data from web service.
        /// </summary>
        /// <param name="id">Id of the species fact</param>
        /// <param name="sortOrder">Sort order of the species fact</param>
        /// <param name="taxonId">Taxon Id of the species fact</param>
        /// <param name="individualCategoryId">Individual Category Id of the species fact</param>
        /// <param name="factorId">Foctor Id of the species fact</param>
        /// <param name="hostId">Taxon Id of the host taxon associated with the species fact</param>
        /// <param name="hasHost">Indicates if this species fact has a host.</param>
        /// <param name="periodId">Period Id of the species fact</param>
        /// <param name="hasPeriod">Indicates if this species fact has a period.</param>
        /// <param name="fieldValue1">Field value of field 1 for the species fact</param>
        /// <param name="hasFieldValue1">Indicates if field 1 has a value.</param>
        /// <param name="fieldValue2">Field value of field 2 for the species fact</param>
        /// <param name="hasFieldValue2">Indicates if field 2 has a value.</param>
        /// <param name="fieldValue3">Field value of field 3 for the species fact</param>
        /// <param name="hasFieldValue3">Indicates if field 3 has a value.</param>
        /// <param name="fieldValue4">Field value of field 4 for the species fact</param>
        /// <param name="hasFieldValue4">Indicates if field 4 has a value.</param>
        /// <param name="fieldValue5">Field value of field 5 for the species fact</param>
        /// <param name="hasFieldValue5">Indicates if field 5 has a value.</param>
        /// <param name="qualityId">Quality Id of the species fact</param>
        /// <param name="referenceId">Reference id of the species fact</param>
        /// <param name="updateUserFullName">Full Name of the pdate user of the species fact</param>
        /// <param name="updateDate">Update date of the species fact</param>
        public SpeciesFact(
            Int32 id,
            Int32 sortOrder,
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
            String updateUserFullName,
            DateTime updateDate)
            : base(id, sortOrder)
        {
            Int32 fieldIndex;

            _fields = null;
            _substantialFields = null;
            _mainField = null;
            _hasId = true;
            
            

            _identifier = SpeciesFactManager.GetSpeciesFactIdentifier(
                taxonId,
                individualCategoryId,
                factorId,
                hasHost,
                hostId,
                hasPeriod,
                periodId);

            _taxon = TaxonManager.GetTaxon(taxonId, TaxonInformationType.Basic);
            _individualCategory = IndividualCategoryManager.GetIndividualCategory(individualCategoryId);
            _factor = FactorManager.GetFactor(factorId);

            
            if (hasHost)
            {
                _host = TaxonManager.GetTaxon(hostId, TaxonInformationType.Basic);
                _hasHost = hasHost;
            }
            else
            {
                if (_factor.IsTaxonomic)
                {
                    _host = TaxonManager.GetTaxon(0, TaxonInformationType.Basic); ;
                }
                else
                {
                    _host = null;
                }
                _hasHost = _host.IsNotNull();
            }
            

            if (hasPeriod)
            {
               _period = PeriodManager.GetPeriod(periodId);
            }
            else
            {
               _period = null;
            }
            _hasPeriod = hasPeriod;

            _quality = SpeciesFactManager.GetSpeciesFactQuality(qualityId);
            _oldQuality = _quality;
            _reference = ReferenceManager.GetReference(referenceId);
            _oldReference = _reference;
            _updateUserFullName = updateUserFullName;
            _updateDate = updateDate;
            _hasUpdateDate = true;
            
            if (!_factor.FactorUpdateMode.IsHeader)
            {
                _fields = new SpeciesFactFieldList();
                _substantialFields = new SpeciesFactFieldList();
                _fieldArray = new SpeciesFactField[FactorManager.GetFactorFieldMaxCount()];
                for (fieldIndex = 0; fieldIndex < FactorManager.GetFactorFieldMaxCount(); fieldIndex++)
                {
                    _fieldArray[fieldIndex] = null;
                }

                foreach (FactorField factorField in _factor.FactorDataType.Fields)
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
            _shouldBeSaved = AllowUpdate;
        }

        /// <summary>
        /// Creates a species fact instance with data from web service.
        /// </summary>
        /// <param name="id">Id of the species fact</param>
        /// <param name="sortOrder">Sort order of the species fact</param>
        /// <param name="taxon">Taxon of the species fact</param>
        /// <param name="individualCategoryId">Individual Category Id of the species fact</param>
        /// <param name="factorId">Foctor Id of the species fact</param>
        /// <param name="host">Host taxon associated with the species fact</param>
        /// <param name="hasHost">Indicates if this species fact has a host.</param>
        /// <param name="periodId">Period Id of the species fact</param>
        /// <param name="hasPeriod">Indicates if this species fact has a period.</param>
        /// <param name="fieldValue1">Field value of field 1 for the species fact</param>
        /// <param name="hasFieldValue1">Indicates if field 1 has a value.</param>
        /// <param name="fieldValue2">Field value of field 2 for the species fact</param>
        /// <param name="hasFieldValue2">Indicates if field 2 has a value.</param>
        /// <param name="fieldValue3">Field value of field 3 for the species fact</param>
        /// <param name="hasFieldValue3">Indicates if field 3 has a value.</param>
        /// <param name="fieldValue4">Field value of field 4 for the species fact</param>
        /// <param name="hasFieldValue4">Indicates if field 4 has a value.</param>
        /// <param name="fieldValue5">Field value of field 5 for the species fact</param>
        /// <param name="hasFieldValue5">Indicates if field 5 has a value.</param>
        /// <param name="qualityId">Quality Id of the species fact</param>
        /// <param name="referenceId">Reference id of the species fact</param>
        /// <param name="updateUserFullName">Full Name of the pdate user of the species fact</param>
        /// <param name="updateDate">Update date of the species fact</param>
        public SpeciesFact(
            Int32 id,
            Int32 sortOrder,
            Taxon taxon,
            Int32 individualCategoryId,
            Int32 factorId,
            Taxon host,
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
            String updateUserFullName,
            DateTime updateDate)
            : base(id, sortOrder)
        {
            Int32 hostId;
            Int32 fieldIndex;

            _fields = null;
            _substantialFields = null;
            _mainField = null;
            _hasId = true;
            hostId = 0;
            if (hasHost)
            {
                hostId = host.Id;
            }

            _identifier = SpeciesFactManager.GetSpeciesFactIdentifier(
                taxon.Id,
                individualCategoryId,
                factorId,
                hasHost,
                hostId,
                hasPeriod,
                periodId);

            _taxon = taxon;
            _individualCategory = IndividualCategoryManager.GetIndividualCategory(individualCategoryId);
            _factor = FactorManager.GetFactor(factorId);

            if (hasHost)
            {
                _host = host;
                _hasHost = hasHost;
            }
            else
            {
                if (_factor.IsTaxonomic)
                {
                    _host = TaxonManager.GetTaxon(0, TaxonInformationType.Basic);
                }
                else
                {
                    _host = null;
                }
                _hasHost = _host.IsNotNull();
            }



            if (hasPeriod)
            {
                _period = PeriodManager.GetPeriod(periodId);
            }
            else
            {
                _period = null;
            }
            _hasPeriod = hasPeriod;

            _quality = SpeciesFactManager.GetSpeciesFactQuality(qualityId);
            _oldQuality = _quality;
            _reference = ReferenceManager.GetReference(referenceId);
            _oldReference = _reference;
            _updateUserFullName = updateUserFullName;
            _updateDate = updateDate;
            _hasUpdateDate = true;

            if (!_factor.FactorUpdateMode.IsHeader)
            {
                _fields = new SpeciesFactFieldList();
                _substantialFields = new SpeciesFactFieldList();
                _fieldArray = new SpeciesFactField[FactorManager.GetFactorFieldMaxCount()];
                for (fieldIndex = 0; fieldIndex < FactorManager.GetFactorFieldMaxCount(); fieldIndex++)
                {
                    _fieldArray[fieldIndex] = null;
                }

                foreach (FactorField factorField in _factor.FactorDataType.Fields)
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
                           _factor.FactorUpdateMode.AllowAutomaticUpdate;
                }
                else
                {
                    return _factor.FactorUpdateMode.AllowAutomaticUpdate;
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
                           _factor.FactorUpdateMode.AllowManualUpdate;
                }
                else
                {
                    return _factor.FactorUpdateMode.AllowManualUpdate;
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
        public Factor Factor
        {
            get { return _factor; }
        }

        /// <summary>
        /// Get field 1 of this species fact object.
        /// </summary>
        public SpeciesFactField Field1
        {
            get { return _fieldArray[0]; }
        }

        /// <summary>
        /// Get field 2 of this species fact object.
        /// </summary>
        public SpeciesFactField Field2
        {
            get { return _fieldArray[1]; }
        }

        /// <summary>
        /// Get field 3 of this species fact object.
        /// </summary>
        public SpeciesFactField Field3
        {
            get { return _fieldArray[2]; }
        }

        /// <summary>
        /// Get field 4 of this species fact object.
        /// </summary>
        public SpeciesFactField Field4
        {
            get { return _fieldArray[3]; }
        }

        /// <summary>
        /// Get field 5 of this species fact object.
        /// </summary>
        public SpeciesFactField Field5
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
                if (DataId.AreNotEqual(_oldReference, _reference))
                {
                    hasChanged = true;
                }
                if (DataId.AreNotEqual(_oldQuality, _quality))
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
        /// Indicates whether or not an host object has been associated with this species fact 
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
        /// Indicates whether or not a period object has been associated with this species fact 
        /// </summary>
        public Boolean HasPeriod
        {
            get { return _hasPeriod; }
        }

        /// <summary>
        /// Indicates whether or not a update date has been associated with this species fact 
        /// </summary>
        public Boolean HasUpdateDate
        {
            get { return _hasUpdateDate; }
        }

        /// <summary>
        /// Get the host of this species fact object.
        /// </summary>
        public Taxon Host
        {
            get { return _host; }
        }

        /// <summary>
        /// Get the unique identifier for this species fact
        /// </summary>
        public String Identifier
        {
            get { return _identifier; }
        }

        /// <summary>
        /// Get the individual category of this species fact object.
        /// </summary>
        public IndividualCategory IndividualCategory
        {
            get { return _individualCategory; }
        }

        /// <summary>
        /// Get the main field of this species fact object.
        /// </summary>
        public SpeciesFactField MainField
        {
            get { return _mainField; }
        }

        /// <summary>
        /// Get the Period of this species fact object.
        /// </summary>
        public Period Period
        {
            get { return _period; }
        }


        /// <summary>
        /// Used for databinding against speciesfact quality
        /// </summary>
        public Int32 QualityIndex
        {
            get 
            {
                SpeciesFactQualityList qualities = SpeciesFactManager.GetSpeciesFactQualities();
                Int32 i = 0;
                foreach (SpeciesFactQuality quality in qualities)
                {
                    if (Quality.Id == quality.Id)
                        return i;
                    else
                        i++;

                }

                return -1; 
            }
            set
            {
                SpeciesFactQualityList qualities = SpeciesFactManager.GetSpeciesFactQualities();
                Quality = qualities[value];
            }
        }


        /// <summary>
        /// Used for databinding against speciesfact quality
        /// </summary>
        public int QualityId
        {
            get
            {
                if (Quality != null)
                {
                    return Quality.Id;
                }
                else
                {
                    return -1;
                }
            }
            set
            {
                Quality = SpeciesFactManager.GetSpeciesFactQuality(value);

            }
        }

        /// <summary>
        /// Handle the species fact quality of this species fact object.
        /// </summary>
        public SpeciesFactQuality Quality
        {
            get { return _quality; }
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
        public Reference Reference
        {
            get { return _reference; }
            set
            {
                CheckUpdate();
                _reference = value;
                FireUpdateEvent();
            }
        }

        /// <summary>
        /// Handle the Reference of this species fact object.
        /// </summary>
        public Int32 ReferenceId
        {
            get
            {
                if (_reference == null)
                    return -1;
                else
                    return _reference.Id;
            }
            set
            {
                CheckUpdate();

                //Find reference based on name
                Reference = ReferenceManager.GetReference(value);
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
        public Taxon Taxon
        {
            get { return _taxon; }
        }

        /// <summary>
        /// Get the update date of this species fact object.
        /// </summary>
        public DateTime UpdateDate
        {
            get { return _updateDate; }
        }

        /// <summary>
        /// Get the update user full name of this species fact object.
        /// </summary>
        public String UpdateUserFullName
        {
            get { return _updateUserFullName; }
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
        internal void Reset()
        {
            UpdateId(-1);
            _hasId = false;
            _quality = SpeciesFactManager.GetDefaultSpeciesFactQuality();
            _oldQuality = _quality;
            _reference = null;
            _oldReference = null;
            _updateUserFullName = null;
            _updateDate = DateTime.MinValue;
            _hasUpdateDate = false;
            foreach (SpeciesFactField field in _fields)
            {
                field.Reset();
            }
        }

        /// <summary>
        /// Update species fact with new data from database.
        /// </summary>
        /// <param name="id">Id of the species fact</param>
        /// <param name="referenceId">Reference id of the species fact</param>
        /// <param name="updateDate">Update date of the species fact</param>
        /// <param name="updateUserFullName">Full Name of the pdate user of the species fact</param>
        /// <param name="hasFieldValue1">Indicates if field 1 has a value.</param>
        /// <param name="fieldValue1">Field value of field 1 for the species fact</param>
        /// <param name="hasFieldValue2">Indicates if field 2 has a value.</param>
        /// <param name="fieldValue2">Field value of field 2 for the species fact</param>
        /// <param name="hasFieldValue3">Indicates if field 3 has a value.</param>
        /// <param name="fieldValue3">Field value of field 3 for the species fact</param>
        /// <param name="hasFieldValue4">Indicates if field 4 has a value.</param>
        /// <param name="fieldValue4">Field value of field 4 for the species fact</param>
        /// <param name="hasFieldValue5">Indicates if field 5 has a value.</param>
        /// <param name="fieldValue5">Field value of field 5 for the species fact</param>
        /// <param name="qualityId">Quality Id of the species fact</param>
        internal void Update(Int32 id,
                             Int32 referenceId,
                             DateTime updateDate,
                             String updateUserFullName,
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
                             Int32 qualityId)
        {
            UpdateId(id);
            _hasId = true;
            _quality = SpeciesFactManager.GetSpeciesFactQuality(qualityId);
            _oldQuality = _quality;
            _reference = ReferenceManager.GetReference(referenceId);
            _oldReference = _reference;
            _updateUserFullName = updateUserFullName;
            _updateDate = updateDate;
            _hasUpdateDate = true;

            foreach (SpeciesFactField field in _fields)
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
