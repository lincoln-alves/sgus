using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using classes = Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BM.Classes.Moodle;
using Sebrae.Academico.Dominio.Classes.Moodle;
using System.Linq;

namespace Sebrae.Academico.WebForms.Cadastros
{
    public partial class ListarProfessor : PageBase
    {

        protected override void OnPreRenderComplete(EventArgs e)
        {
            if (this.dgvProfessor.Rows.Count > 0)
            {
                this.dgvProfessor.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
        

        private ManterProfessor manterProfessor = null;

        protected override enumFuncionalidade Funcionalidade
        {
            get { return enumFuncionalidade.Professor; }
        }

        protected override System.Collections.Generic.IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade
        {
            get { throw new NotImplementedException(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    base.LogarAcessoFuncionalidade();
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private void ExecutarSincronizacao(int idProfessor)
        {
            var professor = new BMProfessor().ObterPorId(idProfessor);

            if (professor != null)
            {
                var bmUsuarioMoodle = new BMUsuarioMoodle();

                string cpf = professor.Cpf;
                string email = professor.Email;

                var usuarioSgus = new BMUsuario().ObterPorCPF(cpf);

                if (usuarioSgus == null)
                {
                    usuarioSgus = new Usuario();
                    usuarioSgus.Auditoria = new Auditoria(new BMUsuario().ObterUsuarioLogado().CPF);
                    usuarioSgus.Bairro = professor.Bairro;
                    usuarioSgus.Cep = professor.CEP;
                    usuarioSgus.Cidade = professor.Cidade;
                    usuarioSgus.CPF = professor.Cpf;
                    usuarioSgus.DataNascimento = professor.DataNascimento;
                    usuarioSgus.Email = professor.Email;
                    usuarioSgus.Endereco = professor.Endereco;
                    usuarioSgus.Estado = professor.Estado;
                    usuarioSgus.EstadoCivil = professor.EstadoCivil;
                    usuarioSgus.Nacionalidade = professor.Nacionalidade;
                    usuarioSgus.Naturalidade = professor.Naturalidade;
                    usuarioSgus.Nome = professor.Nome;
                    usuarioSgus.NomeMae = professor.NomeMae;
                    usuarioSgus.NomePai = professor.NomePai;
                    usuarioSgus.Senha = CriptografiaHelper.Criptografar("sebrae2014");
                    usuarioSgus.TelCelular = professor.TelefoneCelular;
                    usuarioSgus.TelefoneExibicao = professor.Telefone;
                    usuarioSgus.TipoDocumento = professor.TipoDocumentoRG;

                    //new BMUsuario().Salvar(usuarioSgus);
                }

                bool usuarioExistenteNoMoodle = bmUsuarioMoodle.ObterPorEmailOuUsuarioExistente(usuarioSgus.CPF, usuarioSgus.Email);

                if (!usuarioExistenteNoMoodle)
                {
                    var usuarioMoodle = new UsuarioMoodle();

                    usuarioMoodle.Autorizacao = "sgus";
                    usuarioMoodle.Usuario = usuarioSgus.CPF;
                    usuarioMoodle.Senha = CriptografiaHelper.ObterHashMD5(CriptografiaHelper.Decriptografar(usuarioSgus.Senha));
                    usuarioMoodle.IdNumero = "";
                    usuarioMoodle.Nome = usuarioSgus.Nome.Split(' ')[0].Replace(" ", "");
                    usuarioMoodle.Sobrenome = usuarioSgus.Nome.Split(' ')[usuarioSgus.Nome.Split(',').Length].Replace(" ", "");
                    usuarioMoodle.Email = usuarioSgus.Email;
                    usuarioMoodle.EmailParado = false;
                    if (!string.IsNullOrEmpty(usuarioSgus.Cidade))
                    {
                        usuarioMoodle.Cidade = usuarioSgus.Cidade;
                        if (usuarioSgus.UF != null)
                        {
                            usuarioMoodle.Cidade += "/" + usuarioSgus.UF.Sigla;
                        }
                    }
                    usuarioMoodle.Pais = "BR";
                    usuarioMoodle.Idioma = "pt_br";
                    usuarioMoodle.ZonaHoraria = "99";

                    //bmUsuarioMoodle.Salvar(usuarioMoodle);

                    var bmSgusMoodleCursos = new BMSgusMoodleCurso();
                    int codCat = 0;

                    foreach (var item in professor.ListaTurma)
                    {
                        if (item.Oferta.SolucaoEducacional.Fornecedor.ID == (int)enumFornecedor.MoodleSebrae)
                        {
                            if (int.TryParse(item.Oferta.SolucaoEducacional.IDChaveExterna, out codCat))
                            {
                                var categoria = bmSgusMoodleCursos.ObterPorCategoria(codCat);

                                var enrol = new BMInscricao().ObterPorFiltro(new Inscricao { IDCurso = categoria.CodigoCurso, TipoInscricao = "manual" }).FirstOrDefault();

                                if (enrol != null)
                                {
                                    UsuarioMoodleInscricao pUsuarioMoodleInscricao = new UsuarioMoodleInscricao();
                                    pUsuarioMoodleInscricao.IDInscricao = enrol.ID;
                                    pUsuarioMoodleInscricao.UsuarioMoodle = usuarioMoodle;

                                    //new BMUsuarioMoodleInscricao().Salvar(pUsuarioMoodleInscricao);
                                }
                            }
                        }
                    }
                }
            }

            WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Professor sincronizado com sucesso", "ListarProfessor.aspx");
        }

        protected void dgvProfessor_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("excluir"))
            {

                try
                {
                    manterProfessor = new ManterProfessor();
                    int idProfessor = int.Parse(e.CommandArgument.ToString());
                    manterProfessor.ExcluirProfessor(idProfessor);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!", "ListarProfessor.aspx");
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }
            }
            else if (e.CommandName.Equals("editar"))
            {
                int idProfessor = int.Parse(e.CommandArgument.ToString());
                //Session.Add("ProfessorEdit", idProfessor);
                Response.Redirect("EdicaoProfessor.aspx?Id=" + idProfessor.ToString(), false);
            }
            else if (e.CommandName.Equals("sincronizarComMoodle"))
            {
                int idProfessor = int.Parse(e.CommandArgument.ToString());

                ExecutarSincronizacao(idProfessor);
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            //Session["ProfessorEdit"] = null;
            Response.Redirect("EdicaoProfessor.aspx");
        }

        private classes.Professor ObterObjetoProfessor()
        {
            Professor professor = new Professor();

            if (!string.IsNullOrWhiteSpace(this.txtNome.Text))
                professor.Nome = this.txtNome.Text.Trim();

            return professor;
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    Professor professor = ObterObjetoProfessor();
                    manterProfessor = new ManterProfessor();
                    IList<Professor> ListaProfessor = manterProfessor.ObterProfessorPorFiltro(professor);
                    WebFormHelper.PreencherGrid(ListaProfessor, this.dgvProfessor);

                    if (ListaProfessor != null && ListaProfessor.Count > 0)
                    {
                        WebFormHelper.PreencherGrid(ListaProfessor, this.dgvProfessor);
                        pnlProfessor.Visible = true;
                    }
                    else
                    {
                        pnlProfessor.Visible = false;
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Nenhuma Informação Encontrada");
                    }
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }
        }

        protected void dgvProfessor_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                Professor professor = (Professor)e.Row.DataItem;
                var usuario = new BMUsuario().ObterPorCPF(professor.Cpf);
                var bmUsuario = new BMUsuarioMoodle();

                if (usuario != null && bmUsuario.ObterPorEmailOuUsuarioExistente(usuario.CPF, usuario.Email))
                {
                    LinkButton lkbSincronizarComMoodle = (LinkButton)e.Row.FindControl("lkbSincronizarComMoodle");
                    lkbSincronizarComMoodle.Visible = false;

                    //var usuarioMoodle = new BMUsuarioMoodle().ObterPorCPF(usuario.CPF);

                }
            }
        }
    }
}