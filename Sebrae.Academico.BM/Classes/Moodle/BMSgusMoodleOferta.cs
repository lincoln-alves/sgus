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
    public class BMSgusMoodleOferta : BusinessManagerBase, IDisposable
    {
        private RepositorioBaseMdl<SgusMoodleOferta> repositorio;

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public BMSgusMoodleOferta()
        {
            repositorio = new RepositorioBaseMdl<SgusMoodleOferta>();
        }

        public SgusMoodleOferta ObterPorCodigoCurso(int codCurso)
        {
            var query = repositorio.session.Query<SgusMoodleOferta>();
            return query.FirstOrDefault(x => x.CodigoCurso == codCurso);
        }

        /// <summary>
        /// Obtem a última oferta ativa para o curso especificado
        /// </summary>
        /// <param name="codCurso"></param>
        /// <returns></returns>
        public SgusMoodleOferta ObterUltimaOfertaPorCodigoCurso(int codCurso)
        {
            var query = repositorio.session.Query<SgusMoodleOferta>();
            return query.OrderByDescending(x => x.ID).FirstOrDefault(x => x.CodigoCurso == codCurso);
        }

        public void Cadastrar(SgusMoodleOferta dados)
        {
            repositorio.Salvar(dados);
        }
    }
}
