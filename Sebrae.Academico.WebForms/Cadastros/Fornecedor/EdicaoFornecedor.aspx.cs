using System;
using System.Web.UI;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.WebForms.Cadastros
{
    public partial class EdicaoFornecedor : Page
    {
        private Fornecedor _fornecedor;
        private ManterFornecedor manterFornecedor = new ManterFornecedor();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.PreencherCombos();

                if (Request["Id"] != null)
                {
                    int idFornecedor = int.Parse(Request["Id"]);
                    _fornecedor = manterFornecedor.ObterFornecedorPorID(idFornecedor);
                    PreencherCampos(_fornecedor);

                }
                else
                {
                    MostrarCampoSenha();
                }
                txtNome.Focus();

                TratarVisibilidadeNomeApresentacao();
            }

        }

        private void MostrarCampoSenha()
        {
            btnAlterarSenha.Visible = false;
            txtSenha.Visible = true;
        }

        private void PreencherCombos()
        {
            WebFormHelper.PreencherComponenteComOpcoesSimNao(rblInGestaoSgus);
            WebFormHelper.PreencherComponenteComOpcoesSimNao(rblInCriarOferta);
            WebFormHelper.PreencherComponenteComOpcoesSimNao(rblInCriarTurma);
            WebFormHelper.PreencherComponenteComOpcoesSimNao(rblInApresentarComoFornecedorNoPortal);
            ucUF.PreencherUFs();
            rblInGestaoSgus.SelectedValue = Constantes.SiglaSim;
            rblInCriarOferta.SelectedValue = Constantes.SiglaSim;
            rblInCriarTurma.SelectedValue = Constantes.SiglaSim;
            rblInApresentarComoFornecedorNoPortal.SelectedValue = Constantes.SiglaNao;
        }

        private void PreencherCampos(Fornecedor fornecedor)
        {
            if (fornecedor != null)
            {
                this.txtNome.Text = fornecedor.Nome;
                this.txtLogin.Text = fornecedor.Login;
                this.txtLinkAcesso.Text = fornecedor.LinkAcesso;
                this.txtTextoCriptografia.Text = fornecedor.TextoCriptografia;
                this.rblInGestaoSgus.SelectedValue = fornecedor.PermiteGestaoSGUS ? Constantes.SiglaSim : Constantes.SiglaNao;
                this.rblInCriarOferta.SelectedValue = fornecedor.PermiteCriarOferta ? Constantes.SiglaSim : Constantes.SiglaNao;
                this.rblInCriarTurma.SelectedValue = fornecedor.PermiteCriarTurma ? Constantes.SiglaSim : Constantes.SiglaNao;
                this.rblInApresentarComoFornecedorNoPortal.SelectedValue = fornecedor.ApresentarComoFornecedorNoPortal ? Constantes.SiglaSim : Constantes.SiglaNao;
                txtNomeApresentacao.Text = _fornecedor.NomeApresentacao;
                if (fornecedor.PermiteGestaoSGUS)
                {
                    divInCriarOferta.Visible = true;
                    divInCriarTurma.Visible = true;
                }
                else
                {
                    divInCriarOferta.Visible = false;
                    divInCriarTurma.Visible = false;

                }

                this.ucUF.PreencherUfsFornecedor(fornecedor);
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e){
            try{
                _fornecedor = ObterObjetoFornecedor();
                if (Request["Id"] == null){
                    manterFornecedor.IncluirFornecedor(_fornecedor);
                }else{
                    manterFornecedor.AlterarFornecedor(_fornecedor);
                }
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso !", "ListarFornecedor.aspx");
            }catch (AcademicoException ex){
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }


        protected void btnAlterarSenha_Click(object sender, EventArgs e)
        {
            MostrarCampoSenha();
        }

        private Fornecedor ObterObjetoFornecedor()
        {

            if (Request["Id"] != null)
            {
                _fornecedor = new ManterFornecedor().ObterFornecedorPorID(int.Parse(Request["Id"])); ;
            }
            else
            {
                _fornecedor = new Fornecedor();
            }

            //Nome
            _fornecedor.Nome = txtNome.Text;

            //Link de Acesso
            _fornecedor.LinkAcesso = txtLinkAcesso.Text;

            //Texto de Criptografia
            _fornecedor.TextoCriptografia = txtTextoCriptografia.Text;

            //Login
            _fornecedor.Login = txtLogin.Text;

            //Senha
            if (!string.IsNullOrWhiteSpace(txtSenha.Text))
            {
                _fornecedor.Senha = CriptografiaHelper.Criptografar(txtSenha.Text.Trim());
            }

            //Gestão SGUS ?
            if (rblInGestaoSgus.SelectedItem != null && !string.IsNullOrWhiteSpace(rblInGestaoSgus.SelectedItem.Value))
            {
                if (rblInGestaoSgus.SelectedItem.Value.ToString().Equals(Constantes.SiglaSim))
                {
                    _fornecedor.PermiteGestaoSGUS = true;

                    //Permite Criar Oferta ?
                    if (rblInCriarOferta.SelectedItem != null && !string.IsNullOrWhiteSpace(rblInCriarOferta.SelectedItem.Value))
                    {
                        _fornecedor.PermiteCriarOferta = rblInCriarOferta.SelectedItem.Value.ToUpper().Equals(Constantes.SiglaSim);
                    }
                    //Permite Criar Turma ?
                    if (rblInCriarTurma.SelectedItem != null && !string.IsNullOrWhiteSpace(rblInCriarTurma.SelectedItem.Value))
                    {
                        _fornecedor.PermiteCriarTurma = rblInCriarTurma.SelectedItem.Value.ToUpper().Equals(Constantes.SiglaSim);
                    }
                }
                else
                {
                    _fornecedor.PermiteGestaoSGUS = false;
                    _fornecedor.PermiteCriarTurma = false;
                    _fornecedor.PermiteCriarOferta = false;
                }
            }
            else
            {
                _fornecedor.PermiteGestaoSGUS = false;
                _fornecedor.PermiteCriarTurma = false;
                _fornecedor.PermiteCriarOferta = false;
            }

            //Permite Criar Turma ?
            if (rblInApresentarComoFornecedorNoPortal.SelectedItem != null && !string.IsNullOrWhiteSpace(rblInApresentarComoFornecedorNoPortal.SelectedItem.Value)){
                _fornecedor.ApresentarComoFornecedorNoPortal = rblInApresentarComoFornecedorNoPortal.SelectedItem.Value.ToUpper().Equals(Constantes.SiglaSim);
            }

            if (_fornecedor.ApresentarComoFornecedorNoPortal && string.IsNullOrEmpty(txtNomeApresentacao.Text)) {
                throw new AcademicoException("Informe o Nome da Instituição para Apresentação no Portal");
            }

            _fornecedor.NomeApresentacao = txtNomeApresentacao.Text;

            this.incluirUfs(_fornecedor);

            return _fornecedor;
        }

        private void incluirUfs(Fornecedor fornecedor)
        {
           var manterFornecedor = new ManterFornecedor();
           manterFornecedor.IncluirUfs(fornecedor, ucUF.IdsUfsMarcados);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListarFornecedor.aspx");
        }

        private void TratarVisibilidadeNomeApresentacao() {
            divNomeApresentacao.Visible = rblInApresentarComoFornecedorNoPortal.SelectedValue.ToUpper().Equals(Constantes.SiglaSim);
        }

        protected void rblInApresentarComoFornecedorNoPortal_SelectedIndexChanged(object sender, EventArgs e) {
            TratarVisibilidadeNomeApresentacao();
        }

        protected void rblInGestaoSgus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblInGestaoSgus.SelectedValue.ToUpper().Equals(Constantes.SiglaSim))
            {
                divInCriarTurma.Visible = true;
                divInCriarOferta.Visible = true;
            }
            else
            {
                divInCriarTurma.Visible = false;
                divInCriarOferta.Visible = false;
            }
        }


    }
}