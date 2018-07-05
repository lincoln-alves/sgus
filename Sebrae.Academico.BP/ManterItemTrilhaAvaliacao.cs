using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;
using System.Collections;
using System.Linq;

namespace Sebrae.Academico.BP
{
    public class ManterItemTrilhaAvaliacao : RepositorioBase<ItemTrilhaAvaliacao>, IDisposable
    {
        public void Salvar(ItemTrilhaAvaliacao itemTrilhaAvaliacao)
        {
            itemTrilhaAvaliacao.DataAlteracao = DateTime.Now;
            base.Salvar(itemTrilhaAvaliacao);
        }

        public IQueryable<ItemTrilhaAvaliacao> ObterPorNivel(int idNivel)
        {
            return ObterTodosIQueryable().Where(x => x.ItemTrilha.Missao.PontoSebrae.TrilhaNivel.ID == idNivel);
        }

        public IQueryable<ItemTrilhaAvaliacao> ObterPorPontoSebrae(int idPontoSebrae)
        {
            return ObterTodosIQueryable().Where(x => x.ItemTrilha.Missao.PontoSebrae.ID == idPontoSebrae);
        }
    }
}
