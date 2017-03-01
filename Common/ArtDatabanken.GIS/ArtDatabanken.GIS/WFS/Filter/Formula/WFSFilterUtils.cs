using System;

namespace ArtDatabanken.GIS.WFS.Filter.Formula
{


    /// <summary>
    /// This static class contains utility functions for WFS formula creation and parsing
    /// This class can be compiled using SharpKit C# to JavaScript compiler in Ext Js mode http://sharpkit.net/
    /// </summary>
    public static class WFSFilterUtils
    {

        /// <summary>
        /// Determines whether the parameter strWfsComparisionOperator is a comparision operator.
        /// </summary>
        /// <param name="strWfsComparisionOperator">String to examine.</param>        
        public static bool IsComparisionOperator(string strWfsComparisionOperator)
        {
            string str = strWfsComparisionOperator.ToLower();

            if (str == "PropertyIsGreaterThan".ToLower())
                return true;
            if (str == "PropertyIsLessThan".ToLower())
                return true;
            if (str == "PropertyIsGreaterThanOrEqualTo".ToLower())
                return true;
            if (str == "PropertyIsLessThanOrEqualTo".ToLower())
                return true;
            if (str == "PropertyIsNotEqualTo".ToLower())
                return true;
            if (str == "PropertyIsEqualTo".ToLower())
                return true;
            if (str == "PropertyIsLike".ToLower())
                return true;
            if (str == "PropertyIsNull".ToLower())
                return true;

            return false;
        }


        /// <summary>
        /// Gets a comparision operator enum from a string.
        /// </summary>
        /// <param name="strWfsComparisionOperator">WFS comparision operator string.</param>        
        public static WFSComparisionOperator GetComparisionOperator(string strWfsComparisionOperator)
        {
            string str = strWfsComparisionOperator.ToLower();

            if (str == "PropertyIsGreaterThan".ToLower())
                return WFSComparisionOperator.GreaterThan;
            if (str == "PropertyIsLessThan".ToLower())
                return WFSComparisionOperator.LessThan;
            if (str == "PropertyIsGreaterThanOrEqualTo".ToLower())
                return WFSComparisionOperator.GreaterOrEqualTo;
            if (str == "PropertyIsLessThanOrEqualTo".ToLower())
                return WFSComparisionOperator.LessOrEqualTo;
            if (str == "PropertyIsNotEqualTo".ToLower())
                return WFSComparisionOperator.NotEqualTo;
            if (str == "PropertyIsEqualTo".ToLower())
                return WFSComparisionOperator.EqualTo;
            if (str == "PropertyIsLike".ToLower())
                return WFSComparisionOperator.Like;
            if (str == "PropertyIsNull".ToLower())
                return WFSComparisionOperator.IsNull;
            throw new Exception("Operator not found");
        }

        /// <summary>
        /// Gets a binary comparision operator enum from a string.
        /// </summary>        
        /// <param name="strWfsBinaryComparisionOperator">WFS binary comparision operator string.</param>
        public static WFSBinaryComparisionOperator GetBinaryComparisionOperator(string strWfsBinaryComparisionOperator)
        {
            string str = strWfsBinaryComparisionOperator.ToLower();

            if (str == "PropertyIsGreaterThan".ToLower())
                return WFSBinaryComparisionOperator.GreaterThan;
            if (str == "PropertyIsLessThan".ToLower())
                return WFSBinaryComparisionOperator.LessThan;
            if (str == "PropertyIsGreaterThanOrEqualTo".ToLower())
                return WFSBinaryComparisionOperator.GreaterOrEqualTo;
            if (str == "PropertyIsLessThanOrEqualTo".ToLower())
                return WFSBinaryComparisionOperator.LessOrEqualTo;
            if (str == "PropertyIsNotEqualTo".ToLower())
                return WFSBinaryComparisionOperator.NotEqualTo;
            if (str == "PropertyIsEqualTo".ToLower())
                return WFSBinaryComparisionOperator.EqualTo;
            if (str == "PropertyIsLike".ToLower())
                return WFSBinaryComparisionOperator.Like;

            throw new Exception("Operator not found");
        }

        /// <summary>
        /// Gets a unary comparision operator enum from a string.
        /// </summary>                
        /// <param name="strWfsUnaryComparisionOperator">WFS unary comparision operator string.</param>
        public static WFSUnaryComparisionOperator GetUnaryComparisionOperator(string strWfsUnaryComparisionOperator)
        {
            string str = strWfsUnaryComparisionOperator.ToLower();

            if (str == "PropertyIsNull".ToLower())
                return WFSUnaryComparisionOperator.IsNull;

            throw new Exception("Operator not found");
        }


        /// <summary>
        /// Determines whether the parameter string is an logical operator.
        /// </summary>
        /// <param name="strWfsLogicalOperator">The string to examine.</param>        
        public static bool IsLogicalOperator(string strWfsLogicalOperator)
        {
            string str = strWfsLogicalOperator.ToLower();

            if (str == "And".ToLower())
                return true;
            if (str == "Or".ToLower())
                return true;
            if (str == "Not".ToLower())
                return true;

            return false;
        }

        /// <summary>
        /// Gets a wfs logical operator enum from a string.
        /// </summary>        
        /// <param name="strWfsLogicalOperator">WFS logical operator string.</param>
        public static WFSLogicalOperator GetLogicalOperator(string strWfsLogicalOperator)
        {
            string str = strWfsLogicalOperator.ToLower();

            if (str == "And".ToLower())
                return WFSLogicalOperator.And;
            if (str == "Or".ToLower())
                return WFSLogicalOperator.Or;
            if (str == "Not".ToLower())
                return WFSLogicalOperator.Not;

            throw new Exception("Operator not found");
        }

        /// <summary>
        /// Gets a wfs logical operator enum from a string.
        /// </summary>                
        /// <param name="strWfsBinaryLogicalOperator">WFS binary logical operator string. </param>
        public static WFSBinaryLogicalOperator GetBinaryLogicalOperator(string strWfsBinaryLogicalOperator)
        {
            string str = strWfsBinaryLogicalOperator.ToLower();

            if (str == "And".ToLower())
                return WFSBinaryLogicalOperator.And;
            if (str == "Or".ToLower())
                return WFSBinaryLogicalOperator.Or;

            throw new Exception("Operator not found");
        }

        /// <summary>
        /// Gets a wfs logical operator enum from a string.
        /// </summary>                
        /// <param name="strWfsUnaryLogicalOperator">WFS unary logical operator string. </param>
        public static WFSUnaryLogicalOperator GetUnaryLogicalOperator(string strWfsUnaryLogicalOperator)
        {
            string str = strWfsUnaryLogicalOperator.ToLower();

            if (str == "Not".ToLower())
                return WFSUnaryLogicalOperator.Not;            

            throw new Exception("Operator not found");
        }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public static string GetResultingUrl(string server, string typeName, string version, string filter = null, string outputFormat = null, string maxFeatures = null, string srsName = null)
        {
            var url = string.Format("{0}?service=wfs&version={1}&request=GetFeature&typeName={2}", server, version, typeName);
            
            if (!string.IsNullOrEmpty(filter))
                url += "&filter=" + filter;
            if (!string.IsNullOrEmpty(outputFormat))
                url += "&outputFormat=" + outputFormat;
            if (!string.IsNullOrEmpty(maxFeatures))
                url += "&maxFeatures=" + maxFeatures;
            if (!string.IsNullOrEmpty(srsName))
                url += "&srsName=" + srsName;

            return url;
        }

       

    }
}
