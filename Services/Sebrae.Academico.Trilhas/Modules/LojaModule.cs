using Nancy.Security;
using Sebrae.Academico.BP.DTO.Services.Trilhas;
using Sebrae.Academico.BP.Services;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.Trilhas.Modules
{
    public class LojaModule : GenericModule
    {
        public LojaModule() : base("Loja")
        {
            this.RequiresAuthentication();

            // Obter dados da loja. Tem que passar o Id do nível (Mapa) e o Id da loja (Tópico Temático)
            Get["/{lojaId:int}"] = p =>
            {
                var trilhaServices = new TrilhaServices();

                // Objeto da loja obtido no método de dados da loja. É usado pra obter as mensagens da guia.
                PontoSebrae pontoSebrae;

                trilhaServices.VerificarConclusaoMissao(AcessoAtual);
                
                // Retornar os dados da loja.
                var response = new DtoResponse(
                    trilhaServices.ObterDadosLoja(AcessoAtual, p.lojaId, out pontoSebrae),
                    trilhaServices.ObterMensagensGuiaLoja(AcessoAtual.Matricula, pontoSebrae)
                );

                return response;
            };

            Get["/cambio/mensagemguia"] = p =>
            {
                return new DtoResponse(new TrilhaServices().ObterMensagensGuiaCambio(AcessoAtual.Matricula));
            };

            Post["/Cambio"] = p =>
            {
                VerificarBloqueio();
                // Retornar a quantidade de moedas de ouro.

                return new DtoResponse(new TrilhaServices().ConverterMoedasOuro(AcessoAtual));
            };
        }
    }
}