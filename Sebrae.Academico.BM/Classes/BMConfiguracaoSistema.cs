using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMConfiguracaoSistema : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<ConfiguracaoSistema> repositorio;

        public BMConfiguracaoSistema()
        {
            repositorio = new RepositorioBase<ConfiguracaoSistema>();
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public void Salvar(ConfiguracaoSistema configuracaoSistema)
        {
            repositorio.Salvar(configuracaoSistema);
        }

        public void Salvar(IList<ConfiguracaoSistema> listaConfiguracaoSistema)
        {
            repositorio.Salvar(listaConfiguracaoSistema);
        }

        public IList<ConfiguracaoSistema> ObterTodos()
        {
            return repositorio.ObterTodos();
        }

        public ConfiguracaoSistema ObterPorID(int pId)
        {

            ConfiguracaoSistema configuracaoSistema = null;
            var query = repositorio.session.Query<ConfiguracaoSistema>();
            configuracaoSistema = query.FirstOrDefault(x => x.ID == pId);
            return configuracaoSistema;
        }

    }
}