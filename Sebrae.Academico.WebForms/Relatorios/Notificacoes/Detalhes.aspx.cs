using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sebrae.Academico.WebForms.Relatorios.Notificacoes
{
    public partial class Detalhes : System.Web.UI.Page
    {

        public override void VerifyRenderingInServerForm(Control control)
        {
            //base.VerifyRenderingInServerForm(control);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            int id;

            if (int.TryParse(Request.QueryString["id"], out id))
            {

                PreencherGridRelatorio(id);
            }
            else
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Não foi possível recuperar os dados");
            }
        }

        private void PreencherGridRelatorio(int id)
        {
            var notificacaoEnvio = new ManterNotificacaoEnvio().ObterNotificacaoEnvioPorID(id);
            PreencherRelatorio(notificacaoEnvio.Notificacoes);


            if (dgvNotificacoes.Rows.Count > 0)
            {
                componenteGeracaoRelatorio.Visible = true;
            }
        }

        private void PreencherRelatorio(IEnumerable<Notificacao> notificacoes)
        {
            // Adequando formato de dados ao esperado pelo grid view
            var notificacaoesFormatado = notificacoes.Select(x => new
            {
                Usuario = x.Usuario,
                DataEnvio = x.DataNotificacao
            }).ToList();

            WebFormHelper.PreencherGrid(notificacaoesFormatado, dgvNotificacoes);
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            var usuario = new Usuario
            {
                Nome = txtNome.Text,
                CPF = txtCPF.Text,
                Email = txtEmail.Text
            };

            int id;

            if (int.TryParse(Request.QueryString["id"], out id))
            {
                FiltrarRelatorio(id, usuario);
            }
            else
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Não foi possível filtrar o relatório");
            }
        }


        private void FiltrarRelatorio(int id, Usuario usuario)
        {
            var dataDeEnvio = !string.IsNullOrEmpty(txtDataDeEnvio.Text) ? DateTime.Parse(txtDataDeEnvio.Text) : (DateTime?)null;

            var notificacoes = new ManterNotificacao().ObterPorFiltro(usuario, dataDeEnvio, id);

            var notificacaoesFormatado = notificacoes.Select(x => new
            {
                Usuario = x.Usuario,
                DataEnvio = x.DataNotificacao
            }).ToList();

            WebFormHelper.PreencherGrid(notificacaoesFormatado.ToList(), dgvNotificacoes);


            if (dgvNotificacoes.Rows.Count > 0)
            {
                componenteGeracaoRelatorio.Visible = true;
            }
        }

        protected void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            WebFormHelper.GerarArquivoRelatorio(enumTipoSaidaRelatorio.EXCEL, dgvNotificacoes);
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Cadastros/NotificacaoEnvio/ListarNotificacaoEnvio.aspx");
        }
    }
}