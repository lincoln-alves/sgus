using System;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.BP
{
    public class ManterTurmaProfessor : BusinessProcessBase, IDisposable
    {
        private BMTurmaProfessor bmTurmaProfessor = null;

        public ManterTurmaProfessor()
            : base()
        {
            bmTurmaProfessor = new BMTurmaProfessor();
        }

        public IQueryable<TurmaProfessor> ObterTurmaProfessorPorTurma(int idTurma)
        {
            return bmTurmaProfessor.ObterTurmaProfessorPorTurma(idTurma);
        }

        public IQueryable<TurmaProfessor> ObterTurmaProfessorPorProfessor(int idProfessor)
        {
            return bmTurmaProfessor.ObterTurmaProfessorPorProfessor(idProfessor);
        }

        public IQueryable<TurmaProfessor> ObterTodos()
        {
            return bmTurmaProfessor.ObterTodos();
        }

        public void Dispose()
        {
            bmTurmaProfessor.Dispose();
            GC.Collect();
        }
    }
}
