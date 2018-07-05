using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMTurmaProfessor : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<TurmaProfessor> repositorio;


        public BMTurmaProfessor()
        {
            repositorio = new RepositorioBase<TurmaProfessor>();

        }

        public void Salvar(TurmaProfessor pTurmaProfessor)
        {
            if (pTurmaProfessor.Professor == null)
                throw new AcademicoException("Não foi possível salvar, não encontrado o objeto Professor.");
            if (pTurmaProfessor.Turma == null)
                throw new AcademicoException("Não foi possível salvar, não encontrado o objeto Turma.");

            repositorio.Salvar(pTurmaProfessor);
        }


        public void Excluir(TurmaProfessor pTurmaProfessor)
        {
            if (this.ValidarDependencias(pTurmaProfessor))
                throw new AcademicoException("Exclusão de registro negada. Existem Registros Dependentes desta Turma.");

            repositorio.Excluir(pTurmaProfessor);
        }

        public IQueryable<TurmaProfessor> ObterTodos()
        {
            return repositorio.session.Query<TurmaProfessor>();
        }

        public TurmaProfessor ObterPorID(int pId)
        {
            return repositorio.ObterPorID(pId);
        }

        public IList<TurmaProfessor> ObterPorFiltro(TurmaProfessor turmaProfessor)
        {
            var query = repositorio.session.Query<TurmaProfessor>();
            if (turmaProfessor != null)
            {
                if (turmaProfessor.ID > 0)
                    query = query.Where(x => x.ID == turmaProfessor.ID);

                if (turmaProfessor.Turma != null && turmaProfessor.Turma.ID > 0)
                    query = query.Where(x => x.Turma.ID == turmaProfessor.Turma.ID);

                if (turmaProfessor.Professor != null && turmaProfessor.Professor.ID > 0)
                    query = query.Where(x => x.Professor.ID == turmaProfessor.Professor.ID);
            }

            return query.ToList<TurmaProfessor>();
        }

        public IQueryable<TurmaProfessor> ObterTurmaProfessorPorTurma(int idTurma)
        {
            return repositorio.session.Query<TurmaProfessor>().Where(x => x.Turma.ID == idTurma);
        }

        public IQueryable<TurmaProfessor> ObterTurmaProfessorPorProfessor(int idProfessor)
        {
            return repositorio.session.Query<TurmaProfessor>().Where(x => x.Professor.ID == idProfessor);
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }
    }
}
