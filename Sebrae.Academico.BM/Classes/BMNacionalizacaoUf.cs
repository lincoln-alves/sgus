using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Classes
{
    public class BMNacionalizacaoUf : BusinessManagerBase, IDisposable
    {

        #region Atributos

        private RepositorioBase<NacionalizacaoUf> repositorio;

        #endregion

        #region "Construtor"


        public BMNacionalizacaoUf()
        {
            repositorio = new RepositorioBase<NacionalizacaoUf>();
        }

        public void Salvar(NacionalizacaoUf nacUf)
        {
            repositorio.Salvar(nacUf);
        }

        #endregion

        public void Excluir(NacionalizacaoUf nacUf)
        {
            repositorio.Excluir(nacUf);
        }

        public IQueryable<NacionalizacaoUf> ObterTodos()
        {
            return repositorio.ObterTodosIQueryable();
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

    }
}
