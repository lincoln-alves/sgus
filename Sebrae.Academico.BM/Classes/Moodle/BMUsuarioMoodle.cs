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
    public class BMUsuarioMoodle : BusinessManagerBase, IDisposable
    {
        private RepositorioBaseMdl<UsuarioMoodle> repositorio;

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public BMUsuarioMoodle()
        {
            repositorio = new RepositorioBaseMdl<UsuarioMoodle>();
        }

        public IList<UsuarioMoodle> ObterTodos()
        {
            var query = repositorio.session.Query<UsuarioMoodle>();
            return query.ToList<UsuarioMoodle>();
        }

        public UsuarioMoodle ObterPorCPF(string cpf)
        {
            var query = repositorio.session.Query<UsuarioMoodle>();
            return query.FirstOrDefault(x => x.Usuario == cpf);
        }

        public bool ObterPorEmailOuUsuarioExistente(string usuario, string email)
        {
            return repositorio.session.Query<UsuarioMoodle>().Any(x => x.Email.Contains(email) || x.Usuario.Contains(usuario));
        }

        public IList<UsuarioMoodle> ObterPorFiltro(UsuarioMoodle usuarioMoodle)
        {
            var query = repositorio.session.Query<UsuarioMoodle>();

            if (!string.IsNullOrEmpty(usuarioMoodle.Email))
            {
                query = query.Where(x => x.Email.Contains(usuarioMoodle.Email));
            }
            if (!string.IsNullOrEmpty(usuarioMoodle.Usuario))
            {
                query = query.Where(x => x.Usuario.Contains(usuarioMoodle.Usuario));
            }

            return query.ToList();
        }

        public void Salvar(UsuarioMoodle usuarioMoodle)
        {
            repositorio.Salvar(usuarioMoodle);
        }
    }
}
