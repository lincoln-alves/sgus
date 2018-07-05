using System;
using System.Linq;
using System.Web.UI;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using classes = Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.WebForms.Cadastros.Missao
{
    public partial class EdicaoMissao : Page
    {
        private readonly ManterMissao _manterMissao = new ManterMissao();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                WebFormHelper.PreencherLista(new ManterTrilha().ObterTodasTrilhas(), ddlTrilha, false, true);

                if (Request["Id"] != null)
                {
                    WebFormHelper.PreencherLista(new ManterTrilhaNivel().ObterTodosTrilhaNivel(), ddlTrilhaNivel, false,
                        true);
                    WebFormHelper.PreencherListaCustomizado(new ManterPontoSebrae().ObterAtivos().ToList(),
                        ddlPontoSebrae, "ID", "NomeExibicao", false, true);

                    var missao = _manterMissao.ObterPorID(int.Parse(Request["Id"]));
                    PreencherCampos(missao);
                }
            }
        }


        private void PreencherCampos(classes.Missao missao)
        {
            WebFormHelper.SetarValorNaCombo(missao.PontoSebrae.TrilhaNivel.Trilha.ID.ToString(), ddlTrilha, true);
            WebFormHelper.SetarValorNaCombo(missao.PontoSebrae.TrilhaNivel.ID.ToString(), ddlTrilhaNivel, true);
            WebFormHelper.SetarValorNaCombo(missao.PontoSebrae.ID.ToString(), ddlPontoSebrae, true);

            txtMissao.Text = missao.Nome;
        }


        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    ValidarCampos();

                    var missao = ObterObjetoMissao();

                    if (Request["Id"] == null)
                    {
                        _manterMissao.IncluirMissao(missao);
                    }
                    else
                    {
                        _manterMissao.AlterarMissao(missao);
                    }

                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso!",
                        "ListarMissao.aspx");

                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }
        }

        private void ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtMissao.Text))
                throw new AcademicoException("O campo Nome é obrigatório");

            if(ddlPontoSebrae.SelectedIndex <= 0)
                throw new AcademicoException("O campo Ponto Sebrae é obrigatório");
        }

        private classes.Missao ObterObjetoMissao()
        {
            var missao = Request["Id"] != null
                ? _manterMissao.ObterPorID(int.Parse(Request["Id"]))
                : new classes.Missao();

            missao.PontoSebrae = new ManterPontoSebrae().ObterPorId(int.Parse(ddlPontoSebrae.SelectedValue));

            missao.Nome = txtMissao.Text.Trim();

            return missao;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListarMissao.aspx");
        }

        protected void ddlTrilha_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var trilhaId = int.Parse(ddlTrilha.SelectedValue);

            WebFormHelper.PreencherLista(new ManterTrilhaNivel().ObterPorTrilha(new classes.Trilha { ID = trilhaId }),
                ddlTrilhaNivel, false, true);

            ddlPontoSebrae.Items.Clear();
        }

        protected void ddlTrilhaNivel_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var trilhaNivelId = int.Parse(ddlTrilhaNivel.SelectedValue);

            var trilhaNivel = new ManterTrilhaNivel().ObterTrilhaNivelPorID(trilhaNivelId);

            WebFormHelper.PreencherListaCustomizado(trilhaNivel.ObterPontosSebraeAtivos().ToList(), ddlPontoSebrae, "ID",
                "NomeExibicao", false, true);
        }
    }
}