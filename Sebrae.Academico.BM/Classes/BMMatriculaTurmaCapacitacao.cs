using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMMatriculaTurmaCapacitacao : BusinessManagerBase 
    {
        private RepositorioBase<MatriculaTurmaCapacitacao> repositorio;

        public BMMatriculaTurmaCapacitacao()
        {
            repositorio = new RepositorioBase<MatriculaTurmaCapacitacao>();
        }

        public IList<MatriculaTurmaCapacitacao> ObterPorMatriculaCapacitacao(int idMatriculaCapacitacao)
        {
            var query = repositorio.session.Query<MatriculaTurmaCapacitacao>();
            return query.Where(x=>x.MatriculaCapacitacao.ID == idMatriculaCapacitacao).ToList<MatriculaTurmaCapacitacao>();
        }

        public void Salvar(MatriculaTurmaCapacitacao mt)
        {
            repositorio.Salvar(mt);
        }


        public IList<MatriculaTurmaCapacitacao> ObterPorTurmaCapacitacao(TurmaCapacitacao turmaCapacitacao)
        {
            var query = repositorio.session.Query<MatriculaTurmaCapacitacao>();
            if (turmaCapacitacao != null) {
                query = query.Where(x => x.TurmaCapacitacao == turmaCapacitacao);
            }

            return query.ToList<MatriculaTurmaCapacitacao>();
        }


        public void Excluir(MatriculaTurmaCapacitacao pMatriculaTurmaCapacitacao)
        {
            repositorio.Excluir(pMatriculaTurmaCapacitacao);
        }

        public void ExcluirLista(IList<MatriculaTurmaCapacitacao> listMatTurCap) {

            foreach (MatriculaTurmaCapacitacao mat in listMatTurCap)
            {
                this.Excluir(mat);
            }
        }

    }
}
