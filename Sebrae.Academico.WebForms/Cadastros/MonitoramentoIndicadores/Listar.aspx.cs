using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.WebForms.Cadastros.MonitoramentoIndicadores
{
    public partial class Listar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e){
            if (!IsPostBack){
                
            }
        }
        protected void dgvMonitoramentoIndicador_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("excluir"))
            {
                try
                {
                    var manterMonitoramentoIndicadores = new ManterMonitoramentoIndicadores();
                    var idModel = int.Parse(e.CommandArgument.ToString());
                    manterMonitoramentoIndicadores.Excluir(idModel);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!", "Listar.aspx");
                }catch (AcademicoException ex){
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }
            }
            else if (e.CommandName.Equals("editar"))
            {
                int id = int.Parse(e.CommandArgument.ToString());
                Response.Redirect("Editar.aspx?Id=" + id.ToString(), false);
            }
        }
        protected void btnPesquisar_Click(object sender, EventArgs e){
            try{
                var manterMonitoramento = new ManterMonitoramentoIndicadores();
                var ano = -1;
                if (!string.IsNullOrEmpty(txtAno.Text)) {
                    if (!int.TryParse(txtAno.Text.Trim(), out ano)) {
                        throw new AcademicoException("Informe um ano válido.");
                    }
                }
                var lista = ano == -1 ? manterMonitoramento.ObterTodosMonitoramentosIndicadores() : manterMonitoramento.ObterTodosMonitoramentosIndicadoresPorFiltro(new Dominio.Classes.MonitoramentoIndicadores { Ano = ano});
                if (lista.Count > 0){
                    WebFormHelper.PreencherGrid(lista, dgvMonitoramentoIndicador);
                    pnlMonitoramentoIndicador.Visible = true;
                    return;
                }

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Informacao, "Nenhum Registro encontrado");
            }catch (AcademicoException ex){
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }
        protected void btnNovo_Click(object sender, EventArgs e){
            Response.Redirect("Editar.aspx");
        }
    }
}