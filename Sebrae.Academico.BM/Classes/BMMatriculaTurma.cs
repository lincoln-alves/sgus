using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMMatriculaTurma : BusinessManagerBase //, IDisposable
    {
        #region "Atributos Privados"

        private RepositorioBase<MatriculaTurma> repositorio;

        #endregion

        #region "Construtor"

        public BMMatriculaTurma()
        {
            repositorio = new RepositorioBase<MatriculaTurma>();
        }

        #endregion

        #region "Métodos Públicos"

        public IList<MatriculaTurma> ObterMatriculasDataExpiracao(int pDiasExpiracao)
        {
            var query = repositorio.session.Query<MatriculaTurma>();

            query = query.Where(x => x.MatriculaOferta.StatusMatricula == enumStatusMatricula.Inscrito ||
                                      x.MatriculaOferta.StatusMatricula == enumStatusMatricula.PendenteConfirmacaoAluno);


            return query.ToList().Where(x => DateTime.Now.Date.AddDays((pDiasExpiracao - 1) * -1).Equals(x.DataLimite.Date)).ToList<MatriculaTurma>();

        }

        public IQueryable<MatriculaTurma> ObterTodosIQueryable()
        {
            return repositorio.session.Query<MatriculaTurma>();
        }

        public IQueryable<MatriculaTurma> ObterPorFiltro(MatriculaTurma matriculaTurma)
        {
            var query = repositorio.session.Query<MatriculaTurma>();

            if (matriculaTurma.DataLimite != DateTime.MinValue)
                query = query.Where(x => x.DataLimite.AddDays(1).Date == matriculaTurma.DataLimite.Date);

            if (matriculaTurma.MatriculaOferta.Usuario != null)
                query = query.Where(x => x.MatriculaOferta.Usuario.ID == matriculaTurma.MatriculaOferta.Usuario.ID);

            if (matriculaTurma.Turma != null)
                query = query.Where(x => x.Turma.ID == matriculaTurma.Turma.ID);

            if (matriculaTurma.MatriculaOferta != null)
                query = query.Where(x => x.MatriculaOferta.ID == matriculaTurma.MatriculaOferta.ID);

            return query;
        }

        public IList<MatriculaTurma> ObterVencidos()
        {
            var query = repositorio.session.Query<MatriculaTurma>();
            query = query.Where(x => x.DataLimite.Date <= DateTime.Now.Date);
            query = query.Where(x => x.MatriculaOferta.StatusMatricula == enumStatusMatricula.Inscrito);

            return query.ToList();
        }

        public IQueryable<MatriculaTurma> ObterPorUsuario(int IdUsuario)
        {
            var query = repositorio.session.Query<MatriculaTurma>();
            query = query.Where(x => x.MatriculaOferta.Usuario.ID == IdUsuario);
            return query;
        }

        public IQueryable<MatriculaTurma> ObterPorTurma(Turma turma)
        {
            var query = repositorio.session.Query<MatriculaTurma>();
            return query.Where(x => x.Turma.ID == turma.ID);
        }

        public void Salvar(MatriculaTurma mt)
        {
            repositorio.Salvar(mt);
        }

        public void Salvar(IEnumerable<MatriculaTurma> mt)
        {
            repositorio.Salvar(mt.ToList());
        }

        public void Atualizar(MatriculaTurma mt)
        {
            repositorio.Salvar(mt);
        }

        public void SalvarSemCommit(MatriculaTurma mt)
        {
            repositorio.SalvarSemCommit(mt);
        }

        public MatriculaTurma ObterTurmaOndeOUsuarioEstaMatriculado(int idUsuario, int idOferta)
        {
            var query = repositorio.session.Query<MatriculaTurma>();
            MatriculaTurma matriculaTurma = query.FirstOrDefault(x => x.MatriculaOferta.Usuario.ID == idUsuario && x.Turma.Oferta.ID == idOferta);
            return matriculaTurma;
        }

        public MatriculaTurma ObterMatriculaTurma(int idUsuario, int idTurma)
        {
            var query = repositorio.session.Query<MatriculaTurma>();
            MatriculaTurma matriculaTurma = query.FirstOrDefault(x => x.MatriculaOferta.Usuario.ID == idUsuario && x.Turma.ID == idTurma);
            return matriculaTurma;
        }


        public MatriculaTurma ObterPorID(int pIdMatriculaTurma)
        {
            return repositorio.ObterPorID(pIdMatriculaTurma);
        }

        public bool MatriculaPertenceStatus(int idUsuario, int idTurma, List<int> idsStatusMatricula)
        {
            var matricula = ObterMatriculaTurma(idUsuario, idTurma);

            MatriculaOferta matriculaOferta;

            return matricula != null && (matriculaOferta = matricula.MatriculaOferta) != null &&
                   idsStatusMatricula.Contains((int)matriculaOferta.StatusMatricula);
        }

        public void Dispose()
        {
            repositorio.Dispose();
        }

        public IQueryable<MatriculaTurma> ObterMatriculasTurmaCredenciamento()
        {
            return repositorio.session.Query<MatriculaTurma>().Where(x => x.MatriculaOferta.Oferta.SolucaoEducacional.IDEvento != null);
        }

        #endregion
    }
}
