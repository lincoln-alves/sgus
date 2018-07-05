using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

using System.Linq;

namespace Sebrae.Academico.WebForms.Cadastros
{
    public partial class ListarCategoriaConteudo : PageBase
    {

        protected override void OnPreRenderComplete(EventArgs e)
        {
            if (this.dgvCategoriaConteudo.Rows.Count > 0)
            {
                this.dgvCategoriaConteudo.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }


        private ManterCategoriaConteudo manterCategoriaConteudo = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    base.LogarAcessoFuncionalidade();
                }

                //var usuario = new ManterUsuario().ObterUsuarioLogado();
                //if (usuario.SuperAdministrador)
                //{
                //    btnResolverHerancaSigla.Visible = true;
                //}
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        protected override System.Collections.Generic.IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade
        {
            get { throw new NotImplementedException(); }
        }

        protected override enumFuncionalidade Funcionalidade
        {
            get { return enumFuncionalidade.CategoriaConteudo; }
        }

        protected void dgvFormaAquisicao_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("excluir"))
            {
                try
                {
                    manterCategoriaConteudo = new ManterCategoriaConteudo();
                    var idCategoriaConteudo = int.Parse(e.CommandArgument.ToString());
                    manterCategoriaConteudo.ExcluirCategoriaConteudo(idCategoriaConteudo);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!", "ListarCategoriaConteudo.aspx");
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }
            else if (e.CommandName.Equals("editar"))
            {
                int idCategoriaConteudo = int.Parse(e.CommandArgument.ToString());
                //Session.Add("CategoriaConteudoEdit", idCategoriaConteudo);
                Response.Redirect("EdicaoCategoriaConteudo.aspx?Id=" + idCategoriaConteudo, false);
            }
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            //Session.Remove("CategoriaConteudoEdit");
            Response.Redirect("EdicaoCategoriaConteudo.aspx");
        }

        void ListaResultado(ref List<Dominio.Classes.CategoriaConteudo> listagem, Dominio.Classes.CategoriaConteudo pai, IQueryable<Dominio.Classes.CategoriaConteudo> listaGeral) {
            listagem.Add(pai);

            foreach (var filho in listaGeral.Where(x => x.CategoriaConteudoPai != null && x.CategoriaConteudoPai.ID == pai.ID))
            {
                if (filho.Nome.ToLower().Contains(txtNome.Text.ToLower()) || VerificarNomeEmCategoriasFilhas(filho.ListaCategoriaConteudoFilhos))
                {
                    filho.Nome = "&nbsp;&nbsp;&nbsp;|_ " + filho.Nome;

                    listagem.Add(filho);

                    foreach (var neto in listaGeral.Where(x => x.CategoriaConteudoPai != null && x.CategoriaConteudoPai.ID == filho.ID))
                    {
                        if (neto.Nome.ToLower().Contains(txtNome.Text.ToLower()) || VerificarNomeEmCategoriasFilhas(neto.ListaCategoriaConteudoFilhos))
                        {
                            neto.Nome = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|_ " + neto.Nome;

                            listagem.Add(neto);

                            foreach (var bisneto in listaGeral.Where(x => x.CategoriaConteudoPai != null && x.CategoriaConteudoPai.ID == neto.ID))
                            {
                                if (bisneto.Nome.ToLower().Contains(txtNome.Text.ToLower()) || VerificarNomeEmCategoriasFilhas(bisneto.ListaCategoriaConteudoFilhos))
                                {
                                    bisneto.Nome = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|_ " + bisneto.Nome;

                                    listagem.Add(bisneto);
                                }
                            }
                        }
                    }
                }
            }
        }

        bool VerificarNomeEmCategoriasFilhas(IList<Dominio.Classes.CategoriaConteudo> listagem) {
            return listagem.Any(item => item.Nome.ToLower().Contains(txtNome.Text.ToLower()) || VerificarNomeEmCategoriasFilhas(item.ListaCategoriaConteudoFilhos));
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            var listagem = new List<Dominio.Classes.CategoriaConteudo>();
            var listaGeral = new ManterCategoriaConteudo().ObterTodasCategoriasConteudo();

            foreach (var pai in listaGeral.Where(x => x.CategoriaConteudoPai == null))
            {
                if (!string.IsNullOrEmpty(txtNome.Text)) {
                    //procurar no pai e procurar em todos os filhos
                    if(pai.Nome.ToLower().Contains(txtNome.Text.ToLower()) || VerificarNomeEmCategoriasFilhas(pai.ListaCategoriaConteudoFilhos)) ListaResultado(ref listagem, pai, listaGeral);
                } else {
                    ListaResultado(ref listagem, pai, listaGeral);
                }
            }

            WebFormHelper.PreencherGrid(listagem, dgvCategoriaConteudo);
            pnlCategoriaConteudo.Visible = true;
        }

        /// <summary>
        /// Funcionalidade criada para Atualizara Sigla de hierarquia da Categoria Conteudo para o mesmo valor da Categoria Super Pai (CategoriaConteudoPai == null)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnResolverHerancaSigla_Click(object sender, EventArgs e)
        {
            var manterCategoriaConteudo = new ManterCategoriaConteudo();
            var listaCategoriaConteudoSuperPai = manterCategoriaConteudo.ObterTodasCategoriasConteudo().Where(x => x.CategoriaConteudoPai == null);

            if (listaCategoriaConteudoSuperPai.Any())
            {
                foreach (var categoriaConteudoPai in listaCategoriaConteudoSuperPai)
                {

                    var filhas =
                        manterCategoriaConteudo.ObterTodasCategoriasFilhas(categoriaConteudoPai.ID)
                            .Where(x => x.ID != categoriaConteudoPai.ID);

                    foreach (var filha in filhas)
                    {
                        filha.Sigla = categoriaConteudoPai.Sigla;
                        manterCategoriaConteudo.AlterarCategoriaConteudo(filha);
                    }
                }
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registros de Sigla Corrigidos com sucesso!", "ListarCategoriaConteudo.aspx");
            }
        }
    }
}