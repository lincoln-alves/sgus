using Sebrae.Academico.Dominio.Enumeracao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.Dominio.Classes
{
    public class Processo : EntidadeBasica
    {
        public virtual bool Ativo { get; set; }
        public virtual bool? Status { get; set; }
        public virtual IList<ProcessoResposta> ListaProcessoResposta { get; set; }
        public virtual IList<Etapa> ListaEtapas { get; set; }
        public virtual enumTipoProcesso Tipo { get; set; }
        public virtual bool?  Mensal { get; set; }
        public virtual int? DiaInicio { get; set; }
        public virtual int? DiaFim { get; set; } 
        public virtual Uf Uf { get; set; }

        public Processo()
        {
            ListaProcessoResposta = new List<ProcessoResposta>();
            ListaEtapas = new List<Etapa>();
            Ativo = true;
            Status = null;
            Mensal = false;
        }
    }
}
