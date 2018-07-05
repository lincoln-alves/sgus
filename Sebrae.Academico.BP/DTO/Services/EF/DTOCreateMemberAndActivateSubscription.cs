﻿using System;
using System.Xml.Serialization;

namespace Sebrae.Academico.BP.DTO.Services.EF
{
    [Serializable, XmlRoot("CreateMemberAndActivateSubscriptionResult")]
    public class DTOCreateMemberAndActivateSubscription : DTOCreateMember
    {
        public int SubscriptionId { get; set; }
    }
}