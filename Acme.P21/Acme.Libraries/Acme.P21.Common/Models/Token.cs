using System.Xml.Serialization;

namespace Acme.P21.Common.Models
{
    [XmlRoot(ElementName = "Token", Namespace = "http://distributionsuite.activant.com")]
    public class Token
    {

        [XmlElement(ElementName = "AccessToken", Namespace = "")]
        public string AccessToken { get; set; }

        [XmlElement(ElementName = "TokenType", Namespace = "")]
        public string TokenType { get; set; }

        [XmlElement(ElementName = "UserName", Namespace = "")]
        public string UserName { get; set; }

        [XmlElement(ElementName = "ExpiresIn", Namespace = "")]
        public int ExpiresIn { get; set; }

        [XmlElement(ElementName = "RefreshToken", Namespace = "")]
        public object RefreshToken { get; set; }

        [XmlElement(ElementName = "Scope", Namespace = "")]
        public object Scope { get; set; }

        [XmlElement(ElementName = "SessionId", Namespace = "")]
        public string SessionId { get; set; }

        [XmlAttribute(AttributeName = "xsd", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsd { get; set; }

        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }

        [XmlAttribute(AttributeName = "act", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Act { get; set; }

        [XmlText]
        public string Text { get; set; }
    }
}
