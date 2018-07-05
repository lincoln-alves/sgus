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
    public class BMCurso : BusinessManagerBase, IDisposable
    {
        private RepositorioBaseMdl<Curso> repositorio;

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public BMCurso()
        {
            repositorio = new RepositorioBaseMdl<Curso>();
        }

        public Curso ObterPorID(int id)
        {
            var query = repositorio.session.Query<Curso>();
            return query.FirstOrDefault(x => x.ID == id);
        }

        public IList<Curso> ObterPorCategoria(int categoriaId)
        {
            var query = repositorio.session.Query<Curso>();
            return query.Where(x => x.CodigoCategoria == categoriaId).ToList();
        }
    }
}
