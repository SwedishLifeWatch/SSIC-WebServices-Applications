using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace AnalysisPortal
{
    /// <summary>
    /// Summary description for Proxy
    /// </summary>
    public class Proxy : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            HttpResponse response = context.Response;

            // Check for query string
            string uri = HttpUtility.UrlDecode(context.Request.QueryString["url"]);
            if (string.IsNullOrWhiteSpace(uri))
            {
                response.StatusCode = 403;
                response.End();
                return;
            }

            // Filter requests - todo           
            //if (!uri.ToLowerInvariant().Contains("wikimedia.org"))
            //{
            //    response.StatusCode = 403;
            //    response.End();
            //    return;
            //}

            // Create web request
            WebRequest webRequest = WebRequest.Create(new Uri(uri));
            webRequest.Timeout = (int)TimeSpan.FromMinutes(10).TotalMilliseconds;
            webRequest.Method = context.Request.HttpMethod;

            if (webRequest.Method == "POST")
            {
                // copy original request "body" to the new request
                string input = new StreamReader(context.Request.InputStream).ReadToEnd();
                // encode it using the predefined encoding (see above, req.ContentType)
                var encoding = new System.Text.UTF8Encoding();
                byte[] bytesToSend = encoding.GetBytes(input);
                // Set the content length of the string being posted.
                webRequest.ContentLength = bytesToSend.Length;
                Stream newStream = webRequest.GetRequestStream(); // This method has the side effect of initiating delivery of the request in its current state to the server. Any properties like the request method, content type or content length as well as any custom headers need to be assigned before calling the GetRequestStream() method.
                newStream.Write(bytesToSend, 0, bytesToSend.Length);
                // Close the Stream object.
                newStream.Close();
            } // else GET, no body to send. Other verbs are not supported at the moment.

            // Send the request to the server
            WebResponse serverResponse = null;
            try
            {
                serverResponse = webRequest.GetResponse();
            }
            catch (WebException webExc)
            {
                response.StatusCode = 500;
                response.StatusDescription = webExc.Status.ToString();
                response.Write(webExc.Response);
                response.End();
                return;
            }

            // Configure reponse
            response.ContentType = serverResponse.ContentType;
            Stream stream = serverResponse.GetResponseStream();

            var buffer = new byte[32768];
            int read = 0;

            int chunk;
            while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
            {
                read += chunk;
                if (read != buffer.Length) { continue; }
                int nextByte = stream.ReadByte();
                if (nextByte == -1) { break; }

                // Resize the buffer
                var newBuffer = new byte[buffer.Length * 2];
                Array.Copy(buffer, newBuffer, buffer.Length);
                newBuffer[read] = (byte)nextByte;
                buffer = newBuffer;
                read++;
            }

            // Buffer is now too big. Shrink it.
            var ret = new byte[read];
            Array.Copy(buffer, ret, read);

            response.OutputStream.Write(ret, 0, ret.Length);
            serverResponse.Close();
            stream.Close();
            response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}