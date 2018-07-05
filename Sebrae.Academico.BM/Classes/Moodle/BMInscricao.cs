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
    public class BMInscricao : BusinessManagerBase, IDisposable
    {
        private RepositorioBaseMdl<Inscricao> repositorio;

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public BMInscricao()
        {
            repositorio = new RepositorioBaseMdl<Inscricao>();
        }

        public IList<Inscricao> ObterPorFiltro(Inscricao pInscricao)
        {
            var query = repositorio.session.Query<Inscricao>();

            if (!string.IsNullOrEmpty(pInscricao.TipoInscricao))
                query = query.Where(x => x.TipoInscricao.Contains(pInscricao.TipoInscricao));

            if (pInscricao.IDCurso > 0)
                query = query.Where(x => x.IDCurso == pInscricao.IDCurso);

            return query.ToList<Inscricao>();
        }

        // Suspende ou remove a suspensão de uma matricula de um aluno no Moodle
        public void alterarInscricao(int courseId, string userCPF, int status)
        {
            var query = repositorio.session.QueryOver<UsuarioMoodle>();

            UsuarioMoodle usuarioMoodle = query.Where(x => x.Usuario == userCPF).List().FirstOrDefault();

            //usuarioMoodle.ListaMoodleInscricoes.ForEach(l => l.
            UsuarioMoodleInscricao usuarioMoodleInscricao = usuarioMoodle != null ? usuarioMoodle.ListaMoodleInscricoes.FirstOrDefault(x => x.Inscricao.IDCurso == courseId) : null;

            if (usuarioMoodleInscricao != null)
            {
                usuarioMoodleInscricao.Status = status;
                BMUsuarioMoodleInscricao bmUsuarioInscricao = new BMUsuarioMoodleInscricao();
                bmUsuarioInscricao.Salvar(usuarioMoodleInscricao);
            }
        }
    }
}
