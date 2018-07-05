using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;
using Sebrae.Academico.Dominio.Classes.Moodle;

namespace Sebrae.Academico.BM.Classes.Moodle
{
    public class BMUsuarioMoodleInscricao : BusinessManagerBase, IDisposable
    {
        private RepositorioBaseMdl<UsuarioMoodleInscricao> repositorio;

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public BMUsuarioMoodleInscricao()
        {
            repositorio = new RepositorioBaseMdl<UsuarioMoodleInscricao>();
        }

        public void Salvar(UsuarioMoodleInscricao pUsuarioMoodleInscricao)
        {
            repositorio.Salvar(pUsuarioMoodleInscricao);
        }

        public IList<UsuarioMoodleInscricao> ObterTodos()
        {
            return repositorio.ObterTodos();
        }
    }
}
