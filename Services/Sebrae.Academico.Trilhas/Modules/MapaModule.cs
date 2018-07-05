using Nancy.Security;
using Sebrae.Academico.BP.DTO.Services.Trilhas;
using Sebrae.Academico.BP.Services;

namespace Sebrae.Academico.Trilhas.Modules
{
    public class MapaModule : GenericModule
    {
        public MapaModule() : base("Mapa")
        {
            this.RequiresAuthentication();

            // Retornar os dados do mapa.
            Get["/"] = p =>
            {
                var trilhaServices = new TrilhaServices();

                return new DtoResponse(trilhaServices.ObterDadosMapa(AcessoAtual), trilhaServices.ObterMensagensGuiaMapa(AcessoAtual.Matricula));
            };

            // Retornar os créditos do nível da trilha.
            Get["/Creditos"] = p => new DtoResponse(new { Creditos = (AcessoAtual.Nivel.Trilha.Credito != "") ? AcessoAtual.Nivel.Trilha.Credito : "" , NomeTrilha = (AcessoAtual.Nivel.Trilha.Nome != "") ? AcessoAtual.Nivel.Trilha.Nome : "" });

            // Retornar o Ranking dos usuários do nível da trilha.
            // page: página do ranking.
            // nome: filtro de nome do aluno.
            Get["/Ranking/{page?}"] =
            Get["/Ranking/{page:int}/{nome?}/"] =
            Get["/Ranking/{page:int}/{nome?}/{uf?}/"] =
            Get["/Ranking/{page:int}/{nome:string}/{uf?}/{itensPorPagina?}/"] = p =>
            {
                return new DtoResponse(new TrilhaServices().ObterRanking(
                    AcessoAtual.Nivel,
                    p.page.HasValue ? p.page : 0,
                    p.nome.HasValue ? p.nome : null,
                    p.uf.HasValue ? p.uf : null,
                    p.itensPorPagina,
                    AcessoAtual
                 ));
            };
            
            // Obter dados do questionário. UC006 - Solução Sebrae Atividade Dissertativa - PI07 a PI11
            Get["/participacao/{itemTrilha:int}"] = p =>
            {
                VerificarBloqueio();
                // Retornar os dados do Questionário.
                return new DtoResponse(new TrilhaServices().ObterParticipacao(AcessoAtual.Matricula, AcessoAtual.Nivel, p.itemTrilha));
            };

            Get["/notificacoes"] = p =>
            {
                return new DtoResponse(new TrilhaServices().ObterNotificacoes(AcessoAtual.Matricula));
            };

            //Get["/usuarioaprovado"] = p => new DtoResponse(new TrilhaServices().UsuarioAprovado(AcessoAtual.Matricula));
        }
    }
}