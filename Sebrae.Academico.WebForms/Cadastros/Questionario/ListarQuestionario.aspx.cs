using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.Cadastros
{
    public partial class ListarQuestionario : PageBase
    {
        protected override void OnPreRenderComplete(EventArgs e)
        {
            if (dgvQuestionarios.Rows.Count > 0)
            {
                dgvQuestionarios.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        private ManterQuestionario _manterQuestionario;

        protected override enumFuncionalidade Funcionalidade
        {
            get { return enumFuncionalidade.Questionario; }
        }

        protected override IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade
        {
            get { throw new NotImplementedException(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    WebFormHelper.LimparVariaveisDeSessao();
                    LogarAcessoFuncionalidade();
                    
                    PreencherFiltros();
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private void PreencherFiltros()
        {
            ListBoxesUF.PreencherItens(new ManterUf().ObterTodosIQueryable().Select(x => new {x.ID, x.Nome}), "ID", "Nome");

            ListBoxesTipoQuestionario.PreencherItens(new ManterTipoQuestionario().ObterTodosIQueryable().Select(x => new {x.ID, x.Nome}), "ID", "Nome");
        }


        protected void dgvQuestionarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var idQuestionario = int.Parse(e.CommandArgument.ToString());
            _manterQuestionario = new ManterQuestionario();

            if (e.CommandName.Equals("excluir"))
            {
                try
                {
                    var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

                    _manterQuestionario.ExcluirQuestionario(idQuestionario, usuarioLogado);

                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!",
                        "ListarQuestionario.aspx");
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
                catch (Exception)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro,
                        "Não foi possível processar a solicitação. Favor verificar registros dependentes.");
                }
            }
            else if (e.CommandName.Equals("editar"))
            {
                Response.Redirect(string.Format("EdicaoQuestionarioU.aspx?Id={0}&Session={1}", idQuestionario,
                    WebFormHelper.ObterStringAleatoria()));
            }
            else if (e.CommandName.Equals("duplicar"))
            {
                Response.Redirect(string.Format("EdicaoQuestionarioU.aspx?Id={0}&Session={1}&Duplicar=1", idQuestionario,
                    WebFormHelper.ObterStringAleatoria()));
            }
            else if (e.CommandName.Equals("visualizar"))
            {
                if (Master != null)
                {
                    if (Master.Master != null)
                    {
                        var pnlbackdrop = ((Panel)Master.Master.FindControl("pnlbackdrop"));

                        if (pnlbackdrop != null)
                        {
                            pnlbackdrop.Visible =
                                pnlVisualizar.Visible = true;

                            PreencherVisualizacaoQuestionario(_manterQuestionario.ObterQuestionarioPorID(idQuestionario));

                            return;
                        }
                    }
                }

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao,
                    "Não foi possível obter o Termo para exibição. Tente novamente.");
            }
        }

        private void PreencherVisualizacaoQuestionario(Questionario questionario)
        {
            ltrNome.Text = questionario.Nome;

            ltrCategoria.Text = questionario.ListaCategoriaConteudo.Any()
                ? string.Join(", ", questionario.ListaCategoriaConteudo.Select(x => x.Nome))
                : "Nenhuma";

            foreach (var questao in questionario.ListaItemQuestionario.OrderBy(x => x.Ordem))
            {
                InserirQuestao(questao);
            }
        }

        private void InserirQuestao(ItemQuestionario questao)
        {
            var isEnunciado = questao.TipoItemQuestionario.ID == (int) enumTipoItemQuestionario.AgrupadorDeQuestoes;

            var append = "<p>" + (isEnunciado ? "<strong>" : "<span style=\"margin-left: 15px;\">") + questao.Ordem +
                         " - " + questao.Questao + (isEnunciado ? "</strong>" : "</span>") + "</p>";

            divDetalhesQuestoes.InnerHtml += append;
        }

        private void LimparVisualizacaoQuestionario()
        {
            ltrNome.Text = "";
            divDetalhesQuestoes.InnerText = "";
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("EdicaoQuestionarioU.aspx?Session={0}", WebFormHelper.ObterStringAleatoria()));
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

                var listaQuestionario = new ManterQuestionario().ObterQuestionarioPorUf(usuarioLogado);

                var text = txtNome.Text.Trim().ToLower();

                if (!string.IsNullOrWhiteSpace(text))
                    listaQuestionario = listaQuestionario.Where(x => x.Nome.Trim().ToLower().Contains(text));

                var idsUfs = ListBoxesUF.RecuperarIdsSelecionados<int>().ToList();

                if (idsUfs.Count > 0)
                    listaQuestionario = listaQuestionario.Where(x => x.Uf != null && idsUfs.Contains(x.Uf.ID));

                var idsTiposQuestionario = ListBoxesTipoQuestionario.RecuperarIdsSelecionados<int>().ToList();

                if (idsTiposQuestionario.Count > 0)
                    listaQuestionario =
                        listaQuestionario.Where(x => idsTiposQuestionario.Contains(x.TipoQuestionario.ID));
                
                var status = int.Parse(ddlStatus.SelectedValue);

                if (status == 1)
                    listaQuestionario = listaQuestionario.Where(x => x.Ativo == true);

                if (status == 2)
                    listaQuestionario = listaQuestionario.Where(x => x.Ativo != true);

                if (listaQuestionario.Any())
                {
                    WebFormHelper.PreencherGrid(listaQuestionario.ToList(), dgvQuestionarios);
                    pnlQuestionario.Visible = true;
                }
                else
                {
                    pnlQuestionario.Visible = false;
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Nenhuma Informação Encontrada");
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        protected void dgvQuestionarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            var questionario = (Questionario)e.Row.DataItem;

            var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

            if (usuarioLogado.IsGestor())
            {
                if (questionario.Uf == null || usuarioLogado.UF.ID != questionario.Uf.ID)
                {
                    var lkbEditar = (LinkButton)e.Row.FindControl("lkbEditar");
                    var lkbExcluir = (LinkButton)e.Row.FindControl("lkbExcluir");

                    lkbEditar.Visible =
                        lkbExcluir.Visible = false;
                }
            }
        }

        protected void FecharDetalhesQuestionario_Click(object sender, EventArgs e)
        {
            if (Master != null)
            {
                if (Master.Master != null)
                {
                    var pnlbackdrop = ((Panel)Master.Master.FindControl("pnlbackdrop"));

                    if (pnlbackdrop != null)
                    {
                        pnlbackdrop.Visible = false;
                    }
                }
            }

            pnlVisualizar.Visible = false;

            LimparVisualizacaoQuestionario();
        }
    }
}