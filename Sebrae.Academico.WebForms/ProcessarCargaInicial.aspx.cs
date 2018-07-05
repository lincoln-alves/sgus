using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BM.Classes;

namespace Sebrae.Academico.WebForms
{
    public partial class ProcessarCargaInicial : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnProcessarCargaInicial_Click(object sender, EventArgs e)
        {
            try
            {
                ManterUsuario manterUsuario = new ManterUsuario();
                var listaUsuarios = manterUsuario.ObterTodos();

                //Faz o filtro de usuário, em memória
                IList<Usuario> listaUsuariosComSenhaMenores = listaUsuarios.Where(x => !string.IsNullOrWhiteSpace(x.Senha) &&
                                                                                  x.Senha.Length > 1 && x.Senha.Length <= 10).ToList();

                string stringCriptografadaComAes = string.Empty;

                foreach (Usuario usuario in listaUsuariosComSenhaMenores)
                {
                    try
                    {
                        stringCriptografadaComAes = CriptografiaHelper.Criptografar(usuario.Senha);
                        usuario.Senha = stringCriptografadaComAes;
                        manterUsuario.SalvarSemValidacao(usuario);
                    }
                    catch (Exception ex)
                    {
                        Response.Write(ex.Message);
                        continue;
                    }
                }
                new BMUsuario().Commit();

            }
            catch
            {

                throw;
            }
        }


    }
}