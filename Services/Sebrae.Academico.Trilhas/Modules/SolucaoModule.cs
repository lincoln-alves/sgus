using Nancy.ModelBinding;
using Nancy.Security;
using Sebrae.Academico.BP.Auth;
using Sebrae.Academico.BP.DTO.Services.Trilhas;
using Sebrae.Academico.BP.DTO.Services.Trilhas.Loja;
using Sebrae.Academico.BP.DTO.Services.Trilhas.Solucao;
using Sebrae.Academico.BP.Services;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System.Linq;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.DTO.Services.Trilhas.Mapa;
using Sebrae.Academico.BP.DTO.Services.Trilhas.ConheciGame;
using Sebrae.Academico.BP.DTO.Dominio;

namespace Sebrae.Academico.Trilhas.Modules
{
    public class SolucaoModule : GenericModule
    {
        public SolucaoModule() : base("solucao")
        {
            this.RequiresAuthentication();

            Get["/{itemTrilhaId:int}"] = p =>
            {
                // Retornar os dados da solução trilheiro.
                return new DtoResponse(new TrilhaServices().ObterDadosSolucaoTrilheiro(p.itemTrilhaId));
            };

            Get["/status/{solucaoSebraeId:int}"] = p =>
            {
                return new DtoResponse(new TrilhaServices().ObterStatusSolucaoSebrae(p.solucaoSebraeId, AcessoAtual));
            };

            Get["/Curtir/{itemTrilhaId:int}/{acao:int}"] = p =>
            {
                VerificarBloqueio();
                // Retornar Sucesso ou não da operação.
                return new DtoResponse(new TrilhaServices().DefinirCurtida(p.itemTrilhaId, (enumTipoCurtida)p.acao, AcessoAtual.Matricula));
            };

            Get["/Curtidas/{itemTrilhaId:int}/{soCurtidas:bool}"] = p =>
            {
                // Retornar lista ou não da operação.
                return new DtoResponse(new TrilhaServices().ListarCurtidas(p.itemTrilhaId, p.soCurtidas, AcessoAtual.Matricula));
            };

            // UC007 - Inserir Solucao Trilheiro
            Post["/solucaotrilheiro/new"] = paramentros =>
            {
                VerificarBloqueio();
                // Fazer o bind dos valores do POST para um objeto fortemente tipado.
                DTOSolucaoTrilheiro dtoSolucaoTrilheiro = this.Bind();

                return new DtoResponse(new TrilhaServices().CadastrarSolucaoTrilheiro(dtoSolucaoTrilheiro, AcessoAtual.Matricula, AcessoAtual.Nivel));
            };

            // UC007 - Carregar Campos select
            Get["/solucaotrilheiro/{lojaId:int}"] = p =>
            {
                VerificarBloqueio();

                var pontoSebrae = AcessoAtual.Nivel.ListaPontoSebrae.FirstOrDefault(x => x.ID == p.lojaId);

                // Se o usuário não estiver matriculado, não poderá exibir a trilha.
                if (pontoSebrae == null)
                    throw new ResponseException(enumResponseStatusCode.RegistroNaoEncontrado, "Loja não encontrada.");

                var trilhaServices = new TrilhaServices();

                // Retornar os dados da loja.
                var retorno = new DtoResponse(
                        trilhaServices.ObterDadosCadastroSolucaoTrilheiro(AcessoAtual.Nivel, pontoSebrae, AcessoAtual.Matricula),
                        trilhaServices.ObterMensagensGuiaSolucaoTrilheiro(AcessoAtual.Matricula));

                return retorno;

            };

            Get["/disponibilidade/{itemTrilhaId:int}"] = p =>
            {
                return new DtoResponse(new TrilhaServices().ConsultarDisponibilidadeMatriculaSolucaoEducacional(AcessoAtual, p.itemTrilhaId));
            };

            Post["/matricularturma/{itemTrilhaId:int}"] = p =>
            {
                VerificarBloqueio();
                var turma = this.Bind<DTOMatriculaTurma>().Turma;

                if (turma == null)
                    throw new ResponseException(enumResponseStatusCode.TurmaNaoEncontrada);

                // Realiza inscrição do usuário na turma
                return new DtoResponse(new TrilhaServices().MatriculaTurma(AcessoAtual, turma.Value, p.itemTrilhaId));
            };

            Post["/matricularsolucaoeducacional/{itemTrilhaId:int}"] = p =>
            {
                VerificarBloqueio();
                var dados = this.Bind<DTOMatriculaTurma>();

                if (dados.SolucaoEducacional == null)
                    throw new ResponseException(enumResponseStatusCode.SolucaoEducacionalNaoEncontrada);

                if (dados.Oferta == null)
                    throw new ResponseException(enumResponseStatusCode.OfertaNaoEncontrada);

                // Realiza inscrição do usuário na turma
                return new DtoResponse(new TrilhaServices().MatriculaSolucaoEducacional(AcessoAtual, dados.SolucaoEducacional.Value, dados.Oferta.Value, p.itemTrilhaId));
            };

            // UC006 - Solução Sebrae Atividade Dissertativa - PI07 a PI11
            Post["/participacao/new"] = parametro =>
            {
                VerificarBloqueio();
                // Fazer o bind dos valores do POST para um objeto fortemente tipado.
                DTOParticipacao dtoParticipacao = this.Bind();

                return new DtoResponse(new TrilhaServices().CadastrarItemTrilhaParticipacao(dtoParticipacao, AcessoAtual.Matricula, AcessoAtual.Nivel));
            };

            Post["/conhecigame"] = parametro =>
            {
                DTOParticipacaoConheciGame dtoParicipacao = this.Bind();
                var response = new DtoResponse(new TrilhaServices().informarParticipacaoConheciGame(dtoParicipacao));
                return response;
            };

            //Excluir Solucao trilheiro
            Delete["/excluir/{itemTrilhaId:int}"] = p =>
            {
                return new DtoResponse(new TrilhaServices().ExcluirSolucaoTrilheiro(AcessoAtual, p.itemTrilhaId));
            };

            Get["/obtermensagensguia/{lojaId:int}"] = p =>
            {
                var loja = new ManterTrilhaTopicoTematico().ObterTrilhaTopicoTematicoPorID(p.lojaId);

                return new DtoResponse(new TrilhaServices().ObterMensagensGuiaConclusaoSolucaoSebrae(AcessoAtual.Matricula, loja));
            };

            Post["/avaliar"] = parametro =>
            {
                DTOAvaliacaoSolucaoSebrae dto = this.Bind();
                var response = new DtoResponse(new TrilhaServices().InformarAvaliacao(dto.ID_ItemTrilha, dto.Resenha, dto.Avaliacao, AcessoAtual.Matricula));
                return response;
            };
        }
    }
}