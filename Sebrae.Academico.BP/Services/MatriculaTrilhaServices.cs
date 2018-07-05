using System;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Seguranca.Autenticacao;
using Sebrae.Academico.Util.Classes;
using System.Collections.Generic;
using Sebrae.Academico.BP.DTO.Services;

namespace Sebrae.Academico.BP.Services
{
    public class MatriculaTrilhaServices : BusinessProcessServicesBase
    {
        private BMTrilha trilhaBM;
        private BMTrilhaNivel trilhaNivelBM;
        private BMUsuarioTrilha usuarioTrilhaBM;

        [Obsolete("Utilizar o método TrilhaServices.InscreverUsuarioTrilhaNivel para inscrição em um nível de trilha.")]
        public Trilha RegistrarMatriculatrilha(int pID_Usuario, int pID_TrilhaNivel, AuthenticationRequest autenticacao)
        {

            Usuario usuario = new BMUsuario().ObterPorId(pID_Usuario);
            TrilhaNivel trilhaNivel = new BMTrilhaNivel().ObterPorID(pID_TrilhaNivel);

            if (ExisteNiveltrilhaPendente(trilhaNivel, usuario))
            {
                throw new Exception("Nível da Trilha bloqueado.");
            }

            UsuarioTrilha userTrilha = new UsuarioTrilha()
            {
                DataInicio = DateTime.Now,
                DataLimite = DateTime.Now.AddDays(trilhaNivel.QuantidadeDiasPrazo).Date,
                NivelOcupacional = usuario.NivelOcupacional,
                Uf = usuario.UF,
                TrilhaNivel = trilhaNivel,
                Usuario = usuario,
                StatusMatricula = enumStatusMatricula.Inscrito,
                Auditoria = new Auditoria(autenticacao.Login),
                AcessoBloqueado = false

            };


            if (ValidarMatriculaAtivaExistente(userTrilha))
                throw new AcademicoException("Usuário já Matriculado na trilha.");

            usuarioTrilhaBM = new BMUsuarioTrilha();
            usuarioTrilhaBM.Salvar(userTrilha);
            try
            {
                // Enviar Email
                new ManterUsuarioTrilha().EnviarEmailBoasVindas(userTrilha);
            }
            catch
            {
                //TODO: CRIAR LOG DE ERROS NO SISTEMA (29/03/2016)
            }
            return trilhaNivel.Trilha;
        }

        public void CancelarMatriculaTrilha(int idUsuariotrilha, AuthenticationRequest autenticacao)
        {
            var manterUsuarioTrilha = new ManterUsuarioTrilha();

            var usuarioTrilha = manterUsuarioTrilha.ObterPorId(idUsuariotrilha);

            // Caso esteja inscrito ou não aprovado na prova pode ser cancelada a sua matrícula
            if (usuarioTrilha != null &&
                (usuarioTrilha.StatusMatricula == enumStatusMatricula.Inscrito ||
                 usuarioTrilha.StatusMatricula == enumStatusMatricula.Reprovado))
            {
                DateTime limiteCancelamentoUsuario = usuarioTrilha.TrilhaNivel.LimiteCancelamento > 0
                                   ? usuarioTrilha.DataInicio.AddDays(usuarioTrilha.TrilhaNivel.LimiteCancelamento)
                                   : usuarioTrilha.DataInicio.AddDays(usuarioTrilha.TrilhaNivel.QuantidadeDiasPrazo);

                if (DateTime.Now > limiteCancelamentoUsuario)
                {
                    //Passou do Limite para cancelamento
                    throw new AcademicoException("O prazo de cancelamento expirou");
                }

                // Atribuindo status a matrícula do usuário
                usuarioTrilha.DataFim = DateTime.Now;
                usuarioTrilha.DataAlteracaoStatus = DateTime.Now;
                usuarioTrilha.StatusMatricula = enumStatusMatricula.CanceladoAluno;
                manterUsuarioTrilha.Salvar(usuarioTrilha);
            }
            else
            {
                throw new AcademicoException("Não foi encontrada nenhuma matrícula com cancelamento permitido");
            }
        }


        /// <summary>
        /// Método utilizado para exclusão de matrículas em trilhas.
        /// </summary>
        /// <param name="idUsuariotrilha"></param>
        /// <param name="autenticacao"></param>
        public void CancelarMatriculaTrilhaExcluindo(int idUsuariotrilha, AuthenticationRequest autenticacao)
        {
            var bmUserTrilha = new BMUsuarioTrilha();

            var usuarioTrilha = bmUserTrilha.ObterPorId(idUsuariotrilha);

            // Caso esteja inscrito ou não aprovado na prova pode ser cancelada a sua matrícula
            if (usuarioTrilha != null &&
                (usuarioTrilha.StatusMatricula == enumStatusMatricula.Inscrito ||
                 usuarioTrilha.StatusMatricula == enumStatusMatricula.Reprovado))
            {
                DateTime limiteCancelamentoUsuario = usuarioTrilha.TrilhaNivel.LimiteCancelamento > 0
                                   ? usuarioTrilha.DataInicio.AddDays(usuarioTrilha.TrilhaNivel.LimiteCancelamento)
                                   : usuarioTrilha.DataInicio.AddDays(usuarioTrilha.TrilhaNivel.QuantidadeDiasPrazo);

                if (DateTime.Now > limiteCancelamentoUsuario)
                {
                    //Passou do Limite para cancelamento
                    throw new AcademicoException("O prazo de cancelamento expirou");
                }

                BMQuestionarioParticipacao questPart = new BMQuestionarioParticipacao();
                questPart.ExcluiParticipacoes(questPart.ObterPorUsuarioTrilhaNivel(usuarioTrilha.Usuario,
                    usuarioTrilha.TrilhaNivel));

                // Exclui a matrícula do usuário no banco
                bmUserTrilha.ExcluirSemValidacao(usuarioTrilha);
            }
            else
            {
                throw new AcademicoException("Não foi encontrada nenhuma matrícula com cancelamento permitido");
            }
        }

