using Sebrae.Academico.BM.Classes.Moodle.Views;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Classes.Moodle;
using Sebrae.Academico.Dominio.Classes.Views;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BM.Classes.Moodle
{
    public class BMLogMoodle : BusinessManagerBase, IDisposable
    {
        private RepositorioBaseMdl<LogMoodle> repositorio;

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public BMLogMoodle()
        {
            repositorio = new RepositorioBaseMdl<LogMoodle>();
        }

        public IList<LogMoodle> ObterTodos()
        {
            return repositorio.ObterTodos();
        }

        /// <summary>
        /// Retorna a quantidade de acessos que a solução educacional teve no moodle.
        /// </summary>
        /// <returns></returns>
        public int ObterQuantidadeDeAcessos(int idOfertaMoodle, string CPF, IEnumerable<ViewLogMoodle> lista)
        {
            return lista.Count(v => v.Usuario == CPF && v.ID_Oferta == idOfertaMoodle);
        }
    }
}
