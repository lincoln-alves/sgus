using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP;
using classes = Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.WebForms.Cadastros.Modulo
{
    public partial class EditarModulo : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                WebFormHelper.PreencherLista(new BMPrograma().ObterTodos().OrderBy(x => x.Nome).ToList(), ddlPrograma, false, true);
                if (Request["Id"] != null)
                {
                    var idModulo = int.Parse(Request["Id"]);
                    var modulo = new BMModulo().ObterPorId(idModulo);
                    PreencherCampos(modulo);
                    PreencherModulosDaCapacitacao(modulo);
                }
            }
        }

        private void PreencherCampos(classes.Modulo moduloEdicao)
        {
            if (moduloEdicao != null)
            {
                //Nome
                ddlPrograma.SelectedValue = moduloEdicao.Capacitacao.Programa.ID.ToString();
                ddlPrograma_OnSelectedIndexChanged(null, null);
                ddlCapacitacao.SelectedValue = moduloEdicao.Capacitacao.ID.ToString();
                txtNome.Text = moduloEdicao.Nome;
                txtDescricao.Text = moduloEdicao.Descricao;
                txtDtInicio.Text = moduloEdicao.DataInicio.ToString("dd/MM/yyyy");
                txtDtFim.Text = moduloEdicao.DataFim.HasValue ? moduloEdicao.DataFim.Value.ToString("dd/MM/yyyy") : "";

                PreencherListaSolucaoEducacional(moduloEdicao);
            }
        }

        private void PreencherListaSolucaoEducacional(classes.Modulo moduloEdicao)
        {
            //Obtém a lista de soluções educacionais gravadas no banco
            IList<classes.SolucaoEducacional> ListaSolucaoEducacional = moduloEdicao.ListaSolucaoEducacional.Where(x => x.SolucaoEducacional != null)
                    .Select(x => new classes.SolucaoEducacional { ID = x.SolucaoEducacional.ID, Nome = x.SolucaoEducacional.Nome }).ToList<classes.SolucaoEducacional>();

            ucSolucaoEducacional1.PreencherListBoxComSolucoesEducacionaisGravadasNoBanco(ListaSolucaoEducacional);
        }


        protected void ddlPrograma_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPrograma.SelectedIndex > 0)
            {
                ddlCapacitacao.Enabled = true;
                var filtro = new classes.Capacitacao();
                filtro.Programa.ID = int.Parse(ddlPrograma.SelectedValue);
                WebFormHelper.PreencherLista(new BMCapacitacao().ObterPorFiltro(filtro).ToList(), ddlCapacitacao, true, false);

                PreencherModulosDaCapacitacao();
            }
        }

        private void PreencherModulosDaCapacitacao()
        {
            PreencherModulosDaCapacitacao(null);
        }

        private void PreencherModulosDaCapacitacao(classes.Modulo modulo)
        {
            if (ddlCapacitacao.SelectedIndex > 0)
            {
                var bmModulo = new BMModulo();

                int idCapacitacao = int.Parse(ddlCapacitacao.SelectedValue);
                var modulosPorCapacitacao = bmModulo.ObterPorCapacitacao(idCapacitacao);

                if (modulo != null)
                {
                    modulosPorCapacitacao = modulosPorCapacitacao.Where(x => x.ID != modulo.ID);
                }

                if (modulosPorCapacitacao.Count() > 0)
                {
                    WebFormHelper.PreencherGrid(modulosPorCapacitacao.ToList(), gvModulosPreRequisitos);
                }

                if (modulo != null)
                {
                    for (int i = 0; i < gvModulosPreRequisitos.Rows.Count; i++)
                    {
                        int idModuloPai = int.Parse(gvModulosPreRequisitos.DataKeys[i].Value.ToString());
                        CheckBox ckbModuloPai = (CheckBox)gvModulosPreRequisitos.Rows[i].FindControl("ckbModuloPai");
                        if (ckbModuloPai != null)
                        {
                            ckbModuloPai.Checked = modulo.ListaModuloPai.Any(x => x.ModuloPai.ID == idModuloPai);
                        }
                    }
                }
            }
        }

        protected void ddlCapacitacao_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PreencherModulosDaCapacitacao();
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                classes.Modulo modulo = ObterObjetoModulo();

                var listaModuloSolucaoEducacional = modulo.ListaSolucaoEducacional.ToList();
                var listaModuloPreRequisito = modulo.ListaModuloPai.ToList();
                modulo.ListaSolucaoEducacional = null;                    
                modulo.ListaModuloPai = null;

                new BMModulo().Salvar(modulo);
                new BMModuloSolucaoEducacional().CadastrarLista(listaModuloSolucaoEducacional, modulo.ID);
                new BMModuloPreRequisito().CadastrarLista(listaModuloPreRequisito, modulo.ID);

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso !", "ListarModulo.aspx");
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private classes.Modulo ObterObjetoModulo()
        {
            var retorno = new classes.Modulo();

            if (Request["Id"] != null)
                retorno = new BMModulo().ObterPorId(Convert.ToInt32(Request["Id"]));

            if (string.IsNullOrEmpty(txtNome.Text))
                throw new AcademicoException("Você deve informar o nome do módulo");
            else
                retorno.Nome = txtNome.Text;

            if (string.IsNullOrEmpty(txtDescricao.Text))
                throw new AcademicoException("Você deve informar a descrição do módulo");
            else
                retorno.Descricao = txtDescricao.Text;

            if (ddlPrograma.SelectedIndex <= 0)
                throw new AcademicoException("Você deve informar o programa do módulo");

            if (ddlCapacitacao.SelectedIndex <= 0)
                throw new AcademicoException("Você deve informar a Oferta do módulo");
            else
                retorno.Capacitacao.ID = int.Parse(ddlCapacitacao.SelectedValue);

            DateTime dtIni;
            DateTime dtFim;
            if (DateTime.TryParse(txtDtInicio.Text, out dtIni))
                retorno.DataInicio = dtIni;
            else {
                throw new AcademicoException("Você deve informar a Data Inicio da Realização do módulo");
            }

            if (DateTime.TryParse(txtDtFim.Text, out dtFim))
                retorno.DataFim = dtFim;

            ListBox lbSolucoesEscolhidas = (ListBox)this.ucSolucaoEducacional1.FindControl("lbSolucoesEscolhidas");
            if (lbSolucoesEscolhidas != null)
            {
                var solucoesEducacionaisListadas = lbSolucoesEscolhidas.Items;
                int i = 0;
                foreach (ListItem item in solucoesEducacionaisListadas)
                {
                    var moduloSolucaoEducacional = new ModuloSolucaoEducacional();
                    moduloSolucaoEducacional.Ordem = i;
                    moduloSolucaoEducacional.SolucaoEducacional.ID = Convert.ToInt32(item.Value);

                    if (!retorno.ListaSolucaoEducacional.Any(x => x.SolucaoEducacional.ID == moduloSolucaoEducacional.SolucaoEducacional.ID))
                    {
                        retorno.ListaSolucaoEducacional.Add(moduloSolucaoEducacional);
                        i++;
                    }
                }

                foreach (var item in retorno.ListaSolucaoEducacional.ToList())
                {
                    if (item.ID > 0)
                    {
                        if (!solucoesEducacionaisListadas.Contains(new ListItem { Value = item.SolucaoEducacional.ID.ToString(), Text = item.SolucaoEducacional.Nome }))
                        {
                            retorno.ListaSolucaoEducacional.Remove(item);
                        }
                    }
                }
            }

            for (int i = 0; i < gvModulosPreRequisitos.Rows.Count; i++)
            {
                int idModuloPai = int.Parse(gvModulosPreRequisitos.DataKeys[i].Value.ToString());
                CheckBox ckbModuloPai = (CheckBox)gvModulosPreRequisitos.Rows[i].FindControl("ckbModuloPai");
                if (ckbModuloPai.Checked)
                {
                    if (!retorno.ListaModuloPai.Any(x => x.ModuloPai.ID == idModuloPai))
                    {
                        var moduloPai = new ModuloPreRequisito();
                        moduloPai.ModuloPai.ID = idModuloPai;
                        retorno.ListaModuloPai.Add(moduloPai);
                    }
                }
                else
                {
                    if (retorno.ListaModuloPai.Any(x => x.ModuloPai.ID == idModuloPai))
                    {
                        retorno.ListaModuloPai.Remove(retorno.ListaModuloPai.FirstOrDefault(x=>x.ModuloPai.ID == idModuloPai));
                    }
                }
            }

            return retorno;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListarModulo.aspx");
        }
    }
}