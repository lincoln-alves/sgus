using System;
using System.Web.UI;
using Sebrae.Academico.BP;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using classes = Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.WebForms.Cadastros
{
    public partial class EdicaoObjetivo : Page
    {
        private classes.Objetivo _objetivoEdicao;

        private ManterObjetivo manterObjetivo = new ManterObjetivo();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request["Id"] != null)
                {
                    int idObjetivo = int.Parse(Request["Id"].ToString());
                    _objetivoEdicao = manterObjetivo.ObterObjetivoPorID(idObjetivo);
                    PreencherCampos(_objetivoEdicao);
                }
            }
        }


        private void PreencherCampos(classes.Objetivo Objetivo)
        {
            if (Objetivo != null)
            {
                txtObjetivo.Text = Objetivo.Nome;
                txtChaveExterna.Text = Objetivo.ChaveExterna;
            }
        }


        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    _objetivoEdicao = new classes.Objetivo();
                    _objetivoEdicao = ObterObjetoObjetivo();

                    ValidarCampos(_objetivoEdicao);

                    if (Request["Id"] == null)
                    {
                        manterObjetivo.IncluirObjetivo(_objetivoEdicao);
                    }
                    else
                    {
                        manterObjetivo.AlterarObjetivo(_objetivoEdicao);
                    }

                    //Session.Remove("ObjetivoEdit");

                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso !", "ListarObjetivo.aspx");

                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }
        }

        private void ValidarCampos(classes.Objetivo objetivoEdicao)
        {
            if (string.IsNullOrWhiteSpace(txtObjetivo.Text))
                throw new AcademicoException("O campo Objetivo é obrigatório");

            if (txtChaveExterna.Text.Trim() != "") {
                if (!manterObjetivo.ChaveExternaUnica(txtChaveExterna.Text.Trim(), objetivoEdicao.ID)) { 
                    throw new AcademicoException("Chave externa já em uso. Por favor escolher outra.");
                }
            }
        }

        private classes.Objetivo ObterObjetoObjetivo()
        {


            if (Request["Id"] != null)
            {
                _objetivoEdicao = manterObjetivo.ObterObjetivoPorID(int.Parse(Request["Id"]));
            }
            else
            {
                _objetivoEdicao = new classes.Objetivo();
            }

            _objetivoEdicao.Nome = txtObjetivo.Text.Trim();
            _objetivoEdicao.ChaveExterna = txtChaveExterna.Text.Trim();

            return _objetivoEdicao;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Session.Remove("ObjetivoEdit");
            Response.Redirect("ListarObjetivo.aspx");
        }
    }
}