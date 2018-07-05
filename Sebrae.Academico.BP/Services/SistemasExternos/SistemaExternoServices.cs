using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.BP.solTokenService;
using Sebrae.Academico.BP.Services.SistemasExternos;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP.Services
{
    public class SistemaExternoServices : BusinessProcessServicesBase
    {
        public IList<DTOSistemaExterno> ObterTodosPorUsuario(int idUsuario)
        {
            var lista = new List<DTOSistemaExterno>();

            // Se o id do usuário for 0 só retorna os sistemas externos de acesso público
            if (idUsuario == 0)
            {
                var sistemaExterno = new SistemaExterno { Publico = true };

                lista =
                    new ManterSistemaExterno().ObterSistemaExternoPorFiltro(sistemaExterno)
                        .Select(
                            x =>
                                new DTOSistemaExterno
                                {
                                    ID = x.ID,
                                    Nome = x.Nome,
                                    LinkAcesso = x.LinkSistemaExterno,
                                    Descricao = x.Descricao
                                })
                        .ToList();
            }
            // Do contrário segue checando as permissões de cada
            else
            {
                var usuario = new BMUsuario().ObterPorId(idUsuario);
                usuario.ListaPermissaoSistemasExternos = new ManterUsuario().ObterListaPermissaoSistemasExternos(usuario.ID);

                var senhaMd5 = CriptografiaHelper.ObterHashMD5(CriptografiaHelper.Decriptografar(usuario.Senha));
                var tokenSaber = CriptografiaHelper.ObterHashMD5(usuario.CPF + "fgv2012").ToLower();
                var tokenPearson = CriptografiaHelper.ObterHashMD5(usuario.CPF + "UwmdFPxx4AT6kTqtKqISn6fwqjLrPz3F").ToLower();

                // Percorre a lista de permissões do usuário e dá replace montando o SSO.
                foreach (var registro in usuario.ListaPermissaoSistemasExternos)
                {
                    try
                    {

                        var dtoSistemaExterno = new DTOSistemaExterno
                    {
                        ID = registro.SistemaExterno.ID,
                        Nome = registro.SistemaExterno.Nome,
                        LinkAcesso = registro.SistemaExterno.LinkSistemaExterno,
                        Descricao = registro.SistemaExterno.Descricao,
                        MesmaJanela = registro.SistemaExterno.MesmaJanela == true
                    };

                    if (registro.SistemaExterno.EnglishTown == true)
                    {
                        try
                        {
                            var memberEfApi = new EfServices().CreateMemberAndActivateSubscriptionApi(usuario, true);

                            if (memberEfApi != null)
                        {
                                dtoSistemaExterno.LinkAcesso = memberEfApi.LaunchUrl;
                            }

                            else continue;
                        }
                        catch (Exception)
                        {
                            continue;
                    }
                    }
                    else
                    {
                        // Substituindo placeholders na descrição - Utilizado no caso da biblioteca CENGAGE
                        var dataurl = new
                        {
                            plate = usuario.ID,
                            email = usuario.Email,
                            user = usuario.Nome
                        };
                        var data = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(dataurl)));
                        dtoSistemaExterno.Descricao = dtoSistemaExterno.Descricao.Replace("#DATAURL", data);

                        // Substituindo placeholders no link de acesso
                        dtoSistemaExterno.LinkAcesso = dtoSistemaExterno.LinkAcesso.Replace("#CPF", usuario.CPF);
                        dtoSistemaExterno.LinkAcesso = dtoSistemaExterno.LinkAcesso.Replace("#SENHAMD5 ", senhaMd5);
                        dtoSistemaExterno.LinkAcesso = dtoSistemaExterno.LinkAcesso.Replace("#TOKENSABER", tokenSaber);
                        dtoSistemaExterno.LinkAcesso = dtoSistemaExterno.LinkAcesso.Replace("#TOKENPEARSON", tokenPearson);

                        if (registro.SistemaExterno.LinkSistemaExterno.Contains("SOL"))
                        {
                            var tokenSol = RecuperarTokenSOL(usuario.CPF, usuario.Senha);
                            dtoSistemaExterno.LinkAcesso = dtoSistemaExterno.LinkAcesso.Replace("#TOKENSOL", tokenSol);
                        }
                    }

                    lista.Add(dtoSistemaExterno);
                }
                    catch
                    {
                        continue;
                    }
                }
            }

            return lista;
        }

        private string RecuperarTokenSOL(string cpf, string senha)
        {
            string token = string.Empty;

            try
            {
                TokenWSClient twsc = new TokenWSClient();
                string senhaDescriptografada = string.Empty;
                if (!string.IsNullOrEmpty(senha))
                    senhaDescriptografada = CriptografiaHelper.Decriptografar(senha);
                token = twsc.gerarToken(cpf, senhaDescriptografada);
            }
            catch (Exception)
            {

            }
            return token;
        }
    }
}
