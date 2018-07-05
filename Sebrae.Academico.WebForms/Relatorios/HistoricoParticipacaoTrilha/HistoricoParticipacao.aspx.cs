using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using classes = Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.BP.DTO.Relatorios;
using RelatoriosHelper = Sebrae.Academico.BP.Helpers.RelatoriosHelper;
using Sebrae.Academico.InfraEstrutura.Core.Extensions.Others;
using Sebrae.Academico.BP.Services;
using Sebrae.Academico.Dominio.DTO;
using System.Web.UI.HtmlControls;

namespace Sebrae.Academico.WebForms.Relatorios.HistoricoParticipacaoTrilha
{
    public partial class HistoricoParticipacao : PageBase
    {
        private ManterMatriculaTrilha manterMatriculaTrilha = null;
        private BMUsuarioTrilhaMoedas bmUsuarioTrilhaMoedas = null;

        private int IdUsuarioTrilhaRanking
        {
            get
            {
                return ViewState["UsuarioTrilhaRanking"] != null ? int.Parse(ViewState["UsuarioTrilhaRanking"].ToString()) : 0;
            }
            set
            {
                ViewState["UsuarioTrilhaRanking"] = value;
            }
        }

        private long PosicaoRanking
        {
            get
            {
                return ViewState["PosicaoRanking"] != null ? long.Parse(ViewState["PosicaoRanking"].ToString()) : 0;
            }
            set
            {
                ViewState["PosicaoRanking"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    this.PreencherCombos();
                    base.LogarAcessoFuncionalidade();
                }
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
            get { return enumFuncionalidade.MatriculaTrilha; }
        }

        #region  "Métodos Privados"

        private void PreencherCombos()
        {
            try
            {
                PreencherComboTrilhas();
                PreencherComboUfs();
                PreencherCombosTipoSolucao();
                PreencherComboNivelOcupacional();
                PreencherComboStatus();
                //PreencherComboSolucaoSebrae();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void PreencherComboSolucaoSebrae()
        {
            var manter = new ManterItemTrilha();
            var query = manter.ObterTodosIQueryable();

            query = cblTipoSolucao.Items[0].Selected ? query.Where(x => x.Usuario == null) : query.Where(x => x.Usuario != null);
            query = FiltrarItensTrilhaPorSelecao(query);

            var solucoes = query.ToList();

            WebFormHelper.PreencherLista(solucoes, ddlSoucaoEducacional, false, true);
        }

        private void PreencherComboStatus()
        {
            var values = Enum.GetValues(typeof(enumStatusParticipacaoItemTrilha)).Cast<enumStatusParticipacaoItemTrilha>().Select(x => new { Nome = x.GetDescription(), ID = (int)x }).ToList();

            WebFormHelper.PreencherLista(values, ddlStatus, false, true);
        }

        private void PreencherComboNivelOcupacional()
        {
            var niveis = new ManterNivelOcupacional().ObterTodosNivelOcupacional();
            ucSelectNivelOcupacional.PreencherItens(niveis, "ID", "Nome", false);
        }

        private void PreencherCombosTipoSolucao()
        {
            var tipoSolucaoSebrae = new ListItem
            {
                Text = "Solução Sebrae",
                Value = "0"
            };

            var tipoSolucaoTrilheiro = new ListItem
            {
                Text = "Solução Trilheiro",
                Value = "1"
            };

            cblTipoSolucao.Items.Add(tipoSolucaoSebrae);
            cblTipoSolucao.Items.Add(tipoSolucaoTrilheiro);
            cblTipoSolucao.DataBind();
        }

        private void PreencherComboUfs()
        {
            var ufs = Enum.GetValues(typeof(enumUF)).Cast<enumUF>().Select(x => new { ID = (int)x, Name = x.GetDescription() });
            ucMultiplosUF.PreencherItens(ufs, "ID", "Name");
        }

        private void PreencherComboTrilhas()
        {
            ManterTrilha manterTrilha = new ManterTrilha();
            IList<Trilha> ListaTrilhas = manterTrilha.ObterTodasTrilhas();
            WebFormHelper.PreencherLista(ListaTrilhas, this.ddlTrilha, false, true);
        }

        #endregion

        public UsuarioTrilha ObterObjetoUsuarioTrilha()
        {
            var usuarioTrilha = new UsuarioTrilha();

            //Trilha Nivel
            if (ddlTrilhaNivel != null && ddlTrilhaNivel.SelectedItem != null && int.Parse(ddlTrilhaNivel.SelectedItem.Value) != 0 &&
                !string.IsNullOrWhiteSpace(ddlTrilhaNivel.SelectedItem.Value))
            {
                usuarioTrilha.TrilhaNivel = new classes.TrilhaNivel { ID = int.Parse(ddlTrilhaNivel.SelectedItem.Value) };
            }
            else
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Selecione um Nível de Trilha");
                return null;
            }

            return usuarioTrilha;

        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            try
            {
                componenteGeracaoRelatorio.Visible = false;

                var usuarioTrilha = ObterObjetoUsuarioTrilha();

                manterMatriculaTrilha = new ManterMatriculaTrilha();

                if (usuarioTrilha == null)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Nenhuma Informação Encontrada");
                }
                else
                {
                    if (ucLupaUsuario.SelectedUser != null)
                    {
                        usuarioTrilha.Usuario = ucLupaUsuario.SelectedUser;
                    }
                }

                // Ufs selecionados
                var ufsSelecionados = ucMultiplosUF.RecuperarIdsSelecionados<int>();

                //var listaUsuarioTrilha = manterMatriculaTrilha.ObterMatriculaTrilhaPorFiltro(usuarioTrilha, ufsSelecioandos).Take(10);
                var listaUsuarioTrilha = manterMatriculaTrilha.ObterMatriculaTrilhaPorFiltro(usuarioTrilha, ufsSelecionados);

                // Filtro por status de matrícula para que a consulta fique menor no momento do bind
                int idStatus;
                if (int.TryParse(ddlStatus.SelectedValue, out idStatus) && idStatus > 0)
                {
                    listaUsuarioTrilha = listaUsuarioTrilha.Where(x =>
                    x.ListaItemTrilhaParticipacao.Any(p => p.ItemTrilha.ObterStatusParticipacoesItemTrilha(p.UsuarioTrilha) == (enumStatusParticipacaoItemTrilha)idStatus)).ToList();
                }

                var niveisOcupacionais = ucSelectNivelOcupacional.RecuperarIdsSelecionados<int>().ToList();
                listaUsuarioTrilha = niveisOcupacionais.Any() ?
                    listaUsuarioTrilha.Where(x => niveisOcupacionais.Contains(x.NivelOcupacional.ID)).ToList() :
                    listaUsuarioTrilha;

                // Filtro por data limite de conclusão
                listaUsuarioTrilha = !string.IsNullOrEmpty(txtDataLimite.Text) ?
                    listaUsuarioTrilha.Where(x => x.DataLimite <= DateTime.Parse(txtDataLimite.Text)).ToList() :
                    listaUsuarioTrilha;

                // Filtro por data inicio de conclusão
                listaUsuarioTrilha = !string.IsNullOrEmpty(txtPeriodoInicial.Text) ?
                    listaUsuarioTrilha.Where(x => x.DataInicio >= DateTime.Parse(txtPeriodoInicial.Text)).ToList() :
                    listaUsuarioTrilha;

                // Filtro por data fim de conclusão
                listaUsuarioTrilha = !string.IsNullOrEmpty(txtPeriodoFinal.Text) ?
                    listaUsuarioTrilha.Where(x => x.DataFim <= DateTime.Parse(txtPeriodoFinal.Text)).ToList() :
                    listaUsuarioTrilha;

                if (listaUsuarioTrilha.Any())
                {
                    
                    componenteGeracaoRelatorio.Visible = true;

                    rptUsuariosTrilha.DataSource = listaUsuarioTrilha;
                    rptUsuariosTrilha.DataBind();


                   Cache["dsRelatorioHistoricoParticipacao"] = listaUsuarioTrilha;
                    Cache["dsCamposRelatorioHistoricoParticipacao"] = chkListaCamposVisiveis;
                    Cache["dsFiltrosRelatorioHistoricoParticipacao"] = new DTOFiltrosHistoricoParticipacaoTrilha
                    {
                        ddlPontoSebrae = ddlPontoSebrae.SelectedValue,
                        ddlMissao = ddlMissao.SelectedValue,
                        rblTipoSolucao = cblTipoSolucao.Items[0].Selected ? "0" : "1",
                        ddlStatus = ddlStatus.SelectedValue,
                        usuarioTrilha = usuarioTrilha,
                        niveisSelecionados = ucSelectNivelOcupacional.RecuperarIdsSelecionados<int>().ToList()
                    };

                    var query = new ManterPontoSebrae().ObterTodosIqueryable();

                    // Recupera ponto sebrae selecionado
                    int idPontoSebraeSelecionado;
                    if (int.TryParse(ddlPontoSebrae.SelectedValue, out idPontoSebraeSelecionado) && idPontoSebraeSelecionado != 0)
                    {
                        query = query.Where(x => x.ID == idPontoSebraeSelecionado);
                    }

                    // Filtrar pro trilha nível
                    query = query.Where(x => x.TrilhaNivel.ID == usuarioTrilha.TrilhaNivel.ID);

                    var niveisSelecionados = ucSelectNivelOcupacional.RecuperarIdsSelecionados<int>();

                    // Filtra pro nível ocupacional
                    query = niveisSelecionados.Any() ?
                        query.Where(x => x.TrilhaNivel.ListaPermissao.Where(p => p.NivelOcupacional != null).Any(p => niveisSelecionados.Contains(p.NivelOcupacional.ID)))
                        : query;

                    var dtoPontoSebrae = query.OrderBy(x => x.ID).Select(p => new DTOPontoSebrae
                    {
                        ID = p.ID.ToString(),
                        NomePontoSebrae = p.NomeExibicao
                    }).ToList();

                    // rptPontosSebrae.DataSource = dtoPontoSebrae;
                    // rptPontosSebrae.DataBind();

                    pnlParticipacaoTrilha.Visible = true;

                    //if (!chkListaCamposVisiveis.Items.FindByValue("CPF").Selected)  CPF.Visible = false;
                    //if (!chkListaCamposVisiveis.Items.FindByValue("DataAlteracaoStatusParticipacao").Selected) DataInclusaoTrilha.Visible = false;
                    //if (!chkListaCamposVisiveis.Items.FindByValue("DataInclusaoTrilha").Selected) DataInclusaoTrilha.Visible = false;

                    //WebFormHelper.ValidarVisibilidadeCamposGrid(dgRelatorio, chkListaCamposVisiveis.Items);
                }
                else
                {
                    pnlParticipacaoTrilha.Visible = false;
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Nenhuma Informação Encontrada");
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }

        }
        
        protected void rptUsuariosTrilha_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

           var rptPontosSebraeUsuario = (Repeater)e.Item.FindControl("rptPontosSebraeUsuario");
           var rptsolucaoesDaTrilhaOnline = (Repeater)e.Item.FindControl("rptsolucaoesDaTrilhaOnline");
           var rptsolucaoesDoTrilheiro = (Repeater)e.Item.FindControl("rptsolucaoesDoTrilheiro");
           var rptsolucaoesDesempenhoGeral = (Repeater)e.Item.FindControl("rptSolucaoesDesempenhoGeral");

            // Inception de repeaters.
            // Obter as questões por aqui.
            var usuarioTrilha = (UsuarioTrilha)e.Item.DataItem;

            var dic = new Dictionary<string, object>();
            dic.Add("@ID_Usuariario_Trilha", usuarioTrilha.ID);

            var solucaoesDaTrilha = new BMItemTrilha().ExecutarProcedure<DTOSolucoesDaTrilha>("SP_solucoes_da_trilha", dic);
            var solucaoesDaTrilhaOnline = new BMItemTrilha().ExecutarProcedure<DTOCursosOnlineUCSebrae>("SP_cursos_online_ucsebrae", dic);
            var solucaoesDoTrilheiro = new BMItemTrilha().ExecutarProcedure<DTOSolucoesTrilheiro>("SP_solucoes_do_trilheiro", dic);
            var solucaoesDesempenhoGeral = new BMItemTrilha().ExecutarProcedure<DTOSolucaoesDesempenhoGeral>("SP_solucoes_do_desempenho_geral", dic);            


            /* -- Solução da trilha  --*/
            rptPontosSebraeUsuario.DataSource = solucaoesDaTrilha;
            rptPontosSebraeUsuario.DataBind();
            
            var rptPontosSebraeUsuarioObjetivos  = (HtmlTableCell)e.Item.FindControl("rptPontosSebraeUsuarioObjetivos");
            rptPontosSebraeUsuarioObjetivos.InnerText = solucaoesDaTrilha.Count().ToString();

            var rptPontosSebraeUsuarioTotalHoras = (HtmlTableCell)e.Item.FindControl("rptPontosSebraeUsuarioTotalHoras");
            rptPontosSebraeUsuarioTotalHoras.InnerText = solucaoesDaTrilha.Sum(x => x.CargaHoraria).ToString() + "h";

            var rptPontosSebraeUsuarioMoedas = (HtmlTableCell)e.Item.FindControl("rptPontosSebraeUsuarioMoedas");
            rptPontosSebraeUsuarioMoedas.InnerText = solucaoesDaTrilha.Sum(x => x.Moedas).ToString();
            /* -- Solução da trilha  --*/
            
            
            /* -- Solução da trilha online --*/
            rptsolucaoesDaTrilhaOnline.DataSource = solucaoesDaTrilhaOnline;
            rptsolucaoesDaTrilhaOnline.DataBind();
            

            var rptsolucaoesDaTrilhaOnlineObjetivo = (HtmlTableCell)e.Item.FindControl("rptsolucaoesDaTrilhaOnlineObjetivo");
            rptsolucaoesDaTrilhaOnlineObjetivo.InnerText = solucaoesDaTrilhaOnline.Count().ToString();

            var rptsolucaoesDaTrilhaOnlineMoedasProvaFinal = (HtmlTableCell)e.Item.FindControl("rptsolucaoesDaTrilhaOnlineMoedasProvaFinal");
            rptsolucaoesDaTrilhaOnlineMoedasProvaFinal.InnerText = solucaoesDaTrilhaOnline.Sum(x => x.Moedas).ToString();

            var rptsolucaoesDaTrilhaOnlineTotal = (HtmlTableCell)e.Item.FindControl("rptsolucaoesDaTrilhaOnlineTotal");
            rptsolucaoesDaTrilhaOnlineTotal.InnerText = solucaoesDaTrilhaOnline.Sum(x => x.CargaHoraria).ToString();
            /* -- Solução da trilha online --*/


            /* -- Solução do trilheiro --*/
            rptsolucaoesDoTrilheiro.DataSource = solucaoesDoTrilheiro;
            rptsolucaoesDoTrilheiro.DataBind();
            /* -- Solução do trilheiro --*/

            /* --Desempenho geral-- */
            rptsolucaoesDesempenhoGeral.DataSource = solucaoesDesempenhoGeral;
            rptsolucaoesDesempenhoGeral.DataBind();

            var sDG = solucaoesDesempenhoGeral.FirstOrDefault();
            //int _totalHoras = sDG != null ? sDG.CargaHoraria : 0;

            //var rptsolucaoesDesempenhoGeralObjetos = (HtmlTableCell)e.Item.FindControl("rptsolucaoesDesempenhoGeralObjetos");
            //rptsolucaoesDesempenhoGeralObjetos.InnerText = sDG != null ? sDG.Objetivo.ToString() : "0";

            //var rptsolucaoesDesempenhoGeralHoras = (HtmlTableCell)e.Item.FindControl("rptsolucaoesDesempenhoGeralHoras");
            //rptsolucaoesDesempenhoGeralHoras.InnerText = _totalHoras.ToString() + "h";

            //var rptsolucaoesDesempenhoGeralMoedas = (HtmlTableCell)e.Item.FindControl("rptsolucaoesDesempenhoGeralMoedas");
            //rptsolucaoesDesempenhoGeralMoedas.InnerText = sDG != null ? sDG.Moedas.ToString() : "0";
            /* --Desempenho geral-- */

            ChecarExibirItem("CPF", e);
            ChecarExibirItem("DataAlteracaoStatusParticipacao", e);
            ChecarExibirItem("DataInclusaoTrilha", e);

        }

        public virtual void rptPontosSebraeUsuario_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
        }
        public virtual void rptsolucaoesDaTrilhaOnline_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
        }
        public virtual void rptsolucaoesDoTrilheiro_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
        }
        public virtual void rptsolucaoesDesempenhoGeral_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
        }

        protected void rptItemTrilhaUsuario_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ChecarExibirItem("PontoSebrae", e);
            ChecarExibirItem("TipoSolucao", e);
            ChecarExibirItem("SolucaoEducacional", e);
            ChecarExibirItem("Missao", e);
            ChecarExibirItem("FormaAquisicao", e);
            ChecarExibirItem("NotaObtida", e);
            ChecarExibirItem("TotalCurtidas", e);
            ChecarExibirItem("TotalDescurtidas", e);
            ChecarExibirItem("Ranking", e);
        }

        private void ChecarExibirItem(string nome, RepeaterItemEventArgs e)
        {
            if (chkListaCamposVisiveis.Items.FindByValue(nome).Selected == false)
                e.Item.FindControl(nome).Visible = false;
        }

        private long ObterRanking(DTOPontoSebrae pontoSebraeUsuario)
        {
            var posicao = PosicaoRanking;

            if (pontoSebraeUsuario.UsuarioTrilha.ID != IdUsuarioTrilhaRanking)
            {
                var nivel = pontoSebraeUsuario.UsuarioTrilha.TrilhaNivel;

                var ordem =
                    new BMRankingTrilhas().ObterRanking(nivel,
                        nivel.ListaUsuarioTrilha.Count())
                        .FirstOrDefault(y => y.ID == pontoSebraeUsuario.UsuarioTrilha.Usuario.ID)?.Ordem;

                PosicaoRanking = ordem ?? 0;
                IdUsuarioTrilhaRanking = pontoSebraeUsuario.UsuarioTrilha.ID;
            }

            return posicao;
        }

        protected void rptPontosSebrae_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var item = (DTOPontoSebrae)e.Item.DataItem;
                var thCabecalho = (Literal)e.Item.FindControl("ThCabecalho");

                // Criar HTML da linha dos enunciados.
                if (!string.IsNullOrEmpty(item.NomePontoSebrae))
                {
                    thCabecalho.Text = "<th class=\"text-center\" \">" + item.NomePontoSebrae + "</th>";
                }
            }

        }

        protected void ddlTrilha_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ddlTrilha.SelectedItem.Value))
            {
                try
                {
                    //Busca os níveis associados à trilha
                    Trilha trilha = new Trilha() { ID = int.Parse(ddlTrilha.SelectedItem.Value) };
                    this.PreencherComboTrilhaNivel(trilha);
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }
            else
            {
                ddlTrilhaNivel.Items.Clear();
            }
        }

        private void PreencherComboTrilhaNivel(Trilha trilha)
        {
            ManterTrilhaNivel manterTrilhaNivel = new ManterTrilhaNivel();
            IList<classes.TrilhaNivel> ListaTrilhaNivel = manterTrilhaNivel.ObterPorTrilha(trilha).OrderBy(x => x.Nome).ToList();
            WebFormHelper.PreencherLista(ListaTrilhaNivel, this.ddlTrilhaNivel, false, true);

            if (ListaTrilhaNivel != null && ListaTrilhaNivel.Count > 0)
            {
                WebFormHelper.PreencherLista(ListaTrilhaNivel, this.ddlTrilhaNivel, false, true);
            }
            else
            {
                ddlTrilhaNivel.Items.Clear();
            }
        }

        public List<DTOPontoSebrae> ConvertDtoPontoSebrae(IList<PontoSebrae> pontosSebrae, UsuarioTrilha usuarioTrilha)
        {
            var listaPontoSebrae = new List<DTOPontoSebrae>();
            foreach (var pontoSebrae in pontosSebrae)
            {
                var pontoSebraeDto = new DTOPontoSebrae()
                {
                    NomePontoSebrae = pontoSebrae.Nome
                };
            }

            return listaPontoSebrae;
        }

        private bool IsDadosNull()
        {
            bool isNull = true;
            return isNull;
        }

        protected void btnGerarRelatorio_Click(object sender, EventArgs e)
        {            

            var requestUrl = new ManterConfiguracaoSistema().ObterConfiguracaoSistemaPorID(
                (int)enumConfiguracaoSistema.EnderecoSGUS).Registro + "/Relatorios/HistoricoParticipacaoTrilha/HistoricoParticipacaoForm.aspx";

            var quantidadeRegistro = 1;

            var nomeAmigavel = "Histórico de Participações em Trilha";

            var nomeRelatorio = "HistoricoParticipacaoTrilha";

            RelatoriosHelper.ExecutarThreadSolicitacaoRelatorioRequisicao(requestUrl, enumTipoSaidaRelatorio.EXCEL,
                nomeRelatorio, nomeAmigavel, quantidadeRegistro);

        }

        protected void ddlTrilhaNivel_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idSelecionado;

            if (int.TryParse(ddlTrilhaNivel.SelectedValue, out idSelecionado))
            {
                var pontosSebrae = new ManterPontoSebrae().ObterPorTrilhaNivel(new TrilhaNivel
                {
                    ID = idSelecionado
                }).OrderBy(x => x.Nome);

                WebFormHelper.PreencherLista(pontosSebrae, ddlPontoSebrae, false, true);
            }

            PreencherComboSolucaoSebrae();
        }

        protected void ddlPontoSebrae_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idSelecionado;

            if (int.TryParse(ddlPontoSebrae.SelectedValue, out idSelecionado))
            {
                var missoes = new ManterMissao().ObterPorPontoSebrae(new PontoSebrae
                {
                    ID = idSelecionado
                }).OrderBy(x => x.Nome);

                WebFormHelper.PreencherLista(missoes, ddlMissao, false, true);
            }

            PreencherComboSolucaoSebrae();
        }

        protected void ddlMissao_SelectedIndexChanged(object sender, EventArgs e)
        {
            PreencherComboSolucaoSebrae();
        }

        protected void cblTipoSolucao_SelectedIndexChanged(object sender, EventArgs e)
        {

            //verifica se a opção solução está marcada para exibir o painel de solução.
            pnlSolucao.Visible = cblTipoSolucao.Items.Cast<ListItem>()
                .Any(x => x.Selected && x.Value == "0");            
         
        }

        /// <summary>
        /// Filtra por missão ou ponto sebrae ou nivel trilha
        /// </summary>
        /// <param name="itensTrilha"></param>
        /// <returns></returns>
        private IQueryable<ItemTrilha> FiltrarItensTrilhaPorSelecao(IQueryable<ItemTrilha> itensTrilha)
        {
            int idMissao;
            int idTrilhaNivel;
            int idPontoSebrae;
            // Caso tenha missão selecionada
            if (int.TryParse(ddlMissao.SelectedValue, out idMissao) && idMissao != 0)
            {
                return itensTrilha.Where(x => x.Missao.ID == idMissao);
            }
            // Caso tenha ponto sebrae selecionado
            else if (int.TryParse(ddlPontoSebrae.SelectedValue, out idPontoSebrae) && idPontoSebrae != 0)
            {
                return itensTrilha.Where(x => x.Missao.PontoSebrae.ID == idPontoSebrae);
            }
            // Caso tenha nivel da trilha selecionado
            else if (int.TryParse(ddlTrilhaNivel.SelectedValue, out idTrilhaNivel) && idTrilhaNivel != 0)
            {
                return itensTrilha.Where(x => x.Missao.PontoSebrae.TrilhaNivel.ID == idTrilhaNivel);
            }

            return itensTrilha;
        }
    }
}