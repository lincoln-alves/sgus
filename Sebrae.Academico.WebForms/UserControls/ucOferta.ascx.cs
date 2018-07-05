using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BM.Classes.Moodle;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.BP.SGC;

namespace Sebrae.Academico.WebForms.UserControls
{
    /// <summary>
    /// User control para cadastro de oferta.
    /// </summary>
    public partial class ucOferta : System.Web.UI.UserControl
    {
        public ucOferta()
        {
            CarregarLoad = true;
        }

        public enum enumAcaoDaTela
        {
            EdicaoDeUmaOferta = 1
        }

        public bool IsAdmin { get; set; }
        public bool IsGestor { get; set; }
        public bool CarregarLoad { get; set; }

        private enumTipoOferta ObterTipoOfertaSelecionado()
        {
            return (enumTipoOferta)int.Parse(ddlTipo.SelectedValue);
        }

        #region "Eventos referentes ao Cancelamento do cadastro de uma Oferta"

        public delegate void CadastroDeOfertaCancelado(object sender, CadastrarOfertaEventArgs e);
        //public event CadastroDeOfertaCancelado CancelouCadastroDeOferta;

        #endregion

        #region "Eventos referentes ao cadastro de uma Oferta"

        public delegate void CadastroDeOfertaRealizado(object sender, CadastrarOfertaEventArgs e);
        //public event CadastroDeOfertaRealizado CadastrouAOferta;

        //public event CadastroDeOfertaRealizado CadastrouAOferta
        //{
        //    add { CadastrouAOferta += value; }
        //    remove { CadastrouAOferta -= value; }
        //}

        #endregion

        private Oferta ofertaEdicao = null;
        private ManterOferta manterOferta = new ManterOferta();

