using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;

namespace Sebrae.Academico.BM.Classes
{
    public class BMCampoResposta : BusinessManagerBase, IDisposable
    {
        #region Atributos

        private RepositorioBase<CampoResposta> repositorio;

        #endregion

        #region "Construtor"

        public BMCampoResposta()
        {
            repositorio = new RepositorioBase<CampoResposta>();
        }

        #endregion

        public IList<CampoResposta> ObterTodos()
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<CampoResposta>();
            return query.ToList<CampoResposta>();
        }

        public CampoResposta ObterPorId(int pId)
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<CampoResposta>();
            return query.FirstOrDefault(x => x.ID == pId);
        }

        public IList<CampoResposta> ObterPorFiltro(CampoResposta modulo)
        {
            var query = repositorio.session.Query<CampoResposta>();

            return query.ToList<CampoResposta>();
        }

        public CampoResposta ObterPorCampoID(int idCampo)
        {
            var query = repositorio.session.Query<CampoResposta>();

            query = query.Where(x => x.Campo.ID == idCampo);

            return query.FirstOrDefault();
        }

        public IEnumerable<CampoResposta> ObterRespostasPorCampo(int idCampo)
        {
            var query = repositorio.session.Query<CampoResposta>();

            return query.Where(x => x.Campo.ID == idCampo);
        }

        public IList<CampoResposta> ObterPorEtapaRespostaId(int idEtapaResposta)
        {
            return repositorio.session.Query<CampoResposta>()
                .Where(x => x.EtapaResposta.ID == idEtapaResposta)
                .ToList();
        }

        public CampoResposta ObterPorEtapaRespostaId(int IdEtapaResposta, int IdCampo)
        {
            var query = repositorio.session.Query<CampoResposta>();

            query = query.Where(x => x.EtapaResposta.ID == IdEtapaResposta && x.Campo.ID == IdCampo);

            return query.FirstOrDefault();
        }

        public CampoResposta ObterPorCampoProcessoResposta(int campo, int processoResposta)
        {
            var query = repositorio.session.Query<CampoResposta>();
            return query.Where(x => x.Campo.ID == campo && x.EtapaResposta.ProcessoResposta.ID == processoResposta).FirstOrDefault();
        }

        public void Salvar(CampoResposta model)
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

        public void Salvar(List<CampoResposta> model)
        {
            foreach (var item in model)
            {
                if (item.ID == 0)
                {
                    if (this.ObterPorId(item.ID) != null)
                    {
                        throw new AcademicoException("Já existe um registro.");
                    }
                }

                repositorio.Salvar(item);
            }
        }

        public void Excluir(CampoResposta model)
        {
            repositorio.Excluir(model);
        }

        private void ValidarModuloInformada(CampoResposta model)
        {
            //throw new NotImplementedException();
        }

        public IEnumerable<CampoResposta> ObterRespostas(Campo campo)
        {
            var query = repositorio.session.Query<CampoResposta>();

            return query.Where(x => x.Campo.ID == campo.ID);
        }

        public IQueryable<CampoResposta> ObterTodosIQueryable()
        {
            return repositorio.session.Query<CampoResposta>(); ;
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

    }
}
