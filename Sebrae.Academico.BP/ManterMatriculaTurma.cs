using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BP.DTO.Services.Credenciamento;

namespace Sebrae.Academico.BP
{
    public class ManterMatriculaTurma : BusinessProcessBase, IDisposable
    {
        #region "Atributos Privados"

        private BMMatriculaTurma bmMatriculaTurma = null;

        #endregion

        #region "Construtor"

        public ManterMatriculaTurma()
            : base()
        {
            bmMatriculaTurma = new BMMatriculaTurma();
        }

        #endregion

        #region "Métodos Privados"

        private void TransferirInformacoesDeUmaTurmaParaOutra(MatriculaTurma pTurmaAnterior, MatriculaTurma pNovaTurma)
        {
            pNovaTurma.Nota1 = pTurmaAnterior.Nota1;
            pNovaTurma.Nota2 = pTurmaAnterior.Nota2;
            pNovaTurma.MediaFinal = pTurmaAnterior.MediaFinal;
            //pNovaTurma.StatusMatricula = pTurmaAnterior.StatusMatricula;
            pNovaTurma.Turma = pTurmaAnterior.Turma;
            //pNovaTurma.Usuario = pTurmaAnterior.Usuario;
            pNovaTurma.DataLimite = pTurmaAnterior.DataLimite;
            pNovaTurma.DataMatricula = DateTime.Now;
        }

        #endregion

        #region "Métodos Públicos"

