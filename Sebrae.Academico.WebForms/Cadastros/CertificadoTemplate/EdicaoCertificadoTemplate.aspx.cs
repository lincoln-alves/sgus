using System;
using System.Linq;
using System.Web.UI;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using classes = Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.WebForms.Cadastros
{
    public partial class EdicaoCertificadoTemplate : Page
    {
        private readonly ManterUsuario _manterUsuario = new ManterUsuario();
        private classes.CertificadoTemplate _certificadoTemplate;
        private ManterCertificadoTemplate _manterCertificadoTemplate = new ManterCertificadoTemplate();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                PreencherCombos();

                if (Request["Id"] != null)
                {
                    int idCertificadoTemplate = int.Parse(Request["Id"]);
                    _certificadoTemplate = _manterCertificadoTemplate.ObterCertificadoTemplatePorID(idCertificadoTemplate);
                    PreencherCampos(_certificadoTemplate);

                    DesabilitaSeVisualizar();

                    if (Request["C"] != null) btnVisualizar.Visible = false;
                }
                else
                {
                    var usuario = _manterUsuario.ObterUsuarioLogado();
                    ucCategorias1.PreencherCategorias(false, null, usuario, false, false, true);
                    btnVisualizar.Visible = false;
                }
            }
        }

        private void PreencherCombos()
        {
            WebFormHelper.PreencherComponenteComOpcoesSimNao(rblAtivo, false);
        }

        private void PreencherCampos(classes.CertificadoTemplate certificadoTemplate)
        {
            if (certificadoTemplate != null)
            {

                //Nome
                if (!string.IsNullOrWhiteSpace(certificadoTemplate.Nome))
                {
                    txtNome.Text = certificadoTemplate.Nome;
                }

                //Texto do Certificado
                if (!string.IsNullOrWhiteSpace(certificadoTemplate.TextoDoCertificado))
                    txtTexto.Text = certificadoTemplate.TextoDoCertificado;

                if (!string.IsNullOrWhiteSpace(certificadoTemplate.TextoCertificado2))
                    txtTextoCertificado2.Text = certificadoTemplate.TextoCertificado2;

                //Imagem
                if (!string.IsNullOrEmpty(certificadoTemplate.Imagem))
                {
                    imgImagem1.Src = certificadoTemplate.Imagem;
                }
                else
                {
                    imgImagem1.Visible = false;
                }

                if (!string.IsNullOrEmpty(certificadoTemplate.Imagem2))
                {
                    imgImagem2.Src = certificadoTemplate.Imagem2;
                }
                else
                {
                    imgImagem2.Visible = false;
                }

                var usuario = _manterUsuario.ObterUsuarioLogado();

                ucCategorias1.PreencherCategorias(false,
                    certificadoTemplate.ListaCategoriaConteudo.Any()
                        ? certificadoTemplate.ListaCategoriaConteudo.Select(x => x.ID).ToList()
                        : null, usuario, false, false, true);

                ViewState.Add("idcertificadoTemplate", certificadoTemplate.ID);

                if (certificadoTemplate.Professor)
                    rblInProfessor.SelectedValue = "Professor";
                else
                    rblInProfessor.SelectedValue = "Aluno";

                rblAtivo.SelectedValue = certificadoTemplate.Ativo ? "S" : "N";

                // Certificado de Trilhas
                chkCertificadoTrilhas.Checked = certificadoTemplate.CertificadoTrilhas == true;
                divVerso.Visible = certificadoTemplate.CertificadoTrilhas != true;
            }
            else
            {
                var usuario = _manterUsuario.ObterUsuarioLogado();
                ucCategorias1.PreencherCategorias(false, null, usuario);

            }
        }

        private void DesabilitaSeVisualizar()
        {
            if (ExibirSoVisualizacao())
                DesabilitaEdicao();
        }

        private bool ExibirSoVisualizacao()
        {
            var usuarioLogado = _manterUsuario.ObterUsuarioLogado();



            if (usuarioLogado != null)
            {
                if (!usuarioLogado.IsAdministrador())
                {
                    if (Request["C"] != null)
                    {
                        if (Request["C"] == "S" && !usuarioLogado.IsGestor())
                            return true;
                    }
                    else
                    {
                        if (usuarioLogado.UF.ID != _certificadoTemplate.UF.ID)
                            return true;
                    }
                }

            }

            if (Request["V"] != null)
            {
                if (Request["V"] == "S")
                    return true;
            }

            return false;
        }

        private void DesabilitaEdicao()
        {
            pnlConteudoGeral.Enabled = false;
            divCategoriaMoodle.EnableViewState = false;
            ucCategorias1.Enabled = false;
            fupldArquivoEnvio1.Enabled = false;
            fupldArquivoEnvio2.Enabled = false;
            btnSalvar.Visible = false;

            string script = "$(document).ready(function (){$('.fake-img').off('click');$('.fake-img, .action-label').css('cursor', 'not-allowed');});";

            ScriptManager.RegisterStartupScript(this, typeof(Page), "disableClickFakeImagens", script, true);
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                _manterCertificadoTemplate = new ManterCertificadoTemplate();


                if (Request["Id"] == null ||
                    Request["C"] != null)
                {
                    _certificadoTemplate = ObterObjetoCertificadoTemplate(true);
                    _manterCertificadoTemplate.IncluirCertificadoTemplate(_certificadoTemplate);
                }
                else
                {
                    _certificadoTemplate = this.ObterObjetoCertificadoTemplate(false);
                    _manterCertificadoTemplate.AlterarCertificadoTemplate(_certificadoTemplate);
                }

                Session.Remove("CertificadoTemplateEdit");
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                return;
            }

            WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso !", "ListarCertificadoTemplate.aspx");

        }

        private void ValidarTemplate()
        {
            if (!ucCategorias1.IdsCategoriasMarcadas.Any())
            {
                throw new AcademicoException("Selecione ao menos uma categoria de conteúdo.");
            }
        }

        private classes.CertificadoTemplate ObterObjetoCertificadoTemplate(bool criaNovoObjeto)
        {

            if (Request["Id"] != null && !criaNovoObjeto)
            {
                _certificadoTemplate = new ManterCertificadoTemplate().ObterCertificadoTemplatePorID((int.Parse(Request["Id"])));
            }
            else
            {
                _certificadoTemplate = new classes.CertificadoTemplate();

                var usuarioLogado = _manterUsuario.ObterUsuarioLogado();

                _certificadoTemplate.UF = new ManterUf().ObterUfPorID(usuarioLogado.UF.ID);
            }

            if (Request["Id"] != null)
            {
                var tempCertificadoTemplate = new ManterCertificadoTemplate().ObterCertificadoTemplatePorID((int.Parse(Request["Id"])));

                if (!string.IsNullOrEmpty(tempCertificadoTemplate.Imagem))
                    _certificadoTemplate.Imagem = tempCertificadoTemplate.Imagem;

                if (!string.IsNullOrEmpty(tempCertificadoTemplate.Imagem2))
                    _certificadoTemplate.Imagem2 = tempCertificadoTemplate.Imagem2;
            }

            ValidarTemplate();

            //Nome
            _certificadoTemplate.Nome = txtNome.Text;

            //Texto
            _certificadoTemplate.TextoDoCertificado = txtTexto.Text;


            //Obtém a Imagem salva
            if (chkbExcluir1.Checked)
            {
                _certificadoTemplate.Imagem = string.Empty;
            }
            if (fupldArquivoEnvio1 != null && fupldArquivoEnvio1.PostedFile != null && fupldArquivoEnvio1.PostedFile.ContentLength > 0)
            {
                try
                {
                    var imagem = fupldArquivoEnvio1.PostedFile.InputStream;
                    string imagemConvertidaEmBase64String = CommonHelper.ObterBase64String(imagem);
                    string informacoesDoArquivoParaBase64 = CommonHelper.GerarInformacoesDoArquivoParaBase64(fupldArquivoEnvio1);
                    _certificadoTemplate.Imagem = string.Concat(informacoesDoArquivoParaBase64, ",", imagemConvertidaEmBase64String);
                }
                catch (AcademicoException ex)
                {
                    throw ex;
                }
                catch
                {
                    //Todo: -> Logar erro
                    throw new AcademicoException("Ocorreu um Erro ao Salvar o arquivo");
                }
            }

            //Obtém a segunda Imagem
            if (chkbExcluir2.Checked)
            {
                _certificadoTemplate.Imagem2 = string.Empty;
            }
            if (fupldArquivoEnvio2 != null && fupldArquivoEnvio2.PostedFile != null && fupldArquivoEnvio2.PostedFile.ContentLength > 0)
            {
                try
                {
                    var imagem = fupldArquivoEnvio2.PostedFile.InputStream;
                    string imagemConvertidaEmBase64String = CommonHelper.ObterBase64String(imagem);
                    string informacoesDoArquivoParaBase64 = CommonHelper.GerarInformacoesDoArquivoParaBase64(fupldArquivoEnvio2);
                    _certificadoTemplate.Imagem2 = string.Concat(informacoesDoArquivoParaBase64, ",", imagemConvertidaEmBase64String);
                }
                catch (AcademicoException ex)
                {
                    throw ex;
                }
                catch
                {
                    //Todo: -> Logar erro
                    throw new AcademicoException("Ocorreu um Erro ao Salvar o arquivo");
                }
            }

            //Certificado de trilhas
            _certificadoTemplate.CertificadoTrilhas = chkCertificadoTrilhas.Checked;

            //Texto do Certificado
            _certificadoTemplate.TextoCertificado2 = txtTextoCertificado2.Text;

            var manterCategoriaConteudo = new ManterCategoriaConteudo();
            //Categorias
            _certificadoTemplate.ListaCategoriaConteudo = ucCategorias1.IdsCategoriasMarcadas.Select(id => manterCategoriaConteudo.ObterCategoriaConteudoPorID(id)).ToList();

            if (rblInProfessor.SelectedValue == "Professor")
                _certificadoTemplate.Professor = true;
            else
                _certificadoTemplate.Professor = false;

            if ((rblAtivo.SelectedItem != null && !string.IsNullOrWhiteSpace(rblAtivo.SelectedItem.Value)))
            {
                _certificadoTemplate.Ativo = rblAtivo.SelectedValue == "S" ? true : false;
            }


            return _certificadoTemplate;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Session.Remove("CertificadoTemplateEdit");
            Response.Redirect("ListarCertificadoTemplate.aspx");
        }

        protected void btnVisualizar_Click(object sender, EventArgs e)
        {
            var idCertificado = int.Parse(Request["Id"]);

            ucMostraPreviaRel.Src = "~/Cadastros/CertificadoTemplate/VisualizaCertificadoTemplate.aspx?Id=" + idCertificado;
            ucMostraPreviaRel.Abre(idCertificado);
        }

        protected void chkCertificadoTrilhas_OnCheckedChanged(object sender, EventArgs e)
        {
            // Esconde ou exibe o verso do certificado.
            divVerso.Visible = !chkCertificadoTrilhas.Checked;
        }
    }
}