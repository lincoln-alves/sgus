using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
namespace Sebrae.Academico.BM.Classes
{
    public class BMTurmaCapacitacao : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<TurmaCapacitacao> repositorio;


        public BMTurmaCapacitacao()
        {
            repositorio = new RepositorioBase<TurmaCapacitacao>();
        }

        public IList<TurmaCapacitacao> ObterPorCapacitacao(int idCapacitacao)
        {
            var query = repositorio.session.Query<TurmaCapacitacao>();
            return query.Where(x => x.Capacitacao.ID == idCapacitacao).ToList<TurmaCapacitacao>();
        }

        public IQueryable<TurmaCapacitacao> ObterPorPrograma(int idPrograma) {
            var query = repositorio.session.Query<TurmaCapacitacao>();
            return query.Where(x => x.Capacitacao.Programa.ID == idPrograma);
        }

        public TurmaCapacitacao ObterPorId(int idTurmaCapacitacao)
        {
            var query = repositorio.session.Query<TurmaCapacitacao>();
            return query.FirstOrDefault(x => x.ID == idTurmaCapacitacao);
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }


        public IList<TurmaCapacitacao> ObterPorFiltro(TurmaCapacitacao turmaCapacitacao)
        {
            var query = repositorio.session.Query<TurmaCapacitacao>();

            if (!string.IsNullOrWhiteSpace(turmaCapacitacao.Nome))
                query = query.Where(x => x.Nome.ToUpper().Contains(turmaCapacitacao.Nome.ToUpper()));

            if (turmaCapacitacao.ID > 0)
                query = query.Where(x => x.ID == turmaCapacitacao.ID);

            if (turmaCapacitacao.Capacitacao.ID > 0)
                query = query.Where(x => x.Capacitacao.ID == turmaCapacitacao.Capacitacao.ID);

            return query.ToList<TurmaCapacitacao>();
        }

        public void Excluir(TurmaCapacitacao turmaCapacitacao)
        {
            IList<MatriculaTurmaCapacitacao> matTurmaCapacitacao = new BMMatriculaTurmaCapacitacao().ObterPorTurmaCapacitacao(turmaCapacitacao);
            if (matTurmaCapacitacao.Count() > 0)
                throw new AcademicoException("Não foi possível realizar a exclusão. Existem matrículas vinculadas a essa turma.");
                
                repositorio.Excluir(turmaCapacitacao);
        }

        public void Salvar(TurmaCapacitacao turmaCapacitacao)
        {
            //demanda #3587
            var obter = ObterPorFiltro(new TurmaCapacitacao { Nome = turmaCapacitacao.Nome }).FirstOrDefault(p => p.ID != turmaCapacitacao.ID);
            if (obter != null){
                throw new AcademicoException("Já existe no banco de dados um registro com esse nome.");
            }
            //fim demanda #3587

            repositorio.Salvar(turmaCapacitacao);
        }
    }
}
