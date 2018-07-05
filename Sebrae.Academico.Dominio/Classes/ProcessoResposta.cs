using Sebrae.Academico.Dominio.Enumeracao;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sebrae.Academico.Dominio.Classes
{
    public class ProcessoResposta : EntidadeBasica
    {
        public virtual Processo Processo { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual bool Concluido { get; set; }
        public virtual enumStatusProcessoResposta Status { get; set; }
        public virtual string JustificativaCancelamento { get; set; }
        public virtual Usuario UsuarioCancelamento { get; set; }

        public virtual DateTime DataSolicitacao { get; set; }

        public virtual IList<EtapaResposta> ListaEtapaResposta { get; set; }
        //public virtual UsuarioCargo CargoDemandante { get; set; }
        public new virtual DateTime? DataAlteracao { get; set; }

        public virtual int? VersaoEtapa { get; set; }

        public ProcessoResposta()
        {
            Processo = new Processo();
            Usuario = new Usuario();
            ListaEtapaResposta = new List<EtapaResposta>();
        }

        /// <summary>
        /// Obtém o cargo do demandante quando fez a última participação no processo.
        /// </summary>
        /// <returns></returns>
        public virtual Cargo ObterUltimoCargoDemandante()
        {
            return
                ListaEtapaResposta.LastOrDefault(x => x.Analista != null && x.Analista.ID == Usuario.ID)?.CargoAnalista ??
                Usuario.ObterCargo()?.Cargo;
        }
    }
}