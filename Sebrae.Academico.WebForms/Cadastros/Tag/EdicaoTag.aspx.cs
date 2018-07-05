using System;
using System.Web.UI;
using System.Linq;
using Sebrae.Academico.BP;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Classes = Sebrae.Academico.Dominio.Classes;
using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.WebForms.Cadastros
{
    public partial class EdicaoTag : Page
    {
        private Classes.Tag _tagEdicao;
        private ManterTag manterTag = new ManterTag();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                PreencherCombos();

                //Update, ou seja, estamos alterado os dados de uma tag
                if (Request["Id"] != null)
                {
                    int idTag = int.Parse(Request["Id"].ToString());
                    _tagEdicao = manterTag.ObterTagPorID(idTag);
                    RemoverOFilhoDaListaDePais();
                    PreencherCampos(_tagEdicao);
                }
            }
        }

        private void RemoverOFilhoDaListaDePais()
        {
            //Remove a tag id da lista de tags Pais
            var ListaTag = (IList<Classes.Tag>)ddlTagPai.DataSource;
            var tagAtual = ListaTag.Where(x => x.ID == _tagEdicao.ID).FirstOrDefault();
            ListaTag.Remove(tagAtual);
            WebFormHelper.PreencherLista(ListaTag, ddlTagPai, false, true);
        }

        private void PreencherCombos()
        {
            try
            {
                PreencherComboTagsPai();
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private void PreencherComboTagsPai()
        {
            ManterTag manterTag = new ManterTag();
            IList<Classes.Tag> ListaTag = manterTag.ObterTodasTag();
            ListaTag = ListaTag.OrderBy(x => x.Nome).ToList();
            WebFormHelper.PreencherLista(ListaTag, this.ddlTagPai, false, true);
        }

        private void PreencherCampos(Classes.Tag tag)
        {
            if (tag != null)
            {
                //Nome da Tag
                txtNome.Text = tag.Nome;

                //Tag Pai
                if (tag.TagPai != null)
                    WebFormHelper.SetarValorNaCombo(tag.TagPai.ID.ToString(), ddlTagPai);

                //Sinônimo
                if (tag.InSinonimo.HasValue)
                {
                    this.chkSinonimo.Checked = tag.InSinonimo.Value;
                }
                else
                {
                    this.chkSinonimo.Checked = false;
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Session.Remove("TagEdit");
            Response.Redirect("ListarTag.aspx");
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Request["Id"] == null)
                {
                    manterTag = new ManterTag();
                    _tagEdicao = ObterObjetoTag();
                    manterTag.IncluirTag(_tagEdicao);
                }
                else
                {
                    _tagEdicao = ObterObjetoTag();
                    manterTag.AlterarTag(_tagEdicao);
                }

                Session.Remove("TagEdit");

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso !", "ListarTag.aspx");

            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private Classes.Tag ObterObjetoTag()
        {

            if (Request["Id"] != null)
            {
                _tagEdicao = new BMTag().ObterPorID(int.Parse(Request["Id"].ToString()));
            }
            else
            {
                _tagEdicao = new Classes.Tag();
            }

            //Nome da Tag
            _tagEdicao.Nome = txtNome.Text.Trim();

            //Tag Pai
            if (!string.IsNullOrWhiteSpace(this.ddlTagPai.SelectedItem.Value))
            {
                _tagEdicao.TagPai = new BMTag().ObterPorID(int.Parse(this.ddlTagPai.SelectedItem.Value));
            }
            else
            {
                _tagEdicao.TagPai = null;
            }

            //Sinônimo
            _tagEdicao.InSinonimo = this.chkSinonimo.Checked;

            return _tagEdicao;
        }


        protected void btnCancelar_Click1(object sender, EventArgs e)
        {
            Response.Redirect("ListarTag.aspx");
        }


    }
}