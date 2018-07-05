using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.DTO.SGC;
using Sebrae.Academico.BP.Relatorios;
using Sebrae.Academico.BP.Services.SistemasExternos;
using Sebrae.Academico.BP.SGC;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.WebForms.UserControls;
using classes = Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.WebForms.Cadastros.Painel
{
    /// <summary>
    /// Painel de gerenciamento de informações de um aluno.
    /// </summary>
    public partial class GerenciamentoAluno : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    base.LogarAcessoFuncionalidade();
                    this.pnlUcInformarPagamento.Visible = false;
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private void PreencherCategoriasUsuario(classes.Usuario u)
        {
            var selecionados = u.ListaCategoriaConteudo.Select(l => l.ID).ToList();

            ucCategorias.PreencherCategorias(false, selecionados, u, false, true);
        }

        protected override enumFuncionalidade Funcionalidade
        {
            get { return enumFuncionalidade.GerenciadorAluno; }
        }

        private void ExibirPaineis()
        {
            this.ExibirPanelDeTrilhas();
            this.ExibirPanelDeAbandonos();
            this.ExibirPanelDeCursos();
            this.ExibirPanelDeHistoricoDePagamento();
            this.ExibirPanelDeNotificacoes();
            this.ExibirPanelDePublicacoesDoSaber();
            this.ExibirPanelDeMensagensDoFaleConosco();
            this.ExibirPanelDeAtividadesExtracurriculares();
        }

        private void ExibirPanelDadosPessoais(classes.Usuario usuario)
        {
            //Usuario usuario = new ManterUsuario().ObterUsuarioPorID(this.IdUsuario);
            this.ucUsuario1.PreencherListas();
            this.ucUsuario1.PreencherCampos(usuario);
        }

        //protected void linkNotificacoes_ServerClick(object sender, EventArgs e)
        //{
        //    this.ExibirPanelDeNotificacoes();
        //}

        //public Usuario usuario { get; set; }

        ////Todo-> Nardo -> Confirmar com o Marcelo. Seria LogAcesso ou LogAcessoPagina ?
        //private IList<LogAcesso> ObterUltimosAcessosDoUsuario(Usuario usuario)
        //{
        //    throw new NotImplementedException();
        //}

        public string CPFUsuario
        {
            get
            {
                if (ViewState["ViewStateCPFUsuario"] != null)
                {
                    return (string)ViewState["ViewStateCPFUsuario"];
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState["ViewStateCPFUsuario"] = value;
            }
        }

        public int IdUsuario
        {
            get
            {
                if (ViewState["ViewStateIdUsuario"] != null)
                {
                    return Convert.ToInt32(ViewState["ViewStateIdUsuario"]);
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                ViewState["ViewStateIdUsuario"] = value;
            }

        }

        // OnInformouPagamento="InformarPagamento_InformouPagamento" />

        private void OcultarModalInformarPagamento()
        {
            base.OcultarBackDrop();
            pnlUcInformarPagamento.Visible = false;
        }


        protected void InformarPagamento_InformouPagamento(object sender, ucInformarPagamento.InformarPagamentoEventArgs e)
        {
            //IList<ItemQuestionario> listaItemQuestionario = e.ListaItemQuestionario;
            //this.PreencherItemsDoQuestionario(listaItemQuestionario);
            //this.SetarAcaoDaTela(enumAcaoTelaQuestionario.AdicionouUmItem);


            //Exibe o Modal de 

            try
            {
                //Grava as informações do Pagamento
                this.IdUsuario = e.IdUsuario;
                OcultarModalInformarPagamento();

                //Atualiza a grid com as informações de pagamento do usuário
                this.ExibirPanelDeHistoricoDePagamento();

                pnlUcInformarPagamento.Visible = false;

            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void OcultarHistoricoPagamento_Click(object sender, EventArgs e)
        {
            this.OcultarModalHistoricoPagamento();
        }

        private void OcultarModalHistoricoPagamento()
        {
            base.OcultarBackDrop();
            pnlUcInformarPagamento.Visible = false;
        }

        protected void VisualizarDetalheDoPagamento_VisualizouDetalheDoPagamento(object sender, VisualizarDetalheDoPagamentoEventArgs e)
        {
            try
            {
                int idUsuarioPagamento = e.IdUsuarioPagamento;

                //Abre a tela com os detalhes do Pagamento
                //ucHistoricoPagamento1.Visible = false;
                ucInformarPagamento1.IdUsuarioPagamento = idUsuarioPagamento;
                classes.UsuarioPagamento usuarioPagamento = new ManterUsuarioPagamento().ObterInformacoesDePagamentoPorID(idUsuarioPagamento);
                ucInformarPagamento1.PrepararTelaParaEdicaoDeInformarPagamento(usuarioPagamento);
                this.pnlUcInformarPagamento.Visible = true;

            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void UserSelectedHandler(object sender, CompleteUserSelectionEvent e)
        {
            if (LupaUsuario.SelectedUser != null)
            {
                var usuario = LupaUsuario.SelectedUser;
                IdUsuario = LupaUsuario.SelectedUser.ID;
                var logsDeAcesso = new ManterLogAcesso().ObterUltimosAcessoDosUsuario(usuario.ID);
                WebFormHelper.PreencherGrid(logsDeAcesso, dgvLogAcessosDoUsuario);
                IdUsuario = usuario.ID;
                CPFUsuario = usuario.CPF;
                pnlGerenciador.Visible = true;
                ExibirPaineis();
                ExibirPanelDadosPessoais(usuario);
                btnHistorico.Visible = true;
                PreencherCategoriasUsuario(usuario);

                if (usuario.NivelOcupacional.ID == (int)enumNivelOcupacional.Credenciado)
                {
                    VerificarAreas();
                }

                ExibirTutorias(usuario);
            }
            else
            {
                btnHistorico.Visible = false;
            }
        }

        private void ExibirPanelDeNotificacoes()
        {
            this.ucNotificacoes1.PrepararTelaParaExibirNotificacoesDoUsuario(this.IdUsuario);
        }

        private void ExibirPanelDeTrilhas()
        {
            this.ucTrilhas1.PrepararTelaParaExibirInformacoesDasTrilhas(this.IdUsuario);
        }

        private void ExibirPanelDeCursos()
        {
            this.ucCursos1.PrepararTelaParaExibirInformacoesDosCursos(this.IdUsuario);
        }

        private void ExibirPanelDeAbandonos()
        {
            this.ucAbandono1.IdUsuario = this.IdUsuario;
            this.ucAbandono1.Visible = true;
            this.ucAbandono1.PrepararTelaParaExibirAbandonosDoUsuario(this.IdUsuario);
        }

        public void ExibirPanelDeHistoricoDePagamento()
        {
            this.ucHistoricoPagamento1.IdUsuario = this.IdUsuario;
            this.ucHistoricoPagamento1.CarregarInformacoesSobrePagamento();
        }

        public void ExibirPanelDePublicacoesDoSaber()
        {
            this.ucPublicacaoSaber1.IdUsuario = this.IdUsuario;
            this.ucPublicacaoSaber1.CarregarInformacoesSobrePublicacoesDoSaber();
        }

        public void ExibirPanelDeMensagensDoFaleConosco()
        {
            this.ucFaleConosco1.CPFUsuario = this.CPFUsuario;
            this.ucFaleConosco1.CarregarMensagensDoUsuario();
        }

        public void ExibirPanelDeAtividadesExtracurriculares()
        {
            this.ucAtividadeExtraCurricular1.IdUsuario = this.IdUsuario;
            this.ucAtividadeExtraCurricular1.CarregarHistoricoExtraCurricularDoUsuario();
        }

        protected void btnPagamentos_Click(object sender, EventArgs e)
        {
            this.ExibirPanelDeHistoricoDePagamento();
        }

        protected void btnAbandono_Click(object sender, EventArgs e)
        {
            this.ExibirPanelDeAbandonos();
        }

        #region "Métodos para Upload"


        //public string ObterImagemFormatada()
        //{
        //    string imagemEnviada = null;

        //    //Imagem enviada
        //    if (this.ArquivoFoiEnviado)
        //    {

        //        if (fupldImagemEnviada != null && fupldImagemEnviada.PostedFile != null
        //            && fupldImagemEnviada.PostedFile.ContentLength > 0)
        //        {

        //            var imagem = fupldImagemEnviada.PostedFile.InputStream;

        //            this.ImagemEnviada = fupldImagemEnviada.PostedFile;

        //            string imagemConvertidaEmBase64String = CommonHelper.ObterBase64String(imagem);

        //            string informacoesDoArquivoParaBase64 = CommonHelper.GerarInformacoesDoArquivoParaBase64(fupldImagemEnviada);

        //            //certificadoTemplate.OBImagem = string.Concat(informacoesDoArquivoParaBase64, ",", imagemConvertidaEmBase64String);
        //            imagemEnviada = string.Concat(informacoesDoArquivoParaBase64, ",", imagemConvertidaEmBase64String);

        //            this.Imagem = imagemEnviada;
        //        }
        //        else
        //        {
        //            imagemEnviada = this.Imagem;
        //        }
        //    }

        //    return imagemEnviada;

        //}


        #endregion

        private classes.Usuario ObterObjetoUsuario()
        {
            classes.Usuario usuario = new ManterUsuario().ObterUsuarioPorID(this.IdUsuario);
            usuario = this.ucUsuario1.ObterObjetoUsuario(usuario);
            this.AdicionarCategoriaConteudo(usuario);

            return usuario;
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                var usuario = ObterObjetoUsuario();

                var manterUsuario = new ManterUsuario();

                manterUsuario.Salvar(usuario);

                // Cancela a conexão com o English Town caso a situação não seja Ativa.
                if (!usuario.Ativo)
                {
                    new EfServices().CancelSubscriptionApi(usuario);
                }

                ucUsuario1.PreencherCampos(usuario);
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso !");

            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        protected void btnHistorico_Click(object sender, EventArgs e)
        {
            this.GerarRelatorioHistoricoAcademico(this.IdUsuario);
        }

        private void GerarRelatorioHistoricoAcademico(int pIdUsuario)
        {
            using (RelatorioHistoricoAcademico relHisAcad = new RelatorioHistoricoAcademico())
            {

                var DadosGerais = relHisAcad.ConsultarHistoricoAcademicoDadosGerais(pIdUsuario);
                var lstCursos = relHisAcad.ConsultaHistoricoAcademicoCursos(pIdUsuario);
                var lstExtracurricular = relHisAcad.ConsultarHistoricoAcademicoExtracurricular(pIdUsuario);
                var lstPrograma = relHisAcad.ConsultarHistoricoAcademicoPrograma(pIdUsuario);
                var lstTrilha = relHisAcad.ConsultarHistoricoAcademicoTrilha(pIdUsuario);

                RelatoriosHelper.GerarReportViewerHistoricoAcademicoPDF("HistoricoAcademico.rptHistoricoAcademico.rdlc",
                                                                                       DadosGerais, lstCursos,
                                                                                       lstTrilha, lstPrograma,
                                                                                       lstExtracurricular);
            }
        }

        protected override IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade
        {
            get { throw new NotImplementedException(); }
        }

        private void AdicionarCategoriaConteudo(classes.Usuario usuario)
        {
            var categoriasMarcadas = this.ucCategorias.IdsCategoriasMarcadas;
            var categoriasExistentes = new BMCategoriaConteudo().ObterTodos();

            foreach (var item in categoriasExistentes)
            {
                if (categoriasMarcadas.Contains(item.ID))
                {
                    //categoriaConteudo.Auditoria = new Auditoria(new BMUsuario().ObterUsuarioLogado().CPF);
                    var categoriaConteudo = categoriasExistentes.FirstOrDefault(x => x.ID == item.ID);
                    //categoriaConteudo.Usuario = usuario;
                    usuario.AdicionarCategoriaConteudo(categoriaConteudo);
                }
                else
                {
                    if (usuario.ListaCategoriaConteudo.Any(x => x.ID == item.ID))
                    {
                        usuario.RemoverListaCategoriaConteudo(item.ID);
                    }
                }
            }
        }

        private void VerificarAreas()
        {
            var usuario = new ManterUsuario().ObterUsuarioPorID(IdUsuario);
            var subAreas = new ManterSubarea().ObterPorUsuario(usuario);

            if (subAreas.Any())
            {
                // Converte só uma vez em DTO pra evitar consulta lazy a cada loop dos repeaters de áreas.
                var areasDto = subAreas.Select(x => x.Area).Distinct().Select(a => new DTOArea
                {
                    id = a.ID,
                    nome = a.Nome,
                    subAreas =
                        subAreas.Where(s => s.Area.ID == a.ID)
                            .Select(s => new DTOSubarea
                            {
                                id = s.ID,
                                nome = s.Nome
                            }).ToList()
                }).OrderBy(x => x.id);

                rptAreas.DataSource = areasDto;
                rptAreas.DataBind();

                divAreas.Visible = true;
            }
        }

        protected void rptAreas_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var rptSubaras = (Repeater)e.Item.FindControl("rptSubareas");

                var area = (DTOArea)e.Item.DataItem;

                var literalArea = (Literal)e.Item.FindControl("literalArea");

                literalArea.Text = string.Format("{0} - {1}", area.id, area.nome);

                rptSubaras.DataSource = area.subAreas.OrderBy(x => x.id);
                rptSubaras.DataBind();
            }
        }

        protected void rptSubareas_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var subArea = (DTOSubarea)e.Item.DataItem;

                var literalSubarea = (Literal)e.Item.FindControl("literalSubarea");

                literalSubarea.Text = string.Format("{0} - {1}", subArea.id, subArea.nome);
            }
        }

        public void ExibirTutorias(classes.Usuario usuario)
        {
            if (usuario.ListaPerfil.Any(x => x.Perfil.ID == (int)enumPerfil.Professor))
            {
                ucTutorias.PreencherCampos(usuario);
            }
            else
            {
                pnlTutorias.Visible = false;
            }
        }
    }
}
