using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewManagers
{
    /// <summary>
    /// This class is a base class for MySettings view managers
    /// </summary>
    public class ViewManagerBase
    {
        protected readonly IUserContext UserContext;
        protected readonly AnalysisPortal.MySettings.MySettings MySettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewManagerBase"/> class.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="mySettings">The MySettings object.</param>
        public ViewManagerBase(IUserContext userContext, AnalysisPortal.MySettings.MySettings mySettings)
        {
            UserContext = userContext;
            MySettings = mySettings;
        }

        ///// <summary>
        ///// Initializes a new instance of the <see cref="ViewManagerBase"/> class.
        ///// </summary>
        ///// <param name="userContext">The user context.</param>
        //public ViewManagerBase(IUserContext userContext)
        //{
        //    UserContext = userContext;            
        //}

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewManagerBase"/> class.
        /// </summary>
        public ViewManagerBase()
        {
        }
    }
}
