using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP
{
    public class ManterStatusMatricula : BusinessProcessBase
    {
        private readonly BMStatusMatricula _bmStatusMatricula;

        public ManterStatusMatricula()
            : base()
        {
            _bmStatusMatricula = new BMStatusMatricula();
        }
        

        public StatusMatricula ObterStatusMatriculaPorID(int pId)
        {
            return _bmStatusMatricula.ObterPorId(pId);
        }

        public IList<StatusMatricula> ObterTodosStatusMatricula()
        {
            return _bmStatusMatricula.ObterTodos();
        }

        public IList<StatusMatricula> ObterTodosEspecificos()
        {
            return _bmStatusMatricula.ObterTodosEspecificos();
        }

        public IList<StatusMatricula> ObterTodosIncluindoEspecificos()
        {
            return _bmStatusMatricula.ObterTodosIncluindoEspecificos();
        }

        /// <summary>
        /// Em resumo: buscar status "utilizados" em questionários.
        /// Não resumindo: Obter, de forma bem dispendiosa, status que são utilizados em MatriculaTurma e
        /// UsuarioTrilha, cujos ID_Turma e ID_TrilhaNivel existam em QuestionarioParticipacao.
        /// </summary>
        /// <returns></returns>
        public List<StatusMatricula> ObterTodosQuePossuemQuestionarios()
        {
            var questionariosTrilhaNivel =
                new BMQuestionarioParticipacao().ObterTodosQuestionariosComParticipacaoQueryble()
                    .Where(x => x.TrilhaNivel != null)
                    .Select(x => new QuestionarioParticipacao
                    {
                        Usuario =
                            new Usuario
                            {
                                ID = x.Usuario.ID
                            },
                        TrilhaNivel =
                            x.TrilhaNivel != null
                                ? new TrilhaNivel
                                {
                                    ID = x.TrilhaNivel.ID
                                }
                                : null
                    }).ToList();

            var questionariosTurma =
                new BMQuestionarioParticipacao().ObterTodosQuestionariosComParticipacaoQueryble()
                    .Where(x => x.Turma != null)
                    .Select(x => new QuestionarioParticipacao
                    {
                        Usuario =
                            new Usuario
                            {
                                ID = x.Usuario.ID
                            },
                        Turma =
                            x.Turma != null
                                ? new Turma
                                {
                                    ID = x.Turma.ID
                                }
                                : null,
                    }).ToList();

            var usuariosTrilha = new BMUsuarioTrilha().ObterTodosIQueryable()
                .Select(x =>
                    new UsuarioTrilha
                    {
                        ID = x.ID,
                        StatusMatricula = x.StatusMatricula,
                        Usuario = new Usuario
                        {
                            ID = x.Usuario.ID
                        },
                        TrilhaNivel = new TrilhaNivel
                        {
                            ID = x.TrilhaNivel.ID
                        }
                    }).ToList();

            var matriculasTurma = new BMMatriculaTurma().ObterTodosIQueryable().Select(x =>
                new MatriculaTurma
                {
                    ID = x.ID,
                    Turma =
                        new Turma
                        {
                            ID = x.Turma.ID
                        },
                    MatriculaOferta =
                        new MatriculaOferta
                        {
                            ID = x.MatriculaOferta.ID,
                            Usuario = new Usuario {ID = x.MatriculaOferta.Usuario.ID},
                            StatusMatricula = x.MatriculaOferta.StatusMatricula
                        }
                }).ToList();


            return
                _bmStatusMatricula.ObterTodosIncluindoEspecificos()
                    .Where(
                        status =>
                            matriculasTurma.Any(
                                mt =>
                                    (int) mt.MatriculaOferta.StatusMatricula == status.ID &&
                                    questionariosTurma.Any(
                                        qt =>
                                            qt.Usuario.ID == mt.MatriculaOferta.Usuario.ID && qt.Turma.ID == mt.Turma.ID)) ||
                            usuariosTrilha.Any(
                                ut =>
                                    (int) ut.StatusMatricula == status.ID &&
                                    questionariosTrilhaNivel.Any(
                                        qt => qt.Usuario.ID == ut.Usuario.ID && qt.TrilhaNivel.ID == ut.TrilhaNivel.ID))
                    )
                    .ToList();
        }

        public IList<StatusMatricula> ObterStatusMatriculaDeTrilhas()
        {
            return _bmStatusMatricula.ObterStatusMatriculaDeTrilhas();
        }

        public IList<StatusMatricula> ObterStatusMatriculaPorCategoriaConteudo(CategoriaConteudo categoriaConteudo)
        {
            return _bmStatusMatricula.ObterStatusMatriculaPorCategoriaConteudo(categoriaConteudo);
        }

        public IList<StatusMatricula> ObterStatusParaMatricula(bool somenteStatusParaMatricula = false, bool booFilaEspera = false)
        {
            // Remover status de cancelado/aluno, pois só pode ser alterado pelo próprio aluno, no portal.
            var listaStatus = _bmStatusMatricula.ObterTodosIQueryable().Where(x => x.ID != (int)enumStatusMatricula.CanceladoAluno);

            //demanda #3474
            if (!booFilaEspera)
            {
                if (!somenteStatusParaMatricula)
                {
                    listaStatus =
                        listaStatus.Where(
                            x =>
                                x.ID == (int)enumStatusMatricula.Inscrito ||
                                x.ID == (int)enumStatusMatricula.PendenteConfirmacaoAluno ||
                                x.ID == (int)enumStatusMatricula.Ouvinte);
                }
                else {
                    listaStatus = listaStatus.Where(x =>
                        x.ID != (int)enumStatusMatricula.CanceladoAdm &&
                        x.ID != (int)enumStatusMatricula.CanceladoGestor &&
                        x.ID != (int)enumStatusMatricula.FilaEspera
                        );
                }
            } else
            {
                //#860 -  Se a Oferta possuir Fila de espera e a Quantidade de matriculas já alcancou o limite exibe somente o status de inscrever com Fila de Espera
                listaStatus = listaStatus.Where(x => x.ID == (int)enumStatusMatricula.FilaEspera);
            }

            return listaStatus.ToList();
        }
    }
}
