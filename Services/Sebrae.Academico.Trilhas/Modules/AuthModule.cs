using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using JWT;
using Nancy;
using Nancy.ModelBinding;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.DTO.Services.Trilhas.Auth;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.Trilhas.Modules
{
    public class AuthModule : BaseModule
    {
        private ManterUsuario manterUsuario = new ManterUsuario();
        private ManterTrilhaNivel manterTrilhaNivel = new ManterTrilhaNivel();

        public AuthModule() : base("Auth")
        {
            Get["/"] = p =>
            {
                return "Token Valid!";
            };

            Post["Login"] = p =>
            {
                var dadosAcesso = this.Bind<DTODadosAcesso>();

                var usuario = new ManterUsuario().ObterTodosIQueryable().Where(x => x.CPF == dadosAcesso.Login)
                .Select(x => new Usuario
                {
                    ID = x.ID,
                    Senha = x.Senha
                })
                .FirstOrDefault();

                if (usuario == null)
                    return "Usuário não existe.";

                if (usuario.Senha != dadosAcesso.Senha)
                    return "Senha inválida.";

                var nivel = manterTrilhaNivel.ObterTrilhaNivelPorID(dadosAcesso.NivelID);

                if (nivel == null || !nivel.UsuarioPossuiMatricula(usuario.ID))
                    return null;

                return
                    Response.AsJson(JsonWebToken.Encode(new {id = usuario.ID, nid = nivel.ID},
                        manterUsuario.GerarTokenTrilha(usuario), JwtHashAlgorithm.HS256));
            };

            Post["LoginFornecedor"] = p =>
            {
                var dadosAcesso = this.Bind<DTODadosAcesso>();
                
                var usuario = manterUsuario.ObterPorCPF(dadosAcesso.Login);

                if (usuario == null)
                    return "Usuário não existe.";

                if (usuario.Senha != dadosAcesso.Senha)
                    return "Senha inválida.";

                var nivel = manterTrilhaNivel.ObterTrilhaNivelPorID(dadosAcesso.NivelID);

                if (!nivel.UsuarioPossuiMatricula(usuario.ID))
                    return null;
                
                var manterFornecedor = new ManterFornecedor();
                var fornecedor = manterFornecedor.ObterFornecedorPorID(25);

                if (fornecedor == null)
                    return "Fornecedor não existe.";

                return Response.AsJson(JsonWebToken.Encode(new { id = usuario.ID, fase = 1, itrid = 1, nid = nivel.ID }, fornecedor.TextoCriptografia, JwtHashAlgorithm.HS256), HttpStatusCode.OK);
            };
        }
    }
}