        private bool ValidarMatriculaAtivaExistente(UsuarioTrilha userTrilha)
        {
            bool result = new BMUsuarioTrilha().ObterPorFiltro(new UsuarioTrilha()
            {
                TrilhaNivel = new BMTrilhaNivel().ObterPorID(userTrilha.TrilhaNivel.ID),
                Usuario = new BMUsuario().ObterPorId(userTrilha.Usuario.ID)
            })
                                          .Any(x => x.StatusMatricula != enumStatusMatricula.CanceladoAdm &&
                                                      x.StatusMatricula != enumStatusMatricula.CanceladoAluno);

            return result;
        }

        public bool ExisteNiveltrilhaPendente(TrilhaNivel nivelTrilha, Usuario usuario)
        {
            if (nivelTrilha == null) return true;

            var trilha = nivelTrilha.Trilha;
            var index = trilha.ListaTrilhaNivel.OrderBy(p => p.ValorOrdem).ToList().IndexOf(nivelTrilha);

            if (index <= 0) return false;

            return !usuarioTrilhaBM.ObterPorFiltro(new UsuarioTrilha
            {
                Usuario = usuario,
                TrilhaNivel = trilha.ListaTrilhaNivel[index - 1]
            }).Any(x => x.StatusMatricula == enumStatusMatricula.Aprovado);
        }

        public DTOConsultaDisponibilidadeMatriculaNivelTrilha ConsultaDisponibilidadeMatriculaNivelTrilha(AuthenticationRequest autenticacao, int idNivelTrilha)
        {
            var retorno = new DTOConsultaDisponibilidadeMatriculaNivelTrilha();

            Usuario usuario = new BMUsuario().ObterPorCPF(autenticacao.Login);
            usuarioTrilhaBM = new BMUsuarioTrilha();
            TrilhaNivel nivelTrilha = new BMTrilhaNivel().ObterPorID(idNivelTrilha);

            if (nivelTrilha != null)
            {
                if (ExisteNiveltrilhaPendente(nivelTrilha, usuario))
                {
                    retorno.CodigoDisponibilidade = (int)enumDisponibilidadeSolucaoEducacional.PossuiPreReqPrograma;
                    retorno.TextoDisponibilidade = nivelTrilha.PreRequisito != null ? string.Format("Nível da Trilha bloqueado. Você deve realizar o nível: {0}", nivelTrilha.PreRequisito.Nome) :
                        "Nível da Trilha bloqueado.";
                    return retorno;
                }
            }

            // Verifica se o usuário está inscrito na trilha
            UsuarioTrilha usuarioTrilha = new UsuarioTrilha();
            usuarioTrilha.Usuario = usuario;
            usuarioTrilha.TrilhaNivel = nivelTrilha;

            var usuariosTrilha = usuarioTrilhaBM.ObterPorFiltro(usuarioTrilha);

            if (usuariosTrilha.Any())
            {
                // Caso já tenha alguma matrícula ativa retorna o erro
                if (usuariosTrilha.Any(x => x.StatusMatricula == enumStatusMatricula.Inscrito))
                {
                    retorno.CodigoDisponibilidade = (int)enumDisponibilidadeSolucaoEducacional.Matriculado;
                    retorno.TextoDisponibilidade = "O aluno já está matriculado nesse Nível da Trilha";
                }
                // Status simples pois atualmente os outros status de aprovação não são utilizados em trilhas
                else if (usuariosTrilha.Any(x => x.StatusMatricula == enumStatusMatricula.Aprovado))
                {
                    retorno.CodigoDisponibilidade = (int)enumDisponibilidadeSolucaoEducacional.Cursado;
                    retorno.TextoDisponibilidade = "O aluno já realizou esse nível da trilha";
                }
            }

            // Se não foram encontrados problemas libera a matricula
            if (retorno.CodigoDisponibilidade == 0)
            {
                retorno.CodigoDisponibilidade = (int)enumDisponibilidadeSolucaoEducacional.EfetuarMatricula;
                retorno.TextoDisponibilidade = "";
            }

            // Somente pega o Termo se puder fazer a matrícula
            if (retorno.CodigoDisponibilidade == (int)enumDisponibilidadeSolucaoEducacional.EfetuarMatricula)
            {
                // Pegando o termo de aceite do curso
                TrilhaNivel trilhaNivel = new BMTrilhaNivel().ObterPorID(idNivelTrilha);

                if (trilhaNivel != null)
                {
                    retorno.TermoDeAceite = trilhaNivel.TextoTermoAceite;
                }
            }

            return retorno;
        }
    }
}
