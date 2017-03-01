using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.MySettings.General
{
    [DataContract]
    public class GeneralSettings : SettingBase
    {
        //[DataMember]
        //public string CurrentCoordinateSystem { get; set; }

        //[DataMember]
        //public CoordinateSystemId CoordinateSystemId { get; set; }

        //public GeneralSettings()
        //{
        //    CurrentCoordinateSystem = "EPSG:900913"; // Google Mercator
        //    //CurrentCoordinateSystem = "EPSG:3006"; // SWEREF 99
        //    //CurrentCoordinateSystem = "EPSG:4326"; // WGS84
        //    //CurrentCoordinateSystem = "EPSG:3021"; // RT90
        //    CoordinateSystemId = CoordinateSystemId.GoogleMercator;            
        //}
    }
}
