using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace ArtDatabanken.GIS.WFS.DescribeFeature
{
    /// <summary>
    /// TBD ?!?
    /// </summary>
    public class WFSClient
    {
        /// <summary>
        /// TBD ?!?
        /// </summary>
        public string xml = null;

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public Uri uri;

        /// <summary>
        /// TBD ?!?
        /// </summary>
        private XmlNamespaceManager _nsmgr;

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public Dictionary<string, string> fields;

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public WebProxy Proxy { get; set; }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public static void Test()
        {
            var wfsClient = new WFSClient();
            wfsClient.TypeName = "SLW:SpeciesSpecificOcurrenceCounts_10";
            var serverUrl = "http://slwgeo.artdata.slu.se:8080/geoserver/wfs?&VERSION=1.1.0";
            wfsClient.ReadDescribeFeatureType(serverUrl);            
        }

        /// <summary>
        /// TBD ?!?
        /// </summary>
        public void ReadDescribeFeatureType(string server = "")
        {
            //foreach (var ope in wfs.OperationsMetadata.Operations)
            //{
            //    if (ope.Name == "DescribeFeatureType")
            //    {
            //        foreach (var typ in ope.Dcps)
            //        {
            //            server = typ.Item.GetMethods[0].Href;
            //        }
            //    }
            //}


            if (server != "")
                Server = server;

            Uri u = new Uri(Server);

            Stream stream;
            if (u.IsAbsoluteUri && u.IsFile) //assume web if relative because IsFile is not supported on relative paths
            {
                stream = File.OpenRead(u.LocalPath);
            }
            else
            {
                uri = new Uri(CreateDescribeFeatureRequest(Server));
                stream = GetRemoteXmlStream(uri, Proxy);
            }

            XmlDocument xml = GetXml(stream);
            ParseDescribeFeatureType(xml);

        }

        private void ParseDescribeFeatureType(XmlDocument doc)
        {
            xml = doc.InnerXml;

            fields = new Dictionary<string, string>();
            _nsmgr = new XmlNamespaceManager(doc.NameTable);
            foreach (XmlNode nodes in doc.DocumentElement.Attributes)
            {
                if (nodes.Prefix == "xmlns")
                    _nsmgr.AddNamespace(nodes.Name, nodes.NamespaceURI);
            }
            var t = doc.DocumentElement["xsd:complexType"] == null ? doc.DocumentElement["complexType"] : doc.DocumentElement["xsd:complexType"];
            if (t == null) return;
            var complexContent = t["xsd:complexContent"] == null ? t["complexContent"] : t["xsd:complexContent"];
            var extension = complexContent["xsd:extension"] == null ? complexContent["extension"] : complexContent["xsd:extension"];
            var sequence = extension["xsd:sequence"] == null ? extension["sequence"] : extension["xsd:sequence"];
            fields = new Dictionary<string, string>();

            foreach (XmlNode ele in sequence)
            {
                fields.Add(ele.Attributes["name"].Value.ToString(),
                    ele.Attributes["type"].Value.ToString());

            }
        }

        private string CreateDescribeFeatureRequest(string url)
        {
            string wfsVersion = "1.1.0"; // todo - dynamisk
            var strReq = new StringBuilder(url);

            if (!url.Contains("?"))
            {
                strReq.Append("?");
            }
            else
            {
                if (!url.EndsWith("?"))
                {
                    strReq = new StringBuilder(url.Substring(0, url.IndexOf("?")));
                    strReq.Append("?");
                }


            }

            if (!strReq.ToString().EndsWith("&") && !strReq.ToString().EndsWith("?"))
                strReq.Append("&");

            if (!url.ToLower().Contains("service=wfs"))
                strReq.AppendFormat("SERVICE=WFS&");

            if (!url.ToLower().Contains("request=describefeatureType"))
                strReq.AppendFormat("REQUEST=DescribeFeatureType&");

            if (!url.ToLower().Contains("version=") && wfsVersion != null)
                strReq.AppendFormat("version=" + wfsVersion + "&");

            if (!url.ToLower().Contains("TypeName=") && TypeName != null)
                strReq.AppendFormat("TypeName=" + TypeName + "&");
            return strReq.ToString();
        }


        private XmlDocument GetXml(Stream stream)
        {
            try
            {
                var r = new XmlTextReader(stream);
                r.XmlResolver = null;

                var doc = new XmlDocument();

                doc.XmlResolver = null;
                doc.Load(r);

                stream.Close();

                return doc;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Could not download XML", ex);
            }
        }

        private static Stream GetRemoteXmlStream(Uri uri, WebProxy proxy)
        {
            WebRequest myRequest = WebRequest.Create(uri);
            if (proxy != null) myRequest.Proxy = proxy;

            WebResponse myResponse = myRequest.GetResponse();
            Stream stream = myResponse.GetResponseStream();

            return stream;
        }


    }
}
