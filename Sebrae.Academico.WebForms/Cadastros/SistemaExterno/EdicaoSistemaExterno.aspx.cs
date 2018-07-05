using System;
using System.Collections.Generic;
using System.Web.UI;
using Sebrae.Academico.BP;
using Classes = Sebrae.Academico.Dominio.Classes;
using System.Linq;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using System.Web.UI.WebControls;

namespace Sebrae.Academico.WebForms.Cadastros
{
    public partial class EdicaoSistemaExterno : Page
    {
        private SistemaExterno _sistemaExternoEdicao;
        private ManterSistemaExterno manterSistemaExterno = new ManterSistemaExterno();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                PreencherCombos();

                if (Request["Id"] != null)
                {
                    var idSistemaExterno = int.Parse(Request["Id"]);
                    _sistemaExternoEdicao = manterSistemaExterno.ObterSistemaExternoPorID(idSistemaExterno);
                    PreencherCampos(_sistemaExternoEdicao);
                }
            }
        }

        private void PreencherListaPerfil(SistemaExterno sistemaExterno)
        {
            //Obtém a lista de Perfis gravados no banco
            IList<Perfil> ListaPerfil = sistemaExterno.ListaPermissao.Where(x => x.Perfil != null)
                    .Select(x => new Perfil { ID = x.Perfil.ID, Nome = x.Perfil.Nome }).ToList<Perfil>();

            bool temPerfilPublico = false;

            if (ListaPerfil.Count == 0)
            {
                temPerfilPublico = sistemaExterno.ListaPermissao.Where(x => x.Perfil == null &&
                    x.NivelOcupacional == null && x.Uf == null).Any();
            }

            ucPermissoes.PreencherListBoxComPerfisGravadosNoBanco(ListaPerfil, temPerfilPublico);
        }

        private void PreencherCombos()
        {
            WebFormHelper.PreencherComponenteComOpcoesSimNao(rblPublico);
            WebFormHelper.PreencherComponenteComOpcoesSimNao(rblEnglishTown);
            ucPermissoes.PreencherListas();
        }

        private void PreencherCampos(Classes.SistemaExterno sistemaExterno)
        {
            if (sistemaExterno != null)
            {
                //Nome
                txtNome.Text = sistemaExterno.Nome;

                //Link
                if (!string.IsNullOrWhiteSpace(sistemaExterno.LinkSistemaExterno))
                    txtLink.Text = sistemaExterno.LinkSistemaExterno;

                txtDescricao.Text = sistemaExterno.Descricao;

                WebFormHelper.SetarValorNoRadioButtonList(sistemaExterno.Publico, rblPublico);
                WebFormHelper.SetarValorNoRadioButtonList(sistemaExterno.EnglishTown, rblEnglishTown);

                PreencherListasDoSistemaExterno(sistemaExterno);
            }
        }

        private void PreencherListasDoSistemaExterno(SistemaExterno sistemaExterno)
        {
            PreencherListaUfs(sistemaExterno);
            PreencherListaNivelOcupacional(sistemaExterno);
            PreencherListaPerfil(sistemaExterno);
        }

        private void PreencherListaNivelOcupacional(SistemaExterno sistemaExterno)
        {
            //Obtém a lista de niveis ocupacionais gravadas no banco
            IList<NivelOcupacional> ListaNivelOcupacional = sistemaExterno.ListaPermissao.Where(x => x.NivelOcupacional != null)
                    .Select(x => new NivelOcupacional() { ID = x.NivelOcupacional.ID, Nome = x.NivelOcupacional.Nome }).ToList<NivelOcupacional>();

            ucPermissoes.PreencherListBoxComNiveisOcupacionaisGravadosNoBanco(ListaNivelOcupacional);

        }

        private void PreencherListaUfs(Classes.SistemaExterno sistemaExterno)
        {

            //Obtém a lista de ufs
            IList<Uf> ListaUFs = sistemaExterno.ListaPermissao.Where(x => x.Uf != null)
                      .Select(x => new Uf() { ID = x.Uf.ID, Nome = x.Uf.Nome }).ToList<Uf>();

            ucPermissoes.PreencherListBoxComUfsGravadasNoBanco(ListaUFs);
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                _sistemaExternoEdicao = ObterObjetoSistemaExterno();
                if (Request["Id"] == null)
                {
                    manterSistemaExterno.IncluirSistemaExterno(_sistemaExternoEdicao);
                }
                else
                {
                    manterSistemaExterno.AlterarSistemaExterno(_sistemaExternoEdicao);
                }

                Session.Remove("SistemaExternoEdit");

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso !", "ListarSistemaExterno.aspx");

            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private SistemaExterno ObterObjetoSistemaExterno()
        {

            if (Request["Id"] == null)
            {
                _sistemaExternoEdicao = new Classes.SistemaExterno();
            }
            else
            {
                int idSistemaExterno = int.Parse(Request["Id"].ToString());
                _sistemaExternoEdicao = new ManterSistemaExterno().ObterSistemaExternoPorID(idSistemaExterno);
            }

            //Nome
            _sistemaExternoEdicao.Nome = txtNome.Text.Trim();

            // Descricao
            _sistemaExternoEdicao.Descricao = txtDescricao.Text;

            //Link
            _sistemaExternoEdicao.LinkSistemaExterno = txtLink.Text.Trim();

            //Publico
            if (rblPublico.SelectedItem != null && !string.IsNullOrWhiteSpace(rblPublico.SelectedItem.Value))
            {
                var publico = rblPublico.SelectedItem.Value.Trim();

                _sistemaExternoEdicao.Publico = publico.ToUpper().Equals("S");
            }

            //EnglishTown
            if (rblEnglishTown.SelectedItem != null && !string.IsNullOrWhiteSpace(rblEnglishTown.SelectedItem.Value))
            {
                var englishTown = rblEnglishTown.SelectedItem.Value.Trim();

                _sistemaExternoEdicao.EnglishTown = englishTown.ToUpper().Equals("S");
            }

            AdicionarPermissao(_sistemaExternoEdicao);

            return _sistemaExternoEdicao;
        }

        private void AdicionarPermissao(SistemaExterno sistemaExterno)
        {
            //sistemaExterno.ListaPermissao.Clear();
            AdicionarOuRemoverPerfil(sistemaExterno);
            AdicionarOuRemoverUf(sistemaExterno);
            AdicionarOuRemoverNivelOcupacional(sistemaExterno);
        }

        private void AdicionarOuRemoverNivelOcupacional(SistemaExterno sistemaExterno)
        {
            var todosNiveisOcupacionais = ucPermissoes.ObterTodosNiveisOcupacionais;  //.ObterPerfisSelecionados;

            if (todosNiveisOcupacionais != null && todosNiveisOcupacionais.Count > 0)
            {
                for (int i = 0; i < todosNiveisOcupacionais.Count; i++)
                {
                    var nivelOcupacionalSelecionado = new NivelOcupacional()
                    {
                        ID = int.Parse(todosNiveisOcupacionais[i].Value),
                        Nome = todosNiveisOcupacionais[i].Text
                    };

                    if (todosNiveisOcupacionais[i].Selected)
                    {
                        sistemaExterno.AdicionarNivelOcupacional(nivelOcupacionalSelecionado);
                    }
                    else
                    {
                        _sistemaExternoEdicao.RemoverNivelOcupacional(nivelOcupacionalSelecionado);
                    }
                }
            }
        }

        private void AdicionarOuRemoverUf(SistemaExterno sistemaExterno)
        {
            try
            {
                Repeater rptUFs = (Repeater)ucPermissoes.FindControl("rptUFs");
                for (int i = 0; i < rptUFs.Items.Count; i++)
                {
                    CheckBox ckUF = (CheckBox)rptUFs.Items[i].FindControl("ckUF");
                    Literal lblUF = (Literal)rptUFs.Items[i].FindControl("lblUF");
                    TextBox txtVagas = (TextBox)rptUFs.Items[i].FindControl("txtVagas");

                    int idUf = int.Parse(ckUF.Attributes["ID_UF"]);
                    var ufSelecionado = new Uf()
                    {
                        ID = idUf,
                        Nome = lblUF.Text,
                    };

                    if (ckUF.Checked)
                    {
                        int vagas = 0;
                        if (!string.IsNullOrEmpty(txtVagas.Text))
                            vagas = int.Parse(txtVagas.Text);

                        _sistemaExternoEdicao.AdicionarUfs(ufSelecionado);
                    }
                    else
                    {
                        _sistemaExternoEdicao.RemoverUf(ufSelecionado);
                    }
                }
            }
            catch
            {
                throw new ExecutionEngineException("Você deve informar a quantidade de vagas do estado");
            }
        }

        private void AdicionarOuRemoverPerfil(SistemaExterno sistemaExterno)
        {
            var todosPerfis = ucPermissoes.ObterTodosPerfis;

            if (todosPerfis != null && todosPerfis.Count > 0)
            {
                //sistemaExterno.ListaPermissao.Clear();
                for (int i = 0; i < todosPerfis.Count; i++)
                {
                    var perfilSelecionado = new Perfil
                    {
                        ID = int.Parse(todosPerfis[i].Value),
                        Nome = todosPerfis[i].Text
                    };

                    if (todosPerfis[i].Selected)
                    {
                        sistemaExterno.AdicionarPerfil(perfilSelecionado);
                    }
                    else
                    {
                        sistemaExterno.RemoverPerfil(perfilSelecionado);
                    }
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Session.Remove("SistemaExternoEdit");
            Response.Redirect("ListarSistemaExterno.aspx");
        }
    }
}