using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;

namespace Sebrae.Academico.BM.Classes
{
    public class BMTurma : BusinessManagerBase, IDisposable
    {
        private RepositorioBase<Turma> repositorio;


        public BMTurma()
        {
            repositorio = new RepositorioBase<Turma>();

        }

        /// <summary>
        /// Verifica se já existe uma turma com o mesmo nome para uma mesma oferta.
        /// </summary>
        /// <param name="pTurma">Objeto oferta</param>
        private void VerificarExistenciaDeTurmaNaOferta(Turma pTurma)
        {

            Turma turma = this.ObterPorOfertaENome(pTurma);

            if (pTurma != null)
            {
                if (pTurma.ID != turma.ID)
                {
                    throw new AcademicoException(string.Format("A tag '{0}' já está cadastrado",
                                                 pTurma.Nome.Trim()));
                }
            }
        }

        private Turma ObterPorOfertaENome(Turma pTurma)
        {
            var query = repositorio.session.Query<Turma>();
            Turma turma = query.FirstOrDefault(x => x.Oferta != null && x.Oferta.ID == pTurma.Oferta.ID &&
                                                    x.Nome.Trim().ToUpper() == pTurma.Nome.Trim().ToUpper());

            return turma;
        }


        private void ValidarTurma(Turma pTurma)
        {

            if (string.IsNullOrWhiteSpace(pTurma.Nome)) throw new AcademicoException("Nome. Campo Obrigatório");

            if (pTurma.Oferta == null || (pTurma.Oferta != null && pTurma.Oferta.ID == 0)) throw new AcademicoException("Oferta. Campo Obrigatório");

            this.VerificarExistenciaDeTurmaNaOferta(pTurma);

        }

        public void Excluir(Turma pTurma)
        {

            var lista = pTurma.ListaQuestionarioParticipacao.ToList();
            if (this.ValidarDependencias(pTurma))
                throw new AcademicoException("Exclusão de registro negada. Existem Registros Dependentes desta Turma.");

            repositorio.Excluir(pTurma);
        }

        public IQueryable<Turma> ObterTodos()
        {
            return repositorio.session.Query<Turma>();
        }

        public Turma ObterPorID(int pId, bool carregarProfessores = true)
        {
            var turma = repositorio.ObterPorID(pId);
            
            //// Obtém a lista de professores, pois o mapeamento do NHibernate não está funcionamento adequadamente para trazer pelo Lazy.
            //if (carregarProfessores && turma != null)
            //    turma.ListaProfessores = new BMTurmaProfessor().ObterTurmaProfessorPorTurma(turma.ID).ToList();

            return turma;
        }

        public IList<Turma> ObterTurmasPorOferta(Oferta oferta)
        {
            var query = repositorio.session.Query<Turma>();
            query = query.Where(x => x.Oferta.ID == oferta.ID);
            return query.ToList<Turma>();
        }

        public IQueryable<Turma> ObterTurmasPorQuestionario(int idOferta, int? idProfessor)
        {
            var query = repositorio.session.Query<Turma>();

            query = query.Where(x => x.Oferta.ID == idOferta && x.ListaQuestionarioAssociacao.Any(
                qa =>
                    qa.TipoQuestionarioAssociacao.ID ==
                    (int)enumTipoQuestionarioAssociacao.Pos &&
                    qa.Questionario.TipoQuestionario.ID ==
                    (int)enumTipoQuestionario.Pesquisa));

            if (idProfessor.HasValue)
                query = query.Where(x => x.Professor.ID == idProfessor.Value);

            return query;
        }

        public Turma ObterUltimaTurmaPorChaveExterna(string idChaveExterna)
        {
            return repositorio.session
                .Query<Turma>()
                .OrderByDescending(x => x.ID)
                .FirstOrDefault(x => x.IDChaveExterna == idChaveExterna);
        }

        public IQueryable<Turma> ObterPorFiltro(string nome, string idChaveExterna, int idOferta, int idSolucaoEducacional)
        {
            var query = repositorio.session.Query<Turma>();

            if (!string.IsNullOrWhiteSpace(nome))
                query = query.Where(x => x.Nome.ToUpper().Contains(nome.ToUpper()));

            if (!string.IsNullOrWhiteSpace(idChaveExterna))
                query = query.Where(x => x.IDChaveExterna.ToUpper().Contains(idChaveExterna.ToUpper()));

            if (idOferta > 0)
                query = query.Where(x => x.Oferta.ID == idOferta);

            if (idSolucaoEducacional > 0)
                query = query.Where(x => x.Oferta.SolucaoEducacional.ID == idSolucaoEducacional);

            return query;
        }

        public IList<Turma> ObterPorFiltroNome(Turma turma)
        {
            var query = repositorio.session.Query<Turma>();
            if (turma != null)
            {

                if (!string.IsNullOrWhiteSpace(turma.Nome))
                    query = query.Where(x => x.Nome == turma.Nome);
            }

            return query.ToList();
        }

