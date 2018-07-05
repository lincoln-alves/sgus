using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.Cadastros.Informe
{
    public partial class ListarInformes : Page
    {
        protected void btnNovo_OnClick(object sender, EventArgs e)
        {
            Response.Redirect("EdicaoInforme.aspx", true);
        }

        protected void btnPesquisar_OnClick(object sender, EventArgs e)
        {
            var texto = txtNome.Text.Trim();

            var manterInforme = new ManterInforme();

            var informes = new List<Dominio.Classes.Informe>();

            if (!string.IsNullOrWhiteSpace(texto))
            {
                int valorNumerico;

                if (int.TryParse(texto, out valorNumerico))
                {
                    // Buscar onde o valor numérico informado está contido no mês ou no ano do Informe.

                    informes = manterInforme.ObterPorNumero(valorNumerico).ToList();
                }
                else if (texto.Contains("/"))
                {
                    var mesAno = texto.Split('/');

                    int mes, ano;

                    if (int.TryParse(mesAno[0].Trim(), out mes) && int.TryParse(mesAno[1].Trim(), out ano))
                    {
                        informes = manterInforme.ObterPorMesAno(mes, ano).ToList();
                    }
                    else
                    {
                        informes = manterInforme.ObterPorNome(texto).ToList();
                    }
                }
                else
                {
                    var palavrasChave = texto.Split(' ');

                    if (palavrasChave.Length > 1)
                    {
                        foreach (var palavra in palavrasChave)
                        {
                            if (int.TryParse(palavra, out valorNumerico))
                            {
                                informes.AddRange(manterInforme.ObterPorNumero(valorNumerico).ToList());
                            }
                            else
                            {
                                informes.AddRange(manterInforme.ObterPorNome(palavra));
                            }
                        }

                        // Distinguir pois podem existir repetições dos Informes.
                        informes = informes.Distinct().ToList();
                    }
                    else
                    {
                        informes = manterInforme.ObterPorNome(texto).ToList();
                    }
                }
            }
            else
            {
                informes = manterInforme.ObterTodos().ToList();
            }

            WebFormHelper.PreencherGrid(informes, dgvInformes);
        }

        protected void dgvInformes_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("excluir"))
            {
                try
                {
                    var id = int.Parse(e.CommandArgument.ToString());
                    
                    new ManterInforme().Excluir(id);

                    btnPesquisar_OnClick(null, null);

                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!");
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
                catch (Exception)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Não foi possível remover este Informe. Tente novamente.");
                }
            }
            else if (e.CommandName.Equals("editar"))
            {
                var id = int.Parse(e.CommandArgument.ToString());
                Response.Redirect("EdicaoInforme.aspx?Id=" + id, true);
            }
        }
    }
}