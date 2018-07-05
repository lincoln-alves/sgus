using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BM.Classes;
using classes = Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.BP;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;
using System.Data;
using System.Web.Script.Serialization;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.WebForms.Cadastros.Demanda
{
    public partial class EditarDemanda : Page
    {
        private ManterProcesso _manterProcesso = new ManterProcesso();

        private List<byte> OrdemOriginal
        {
            get
            {
                return ViewState["ordem"] != null ? ViewState["ordem"] as List<byte> : new List<byte>();
            }
            set
            {
                ViewState["ordem"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request["Id"] != null)
                {
                    var idModel = int.Parse(Request["Id"]);
                    var processo = _manterProcesso.ObterPorID(idModel);

                    var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

                    if (usuarioLogado.IsGestor() && (processo.Uf == null || processo.Uf.ID != usuarioLogado.UF.ID))
                    {
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Processo inexistente",
                            "ListarDemanda.aspx");
                        return;
                    }

                    PreencherCampos(processo);
                }
                else
                {
                    PreencherTipos();
                }
            }

        }
        private void PreencherTipos(bool edicao = false)
        {
            if (!edicao)
            {
                ddlTipo.Items.Add("Selecione");
            }

            Enum.GetNames(typeof(enumTipoProcesso))
                .Select((x, index) => new { Value = x, Index = (index + 1) })
                .ToList()
                .ForEach(item =>
                {
                    ddlTipo.Items.Add(new ListItem {
                        Text = item.Value,
                        Value = item.Index.ToString()
                    });
                });
        }

        protected void dgvProcessoEtapas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("excluir"))
            {
                try
                {
                    _manterProcesso = new ManterProcesso();
                    int idProcesso = int.Parse(e.CommandArgument.ToString());
                    _manterProcesso.ExcluirProcesso(idProcesso);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!", "ListarProcesso.aspx");
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }
            }
            else if (e.CommandName.Equals("editar"))
            {
                int idProcesso = int.Parse(e.CommandArgument.ToString());
                Response.Redirect("EditarDemanda.aspx?Id=" + idProcesso.ToString(), false);
            }
        }

        private void PreencherCampos(classes.Processo model)
        {
            if (!string.IsNullOrWhiteSpace(model.Nome))
            {
                txtNome.Text = model.Nome;
            }

            MostrarCampoSelecaoDeDias(model);

            if (!string.IsNullOrWhiteSpace(model.DiaInicio.ToString()))
            {
                txtDiaInicio.Text = model.DiaInicio.ToString().PadLeft(2, '0');
            }

            if (!string.IsNullOrWhiteSpace(model.DiaFim.ToString()))
            {
                txtDiaFinal.Text = model.DiaFim.ToString().PadLeft(2, '0');
            }

            PreencherTipos(true);
            if (!string.IsNullOrWhiteSpace(model.Mensal.Value.ToString()))
            {
                rblMensal.SelectedValue = model.Mensal.Value.ToString();
            }

            if (model.Tipo > 0)
            {
                ddlTipo.SelectedValue = model.Tipo == enumTipoProcesso.Reembolso ? "1" : "2";
            }

            var listaEtapas = new BMEtapa()
                .ObterPorProcessoId(model.ID)
                .Select(x => new
                {
                    ID = x.ID,
                    Nome = x.Nome,
                    Ordem = x.Ordem,
                    PrimeiraEtapa = x.PrimeiraEtapa,
                    TotalEtapasAbertas = x.ListaEtapaResposta.Count(y => y.Status == (int)enumStatusEtapaResposta.Aguardando && y.Ativo == true)
                })
                .OrderBy(d => d.Ordem)
                .ToList();

            OrdemOriginal = listaEtapas
                .Select(x => x.Ordem)
                .ToList();

            rptEtapas.DataSource = listaEtapas;
            rptEtapas.DataBind();
            pnlProcesso.Visible = true;

            rblStatusDemanda.SelectedValue = model.Ativo ? "1" : "0";
        }

        private void MostrarCampoSelecaoDeDias(classes.Processo model)
        {
            rblMensal.SelectedValue = model.Mensal.Value ? "1" : "2";
            datas.Visible = rblMensal.SelectedValue == "1";
        }

        protected void rblMensal_OnChange(object sender, EventArgs e)
        {
            datas.Visible = rblMensal.SelectedIndex == 0;
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                var  processo = new classes.Processo();

                if (Request["Id"] != null)
                {
                    var etapaId = int.Parse(Request["Id"]);

                    processo = _manterProcesso.ObterPorID(etapaId);
                    processo.Nome = txtNome.Text.Trim();
                    processo.Ativo = rblStatusDemanda.SelectedValue == "1";
                    processo.Tipo = (enumTipoProcesso)int.Parse(ddlTipo.SelectedValue);
                    processo.Mensal = rblMensal.SelectedValue == "1";

                    _manipulaDiasDoProcessoMensal(ref processo);

                    _manterProcesso.AlterarProcesso(processo);

                    var serializer = new JavaScriptSerializer();
                    serializer.RegisterConverters(new[] { new DynamicJsonConverter() });
                    dynamic obj = serializer.Deserialize(hdnOrdenacao.Value, typeof(object));

                    if (obj != null)
                    {
                        _manterProcesso.AlterarOrdemEtapas(obj, OrdemOriginal);
                    }
                }
                else
                {
                    if(string.IsNullOrEmpty(txtNome.Text))
                    {
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Preencher o campo nome");
                        return;
                    }

                    processo.Nome = txtNome.Text;

                    if (ddlTipo.SelectedIndex == 0)
                    {
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Selecione um tipo");
                        return;
                    }

                    processo.Tipo = (enumTipoProcesso)int.Parse(ddlTipo.SelectedValue);

                    var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

                    processo.Uf = new classes.Uf { ID = usuarioLogado.UF.ID };

                    _manterProcesso.IncluirProcesso(processo);
                }

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso!", "ListarDemanda.aspx");
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }
        private void _manipulaDiasDoProcessoMensal (ref classes.Processo processo)
        {
            int mensal;
            if (int.TryParse(rblMensal.SelectedValue, out mensal) && mensal > 0)
            {
                processo.Mensal = (mensal == 1);
                int diaInicio;
                int diaFinal;

                if (int.TryParse(txtDiaInicio.Text, out diaInicio) && txtDiaInicio.Text.Count() == 2 && processo.Mensal.Value)
                {
                    processo.DiaInicio = diaInicio;
                }
                else if(processo.Mensal.Value)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Demanda mensal deve ter dia de inicio com dois digitos ex: 09");
                    return;
                }

                if (int.TryParse(txtDiaFinal.Text, out diaFinal) && txtDiaFinal.Text.Count() == 2 && processo.Mensal.Value)
                {
                    processo.DiaFim = diaFinal;
                }
                else if(processo.Mensal.Value)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Demanda mensal deve ter dia de final com dois digitos ex: 25");
                    return;
                }
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Cadastros/Demanda/ListarDemanda.aspx", false);
        }

        protected void Excluir_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            int id = Convert.ToInt16(btn.CommandArgument);
            int idModel = int.Parse(Request["Id"].ToString());

            try
            {
                try
                {

                    var processo = new ManterProcesso().ObterPorID(idModel);
                    if (processo.ListaProcessoResposta.Select(x => new { x.ID}).Any())
                    {
                        throw new AcademicoException("Este processo possui processos respondidos e não pode ser removido");
                    }

                    new ManterEtapa().Excluir(id);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro excluído com sucesso!", "EditarDemanda.aspx?Id=" + idModel.ToString());
                }
                catch (Exception)
                {
                    throw new AcademicoException("Não é possível excluir pois há outros dados dependentes deste registro");
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                return;
            }

        }

        protected void Duplicar_Click(object sender, EventArgs e)
        {
            var btn = (LinkButton)(sender);
            var id = Convert.ToInt16(btn.CommandArgument);
            var idModel = int.Parse(Request["Id"]);

            try
            {
                try
                {
                    var etapa = new ManterEtapa().ObterPorID(id);

                    new ManterEtapa().DuplicarObjeto(etapa, true);
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro duplicado com sucesso!", "EditarDemanda.aspx?Id=" + idModel);
                }
                catch (Exception ex)
                {
                    throw new AcademicoException("Erro ao duplicar o registro");
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                return;
            }

        }

        protected void Repeater_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                dynamic item = e.Item.DataItem as dynamic;

                ((LinkButton)e.Item.FindControl("Editar")).CommandArgument = item.ID.ToString();
                ((LinkButton)e.Item.FindControl("Excluir")).CommandArgument = item.ID.ToString();
                ((LinkButton)e.Item.FindControl("Duplicar")).CommandArgument = item.ID.ToString();

                if (item.PrimeiraEtapa)
                {
                    ((LinkButton)e.Item.FindControl("Excluir")).Visible = false;
                    ((LinkButton)e.Item.FindControl("Duplicar")).Visible = false;
                }
            }
        }

        protected void Editar_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            string id = btn.CommandArgument;
            Response.Redirect("/Cadastros/Etapa/EditarEtapa.aspx?Id=" + id, false);
        }

        protected void AddicionarEtapa_Click(object sender, EventArgs e)
        {
            int idModel = int.Parse(Request["Id"].ToString());
            Response.Redirect("/Cadastros/Etapa/EditarEtapa.aspx?IdProcesso=" + idModel, false);
        }
    }
}