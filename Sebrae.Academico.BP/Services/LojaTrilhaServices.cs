using System;
using System.Linq;
using Sebrae.Academico.BP.Services.SgusWebService;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.BP.Auth;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.BP.Services
{
    public partial class TrilhaServices
    {
        public dynamic ConsultarDisponibilidadeMatriculaSolucaoEducacional(UserIdentity usuarioLogado, int idItemTrilha)
        {
            try
            {
                var itemTrilha = new ManterItemTrilha().ObterItemTrilhaPorID(idItemTrilha);

                if (itemTrilha.SolucaoEducacionalAtividade == null)
                    throw new ResponseException(enumResponseStatusCode.SolucaoEducacionalNaoEncontrada,
                        enumResponseStatusCode.SolucaoEducacionalNaoEncontrada.GetDescription());

                var disponibilidade =
                    new ConsultarStatusMatriculaSolucaoEducacional().ConsultarDisponibilidadeMatriculaSolucaoEducacional(
                        usuarioLogado.Usuario.ID, itemTrilha.SolucaoEducacionalAtividade.ID);

                var se = itemTrilha.SolucaoEducacionalAtividade;

                return new
                {
                    IdSolucaoEducacional = se.ID,
                    IdOferta = disponibilidade.IdOferta,
                    IdTurma = disponibilidade.IdTurma,
                    NomeSolucao = se.Nome,
                    Prazo = disponibilidade.Prazo,
                    CargaHoraria = disponibilidade.CargaHoraria,
                    DataInicioInscricoes = disponibilidade.DataInicioInscricoes,
                    DataFimInscricoes = disponibilidade.DataFimInscricoes,
                    Descricao = se.Apresentacao,
                    TermoDeAceite = disponibilidade.TextoTermoAceite,
                    PoliticaDeConsequencia = disponibilidade.TextoPoliticaConsequencia,
                    InformacaoAdicional = disponibilidade.TextoInformacaoAdicional,
                    TextoDisponibilidade = disponibilidade.TextoDisponibilidade,
                    CodigoDisponibilidade = disponibilidade.CodigoDisponibilidade,
                    OfertasDisponiveis = disponibilidade.OfertasDisponiveis
                };
            }
            catch
            {
                throw new ResponseException(enumResponseStatusCode.RegistroNaoEncontrado, "Não foi possível consultar a disponibilidade");
            }
        }

        public object ObterAcessoTutorial(UsuarioTrilha usuarioTrilha, int categoria_id)
        {
            var trilhaTutorialAcesso = new ManterTrilhaTutorial().ObterTrilhaTutorialAcessoPorCategoria(usuarioTrilha, (enumCategoriaTrilhaTutorial)categoria_id);

            if (trilhaTutorialAcesso != null)
            {
                return new { status = 1 };
            }
            else
            {
                return new { status = 0 };
            }

        }

        public dynamic MatriculaTurma(UserIdentity usuarioLogado, int idTurma, int itemTrilhaId)
        {
            var turma = new ManterTurma().ObterTurmaPorID(idTurma);
            if (turma == null)
                throw new ResponseException(enumResponseStatusCode.TurmaNaoEncontrada, "Turma não encontrada");

            var itemTrilha = new ManterItemTrilha().ObterItemTrilhaPorID(itemTrilhaId);
            if (itemTrilha == null)
                throw new ResponseException(enumResponseStatusCode.TurmaNaoEncontrada, "Solução não encontrada");

            try
            {
                new ManterSolucaoEducacionalService().MatricularTurma(usuarioLogado.Usuario.ID, turma.ID, null, null,
                    usuarioLogado.Usuario.CPF, itemTrilha);
            }
            catch (AcademicoException ex)
            {
                throw new ResponseException(enumResponseStatusCode.ErroRegraNegocioSgus, ex.Message);
            }
           catch
            {
                throw new ResponseException(enumResponseStatusCode.ErroRegraNegocioSgus,
                    "Não foi possível realizar a matrícula na turma");
            }

            return null;
        }

        public dynamic MatriculaSolucaoEducacional(UserIdentity usuarioLogado, int idSolucaoEducacional, int ofertaId, int itemTrilhaId)
        {
            var se = new ManterSolucaoEducacional().ObterSolucaoEducacionalPorId(idSolucaoEducacional);
            if (se == null)
            {
                throw new ResponseException(enumResponseStatusCode.SolucaoEducacionalNaoEncontrada, "Solução Educacional não encontrada");
            }

            var oferta = se.ListaOferta.Where(x => x.ID == ofertaId).FirstOrDefault(); 
            if (oferta == null)
            {
                throw new ResponseException(enumResponseStatusCode.OfertaNaoEncontrada, "Oferta não encontrada");
            }

            var itemTrilha = new ManterItemTrilha().ObterItemTrilhaPorID(itemTrilhaId);
            if (itemTrilha == null)
            {
                throw new ResponseException(enumResponseStatusCode.TurmaNaoEncontrada, "Solução não encontrada");
            }

            try
            {
                new ManterSolucaoEducacionalService().MatricularSolucaoEducacional(usuarioLogado.Usuario.ID, se.ID,
                    oferta.ID, null,
                    null, usuarioLogado.Usuario.CPF, null, itemTrilha);
            }
            catch (AcademicoException ex)
            {
                throw new ResponseException(enumResponseStatusCode.ErroRegraNegocioSgus, ex.Message);
            }
            catch 
            {
                throw new ResponseException(enumResponseStatusCode.ErroRegraNegocioSgus,
                    "Não foi possível realizar a matrícula na turma");
            }

            return true;
        }

        public dynamic ObterAcessoJogo(int solucaoId, UsuarioTrilha matricula)
        {
            var solucaoSebrae = new ManterItemTrilha().ObterItemTrilhaPorID(solucaoId);

            if (solucaoSebrae == null)
                throw new ResponseException(enumResponseStatusCode.SolucaoSebraeNaoEncontrada);

            var ultimaParticipacao =
                solucaoSebrae.ListaItemTrilhaParticipacao.LastOrDefault(
                    x => x.UsuarioTrilha.ID == matricula.ID && x.Autorizado == null);

            // Criar participação sem autorização, se não existir.
            if (ultimaParticipacao == null)
            {
                // Criar uma participação que servirá de "Dummy" para relacionar o aluno com a aprovação do jogo.
                var participacao = new ItemTrilhaParticipacao
                {
                    UsuarioTrilha = matricula,
                    ItemTrilha = solucaoSebrae,
                    DataEnvio = DateTime.Now,
                    TipoParticipacao = enumTipoParticipacaoTrilha.Jogo,
                    Auditoria = new Auditoria(matricula.Usuario.CPF),
                    FileServer = null
                };

                // Criar participação.
                new BP.ManterItemTrilhaParticipacao().Salvar(participacao);
            }

            return new
            {
                LinkJogo = ConsultarLinkJogo(solucaoSebrae, matricula)
            };
        }
    }
}