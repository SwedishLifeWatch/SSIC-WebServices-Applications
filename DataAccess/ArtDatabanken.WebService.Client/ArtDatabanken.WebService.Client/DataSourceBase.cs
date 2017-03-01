//#define SHOW_CLIENT_IP_ADDRESSES
using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.Client
{
    /// <summary>
    /// Base class for all web service data sources.
    /// </summary>
    public abstract class DataSourceBase
    {
#if SHOW_CLIENT_IP_ADDRESSES
        private static Boolean _isIpAddressesSet = false;
        private static String _ipAddresses = null;
#endif // SHOW_CLIENT_IP_ADDRESSES

        /// <summary>
        /// Check if a transaction should be created.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="service">Web service with transaction handling.</param>
        public void CheckTransaction(IUserContext userContext,
                                     ITransactionService service)
        {
            IDataSourceInformation dataSourceInformation;
            Int32 timeout;
            ITransaction transaction;
            TimeSpan duration;

            if (userContext.Transaction.IsNotNull())
            {
                userContext.Transaction.CheckTransactionTimeout();
                dataSourceInformation = service.GetDataSourceInformation();
                if (!(userContext.Transactions.IsDataSourceInTransaction(dataSourceInformation)))
                {
                    // Add data source to transaction.
                    duration = DateTime.Now - userContext.Transaction.Started;
                    timeout = (userContext.Transaction.Timeout - duration.Seconds) +
                              Settings.Default.TransactionTimeoutMargin;
                    transaction = new WebTransaction(userContext, service, timeout);
                    userContext.Transactions.Add(transaction);
                }
                // ELSE, data source already participates in transaction.
            }
            // ELSE, no active transaction.
        }

        /// <summary>
        /// Get WebAuthority from Authority.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="authority">Authority.</param>
        /// <returns>WebAuthority.</returns>
        public WebAuthority GetAuthority(IUserContext userContext,
                                         IAuthority authority)
        {
            WebAuthority webAuthority;
            webAuthority = new WebAuthority();

            webAuthority.ActionGUIDs = authority.ActionGUIDs;
            webAuthority.FactorGUIDs = authority.FactorGUIDs;
            webAuthority.LocalityGUIDs = authority.LocalityGUIDs;
            webAuthority.ProjectGUIDs = authority.ProjectGUIDs;
            webAuthority.RegionGUIDs = authority.RegionGUIDs;
            webAuthority.TaxonGUIDs = authority.TaxonGUIDs;

            webAuthority.ReadPermission = authority.ReadPermission;
            webAuthority.CreatePermission = authority.CreatePermission;
            webAuthority.UpdatePermission = authority.UpdatePermission;
            webAuthority.DeletePermission = authority.DeletePermission;

            webAuthority.MaxProtectionLevel = authority.MaxProtectionLevel;
            webAuthority.ShowNonPublicData = authority.ReadNonPublicPermission;

            webAuthority.RoleId = authority.RoleId;

            if (authority.AdministrationRoleId.HasValue)
            {
                webAuthority.AdministrationRoleId = authority.AdministrationRoleId.Value;
            }
            webAuthority.Identifier = authority.Identifier;
            webAuthority.CreatedBy = authority.UpdateInformation.CreatedBy;
            webAuthority.CreatedDate = authority.UpdateInformation.CreatedDate;
            webAuthority.Description = authority.Description;
            webAuthority.GUID = authority.GUID;
            webAuthority.Id = authority.Id;
            webAuthority.IsAdministrationRoleIdSpecified = authority.AdministrationRoleId.HasValue;
            webAuthority.ModifiedBy = authority.UpdateInformation.ModifiedBy;
            webAuthority.ModifiedDate = authority.UpdateInformation.ModifiedDate;
            webAuthority.Name = authority.Name;
            webAuthority.Obligation = authority.Obligation;
            webAuthority.ValidFromDate = authority.ValidFromDate;
            webAuthority.ValidToDate = authority.ValidToDate;
            webAuthority.AuthorityType = authority.AuthorityType;
            // If authority is of type data type the id and identifier must be set,
            // otherwise the autority is associated with an application.
            // if(authority.AuthorityType.Equals(AuthorityType.DataType))
            if (authority.AuthorityDataType.IsNotNull())
            {
                WebAuthorityDataType webAuthorityDataType = new WebAuthorityDataType();
                webAuthorityDataType.Id = authority.AuthorityDataType.Id;
                webAuthorityDataType.Identifier = authority.AuthorityDataType.Identifier;
                webAuthority.AuthorityDataType = webAuthorityDataType;
            }
            else
            {
                webAuthority.ApplicationId = authority.ApplicationId;

            }
            return webAuthority;
        }

        /// <summary>
        /// Get list of WebAuthority from authority list.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="authorityList">List of authorities.</param>
        /// <returns>List of WebAuthority.</returns>
        protected List<WebAuthority> GetAuthorityList(IUserContext userContext,
                                                      AuthorityList authorityList)
        {
            List<WebAuthority> webAuthorityList;

            webAuthorityList = null;
            if (authorityList.IsNotEmpty())
            {
                webAuthorityList = new List<WebAuthority>();
                foreach (IAuthority authority in authorityList)
                {
                    webAuthorityList.Add(GetAuthority(userContext,
                                                      authority));
                }
            }
            return webAuthorityList;
        }

        /// <summary>
        /// Convert a IBoundingBox instance to a WebBoundingBox instance.
        /// </summary>
        /// <param name="boundingBox">An IBoundingBox instance.</param>
        /// <returns>A WebBoundingBox instance.</returns>
        protected WebBoundingBox GetBoundingBox(IBoundingBox boundingBox)
        {
            if (boundingBox == null)
            {
                return null;
            }

            return new WebBoundingBox()
            {
                Max = new WebPoint()
                {
                    M = boundingBox.Max.M.GetValueOrDefault(),
                    X = boundingBox.Max.X,
                    Y = boundingBox.Max.Y,
                    Z = boundingBox.Max.Z.GetValueOrDefault()
                },
                Min = new WebPoint()
                {
                    M = boundingBox.Min.M.GetValueOrDefault(),
                    X = boundingBox.Min.X,
                    Y = boundingBox.Min.Y,
                    Z = boundingBox.Min.Z.GetValueOrDefault()
                }
            };
        }

        /// <summary>
        /// Get client information object.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>Client information object.</returns>
        public WebClientInformation GetClientInformation(IUserContext userContext)
        {
#if SHOW_CLIENT_IP_ADDRESSES
            IPHostEntry ipEntry;
            String hostName;
#endif // SHOW_CLIENT_IP_ADDRESSES
            WebClientInformation webClientInformation;

            webClientInformation = new WebClientInformation();
            webClientInformation.Locale = GetLocale(userContext.Locale);
            webClientInformation.Token = GetToken(userContext);
            if (userContext.CurrentRole.IsNotNull())
            {
                webClientInformation.Role = GetRole(userContext, userContext.CurrentRole);
            }

#if SHOW_CLIENT_IP_ADDRESSES
            if (!_isIpAddressesSet)
            {
                // Getting Ip addreses of local machine.
                // First get the host name of local machine.
                hostName = Dns.GetHostName();

                // Then using host name, get the IP address list.
                ipEntry = Dns.GetHostEntry(hostName);

                if (ipEntry.AddressList.IsNotEmpty())
                {
                    // Create string with IP addresses.
                    _ipAddresses = null;
                    foreach (IPAddress ipAddress in ipEntry.AddressList)
                    {
                        if (_ipAddresses.IsNotNull())
                        {
                            _ipAddresses += "    ";
                        }
                        _ipAddresses += ipAddress.ToString();
                    }
                }
                _isIpAddressesSet = true;
            }
            if (_ipAddresses.IsNotEmpty())
            {
                webClientInformation.DataFields = new List<WebDataField>();
                webClientInformation.DataFields.SetString("ClientIpAddresses", _ipAddresses);
            }
#endif // SHOW_CLIENT_IP_ADDRESSES

            return webClientInformation;
        }

        /// <summary>
        /// Convert a ICoordinateSystem instance
        /// to a WebCoordinateSystem instance.
        /// </summary>
        /// <param name="coordinateSystem">An ICoordinateSystem instance.</param>
        /// <returns>A WebCoordinateSystem instance.</returns>
        protected WebCoordinateSystem GetCoordinateSystem(ICoordinateSystem coordinateSystem)
        {
            WebCoordinateSystem webCoordinateSystem;

            webCoordinateSystem = null;
            if (coordinateSystem.IsNotNull())
            {
                webCoordinateSystem = new WebCoordinateSystem();
                webCoordinateSystem.Id = coordinateSystem.Id;
                webCoordinateSystem.WKT = coordinateSystem.WKT;
            }

            return webCoordinateSystem;
        }

        /// <summary>
        /// Convert a WebCoordinateSystem instance
        /// to a ICoordinateSystem instance.
        /// </summary>
        /// <param name="webCoordinateSystem">A WebCoordinateSystem instance.</param>
        /// <returns>An ICoordinateSystem instance.</returns>
        protected ICoordinateSystem GetCoordinateSystem(WebCoordinateSystem webCoordinateSystem)
        {
            ICoordinateSystem coordinateSystem;

            coordinateSystem = null;
            if (webCoordinateSystem.IsNotNull())
            {
                coordinateSystem = new CoordinateSystem();
                coordinateSystem.Id = webCoordinateSystem.Id;
                coordinateSystem.WKT = webCoordinateSystem.WKT;
            }

            return coordinateSystem;
        }

        /// <summary>
        /// Get data context.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>Data context object.</returns>
        protected IDataContext GetDataContext(IUserContext userContext)
        {
            return new DataContext(GetDataSourceInformation(), userContext.Locale);
        }

        /// <summary>
        /// Get information about data source.
        /// </summary>
        /// <returns>Information about data source.</returns>
        public abstract IDataSourceInformation GetDataSourceInformation();

        /// <summary>
        /// Convert a IDateTimeSearchCriteria instance
        /// to a WebDateTimeSearchCriteria instance.
        /// </summary>
        /// <param name="dateTimeSearchCriteria">An IDateTimeSearchCriteria instance.</param>
        /// <returns>A WebDateTimeSearchCriteria instance.</returns>
        protected WebDateTimeSearchCriteria GetDateTimeSearchCriteria(IDateTimeSearchCriteria dateTimeSearchCriteria)
        {
            WebDateTimeSearchCriteria webDateTimeSearchCriteria;
            WebDateTimeInterval webDateTimeInterval;

            webDateTimeSearchCriteria = new WebDateTimeSearchCriteria();
#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
            if (dateTimeSearchCriteria.Accuracy.HasValue)
            {
                webDateTimeSearchCriteria.Accuracy = new WebTimeSpan();
                webDateTimeSearchCriteria.Accuracy.Days = dateTimeSearchCriteria.Accuracy.Value.Days;
                webDateTimeSearchCriteria.Accuracy.IsDaysSpecified = true;
            }
#endif
            webDateTimeSearchCriteria.Begin = dateTimeSearchCriteria.Begin;
            webDateTimeSearchCriteria.End = dateTimeSearchCriteria.End;
#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
            webDateTimeSearchCriteria.Operator = dateTimeSearchCriteria.Operator;
#endif

            if (dateTimeSearchCriteria.PartOfYear.IsNotNull())
            {
                webDateTimeSearchCriteria.PartOfYear = new List<WebDateTimeInterval>();
                foreach (IDateTimeInterval dateTimeInterval in dateTimeSearchCriteria.PartOfYear)
                {
                    webDateTimeInterval = new WebDateTimeInterval();
                    webDateTimeInterval.Begin = dateTimeInterval.Begin;
                    webDateTimeInterval.End = dateTimeInterval.End;
#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
                    webDateTimeInterval.IsDayOfYearSpecified = dateTimeInterval.IsDayOfYearSpecified;
#endif
                    webDateTimeSearchCriteria.PartOfYear.Add(webDateTimeInterval);
                }
            }


            return webDateTimeSearchCriteria;
        }

        /// <summary>
        /// Convert an IFactorField instance
        /// to a WebFactorField instance.
        /// </summary>
        /// <param name="factorField">An IFactorField instance.</param>
        /// <returns>A WebFactorField instance.</returns>
        private WebFactorField GetFactorField(IFactorField factorField)
        {
            WebFactorField webFactorField;

            webFactorField = new WebFactorField();
            webFactorField.DatabaseFieldName = factorField.DatabaseFieldName;
            webFactorField.FactorDataTypeId = factorField.FactorDataType.Id;
            if (factorField.Enum.IsNull())
            {
                webFactorField.EnumId = -1;
            }
            else
            {
                webFactorField.EnumId = factorField.Enum.Id;
            }

            webFactorField.Id = factorField.Id;
            webFactorField.Information = factorField.Information;
            webFactorField.IsEnumField = factorField.Enum.IsNotNull();
            webFactorField.IsMain = factorField.IsMain;
            webFactorField.IsSubstantial = factorField.IsSubstantial;
            webFactorField.Label = factorField.Label;
            webFactorField.Size = factorField.Size;
            webFactorField.TypeId = factorField.Type.Id;
            webFactorField.Unit = factorField.Unit;

            return webFactorField;
        }

        /// <summary>
        /// Convert a ILinearRing instance to a WebLinearRing instance.
        /// </summary>
        /// <param name="linearRing">An ILinearRing instance.</param>
        /// <returns>A WebLinearRing instance.</returns>
        protected WebLinearRing GetLinearRing(ILinearRing linearRing)
        {
            WebLinearRing webLinearRing;

            webLinearRing = null;
            if (linearRing.IsNotNull())
            {
                webLinearRing = new WebLinearRing();
                if (linearRing.Points.IsNotEmpty())
                {
                    webLinearRing.Points = new List<WebPoint>();

                    foreach (Point point in linearRing.Points)
                    {
                        webLinearRing.Points.Add(GetPoint(point));
                    }
                }
            }
            return webLinearRing;
        }

        /// <summary>
        /// Get locale from web locale.
        /// </summary>
        /// <param name="webLocale">Web locale.</param>
        /// <returns>Locale.</returns>
        protected ILocale GetLocale(WebLocale webLocale)
        {
            return new Locale(webLocale.Id,
                              webLocale.ISOCode,
                              webLocale.Name,
                              webLocale.NativeName,
                              new DataContext(GetDataSourceInformation(), null));
        }

        /// <summary>
        /// Get web locale from locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Web locale.</returns>
        protected WebLocale GetLocale(ILocale locale)
        {
            WebLocale webLocale;

            webLocale = new WebLocale();
            webLocale.Id = locale.Id;
            webLocale.ISOCode = locale.ISOCode;
            webLocale.Name = locale.Name;
            webLocale.NativeName = locale.NativeName;
            return webLocale;
        }

        /// <summary>
        /// Convert a IPoint instance to a WebPoint instance.
        /// </summary>
        /// <param name="point">An IPoint instance.</param>
        /// <returns>A WebPoint instance.</returns>
        protected WebPoint GetPoint(IPoint point)
        {
            WebPoint webPoint;

            webPoint = null;
            if (point.IsNotNull())
            {
                webPoint = new WebPoint();
                webPoint.IsMSpecified = point.M.HasValue;
                webPoint.IsZSpecified = point.Z.HasValue;
                if (point.M.HasValue)
                {
                    webPoint.M = point.M.Value;
                }
                webPoint.X = point.X;
                webPoint.Y = point.Y;
                if (point.Z.HasValue)
                {
                    webPoint.Z = point.Z.Value;
                }
            }

            return webPoint;
        }

        /// <summary>
        /// Convert a IPolygon instance to a WebPolygon instance.
        /// </summary>
        /// <param name="polygon">An IPolygon instance.</param>
        /// <returns>A WebPolygon instance.</returns>
        protected WebPolygon GetPolygon(IPolygon polygon)
        {
            if (polygon == null)
            {
                return null;
            }

            var webPolygon = new WebPolygon()
            {
                LinearRings = new List<WebLinearRing>()
            };

            foreach (LinearRing linearRing in polygon.LinearRings)
            {
                webPolygon.LinearRings.Add(GetLinearRing(linearRing));
            }

            return webPolygon;
        }

        /// <summary>
        /// Convert a IPolygon list to a WebPolygon list.
        /// </summary>
        /// <param name="polygons">A IPolygon list instance.</param>
        /// <returns>A WebPolygon list.</returns>
        protected List<WebPolygon> GetPolygons(List<IPolygon> polygons)
        {
            List<WebPolygon> webPolygons;

            webPolygons = null;
            if (polygons.IsNotEmpty())
            {
                webPolygons = new List<WebPolygon>();
                foreach (Polygon polygon in polygons)
                {
                    webPolygons.Add(GetPolygon(polygon));
                }
            }
            return webPolygons;
        }


        /// <summary>
        /// Get region from WebRegion.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webRegion">WebRegion.</param>
        /// <returns>Region.</returns>
        protected IRegion GetRegion(IUserContext userContext, WebRegion webRegion)
        {
            return new Region(webRegion.Id,
                              webRegion.CategoryId,
                              webRegion.GUID,
                              webRegion.Name,
                              webRegion.NativeId,
                              webRegion.ShortName,
                              webRegion.SortOrder,
                              GetDataContext(userContext));
        }

        /// <summary>
        /// Get regions from list of WebRegions.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webRegions">List of WebRegions.</param>
        /// <returns>RegionList.</returns>
        protected RegionList GetRegions(IUserContext userContext,
                                        List<WebRegion> webRegions)
        {
            RegionList regionList = new RegionList();
            foreach (WebRegion webRegion in webRegions)
            {
                regionList.Add(GetRegion(userContext, webRegion));
            }
            return regionList;
        }


        /// <summary>
        /// Get WebRole from Role.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="role">Role.</param>
        /// <returns>WebRole.</returns>
        public WebRole GetRole(IUserContext userContext, IRole role)
        {
            WebRole webRole;
            webRole = new WebRole();

            if (role.AdministrationRoleId.HasValue)
            {
                webRole.AdministrationRoleId = role.AdministrationRoleId.Value;
            }
            webRole.Authorities = GetAuthorityList(userContext, role.Authorities);
            webRole.CreatedBy = role.UpdateInformation.CreatedBy;
            webRole.CreatedDate = role.UpdateInformation.CreatedDate;
            webRole.Description = role.Description;
            webRole.GUID = role.GUID;
            webRole.Id = role.Id;
            webRole.Identifier = role.Identifier;
            webRole.IsActivationRequired = role.IsActivationRequired;
            webRole.IsAdministrationRoleIdSpecified = role.AdministrationRoleId.HasValue;
            webRole.IsOrganizationIdSpecified = role.OrganizationId.HasValue;
            webRole.IsUserAdministrationRoleIdSpecified = role.UserAdministrationRoleId.HasValue;
            webRole.ModifiedBy = role.UpdateInformation.ModifiedBy;
            webRole.ModifiedDate = role.UpdateInformation.ModifiedDate;
            webRole.MessageTypeId = role.MessageType.Id;
            webRole.Name = role.Name;
            if (role.OrganizationId.HasValue)
            {
                webRole.OrganizationId = role.OrganizationId.Value;
            }
            webRole.ShortName = role.ShortName;
            if (role.UserAdministrationRoleId.HasValue)
            {
                webRole.UserAdministrationRoleId = role.UserAdministrationRoleId.Value;
            }
            webRole.ValidFromDate = role.ValidFromDate;
            webRole.ValidToDate = role.ValidToDate;

            return webRole;
        }

        /// <summary>
        /// Get web string search criteria from search criteria.
        /// </summary>
        /// <param name="stringSearchCriteria">Search criteria.</param>
        /// <returns>WebRole.</returns>
        protected WebStringSearchCriteria GetStringSearchCriteria(IStringSearchCriteria stringSearchCriteria)
        {
            WebStringSearchCriteria webSearchCriteria;

            if (stringSearchCriteria.IsNull())
            {
                webSearchCriteria = null;
            }
            else
            {
                webSearchCriteria = new WebStringSearchCriteria();
                webSearchCriteria.CompareOperators = stringSearchCriteria.CompareOperators;
                webSearchCriteria.SearchString = stringSearchCriteria.SearchString;
            }

            return webSearchCriteria;
        }

        /// <summary>
        /// Convert an ISpeciesFactFieldSearchCriteria instance
        /// to a WebSpeciesFactFieldSearchCriteria instance.
        /// </summary>
        /// <param name="speciesFactFieldSearchCriteria">An ISpeciesFactFieldSearchCriteria instance.</param>
        /// <returns>A WebSpeciesFactFieldSearchCriteria instance.</returns>
        private WebSpeciesFactFieldSearchCriteria GetSpeciesFactFieldSearchCriteria(ISpeciesFactFieldSearchCriteria speciesFactFieldSearchCriteria)
        {
            WebSpeciesFactFieldSearchCriteria webSpeciesFactFieldSearchCriteria;

            webSpeciesFactFieldSearchCriteria = new WebSpeciesFactFieldSearchCriteria();
            webSpeciesFactFieldSearchCriteria.FactorField = GetFactorField(speciesFactFieldSearchCriteria.FactorField);
            webSpeciesFactFieldSearchCriteria.IsEnumAsString = speciesFactFieldSearchCriteria.IsEnumAsString;
            webSpeciesFactFieldSearchCriteria.Operator = speciesFactFieldSearchCriteria.Operator;
            webSpeciesFactFieldSearchCriteria.Values = speciesFactFieldSearchCriteria.Values;

            return webSpeciesFactFieldSearchCriteria;
        }

        /// <summary>
        /// Convert a list of ISpeciesFactFieldSearchCriteria instances
        /// to a list of WebSpeciesFactFieldSearchCriteria instances.
        /// </summary>
        /// <param name="speciesFactFieldSearchCriteriaList">List of ISpeciesFactFieldSearchCriteria instances.</param>
        /// <returns>A list of WebSpeciesFactFieldSearchCriteria instances.</returns>
        private List<WebSpeciesFactFieldSearchCriteria> GetSpeciesFactFieldSearchCriteria(SpeciesFactFieldSearchCriteriaList speciesFactFieldSearchCriteriaList)
        {
            List<WebSpeciesFactFieldSearchCriteria> webSpeciesFactFieldSearchCriteriaList;

            webSpeciesFactFieldSearchCriteriaList = null;
            if (speciesFactFieldSearchCriteriaList.IsNotEmpty())
            {
                webSpeciesFactFieldSearchCriteriaList = new List<WebSpeciesFactFieldSearchCriteria>();
                foreach (ISpeciesFactFieldSearchCriteria speciesFactFieldSearchCriteria in speciesFactFieldSearchCriteriaList)
                {
                    webSpeciesFactFieldSearchCriteriaList.Add(GetSpeciesFactFieldSearchCriteria(speciesFactFieldSearchCriteria));
                }
            }

            return webSpeciesFactFieldSearchCriteriaList;
        }

        /// <summary>
        /// Convert a ISpeciesFactSearchCriteria instance
        /// to a WebSpeciesFactSearchCriteria instance.
        /// </summary>
        /// <param name="searchCriteria">An ISpeciesFactSearchCriteria object.</param>
        /// <returns>A WebSpeciesFactSearchCriteria object.</returns>
        protected WebSpeciesFactSearchCriteria GetSpeciesFactSearchCriteria(ISpeciesFactSearchCriteria searchCriteria)
        {
            WebSpeciesFactSearchCriteria webSearchCriteria;

            webSearchCriteria = new WebSpeciesFactSearchCriteria();
            if (searchCriteria.IsNotNull())
            {
                if (searchCriteria.FactorDataTypes.IsNotEmpty())
                {
                    webSearchCriteria.FactorDataTypeIds = searchCriteria.FactorDataTypes.GetIds();
                }

                if (searchCriteria.Factors.IsNotEmpty())
                {
                    webSearchCriteria.FactorIds = searchCriteria.Factors.GetIds();
                }

                webSearchCriteria.FieldLogicalOperator = searchCriteria.FieldLogicalOperator;
                webSearchCriteria.FieldSearchCriteria = GetSpeciesFactFieldSearchCriteria(searchCriteria.FieldSearchCriteria);

                if (searchCriteria.Hosts.IsNotEmpty())
                {
                    webSearchCriteria.HostIds = searchCriteria.Hosts.GetIds();
                }

                webSearchCriteria.IncludeNotValidHosts = searchCriteria.IncludeNotValidHosts;
                webSearchCriteria.IncludeNotValidTaxa = searchCriteria.IncludeNotValidTaxa;

                if (searchCriteria.IndividualCategories.IsNotEmpty())
                {
                    webSearchCriteria.IndividualCategoryIds = searchCriteria.IndividualCategories.GetIds();
                }

                if (searchCriteria.Periods.IsNotEmpty())
                {
                    webSearchCriteria.PeriodIds = searchCriteria.Periods.GetIds();
                }

                if (searchCriteria.Taxa.IsNotEmpty())
                {
                    webSearchCriteria.TaxonIds = searchCriteria.Taxa.GetIds();
                }
            }

            return webSearchCriteria;
        }

        /// <summary>
        /// Convert a SpeciesObservationFieldSearchCriteria list
        /// to a WebSpeciesObservationFieldSearchCriteria list.
        /// </summary>
        /// <param name="speciesObservationFieldSearchCriterias">An SpeciesObservationSearchCriteriaList instance.</param>
        /// <returns>A WebSpeciesObservationFieldSearchCriteria list.</returns>
        private List<WebSpeciesObservationFieldSearchCriteria> GetSpeciesObservationFieldSearchCriterias(SpeciesObservationFieldSearchCriteriaList speciesObservationFieldSearchCriterias)
        {
            List<WebSpeciesObservationFieldSearchCriteria> webSpeciesObservationFieldSearchCriterias;
            WebSpeciesObservationFieldSearchCriteria webSpeciesObservationFieldSearchCriteria;

            webSpeciesObservationFieldSearchCriterias = new List<WebSpeciesObservationFieldSearchCriteria>();

            foreach (ISpeciesObservationFieldSearchCriteria speciesObservationFieldSearchCriteria in speciesObservationFieldSearchCriterias)
            {
                webSpeciesObservationFieldSearchCriteria = new WebSpeciesObservationFieldSearchCriteria();
                webSpeciesObservationFieldSearchCriteria.Class = new WebSpeciesObservationClass { Id = speciesObservationFieldSearchCriteria.Class.Id, Identifier = speciesObservationFieldSearchCriteria.Class.Identifier };
                webSpeciesObservationFieldSearchCriteria.Property = new WebSpeciesObservationProperty { Id = speciesObservationFieldSearchCriteria.Property.Id, Identifier = speciesObservationFieldSearchCriteria.Property.Identifier };
                webSpeciesObservationFieldSearchCriteria.Type = (WebDataType)speciesObservationFieldSearchCriteria.Type;
                webSpeciesObservationFieldSearchCriteria.Operator = speciesObservationFieldSearchCriteria.Operator;
                webSpeciesObservationFieldSearchCriteria.Value = speciesObservationFieldSearchCriteria.Value;

                webSpeciesObservationFieldSearchCriterias.Add(webSpeciesObservationFieldSearchCriteria);
            }

            return webSpeciesObservationFieldSearchCriterias;
        }

        /// <summary>
        /// Convert a ISpeciesObservationSearchCriteria instance
        /// to a WebSpeciesObservationSearchCriteria instance.
        /// </summary>
        /// <param name="speciesObservationSearchCriteria">An ISpeciesObservationSearchCriteria instance.</param>
        /// <returns>A WebSpeciesObservationSearchCriteria instance.</returns>
        protected WebSpeciesObservationSearchCriteria GetSpeciesObservationSearchCriteria(ISpeciesObservationSearchCriteria speciesObservationSearchCriteria)
        {
            WebSpeciesObservationSearchCriteria webSpeciesObservationSearchCriteria;

            webSpeciesObservationSearchCriteria = null;
            if (speciesObservationSearchCriteria.IsNotNull())
            {
                webSpeciesObservationSearchCriteria = new WebSpeciesObservationSearchCriteria();
                if (speciesObservationSearchCriteria.Accuracy.HasValue)
                {
                    webSpeciesObservationSearchCriteria.Accuracy = speciesObservationSearchCriteria.Accuracy.Value;
                }

#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
                if (speciesObservationSearchCriteria.BirdNestActivityLimit.IsNotNull())
                {
                    webSpeciesObservationSearchCriteria.BirdNestActivityLimit = speciesObservationSearchCriteria.BirdNestActivityLimit.Id;
                }
#endif

                if (speciesObservationSearchCriteria.BoundingBox.IsNotNull())
                {
                    webSpeciesObservationSearchCriteria.BoundingBox = GetBoundingBox(speciesObservationSearchCriteria.BoundingBox);
                }

                if (speciesObservationSearchCriteria.ChangeDateTime.IsNotNull())
                {
                    webSpeciesObservationSearchCriteria.ChangeDateTime = GetDateTimeSearchCriteria(speciesObservationSearchCriteria.ChangeDateTime);
                }

#if SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
                webSpeciesObservationSearchCriteria.DataSourceGuids = speciesObservationSearchCriteria.DataSourceGuids;
#else
                webSpeciesObservationSearchCriteria.DataProviderGuids = speciesObservationSearchCriteria.DataSourceGuids;
#endif

                if (webSpeciesObservationSearchCriteria.DataFields.IsNull())
                {
                    webSpeciesObservationSearchCriteria.DataFields = new List<WebDataField>();
                }

                webSpeciesObservationSearchCriteria.DataFields.SetString("FieldLogicalOperator", speciesObservationSearchCriteria.FieldLogicalOperator.ToString());

                if (speciesObservationSearchCriteria.FieldSearchCriteria.IsNotNull())
                {
                    webSpeciesObservationSearchCriteria.FieldSearchCriteria = GetSpeciesObservationFieldSearchCriterias(speciesObservationSearchCriteria.FieldSearchCriteria);
                }

                webSpeciesObservationSearchCriteria.IncludeNeverFoundObservations = speciesObservationSearchCriteria.IncludeNeverFoundObservations;
                webSpeciesObservationSearchCriteria.IncludeNotRediscoveredObservations = speciesObservationSearchCriteria.IncludeNotRediscoveredObservations;
                webSpeciesObservationSearchCriteria.IncludePositiveObservations = speciesObservationSearchCriteria.IncludePositiveObservations;
                webSpeciesObservationSearchCriteria.IncludeRedListCategories = speciesObservationSearchCriteria.IncludeRedListCategories;
                webSpeciesObservationSearchCriteria.IncludeRedlistedTaxa = speciesObservationSearchCriteria.IncludeRedlistedTaxa;
                webSpeciesObservationSearchCriteria.IsAccuracyConsidered = speciesObservationSearchCriteria.IsAccuracyConsidered;
                webSpeciesObservationSearchCriteria.IsAccuracySpecified = speciesObservationSearchCriteria.Accuracy.HasValue;
#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
                webSpeciesObservationSearchCriteria.IsBirdNestActivityLimitSpecified = speciesObservationSearchCriteria.BirdNestActivityLimit.IsNotNull();
#endif
                webSpeciesObservationSearchCriteria.IsDisturbanceSensitivityConsidered = speciesObservationSearchCriteria.IsDisturbanceSensitivityConsidered;
                webSpeciesObservationSearchCriteria.IsMaxProtectionLevelSpecified = speciesObservationSearchCriteria.MaxProtectionLevel.HasValue;
                webSpeciesObservationSearchCriteria.IsMinProtectionLevelSpecified = speciesObservationSearchCriteria.MinProtectionLevel.HasValue;
#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
                webSpeciesObservationSearchCriteria.IsIsNaturalOccurrenceSpecified = speciesObservationSearchCriteria.IsNaturalOccurrence.HasValue;
                if (speciesObservationSearchCriteria.IsNaturalOccurrence.HasValue)
                {
                    webSpeciesObservationSearchCriteria.IsNaturalOccurrence = speciesObservationSearchCriteria.IsNaturalOccurrence.Value;
                }
#endif
                if (speciesObservationSearchCriteria.LocalityNameSearchString.IsNotNull())
                {
                    webSpeciesObservationSearchCriteria.LocalityNameSearchString = GetStringSearchCriteria(speciesObservationSearchCriteria.LocalityNameSearchString);
                }

                if (speciesObservationSearchCriteria.MaxProtectionLevel.HasValue)
                {
                    webSpeciesObservationSearchCriteria.MaxProtectionLevel = speciesObservationSearchCriteria.MaxProtectionLevel.Value;
                }

                if (speciesObservationSearchCriteria.MinProtectionLevel.HasValue)
                {
                    webSpeciesObservationSearchCriteria.MinProtectionLevel = speciesObservationSearchCriteria.MinProtectionLevel.Value;
                }

                if (speciesObservationSearchCriteria.ObservationDateTime.IsNotNull())
                {
                    webSpeciesObservationSearchCriteria.ObservationDateTime = GetDateTimeSearchCriteria(speciesObservationSearchCriteria.ObservationDateTime);
                }

                webSpeciesObservationSearchCriteria.ObserverIds = speciesObservationSearchCriteria.ObserverIds;
                if (speciesObservationSearchCriteria.ObserverSearchString.IsNotNull())
                {
                    webSpeciesObservationSearchCriteria.ObserverSearchString = GetStringSearchCriteria(speciesObservationSearchCriteria.ObserverSearchString);
                }

                if (speciesObservationSearchCriteria.Polygons.IsNotNull())
                {
                    webSpeciesObservationSearchCriteria.Polygons = GetPolygons(speciesObservationSearchCriteria.Polygons);
                }

                webSpeciesObservationSearchCriteria.ProjectGuids = speciesObservationSearchCriteria.ProjectGuids;
                webSpeciesObservationSearchCriteria.RegionGuids = speciesObservationSearchCriteria.RegionGuids;
                webSpeciesObservationSearchCriteria.RegionLogicalOperator = speciesObservationSearchCriteria.RegionLogicalOperator;
#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
                if (speciesObservationSearchCriteria.ReportedDateTime.IsNotNull())
                {
                    webSpeciesObservationSearchCriteria.ReportedDateTime = GetDateTimeSearchCriteria(speciesObservationSearchCriteria.ReportedDateTime);
                }
#endif

                webSpeciesObservationSearchCriteria.SpeciesActivityIds = speciesObservationSearchCriteria.SpeciesActivityIds;
                webSpeciesObservationSearchCriteria.TaxonIds = speciesObservationSearchCriteria.TaxonIds;
                webSpeciesObservationSearchCriteria.ValidationStatusIds = speciesObservationSearchCriteria.ValidationStatusIds;

                if (speciesObservationSearchCriteria.CountyProvinceRegionSearchType ==
                    CountyProvinceRegionSearchType.ByName)
                {
                    webSpeciesObservationSearchCriteria.DataFields.Add(new WebDataField()
                    {
                        Name = typeof(CountyProvinceRegionSearchType).ToString(),
                        Type = WebDataType.String,
                        Value = speciesObservationSearchCriteria.CountyProvinceRegionSearchType.ToString()
                    });
                }
            }

            return webSpeciesObservationSearchCriteria;
        }

        /// <summary>
        /// Convert a ISpeciesObservationFieldSortOrder instance
        /// to a WebSpeciesObservationFieldSortOrder instance.
        /// </summary>
        /// <param name="speciesObservationFieldSortOrders">An ISpeciesObservationFieldSortOrder instance.</param>
        /// <returns>A WebSpeciesObservationFieldSortOrder instance.</returns>
        protected List<WebSpeciesObservationFieldSortOrder> GetSpeciesObservationSortOrder(SpeciesObservationFieldSortOrderList speciesObservationFieldSortOrders)
        {
            if (speciesObservationFieldSortOrders.IsNull()) return null;

            List<WebSpeciesObservationFieldSortOrder> webSpeciesObservationSortOrders = new List<WebSpeciesObservationFieldSortOrder>();

            foreach (SpeciesObservationFieldSortOrder speciesObservationFieldSortOrder in speciesObservationFieldSortOrders)
            {
                WebSpeciesObservationFieldSortOrder webSpeciesObservationSortOrder = new WebSpeciesObservationFieldSortOrder();

                if (speciesObservationFieldSortOrder.Class.IsNotNull())
                {
                    webSpeciesObservationSortOrder.Class = new WebSpeciesObservationClass();
                    webSpeciesObservationSortOrder.Class.Id = speciesObservationFieldSortOrder.Class.Id;
                    webSpeciesObservationSortOrder.Class.Identifier = speciesObservationFieldSortOrder.Class.Identifier;
                }

                if (speciesObservationFieldSortOrder.Property.IsNotNull())
                {
                    webSpeciesObservationSortOrder.Property = new WebSpeciesObservationProperty();
                    webSpeciesObservationSortOrder.Property.Id = speciesObservationFieldSortOrder.Property.Id;
                    webSpeciesObservationSortOrder.Property.Identifier = speciesObservationFieldSortOrder.Property.Identifier;
                }
                webSpeciesObservationSortOrder.SortOrder = speciesObservationFieldSortOrder.SortOrder;

                webSpeciesObservationSortOrders.Add(webSpeciesObservationSortOrder);
            }
            return webSpeciesObservationSortOrders;
        }


        /// <summary>
        /// Convert a ISpeciesObservationPageSpecification instance
        /// to a WebSpeciesObservationPageSpecification instance.
        /// </summary>
        /// <param name="speciesObservationPageSpecification">An ISpeciesObservationPageSpecification instance.</param>
        /// <returns>A WebSpeciesObservationPageSpecification instance.</returns>
        protected WebSpeciesObservationPageSpecification GetSpeciesObservationPageSpecification(ISpeciesObservationPageSpecification speciesObservationPageSpecification)
        {
            WebSpeciesObservationPageSpecification webSpeciesObservationPageSpecification;

            webSpeciesObservationPageSpecification = new WebSpeciesObservationPageSpecification();
            webSpeciesObservationPageSpecification.Size = speciesObservationPageSpecification.Size;
            webSpeciesObservationPageSpecification.SortOrder = new List<WebSpeciesObservationFieldSortOrder>();
            webSpeciesObservationPageSpecification.SortOrder = GetSortOrder(speciesObservationPageSpecification.SortOrder);
            webSpeciesObservationPageSpecification.Start = speciesObservationPageSpecification.Start;

            return webSpeciesObservationPageSpecification;
        }

        private List<WebSpeciesObservationFieldSortOrder> GetSortOrder(List<ISpeciesObservationFieldSortOrder> speciesObservationFieldSortOrders)
        {
            List<WebSpeciesObservationFieldSortOrder> webSpeciesObservationFieldSortOrders = new List<WebSpeciesObservationFieldSortOrder>();

            foreach (ISpeciesObservationFieldSortOrder speciesObservationFieldSortOrder in speciesObservationFieldSortOrders)
            {
                WebSpeciesObservationFieldSortOrder webSpeciesObservationFieldSortOrder = new WebSpeciesObservationFieldSortOrder();
                if (speciesObservationFieldSortOrder.Class.IsNotNull())
                {
                    webSpeciesObservationFieldSortOrder.Class = new WebSpeciesObservationClass();
                    webSpeciesObservationFieldSortOrder.Class.Id = speciesObservationFieldSortOrder.Class.Id;
                    webSpeciesObservationFieldSortOrder.Class.Identifier = speciesObservationFieldSortOrder.Class.Identifier;
                }
                if (speciesObservationFieldSortOrder.Property.IsNotNull())
                {
                    webSpeciesObservationFieldSortOrder.Property = new WebSpeciesObservationProperty();
                    webSpeciesObservationFieldSortOrder.Property.Id = speciesObservationFieldSortOrder.Property.Id;
                    webSpeciesObservationFieldSortOrder.Property.Identifier = speciesObservationFieldSortOrder.Property.Identifier;
                }
                webSpeciesObservationFieldSortOrder.SortOrder = speciesObservationFieldSortOrder.SortOrder;

                webSpeciesObservationFieldSortOrders.Add(webSpeciesObservationFieldSortOrder);
            }

            return webSpeciesObservationFieldSortOrders;
        }

        /// <summary>
        /// Get token used when communicating with web service.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>Token</returns>
        protected String GetToken(IUserContext userContext)
        {
            return (String)(userContext.Properties[GetTokenKey(userContext)]); ;
        }

        /// <summary>
        /// Get key used when handling web service token.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>Key used when handling web service token.</returns>
        private String GetTokenKey(IUserContext userContext)
        {
            return Settings.Default.ClientTokenKey +
                   ":" + GetWebServiceName();
        }

        /// <summary>
        /// Get web service name.
        /// </summary>
        protected abstract String GetWebServiceName();

        /// <summary>
        /// Set token used when communicating with web service.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="token">Token.</param>
        protected void SetToken(IUserContext userContext, String token)
        {
            if (token.IsNull())
            {
                // User has logged out. Remove token.
                if (userContext.Properties.ContainsKey(GetTokenKey(userContext)))
                {
                    userContext.Properties.Remove(GetTokenKey(userContext));
                }
            }
            else
            {
                userContext.Properties[GetTokenKey(userContext)] = token;
            }
        }
    }
}
