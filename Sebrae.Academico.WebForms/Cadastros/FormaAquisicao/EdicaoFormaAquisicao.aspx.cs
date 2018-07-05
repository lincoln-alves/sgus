using System;
using System.Web.UI;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BP;
using classes = Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.WebForms.Cadastros.FormaAquisicao
{
    public partial class EdicaoFormaAquisicao : Page
    {
        private classes.FormaAquisicao _formaAquisicaoEdicao = null;
        private ManterFormaAquisicao manterTrilhaFormaAquisicao = new ManterFormaAquisicao();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                PreencherDrops();

                if (Request["Id"] != null)
                {
                    int idFormaAquisicao = int.Parse(Request["Id"].ToString());
                    _formaAquisicaoEdicao = manterTrilhaFormaAquisicao.ObterFormaAquisicaoPorID(idFormaAquisicao);
                    PreencherCampos(_formaAquisicaoEdicao);
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (!UsuarioTemPermissao())
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Usuário não tem permissão para acessar essa página.", "ListarFormaAquisicao.aspx");
        }

        private bool UsuarioTemPermissao()
        {
            var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

            if (usuarioLogado != null)
            {
                if (usuarioLogado.IsGestor() && _formaAquisicaoEdicao != null && _formaAquisicaoEdicao.Uf != null)
                    if (usuarioLogado.UF.ID != _formaAquisicaoEdicao.Uf.ID)
                        return false;
            }

            return true;
        }

        private void PreencherDrops()
        {
            WebFormHelper.PreencherComponenteComTipoFormaDeAquisicao(ddlTipoFormaAquisicao);
        }

        private void PreencherCampos(classes.FormaAquisicao formaAquisicao)
        {
            if (formaAquisicao != null)
            {
                txtNome.Text = formaAquisicao.Nome;
                txtDescricao.Text = formaAquisicao.Descricao;
                cbEnviarPortal.Checked = formaAquisicao.EnviarPortal;
                cbPresencial.Checked = formaAquisicao.Presencial;
                cbPermiteAlterarCargaHoraria.Checked = formaAquisicao.PermiteAlterarCargaHoraria == true;

                //Carga Horária
                if (formaAquisicao.CargaHoraria.HasValue && formaAquisicao.CargaHoraria.Value > 0) txtCargaHoraria.Text = formaAquisicao.CargaHoraria.Value.ToString();
                //Imagem
                ucUpload1.PrepararExibicaoDaImagemSalva(formaAquisicao.Imagem);

                ddlTipoFormaAquisicao.SelectedValue = formaAquisicao.GetValorFormaDeAquisicao().ToString();

                ValidarVisibilidadeDaCargaHoraria(formaAquisicao.TipoFormaDeAquisicao);
            }
        }

        void ValidarVisibilidadeDaCargaHoraria(enumTipoFormaAquisicao valor)
        {
            if (valor == enumTipoFormaAquisicao.Trilha)
            {
                divCargaHoraria.Visible = true;
                divPermiteAlterarCargaHoraria.Visible = true;
            }
            else
            {
                divCargaHoraria.Visible = false;
                divPermiteAlterarCargaHoraria.Visible = false;
            }
        }

        protected void ddlTipoFormaAquisicao_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidarVisibilidadeDaCargaHoraria((enumTipoFormaAquisicao)int.Parse(ddlTipoFormaAquisicao.SelectedValue));
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                _formaAquisicaoEdicao = null;

                var criaNovoObjeto = Request["C"] == "S";

                _formaAquisicaoEdicao = ObterObjetoFormaAquisicao(criaNovoObjeto);
                if (Request["Id"] == null || criaNovoObjeto)
                {
                    manterTrilhaFormaAquisicao.IncluirFormaAquisicao(_formaAquisicaoEdicao);
                    LimparCampos();
                }
                else
                {
                    manterTrilhaFormaAquisicao.AlterarFormaAquisicao(_formaAquisicaoEdicao);
                }

                //Session.Remove("FormaAquisicaoEdit");

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso !", "ListarFormaAquisicao.aspx");

            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }

        }

        private classes.FormaAquisicao ObterObjetoFormaAquisicao(bool criaNovoObjeto)
        {
            _formaAquisicaoEdicao = null;

            if (Request["Id"] != null && !criaNovoObjeto)
            {
                _formaAquisicaoEdicao = new ManterFormaAquisicao().ObterFormaAquisicaoPorID(int.Parse(Request["Id"]));
            }
            else
            {
                _formaAquisicaoEdicao = new classes.FormaAquisicao();
            }

            _formaAquisicaoEdicao.Nome = txtNome.Text;
            _formaAquisicaoEdicao.Descricao = txtDescricao.Text;
            _formaAquisicaoEdicao.EnviarPortal = cbEnviarPortal.Checked;
            _formaAquisicaoEdicao.Presencial = cbPresencial.Checked;

            var tipo = (enumTipoFormaAquisicao)int.Parse(ddlTipoFormaAquisicao.SelectedValue);

            _formaAquisicaoEdicao.PermiteAlterarCargaHoraria = cbPermiteAlterarCargaHoraria.Checked && tipo == enumTipoFormaAquisicao.Trilha;
            _formaAquisicaoEdicao.TipoFormaDeAquisicao = tipo;

            if (ucUpload1.ArquivoFoiEnviado)
                _formaAquisicaoEdicao.Imagem = ucUpload1.ObterImagemFormatada();


            if (_formaAquisicaoEdicao.TipoFormaDeAquisicao == enumTipoFormaAquisicao.Trilha && string.IsNullOrEmpty(txtCargaHoraria.Text))
            {
                throw new AcademicoException("Informe uma carga horária para a Forma de Aquisição do tipo trilha.");
            }

            if (!string.IsNullOrEmpty(txtCargaHoraria.Text))
            {
                int cargaHoraria;
                if (!int.TryParse(txtCargaHoraria.Text, out cargaHoraria))
                {
                    throw new AcademicoException("Carga Horária deve estar em um formato númerico.");
                }
                _formaAquisicaoEdicao.CargaHoraria = cargaHoraria;
            }

            var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

            if (_formaAquisicaoEdicao.Uf == null)
            {
                _formaAquisicaoEdicao.Uf = (new ManterUf()).ObterUfPorID(usuarioLogado.UF.ID);
            }

            _formaAquisicaoEdicao.Responsavel = usuarioLogado;

            return _formaAquisicaoEdicao;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Request["Id"] = null;
            Response.Redirect("ListarFormaAquisicao.aspx");
        }

        private void LimparCampos()
        {
            txtNome.Text = "";
        }

    }
}