        public void Salvar(Turma turma, LogResponsavel logResponsavel = null, LogConsultorEducacional logConsultorEducacional = null)
        {
            ValidarTurmaInformada(turma);

            repositorio.LimparSessao();

            // Incluir log de Responsavel.
            if (logResponsavel != null)
                new BMLogResponsavel().Cadastrar(logResponsavel);

            // Incluir log de Consultor Educacional.
            if (logConsultorEducacional != null)
                new BMLogConsultorEducacional().Cadastrar(logConsultorEducacional);

            repositorio.Salvar(turma);
        }

        public int? ObterProximoCodigoSequencial(Oferta oferta)
        {
            if (oferta == null)
                return null;

            var max = repositorio.session.Query<Turma>()
                .Where(x => x.Oferta.ID == oferta.ID)
                .Max(x => x.Sequencia);

            if (max.HasValue)
                return max.Value + 1;

            return 1;
        }

        public bool AlterouOferta(int idTurma, Oferta novaOferta)
        {
            var turma = repositorio.session.Query<Turma>().First(s => s.ID == idTurma);

            if (turma.Oferta == null)
                return novaOferta != null;

            return novaOferta == null || turma.Oferta.ID != novaOferta.ID;
        }

        //private void ValidarTurmainformada(Turma pTurma)
        //{
        //    ValidarInstancia(pTurma);

        //    if (string.IsNullOrWhiteSpace(pTurma.Nome))
        //        throw new Exception("Nome não informado. Campo Obrigatório!");

        //    if (pTurma.Oferta == null)
        //        throw new Exception("Oferta não informada. Campo Obrigatório!");
        //}

        private void ValidarTurmaInformada(Turma pTurma)
        {

            ValidarDependencias(pTurma);

            if (string.IsNullOrWhiteSpace(pTurma.Nome)) throw new AcademicoException("Nome. Campo Obrigatório.");

            if (pTurma.Oferta.ID == 0) throw new AcademicoException("Oferta. Campo Obrigatório.");

        }

        public Turma ObterTurmaPorFornecedor(string loginFornecedor, string idChaveExternaTurma)
        {
            var query = repositorio.session.Query<Turma>();
            if (string.IsNullOrEmpty(idChaveExternaTurma))
                query = query.Where(x => x.IDChaveExterna == null);
            else
                query = query.Where(x => x.IDChaveExterna == idChaveExternaTurma);

            query = query.Where(x => x.Oferta.SolucaoEducacional.Fornecedor.Login == loginFornecedor);

            return query.FirstOrDefault();

        }

        public Turma ObterTurmaPorFornecedoreOferta(string loginFornecedor, string idChaveExternaTurma, string idChaveExternaOferta)
        {
            var query = repositorio.session.Query<Turma>();
            if (string.IsNullOrEmpty(idChaveExternaTurma))
                query = query.Where(x => x.IDChaveExterna == null);
            else
                query = query.Where(x => x.IDChaveExterna == idChaveExternaTurma);
            query = query.Where(x => x.Oferta.IDChaveExterna == idChaveExternaOferta);
            query = query.Where(x => x.Oferta.SolucaoEducacional.Fornecedor.Login == loginFornecedor);

            //MANTER O ORDER BY PARA PEGAR A MESMA PENDENCIA (CHAVEEXTERNA NULA) NA CONSULTA E NO MANTER
            return query.OrderBy(x => x.ID).FirstOrDefault();

        }

        public IList<MatriculaTurma> ObterMatriculaTurmaPorAluno(int pIdUsuario, int pIdOferta)
        {
            return repositorio.session.Query<MatriculaTurma>()
                    .Where(x => x.MatriculaOferta.Usuario.ID == pIdUsuario && x.Turma.Oferta.ID == pIdOferta).ToList();


        }

        public IList<Turma> ObterTurmasDoProfessor(int usuarioId)
        {
            return repositorio.session.Query<TurmaProfessor>().Where(t => t.Professor.ID == usuarioId).Select(t => t.Turma).Distinct().ToList();
        }


        public IQueryable<Turma> ObterTurmasFechadasDoProfessor(int usuarioId)
        {
            return
                repositorio.session.Query<TurmaProfessor>()
                    .Where(t => t.Professor.ID == usuarioId)
                    .Select(t => t.Turma)
                    .Where(x => (x.DataFinal.HasValue && x.DataFinal.Value < DateTime.Today))
                    .Distinct();
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        protected override bool ValidarDependencias(object pObject)
        {
            Turma pTurma = (Turma)pObject;
            if (pTurma.ListaMatriculas != null && pTurma.ListaMatriculas.Count > 0)
                return true;
            return false;
        }

        public void LimparSessao()
        {
            repositorio.LimparSessao();
        }

        public IQueryable<Turma> ObterTodosIQueryable()
        {
            return repositorio.session.Query<Turma>().AsQueryable();
        }
    }
}