        /// <summary>
        /// ID da Oferta. O ID da oferta é persistido (armazenado) no viewstate da página.
        /// </summary>
        public int? IdOferta
        {
            get
            {
                if (ViewState["ViewStateIdOferta"] != null)
                {
                    return (int)ViewState["ViewStateIdOferta"];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                ViewState["ViewStateIdOferta"] = value;
            }

        }

        /// <summary>
        /// Id da Solução Educacional.
        /// </summary>
        public int? IdSolucaoEducacional
        {
            get
            {
                if (ViewState["ViewStateIdSolucaoEducacional"] != null)
                {
                    return (int)ViewState["ViewStateIdSolucaoEducacional"];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                ViewState["ViewStateIdSolucaoEducacional"] = value;
            }

        }

        /// <summary>
        /// Propriedade que indica a ação realizada pelo usuário em uma tela.
        /// </summary>
        public int AcaoDaTela
        {
            get
            {
                if (ViewState["ViewStateAcaoDaTelaDeOferta"] != null)
                {
                    return (int)ViewState["ViewStateAcaoDaTelaDeOferta"];
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                ViewState["ViewStateAcaoDaTelaDeOferta"] = value;
            }

        }

        public void PrepararTelaParaEdicaoDeUmaOferta(Oferta oferta)
        {
            if (oferta != null)
            {
                this.IdOferta = oferta.ID;
                this.PreencherInformacoesDaOferta(oferta);
            }
        }

        private enumOperacao Operacao
        {
            get
            {
                enumOperacao? operacao = null;

                if (ViewState["OfertaEdit"] != null)
                {
                    operacao = enumOperacao.Edicao;
                }
                else
                {
                    operacao = enumOperacao.Consulta;
                }

                return operacao.Value;
            }
        }

        private void ExibirCamposAdministrador()
        {
            var bmUsuario = new BMUsuario();
            var exibir = bmUsuario.PerfilAdministrador();
            IsAdmin = exibir;
            if (!IsAdmin)
            {
                IsGestor = bmUsuario.PerfilGestor();
            }

            divChaveExterna.Visible = exibir;
            divValorPrevisto.Visible = exibir;
            divValorRealizado.Visible = exibir;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack && CarregarLoad)
            {
                CarregarDados();
            }


            VerificarExibicaoAreas();
        }

        public void CarregarDados()
        {
            ExibirCamposAdministrador();

            PreencherCampos();
            ValidarGestor();

            if (IdOferta.HasValue && IdOferta.Value > 0)
            {
                ofertaEdicao = manterOferta.ObterOfertaPorID(IdOferta.Value);
                PreencherInformacoesDaOferta(ofertaEdicao);
                PreencherOfertaTrancadaParaParticipantes(ofertaEdicao);
                PreencherPublicoAlvo(ofertaEdicao);
            }
            else
            {
                divTxtCodigo.Visible = false;
                ucPermissoes2.PreencherListas(exibirVagasUfs: true, modoDistribuicao: enumDistribuicaoVagasOferta.OrdemDeInscricao);
            }

            AtualizarCursos(ofertaEdicao);
        }

        private void ValidarGestor()
        {
            Usuario usuarioLogado = new ManterUsuario().ObterUsuarioLogado();
            if (usuarioLogado.IsGestor() && usuarioLogado.UF.ID != 1)
            {
                rblGestorUC.SelectedValue = "S";
                divPermitirInscricaoGestor.Visible = false;

                rblAlteracaoPeloGestor.SelectedValue = "S";
                divPermitirAlteracaoGestor.Visible = false;

                rbl_PermiteCadastroTurmaPeloGestorUC.SelectedValue = "S";
                divPermitirCadastroGestor.Visible = false;
            }
        }

        private void PreencherPublicoAlvo(Oferta ofertaEdicao)
        {
            if (ofertaEdicao.ListaPublicoAlvo != null)
            {
                for (int i = 0; i < ckblstPublicoAlvo.Items.Count; i++)
                {
                    int id = int.Parse(ckblstPublicoAlvo.Items[i].Value);
                    ckblstPublicoAlvo.Items[i].Selected = ofertaEdicao.ListaPublicoAlvo.Any(x => x.PublicoAlvo.ID == id);
                }
            }
        }

        private void PreencherOfertaTrancadaParaParticipantes(Oferta oferta)
        {
            var niveisTrancados = oferta.ListaNiveisTrancados;

            foreach (var item in from ListItem item in cblOfertaTrancadaParaPagantes.Items
                                 let nivelId = int.Parse(item.Value)
                                 let trancamento = niveisTrancados.FirstOrDefault(x => x.ID == nivelId)
                                 where trancamento != null
                                 select item)
            {
                item.Selected = true;
            }
        }

        public void PreencherInformacoesDaOferta(Oferta oferta)
        {
            if (oferta != null)
            {
                var republicarOferta = !string.IsNullOrEmpty(Request["Republicar"]);

                //Tipo de Oferta
                ddlTipo.ClearSelection();
                var idTipoOferta = (int)oferta.TipoOferta;
                WebFormHelper.SetarValorNaCombo(idTipoOferta.ToString(), ddlTipo);

                if (AcaoDaTela.Equals((int)enumAcaoDaTela.EdicaoDeUmaOferta))
                {
                    txtSolucaoEducacional.Text = oferta.SolucaoEducacional.ID.ToString();
                    //this.ddlSolucaoEducacional.Enabled = false;

                    //Persiste o Id da Solução Educacional no Viewstate da Página
                    IdSolucaoEducacional = oferta.SolucaoEducacional.ID;

                    //this.ddlTipo.Enabled = false;
                }
                else
                {
                    PrepararInformacoesSobreSolucaoEducacional(oferta.SolucaoEducacional);
                }

                ControlarVisibilidadeMoodleIdChaveExterna(oferta.SolucaoEducacional);

                PreencherComboCertificadoTemplate(oferta.SolucaoEducacional);

                //Nome
                txtNome.Text = oferta.Nome;

                txtCodigo.Text = !republicarOferta ? oferta.Codigo : "";

                //Certificado
                ddlCertificado.ClearSelection();
                if (oferta.CertificadoTemplate != null)
                {
                    WebFormHelper.SetarValorNaCombo(oferta.CertificadoTemplate.ID.ToString(), ddlCertificado);
                }

                //Certificado Professor
                ddlCertificadoProfessor.ClearSelection();
                if (oferta.CertificadoTemplateProfessor != null)
                {
                    WebFormHelper.SetarValorNaCombo(oferta.CertificadoTemplateProfessor.ID.ToString(), ddlCertificadoProfessor);
                }

                //Id da Chave Externa
                if (!string.IsNullOrWhiteSpace(oferta.IDChaveExterna))
                    txtIDChaveExterna.Text = oferta.IDChaveExterna;

                txtDiasPrazo.Text = oferta.DiasPrazo.ToString();

                if (oferta.CodigoMoodle.HasValue)
                    txtCodigoMoodle.Text = oferta.CodigoMoodle.Value.ToString();

                //Está Na Fila de Espera ?
                rblFilaDeEspera.ClearSelection();
                WebFormHelper.SetarValorNoRadioButtonList(oferta.FiladeEspera, rblFilaDeEspera);

                //E-mail do Responsável
                if (!string.IsNullOrWhiteSpace(oferta.EmailResponsavel))
                    txtEmailResponsavel.Text = oferta.EmailResponsavel;

                //Data de Início das Inscrições
                if (oferta.DataInicioInscricoes.HasValue && !oferta.DataInicioInscricoes.Equals(DateTime.MinValue) && !republicarOferta)
                    txtDtInicioInscricoes.Text = oferta.DataInicioInscricoes.Value.ToString("dd/MM/yyyy HH:mm:ss");

                //Data Fim das Inscrições
                if (oferta.DataFimInscricoes.HasValue && !oferta.DataFimInscricoes.Equals(DateTime.MinValue) && !republicarOferta)
                    txtDtFimInscricoes.Text = oferta.DataFimInscricoes.Value.ToString("dd/MM/yyyy HH:mm:ss");

                //Inscrição On-Line ?
                rblInscricaoOnLine.ClearSelection();
                WebFormHelper.SetarValorNoRadioButtonList(oferta.InscricaoOnline, rblInscricaoOnLine);

                if (oferta != null  && oferta.TipoOferta == enumTipoOferta.Exclusiva)
                {
                    rblInscricaoOnLine.Enabled = false;
                }
                else
                {
                    rblInscricaoOnLine.Enabled = true;
                }

                //GestorUC ?
                rblGestorUC.ClearSelection();
                WebFormHelper.SetarValorNoRadioButtonList(oferta.MatriculaGestorUC, rblGestorUC);

                //Altera GestorUC ?
                rblAlteracaoPeloGestor.ClearSelection();
                WebFormHelper.SetarValorNoRadioButtonList(oferta.AlteraPeloGestorUC, rblAlteracaoPeloGestor);

                //Valor Previsto
                if (oferta.ValorPrevisto.HasValue)
                    txtValorPrevisto.Text = oferta.ValorPrevisto.Value.ToString();

                //Valor Realizado
                if (oferta.ValorRealizado.HasValue)
                    txtValorRealizado.Text = oferta.ValorRealizado.Value.ToString();

                //Link 
                txtLink.Text = !string.IsNullOrEmpty(ofertaEdicao.Link) ? ofertaEdicao.Link : "";

                //Quantidade máxima de Inscrições
                if (oferta.QuantidadeMaximaInscricoes >= 0)
                    txtQtdMaxInscricoes.Text = oferta.QuantidadeMaximaInscricoes.ToString();

                //Carga Horária
                if (oferta.CargaHoraria >= 0)
                    txtCargaHoraria.Text = oferta.CargaHoraria.ToString();

                if (oferta.PermiteCadastroTurmaPeloGestorUC.HasValue)
                    WebFormHelper.SetarValorNoRadioButtonList(oferta.PermiteCadastroTurmaPeloGestorUC.Value, rbl_PermiteCadastroTurmaPeloGestorUC);

                if (oferta.SolucaoEducacional.Fornecedor.ID == (int)enumFornecedor.MoodleSebrae)
                    txtCodigoMoodle.ReadOnly = false;
                else
                    txtCodigoMoodle.ReadOnly = true;

                divDiasPrazo.Visible = oferta.TipoOferta == enumTipoOferta.Continua;

                lblDtFimInscricoes.Text = "Data Fim das Inscrições";

                // Informação Adicional
                txtInformacaoAdicional.Text = oferta.InformacaoAdicional;

                // Exibir radio de distribuição de vagas com valor default ou valor selecionado.
                ucPermissoes2.VerificarModoDistribuicaoVagas(oferta.DistribuicaoVagas != null
                    ? oferta.DistribuicaoVagas.Value
                    : enumDistribuicaoVagasOferta.OrdemDeInscricao);

                // Permissões.
                PreencherListasDasPermissoes(oferta.SolucaoEducacional, oferta.DistribuicaoVagas == enumDistribuicaoVagasOferta.VagasPorUf);
            }

        }

        public void PrepararInformacoesSobreSolucaoEducacional(SolucaoEducacional solucaoEducacional, bool desabilitaCombo = false)
        {
            //Solução Educacional
            LimparCampos();
            txtSolucaoEducacional.Text = solucaoEducacional.ID.ToString();
            txtSolucaoEducacional.Enabled = desabilitaCombo;
        }

        private void AdicionarPermissao(Oferta oferta)
        {
            // Antes de obter as permissões, deve-se obter a modalidade de vagas da Oferta.
            oferta.DistribuicaoVagas = ucPermissoes2.ObterDistribuicaoVagas();

            if (oferta.DistribuicaoVagas == null)
                throw new AcademicoException("Campo da modalidade de vagas é obrigatório.");

            AdicionarOuRemoverPerfil(oferta);
            AdicionarOuRemoverUf(oferta);
            AdicionarOuRemoverNivelOcupacional(oferta);
            AdicionarOuRemoverAreasSubareas(oferta);
        }

        private void AdicionarOuRemoverAreasSubareas(Oferta oferta)
        {
            // Caso seja de uma categoria que possui gerenciamento de áreas e possua permissão de colaborador
            // busca as áreas e subareas selecionadas, caso aplicável.
            if (oferta.SolucaoEducacional != null &&
                oferta.SolucaoEducacional.CategoriaConteudo.PossuiGerenciamentoAreas() &&
                oferta.ListaPermissao.Any(x => x.NivelOcupacional != null && x.NivelOcupacional.ID == (int)enumNivelOcupacional.Credenciado) &&
                new ManterSubarea().ObterTodos().Any())
            {

                var permissaoCredenciado =
                    oferta.ListaPermissao.FirstOrDefault(
                        x =>
                            x.NivelOcupacional != null &&
                            x.NivelOcupacional.ID == (int)enumNivelOcupacional.Credenciado);

                if (permissaoCredenciado == null) return;

                var subAreasSelecionadas = ucPermissoes2.ObterSubareasSelecionadas();

                //#762 - Remover obrigação de seleção de pré-requisito
                //if (!subAreasSelecionadas.Any())
                //    throw new AcademicoException("Falta selecionar um pré-requisito");

                // Incluir subareas selecionadas.
                foreach (var subArea in subAreasSelecionadas)
                    permissaoCredenciado.AdicionarPermissaoSubarea(subArea);

                // Remover subareas não selecionadas.
                foreach (
                    var subArea in
                        permissaoCredenciado.Subareas.Where(
                            x => !subAreasSelecionadas.Select(s => s.ID).Contains(x.ID)).ToList())
                    permissaoCredenciado.RemoverPermissaoSubarea(subArea);
            }
        }

        private void AdicionarOuRemoverNivelOcupacional(Oferta oferta)
        {
            var niveisDto = ucPermissoes2.ObterNiveis();

            foreach (var nivelDto in niveisDto)
            {
                var nivel = new NivelOcupacional { ID = nivelDto.ID };

                if (nivelDto.IsSelecionado)
                    oferta.AdicionarNivelOcupacional(nivel);
                else
                    oferta.RemoverNivelOcupacional(nivel);
            }
        }

        private void AdicionarOuRemoverUf(Oferta oferta)
        {
            var ufsDto = ucPermissoes2.ObterUfs(oferta);

            var manterOfertaPermissao = new ManterOfertaPermissao();
            var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

            foreach (var dtoUf in ufsDto)
            {
                var uf = new Uf { ID = dtoUf.ID };

                var permissao = new OfertaPermissao
                {
                    Uf = uf,
                    Oferta = oferta
                };

                permissao = manterOfertaPermissao.ObterExistente(permissao) ?? permissao;

                // Só interessa salvar a quantidade de vagas por UF caso a opção esteja marcada.
                if (oferta.DistribuicaoVagas == enumDistribuicaoVagasOferta.VagasPorUf)
                    permissao.QuantidadeVagasPorEstado = dtoUf.Vagas ?? 0;

                permissao.Auditoria = new Auditoria(usuarioLogado.CPF); // Auditoria maravilhosa.. *palmas lentas*

                if (dtoUf.IsSelecionado)
                    ofertaEdicao.AdicionarUfs(permissao);
                else
                    ofertaEdicao.RemoverUf(uf);
            }
        }

        private void AdicionarOuRemoverPerfil(Oferta oferta)
        {
            var perfisDto = ucPermissoes2.ObterPerfis();

            foreach (var perfilDto in perfisDto)
            {
                var perfil = new Perfil
                {
                    ID = perfilDto.ID
                };

                if (perfilDto.IsSelecionado)
                    oferta.AdicionarPerfil(perfil);
                else
                    oferta.RemoverPerfil(perfil);
            }
        }

        public void InformarAcaoDeEdicao()
        {
            spanAcao.InnerText = "Edição de Oferta";
        }

        public void InformarAcaoDeCadastro()
        {
            spanAcao.InnerText = "Adição de Oferta";
        }

        public void InformarAcaoDeConsulta()
        {
            Session["OfertaEdit"] = null;
            spanAcao.InnerText = "Filtro de Oferta";
        }

        private void VerificarDatas(Oferta oferta)
        {
            ValidaDatas();

            //Data de Início das Inscrições
            if (!string.IsNullOrWhiteSpace(txtDtInicioInscricoes.Text))
                oferta.DataInicioInscricoes = CommonHelper.TratarData(txtDtInicioInscricoes.Text, "Data de Início das Inscrições").Value;

            //Data Fim das Inscrições
            if (!string.IsNullOrWhiteSpace(txtDtFimInscricoes.Text))
                oferta.DataFimInscricoes = CommonHelper.TratarData(txtDtFimInscricoes.Text, "Data Fim das Inscrições").Value;
        }


        private void PreencherCampos()
        {
            try
            {
                PreencherComboTipoOferta();
                PreencherComboSolucaoEducacional();

                int idSe;
                int.TryParse(txtSolucaoEducacional.Text, out idSe);
                if (idSe <= 0 && !IdOferta.HasValue)
                {
                    var lista = new List<ListItem>
                    {
                        new ListItem("Selecione uma solução educacional", "0")
                    };

                    WebFormHelper.PreencherListaCustomizado(lista, ddlCertificado, "Value", "Text");
                    WebFormHelper.PreencherListaCustomizado(lista, ddlCertificadoProfessor, "Value", "Text");
                    return;
                }

                PreencherComboCertificadoTemplate();
                WebFormHelper.PreencherComponenteComOpcoesSimNao(rblFilaDeEspera);
                WebFormHelper.PreencherComponenteComOpcoesSimNao(rblInscricaoOnLine);
                WebFormHelper.PreencherComponenteComOpcoesSimNao(rblGestorUC);
                WebFormHelper.PreencherComponenteComOpcoesSimNao(rblAlteracaoPeloGestor);
                WebFormHelper.PreencherComponenteComOpcoesSimNao(rbl_PermiteCadastroTurmaPeloGestorUC);
                WebFormHelper.PreencherLista(new BMPublicoAlvo().ObterTodos(), ckblstPublicoAlvo);
                txtEmailResponsavel.Text = new BMUsuario().ObterUsuarioLogado().Email;
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private List<DTONivelOcupacional> ObterNiveisOcupacionais(SolucaoEducacional solucao)
        {
            // Preencher DTOs de acordo com a seleção na SE.
            var niveisDto = new ManterNivelOcupacional().ObterTodosNivelOcupacional()
               .Select(x => new DTONivelOcupacional
               {
                   ID = x.ID,
                   Nome = x.Nome,
                   // Só habilita a edição se o nível estiver contido na Solução selecionada.
                   IsHabilitado = (solucao.ListaPermissao.Where(ns => ns.NivelOcupacional != null)
                                      .Select(ns => ns.NivelOcupacional.ID)
                                      .ToList()
                                      .Contains(x.ID)),
                   // Busca a seleção da Solução Educacional.
                   IsSelecionado = (solucao.ListaPermissao.Where(p => p.NivelOcupacional != null)
                               .Select(p => p.NivelOcupacional.ID)
                               .ToList()
                               .Contains(x.ID))

               }).ToList();

            return niveisDto;
        }

        private List<DTOPerfil> ObterPerfis(SolucaoEducacional solucao = null)
        {
            var perfisDto = new ManterPerfil().ObterTodosPerfis()
               .Select(x => new DTOPerfil
               {
                   ID = x.ID,
                   Nome = x.Nome,
                   // Só habilita a edição se o perfil estiver contido na Solução selecionada.
                   IsHabilitado = solucao != null &&
                    solucao.ListaPermissao.Where(ns => ns.Perfil != null)
                                      .Select(ns => ns.Perfil.ID)
                                      .ToList()
                                      .Contains(x.ID),
                   // Só seleciona se o perfil estiver selecionado na oferta sendo editada, caso exista.
                   // Se for cadastro, vem marcado como Default.
                   IsSelecionado = solucao.ListaPermissao.Any(so => so.Perfil != null && so.Perfil.ID == x.ID)
               }).ToList();

            return perfisDto;
        }

        private List<DTOUf> ObterUfs(SolucaoEducacional solucao)
        {
            Oferta oferta = null;

            if (Request["Id"] != null)
                oferta = new ManterOferta().ObterOfertaPorID(int.Parse(Request["Id"]));

            var isCadastro = oferta == null;

            // Objeto para facilitar a obtenção da vaga em cada loop das vagas.
            OfertaPermissao permissao;

            var ufsDto = new ManterUf().ObterTodosUf()
                .Select(x => new DTOUf
                {
                    ID = x.ID,
                    Nome = x.Sigla,
                    // Só habilita a edição se a UF da solução for a mesma do gestor logado.
                    IsHabilitado = (solucao.ListaPermissao.Where(p => p.Uf != null).Any(p => p.Uf.ID == x.ID)),
                    // Só seleciona se o nível estiver selecionado na oferta sendo editada, caso exista.
                    // Caso não exista, busca da Solução Educacional.
                    IsSelecionado = (solucao.ListaPermissao.Where(p => p.Uf != null)
                                .Select(p => p.Uf.ID)
                                .ToList()
                                .Contains(x.ID)),
                    Vagas =
                        !isCadastro && oferta != null &&
                        (permissao = oferta.ListaPermissao.FirstOrDefault(op => op.Uf != null && op.Uf.ID == x.ID)) !=
                        null
                            ? (int?)permissao.QuantidadeVagasPorEstado
                            : null
                }).ToList();

            return ufsDto;
        }

        private void PreencherListasDasPermissoes(SolucaoEducacional solucao, bool exibirVagasPorUf)
        {
            if (solucao == null)
                return;

            ucPermissoes2.PreencherNiveisOcupacionais(ObterNiveisOcupacionais(solucao));
            ucPermissoes2.PreencherPerfis(ObterPerfis(solucao));

            // Só exibir as vagas por UF caso a modalidade de distribuição de vagas seja de Vagas por UF.
            ucPermissoes2.PreencherUfs(ObterUfs(solucao), exibirVagasPorUf);
        }

        private void VerificarExibicaoAreas()
        {
            var se = AtualizarSolucaoEducacional();

            var ofertaSelecionada = Request["Id"] == null
                ? null
                : new ManterOferta().ObterOfertaPorID(int.Parse(Request["Id"]));

            // Caso editando, passa a oferta sendo editada para recuperar as seleções das áreas.
            ucPermissoes2.OfertaSelecionada = ofertaSelecionada;

            ucPermissoes2.ExibirAreas = se != null && se.CategoriaConteudo.PossuiGerenciamentoAreas();

        }

        private void PreencherComboCertificadoTemplate(SolucaoEducacional solucaoEducacional = null, Oferta oferta = null)
        {
            if (solucaoEducacional == null)
            {
                var lista = new List<ListItem>
                    {
                        new ListItem("Selecione uma solução educacional", "0")
                    };

                WebFormHelper.PreencherListaCustomizado(lista, ddlCertificado, "Value", "Text");
                WebFormHelper.PreencherListaCustomizado(lista, ddlCertificadoProfessor, "Value", "Text");
                return;
            }

            var manterCertificadoTemplate = new ManterCertificadoTemplate();

            var certificadosCategoria = manterCertificadoTemplate.ObterTemplateAtivoPorCategoriaConteudo(solucaoEducacional.CategoriaConteudo);

            var certificadosAluno = certificadosCategoria.Where(x => !x.Professor).ToList();

            var certificadosTutor = certificadosCategoria.Where(x => x.Professor).ToList();

            // Adiciona as ofertas atuais, caso esteja editando. Isso impede o usuário de editar a oferta e o certificado
            // inserido anteriormente for alterado, caso tenha sido salvo anterormente e não esteja na lista de certificados atual.
            if (oferta != null)
            {
                if (oferta.CertificadoTemplate != null && certificadosAluno.All(x => x.ID != oferta.CertificadoTemplate.ID))
                    certificadosAluno.Add(oferta.CertificadoTemplate);

                if (oferta.CertificadoTemplateProfessor != null && certificadosTutor.All(x => x.ID != oferta.CertificadoTemplateProfessor.ID))
                    certificadosTutor.Add(oferta.CertificadoTemplateProfessor);
            }

            WebFormHelper.PreencherLista(certificadosAluno, ddlCertificado, false, true);
            WebFormHelper.PreencherLista(certificadosTutor, ddlCertificadoProfessor, false, true);
        }

        private void PreencherComboTipoOferta()
        {
            var manterTipoOferta = new ManterTipoOferta();
            var listaTipoOferta = manterTipoOferta.ObterTodosTiposDeOferta();
            WebFormHelper.PreencherLista(listaTipoOferta, ddlTipo, false, true);
        }

        private void PreencherComboSolucaoEducacional()
        {
            var manterSolucaoEducacional = new ManterSolucaoEducacional();

            var listaSolucaoEducacional = manterSolucaoEducacional.ObterTodosPorGestor().Where(s => s.Ativo);

            ViewState["_SE"] = Helpers.Util.ObterListaAutocomplete(listaSolucaoEducacional);
        }

        public Oferta ObterObjetoOferta(bool republicarOferta = false)
        {
            if (IdOferta.HasValue && IdOferta.Value > 0 && !republicarOferta)
            {
                ofertaEdicao = new ManterOferta().ObterOfertaPorID(IdOferta.Value);
            }
            else
            {
                ofertaEdicao = new Oferta();
            }

            //Nome da Oferta
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                throw new AcademicoException("O Nome da Oferta deve ser informado.");
            }

            VerificarDatas(ofertaEdicao);

            //Tipo de Oferta
            if (ddlTipo.SelectedItem != null && !string.IsNullOrWhiteSpace(ddlTipo.SelectedItem.Value))
            {
                ofertaEdicao.TipoOferta = ObterTipoOfertaSelecionado();
            }

            // Se o tipo de Oferta for contínua, o valor do campo "Dias de Prazo" é obrigatório.
            if (ofertaEdicao.TipoOferta == enumTipoOferta.Continua)
            {
                if (!string.IsNullOrWhiteSpace(txtDiasPrazo.Text))
                {
                    int diasPrazo;

                    if (!int.TryParse(txtDiasPrazo.Text.Trim(), out diasPrazo))
                    {
                        throw new AcademicoException("Valor inválido para a quantidade de dias de prazo.");
                    }

                    ofertaEdicao.DiasPrazo = diasPrazo;
                }
                else
                {
                    throw new AcademicoException("Campo \"Prazo para realização (dias)\" é obrigatório.");
                }
            }

            //Solução Educacional
            var solucao = AtualizarSolucaoEducacional();

            if (solucao == null)
            {
                throw new AcademicoException("Solução Educacional é obrigatória");
            }

            ofertaEdicao.SolucaoEducacional = solucao;

            // Verificar datas de acordo com a SE caso a oferta seja contínua
            if (ofertaEdicao.TipoOferta == enumTipoOferta.Continua &&
                ofertaEdicao.SolucaoEducacional.TeraOfertasContinuas)
            {
                // Verificar datas da solução educacional, caso estejam mal formatadas.
                if (ofertaEdicao.SolucaoEducacional.Inicio == null || ofertaEdicao.SolucaoEducacional.Fim == null)
                    throw new AcademicoException(
                        string.Format(
                            "A solução educacional {0} permite ofertas contínuas, porém não possui datas definidas. Altere a solução e informe as datas de início e fim do ciclo.",
                            ofertaEdicao.SolucaoEducacional.Nome));
            }
            else
            {
                if (!ofertaEdicao.DataFimInscricoes.HasValue)
                {
                    throw new AcademicoException(
                        "A Data Fim das Inscrições deve ser informada.");
                }
            }

            //Nome
            ofertaEdicao.Nome = txtNome.Text.Trim();

            //E-mail do Responsável
            ofertaEdicao.EmailResponsavel = txtEmailResponsavel.Text.Trim();

            //Id da Chave Externa
            ofertaEdicao.IDChaveExterna = txtIDChaveExterna.Text.Trim();

            if (string.IsNullOrEmpty(ofertaEdicao.IDChaveExterna))
                ofertaEdicao.IDChaveExterna = null;

            //Codigo Moodle
            if ((!string.IsNullOrWhiteSpace(txtCodigoMoodle.Text) || !string.IsNullOrWhiteSpace(ddlCursosMoodle.Text)) &&
                 (ofertaEdicao.SolucaoEducacional != null && ofertaEdicao.SolucaoEducacional.Fornecedor.ID == (int)enumFornecedor.MoodleSebrae))
            {
                int valorCodigoMoodle;

                if (IdOferta.HasValue && IdOferta > 0)
                {
                    int.TryParse(ddlCursosMoodle.Text.Trim(), out valorCodigoMoodle);
                }
                else
                {
                    int.TryParse(txtCodigoMoodle.Text.Trim(), out valorCodigoMoodle);
                }

                if (valorCodigoMoodle <= 0)
                    throw new AcademicoException("Valor inválido para o código Moodle.");

                ofertaEdicao.CodigoMoodle = valorCodigoMoodle;
            }
            else
            {
                ofertaEdicao.CodigoMoodle = null;
            }

            //Está Na Fila de Espera ?
            if (rblFilaDeEspera.SelectedItem != null && !string.IsNullOrWhiteSpace(rblFilaDeEspera.SelectedItem.Value))
            {
                string valorInformadoParaFilaDeEspera = rblFilaDeEspera.SelectedItem.Value;

                if (valorInformadoParaFilaDeEspera.ToUpper().Equals("S"))
                    ofertaEdicao.FiladeEspera = true;
                else if (valorInformadoParaFilaDeEspera.ToUpper().Equals("N"))
                    ofertaEdicao.FiladeEspera = false;
            }

            // Permitir inscrição online.
            if (!string.IsNullOrEmpty(ddlTipo.SelectedValue) && int.Parse(ddlTipo.SelectedValue) == (int)enumTipoOferta.Exclusiva)
                ofertaEdicao.InscricaoOnline = false;
            else
                if (rblInscricaoOnLine.SelectedItem != null && !string.IsNullOrWhiteSpace(rblInscricaoOnLine.SelectedItem.Value))
            {
                string valorInformadoParaInscricaoOnLine = rblInscricaoOnLine.SelectedItem.Value;

                if (valorInformadoParaInscricaoOnLine.ToUpper().Equals("S"))
                    ofertaEdicao.InscricaoOnline = true;
                else if (valorInformadoParaInscricaoOnLine.ToUpper().Equals("N"))
                    ofertaEdicao.InscricaoOnline = false;
            }
            else
                ofertaEdicao.InscricaoOnline = false;

            if (rblGestorUC.SelectedItem != null && !string.IsNullOrWhiteSpace(rblGestorUC.SelectedItem.Value))
            {
                string valorInformadoParaGestorUC = rblGestorUC.SelectedItem.Value;

                if (valorInformadoParaGestorUC.ToString().ToUpper().Equals("S"))
                    ofertaEdicao.MatriculaGestorUC = true;
                else if (valorInformadoParaGestorUC.ToString().ToUpper().Equals("N"))
                    ofertaEdicao.MatriculaGestorUC = false;
            }
            else
            {
                ofertaEdicao.MatriculaGestorUC = false;
            }

            if (rblAlteracaoPeloGestor.SelectedItem != null && !string.IsNullOrWhiteSpace(rblAlteracaoPeloGestor.SelectedItem.Value))
            {
                string valorInformadoParaAlteracaoPeloGestor = rblAlteracaoPeloGestor.SelectedItem.Value;

                if (valorInformadoParaAlteracaoPeloGestor.ToString().ToUpper().Equals("S"))
                    ofertaEdicao.AlteraPeloGestorUC = true;
                else if (valorInformadoParaAlteracaoPeloGestor.ToString().ToUpper().Equals("N"))
                    ofertaEdicao.AlteraPeloGestorUC = false;
            }
            else
            {
                ofertaEdicao.AlteraPeloGestorUC = false;
            }

            //Valor Previsto
            if (!string.IsNullOrWhiteSpace(this.txtValorPrevisto.Text))
            {
                double valorPrevisto = 0;
                if (!double.TryParse(this.txtValorPrevisto.Text.Trim(), out valorPrevisto))
                    throw new AcademicoException("Valor Inválido para o Campo Valor Previsto.");
                else
                    ofertaEdicao.ValorPrevisto = valorPrevisto;
            }
            else
            {
                ofertaEdicao.ValorPrevisto = null;
            }

            //Valor Realizado
            if (!string.IsNullOrWhiteSpace(this.txtValorRealizado.Text))
            {
                double valorRealizado = 0;
                if (!double.TryParse(this.txtValorRealizado.Text.Trim(), out valorRealizado))
                    throw new AcademicoException("Valor Inválido para o Campo Valor Realizado.");
                else
                    ofertaEdicao.ValorRealizado = valorRealizado;
            }
            else
            {
                ofertaEdicao.ValorRealizado = null;
            }

            //Quantidade máxima de inscrições
            if (!string.IsNullOrWhiteSpace(txtQtdMaxInscricoes.Text))
            {
                int qtdMaxInscricoes;
                if (!int.TryParse(txtQtdMaxInscricoes.Text.Trim(), out qtdMaxInscricoes))
                    throw new AcademicoException("Valor Inválido para o Campo Quantidade máxima de inscrições.");

                ofertaEdicao.QuantidadeMaximaInscricoes = qtdMaxInscricoes;
            }

            //Carga Horária
            if (!string.IsNullOrWhiteSpace(txtCargaHoraria.Text))
            {
                int qtdCargaHoraria = 0;
                if (!int.TryParse(this.txtCargaHoraria.Text.Trim(), out qtdCargaHoraria))
                    throw new AcademicoException("Valor Inválido para a carga horária.");
                else
                    ofertaEdicao.CargaHoraria = qtdCargaHoraria;
            }

            // Tratamento do link cadastrado
            if (!string.IsNullOrEmpty(txtLink.Text) && !txtLink.Text.ToLower().Contains("http://") && !txtLink.Text.ToLower().Contains("https://"))
                txtLink.Text = txtLink.Text.Insert(0, "http://");

            // Link
            ofertaEdicao.Link = txtLink.Text;

            //Certificado Template
            if (ddlCertificado.Items != null && ddlCertificado.Items.Count > 0
                && !string.IsNullOrWhiteSpace(ddlCertificado.SelectedItem.Value))
            {
                ofertaEdicao.CertificadoTemplate = new ManterCertificadoTemplate().ObterCertificadoTemplatePorID(int.Parse(ddlCertificado.SelectedItem.Value));
            }

            //Certificado Template Professor
            if (ddlCertificadoProfessor.Items != null && ddlCertificadoProfessor.Items.Count > 0
                && !string.IsNullOrWhiteSpace(ddlCertificadoProfessor.SelectedItem.Value))
            {
                ofertaEdicao.CertificadoTemplateProfessor = new ManterCertificadoTemplate().ObterCertificadoTemplatePorID(int.Parse(ddlCertificadoProfessor.SelectedItem.Value));
            }

            // INFORMAÇÃO ADICIONAL.
            if (!string.IsNullOrWhiteSpace(txtInformacaoAdicional.Text))
            {
                ofertaEdicao.InformacaoAdicional = txtInformacaoAdicional.Text.Trim();
            }

            if (rbl_PermiteCadastroTurmaPeloGestorUC.SelectedIndex >= 0)
                ofertaEdicao.PermiteCadastroTurmaPeloGestorUC = rbl_PermiteCadastroTurmaPeloGestorUC.SelectedValue == "S";

            var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

            //PUBLICO ALVO
            for (int i = 0; i < ckblstPublicoAlvo.Items.Count; i++)
            {
                var idPublicoAlvo = int.Parse(ckblstPublicoAlvo.Items[i].Value);

                if (ckblstPublicoAlvo.Items[i].Selected)
                {
                    if (ofertaEdicao.ListaPublicoAlvo.All(x => x.PublicoAlvo.ID != idPublicoAlvo))
                    {
                        ofertaEdicao.ListaPublicoAlvo.Add(new OfertaPublicoAlvo
                        {
                            PublicoAlvo = new PublicoAlvo { ID = idPublicoAlvo },
                            Oferta = ofertaEdicao,
                            Auditoria = new Auditoria(usuarioLogado.CPF)
                        });
                    }
                }
                else
                {
                    if (ofertaEdicao.ListaPublicoAlvo.Any(x => x.PublicoAlvo.ID == idPublicoAlvo))
                    {
                        ofertaEdicao.ListaPublicoAlvo.Remove(ofertaEdicao.ListaPublicoAlvo.FirstOrDefault(x => x.PublicoAlvo.ID == idPublicoAlvo));
                    }
                }
            }

            AdicionarRemoverTrancamentoParaPagante(ofertaEdicao);

            AdicionarPermissao(ofertaEdicao);

            // Validando quantidade máxima de vagas na oferta
            if (ofertaEdicao.DistribuicaoVagas == enumDistribuicaoVagasOferta.VagasPorUf)
                ucPermissoes2.VerificarQuantidadeDeVagas(ofertaEdicao.QuantidadeMaximaInscricoes);

            return ofertaEdicao;
        }

        private void AdicionarRemoverTrancamentoParaPagante(Oferta oferta)
        {
            if (oferta != null && oferta.ListaNiveisTrancados != null)
            {
                var niveisTrancados = oferta.ListaNiveisTrancados.ToList();

                foreach (ListItem listItem in cblOfertaTrancadaParaPagantes.Items)
                {
                    var idNivel = int.Parse(listItem.Value);

                    if (listItem.Selected)
                        oferta.AdicionarTrancamento(idNivel);
                    else
                    {
                        var nivel = niveisTrancados.FirstOrDefault(x => x.ID == idNivel);

                        if (nivel != null)
                            oferta.RemoverTrancamento(idNivel);
                    }
                }
            }
        }

        private SolucaoEducacional AtualizarSolucaoEducacional()
        {
            SolucaoEducacional solucao = null;

            int idSe;
            int.TryParse(txtSolucaoEducacional.Text, out idSe);
            if (idSe != 0)
                solucao = new ManterSolucaoEducacional().ObterSolucaoEducacionalPorId(idSe);

            return solucao;
        }

        internal void LimparCampos()
        {
            //Tipo de Oferta
            ddlTipo.ClearSelection();

            //Solução Educacional
            txtSolucaoEducacional.Text = string.Empty;

            //Nome
            txtNome.Text = string.Empty;

            //E-mail do Responsável
            txtEmailResponsavel.Text = string.Empty;

            //Id da Chave Externa
            txtIDChaveExterna.Text = string.Empty;

            //Está Na Fila de Espera ?
            rblFilaDeEspera.ClearSelection();

            //Inscrições On-Line
            rblInscricaoOnLine.ClearSelection();

            //Gestor UC
            rblGestorUC.ClearSelection();

            //Valor Previsto
            txtValorPrevisto.Text = string.Empty;

            //Valor Realizado
            txtValorRealizado.Text = txtValorRealizado.Text;

            IdOferta = 0;
            IdSolucaoEducacional = 0;

            ddlCertificado.ClearSelection();
            ddlCertificadoProfessor.ClearSelection();
            txtDtInicioInscricoes.Text = "";
            txtDtFimInscricoes.Text = "";
            txtCargaHoraria.Text = "";
            txtQtdMaxInscricoes.Text = "";
            txtDiasPrazo.Text = "";

            ddlTipo.Enabled = true;
            txtSolucaoEducacional.Enabled = true;
        }

        /// <summary>
        /// Obtém os cursos do Moodle a partir do ID da chave externa da solução educacional selecionada ou salva, caso o fornecedor da solução educacional seja o Moodle.
        /// </summary>
        private void AtualizarCursos(Oferta oferta = null)
        {
            int idSe;
            int.TryParse(txtSolucaoEducacional.Text, out idSe);

            if (idSe > 0)
            {
                SolucaoEducacional solucaoEducacional;

                if (ofertaEdicao == null || ofertaEdicao.ID == 0)
                    solucaoEducacional = new ManterSolucaoEducacional().ObterSolucaoEducacionalPorId(idSe);
                else
                    solucaoEducacional = ofertaEdicao.SolucaoEducacional;

                if (solucaoEducacional != null && solucaoEducacional.Fornecedor.ID == (int)enumFornecedor.MoodleSebrae)
                {
                    if (!string.IsNullOrEmpty(solucaoEducacional.IDChaveExterna))
                    {
                        int IDChaveExterna = 0;

                        try
                        {
                            IDChaveExterna = int.Parse(solucaoEducacional.IDChaveExterna.Trim());

                            txtIDChaveExterna.Enabled = false;
                            Label17.Visible = false;
                            txtCodigoMoodle.Visible = false;

                            panelAlertaIDChaveExterna.Visible = false;
                            panelAlertaCodigoMoodle.Visible = false;
                            panelCursosMoodle.Visible = true;

                            var cursos = new BMCurso().ObterPorCategoria(IDChaveExterna);

                            ddlCursosMoodle.Items.Clear();

                            ddlCursosMoodle.Items.Add(new ListItem("-- Selecione --", ""));

                            foreach (var curso in cursos)
                            {
                                var cursoItem = new ListItem(curso.NomeCompleto, curso.ID.ToString());

                                ddlCursosMoodle.Items.Add(cursoItem);
                            }

                            txtCodigoMoodle.Text = ddlCursosMoodle.SelectedValue;

                        }
                        catch (FormatException)
                        {
                            labelChaveExternaError.Text = "'" + solucaoEducacional.IDChaveExterna + "'";
                            labelSolucaoEducacionalError.Text = "'" + solucaoEducacional.Nome + "'";

                            panelAlertaIDChaveExterna.Visible = true;
                        }
                    }
                    else
                    {
                        panelAlertaCodigoMoodle.Visible = true;
                        panelCursosMoodle.Visible = false;
                    }
                }
                else
                {
                    panelAlertaCodigoMoodle.Visible = false;
                    panelCursosMoodle.Visible = false;

                    Label6.Visible = true;
                    Label17.Visible = true;
                    txtCodigoMoodle.Visible = true;
                }


                if (oferta != null && oferta.CodigoMoodle != null)
                    ddlCursosMoodle.SelectedValue = oferta.CodigoMoodle.ToString();
            }
        }

        protected void ddlCursosMoodle_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCodigoMoodle.Text = ddlCursosMoodle.SelectedValue;
        }

        protected void ddlTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidaDatas();

            lblDtFimInscricoes.Text = "Data Fim das Inscrições*";
            var tipoSelecionado = ObterTipoOfertaSelecionado();

            divDiasPrazo.Visible = tipoSelecionado == enumTipoOferta.Continua;

            if (!string.IsNullOrEmpty(ddlTipo.SelectedValue) && tipoSelecionado == enumTipoOferta.Exclusiva)
            {
                WebFormHelper.SetarValorNoRadioButtonList(false, rblInscricaoOnLine, true);
            }
            else
            {
                rblInscricaoOnLine.ClearSelection();
                rblInscricaoOnLine.Enabled = true;

                if (tipoSelecionado == enumTipoOferta.Continua)
                {
                    lblDtFimInscricoes.Text = "Data Fim das Inscrições";
                }
            }
        }

        public void ValidaDatas()
        {
            txtDtInicioInscricoes.Text = CommonHelper.FormataDataHora(txtDtInicioInscricoes.Text);
            txtDtFimInscricoes.Text = CommonHelper.FormataDataHora(txtDtFimInscricoes.Text);
        }

        protected void txtSolucaoEducacional_OnTextChanged(object sender, EventArgs e)
        {
            AtualizarCursos();

            int idSe;
            if (int.TryParse(txtSolucaoEducacional.Text, out idSe))
            {
                var solucaoEducacional = new ManterSolucaoEducacional().ObterSolucaoEducacionalPorId(idSe);

                PreencherComboCertificadoTemplate(solucaoEducacional);

                ControlarVisibilidadeMoodleIdChaveExterna(solucaoEducacional);

                PreencherListasDasPermissoes(solucaoEducacional, false);
            }
        }

        void ControlarVisibilidadeMoodleIdChaveExterna(SolucaoEducacional solucaoEducacional)
        {
            if (solucaoEducacional.Fornecedor.ID == (int)enumFornecedor.UCSebrae)
            {
                divChaveExterna.Visible = false;
                divCodigoMoodle.Visible = false;
            }
            else if (solucaoEducacional.Fornecedor.ID == (int)enumFornecedor.MoodleSebrae)
            {
                divChaveExterna.Visible = false;
                divCodigoMoodle.Visible = true;
            }
            else
            {
                divChaveExterna.Visible = true;
                divCodigoMoodle.Visible = false;
            }

            if (!IsAdmin && !IsGestor)
            {
                divCodigoMoodle.Visible = false;
                divChaveExterna.Visible = false;
            }
        }

        public void ValidarSubAreasSelecionadas(Oferta oferta)
        {
            //Retirado por que não é obrigatório a seleção de uma área ou subárea - Stefany que falou by tiago.
            //if(oferta.SolucaoEducacional != null &&
            //    oferta.SolucaoEducacional.CategoriaConteudo.PossuiGerenciamentoAreas() &&
            //    oferta.ListaPermissao.Any(x => x.NivelOcupacional != null && x.NivelOcupacional.ID == (int)enumNivelOcupacional.Credenciado))
            //{
            //    if (!ucPermissoes2.ObterSubareasSelecionadas().Any())
            //    {
            //        throw new AcademicoException("Escolha área e subárea");
            //    }
            //}
        }
    }

    public class CadastrarOfertaEventArgs : EventArgs
    {
        public Oferta InformacoesDaOfertaCadastrada { get; set; }

        public CadastrarOfertaEventArgs(Oferta oferta)
        {
            InformacoesDaOfertaCadastrada = oferta;
        }
    }

}