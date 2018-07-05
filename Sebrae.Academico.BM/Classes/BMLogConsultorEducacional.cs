using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMLogConsultorEducacional : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<LogConsultorEducacional> repositorio;

        public BMLogConsultorEducacional()
        {
            repositorio = new RepositorioBase<LogConsultorEducacional>();
        }

        public void Cadastrar(LogConsultorEducacional logConsultorEducacional)
        {
            repositorio.Salvar(logConsultorEducacional);
        }

        public IEnumerable<LogConsultorEducacional> ObterPorTurma(Turma turma)
        {
            return repositorio.ObterTodos().Where(x => x.Turma.ID == turma.ID);
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }
    }
}
