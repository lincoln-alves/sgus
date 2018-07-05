using System;

using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.BP;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.Cadastros
{
    public partial class EdicaoUsuario : System.Web.UI.Page
    {
        Usuario user = new Usuario();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                PreencherListas();
                user = new ManterUsuario().ObterUsuarioPorID(int.Parse(Request["Id"].ToString()));
                PreencherCampos();
            }
       }

        private void PreencherListas()
        {
            this.ucUsuario1.PreencherListas();
        }

        private void PreencherCampos()
        {
            this.ucUsuario1.PreencherCampos(user);

            //lblAnoConclusao.Text = user.AnoConclusao == null ? string.Empty : user.AnoConclusao.ToString();
            //lblBairro.Text = user.Bairro;
            //lblBairro2.Text = user.Bairro2;
            //lblCEP.Text = user.Cep;
            //lblCEP2.Text = user.Cep2;
            //lblCidade.Text = user.Cidade;
            //lblCidade2.Text = user.Cidade2;
            //lblComplemento.Text = user.Complemento;
            //lblComplemento2.Text = user.Complemento2;
            //lblCPF.Text = user.CPF;
            //lblDataAdmissao.Text = user.DataAdmissao == null ? string.Empty : user.DataAdmissao.Value.ToString("dd/MM/yyyy");
            //lblDataExpedicaoIdentidade.Text = user.DataExpedicaoIdentidade == null ? string.Empty : user.DataExpedicaoIdentidade.Value.ToString("dd/MM/yyyy");
            //lblDataNascimento.Text = user.DataNascimento == null ? string.Empty :  user.DataNascimento.Value.ToString("dd/MM/yyyy");
            //lblEmail.Text = user.Email;
            //lblEndereco.Text = user.Endereco;
            //lblEndereco2.Text = user.Endereco2;
            //lblEscolaridade.Text = user.Escolaridade;
            //lblEstado.Text = user.Estado;
            //lblEstado2.Text = user.Estado2;
            //lblEstadoCivil.Text = user.EstadoCivil;
            //lblInstituição.Text = user.Instituicao;
            //lblMaterialDidatico.Text = user.MaterialDidatico;
            //lblMatricula.Text = user.Matricula;
            //lblNacionalidade.Text = user.Nacionalidade;
            //lblNaturalidade.Text = user.Naturalidade;
            //lblNivelOcupacional.Text = user.NivelOcupacional.Nome;
            //lblNome.Text = user.Nome;
            //lblNomeMae.Text = user.NomeMae;
            //lblNomePai.Text = user.NomePai;
            //lblNumeroIdentidade.Text = user.NumeroIdentidade;
            //lblOrgaoEmissor.Text = user.OrgaoEmissor;
            //lblPaís.Text = user.Pais;
            //lblPais2.Text = user.Pais2;
            //lblSexo.Text = user.Sexo;
            //lblSituacao.Text = user.Situacao;
            //lblTelCelular.Text = user.TelCelular;
            //lblTelResidencial.Text = user.TelResidencial;
            //lblTipoDocumento.Text = user.TipoDocumento;
            //lblTipoInstituicao.Text = user.TipoInstituicao;
            //lblUF.Text = user.UF.Nome;
            //lblUnidade.Text = user.Unidade;
            //imgImagem.Src = user.Imagem;
            //lblMinicurriculum.Text = user.MiniCurriculo;

            using (ManterPerfil manterPerfil = new ManterPerfil())
            {
                WebFormHelper.PreencherLista(manterPerfil.ObterTodosPerfis(), chkPerfil);
            }

            using (ManterTag manterTag = new ManterTag())
            {
                WebFormHelper.PreencherLista(manterTag.ObterTodasTag(), chkTags);
            }

            int i;

            for (i = 0; i < chkPerfil.Items.Count; i++)
            {
                chkPerfil.Items[i].Selected = user.ListaPerfil.Any(x => x.Perfil.ID == int.Parse(chkPerfil.Items[i].Value));
            }

            for (i = 0; i < chkTags.Items.Count; i++)
            {
                chkTags.Items[i].Selected = user.ListaTag.Any(x => x.Tag.ID == int.Parse(chkTags.Items[i].Value));
            }


        }



        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //Session.Remove("UsuarioEdit");
            Response.Redirect("ListarUsuario.aspx");
        }

        protected void btnSalvarAlteracaoSenha_Click(object sender, EventArgs e)
        {
            if (!vldSenha.IsValid && !vdlNovaSenha.IsValid)
                return;


            using (ManterUsuario manterUsuario = new ManterUsuario())
            {


                Usuario us = manterUsuario.ObterUsuarioPorID(int.Parse(Request["Id"].ToString()));

                us.Senha = txtNovaSenha.Text;
                us.ConfirmarSenhaLms = txtConfirmarSenha.Text;

                manterUsuario.AlterarSenha(us, 1981245348);
            }

            Session.Remove("UsuarioEdit");

            Response.Redirect("ListarUsuario.aspx");


        }

        protected void btnSalvarPerfil_Click(object sender, EventArgs e)
        {
            using (ManterUsuario manterUsuario = new ManterUsuario())
            {
                user = manterUsuario.ObterUsuarioPorID(int.Parse(Request["Id"].ToString()));

                IList<int> perfilSelecionado = new List<int>();
                IList<int> perfilNaoSelecionado = new List<int>();

                for (int i = 0; i < chkPerfil.Items.Count; i++)
                {
                    if (chkPerfil.Items[i].Selected)
                        perfilSelecionado.Add(int.Parse(chkPerfil.Items[i].Value));
                    else
                        perfilNaoSelecionado.Add(int.Parse(chkPerfil.Items[i].Value));
                }


                foreach (int ns in perfilNaoSelecionado)
                {
                    UsuarioPerfil ur = user.ListaPerfil.Where(x => x.Perfil.ID == ns).FirstOrDefault();
                    user.ListaPerfil.Remove(ur);
                }


                foreach (int s in perfilSelecionado)
                {
                    if (!user.ListaPerfil.Any(x => x.Perfil.ID == s))
                        user.ListaPerfil.Add(new UsuarioPerfil()
                        {
                            Auditoria = new Auditoria(null),
                            Perfil = new ManterPerfil().ObterPerfilPorID(s),
                            Usuario = user
                        });
                }

                manterUsuario.Salvar(user);
                Response.Redirect("ListarUsuario.aspx");
            }
        }

        protected void btnSalvarAlteracaoTag_Click(object sender, EventArgs e)
        {
            using (ManterUsuario manterUsuario = new ManterUsuario())
            {

                user = manterUsuario.ObterUsuarioPorID(int.Parse(Request["Id"].ToString()));

                user = this.ucUsuario1.ObterObjetoUsuario(user);

                IList<int> tagSelecionado = new List<int>();
                IList<int> tagNaoSelecionado = new List<int>();

                for (int i = 0; i < chkTags.Items.Count; i++)
                {
                    if (chkTags.Items[i].Selected)
                        tagSelecionado.Add(int.Parse(chkTags.Items[i].Value));
                    else
                        tagNaoSelecionado.Add(int.Parse(chkTags.Items[i].Value));
                }


                foreach (int ns in tagNaoSelecionado)
                {
                    UsuarioTag ur = user.ListaTag.Where(x => x.Tag.ID == ns).FirstOrDefault();
                    user.ListaTag.Remove(ur);
                }


                foreach (int s in tagSelecionado)
                {
                    if (!user.ListaTag.Any(x => x.Tag.ID == s))
                        user.ListaTag.Add(new UsuarioTag()
                        {
                            Auditoria = new Auditoria(null),
                            Tag = new ManterTag().ObterTagPorID(s),
                            Usuario = user
                        });
                }

                manterUsuario.Salvar(user);
                Response.Redirect("ListarUsuario.aspx");
            }
        }
    }
}