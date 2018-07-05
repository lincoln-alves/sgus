using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;

namespace Sebrae.Academico.BM.Classes
{
    public class BMAlternativa : BusinessManagerBase, IDisposable
    {
        #region Atributos

        private RepositorioBase<Alternativa> repositorio;

        #endregion

        #region "Construtor"

        public BMAlternativa()
        {
            repositorio = new RepositorioBase<Alternativa>();
        }

        #endregion

        public IList<Alternativa> ObterTodos()
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<Alternativa>();
            return query.ToList<Alternativa>();
        }

        public Alternativa ObterPorId(int pId)
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<Alternativa>();
            return query.FirstOrDefault(x => x.ID == pId);
        }

        public IList<Alternativa> ObterPorCampoId(int idCampo)
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<Alternativa>();
            return query.Where(x => x.Campo.ID == idCampo).ToList();
        }

        public void Salvar(Alternativa model)
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

        public void Excluir(Alternativa model)
        {
            repositorio.Excluir(model);
        }

        private void ValidarModuloInformada(Alternativa model)
        {
            //throw new NotImplementedException();
        }


        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public void SalvarSemCommit(Alternativa alternativa)
        {
            repositorio.SalvarSemCommit(alternativa);
        }
    }
}
