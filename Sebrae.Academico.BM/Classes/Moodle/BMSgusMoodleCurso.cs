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
    public class BMSgusMoodleCurso : BusinessManagerBase, IDisposable
    {
        private RepositorioBaseMdl<SgusMoodleCurso> repositorio;

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public BMSgusMoodleCurso()
        {
            repositorio = new RepositorioBaseMdl<SgusMoodleCurso>();
        }
        
        public SgusMoodleCurso ObterPorCategoria(int codCat)
        {
            var query = repositorio.session.Query<SgusMoodleCurso>();
            return query.FirstOrDefault(x => x.CodigoCategoria == codCat);
        }

        public SgusMoodleCurso ObterPorCodigoCurso(int codCurso)
        {
            var query = repositorio.session.Query<SgusMoodleCurso>();
            return query.FirstOrDefault(x => x.CodigoCurso == codCurso);
        }

        public void Cadastrar(SgusMoodleCurso dados)
        {
            repositorio.Salvar(dados);
        }
    }
}