        public void IncluirMatriculaTurma(MatriculaTurma matriculaTurma)
        {
            try
            {
                matriculaTurma.DataMatricula = DateTime.Now;

                if (matriculaTurma.ID == 0)
                {

                    //Se if tipooferta == normal || exclusiva
                    var bmOferta = new BMOferta();
                    var oferta = bmOferta.ObterPorTurma(matriculaTurma.Turma.ID);

                    if (oferta != null)
                    {
                        matriculaTurma.DataLimite = matriculaTurma.CalcularDataLimite(oferta);
                    }

                    //Insere, ou seja, matricula o usuário na turma
                    bmMatriculaTurma.Salvar(matriculaTurma);
                }
                else
                {
                    //Atualiza
                    var turmaAnterior = bmMatriculaTurma.ObterMatriculaTurma(matriculaTurma.MatriculaOferta.Usuario.ID,
                        matriculaTurma.TurmaAnterior.ID);

                    if (turmaAnterior != null)
                    {
                        //Passa os dados de uma Turma para a nova Turma
                        //this.TransferirInformacoesDeUmaTurmaParaOutra(turmaAnterior, pMatriculaTurma);
                        turmaAnterior.Turma = new BMTurma().ObterPorID(matriculaTurma.Turma.ID);
                        bmMatriculaTurma.Salvar(matriculaTurma);
                    }
                }
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public MatriculaTurma ObterTurmaOndeOUsuarioEstaMatriculado(int idUsuario, int idOferta)
        {
            try
            {
                return bmMatriculaTurma.ObterTurmaOndeOUsuarioEstaMatriculado(idUsuario, idOferta);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public void AlterarMatriculaTurma(MatriculaTurma pMatriculaTurma)
        {
            try
            {
                bmMatriculaTurma.Salvar(pMatriculaTurma);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public MatriculaTurma ObterMatriculaTurmaPorId(int idMatriculaTurma)
        {
            try
            {
                return bmMatriculaTurma.ObterPorID(idMatriculaTurma);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public MatriculaTurma ObterMatriculaTurmaPorIdUsuarioIdTurma(int idUsuario, int idTurma)
        {
            try
            {
                return bmMatriculaTurma.ObterMatriculaTurma(idUsuario, idTurma);
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        public void Salvar(MatriculaTurma matriculaTurma)
        {
            bmMatriculaTurma.Salvar(matriculaTurma);
        }

        public void Salvar(IEnumerable<MatriculaTurma> matriculaTurma)
        {
            bmMatriculaTurma.Salvar(matriculaTurma);
        }

        public IQueryable<MatriculaTurma> ObterPorFiltro(MatriculaTurma matriculaTurma)
        {
            return bmMatriculaTurma.ObterPorFiltro(matriculaTurma);
        }

        public IList<MatriculaTurma> ObterMatriculasInscritas(int idUsuario)
        {
            var lista = bmMatriculaTurma.ObterTodosIQueryable()
                .Where(x => x.MatriculaOferta.StatusMatricula == enumStatusMatricula.Inscrito && x.MatriculaOferta.Usuario.ID == idUsuario)
                .ToList();
            return lista;
        }

        public IList<MatriculaTurma> ObterTurmasInscritasPorPeriodo(int idUsuario, DateTime dataInicio, DateTime dataFim)
        {
            return bmMatriculaTurma.ObterTodosIQueryable()
                .Where(x =>
                    x.MatriculaOferta.Usuario.ID == idUsuario && (
                    (dataInicio.Date >= x.DataMatricula.Date && x.DataMatricula.Date <= dataFim.Date) ||
                    (x.Turma.DataInicio.Date >= dataInicio.Date && x.Turma.DataInicio.Date <= dataFim.Date) ||
                    (x.MatriculaOferta.DataSolicitacao.Date >= dataInicio.Date && x.MatriculaOferta.DataSolicitacao.Date <= dataFim.Date)) &&
                    x.MatriculaOferta.StatusMatricula == enumStatusMatricula.Inscrito)
                .ToList();
        }

        public IList<MatriculaTurma> ObterMatriculasEncerrandoPorPeriodo(int idUsuario, DateTime dataInicio, DateTime dataFim)
        {
            return bmMatriculaTurma.ObterTodosIQueryable()
                .Where(x =>
                    x.DataLimite.Date >= dataInicio.Date &&
                    x.DataLimite.Date <= dataFim.Date &&
                    x.MatriculaOferta.StatusMatricula == enumStatusMatricula.Inscrito &&
                    x.MatriculaOferta.Usuario.ID == idUsuario)
                .ToList();
        }
        #endregion


        // Obter todos no intervalo determinado
        public IQueryable<MatriculaTurma> ObterPorIntervalo(int intervalo, enumStatusMatricula? statusMatricula = null)
        {
            DateTime inicio = DateTime.Now.AddMinutes(-intervalo);
            DateTime fim = DateTime.Now;

            var listaAlunosQuestionarioPos = bmMatriculaTurma.ObterTodosIQueryable()
                .Where(x => x.Turma.ListaQuestionarioAssociacao.Any(
                    y => y.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pos
                         &&
                         (
                             ((x.MatriculaOferta.Oferta.TipoOferta == enumTipoOferta.Exclusiva || x.MatriculaOferta.Oferta.TipoOferta == enumTipoOferta.Normal)
                              && y.DataDisparoLinkPesquisa.HasValue && y.DataDisparoLinkPesquisa >= inicio && y.DataDisparoLinkPesquisa <= fim)
                              ||
                             (x.MatriculaOferta.Oferta.TipoOferta == enumTipoOferta.Continua && x.DataTermino.HasValue && x.DataTermino >= inicio && x.DataTermino <= fim)
                         )
                    )
                            && (statusMatricula != null) ? x.MatriculaOferta.StatusMatricula == statusMatricula : x.MatriculaOferta.StatusMatricula != enumStatusMatricula.Inscrito
                            && x.MatriculaOferta.Usuario.Email != ""
                            && x.MatriculaOferta.Usuario.SID_Usuario != ""
                            && !x.Turma.ListaQuestionarioParticipacao.Any(qp =>
                                qp.Usuario.ID == x.MatriculaOferta.Usuario.ID &&
                                qp.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pos &&
                                qp.DataParticipacao.HasValue)
                );

            return listaAlunosQuestionarioPos;

        }

        public IQueryable<MatriculaTurma> ObterTodosIQueryable()
        {
            return bmMatriculaTurma.ObterTodosIQueryable();
        }

        private IQueryable<MatriculaTurma> ObterMatriculasTurmaCredenciamento()
        {
            return bmMatriculaTurma.ObterMatriculasTurmaCredenciamento();

        }

        public List<MatriculaTurma> ObterMatriculasComEventos(List<DTOEvento> presencas)
        {
            var matriculas = ObterMatriculasTurmaCredenciamento().ToList();
            var matriculasTurma = new List<MatriculaTurma>(); 

            foreach (var matricula in matriculas)
            {
                var oferta = new ManterOferta().ObterOfertaPorID(matricula.MatriculaOferta.Oferta.ID);

                // Filtrando matriculas que contem algum dos eventos vinculado
                var matriculasFiltradas = matriculas.Where(x => !x.MatriculaOferta.IsAprovado() && !x.MatriculaOferta.IsReprovado() &&
                    presencas.Any(evento => evento.ID == oferta.SolucaoEducacional.IDEvento && evento.UsuarioCPF == matricula.MatriculaOferta.Usuario.CPF)).ToList();

                matriculasTurma.AddRange(matriculasFiltradas);
            }

            return matriculasTurma;
        }

        public void Dispose()
        {
            bmMatriculaTurma.Dispose();
        }
    }
}
