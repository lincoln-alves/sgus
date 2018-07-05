using System;
using Nancy;
using Nancy.Security;
using Sebrae.Academico.BP.DTO.Services.Trilhas;
using Sebrae.Academico.BP.Services;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.Trilhas.Modules
{
    public class VivenciaModule : GenericModule
    {
        public VivenciaModule() : base("Vivencia")
        {
            this.RequiresAuthentication();

            Get["Finalizar"] = p =>
            {
                //VerificarBloqueio();
                object matriculaId;
                if (!AcessoAtual.Payload.TryGetValue("id", out matriculaId))
                    throw new ResponseException(enumResponseStatusCode.ErroRegraNegocioTrilhas,"User ID não definida.");

                object conclusao;
                if (!AcessoAtual.Payload.TryGetValue("conclusao", out conclusao))
                    throw new ResponseException(enumResponseStatusCode.ErroRegraNegocioTrilhas, "Conclusão não definida.");

                object solucaoId;
                if (!AcessoAtual.Payload.TryGetValue("itrid", out solucaoId))
                    throw new ResponseException(enumResponseStatusCode.ErroRegraNegocioTrilhas, "ItemTrilha não definida.");

                object fase;
                if (!AcessoAtual.Payload.TryGetValue("fase", out fase))
                    throw new ResponseException(enumResponseStatusCode.ErroRegraNegocioTrilhas, "Fase não definida.");
                
                // Aprovar o aluno de acordo com a conclusão.
                new TrilhaServices().AtualizarStatusJogo((int)solucaoId, (bool)conclusao, (int)matriculaId);

                return Response.AsJson("Dados recebidos com sucesso.",HttpStatusCode.OK);
            };

            Get["acesso/{solucaoId:int}"] = p =>
            {
                VerificarBloqueio();

                return new DtoResponse(new TrilhaServices().ObterAcessoJogo((int)p.solucaoId, AcessoAtual.Matricula));
            };

            /*
            Get["Token/{itemTrilhaId:int}"] = p =>
            {
                VerificarBloqueio();

                ItemTrilha itemTrilha = new ManterItemTrilha().ObterItemTrilhaPorID(p.itemTrilhaId);

                Fornecedor fornecedor = new ManterFornecedor().ObterFornecedorPorID(25);

                return Response.AsJson(JsonWebToken.Encode(new { id = AcessoAtual.Usuario.ID, fase = (int)itemTrilha.FaseJogo, itrid = itemTrilha.ID }, fornecedor.TextoCriptografia, JwtHashAlgorithm.HS256), HttpStatusCode.OK);
            };
            */
        }
    }
}