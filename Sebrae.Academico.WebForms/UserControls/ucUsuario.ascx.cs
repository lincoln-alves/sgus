using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucUsuario : UserControl
    {


        public Usuario ObterObjetoUsuario(Usuario usuario)
        {
            if (usuario == null)
                usuario = new Usuario();

            usuario.AnoConclusao = txtAnoConclusao.Text;
            usuario.Bairro = txtBairro.Text;
            usuario.Cep = txtCEP.Text;
            usuario.Cidade = txtCidade.Text;
            usuario.Complemento = txtComplemento.Text;
            //usuario.Bairro2 = txtBairro2.Text;
            //usuario.Cep2 = txtCEP2.Text;
            //usuario.Cidade2 = txtCidade2.Text;
            //usuario.Complemento2 = txtComplemento2.Text;

            if (!string.IsNullOrWhiteSpace(txtCPF.Text))
            {
                usuario.CPF = txtCPF.Text.Trim().Replace(".", string.Empty).Replace("_", string.Empty).Replace("-", string.Empty);
            }

            usuario.Auditoria = new Auditoria(null);
            usuario.Email = txtEmail.Text;
            usuario.Endereco = txtEndereco.Text;
            usuario.Escolaridade = txtEscolaridade.Text;
            usuario.Estado = txtEstado.Text;
            usuario.EstadoCivil = txtEstadoCivil.Text;
            usuario.Instituicao = txtInstituicao.Text;
            usuario.Matricula = txtMatricula.Text;
            usuario.MiniCurriculo = txtMiniCurriculum.Text;
            usuario.Nacionalidade = txtNacionalidade.Text;
            usuario.Naturalidade = txtNaturalidade.Text;
            usuario.NivelOcupacional = new ManterNivelOcupacional().ObterNivelOcupacionalPorID(int.Parse(ddlNivelOcupacional.SelectedValue));
            usuario.Nome = txtNome.Text;
            usuario.NomeMae = txtNomeMae.Text;
            usuario.NomePai = txtNomePai.Text;
            usuario.NumeroIdentidade = txtNumeroIdentidade.Text;
            usuario.OrgaoEmissor = txtOrgaoEmissor.Text;
            usuario.Pais = txtPais.Text;
            //usuario.Endereco2 = txtEndereco2.Text;
            //usuario.Estado2 = txtEstado2.Text;
            //usuario.Pais2 = txtPais2.Text;
            usuario.Sexo = ddlSexo.Text;
            
            if (ddlStatus.SelectedIndex > 0)
            {
                if(ddlStatus.SelectedValue.ToLower() != usuario.Situacao)
                    usuario.Situacao = ddlStatus.SelectedValue.ToLower();
            }

            if (ddlSituacao.SelectedIndex > 0)
            {
                if (ddlStatus.SelectedValue.ToLower() == "ativo" && ddlSituacao.SelectedValue.ToLower() != "ativo")
                    usuario.Situacao = ddlStatus.SelectedValue.ToLower();
                else
                    if (ddlStatus.SelectedValue.ToLower() != "ativo")
                        usuario.Situacao = ddlSituacao.SelectedValue.ToLower();
            }

            usuario.TelCelular = txtTelCelular.Text;
            usuario.TelResidencial = txtTelResidencial.Text;
            usuario.TipoDocumento = txtTipoDocumento.Text;
            usuario.TipoInstituicao = txtTipoInstituicao.Text;
            usuario.UF = new ManterUf().ObterUfPorID(int.Parse(ddlUF.SelectedValue));
            usuario.Unidade = txtUnidade.Text;

            //Campos Editaveis            
            //if (!string.IsNullOrEmpty(txtSenha.Text))
            //{
            //    usuario.Senha = CriptografiaHelper.Criptografar(txtSenha.Text);
            //}

            //Campos Editaveis
            if (!string.IsNullOrWhiteSpace(txtRamalExibicao.Text))
            {
                usuario.RamalExibicao = txtRamalExibicao.Text;
            }

            if (!string.IsNullOrWhiteSpace(txtTelefoneExibicao.Text))
            {
                usuario.TelefoneExibicao = txtTelefoneExibicao.Text;
            }

            if (!string.IsNullOrWhiteSpace(txtTipoTelefoneExibicao.Text))
            {
                usuario.TipoTelefoneExibicao = txtTipoTelefoneExibicao.Text;
            }

            if (!string.IsNullOrWhiteSpace(txtNomeExibicao.Text))
            {
                usuario.NomeExibicao = txtNomeExibicao.Text;
            }

            if (!string.IsNullOrWhiteSpace(txtSID.Text))
            {
                usuario.SID_Usuario = txtSID.Text;
            }

            //if (ddlMaterialDidatico.SelectedItem != null && !string.IsNullOrWhiteSpace(ddlMaterialDidatico.SelectedItem.Value))
            //{
            //    usuario.MaterialDidatico = ddlMaterialDidatico.SelectedItem.Value;
            //}

            if (!string.IsNullOrWhiteSpace(txtDataAdmissao.Text))
                usuario.DataAdmissao = DateTime.ParseExact(txtDataAdmissao.Text, "dd/MM/yyyy", null);

            if (!string.IsNullOrWhiteSpace(txtDataExpedicaoIdentidade.Text))
                usuario.DataExpedicaoIdentidade = DateTime.ParseExact(txtDataExpedicaoIdentidade.Text, "dd/MM/yyyy", null);

            if (!string.IsNullOrWhiteSpace(txtDataNascimento.Text))
                usuario.DataNascimento = DateTime.ParseExact(txtDataNascimento.Text, "dd/MM/yyyy", null);


            int i = 0;

            for (i = 0; i < chkPerfil.Items.Count; i++)
            {
                if (string.IsNullOrEmpty(chkPerfil.Items[i].Value))
                    continue;

                Perfil p = new ManterPerfil().ObterPerfilPorID(int.Parse(chkPerfil.Items[i].Value));

                if (chkPerfil.Items[i].Selected)
                {

                    //Antes de adicionar, verifica se já existe
                    if (!usuario.ListaPerfil.Any(x => x.Perfil.ID == p.ID))
                    {
                        usuario.ListaPerfil.Add(new UsuarioPerfil()
                        {
                            Perfil = p,
                            Usuario = usuario,
                            Auditoria = new Auditoria(null)
                        });
                    }
                }
                else
                {
                    UsuarioPerfil usuarioPefilASerExcluido = usuario.ListaPerfil.FirstOrDefault(x => x.Perfil.ID == p.ID);
                    usuario.ListaPerfil.Remove(usuarioPefilASerExcluido);
                }
            }

            this.AdicionarOuRemoverTags(usuario);

            //Imagem enviada

            if (fupldArquivoEnvio != null && fupldArquivoEnvio.PostedFile != null && fupldArquivoEnvio.PostedFile.ContentLength > 0)
            {

                try
                {

                    string caminhoDiretorioUpload = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.RepositorioUpload).Registro;
                    // WebFormHelper.ObterCaminhoFisicoDoDiretorioDeUpload();
                    string nomeAleatorioDoArquivoParaUploadCriptografado = WebFormHelper.ObterStringAleatoria();
                    string diretorioDeUploadComArquivo = string.Concat(caminhoDiretorioUpload, @"\", nomeAleatorioDoArquivoParaUploadCriptografado);

                    fupldArquivoEnvio.PostedFile.SaveAs(diretorioDeUploadComArquivo);

                    usuario.FileServer = new FileServer();
                    usuario.FileServer.NomeDoArquivoNoServidor = nomeAleatorioDoArquivoParaUploadCriptografado;
                    usuario.FileServer.NomeDoArquivoOriginal = fupldArquivoEnvio.FileName;
                    usuario.FileServer.TipoArquivo = fupldArquivoEnvio.PostedFile.ContentType;
                    usuario.FileServer.MediaServer = true;

                }
                catch (AcademicoException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    ErroUtil.Instancia.TratarErro(ex);
                }

            }

            return usuario;


        }

        public HttpPostedFile ImagemEnviada { get; set; }

        /// <summary>
        /// Imagem Enviada.
        /// </summary>
        public string Imagem
        {
            get
            {
                if (ViewState["ViewStateImagem"] != null)
                {
                    return (string)ViewState["ViewStateImagem"];
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState["ViewStateImagem"] = value;
            }

        }
        public void PreencherListas()
        {
            try
            {
                PreencherListaUf();
                PreencherListaNivelOcupacional();
                PreencherListaComboTodosPerfis();
                this.ucTags1.PreencherTags();
                //PreencherListaTag(usuario);
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }

        }

        public void PreencherCampos(Usuario usuario)
        {

            WebFormHelper.PreencherComponenteComOpcoesSexo(ddlSexo, true);
            this.PreencherListaUf();

            //Imagem
            //this.ucUpload1.PrepararExibicaoDaImagemSalva(user.Imagem);

            txtAnoConclusao.Text = usuario.AnoConclusao == null ? string.Empty : usuario.AnoConclusao.ToString();
            txtBairro.Text = usuario.Bairro;
            txtCEP.Text = usuario.Cep;
            txtCidade.Text = usuario.Cidade;
            txtComplemento.Text = usuario.Complemento;
            txtCPF.Text = usuario.CPF;

            txtSID.Text = usuario.SID_Usuario;
            txtDataAdmissao.Text = usuario.DataAdmissao == null ? string.Empty : usuario.DataAdmissao.Value.ToString("dd/MM/yyyy");
            txtDataExpedicaoIdentidade.Text = usuario.DataExpedicaoIdentidade == null ? string.Empty : usuario.DataExpedicaoIdentidade.Value.ToString("dd/MM/yyyy");
            txtDataNascimento.Text = usuario.DataNascimento == null ? string.Empty : usuario.DataNascimento.Value.ToString("dd/MM/yyyy");
            txtEmail.Text = usuario.Email;
            txtEndereco.Text = usuario.Endereco;

            txtEscolaridade.Text = usuario.Escolaridade;
            txtEstado.Text = usuario.Estado;

            txtEstadoCivil.Text = usuario.EstadoCivil;
            txtInstituicao.Text = usuario.Instituicao;

            //ddlMaterialDidatico.ClearSelection();
            //WebFormHelper.SetarValorNaCombo(usuario.MaterialDidatico, ddlMaterialDidatico);

            txtMatricula.Text = usuario.Matricula;
            txtNacionalidade.Text = usuario.Nacionalidade;
            txtNaturalidade.Text = usuario.Naturalidade;

            ddlNivelOcupacional.ClearSelection();
            WebFormHelper.SetarValorNaCombo(usuario.NivelOcupacional.ID.ToString(), ddlNivelOcupacional);

            txtNome.Text = usuario.Nome;
            txtNomeMae.Text = usuario.NomeMae;
            txtNomePai.Text = usuario.NomePai;
            txtNumeroIdentidade.Text = usuario.NumeroIdentidade;
            txtOrgaoEmissor.Text = usuario.OrgaoEmissor;
            txtPais.Text = usuario.Pais;
            //txtPais2.Text = usuario.Pais2;
            //txtEndereco2.Text = usuario.Endereco2;
            //txtEstado2.Text = usuario.Estado2;
            //txtBairro2.Text = usuario.Bairro2;
            //txtCEP2.Text = usuario.Cep2;
            //txtCidade2.Text = usuario.Cidade2;
            //txtComplemento2.Text = usuario.Complemento2;

            ddlSexo.ClearSelection();
            WebFormHelper.SetarValorNaCombo(usuario.Sexo, ddlSexo);


            ddlStatus.SelectedValue = usuario.Situacao?.ToLower().Trim() == "ativo" ? "ativo" : "inativo";
            ddlSituacao.SelectedValue = usuario.Situacao?.ToLower().Trim();
            
            txtTelCelular.Text = usuario.TelCelular;
            txtTelResidencial.Text = usuario.TelResidencial;
            txtTipoDocumento.Text = usuario.TipoDocumento;
            txtTipoInstituicao.Text = usuario.TipoInstituicao;

            ddlUF.ClearSelection();
            WebFormHelper.SetarValorNaCombo(usuario.UF.ID.ToString(), ddlUF);

            txtUnidade.Text = usuario.Unidade;
            txtMiniCurriculum.Text = usuario.MiniCurriculo;

            txtRamalExibicao.Text = usuario.RamalExibicao;
            txtTelefoneExibicao.Text = usuario.TelefoneExibicao;
            txtTipoTelefoneExibicao.Text = usuario.TipoTelefoneExibicao;
            txtNomeExibicao.Text = usuario.NomeExibicao;
            //txtSituacao.Text = usuario.Situacao;
            //txtSituacao.ReadOnly = true;



            this.PreencherListaTag(usuario);
            this.PreencherListaComPerfisDoUsuario(usuario);

            //if (!string.IsNullOrEmpty(usuario.Senha))
            //{
            //    txtSenha.Text = CriptografiaHelper.Decriptografar(usuario.Senha);
            //}

            if (usuario.FileServer != null && usuario.FileServer.ID > 0)
            {
                this.imgImagem.Src = "/MediaServer.ashx?Identificador=" + usuario.FileServer.ID;
                this.imgImagem.Visible = true;
            }
            else
            {
                this.imgImagem.Src = "";
                this.imgImagem.Visible = false;
            }

        }

        #region "Métodos Privados"

        private void AdicionarOuRemoverTags(Usuario usuario)
        {
            this.ucTags1.ObterInformacoesSobreAsTags();
            this.ucTags1.TagsSelecionadas.ForEach(x => usuario.AdicionarTag(x));
            this.ucTags1.TagsNaoSelecionadas.ForEach(x => usuario.RemoverTag(x));
        }

        private void PreencherListaUf()
        {
            WebFormHelper.PreencherLista(new ManterUf().ObterTodosUf(), ddlUF, false, true);
        }

        private void PreencherListaNivelOcupacional()
        {
            WebFormHelper.PreencherLista(new ManterNivelOcupacional().ObterTodosNivelOcupacional(), ddlNivelOcupacional, false, true);
        }

        public void PreencherListaTag(Usuario usuario)
        {
            IList<Tag> ListaTags = usuario.ListaTag.Where(x => x.Tag != null)
                    .Select(x => new Tag() { ID = x.Tag.ID, Nome = x.Tag.Nome }).ToList<Tag>();

            this.ucTags1.PreencherListViewComTagsGravadosNoBanco(ListaTags);
        }

        private void PreencherListaComboTodosPerfis()
        {
            IList<Perfil> listaPerfis;
            using (ManterPerfil manterPerfil = new ManterPerfil())
            {
                listaPerfis = manterPerfil.ObterTodosPerfis();

                var usuarioLogado = (new ManterUsuario()).ObterUsuarioLogado();
                if (!(usuarioLogado.IsAdministrador() && usuarioLogado.IsNacional()))
                {
                    List<int> filtroPerfis = new ManterPerfil().ObterTodosPerfis().Where(p => p.ID == (int)enumPerfil.Administrador ||
                    p.ID == (int)enumPerfil.AdministradorPortal ||
                    p.ID == (int)enumPerfil.AdministradorTrilha ||
                    p.ID == (int)enumPerfil.MonitorTrilha ||
                    p.ID == (int)enumPerfil.Orientador).Select(p => p.ID).ToList();

                    listaPerfis = listaPerfis.Where(p => !filtroPerfis.Contains(p.ID)).ToList();
                }

                if (!usuarioLogado.IsGestorContrato())
                {
                    List<int> filtroPerfis = new ManterPerfil().ObterTodosPerfis().Where(p => p.ID == (int)enumPerfil.GestorContrato).Select(p => p.ID).ToList();

                    listaPerfis = listaPerfis.Where(p => !filtroPerfis.Contains(p.ID)).ToList();
                }

                WebFormHelper.PreencherLista(listaPerfis, chkPerfil);
            }

        }

        public void PreencherListaComPerfisDoUsuario(Usuario usuario)
        {
            this.PreencherListViewComPerfisGravadosNoBanco(usuario.ListaPerfil);
        }

        private void PreencherListViewComPerfisGravadosNoBanco(IList<UsuarioPerfil> ListaUsuarioPerfil)
        {
            if (ListaUsuarioPerfil != null && ListaUsuarioPerfil.Count > 0)
            {

                IList<Perfil> ListaPerfis = ListaUsuarioPerfil.Where(x => x.Perfil != null)
                   .Select(x => new Perfil() { ID = x.Perfil.ID, Nome = x.Perfil.Nome }).ToList<Perfil>();

                for (int i = 0; i < chkPerfil.Items.Count; i++)
                {
                    if (string.IsNullOrEmpty(chkPerfil.Items[i].Value.ToString()))
                        continue;

                    bool perfilFoiEscolhido = this.VerificarSeOPerfilFoiEscolhido(ListaPerfis, int.Parse(chkPerfil.Items[i].Value));
                    chkPerfil.Items[i].Selected = perfilFoiEscolhido;
                }
            }
        }

        private bool VerificarSeOPerfilFoiEscolhido(IList<Perfil> ListaPerfil, int IDPerfil)
        {
            return ListaPerfil.Where(x => x.ID == IDPerfil).Any();
        }


        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Bloqueia campos caso o usuário não tenha o perfil administrador.
                if (!new BMUsuario().PerfilAdministrador() && !Page.AppRelativeVirtualPath.Contains("InclusaoUsuario.aspx"))
                {
                    txtCPF.Enabled = false;
                    txtSID.Enabled = false;
                    ddlStatus.Enabled = false;
                }
                //WebFormHelper.PreencherComponenteComOpcoesSimNao(this.ddlMaterialDidatico, true);
            }

            PerfilAdministrador();
        }

        protected void PerfilAdministrador()
        {
            //if (!new BMUsuario().PerfilAdministrador())
            //{
            //    for (int a = 0; a < chkPerfil.Items.Count; a++)
            //    {
            //        chkPerfil.Items[a].Enabled = false;
            //    }

            //    TreeView trvTags = (TreeView)this.ucTags1.FindControl("trvTags");
            //    trvTags.Enabled = false;
            //}
        }
    }
}