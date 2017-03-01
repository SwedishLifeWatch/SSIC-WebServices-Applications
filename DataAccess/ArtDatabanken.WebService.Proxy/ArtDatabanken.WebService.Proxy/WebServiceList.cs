using System;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Proxy
{
    /// <summary>
    /// List class for the IWebService interface.
    /// </summary>
    [Serializable]
    public class WebServiceList : DataList<IWebService>
    {
    }
}
