using System.Net;
using System.Net.Http;
using System.Net.Http.Fakes;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using AnalysisPortal.Helpers.WebAPI;
using ArtDatabanken.WebApplication.AnalysisPortal.MySettings;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Helpers;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AnalysisPortal.Tests.Controllers.LiveControllersTest
{
    /// <summary>
    /// Test class for SpeciesObservationSearchCriteriaAPIController, it simulates Web API calls by means of using HttpClient.
    /// </summary>
    [TestClass]
    public class SpeciesObservationSearchCriteriaAPIControllerTest : DBTestControllerBaseTest
    {
        /// <summary>
        /// An instance of HttpClient used to perform the Web API calls.
        /// </summary>
        private HttpClient httpClient;

        /// <summary>
        /// Contains the response after the Web API calls.
        /// </summary>
        private HttpResponseMessage httpResponseMessage;

        private const string SPECIES_OBSERVATION_SEARCH_CRITERIA_API_URI = "/api/SpeciesObservationSearchCriteriaAPI";

        /// <summary>
        /// Serializes the result from the Http call.
        /// </summary>
        private JavaScriptSerializer javaScriptSerializer;

        /// <summary>
        /// The result in the response.
        /// </summary>
        private MySettings result;

        /// <summary>
        /// Creates the necessary object instances in order to run the tests.
        /// </summary>
        [TestInitialize]
        public void SpeciesObservationSearchCriteriaAsJSONTestInitialize()
        {
            // Arrange
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            javaScriptSerializer = new JavaScriptSerializer();
        }

        /// <summary>
        /// Simulates a Web API GET call to retrieve the current instance of the MySettings object.
        /// </summary>
        [TestMethod]
        [Ignore]
        [TestCategory("NightlyTestApp")]
        public void SpeciesObservationSearchCriteriaAPIGet()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var shimHttpClient = new ShimHttpClient(httpClient);

                // Act
                shimHttpClient.GetAsyncString = (uri) =>
                    {
                        var responseTask = new TaskCompletionSource<HttpResponseMessage>();
                        var httpResponse = new HttpResponseMessage
                                               {
                                                   StatusCode = HttpStatusCode.OK,
                                                   Content = new ObjectContent(typeof(MySettings), SessionHandler.MySettings, new JsonMediaTypeFormatter())
                                               };

                        responseTask.SetResult(httpResponse);
                        return responseTask.Task;
                    };
                httpResponseMessage = httpClient.GetAsync(SPECIES_OBSERVATION_SEARCH_CRITERIA_API_URI).Result;

                // Assert
                Assert.IsNotNull(httpResponseMessage);
                Assert.IsTrue(httpResponseMessage.IsSuccessStatusCode);
                result = javaScriptSerializer.Deserialize<MySettings>(httpResponseMessage.Content.ReadAsStringAsync().Result);
                Assert.IsNotNull(result);
            }
        }

        /// <summary>
        /// Simulates a Web API POST call to create a new instance of the MySettings object.
        /// </summary>
        [TestMethod]
        [Ignore]
        [TestCategory("NightlyTestApp")]
        public void SpeciesObservationSearchCriteriaAPIPost()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var shimHttpClient = new ShimHttpClient(httpClient);

                // Act
                shimHttpClient.PostAsyncUriHttpContent = (uri, content) =>
                    {
                        var responseTask = new TaskCompletionSource<HttpResponseMessage>();
                        var httpResponse = new HttpResponseMessage
                                               {
                                                   StatusCode = HttpStatusCode.Created,
                                                   Content = new ObjectContent(typeof(MySettings), new MySettings(), new JsonMediaTypeFormatter())
                                               };

                        responseTask.SetResult(httpResponse);
                        return responseTask.Task;
                    };
                httpResponseMessage = httpClient.PostAsync(SPECIES_OBSERVATION_SEARCH_CRITERIA_API_URI, null).Result;

                // Assert
                Assert.IsNotNull(httpResponseMessage);
                Assert.IsTrue(httpResponseMessage.IsSuccessStatusCode);
                Assert.AreEqual(httpResponseMessage.StatusCode, HttpStatusCode.Created);
                result = javaScriptSerializer.Deserialize<MySettings>(httpResponseMessage.Content.ReadAsStringAsync().Result);
                Assert.IsNotNull(result);
            }
        }

        /// <summary>
        /// Simulates a Web API PUT call to update the current instance of the MySettings object.
        /// </summary>
        [TestMethod]
        [Ignore]
        [TestCategory("NightlyTestApp")]
        public void SpeciesObservationSearchCriteriaAPIPut()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var shimHttpClient = new ShimHttpClient(httpClient);
                MySettings mySettings = new MySettings();

                mySettings.Filter.Taxa.IsActive = true;
                mySettings.Filter.Taxa.TaxonIds.Add(34);

                MySettingsWebAPI mySettingsWebAPI = new MySettingsWebAPI { Filter = mySettings.Filter };

                // Act
                shimHttpClient.PutAsyncUriHttpContent = (uri, content) =>
                    {
                        SessionHandler.MySettings.Filter = mySettingsWebAPI.Filter;

                        var responseTask = new TaskCompletionSource<HttpResponseMessage>();
                        var httpResponse = new HttpResponseMessage
                                               {
                                                   StatusCode = HttpStatusCode.OK,
                                                   Content = new ObjectContent(typeof(MySettings), SessionHandler.MySettings, new JsonMediaTypeFormatter())
                                               };

                        responseTask.SetResult(httpResponse);
                        return responseTask.Task;
                    };
                httpResponseMessage = httpClient.PutAsync(SPECIES_OBSERVATION_SEARCH_CRITERIA_API_URI, null).Result;

                // Assert
                Assert.IsNotNull(httpResponseMessage);
                Assert.IsTrue(httpResponseMessage.IsSuccessStatusCode);
                result = javaScriptSerializer.Deserialize<MySettings>(httpResponseMessage.Content.ReadAsStringAsync().Result);
                Assert.IsNotNull(result);
                Assert.AreEqual(mySettings.Filter.Taxa.TaxonIds[0], result.Filter.Taxa.TaxonIds[0]);
            }
        }

        [TestMethod]
        [Ignore]
        [TestCategory("NightlyTestApp")]
        public void SpeciesObservationSearchCriteriaAPIDelete()
        {
            using (ShimsContext.Create())
            {
                // Arrange
                var shimHttpClient = new ShimHttpClient(httpClient);

                // Act
                shimHttpClient.DeleteAsyncUri = (uri) =>
                    {
                        var responseTask = new TaskCompletionSource<HttpResponseMessage>();
                        var httpResponse = new HttpResponseMessage { StatusCode = HttpStatusCode.NotImplemented };

                        responseTask.SetResult(httpResponse);
                        return responseTask.Task;
                    };
                httpResponseMessage = httpClient.DeleteAsync(SPECIES_OBSERVATION_SEARCH_CRITERIA_API_URI).Result;

                // Assert
                Assert.IsNotNull(httpResponseMessage);
                Assert.IsFalse(httpResponseMessage.IsSuccessStatusCode);
                Assert.AreEqual(httpResponseMessage.StatusCode, HttpStatusCode.NotImplemented);
            }
        }
    }
}