using System;
using System.Xml.Serialization;

namespace Sebrae.Academico.BP.DTO.Services.EF
{
    [Serializable, XmlRoot("CreateMemberResult")]
    public class DTOCreateMember : BasicEfEntity
    {
        public string LaunchUrl { get; set; }
        public int MemberId { get; set; }
    }
}