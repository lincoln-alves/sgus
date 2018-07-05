using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using classes = Sebrae.Academico.Dominio.Classes;
using System.Linq;
using System.Collections.Generic;

namespace Sebrae.Academico.WebForms.Cadastros.Missao
{
    public partial class ListarMissao : Page
    {
        private readonly ManterMissao _manterMissao;

        private bool ObterMissaoPorTrilhas
        {
            get
            {
                return !string.IsNullOrEmpty(txtTrilha.Text) && string.IsNullOrEmpty(txtTrilhaNivel.Text);
            }
        }

        public ListarMissao()
        {
            _manterMissao = new ManterMissao();
        }

        protected override void OnPreRenderComplete(EventArgs e)
        {
            if (dgvMissao.Rows.Count > 0)
            {
                dgvMissao.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;

            PreencherCombos();
        }

        private void PreencherCombos()
        {
            PreencherTrilhas();
        }

        private void PreencherTrilhas()
        {
            var trilhas = new ManterTrilha().ObterTodasTrilhas().OrderBy(x => x.Nome);
            ViewState["_trilha"] = Helpers.Util.ObterListaAutocomplete(trilhas.AsQueryable<classes.Trilha>());
        }

        protected void dgvMissao_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("excluir"))
            {
                try
                {
                    int idMissao = int.Parse(e.CommandArgument.ToString());
                    _manterMissao.ExcluirMissao(idMissao);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!", "ListarMissao.aspx");
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }
            else if (e.CommandName.Equals("editar"))
            {
                int idTrilhaTopicoTematico = int.Parse(e.CommandArgument.ToString());
                Response.Redirect("EdicaoMissao.aspx?Id=" + idTrilhaTopicoTematico, false);
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            //Session.Remove("MissaoEdit");
            Response.Redirect("EdicaoMissao.aspx");
        }

        private classes.Missao ObterObjetoMissao()
        {
            var missao = new classes.Missao();

            if (!string.IsNullOrWhiteSpace(txtMissao.Text))
                missao.Nome = txtMissao.Text.Trim();

            if (!string.IsNullOrEmpty(txtTrilhaNivel.Text))
            {
                missao.PontoSebrae = new classes.PontoSebrae();
                missao.PontoSebrae.TrilhaNivel = new Dominio.Classes.TrilhaNivel
                {
                    ID = int.Parse(txtTrilhaNivel.Text)
                };
            }

            return missao;
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                var missao = ObterObjetoMissao();
                IList<classes.Missao> listaMissao = new List<classes.Missao>();

                if (!string.IsNullOrEmpty(txtIdMissao.Text))
                {
                    var missaoPorId = new ManterMissao().ObterPorID(int.Parse(txtIdMissao.Text));
                    listaMissao.Add(missaoPorId);
                }
                else
                {
                    listaMissao = ObterMissaoPorTrilhas ? _manterMissao.ObterPorPontoSebraeTrilha(new Dominio.Classes.Trilha { ID = int.Parse(txtTrilha.Text) }).ToList() :
                    _manterMissao.ObterMissaoPorFiltro(missao);
                }


                if (listaMissao != null && listaMissao.Any())
                {
                    WebFormHelper.PreencherGrid(listaMissao.ToList(), dgvMissao);
                    pnlMissao.Visible = true;
                }
                else
                {
                    pnlMissao.Visible = false;
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Nenhuma Informação Encontrada");
                }

            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private void PreencherNiveis()
        {
            int idTrilha;

            if (int.TryParse(txtTrilha.Text, out idTrilha))
            {
                var niveis = new ManterTrilhaNivel().ObterPorTrilha(idTrilha).OrderBy(x => x.Nome);
                ViewState["_niveis"] = Helpers.Util.ObterListaAutocomplete(niveis.AsQueryable());
            }
            else
            {
                ViewState["_niveis"] = null;
            }
        }

        protected void txtTrilha_TextChanged(object sender, EventArgs e)
        {
            PreencherNiveis();
        }
    }
}