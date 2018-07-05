using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.BP.DTO.Dominio;

namespace Sebrae.Academico.BP.DTO.Services.Processo
{
    public class DTOProcessoResposta
    {
        public virtual int ID { get; set; }
        public virtual int ID_Processo { get; set; }
        public string NomeProcesso { get; set; }
        public virtual DTOUsuario Usuario { get; set; }
        public virtual DTOEtapa EtapaAtual { get; set; }
        public virtual int ID_ProcessoReposta { get; set; }
        public virtual bool LiberarCancelamento { get; set; }
        public virtual DTOSituacaoProcesso Situacao { get; set; }

        // Campo relacionado a reprovação do usuário        
        public virtual List<DTOEtapa> ListaEtapasReprovadas { get; set; }
        public virtual bool DeveReiniciarProcesso { get; set; }

        public virtual List<DTOEtapa> ListaEtapas { get; set; }
        public virtual DateTime? DataUltimaAtualizacao { get; set; }
        public virtual DateTime? DataSolicitacao { get; set; }

        public DTOProcessoResposta() {
            ListaEtapas = new List<DTOEtapa>();
            ListaEtapasReprovadas = new List<DTOEtapa>();
            DeveReiniciarProcesso = false;
        }
    }
}
