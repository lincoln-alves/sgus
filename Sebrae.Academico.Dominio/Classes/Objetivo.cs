using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.Dominio.Classes
{
    public class Objetivo : EntidadeBasica
    {
        public Objetivo()
        {
            ListaItemTrilha = new List<ItemTrilha>();
        }

        public virtual IList<ItemTrilha> ListaItemTrilha { get; set; }
        public virtual string NomeExibicao { get; set; }
        public virtual string ChaveExterna { get; set; }

        /// <summary>
        /// Obter a quantidade de itens trilha por forma de aquisição.
        /// </summary>
        /// <param name="formaAquisicaoId">ID do grupo da forma de aquisição</param>
        /// <returns></returns>
        public virtual int ContarItensPorGrupoFormaAquisicao(int formaAquisicaoId)
        {
            return ListaItemTrilha.AsEnumerable().Count(it => it.FormaAquisicao.ID == formaAquisicaoId);
        }

        public virtual IEnumerable<FormaAquisicao> ObterFormasAquisicao()
        {
            return ListaItemTrilha.Select(it => it.FormaAquisicao).Distinct();
        }
    }
}