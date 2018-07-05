using System;
using System.Xml.Serialization;

namespace Sebrae.Academico.BP.DTO.Services.EF
{
    [Serializable, XmlRoot("ActivateSubscriptionResult")]
    public class DTOActivateSubscription : BasicEfEntity
    {
        public string LaunchUrl { get; set; }
        public int SubscriptionId { get; set; }
    }
}