using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMLogResponsavel : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<LogResponsavel> repositorio;

        public BMLogResponsavel()
        {
            repositorio = new RepositorioBase<LogResponsavel>();
        }

        public void Cadastrar(LogResponsavel logResponsavel)
        {
            repositorio.Salvar(logResponsavel);
        }

        public IEnumerable<LogResponsavel> ObterPorTurma(Turma turma)
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
