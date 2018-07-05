using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using classes = Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.WebForms;

namespace Sebrae.Academico.WebForms.Cadastros.TurmaCapacitacao
{
    public partial class ListarTurmaCapacitacao : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var bmCapacitacao = new BMCapacitacao();
                WebFormHelper.PreencherLista(new BMPrograma().ObterTodos().OrderBy(x => x.Nome).ToList(), ddlPrograma, true);
            }
        }

        protected void ddlPrograma_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPrograma.SelectedIndex > 0)
            {
                ddlCapacitacao.Enabled = true;
                var filtro = new classes.Capacitacao();
                filtro.Programa.ID = int.Parse(ddlPrograma.SelectedValue);
                WebFormHelper.PreencherLista(new BMCapacitacao().ObterPorFiltro(filtro).ToList(), ddlCapacitacao, true, false);
            }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            classes.TurmaCapacitacao capacitacao = ObterObjetoCapacitacao();
            IList<classes.TurmaCapacitacao> listaTurma = new BMTurmaCapacitacao().ObterPorFiltro(capacitacao);

            if (listaTurma != null && listaTurma.Count > 0)
            {
                WebFormHelper.PreencherGrid(listaTurma, this.gvTurmaCapacitacao);
                pnlTurmaCapacitacao.Visible = true;
            }
            else
            {
                pnlTurmaCapacitacao.Visible = false;
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Nenhuma Informação Encontrada");
            }
        }

        private classes.TurmaCapacitacao ObterObjetoCapacitacao()
        {
            var turma = new classes.TurmaCapacitacao();

            if (!string.IsNullOrEmpty(txtNome.Text))
                turma.Nome = txtNome.Text;

            if (ddlCapacitacao.SelectedIndex > 0)
                turma.Capacitacao.ID = int.Parse(ddlCapacitacao.SelectedValue);

            return turma;
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("EditarTurmaCapacitacao.aspx");
        }

        protected void dgvTurmaCapacitacao_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idCapacitacao = int.Parse(e.CommandArgument.ToString());
            if (e.CommandName.Equals("editar"))
            {
                Response.Redirect("EditarTurmaCapacitacao.aspx?Id=" + idCapacitacao.ToString(), false);
            }
            if (e.CommandName.Equals("excluir"))
            {
                try
                {
                    var bm = new BMTurmaCapacitacao();
                    var objTurmaCapacitacao = bm.ObterPorId(idCapacitacao);
                    bm.Excluir(objTurmaCapacitacao);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso", "ListarTurmaCapacitacao.aspx");
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }
                
            }
            if (e.CommandName.Equals("duplicar"))
            {
                hdIndexOfIdTurma.Value = idCapacitacao.ToString();
                ExibirModal();
            }
        }

        private void ExibirModal()
        {
            var pnlbackdrop = ((Panel)Master.Master.FindControl("pnlbackdrop"));
            if (pnlbackdrop != null)
                pnlbackdrop.Visible = true;
            pnlModal.Visible = true;
        
        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hdIndexOfIdTurma.Value))
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(txtNomeTurmaDuplicar.Text))
                    {
                        BMTurmaCapacitacao bmTurmaCapacitacao = new BMTurmaCapacitacao();

                        classes.TurmaCapacitacao turmaCapacitacao = bmTurmaCapacitacao.ObterPorId(int.Parse(hdIndexOfIdTurma.Value));
                        if (turmaCapacitacao != null)
                        {
                            classes.TurmaCapacitacao novaTurmaCapacitacao = new classes.TurmaCapacitacao();
                            novaTurmaCapacitacao.Nome = txtNomeTurmaDuplicar.Text;
                            if (turmaCapacitacao.ListaPermissao.Count > 0)
                            {
                                foreach (var permissao in turmaCapacitacao.ListaPermissao)
                                {
                                    novaTurmaCapacitacao.ListaPermissao.Add(new classes.TurmaCapacitacaoPermissao
                                    {
                                        TurmaCapacitacao = novaTurmaCapacitacao,
                                        Uf = permissao.Uf,
                                        QuantidadeVagasPorEstado = permissao.QuantidadeVagasPorEstado
                                    });
                                }
                            }
                            novaTurmaCapacitacao.Capacitacao = turmaCapacitacao.Capacitacao;
                            bmTurmaCapacitacao.Salvar(novaTurmaCapacitacao);
                            WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso !", "ListarTurmaCapacitacao.aspx");
                        }
                        else
                        {
                            throw new AcademicoException("A turma não foi localizada para ser duplicada.");
                        }
                    }
                    else
                    {
                        throw new AcademicoException("Informe o nome da turma para a duplicação.");
                    }
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
                catch (Exception)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro ao recuprar os dados da sessão para edição");
                }
            }
            else
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro ao recuperar turma para duplicar.");
            }
            Console.Write(hdIndexOfIdTurma.Value);
        }

        protected void OcultarModal_Click(object sender, EventArgs e)
        {
            
            OcultarModal();
        }

        private void OcultarModal()
        {
            var pnlbackdrop = ((Panel)Master.Master.FindControl("pnlbackdrop"));
            if (pnlbackdrop != null)
                pnlbackdrop.Visible = false;
            pnlModal.Visible = false;
        }
    }
}