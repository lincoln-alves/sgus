using System;
using System.Collections.Generic;
using System.Linq;
using JWT;
using Nancy.Authentication.Stateless;
using Nancy.Bootstrapper;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.Auth;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.Trilhas
{
    public class Security : IApplicationStartup
    {
        public void Initialize(IPipelines pipelines)
        {
            StatelessAuthentication.Enable(pipelines, new StatelessAuthenticationConfiguration(ctx =>
            {
                if (ctx.Request.Method == "OPTIONS")
                {
                    return null;
                }

                var manterUsuario = new ManterUsuario();
                var manterUsuarioTrilha = new ManterUsuarioTrilha();
                var manterTrilhaNivel = new ManterTrilhaNivel();
                var manterFornecedor = new ManterFornecedor();

                try
                {
                    var jwtToken = ctx.Request.Headers.Authorization;
                    
                    if (string.IsNullOrEmpty(jwtToken))
                        return null;

                    var payload = JsonWebToken.DecodeToObject(jwtToken, "", false) as IDictionary<string, object>;

                    if (payload != null)
                    {
                        var usuario = manterUsuario.ObterTodosIQueryable().Where(x => x.ID == (int) payload["id"])
                        .Select(x => new Usuario
                        {
                            ID = x.ID,
                            Nome = x.Nome,
                            CPF = x.CPF,
                            TrilhaTokenExpiry = x.TrilhaTokenExpiry,
                            TrilhaToken = x.TrilhaToken,
                            Senha = x.Senha
                        })
                        .FirstOrDefault();

                        if (usuario != null && usuario.TrilhaTokenExpiry > DateTime.Now)
                        {
                            JsonWebToken.Decode(jwtToken, usuario.TrilhaToken);

                            TrilhaNivel nivel = manterTrilhaNivel.ObterTrilhaNivelPorID((int) payload["nid"]);
                            UsuarioTrilha matricula = null;

                            if (payload.ContainsKey("experimenta") && !payload["experimenta"].Equals("experimente"))
                            {
                                if (!nivel.UsuarioPossuiMatricula((int) payload["id"]))
                                    return null;

                                matricula = manterUsuarioTrilha.ObterPorUsuarioNivel(usuario.ID, nivel.ID);

                                if (matricula == null)
                                    return null;
                            }
                            else{
                                matricula = new UsuarioTrilha
                                {
                                    ID = 0,
                                    TrilhaNivel = nivel,
                                    StatusMatricula = Dominio.Enumeracao.enumStatusMatricula.Inscrito
                                };
                            }

                            manterUsuario.AdicionarTempoTokenTrilha(usuario);

                            return new UserIdentity
                            {
                                UserName = usuario.CPF,
                                Usuario = usuario,
                                Matricula = matricula,
                                Nivel = nivel,
                                Payload = payload,
                                JwtToken = jwtToken
                            };
                        }

                        // Se chegou até aqui, verifica se o acesso é de fornecedor.
                        // Trazer os fornecedores apenas com os campos que interessam.
                        var fornecedores = manterFornecedor.ObterTodosIQueryable().Select(x => new Fornecedor
                        {
                            ID = x.ID,
                            Nome = x.Nome,
                            TextoCriptografia = x.TextoCriptografia
                        }).ToList();

                        // Tentar decodificar o JWT para verificar se o acesso é de fornecedor.
                        foreach (var fornecedor in fornecedores)
                        {
                            try
                            {
                                JsonWebToken.Decode(jwtToken, fornecedor.TextoCriptografia);

                                return new UserIdentity()
                                {
                                    UserName = fornecedor.Nome,
                                    Fornecedor = fornecedor,
                                    Payload = payload
                                };
                            }
                            catch
                            {
                                // Ignored.
                            }
                        }
                    }

                    return null;
                }
                catch(Exception ex)
                {
                    return null;
                }
                finally
                {
                    manterUsuario.Dispose();
                    manterUsuarioTrilha.Dispose();
                    manterTrilhaNivel.Dispose();
                    manterFornecedor.Dispose(); ;
                }
            }));
        }
    }
}