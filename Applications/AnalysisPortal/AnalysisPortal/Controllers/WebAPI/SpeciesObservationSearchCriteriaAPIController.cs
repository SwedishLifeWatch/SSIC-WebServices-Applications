using AnalysisPortal.Helpers.WebAPI;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace AnalysisPortal.Controllers.WebAPI
{
    /// <summary>
    /// A Web API controller for accessing and setting MySettings.Filter in order to modify SpeciesObservationSearchCriteria.
    /// API is accessible with the following URI: /api/SpeciesObservationSearchCriteriaAPI.
    /// The data is sent back and forth in Json format, and automatically serialized/deserialized:
    /// {
    ///     FullUpdate: true,
    ///     Filter: {
    ///         Taxa: {
    ///             IsActive: true,
    ///             TaxonIds: [12,45,78]
    ///         },
    ///         Temporal: {
    ///             IsActive: true,
    ///             ObservationDate: {
    ///                 UseSetting: false,
    ///                 Annually: false,
    ///                 StartDate: "2009-04-12T20:44:55",
    ///                 EndDate: "2009-04-12T20:44:55"
    ///             },
    ///             RegistrationDate: {
    ///                 UseSetting: false,
    ///                 Annually: false,
    ///                 StartDate: "2009-04-12T20:44:55",
    ///                 EndDate: "2009-04-12T20:44:55"
    ///             },
    ///             ChangeDate: {
    ///                 UseSetting: false,
    ///                 Annually: false,
    ///                 StartDate: "2009-04-12T20:44:55",
    ///                 EndDate: "2009-04-12T20:44:55"
    ///             }
    ///         },
    ///         Accuracy: {
    ///             IsActive: true,
    ///             MaxCoordinateAccuracy: 100,
    ///             Inclusive: false
    ///         },
    ///         Occurrence: {
    ///             IncludeNeverFoundObservations: true,
    ///             IncludeNotRediscoveredObservations: false,
    ///             IncludePositiveObservations: true,
    ///             IsNaturalOccurrence: false,
    ///             IsNotNaturalOccurrence: false
    ///         }
    ///     }
    /// }
    /// The FullUpdate property is optional, as all the sections are.
    /// This is handled in the code by checking if the corresponding property is null in the MySettingsWebAPI class.
    /// </summary>
    public class SpeciesObservationSearchCriteriaAPIController : ApiController
    {
        /// <summary>
        /// Retrieves the actual settings for the filter.
        /// Handles a GET request.
        /// </summary>
        /// <returns></returns>
        public MySettings GetMySettings()
        {
            return SessionHandler.MySettings;
        }

        /// <summary>
        /// Creates a new instance of the filter.
        /// Handles a POST request.
        /// </summary>
        /// <returns>A HTTP response containing a 201 Created status code and the newly created instance of the filter.</returns>
        public HttpResponseMessage PostMySettings()
        {
            try
            {
                SessionHandler.MySettings = new MySettings();

                var response = Request.CreateResponse(HttpStatusCode.Created, SessionHandler.MySettings);

#if DEBUG
                response.Headers.Location = new Uri(Url.Link("Default", new { controller = "Debug", action = "SpeciesObservationSearchCriteriaAPI" }));
#endif
                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Updates the current settings of the filter.
        /// Handles a PUT request.
        /// </summary>
        /// <param name="mySettingsWebApi">The Json object sent together with the request.</param>
        /// <returns>A HTTP response containing a 20x  status code and the modified instance of the filter.</returns>
        public HttpResponseMessage PutMySettings(MySettingsWebAPI mySettingsWebApi)
        {
            try
            {
                if (mySettingsWebApi == null)
                {
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent("No data sent.") });
                }

                if (mySettingsWebApi.Filter != null)
                {
                    bool fullUpdate = mySettingsWebApi.FullUpdate.GetValueOrDefault();
                    StringBuilder errorMessage = new StringBuilder();

                    if (mySettingsWebApi.Filter.Taxa != null)
                    {
                        SessionHandler.MySettings.Filter.Taxa.IsActive = mySettingsWebApi.Filter.Taxa.IsActive;
                        SessionHandler.MySettings.Filter.Taxa.TaxonIds.Clear();
                        if (mySettingsWebApi.Filter.Taxa.IsActive)
                        {
                            SessionHandler.MySettings.Filter.Taxa.AddTaxonIds(mySettingsWebApi.Filter.Taxa.TaxonIds);
                        }
                    }

                    if (mySettingsWebApi.Filter.Temporal != null)
                    {
                        SessionHandler.MySettings.Filter.Temporal.IsActive = mySettingsWebApi.Filter.Temporal.IsActive;
                        if (mySettingsWebApi.Filter.Temporal.IsActive)
                        {
                            bool error = false;

                            if (mySettingsWebApi.Filter.Temporal.ObservationDate.StartDate > mySettingsWebApi.Filter.Temporal.ObservationDate.EndDate)
                            {
                                error = true;
                                errorMessage.Append("EndDate can not be smaller than StartDate for ObservationDate.");
                            }

                            if (mySettingsWebApi.Filter.Temporal.RegistrationDate.StartDate > mySettingsWebApi.Filter.Temporal.RegistrationDate.EndDate)
                            {
                                error = true;
                                errorMessage.Append("EndDate can not be smaller than StartDate for RegistrationDate.");
                            }

                            if (mySettingsWebApi.Filter.Temporal.ChangeDate.StartDate > mySettingsWebApi.Filter.Temporal.ChangeDate.EndDate)
                            {
                                error = true;
                                errorMessage.Append("EndDate can not be smaller than StartDate for ChangeDate.");
                            }

                            if (error)
                            {
                                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent(errorMessage.ToString()) });
                            }
                            SessionHandler.MySettings.Filter.Temporal.ObservationDate.StartDate = mySettingsWebApi.Filter.Temporal.ObservationDate.StartDate;
                            SessionHandler.MySettings.Filter.Temporal.ObservationDate.EndDate = mySettingsWebApi.Filter.Temporal.ObservationDate.EndDate;
                            SessionHandler.MySettings.Filter.Temporal.ObservationDate.Annually = mySettingsWebApi.Filter.Temporal.ObservationDate.Annually;
                            SessionHandler.MySettings.Filter.Temporal.ObservationDate.UseSetting = mySettingsWebApi.Filter.Temporal.ObservationDate.UseSetting;
                            SessionHandler.MySettings.Filter.Temporal.RegistrationDate.StartDate = mySettingsWebApi.Filter.Temporal.RegistrationDate.StartDate;
                            SessionHandler.MySettings.Filter.Temporal.RegistrationDate.EndDate = mySettingsWebApi.Filter.Temporal.RegistrationDate.EndDate;
                            SessionHandler.MySettings.Filter.Temporal.RegistrationDate.Annually = mySettingsWebApi.Filter.Temporal.RegistrationDate.Annually;
                            SessionHandler.MySettings.Filter.Temporal.RegistrationDate.UseSetting = mySettingsWebApi.Filter.Temporal.RegistrationDate.UseSetting;
                            SessionHandler.MySettings.Filter.Temporal.ChangeDate.StartDate = mySettingsWebApi.Filter.Temporal.ChangeDate.StartDate;
                            SessionHandler.MySettings.Filter.Temporal.ChangeDate.EndDate = mySettingsWebApi.Filter.Temporal.ChangeDate.EndDate;
                            SessionHandler.MySettings.Filter.Temporal.ChangeDate.Annually = mySettingsWebApi.Filter.Temporal.ChangeDate.Annually;
                            SessionHandler.MySettings.Filter.Temporal.ChangeDate.UseSetting = mySettingsWebApi.Filter.Temporal.ChangeDate.UseSetting;
                        }
                        else
                        {
                            SessionHandler.MySettings.Filter.Temporal.ObservationDate.ResetSettings();
                            SessionHandler.MySettings.Filter.Temporal.RegistrationDate.ResetSettings();
                            SessionHandler.MySettings.Filter.Temporal.ChangeDate.ResetSettings();
                        }
                    }

                    if (mySettingsWebApi.Filter.Accuracy != null)
                    {
                        SessionHandler.MySettings.Filter.Accuracy.IsActive = mySettingsWebApi.Filter.Accuracy.IsActive;
                        if (mySettingsWebApi.Filter.Accuracy.IsActive)
                        {
                            SessionHandler.MySettings.Filter.Accuracy.MaxCoordinateAccuracy = mySettingsWebApi.Filter.Accuracy.MaxCoordinateAccuracy;
                            SessionHandler.MySettings.Filter.Accuracy.Inclusive = mySettingsWebApi.Filter.Accuracy.Inclusive;
                        }
                        else
                        {
                            SessionHandler.MySettings.Filter.Accuracy.ResetSettings();
                        }
                    }

                    if (mySettingsWebApi.Filter.Occurrence != null)
                    {
                        SessionHandler.MySettings.Filter.Occurrence.IncludeNeverFoundObservations = mySettingsWebApi.Filter.Occurrence.IncludeNeverFoundObservations;
                        SessionHandler.MySettings.Filter.Occurrence.IncludeNotRediscoveredObservations = mySettingsWebApi.Filter.Occurrence.IncludeNotRediscoveredObservations;
                        SessionHandler.MySettings.Filter.Occurrence.IncludePositiveObservations = mySettingsWebApi.Filter.Occurrence.IncludePositiveObservations;
                        SessionHandler.MySettings.Filter.Occurrence.IsNaturalOccurrence = mySettingsWebApi.Filter.Occurrence.IsNaturalOccurrence;
                        SessionHandler.MySettings.Filter.Occurrence.IsNotNaturalOccurrence = mySettingsWebApi.Filter.Occurrence.IsNotNaturalOccurrence;
                    }
                }

                var response = Request.CreateResponse(HttpStatusCode.OK, SessionHandler.MySettings);

#if DEBUG
                response.Headers.Location = new Uri(Url.Link("Default", new { controller = "Debug", action = "SpeciesObservationSearchCriteriaAPI" }));
#endif
                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public void DeleteMySettings()
        {
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotImplemented));
        }
    }
}