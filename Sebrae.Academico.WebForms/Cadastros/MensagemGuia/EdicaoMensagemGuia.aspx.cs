using System;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;
using System.Collections.Generic;

namespace Sebrae.Academico.WebForms.Cadastros.MensagemGuia
{
    public partial class EdicaoMensagemGuia : Page
    {
        public string HashTagsJson
        {
            get
            {
                return ViewState["HashTagsJson"] != null ? ViewState["HashTagsJson"].ToString() : "";  
            }
            set
            {
                ViewState["HashTagsJson"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request["Id"] == null)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao, "Mensagem inválida. Tente novamente.");

                    Response.Redirect("ListarMensagemGuia.aspx");
                }
                else
                {
                    var mensagemGuia = new ManterMensagemGuia().ObterPorId(int.Parse(Request["Id"]));

                    if (mensagemGuia == null)
                    {
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao, "Mensagem inválida. Tente novamente.");

                        Response.Redirect("ListarMensagemGuia.aspx");
                    }
                    else
                    {
                        PreencherCampos(mensagemGuia);
                    }
                }
            }
        }

        /// <summary>
        /// Preencher campos na edição.
        /// </summary>
        /// <param name="mensagemGuia">Objeto MensagemGuia para preencher os dados.</param>
        private void PreencherCampos(Dominio.Classes.MensagemGuia mensagemGuia)
        {
            ltrMomento.Text = mensagemGuia.ID.GetDescription();

            ddlTipo.SelectedValue = ((int)mensagemGuia.Tipo).ToString();

            // Criar uma lista com um nome custom para exibir no DropDown.
            var lista =
                new ManterTrilhaTutorial().ObterTodosIQueryable()
                    .OrderBy(x => x.Categoria.GetDescription())
                    .Select(x => new
                    {
                        x.ID,
                        Nome = string.Format("{0} - {1}", x.Categoria.GetDescription(), x.Nome)
                    });

            if (mensagemGuia.Tipo == enumTipoMensagemGuia.Personalizada)
            {
                divTexto.Visible = true;
                divTutorial.Visible = false;

                txtTexto.Text = mensagemGuia.Texto;

                SetarHashTags(mensagemGuia);
            }
            else
            {
                divTexto.Visible = false;
                divTutorial.Visible = true;

                //if (mensagemGuia.Tutorial != null)
                //    ddlTutorial.SelectedValue = mensagemGuia.Tutorial.ID.ToString();

                PreencherTutoriais();

                uclistTutorial.MarcarComoSelecionados(mensagemGuia.Tutoriais.Select(x => x.ID));
            }
        }

        private void PreencherTutoriais()
        {
            uclistTutorial.PreencherItens(new ManterTrilhaTutorial().ObterTodosTrilhaTutorials(), "ID", "Nome");
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListarMensagemGuia.aspx");
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                SalvarMensagemGuia();

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados gravados com sucesso.", "ListarMensagemGuia.aspx");
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
            catch (Exception)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Houve um erro inesperado no cadastro da Mensagem da Guia. Tente novamente.");
            }
        }

        private Dominio.Classes.MensagemGuia ObterObjetoMensagemGuia()
        {
            ValidarMensagemGuia();

            var mensagemGuia = new ManterMensagemGuia().ObterPorId(int.Parse(Request["Id"]));

            mensagemGuia.Tipo = (enumTipoMensagemGuia)int.Parse(ddlTipo.SelectedValue);

            switch (mensagemGuia.Tipo)
            {
                case enumTipoMensagemGuia.Personalizada:
                    mensagemGuia.Texto = txtTexto.Text;
                    break;
                case enumTipoMensagemGuia.Tutorial:

                    var tutorais = new List<Dominio.Classes.TrilhaTutorial>();

                    foreach (var tutorial in uclistTutorial.RecuperarIdsSelecionados())
                    {
                        tutorais.Add(new Dominio.Classes.TrilhaTutorial
                        {
                            ID = int.Parse(tutorial)
                        });
                    }

                    mensagemGuia.Tutoriais = tutorais;

                    break;
                default:
                    break;
            }

            return mensagemGuia;
        }

        private void ValidarMensagemGuia()
        {
            if (ddlTipo.SelectedValue == "-1")
                throw new AcademicoException("Campo \"Tipo\" é obrigatório.");

            if (ddlTipo.SelectedValue == "0")
            {
                if (string.IsNullOrWhiteSpace(txtTexto.Text))
                    throw new AcademicoException("Campo \"Texto\" é obrigatório.");
            }
            else
                if (!uclistTutorial.RecuperarIdsSelecionados().Any())
                throw new AcademicoException("Selecione um Tutorial.");
        }

        protected void btnEnviar_OnClick(object sender, EventArgs e)
        {
            try
            {
                SalvarMensagemGuia();

                Response.Redirect("ListarMensagemGuia.aspx");
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
            catch (Exception)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Houve um erro inesperado no cadastro da Mensagem da Guia. Tente novamente.");
            }
        }

        private void SalvarMensagemGuia()
        {
            var mensagem = ObterObjetoMensagemGuia();

            new ManterMensagemGuia().Salvar(mensagem);
        }

        protected void ddlTipo_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var tipo = int.Parse(ddlTipo.SelectedValue);

            switch (tipo)
            {
                case -1:
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Selecione um tipo.");

                    break;
                case 0:
                    divTexto.Visible = true;
                    divTutorial.Visible = false;

                    if (!string.IsNullOrEmpty(Request["Id"]))
                    {
                        var mensagemGuia = new ManterMensagemGuia().ObterPorId(int.Parse(Request["Id"]));
                        txtTexto.Text = mensagemGuia.Texto;

                        if (mensagemGuia.HashTags != null)
                        {
                            SetarHashTags(mensagemGuia);

                            divHashTag.Visible = true;
                        }
                        else
                        {
                            divHashTag.Visible = false;
                        }
                    }

                    break;
                case 1:
                    divTexto.Visible = false;
                    txtTexto.Text = "";
                    divTutorial.Visible = true;

                    PreencherTutoriais();
                    break;

                default:
                    break;
            }
        }

        private void SetarHashTags(Dominio.Classes.MensagemGuia mensagemGuia)
        {
            var hashs = mensagemGuia.ObterHashTags();

            if (hashs != null) { 
                
                var hashsObj = hashs.Select(x =>
                new
                {
                    id = x,
                    label = "#" + x
                }).ToList();

                var hashsString = new JavaScriptSerializer().Serialize(hashsObj);

                HashTagsJson = hashsString;
            }
        }
      
    }
}