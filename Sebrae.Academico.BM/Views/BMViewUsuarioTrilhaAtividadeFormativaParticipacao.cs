using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Views
{
    public class BMViewUsuarioTrilhaAtividadeFormativaParticipacao : BusinessManagerBase
    {
        private RepositorioBase<ViewUsuarioTrilhaAtividadeFormativaParticipacao> repositorio;

        public BMViewUsuarioTrilhaAtividadeFormativaParticipacao()
        {
            repositorio = new RepositorioBase<ViewUsuarioTrilhaAtividadeFormativaParticipacao>();
        }

        public IList<Trilha> ObterTrilhas()
        {
            ICriteria criteria = repositorio.session.CreateCriteria(typeof(ViewUsuarioTrilhaAtividadeFormativaParticipacao), "vw");
            ProjectionList projList = Projections.ProjectionList();
            projList.Add(Projections.Distinct(Projections.Property<ViewUsuarioTrilhaAtividadeFormativaParticipacao>(d => d.TrilhaOrigem)));
            criteria.SetProjection(projList);
            return criteria.List<Trilha>();
        }

        public IList<TrilhaTopicoTematico> ObterTopicosTematicos(Trilha trilha, TrilhaNivel trilhaNivel)
        {
            ICriteria criteria = repositorio.session.CreateCriteria(typeof(ViewUsuarioTrilhaAtividadeFormativaParticipacao), "vw");
            criteria = criteria.Add(Expression.Eq("TrilhaOrigem", trilha));
            criteria = criteria.Add(Expression.Eq("TrilhaNivelOrigem", trilhaNivel));

            ProjectionList projList = Projections.ProjectionList();
            projList.Add(Projections.Distinct(Projections.Property<ViewUsuarioTrilhaAtividadeFormativaParticipacao>(d => d.TopicoTematico)));
            criteria.SetProjection(projList);
            return criteria.List<TrilhaTopicoTematico>();
        }

        public IList<TrilhaNivel> ObterTrilhasNivelPorTrilha(Trilha trilha)
        {
            ICriteria criteria = repositorio.session.CreateCriteria(typeof(ViewUsuarioTrilhaAtividadeFormativaParticipacao), "vw");
            criteria = criteria.Add(Expression.Eq("TrilhaOrigem", trilha));

            ProjectionList projList = Projections.ProjectionList();
            projList.Add(Projections.Distinct(Projections.Property<ViewUsuarioTrilhaAtividadeFormativaParticipacao>(d => d.TrilhaNivelOrigem)));
            criteria.SetProjection(projList);
            return criteria.List<TrilhaNivel>();
        }

        public IList<ViewUsuarioTrilhaAtividadeFormativaParticipacao> ObterViewUsuarioTrilhaAtividadeFormativaParticipacaoPorFiltro(ViewUsuarioTrilhaAtividadeFormativaParticipacao pFiltro)
        {
            var query = repositorio.session.Query<ViewUsuarioTrilhaAtividadeFormativaParticipacao>();

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
                        
            query = query.Fetch(x => x.TrilhaOrigem);
            query = query.Fetch(x => x.TrilhaNivelOrigem);
            query = query.Fetch(x => x.TopicoTematico);
            query = query.Fetch(x => x.UsuarioOrigem);

            return query.ToList<ViewUsuarioTrilhaAtividadeFormativaParticipacao>();

        }
    }

}