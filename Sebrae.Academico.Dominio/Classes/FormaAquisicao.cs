using System.Collections.Generic;
using System;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class FormaAquisicao : EntidadeBasica
    {
        public virtual IList<ItemTrilha> ListaItemTrilha { get; set; }
        public virtual IList<SolucaoEducacional> ListaSolucaoEducacional { get; set; }
        public virtual string Imagem { get; set; }
        public virtual string Descricao { get; set; }
        public virtual enumTipoFormaAquisicao TipoFormaDeAquisicao { get; set; }
        public virtual bool EnviarPortal { get; set; }
        public virtual int? CargaHoraria { get; set; }
        
        /// <summary>
        /// Usuário que cadastrou a demanda no sistema
        /// </summary>
        public virtual Usuario Responsavel { get; set; }

        public virtual bool Presencial { get; set; }
        public virtual Uf Uf { get; set; }
        public virtual bool? PermiteAlterarCargaHoraria { get; set; }

        public virtual string UFResponsavel { get; set; }

        public virtual int GetValorFormaDeAquisicao()
        {
            return (int)this.TipoFormaDeAquisicao;
        }

        public virtual string EnviarPortalFormatado
        {
            get { return this.EnviarPortal ? "Sim" : "Não"; }
        }

        public virtual string CargaHorariaFormatada
        {
            get { return this.CargaHoraria.HasValue ? this.CargaHoraria.Value.ToString() : ""; }
        }
    }

}
