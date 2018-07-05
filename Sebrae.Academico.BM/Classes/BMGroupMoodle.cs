using Sebrae.Academico.Dominio.Classes.Moodle;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.BM.Classes
{
    public class BMGroupMoodle  : BusinessManagerBase, IDisposable
    {
        private RepositorioBaseMdl<GroupMoodle> repositorio;

        public BMGroupMoodle()
        {
            repositorio = new RepositorioBaseMdl<GroupMoodle>();
        }

        public void Dispose()
        {
            repositorio.Dispose();
        }

        public IQueryable<GroupMoodle> ObterTodosQueryble()
        {
            return repositorio.ObterTodosQueryble();
        }
       
    }
}
