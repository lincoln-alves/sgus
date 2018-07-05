using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Sebrae.Academico.Util.Classes
{
    /// <summary>
    /// This class is an alternative when you can't use Service References. It allows you to invoke Web Methods on a given Web Service URL.
    /// Based on the code from http://stackoverflow.com/questions/9482773/web-service-without-adding-a-reference
    /// </summary>
    public class WebService
    {
        public string Url { get; set; }
        public string Method { get; private set; }
        public Dictionary<string, string> Params = new Dictionary<string, string>();
        public XDocument ResponseSoap = XDocument.Parse("<root/>");
        public XDocument ResultXml = XDocument.Parse("<root/>");
        public string ResultString = string.Empty;
        public string RequestEnvelop = string.Empty;


        public WebService()
        {
            Url = string.Empty;
            Method = string.Empty;
        }

        public WebService(string baseUrl)
        {
            Url = baseUrl;
            Method = string.Empty;
        }

        public WebService(string baseUrl, string methodName)
        {
            Url = baseUrl;
            Method = methodName;
        }

        // Public API

        /// <summary>
        /// Adds a parameter to the WebMethod invocation.
        /// </summary>
        /// <param name="name">Name of the WebMethod parameter (case sensitive)</param>
        /// <param name="value">Value to pass to the paramenter</param>
        public void AddParameter(string name, string value)
        {
            Params.Add(name, value);
        }

        public void Invoke(string username, string password, string serviceName, List<string> aditionalNamespaces = null)
        {
            Invoke(username, password, serviceName, Method, true, aditionalNamespaces);
        }

        /// <summary>
        /// Using the base url, invokes the WebMethod with the given name
        /// </summary>
        /// <param name="serviceName">Service name</param>
        /// <param name="methodName">Web Method name</param>
        /// <param name="username">Login user to the server</param>
        /// <param name="password">Password to the login user</param>
        /// <param name="aditionalNamespaces">Aditional params that will be concatenated to the main XML string.</param>
        /// <param name="defaultXmlns">Default xmlns namespace</param>
        public void Invoke(string username, string password, string serviceName, string methodName, List<string> aditionalNamespaces = null, string defaultXmlns = null)
        {
            Invoke(username, password, serviceName, methodName, true, aditionalNamespaces, defaultXmlns);
        }

        /// <summary>
        /// Cleans all internal data used in the last invocation, except the WebService's URL.
        /// This avoids creating a new WebService object when the URL you want to use is the same.
        /// </summary>
        public void CleanLastInvoke()
        {
            ResponseSoap = ResultXml = null;
            ResultString = Method = string.Empty;
            Params = new Dictionary<string, string>();
            RequestEnvelop = string.Empty;
        }

        #region Helper Methods

        /// <summary>
        /// Checks if the WebService's URL and the WebMethod's name are valid. If not, throws ArgumentNullException.
        /// </summary>
        /// <param name="methodName">Web Method name (optional)</param>
        private void AssertCanInvoke(string methodName = "")
        {
            if (Url == string.Empty)
                throw new Exception("You tried to invoke a webservice without specifying the WebService's URL.");

            if ((methodName == "") && (Method == string.Empty))
                throw new Exception("You tried to invoke a webservice without specifying the WebMethod.");
        }

        private void ExtractResult(string methodName)
        {
            try
            {
                // Selects just the elements with namespace http://tempuri.org/ (i.e. ignores SOAP namespace)
                var namespMan = new XmlNamespaceManager(new NameTable());
                namespMan.AddNamespace("foo", "http://tempuri.org/");

                var webMethodResult = ResponseSoap.XPathSelectElement("//foo:" + methodName + "Result", namespMan);
                // If the result is an XML, return it and convert it to string
                if (webMethodResult.FirstNode.NodeType == XmlNodeType.Element)
                {
                    ResultXml = XDocument.Parse(webMethodResult.ToString());
                    ResultXml = XmlUtil.RemoveNamespaces(ResultXml);
                    ResultString = ResultXml.ToString();
                }
                // If the result is a string, return it and convert it to XML (creating a root node to wrap the result)
                else
                {
                    ResultString = webMethodResult.FirstNode.ToString();
                    ResultXml = XDocument.Parse("<root>" + ResultString + "</root>");
                }
            }
            catch (Exception)
            {
                throw new Exception("Objeto XML da biblioteca externa em formato não serializável.");
            }
        }

        /// <summary>
        /// Invokes a Web Method, with its parameters encoded or not.
        /// </summary>
        /// <param name="serviceName">Name of the service being used in the URL</param>
        /// <param name="methodName">Name of the web method you want to call (case sensitive)</param>
        /// <param name="encode">Do you want to encode your parameters? (default: true)</param>
        /// <param name="username">Login user to the server</param>
        /// <param name="password">Password to the login user</param>
        /// <param name="aditionalNamespaces">Aditional params that will be concatenated to the main XML string.</param>
        /// <param name="defaultXmlns">Default xmlns namespace</param>
        private void Invoke(string username, string password, string serviceName, string methodName, bool encode, List<string> aditionalNamespaces = null, string defaultXmlns = null)
        {
            AssertCanInvoke(methodName);

            var soapStr = RequestEnvelop;

            var req = (HttpWebRequest)WebRequest.Create(Url);
            req.Headers.Add("SOAPAction", "\"http://tempuri.org/" + serviceName + "/" + methodName + "\"");
            req.ContentType = "text/xml;charset=\"utf-8\"";
            req.Accept = "text/xml";
            req.Method = "POST";

            using (var stm = req.GetRequestStream())
            {
                var postValues = "";

                foreach (var param in Params)
                {
                    if (encode)
                        postValues +=
                            string.Format(
                                "<{0}{2}>{1}</{0}>",
                                HttpUtility.HtmlEncode(param.Key),
                                HttpUtility.HtmlEncode(param.Value),
                                string.IsNullOrWhiteSpace(defaultXmlns)
                                    ? ""
                                    : string.Format(" xmlns=\"{0}\"", defaultXmlns));

                    else postValues += string.Format("<{0}>{1}</{0}>", param.Key, param.Value);
                }

                // Default authentication.
                soapStr = soapStr.Replace("{0}", username);
                soapStr = soapStr.Replace("{1}", password);

                soapStr = soapStr.Replace("{2}", postValues);

                var ctNamespaces = 3;

                if (aditionalNamespaces != null && aditionalNamespaces.Any())
                {
                    foreach (var adtName in aditionalNamespaces)
                    {
                        soapStr = soapStr.Replace("{" + ctNamespaces + "}", adtName);

                        ctNamespaces++;
                    }
                }

                using (var stmw = new StreamWriter(stm))
                {
                    stmw.Write(soapStr);
                }
            }

            using (var responseReader = new StreamReader(req.GetResponse().GetResponseStream()))
            {
                var result = responseReader.ReadToEnd();


                ResponseSoap = XDocument.Parse(result);

                ExtractResult(methodName);
            }
        }

        public void PreInvoke()
        {
            CleanLastInvoke();
        }

        #endregion
    }
}
