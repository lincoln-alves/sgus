using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Sebrae.Academico.Util.Classes
{
    public class XmlUtil
    {
        /// <summary>
        /// Remove all xmlns:* and a:* instances from the passed XmlDocument to simplify our xpath expressions
        /// </summary>
        public static XDocument RemoveNamespaces(XDocument oldXml)
        {
            // FROM: http://social.msdn.microsoft.com/Forums/en-US/bed57335-827a-4731-b6da-a7636ac29f21/xdocument-remove-namespace?forum=linqprojectgeneral
            try
            {
                oldXml.Descendants().Attributes().Where(a => a.IsNamespaceDeclaration).Remove();

                var newXml = XDocument.Parse(Regex.Replace(
                    oldXml.ToString(),
                    @"(xmlns:?[^=]*=[""][^""]*[""])",
                    "",
                    RegexOptions.IgnoreCase | RegexOptions.Multiline)
                    );

                return newXml;
            }
            catch (XmlException error)
            {
                throw new XmlException(error.Message + " at Utils.RemoveNamespaces");
            }
        }

        /// <summary>
        /// Remove all xmlns:* instances from the passed XmlDocument to simplify our xpath expressions
        /// </summary>
        public static XDocument RemoveNamespaces(string oldXml)
        {
            var newXml = XDocument.Parse(oldXml);
            return RemoveNamespaces(newXml);
        }

        /// <summary>
        /// Converts a string that has been HTML-enconded for HTTP transmission into a decoded string.
        /// </summary>
        /// <param name="escapedString">String to decode.</param>
        /// <returns>Decoded (unescaped) string.</returns>
        public static string UnescapeString(string escapedString)
        {
            return HttpUtility.HtmlDecode(escapedString);
        }

        public static T FromXml<T>(XDocument xml)
        {
            try
            {
                var xmlSerializer = new XmlSerializer(typeof(T));

                using (var reader = (xml.Root == null ? xml.CreateReader() : xml.Root.CreateReader()))
                {
                    return (T)xmlSerializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro na deserialização do XML para Objeto. Mensagem do erro: " + ex.Message);
            }
        }
    }
}
