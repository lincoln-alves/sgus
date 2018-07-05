using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Views
{
    public class BMViewUsuarioItemTrilhaParticipacao : BusinessManagerBase
    {
        private RepositorioBase<ViewUsuarioItemTrilhaParticipacao> repositorio;

        public BMViewUsuarioItemTrilhaParticipacao()
        {
            repositorio = new RepositorioBase<ViewUsuarioItemTrilhaParticipacao>();
        }

        public IList<ViewUsuarioItemTrilhaParticipacao> ObterViewUsuarioItemTrilhaParticipacaoPorFiltro(ViewUsuarioItemTrilhaParticipacao pFiltro)
        {
            var query = repositorio.session.Query<ViewUsuarioItemTrilhaParticipacao>();

            if (pFiltro.TrilhaOrigem != null)
            {
                query = query.Where(x => x.TrilhaOrigem.ID == pFiltro.TrilhaOrigem.ID);
            }

            if (pFiltro.TrilhaNivelOrigem != null)
            {
                query = query.Where(x => x.TrilhaNivelOrigem.ID == pFiltro.TrilhaNivelOrigem.ID);
            }

            if (pFiltro.TopicoTematico != null)
            {
                query = query.Where(x => x.TopicoTematico.ID == pFiltro.TopicoTematico.ID);
            }

            if (pFiltro.UsuarioOrigem != null)
            {
                query = query.Where(x => x.UsuarioOrigem.ID == pFiltro.UsuarioOrigem.ID);
            }

            if (pFiltro.ItemTrilha != null)
            {
                query = query.Where(x => x.ItemTrilha.ID == pFiltro.ItemTrilha.ID);
            }

            query = query.Fetch(x => x.TrilhaOrigem);
            query = query.Fetch(x => x.TrilhaNivelOrigem);
            query = query.Fetch(x => x.TopicoTematico);
            query = query.Fetch(x => x.ItemTrilha);
            query = query.Fetch(x => x.UsuarioOrigem);
                     
            return query.ToList<ViewUsuarioItemTrilhaParticipacao>();

        }
    }
}
