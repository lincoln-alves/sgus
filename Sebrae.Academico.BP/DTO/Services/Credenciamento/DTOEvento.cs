using Newtonsoft.Json;
using System;

namespace Sebrae.Academico.BP.DTO.Services.Credenciamento
{
    public class DTOEvento
    {
        [JsonProperty("id_evento")]
        public virtual int ID { get; set; }

        [JsonProperty("titulo")]
        public virtual string Titulo { get; set; }

        [JsonProperty("data_inicio")]
        public virtual DateTime DataInicio { get; set; }

        [JsonProperty("data_fim")]
        public virtual DateTime DataFim { get; set; }

        [JsonProperty("Presencas")]
        public virtual int Presencas { get; set; }

        [JsonProperty("PresencasMinimas")]
        public virtual int? PresencasMinimas { get; set; }

        [JsonProperty("cpf")]
        public virtual string UsuarioCPF { get; set; }
    }
}
