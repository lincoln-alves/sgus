using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Relatorios;

namespace Sebrae.Academico.WebForms.Cadastros.Nacionalizacao
{
    /// <summary>
    /// Tela dinâmica de Configurações do Sistema.
    /// </summary>
    public partial class SelecionarUfs : PageBase
    {
        protected override IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade
        {
            get
            {
                var perfis = new List<enumPerfil>();
                perfis.Add(enumPerfil.Administrador);
                return perfis;
            }
        }

        protected override enumFuncionalidade Funcionalidade
        {
            get { return enumFuncionalidade.Nacionalizacao; }
        }

        private void Page_Load(object sender, EventArgs e)
        {
            var manterUf = new ManterUf();

            foreach (var uf in manterUf.ObterTodosUf())
            {
                var row = new TableRow();
                row.TableSection = TableRowSection.TableBody;

                var cellUf = new TableCell();
                cellUf.Text = uf.Nome;

                var btnNacionalizacao = new Button
                {
                    Text = (uf.IsNacionalizado() ? "Remover" : "Nacionalizar"),
                    CssClass = "btn btn-block " + (uf.IsNacionalizado() ? "btn-default" : "btn-primary"),
                    CommandArgument = uf.ID.ToString()
                    //, CommandName = (uf.IsNacionalizado() ? "NAC" : "DESNAC") // NAC = Nacionalizar, DESNAC = Desnacionalizar
                };

                btnNacionalizacao.Click += btnNacionalizacao_OnClick;

                // Performar PostBack no Click.


                var cellBtn = new TableCell();
                cellBtn.Controls.Add(btnNacionalizacao);


                row.Cells.Add(cellUf);
                row.Cells.Add(cellBtn);
                myTable.Rows.Add(row);
            }
        }

        protected void btnNacionalizacao_OnClick(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            var idUf = int.Parse(btn.CommandArgument);
            
            var manterUf = new ManterUf();

            manterUf.Nacionalizar(idUf);

            // Forçar a atualização da página.
            Response.Redirect("/Cadastros/Nacionalizacao/SelecionarUfs.aspx");
        }
    }
}