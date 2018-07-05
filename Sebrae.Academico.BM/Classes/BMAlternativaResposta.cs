using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;

namespace Sebrae.Academico.BM.Classes
{
    public class BMAlternativaResposta : BusinessManagerBase, IDisposable
    {
        #region Atributos

        private RepositorioBase<AlternativaResposta> repositorio;

        #endregion

        #region "Construtor"

        public BMAlternativaResposta()
        {
            repositorio = new RepositorioBase<AlternativaResposta>();
        }

        #endregion

        public IList<AlternativaResposta> ObterTodos()
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<AlternativaResposta>();
            return query.ToList<AlternativaResposta>();
        }

        public AlternativaResposta ObterPorId(int pId)
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<AlternativaResposta>();
            return query.FirstOrDefault(x => x.ID == pId);
        }

        public List<AlternativaResposta> ObterPorCampoRespostaId(int pId)
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<AlternativaResposta>();
            return query.Where(x => x.CampoResposta.ID == pId).ToList();
        }

        public List<int> ObterIdsPorCampoId(int pId)
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<AlternativaResposta>();
            return query.Where(x => x.CampoResposta.ID == pId).Select(d=> d.Alternativa.ID).ToList();
        }

        public IList<AlternativaResposta> ObterPorFiltro(AlternativaResposta modulo)
        {
            var query = repositorio.session.Query<AlternativaResposta>();

            //if (!string.IsNullOrEmpty(modulo.Nome))
            //    query = query.Where(x => x.Nome.Contains(modulo.Nome));

            //if (modulo.Capacitacao.ID > 0)
            //{
            //    query = query.Where(x => x.Capacitacao.ID == modulo.Capacitacao.ID);
            //}
            //else if (modulo.Capacitacao.ID == 0 && modulo.Capacitacao.Programa.ID > 0)
            //{
            //    query = query.Where(x => x.Capacitacao.Programa.ID == modulo.Capacitacao.Programa.ID);
            //}

            return query.ToList<AlternativaResposta>();
        }

        public void Salvar(AlternativaResposta model)
        {

            //Caso seja unico, descomentar a linha baixo e implementar
            //a verificacao por nome do programa.
            if (model.ID == 0)
            {
                if (this.ObterPorId(model.ID) != null)
                {
                    throw new AcademicoException("Já existe um registro.");
                }
            }

            repositorio.Salvar(model);

        }

        public void Salvar(List<AlternativaResposta> model)
        {

            foreach (var item in model)
            {
                if (item.ID == 0)
                {
                    if (ObterPorId(item.ID) != null)
                    {
                        throw new AcademicoException("Já existe um registro.");
                    }
                }

                repositorio.Salvar(item);
            }
        }

        public void Excluir(AlternativaResposta model)
        {
            repositorio.Excluir(model);
        }


        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

    }
}
