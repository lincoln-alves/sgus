using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System;
using System.Collections.Generic;

namespace Sebrae.Academico.WebForms.Cadastros.Faq
{
    public partial class EdicaoFaq : PageBase
    {
        private TrilhaFaq _faq;

        protected override IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        protected override enumFuncionalidade Funcionalidade
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PreencherCombos();

                if (Request["Id"] != null)
                {
                    int id;
                    if (int.TryParse(Request["Id"], out id))
                    {
                        PreencherCampos();
                    }
                }
            }
        }

        private void PreencherCampos()
        {
            try
            {
                int id;
                if (int.TryParse(Request["Id"], out id))
                {
                    var faq = new ManterTrilhaFaq().ObterPorId(id);

                    txtNome.Text = faq.Nome;
                    txtDescricao.Text = faq.Descricao;

                    if (faq.Assunto != null)
                    {
                        WebFormHelper.SetarValorNaCombo(faq.Assunto.ID.ToString(), ddlAssunto);
                    }
                }
            }
            catch (AcademicoException)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Não foi possível recuperar o registro.");
            }
        }

        private void PreencherCombos()
        {
            var assuntos = new ManterTrilhaFaq().ObterTodosAssunto();
            WebFormHelper.PreencherLista(assuntos, ddlAssunto, false, true);

            dvgAssuntos.DataSource = assuntos;
            dvgAssuntos.DataBind();
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                _faq = ObterObjetoTrilhaFaq();
                new ManterTrilhaFaq().Salvar(_faq);

                hdAssuntoEdicao.Value = "";

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Cadastrado com sucesso !", "ListarFaq.aspx");
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private TrilhaFaq ObterObjetoTrilhaFaq()
        {
            _faq = Request["Id"] != null
                ? new ManterTrilhaFaq().ObterPorId((int.Parse(Request["Id"])))
                : new TrilhaFaq();

            _faq.Nome = txtNome.Text;
            _faq.Descricao = txtDescricao.Text;

            if (!string.IsNullOrEmpty(ddlAssunto.SelectedValue))
            {
                int idAssunto;

                if (int.TryParse(ddlAssunto.SelectedValue, out idAssunto))
                {
                    var assunto = new ManterTrilhaFaq().ObterAssuntoPorId(idAssunto);
                    _faq.Assunto = assunto;
                }
            }

            return _faq;
        }

        private AssuntoTrilhaFaq ObterObjetoAssuntoTrilhaFaq()
        {
            AssuntoTrilhaFaq assunto = new AssuntoTrilhaFaq();

            if (!string.IsNullOrEmpty(hdAssuntoEdicao.Value))
            {
                assunto = new ManterTrilhaFaq().ObterAssuntoPorId(int.Parse(hdAssuntoEdicao.Value));
            }

            assunto.Nome = txtAssuntoTrilhas.Text;

            return assunto;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListarFaq.aspx");
        }

        protected void btnEnviarAssunto_Click(object sender, EventArgs e)
        {
            try
            {
                var assunto = ObterObjetoAssuntoTrilhaFaq();
                new ManterTrilhaFaq().SalvarAssunto(assunto);

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro cadastrado com sucesso !");

                PreencherCombos();

                txtAssuntoTrilhas.Text = "";
                hdAssuntoEdicao.Value = "";
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
        }

        protected void dvgAssuntos_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("excluir"))
            {
                try
                {
                    var manter = new ManterTrilhaFaq();
                    int idFaq = int.Parse(e.CommandArgument.ToString());
                    var assunto = manter.ObterAssuntoPorId(idFaq);

                    manter.ExcluirAssunto(assunto);

                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!");

                    PreencherCombos();
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }
            }
            else if (e.CommandName.Equals("editar"))
            {
                try
                {
                    int idAssunto = int.Parse(e.CommandArgument.ToString());
                    var assunto = new ManterTrilhaFaq().ObterAssuntoPorId(idAssunto);

                    // Usado na edição de um assunto
                    hdAssuntoEdicao.Value = idAssunto.ToString();

                    txtAssuntoTrilhas.Text = assunto.Nome;
                }
                catch (AcademicoException)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Não foi possível recuperar o registro.");
                }
            }
        }

        protected void lbkAssuntos_Click(object sender, EventArgs e)
        {
            AlterarStatusTab(lbkAssuntos, collapseAlunos);
        }
    }
}