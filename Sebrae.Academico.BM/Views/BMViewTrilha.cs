using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using PowerfulExtensions.Linq;
using System;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
namespace Sebrae.Academico.BM.Views
{

    public class BMViewTrilha : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<ViewTrilha> repositorio;

        public BMViewTrilha()
        {
            repositorio = new RepositorioBase<ViewTrilha>();
        }

        public IList<Trilha> ObterTrilhas()
        {
            ICriteria criteria = repositorio.session.CreateCriteria(typeof(ViewTrilha), "vw");
            ProjectionList projList = Projections.ProjectionList();
            projList.Add(Projections.Distinct(Projections.Property<ViewTrilha>(d => d.TrilhaOrigem)));
            criteria.SetProjection(projList);
            return criteria.List<Trilha>();
        }

        public IList<ViewTrilha> ObterObjetivosDaTrilha(TrilhaNivel trilhaNivel)
        {

            if (trilhaNivel == null)
            {
                throw new AcademicoException("Nível da Trilha. Campo Obrigatório");
            }

            var query = repositorio.session.Query<ViewTrilha>();
            IList<ViewTrilha> ListaViewTrilha = query.Where(x => x.TrilhaNivelOrigem.ID == trilhaNivel.ID &&  x.UsuarioOrigem == null)
                                                     .Distinct(x => x.TopicoTematico.NomeExibicao, x => x.Objetivo).ToList();
            return ListaViewTrilha;
        }

        public IList<TrilhaNivel> ObterInformacoesCompletasDasTrilhas()
        {
            var query = repositorio.session.Query<ViewTrilha>();
            IList<TrilhaNivel> ListaTrilhaNivel = query.Distinct(x => x.TrilhaOrigem.Nome, x => x.TrilhaNivelOrigem.Nome, x => x.TrilhaNivelOrigem.ID)
                 .Select(x => x.TrilhaNivelOrigem).ToList();

            return ListaTrilhaNivel;


        }

        public IList<TrilhaNivel> ObterTrilhasNivelPorTrilha(Trilha trilha)
        {
            ICriteria criteria = repositorio.session.CreateCriteria(typeof(ViewTrilha), "vw");
            criteria = criteria.Add(Expression.Eq("TrilhaOrigem", trilha));

            ProjectionList projList = Projections.ProjectionList();
            projList.Add(Projections.Distinct(Projections.Property<ViewTrilha>(d => d.TrilhaNivelOrigem)));
            criteria.SetProjection(projList);
            return criteria.List<TrilhaNivel>();
        }


        public IList<ViewTrilha> ObterViewTrilhaPorFiltro(ViewTrilha pFiltro)
        {
            //TODOMAR: Ordenar resultado
            var query = repositorio.session.Query<ViewTrilha>();
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
            query = query.Fetch(x => x.ItemTrilha);
            query = query.Fetch(x => x.UsuarioOrigem);

            return query.AsParallel().ToList<ViewTrilha>();

        }

        public IList<TrilhaTopicoTematico> ObterTopicosTematicos(Trilha trilha, TrilhaNivel trilhaNivel)
        {
            ICriteria criteria = repositorio.session.CreateCriteria(typeof(ViewTrilha), "vw");
            criteria = criteria.Add(Expression.Eq("TrilhaOrigem", trilha));
            criteria = criteria.Add(Expression.Eq("TrilhaNivelOrigem", trilhaNivel));

            ProjectionList projList = Projections.ProjectionList();
            projList.Add(Projections.Distinct(Projections.Property<ViewTrilha>(d => d.TopicoTematico)));
            criteria.SetProjection(projList);
            return criteria.List<TrilhaTopicoTematico>();
        }

        public IList<ItemTrilha> ObterItemsTrilha(ViewTrilha viewTrilha)
        {

            var query = repositorio.session.Query<ViewUsuarioItemTrilhaParticipacao>();
            //query = query.Fetch(x => x.ItemTrilha);

            if (viewTrilha.TrilhaOrigem != null)
                query = query.Where(x => x.TrilhaOrigem.ID == viewTrilha.TrilhaOrigem.ID);

            if (viewTrilha.TrilhaNivelOrigem != null)
                query = query.Where(x => x.TrilhaNivelOrigem.ID == viewTrilha.TrilhaNivelOrigem.ID);

            if (viewTrilha.TopicoTematico != null)
                query = query.Where(x => x.TopicoTematico.ID == viewTrilha.TopicoTematico.ID);

            if (viewTrilha != null && viewTrilha.UsuarioOrigem != null)
                query = query.Where(x => x.UsuarioOrigem.ID == viewTrilha.UsuarioOrigem.ID);


            IList<ItemTrilha> ListaItemTrilha = new List<ItemTrilha>();

            using (BMItemTrilha itemTrilhaBM = new BMItemTrilha())
            {

                foreach (ViewUsuarioItemTrilhaParticipacao vt in query.ToList())
                {
                    ListaItemTrilha.Add(itemTrilhaBM.ObterPorID(vt.ItemTrilha.ID));
                }
            }

            return ListaItemTrilha;

        }



        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }
    }
}
