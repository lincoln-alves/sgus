using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMMetaIndividual: BusinessManagerBase, IDisposable
    {
        #region "Atributos Privados"

        private RepositorioBase<MetaIndividual> repositorio;

        #endregion

        #region "Construtor"
        
        public BMMetaIndividual()
        {
            repositorio = new RepositorioBase<MetaIndividual>();
        }

        #endregion

        public IList<MetaIndividual> ObterTodos()
        {
            return repositorio.ObterTodos();
        }

        public MetaIndividual ObterPorID(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        public void Salvar(MetaIndividual pMetaIndividual)
        {
            ValidarMetaIndividualInformada(pMetaIndividual);
            repositorio.Salvar(pMetaIndividual);
        }

        public IList<MetaIndividual> ObterPorFiltro(MetaIndividual pMetaIndividual)
        {
            var query = repositorio.session.Query<MetaIndividual>();


            if (!string.IsNullOrWhiteSpace(pMetaIndividual.Nome))
                query = query.Where(x => x.Nome.ToLower().Contains(pMetaIndividual.Nome.ToLower()));

            if (pMetaIndividual.DataValidade != new DateTime(1, 1, 1))
                query = query.Where(x => x.DataValidade.Equals(pMetaIndividual.DataValidade));

            return query.ToList();
        }

        public IList<MetaIndividual> ObterPorDataValidade(string pNome, Usuario pIdUsuario, DateTime? pDataValidadeInicial, DateTime? pDataValidadeFinal)
        {
            var query = repositorio.session.Query<MetaIndividual>();

            if (!string.IsNullOrWhiteSpace(pNome))
                query = query.Where(x => x.Nome.ToLower().Contains(pNome.ToLower()));

            if (pDataValidadeInicial != new DateTime(1,1,1))
                query = query.Where(x => x.DataValidade >= pDataValidadeInicial);

            if (pDataValidadeFinal != new DateTime(1, 1, 1))
                query = query.Where(x => x.DataValidade <= pDataValidadeFinal);

            if (pIdUsuario != null)
                query = query.Where(x => x.Usuario.ID == pIdUsuario.ID);

            return query.ToList();
        }

        private void ValidarMetaIndividualInformada(MetaIndividual pMetaIndividual)
        {

            this.ValidarInstancia(pMetaIndividual);

            if (string.IsNullOrWhiteSpace(pMetaIndividual.Nome))
                throw new Exception("Nome não informado. Campo Obrigatório!");

            if (pMetaIndividual.DataValidade.Date == new DateTime(1,1,1).Date)
                throw new Exception("Data de Validade não informada. Campo Obrigatório!");

            if (pMetaIndividual.Usuario == null)
                throw new Exception("Usuário não informado. Campo Obrigatório!");

        }

        public void Excluir(MetaIndividual pMetaIndividual)
        {
            repositorio.Excluir(repositorio.ObterPorID(pMetaIndividual.ID));
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }
    }
}
