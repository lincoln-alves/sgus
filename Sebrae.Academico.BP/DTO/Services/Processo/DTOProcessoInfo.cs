using Sebrae.Academico.BP.DTO.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Services.Processo
{
    public class DTOProcessoInfo
    {
        public DTOProcessoInfo()
        {
            Situacao = new DTOSituacaoProcesso();
        }

        public virtual int ID { get; set; }

        public virtual string Nome { get; set; }

        public virtual DTOSituacaoProcesso Situacao { get; set; }

        public virtual List<DTOCargosDemandante> Cargos { get; set; }

        public virtual bool Concluido { get; set; }

        public virtual int EtapaAtual { get; set; }

        public virtual bool AnalisePeloUsuarioLogado { get; set; }

        public virtual string Demandante { get; set; }

        public virtual string DemandanteEmail { get; set; }

        public virtual DateTime DataSolicitacao { get; set; }

        public virtual List<DTOEtapaPermissaoNucleo> AnalistasPorNucleo { get; set; }
        
        public virtual int ProximaEtapa { get; set; }
    }
}
