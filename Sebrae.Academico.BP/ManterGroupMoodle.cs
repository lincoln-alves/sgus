using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes.Moodle;

namespace Sebrae.Academico.BP
{
    public class ManterGroupMoodle : BusinessProcessBase
    {
        private BMGroupMoodle bmGroup = null;

        public ManterGroupMoodle()
        {
            bmGroup = new BMGroupMoodle();
        }
      
        public IQueryable<GroupMoodle> ObterTodos()
        {
            return bmGroup.ObterTodosQueryble();
        }
    }
}
