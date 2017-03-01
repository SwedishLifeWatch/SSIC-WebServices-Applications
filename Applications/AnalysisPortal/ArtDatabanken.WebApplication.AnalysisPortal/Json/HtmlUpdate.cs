using Newtonsoft.Json;
namespace ArtDatabanken.WebApplication.AnalysisPortal.Json
{
    /// <summary>
    /// Used to enable Html updates in one Ajax request
    /// Not used yet...
    /// </summary>
    /// <remarks>
    /// 
    /// In JsonModel a property like this could be added to enable html updates in one request
    ///   /// <summary>
    ///   /// Gets or sets the HTML updates.
    ///   /// 
    ///   /// Contains Html for (usually) Divs that needs to
    ///   /// update their content due to changes in session state
    ///   /// that was made by this Ajax call.
    ///   /// </summary>
    ///   [JsonProperty(PropertyName = "htmlUpdates")]
    ///   public List<HtmlUpdate> HtmlUpdates { get; set; }
    /// </remarks>
    public class HtmlUpdate
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "html")]
        public string Html { get; set; }

        [JsonProperty(PropertyName = "javascript")]
        public string JavaScript { get; set; }
    }
}
