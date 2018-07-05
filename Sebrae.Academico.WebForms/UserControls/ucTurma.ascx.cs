using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.WebForms.Cadastros.Oferta;

namespace Sebrae.Academico.WebForms.UserControls
{
    /// <summary>
    /// User control utilizado no cadastro de uma turma.
    /// </summary>
    public partial class ucTurma : UserControl
    {
        public ucTurma()
        {
            CarregarLoad = true;
        }

        #region Variáveis úteis armazenadas na memória durante a requisição. Não devem ser chamadas diretamente, e sim pelos métodos abaixo.

        private Turma Turma { get; set; }
        private bool? IsAdministrador { get; set; }
        public bool CarregarLoad { get; set; }
        private Turma TurmaAntesModificacao;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack && CarregarLoad)
            {
                CarregarDados();
            }
        }

        public void CarregarDados()
        {
            if (ObterTurma() == null)
            {
                RegrasAdicao();
            }
            else
            {
                RegrasEdicao();
            }

            // REGRAS GERAIS
            // 1. Nota só poderá ser alterada pelo Administrador.
            divNotaMinima.Disabled = !ObterIsAdministrador();

            // 2. Desabilitar a Oferta quando não houver SE selecionada.
            txtOfertaTurma.Enabled = false;
            txtOfertaTurma.Attributes.Add("disabled", "disabled");

            // PREENCHER CAMPOS DA TELA.
            PreencherCampos(null, Request["Republicar"] != null && Request["Republicar"] == "Sim");
            ValidarExibicao();
        }

        private void ValidarExibicao()
        {
            if (new ManterUsuario().ObterUsuarioLogado().IsGestor())
            {
                pnlWIFI.Visible = false;
            }
        }

        /// <summary>
        /// Atualizar SEs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PreencherSolucaoEducacional(object sender, EventArgs e)
        {
            SolucaoEducacional se = null;

            // Caso seja Cadastro, busca as SEs do banco, caso não, busca somente a SE da própria Turma.
            if (ObterTurma() == null)
            {
                ViewState["_Se_ucTurma"] = Helpers.Util.ObterListaAutocomplete(new ManterSolucaoEducacional().ObterTodosPorGestor());
            }
            else
            {
                se = ObterTurma().Oferta.SolucaoEducacional;

                ViewState["_Se_ucTurma"] = Helpers.Util.ObterListaAutocomplete(new List<SolucaoEducacional> { se }.AsQueryable());
                txtSolucao.Text = se.ID.ToString();
            }

            // Verifica a exibição dos campos Responsável e Consultor, de acordo com a SE selecionada.
            AtualizarExibicaoResponsavelConsultor(se);
        }

        protected void txtSolucao_OnTextChanged(object sender, EventArgs e)
        {
            var solucaoEducacional = ObterSolucaoEducacionalDropDown();

            if (solucaoEducacional != null)
            {
                try
                {
                    // Preencher Ofertas.
                    PreencherOfertas(solucaoEducacional);

                    // Preencher Questionários.
                    PreencherQuestionarios(solucaoEducacional.CategoriaConteudo);

                    if (solucaoEducacional.ListaOferta.Any())
                    {
                        txtOfertaTurma.Enabled = true;
                        txtOfertaTurma.Attributes.Remove("disabled");
                    }

                    PreencherStatus();
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }

            AtualizarExibicaoResponsavelConsultor();
        }

        protected void txtOfertaTurma_OnTextChanged(object sender, EventArgs e)
        {
            // Obtém a oferta da turma de edição ou da oferta do DropDown.
            var oferta = ObterOfertaDropDown();

            if (oferta != null)
            {
                AtualizarVagasOferta(oferta);
                if (oferta.TipoOferta != enumTipoOferta.Continua)
                {
                    divDataDisparoLinkPesquisa.Visible = true;
                }

                // Atualizar Grupos do Moodle somente no Cadastro.
                if (ObterTurma() == null)
                {
                    AtualizarGruposDoMoodle();
                }
            }
        }

        protected void ddlTipoTutoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Só permite exibição dos professores\ caso as opções Pró-Ativa e Reativa estejam marcadas.

            if (ddlTipoTutoria.SelectedIndex == 1 || ddlTipoTutoria.SelectedIndex == 2)
            {
                divTutores.Visible = true;

                AtualizarExibicaoResponsavelConsultor();
            }
            else
            {
                divTutores.Visible = false;
            }
        }

        private void AtualizarExibicaoResponsavelConsultor(SolucaoEducacional se = null)
        {
            se = se ?? ObterSolucaoEducacionalDropDown();

            if (se != null)
            {
                // Caso seja de Formação de Formadores, exibir os campos de Responsável e Consultor.
                divResponsavelConsultor.Visible =
                    new ManterCategoriaConteudo().IsFormacaoDeFormadores(se.CategoriaConteudo);

                var consultores = new ManterUsuario().ObterTodosPorPerfil(enumPerfil.ConsultorEducacional).AsQueryable();

                ViewState["_ConsultoresEducacionais"] = Helpers.Util.ObterListaAutocomplete(consultores);
            }
            else
            {
                divResponsavelConsultor.Visible = false;
            }
        }

        protected void rblSelecionaAcessoWifi_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            // Exibe o campo Local quando o campo "Acesso a Wi-Fi" seja Sim.
            divAcessoWifi.Visible = rblSelecionaAcessoWifi.SelectedValue == "1";
        }

        protected void btnSalvar_OnClick(object sender, EventArgs e)
        {
            try
            {
                SalvarTurma();
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        public void ExibirModalConfirmacaoExclusaoNotasMatriculasTurma(Turma turma)
        {
            txtNumeroMatriculasTurmaAfetados.InnerText = turma.ListaMatriculas.Count().ToString();
            pnlModalConfirmacaoExclusaoNotasMatriculasTurma.Visible = true;
            return;
        }

        protected void OcultarModalConfirmacaoExclusaoNotasMatriculasTurma_Click(object sender, EventArgs e)
        {
            pnlModalConfirmacaoExclusaoNotasMatriculasTurma.Visible = false;
        }

        protected void btnConfirmaExclusaoNotasMatriculasTurma_Click(object sender, EventArgs e)
        {
            pnlModalConfirmacaoExclusaoNotasMatriculasTurma.Visible = false;

            //REMOVE NOTAS DAS MATRICULAS DA TURMA
            RemoveNotasMatriculasTurma();
            
            //SALVA TURMA E GRAVA DADOS DE AUDITORIA
            SalvarTurma(false);
        }

        protected void btnCancelarConfirmacaoExclusaoNotasMatriculasTurma_Click(object sender, EventArgs e)
        {
            pnlModalConfirmacaoExclusaoNotasMatriculasTurma.Visible = false;

            return;
        }

        #region Métodos de assistência

        public void RegrasAdicao()
        {
            // REGRAS PARA CRIAÇÃO DA TURMA
            // 1. Nota mínima padrão.
            txtValorNotaMinima.Text = "7";

            // 2. Esconder campo "Pode ser acessado após o Término?" na criação.
            divAcessoPosTermino.Visible = false;

            // 3. Esconder o campo ID da Chave Externa no cadastro.
            divChaveExterna.Visible = false;

            // 4. Esconder o campo Código no cadastro.
            divCodigoTurma.Visible = false;

            // 5. Esconder o campo Professor no início do Cadastro
            divTutores.Visible = false;

            // 6. Esconder o campo local no início do cadastro.
            divAcessoWifi.Visible = false;

            // 7. Habilitar SE na adição.
            txtSolucao.Enabled = true;
            txtSolucao.Attributes.Remove("disabled");

            // 8. Desabilitar Oferta na adição.
            txtOfertaTurma.Enabled = false;
            txtOfertaTurma.Attributes.Add("disabled", "disabled");
        }

        public void RegrasEdicao()
        {
            // REGRAS PARA EDIÇÃO DA TURMA
            // 1. Exibir campo "Pode ser acessado após o Término?" somente quando o Fornecedor da SE não for UCSEBRAE e o ID da Chave Externa não for preenchido.
            divAcessoPosTermino.Visible = (ObterTurma().Oferta.SolucaoEducacional.Fornecedor.ID != (int)enumFornecedor.UCSebrae
                                           && !string.IsNullOrEmpty(ObterTurma().IDChaveExterna));

            // 2. Exibir campo ID da Chave Externa somente para o Administrador, somente na edição e somente se possuir ID da Chave Externa.
            divChaveExterna.Visible = ObterIsAdministrador() && ObterTurma().IDChaveExterna != null;

            // 3. Esconder campo Grupos Moodle na edição caso possua ID da Chave Externa.
            var selecionarChaveExterna = ObterIsAdministrador() && ObterTurma().IDChaveExterna == null;

            divGruposMoodle.Visible = selecionarChaveExterna;
            if (selecionarChaveExterna)
                AtualizarGruposDoMoodle();

            // 4. Desabilitar SE na edição.
            txtSolucao.Enabled = false;
            txtSolucao.Attributes.Add("disabled", "disabled");

            // 5. Desabilitar Oferta na edição.
            txtOfertaTurma.Enabled = false;
            txtOfertaTurma.Attributes.Add("disabled", "disabled");

            // 6. Data de Disparo de Link de pesquisa
            if (ObterTurma().Oferta.TipoOferta != enumTipoOferta.Continua)
            {
                divDataDisparoLinkPesquisa.Visible = true;
            }
        }

        public void SelecionarOferta(Oferta oferta, bool desabilitarCampos)
        {
            if (desabilitarCampos)
            {
                // Desabilitar SE na adição.
                txtSolucao.Enabled = false;
                txtSolucao.Attributes.Add("disabled", "disabled");

                // Desabilitar Oferta na adição.
                txtOfertaTurma.Enabled = false;
                txtOfertaTurma.Attributes.Add("disabled", "disabled");
            }

            txtSolucao.Text = oferta.SolucaoEducacional.ID.ToString();
            ViewState["_Se_ucTurma"] = Helpers.Util.ObterListaAutocomplete(new List<SolucaoEducacional> { oferta.SolucaoEducacional }.AsQueryable());

            txtOfertaTurma.Text = oferta.ID.ToString();
            ViewState["_Oferta_ucTurma"] = Helpers.Util.ObterListaAutocomplete(new List<Oferta> { oferta }.AsQueryable());

            AtualizarVagasOferta(oferta);
        }

        /// <summary>
        /// Obter a Turma pelo ID da Request.
        /// </summary>
        /// <returns></returns>
        public Turma ObterTurma(int IdTurma = 0)
        {
            if (Turma != null)
                return Turma;

            var manterTurma = new ManterTurma();

            if (IdTurma != 0)
            {
                Turma = manterTurma.ObterTurmaPorID(IdTurma);
                TurmaAntesModificacao = (Turma)Turma.Clone();
                return Turma;
            }

            if (txtIdTurma.Text != "" && txtIdTurma.Text != "0")
            {
                Turma = manterTurma.ObterTurmaPorID(int.Parse(txtIdTurma.Text));
                TurmaAntesModificacao = (Turma)Turma.Clone();
                return Turma;
            }

            if (Request["id"] != null)
            {
                Turma = manterTurma.ObterTurmaPorID(int.Parse(Request["id"]));
                TurmaAntesModificacao = (Turma)Turma.Clone();
            }


            return Turma;
        }

        /// <summary>
        /// Verificar se o usuário logado é Administrador, e armazenar na memória para não buscar no banco novamente durante a Request.
        /// </summary>
        /// <returns></returns>
        private bool ObterIsAdministrador()
        {
            if (IsAdministrador != null)
                return IsAdministrador.Value;

            IsAdministrador = new ManterUsuario().PerfilAdministrador();

            return (bool)IsAdministrador;
        }

        /// <summary>
        /// Obtém o objeto Solução Educacional selecionado no DropDownList da tela.
        /// </summary>
        /// <returns></returns>
        private SolucaoEducacional ObterSolucaoEducacionalDropDown()
        {
            var id = !string.IsNullOrWhiteSpace(txtSolucao.Text) ? int.Parse(txtSolucao.Text) : 0;

            return id == 0 ? null : new ManterSolucaoEducacional().ObterSolucaoEducacionalPorId(id);
        }

        /// <summary>
        /// Obtém o objeto Oferta selecionado no DropDownList da tela.
        /// </summary>
        /// <returns></returns>
        private Oferta ObterOfertaDropDown(string ofertaTurma = null)
        {
            var id = 0;
            if (!int.TryParse(ofertaTurma, out id))
            {
                int.TryParse(txtOfertaTurma.Text, out id) ;
            }

            return new ManterOferta().ObterOfertaPorID(id);
        }

        public void ValidarCampos()
        {
            // REGRAS DE VALIDAÇÃO.
            // 1. Caso o campo "Acesso a Wi-Fi" seja Sim, o campo "Locais" deverá ser obrigatório.
            if (rblSelecionaAcessoWifi.SelectedValue == "1" && rblAcessoWifi.SelectedValue == null)
                throw new AcademicoException("Campo \"Locais\" obrigatório para turmas com acesso a Wi-Fi.");


            // VALIDAÇÕES DE CAMPOS

            #region Oferta

            var oferta = ObterOfertaDropDown();

            if (oferta == null)
                throw new AcademicoException("Oferta é obrigatória.");

            // Caso o Fornecedor da SE seja Moodle e a Oferta não possua ID da Chave Externa, reprova a validação.
            //if(oferta.SolucaoEducacional.Fornecedor.ID == (int)enumFornecedor.MoodleSebrae && oferta.IDChaveExterna == null)
            //    throw new AcademicoException("A Oferta não possui ID da Chave Externa.");

            #endregion

            #region Grupo Moodle

            if (oferta.SolucaoEducacional.Fornecedor.ID == (int)enumFornecedor.MoodleSebrae && oferta.IDChaveExterna == null)
            {
                if (ddlGruposMoodle.SelectedValue == "0")
                {
                    throw new AcademicoException("Grupo do Moodle é obrigatório para Soluções Educacionais com o fornecedor Moodle. Por favor, selecione um Grupo do Moodle ou selecione a opção \"- Criar Grupo -\" para criar um novo grupo no Moodle.");
                }
            }

            #endregion

            // Nome
            if (string.IsNullOrEmpty(txtNomeTurma.Text))
                throw new AcademicoException("Nome da Turma é obrigatório.");

            if (Request["Id"] == null)
            {
                if (!string.IsNullOrEmpty(txtNomeTurma.Text))
                {
                    var turmaDuplicada = new ManterTurma().ObterTurmaPorFiltro(txtNomeTurma.Text, null, oferta.ID, 0);
                    if (turmaDuplicada.Any())
                    {
                        throw new AcademicoException("Nome da Turma já utilizado para essa Oferta.");
                    }
                }
            }

            // Tipo Tutoria
            if (ddlTipoTutoria.SelectedIndex == 0)
                throw new AcademicoException("Tipo de Tutoria é obrigatório.");

            // Quantidade máxima de inscrições.
            // #1084 - Ajuste de quantidade de vagas em Turma
            if (!string.IsNullOrEmpty(txtQTMaxInscricoes.Text))
            {
                var qnt = int.Parse(txtQTMaxInscricoes.Text);

                if (qnt < 1)
                    throw new AcademicoException("A quantidade de inscrições da turma não pode ser 0 ou negativa.");

                var totalUtilizado = oferta.ListaTurma.Select(x => x.QuantidadeMaximaInscricoes).Sum();

                if (Request["Id"] != null)
                {
                    int idTurma = int.Parse(Request["id"]);
                    totalUtilizado = oferta.ListaTurma.Where(x => x.ID != idTurma).Select(x => x.QuantidadeMaximaInscricoes).Sum();
                }

                var total = oferta.QuantidadeMaximaInscricoes;

                var totalDisponivel = total - totalUtilizado;

                if (qnt > totalDisponivel)
                    throw new AcademicoException("A quantidade de inscrições da turma não pode ser maior que " + totalDisponivel);
            }

            // Data inicial.
            var dataInicio = CommonHelper.TratarData(TxtDtInicio.Text, "Data de Início");
            if (!dataInicio.HasValue)
                throw new AcademicoException("Data de início é obrigatória.");

            // Data Final.
            var dataFinal = CommonHelper.TratarData(TxtDtFinal.Text, "Data Final");
            if (!dataFinal.HasValue)
                throw new AcademicoException("Data de final é obrigatória.");

            var permiteAlteracaoStatus = oferta.SolucaoEducacional.CategoriaConteudo.PossuiGerenciamentoStatus();

            if (permiteAlteracaoStatus && dataFinal == null)
                throw new AcademicoException("A data final é obrigatória em turmas que possuem Status");

            if (dataFinal.HasValue)
            {
                if (dataFinal.Value.Date < dataInicio.Value.Date)
                    throw new AcademicoException("A data final não pode ser menor que a data de inicio.");
            }

            // Nota mínima.
            if (!string.IsNullOrWhiteSpace(txtValorNotaMinima.Text))
            {
                decimal valorNotaMinima = 0;
                if (!decimal.TryParse(txtValorNotaMinima.Text.Trim(), out valorNotaMinima))
                    throw new AcademicoException("Nota Mínima inválida.");

                if (valorNotaMinima > 10 || valorNotaMinima < 0)
                    throw new AcademicoException("A Nota Mínima deve estar entre 0 e 10.");
            }

            // Status
            int idStatus;
            if (int.TryParse(ddlStatus.SelectedValue, out idStatus))
            {
                var statusSelecionado = (enumStatusTurma)idStatus;

                // Justificativa de cancelamento
                if (statusSelecionado == enumStatusTurma.Cancelada && string.IsNullOrWhiteSpace(txtJustificativa.Text))
                    throw new AcademicoException("Justificativa de cancelamento é obrigatória.");
            }
            else
                if (permiteAlteracaoStatus)
                throw new AcademicoException("Status inválido");
        }

        public Turma ObterObjetoTurma(bool criaNovoObjeto = false)
        {
            // Verificar campos
            ValidarCampos();

            Turma turma;
            if (criaNovoObjeto)
                turma = new Turma();
            else
                turma = ObterTurma() ?? new Turma();

            // Oferta.
            turma.Oferta = new ManterOferta().ObterOfertaPorID(int.Parse(txtOfertaTurma.Text));

            // Número sequencial (somente caso seja cadastro).
            if (ObterTurma() == null || criaNovoObjeto)
                turma.Sequencia = new ManterTurma().ObterProximoCodigoSequencial(turma.Oferta);

            // Nome.
            turma.Nome = txtNomeTurma.Text.Trim();

            // Tipo Tutoria.
            turma.TipoTutoria = ddlTipoTutoria.SelectedItem.Value;

            // Professores.
            turma.Professores = ObterProfessoresSelecionados(turma);

            // Local.
            turma.Local = txtLocal.Text;

            // Data Início
            turma.DataInicio = CommonHelper.TratarData(TxtDtInicio.Text, "Data de Início").Value;

            // Data Final
            turma.DataFinal = CommonHelper.TratarData(TxtDtFinal.Text.Trim(), "Data Final");

            turma.InAvaliacaoAprendizagem = rblInAvaliacaoAprendizagem.SelectedItem != null && (rblInAvaliacaoAprendizagem.SelectedItem.Value.ToUpper().Equals(Constantes.SiglaSim) || (rblInAvaliacaoAprendizagem.SelectedItem.Value.ToUpper().Equals(Constantes.SiglaSim)));

            // Nota Mínima.
            turma.NotaMinima = !string.IsNullOrEmpty(txtValorNotaMinima.Text.Trim()) ? (decimal?)(decimal.Parse(txtValorNotaMinima.Text)) : null;

            // Aberta para novas inscrições.
            turma.InAberta = rblInAberta.SelectedItem != null && (rblInAberta.SelectedItem.Value.ToUpper().Equals(Constantes.SiglaSim) || (rblInAberta.SelectedItem.Value.ToUpper().Equals(Constantes.SiglaSim)));

            // Pode ser acessado após o término.
            turma.AcessoAposConclusao = rblAcessoAposConclusao.SelectedItem != null && (rblAcessoAposConclusao.SelectedItem.Value.ToUpper().Equals(Constantes.SiglaSim) || (rblAcessoAposConclusao.SelectedItem.Value.ToUpper().Equals(Constantes.SiglaSim)));

            // Quantidade Máxima de Inscrições.
            turma.QuantidadeMaximaInscricoes = string.IsNullOrEmpty(txtQTMaxInscricoes.Text) ? TotalVagasDisponiveis(turma.Oferta) : int.Parse(txtQTMaxInscricoes.Text);

            // Questionários Associação.
            turma.ListaQuestionarioAssociacao = ObterQuestionariosAssociacao(turma);

            // Acesso Wifi
            if (rblSelecionaAcessoWifi.SelectedValue == "1")
            {
                turma.AcessoWifi = int.Parse(rblAcessoWifi.SelectedValue);
            }

            // Responsável.
            var responsavel = ucLupaUsuario.SelectedUser;

            // Caso o responsável selecionado não seja o anterior, permite inclusão no log.
            turma.PermitirLogResponsavel = responsavel != null && turma.Responsavel != null &&
                                           responsavel.ID != turma.Responsavel.ID;

            if (turma.PermitirLogResponsavel)
            {
                turma.LogResponsavel = turma.Responsavel;
            }

            if (responsavel != null)
                turma.Responsavel = responsavel;

            // Consultor.
            var consultor = ObterConsultorEducacionalDropDown();

            // Caso o consultor selecionado não seja o anterior, permite inclusão no log.
            turma.PermitirLogConsultor = consultor != null && turma.ConsultorEducacional != null &&
                                           consultor.ID != turma.ConsultorEducacional.ID;

            if (turma.PermitirLogConsultor)
            {
                turma.LogConsultorEducacional = turma.ConsultorEducacional;
            }

            if (consultor != null)
                turma.ConsultorEducacional = consultor;

            // Status
            int idStatus;
            if (int.TryParse(ddlStatus.SelectedValue, out idStatus) && turma.Oferta.SolucaoEducacional.CategoriaConteudo.PossuiGerenciamentoStatus())
            {
                var statusSelecionado = (enumStatusTurma)idStatus;

                if ((statusSelecionado == enumStatusTurma.EmAndamento || statusSelecionado == enumStatusTurma.Realizada) && (turma.ID != 0 && turma.Status != statusSelecionado))
                    throw new AcademicoException("O status selecionado só pode ser alterado automaticamente pelo SGUS. Por favor, escolha um Status válido.");

                turma.Status = statusSelecionado;

                //Justificativa.
                if (statusSelecionado == enumStatusTurma.Cancelada)
                    turma.AdicionarJustificativa(txtJustificativa.Text);
            }
            else
                turma.Status = null;

            return turma;
        }

        // Quantidade máxima de inscrições.
        // #1084 - Ajuste de quantidade de vagas em Turma
        public int TotalVagasDisponiveis(Oferta oferta)
        {
            var total = oferta.QuantidadeMaximaInscricoes;
            var totalUtilizado = oferta.ListaTurma.Select(x => x.QuantidadeMaximaInscricoes).Sum();
            var totalDisponivel = total - totalUtilizado;

            return totalDisponivel;
        }

        // Quantidade máxima de inscrições.
        // #1084 - Ajuste de quantidade de vagas em Turma
        private void AtualizarVagasOferta(Oferta oferta)
        {
            if (oferta != null)
            {
                var idUf = new BMUsuario().ObterUfLogadoSeGestor();

                var total = oferta.QuantidadeMaximaInscricoes;
                var totalUtilizado = oferta.ListaTurma.Select(x => x.QuantidadeMaximaInscricoes).Sum();

                var totalDisponivel = total - totalUtilizado;
                // Atualiza o texto do máximo de inscrições.
                lblTxtMaxInscricoesHelp.InnerText = "Vagas utilizadas: " + totalUtilizado + " | Vagas Disponíveis: " + (totalDisponivel < 0 ? 0 : totalDisponivel);
            }
            else
            {
                lblTxtMaxInscricoesHelp.InnerText = "Selecione uma Oferta para ver a quantidade de vagas disponíveis.";
            }
        }

        private int ObterTotalDeVagasUtilizadas(Oferta oferta, int idUfUsuario)
        {
            // Se for gestor pega os dados relativos ao uso no estado dele
            return idUfUsuario > 0 ?
                oferta.ListaMatriculaOferta.Count(x => !x.IsCancelado() && x.UF.ID == idUfUsuario) :
                oferta.ListaMatriculaOferta.Count(x => !x.IsCancelado());
        }


        private int ObterTotalDeVagas(Oferta oferta, int idUf)
        {
            if (idUf > 0)
            {
                var permissao = oferta.ListaPermissao.FirstOrDefault(x => x.Uf != null && x.Uf.ID == idUf);
                return permissao != null ? permissao.QuantidadeVagasPorEstado : 0;
            }

            return oferta.QuantidadeMaximaInscricoes;
        }

        public void AtualizarGruposDoMoodle()
        {

            var oferta = ObterOfertaDropDown(txtOfertaTurma.Text);

            // Verificar se é curso do Moodle.
            if (oferta != null)
            {
                if (oferta.SolucaoEducacional.Fornecedor.ID == (int)enumFornecedor.MoodleSebrae)
                {
                    if (oferta.IDChaveExterna == null)
                    {
                        divGruposMoodle.Visible = true;
                        ddlGruposMoodle.Enabled = false;

                        // Avisa caso a Oferta não possua ID da Chave Externa.
                        var opcaoVazia = new List<ListItem> { new ListItem { Text = "A Oferta selecionada não possui ID da Chave Externa", Value = "0" } };
                        WebFormHelper.PreencherLista(opcaoVazia.Select(x => new { ID = x.Value, Nome = x.Text }).ToList(), ddlGruposMoodle);

                        return;
                    }

                    try
                    {

                        ManterGroupMoodle groupMoodle = new ManterGroupMoodle();
                        var group = groupMoodle.ObterTodos();
                        var gruposFiltrados = group.Where(g => g.IdNumber == oferta.IDChaveExterna);

                        WebFormHelper.PreencherLista(gruposFiltrados.Select(p => new { ID = p.ID, Nome = p.Name }).ToList(), ddlGruposMoodle, false, true);
                        divGruposMoodle.Visible = true;
                        ddlGruposMoodle.Enabled = true;
                    }
                    catch (Exception ex)
                    {
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta,
                            "Não foi possivel listar os grupos do Moodle", "ListarTurma.aspx");
                    }
                }
                else
                {
                    divGruposMoodle.Visible = false;
                }
            }
            else
            {
                divGruposMoodle.Visible = false;
            }
        }

        private string ObterJSON(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        private void AtualizarTitulo()
        {
            divAcao.Visible = true;
            textoAcao.InnerText = (ObterTurma() == null ? "Cadastrar" : "Editar") + " turma";
        }

        private void PreencherOfertas(SolucaoEducacional solucaoEducacional = null)
        {
            // Caso possua turma, preencher somente com uma Oferta.
            if (ObterTurma() != null)
            {
                var oferta = ObterTurma().Oferta;

                txtOfertaTurma.Text = oferta.ID.ToString();
                ViewState["_Oferta_ucTurma"] = Helpers.Util.ObterListaAutocomplete(new List<Oferta> { oferta }.AsQueryable());

                return;
            }

            string texto;

            // Habilitar a Oferta quando selecionar alguma SE que possua ofertas.
            if (solucaoEducacional == null)
            {
                texto = "Selecione uma Solução Educacional";
            }
            else
            {
                if (solucaoEducacional.ListaOferta != null && !solucaoEducacional.ListaOferta.Any())
                {
                    texto = "Solução Educacional não possui Ofertas";
                }
                else
                {
                    txtOfertaTurma.Text = "";
                    if (solucaoEducacional.ListaOferta != null)
                        ViewState["_Oferta_ucTurma"] = Helpers.Util.ObterListaAutocomplete(solucaoEducacional.ListaOferta.AsQueryable());

                    // Habilita a seleção da Oferta.
                    txtOfertaTurma.Enabled = true;
                    txtOfertaTurma.Attributes.Remove("disabled");
                    return;
                }
            }

            txtOfertaTurma.Enabled = false;
            txtOfertaTurma.Attributes.Add("disabled", "disabled");

            // Exibe o motivo de não ter Ofertas para selecionar.
            txtOfertaTurma.Attributes.Add("data-mensagemVazia", texto);
            ViewState["_Oferta_ucTurma"] = null;
        }

        private void PreencherQuestionarios(CategoriaConteudo categoriaConteudo, Turma turma = null)
        {
            try
            {
                var manterQuestionario = new ManterQuestionario();
                var usuario = new ManterUsuario().ObterUsuarioLogado();

                var listaQuestionariosDePesquisa = usuario.IsGestor()
                    ? manterQuestionario.ObterQuestionariosDePesquisaPorCategoriaGestor(categoriaConteudo.ID, usuario.UF)
                    : manterQuestionario.ObterQuestionariosDePesquisaPorCategoria(categoriaConteudo.ID);

                listaQuestionariosDePesquisa = ObterQuestionariosAssociados(Turma, listaQuestionariosDePesquisa.ToList(), null);

                ViewState["_SE_QuestionarioPre"] =
                    Helpers.Util.ObterListaAutocomplete(listaQuestionariosDePesquisa.OrderBy(p => p.Nome).ToList());

                // Na listagem de questinário pós devem ser exibidos todos os questionários. redmine #1947
                var listaQuestionariosPos = usuario.IsGestor()
                    ? manterQuestionario.ObterQuestionariosPorCategoriaGestor(null, categoriaConteudo.ID, usuario.UF)
                    : manterQuestionario.ObterTodosPorCategoria(categoriaConteudo.ID);

                listaQuestionariosPos = ObterQuestionariosAssociados(Turma, listaQuestionariosPos.ToList(), enumTipoQuestionarioAssociacao.Pos);

                ViewState["_SE_QuestionarioPos"] =
                    Helpers.Util.ObterListaAutocomplete(listaQuestionariosPos.OrderBy(p => p.Nome).ToList());

                var listaQuestionarioCancelamento = new ManterQuestionario().ObterQuestionariosCancelamento(categoriaConteudo).ToList();
                listaQuestionarioCancelamento = ObterQuestionariosAssociados(Turma, listaQuestionarioCancelamento.ToList(), enumTipoQuestionarioAssociacao.Cancelamento);

                // Preencher questionários Cancelamento.
                ViewState["_SE_QuestionarioCancelamento"] = Helpers.Util.ObterListaAutocomplete(listaQuestionarioCancelamento);

                var listaQuestionarioAbandono = new ManterQuestionario().ObterQuestionariosAbandono(categoriaConteudo).ToList();
                listaQuestionarioAbandono = ObterQuestionariosAssociados(Turma, listaQuestionarioAbandono, enumTipoQuestionarioAssociacao.Abandono);

                // Preencher questionários Abandono.
                ViewState["_SE_QuestionarioAbandono"] = Helpers.Util.ObterListaAutocomplete(listaQuestionarioAbandono);

                // Preencher questionários Abandono.
                ViewState["_SE_QuestionarioEficacia"] = Helpers.Util.ObterListaAutocomplete(listaQuestionariosDePesquisa.OrderBy(p => p.Nome).ToList());

                if (listaQuestionariosDePesquisa.Any())
                {
                    txtQuestionarioPre.Enabled = true;
                    txtQuestionarioPos.Enabled = true;
                }
                else
                {
                    PreencherQuestionarioListaVazia();
                }
            }
            catch (Exception)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro na busca de questionários.");

                PreencherQuestionarioListaVazia();
            }
        }

        private List<Questionario> ObterQuestionariosAssociados(Turma turma, List<Questionario> questionarios, enumTipoQuestionarioAssociacao? tipo = null)
        {
            // Caso tenha algum questionário vinculado que não esteja na categoria adiciona este questionário na listagem
            if (turma != null && turma.ListaQuestionarioAssociacao.Any())
            {
                var questionariosAssociados = turma.ListaQuestionarioAssociacao.Where(x => !questionarios.Contains(x.Questionario) && tipo != null && x.Questionario.TipoQuestionario.ID == (int)tipo).Select(x => x.Questionario);
                questionarios.AddRange(questionariosAssociados);
            }

            return questionarios;
        }

        public List<TurmaProfessor> ObterObjetoTurmaProfessor()
        {
            var turma = ObterTurma();

            var lstTurmaProfessor = new List<TurmaProfessor>();
            var professoresIds = this.ListBoxesProfessor.RecuperarIdsSelecionados<int>();

            if (!professoresIds.Any())
                return lstTurmaProfessor;

            var bmUsuario = new BMUsuario();

            lstTurmaProfessor.AddRange(professoresIds.Select(id => new TurmaProfessor()
            {
                Turma = turma,
                Professor = bmUsuario.ObterPorId(id),
            }));

            return lstTurmaProfessor;
        }

        public void EsconderBotaoSalvar()
        {
            divBtnSalvar.Visible = false;
        }

        public void EsconderTitulo()
        {
            divAcao.Visible = false;
        }

        public void ExibirTitulo()
        {
            divAcao.Visible = true;
        }

        public void RemoveNotasMatriculasTurma()
        {
            var turma = ObterObjetoTurma();
            if (turma.ID > 0 && !turma.InAvaliacaoAprendizagem)
            {
                if (turma.ListaMatriculas.Any())
                {
                    foreach (var matriculaTurma in turma.ListaMatriculas)
                    {
                        matriculaTurma.Nota1 = null;
                        matriculaTurma.Nota2 = null;
                        matriculaTurma.MediaFinal = null;
                    }

                    new ManterTurma().AlterarTurma(turma, null, null, null);
                }
            }
        }

        public void SalvarTurma(bool verificaAvaliacaoAprendizagem = true)
        {
            PrepararQuestionarios();
            var turmaOriginal = ObterTurma();

            var statusAnterior = turmaOriginal != null ? turmaOriginal.Status : null;

            var turma = ObterObjetoTurma();

            if (verificaAvaliacaoAprendizagem)
            {
                if (turma.ID > 0 && !turma.InAvaliacaoAprendizagem && turma.ListaMatriculas.Any())
                {
                    ExibirModalConfirmacaoExclusaoNotasMatriculasTurma(turma);
                    return;
                }
            }
            
            var manterTurma = new ManterTurma();
            var manterOferta = new ManterOferta();

            // Incluir ou alterar de acordo com o ID da Request informado e validado.

            if (Request["Id"] == null)
                manterTurma.IncluirTurma(turma);
            else
            {
                LogResponsavel logResponsavel = null;

                LogConsultorEducacional logConsultorEducacional = null;

                var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

                if (turma.PermitirLogResponsavel)
                {
                    logResponsavel = new LogResponsavel
                    {
                        Responsavel = turma.LogResponsavel,
                        UsuarioAlteracao = usuarioLogado,
                        Turma = new Turma { ID = turma.ID },
                        Data = DateTime.Now
                    };
                }

                if (turma.PermitirLogConsultor)
                {
                    logConsultorEducacional = new LogConsultorEducacional
                    {
                        ConsultorEducacional = turma.LogConsultorEducacional,
                        UsuarioAlteracao = usuarioLogado,
                        Turma = new Turma { ID = turma.ID },
                        Data = DateTime.Now
                    };
                }
                
                manterTurma.AlterarTurma(turma, logResponsavel, logConsultorEducacional, statusAnterior);
            }

            var ofertaEdicao = manterOferta.ObterOfertaPorID(turma.Oferta.ID);

            #region Sincronização com Moodle

            if (ofertaEdicao.SolucaoEducacional.Fornecedor.ID == (int)enumFornecedor.MoodleSebrae &&
                ofertaEdicao.CodigoMoodle.HasValue)
            {
                // Sincronizar oferta e SE.
                manterOferta.SincronizarOfertaComMoodle(ofertaEdicao, ofertaEdicao.SolucaoEducacional);

                var action = Request["Id"] == null ? "cadastrada" : "alterada";
                // Sincronizar Turma.
                try
                {
                    // Executar sincronização.
                    var stringSinc = string.Format(
                                ConfigurationManager.AppSettings["robo_sync"] +
                                "&id_chave_externa_oferta={1}&nome_turma={2}", "Sgus_Moodle_Sync_Turma",
                                ofertaEdicao.IDChaveExterna, turma.Nome);

                    var jsonStringResult = ObterJSON(stringSinc);

                    var serializer = new JavaScriptSerializer();

                    var ofertaJsonResult = serializer.Deserialize<JSONResultReturn>(jsonStringResult);

                    if (ofertaJsonResult.Sucesso == false)
                    {
                        throw new AcademicoException(ofertaJsonResult.Mensagem);
                    }

                    // Salvar ID da chave externa do Moodle (id da tabela mdl_groups)
                    if (ofertaJsonResult.IdGrupo != null)
                    {
                        turma.IDChaveExterna = ofertaJsonResult.IdGrupo.ToString();
                        manterTurma.AlterarTurma(turma);

                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, MessagemErroSicroniaMoodle(action, ofertaJsonResult.Mensagem));
                    }
                }
                catch (Exception ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, MessagemErroSicroniaMoodle(action, ex.Message));
                }
            }

            #endregion
                
            WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados cadastrados com Sucesso!", "../Turma/ListarTurma.aspx");
        }
        private string MessagemErroSicroniaMoodle (string action, string msgErro) {
            return string.Format(
                "Turma {0} com sucesso, porém houve um erro na sincronização com o Moodle. Verifique se a sincronização foi realizada ou tente novamente.{1}",
                action,
                msgErro
            );
        }

        public void RepublicarTurma()
        {
            var turma = ObterObjetoTurma(true);

            ValidarTurmaParaRepublicar(turma);

            turma.ID = 0;

            new ManterTurma().IncluirTurma(turma);
        }

        private void ValidarTurmaParaRepublicar(Turma turmaNova)
        {
            if (TurmaAntesModificacao != null)
                if (turmaNova.Oferta.ID == TurmaAntesModificacao.Oferta.ID &&
                   turmaNova.Nome == TurmaAntesModificacao.Nome)
                    throw new AcademicoException("Já existe uma turma cadastrada com a mesma oferta e nome.");
        }

        public void PreencherCampos(Turma turma = null, bool republicar = false)
        {
            // Caso a turma não tenha sido informada externamente, buscar a turma do Request.
            if (turma == null)
                turma = ObterTurma();

            // Preenchimento geral.
            WebFormHelper.PreencherComponenteComOpcoesSimNao(rblInAvaliacaoAprendizagem);
            WebFormHelper.PreencherComponenteComOpcoesSimNao(rblInAberta);
            WebFormHelper.PreencherComponenteComOpcoesSimNao(rblAcessoAposConclusao);

            // Preencher o combo de Professores.
            PreencherComboProfessor();

            // Preencher combo da SE.
            PreencherSolucaoEducacional(null, null);

            // Preencher combo de Ofertas.
            PreencherOfertas();

            if (turma != null)
            {
                RegrasEdicao();

                var oferta = turma.Oferta;
                var solucaoEducacional = oferta.SolucaoEducacional;
                var categoriaConteudo = solucaoEducacional.CategoriaConteudo;

                // Exibição de Vagas Utilizadas e Vagas Disponíveis.
                AtualizarVagasOferta(oferta);

                // ID da Turma.
                txtIdTurma.Text = turma.ID.ToString();

                // Preencher campos normais
                txtNomeTurma.Text = turma.Nome;

                // Se for Republicação, não insere os campos abaixo
                if (!republicar)
                {
                    txtCodigo.Text = turma.Codigo;
                    TxtChaveExterna.Text = turma.IDChaveExterna;
                }

                // Local
                txtLocal.Text = turma.Local;

                // Data início.
                TxtDtInicio.Text = turma.DataInicio.ToString("dd/MM/yyyy HH:mm:ss");

                // Data fim.
                if (turma.DataFinal != null) TxtDtFinal.Text = turma.DataFinal.Value.ToString("dd/MM/yyyy HH:mm:ss");

                // Questionários.
                PreencherQuestionarios(categoriaConteudo, turma);
                var questionario = turma.ListaQuestionarioAssociacao.FirstOrDefault(p => p.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pre);
                if (questionario != null) txtQuestionarioPre.Text = questionario.Questionario.ID.ToString();

                questionario = turma.ListaQuestionarioAssociacao.FirstOrDefault(p => p.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pos);
                if (questionario != null)
                {
                    txtQuestionarioPos.Text = questionario.Questionario.ID.ToString();
                    if (oferta.TipoOferta != enumTipoOferta.Continua)
                    {
                        if (questionario.DataDisparoLinkPesquisa.HasValue)
                        {
                            txtDataDisparoLinkPesquisa.Text = questionario.DataDisparoLinkPesquisa.Value.ToString("dd/MM/yyyy HH:mm:ss");
                        }
                        else
                        {
                            txtDataDisparoLinkPesquisa.Text = turma.DataFinal.Value.ToString("dd/MM/yyyy HH:mm:ss");
                        }
                    }
                }
                else
                {
                    txtDataDisparoLinkPesquisa.Text = turma.DataFinal.Value.ToString("dd/MM/yyyy HH:mm:ss");
                }

                questionario =
                    turma.ListaQuestionarioAssociacao.FirstOrDefault(p => p.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Cancelamento);
                if (questionario != null) txtQuestionarioCancelamento.Text = questionario.Questionario.ID.ToString();

                questionario =
                    turma.ListaQuestionarioAssociacao.FirstOrDefault(p => p.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Abandono);
                if (questionario != null) txtQuestionarioAbandono.Text = questionario.Questionario.ID.ToString();

                questionario =
                  turma.ListaQuestionarioAssociacao.FirstOrDefault(p => p.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Eficacia);
                if (questionario != null)
                {
                    txtQuestionarioEficacia.Text = questionario.Questionario.ID.ToString();
                    PreencherDataQuestEficacia(questionario);
                    divQuestEficacia.Visible = true;
                }

                rblInAvaliacaoAprendizagem.SelectedValue = turma.InAvaliacaoAprendizagem ? Constantes.SiglaSim : Constantes.SiglaNao;
                divNotaMinima.Visible = turma.InAvaliacaoAprendizagem;
                

                // Aberto para novas Inscrições
                rblInAberta.SelectedValue = turma.InAberta ? Constantes.SiglaSim : Constantes.SiglaNao;

                // Tipo Tutoria.
                ddlTipoTutoria.ClearSelection();
                WebFormHelper.SetarValorNaCombo(turma.TipoTutoria, ddlTipoTutoria);
                ddlTipoTutoria_SelectedIndexChanged(null, null); // Dá trigger no evento para exibir/esconder controles que dependem da seleção do Tipo de Tutoria.

                // Professores.
                ListBoxesProfessor.MarcarComoSelecionados(turma.Professores.Select(x => x.ID));

                // Acesso após término.
                rblAcessoAposConclusao.SelectedValue = turma.AcessoAposConclusao ? Constantes.SiglaSim : Constantes.SiglaNao;

                // Quantidade máxima de inscrições.
                txtQTMaxInscricoes.Text = turma.QuantidadeMaximaInscricoes.ToString();

                // Responsável.
                if (turma.Responsavel != null)
                    ucLupaUsuario.SelectedUser = turma.Responsavel;

                // Consultor.
                if (turma.ConsultorEducacional != null)
                    txtConsultorEducacional.Text = turma.ConsultorEducacional.ID.ToString();

                // Preencher Acesso a Wi-Fi e Local
                if (turma.AcessoWifi.HasValue && turma.AcessoWifi.Value > 0)
                {
                    rblSelecionaAcessoWifi.SelectedValue = "1";
                    rblAcessoWifi.SelectedValue = turma.AcessoWifi.Value.ToString();
                    divAcessoWifi.Visible = true;
                }
                else
                {
                    rblSelecionaAcessoWifi.SelectedValue = "0";
                    divAcessoWifi.Visible = false;
                }

                PreencherStatus(turma);
            }
            else
            {
                PreencherStatus();
                RegrasAdicao();

                // Preencher Questionarios com Lista Vazia
                PreencherQuestionarioListaVazia();

                rblSelecionaAcessoWifi.SelectedValue = "0";
                rblAcessoAposConclusao.SelectedIndex = 1;
            }
        }

        protected void rblInAvaliacaoAprendizagem_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var rblInAvaliacaoAprendizagem = (RadioButtonList)sender;
            if(rblInAvaliacaoAprendizagem.SelectedValue == "S")
            {
                divNotaMinima.Visible = true;
                txtValorNotaMinima.Text = "7";
            }
            else
            {
                divNotaMinima.Visible = false;
                txtValorNotaMinima.Text = null;
            }
        }

        private void PreencherComboProfessor()
        {
            var usuarios = new ManterUsuario();
            var listaProfessor = usuarios.ObterTodosPorPerfil(enumPerfil.Professor);
            ListBoxesProfessor.PreencherItens(listaProfessor, "ID", "Nome");
        }

        private List<Usuario> ObterProfessoresSelecionados(Turma turma)
        {
            var listaRetorno = new List<Usuario>();

            var manterUsuario = new ManterUsuario();
            // Adiciona os professores marcados.
            foreach (var id in ListBoxesProfessor.RecuperarIdsSelecionados())
            {
                listaRetorno.Add(manterUsuario.ObterUsuarioPorID(int.Parse(id)));
            }

            return listaRetorno;
        }

        private void PreencherQuestionarioListaVazia()
        {
            ViewState["_SE_QuestionarioPre"] = "''";
            ViewState["_SE_QuestionarioPos"] = "''";

            txtQuestionarioPre.Enabled = false;
            txtQuestionarioPos.Enabled = false;
        }

        private List<QuestionarioAssociacao> ObterQuestionariosAssociacao(Turma turma)
        {
            var listaRetorno = turma.ListaQuestionarioAssociacao ?? new List<QuestionarioAssociacao>();
            int id;

            // Obter Questionário Pré.
            int.TryParse(txtQuestionarioPre.Text, out id);
            listaRetorno = AtualizarQuestionarioAssociacao(listaRetorno.ToList(), turma, id, enumTipoQuestionarioAssociacao.Pre);

            // Obter Questionário Pós.
            int.TryParse(txtQuestionarioPos.Text, out id);
            listaRetorno = AtualizarQuestionarioAssociacao(listaRetorno.ToList(), turma, id, enumTipoQuestionarioAssociacao.Pos);

            // Obter Questionário Cancelamento.
            int.TryParse(txtQuestionarioCancelamento.Text, out id);
            listaRetorno = AtualizarQuestionarioAssociacao(listaRetorno.ToList(), turma, id, enumTipoQuestionarioAssociacao.Cancelamento);

            // Obter Questionário Reprovação.
            int.TryParse(txtQuestionarioAbandono.Text, out id);
            listaRetorno = AtualizarQuestionarioAssociacao(listaRetorno.ToList(), turma, id, enumTipoQuestionarioAssociacao.Abandono);

            // Obter Questionário Eficácia
            if (!string.IsNullOrWhiteSpace(txtQuestionarioEficacia.Text))
            {
                int.TryParse(txtQuestionarioEficacia.Text, out id);
                listaRetorno = AtualizarQuestionarioAssociacao(listaRetorno.ToList(), turma, id, enumTipoQuestionarioAssociacao.Eficacia);
                listaRetorno.FirstOrDefault(x => x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Eficacia).DataDisparoLinkEficacia = CommonHelper.TratarData(txtDataDisparoLinkEficacia.Text.Trim(), "Data de disparo do Questionário de Eficácia");
            }

            return listaRetorno.ToList();
        }

        private IList<QuestionarioAssociacao> AtualizarQuestionarioAssociacao(IList<QuestionarioAssociacao> listaRetorno, Turma turma, int idQuestionario, enumTipoQuestionarioAssociacao tipo)
        {
            var questionarioAssociacao = listaRetorno.FirstOrDefault(p => p.TipoQuestionarioAssociacao.ID == (int)tipo);
            if (idQuestionario == 0)
            {
                if (questionarioAssociacao == null) return listaRetorno;
                listaRetorno.Remove(questionarioAssociacao);
                return listaRetorno;
            }
            var manterQuestionario = new ManterQuestionario();
            var questionario = manterQuestionario.ObterQuestionarioPorID(idQuestionario);
            if (questionarioAssociacao == null)
            {
                listaRetorno.Add(ObterQuestionarioAssociacao(turma, tipo, questionario));
            }
            else
            {
                var index = listaRetorno.IndexOf(questionarioAssociacao);
                listaRetorno[index].Questionario = questionario;
                listaRetorno[index].TipoQuestionarioAssociacao = new ManterTipoQuestionarioAssociacao().ObterTipoQuestionarioAssociacaoPorID((int)tipo);
            }
            return listaRetorno;
        }

        private QuestionarioAssociacao ObterQuestionarioAssociacao(Turma turma, enumTipoQuestionarioAssociacao tipo, Questionario questionario)
        {
            var questionarioPreAssociacao = new QuestionarioAssociacao
            {
                TipoQuestionarioAssociacao = new ManterTipoQuestionarioAssociacao().ObterTipoQuestionarioAssociacaoPorID((int)tipo),
                Questionario = questionario,
                Evolutivo = false,
                Turma = turma,
                Obrigatorio = true
            };
            return questionarioPreAssociacao;
        }

        private void PrepararQuestionarios()
        {
            var manterTurma = new ManterTurma();
            Turma turma = null;

            if (txtIdTurma.Text != "" && txtIdTurma.Text != "0")
            {
                turma = manterTurma.ObterTurmaPorID(int.Parse(txtIdTurma.Text));
            }
            else if (Request["id"] != null)
            {
                turma = manterTurma.ObterTurmaPorID(int.Parse(Request["id"]));
            }

            if (turma == null) return;

            var lsIds = new List<int>();
            int id;

            int.TryParse(txtQuestionarioPre.Text, out id);
            lsIds = ListaRemoverAssociacaoQuestionario(turma, lsIds, id, enumTipoQuestionarioAssociacao.Pre).ToList();

            //questionario pos
            int.TryParse(txtQuestionarioPos.Text, out id);
            lsIds = ListaRemoverAssociacaoQuestionario(turma, lsIds, id, enumTipoQuestionarioAssociacao.Pos).ToList();

            if (turma.ListaQuestionarioAssociacao.Any(x => x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pos))
            {
                turma.ListaQuestionarioAssociacao.Where(x => x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pos).First().DataDisparoLinkPesquisa = CommonHelper.TratarData(txtDataDisparoLinkPesquisa.Text.Trim(), "Data de disparo");
            }

            //questionario reprovação
            int.TryParse(txtQuestionarioAbandono.Text, out id);
            lsIds = ListaRemoverAssociacaoQuestionario(turma, lsIds, id, enumTipoQuestionarioAssociacao.Abandono).ToList();

            //questionario Cancelamento
            int.TryParse(txtQuestionarioCancelamento.Text, out id);
            lsIds = ListaRemoverAssociacaoQuestionario(turma, lsIds, id, enumTipoQuestionarioAssociacao.Cancelamento).ToList();

            //questionario Eficácia
            int.TryParse(txtQuestionarioEficacia.Text, out id);
            lsIds = ListaRemoverAssociacaoQuestionario(turma, lsIds, id, enumTipoQuestionarioAssociacao.Eficacia).ToList();

            if (turma.ListaQuestionarioAssociacao.Any(x => x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Eficacia))
            {
                turma.ListaQuestionarioAssociacao.Where(x => x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Eficacia).First().DataDisparoLinkEficacia = CommonHelper.TratarData(txtDataDisparoLinkEficacia.Text.Trim(), "Data de disparo do Questionário de Eficácia");
            }

            var bmQuestionarioAssociacao = new BMQuestionarioAssociacao();
            foreach (var i in lsIds)
            {
                (new BMQuestionarioAssociacao()).Excluir(new QuestionarioAssociacao { ID = i });
            }
        }

        static IEnumerable<int> ListaRemoverAssociacaoQuestionario(Turma turma, IList<int> lsIds, int id, enumTipoQuestionarioAssociacao tipo)
        {
            if (id != 0) return lsIds;
            if (turma.ListaQuestionarioAssociacao == null) return lsIds;
            var questionarioAssociacao = turma.ListaQuestionarioAssociacao.FirstOrDefault(p => p.TipoQuestionarioAssociacao.ID == (int)tipo);
            if (questionarioAssociacao != null) lsIds.Add(questionarioAssociacao.ID);
            return lsIds;
        }

        internal void LimparCampos()
        {
            txtNomeTurma.Text = "";
            TxtChaveExterna.Text = "";
            txtLocal.Text = "";
            TxtDtInicio.Text = "";
            TxtDtFinal.Text = "";
            txtValorNotaMinima.Text = "7";
            ddlTipoTutoria.SelectedIndex = 0;
            txtQTMaxInscricoes.Text = "";
            lblTxtMaxInscricoesHelp.InnerText = "Escolha uma oferta para exibir a quantidade de vagas restantes.";
            ListBoxesProfessor.LimparSelecao();
        }

        #endregion

        #region Eventos personalizados

        public delegate void CadastroDeTurmaRealizado(object sender, CadastrarTurmaEventArgs e);
        public event CadastroDeTurmaRealizado CadastrouTurma;

        #endregion

        protected void txtConsultorEducacional_OnTextChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtém o Consultor Educacional selecionado no DropDownList da tela.
        /// </summary>
        /// <returns></returns>
        private Usuario ObterConsultorEducacionalDropDown()
        {
            var id = !string.IsNullOrWhiteSpace(txtConsultorEducacional.Text) ? int.Parse(txtConsultorEducacional.Text) : 0;

            return id == 0 ? null : new ManterUsuario().ObterUsuarioPorID(id);
        }

        public void PreencherStatus(Turma turma = null)
        {
            ddlStatus.Items.Clear();

            var procederExibicaoStatus = true;

            var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

            // Habilita a funcionalidade apenas para gestores e administradores.
            if (!usuarioLogado.IsAdministrador() && !usuarioLogado.IsGestor())
                procederExibicaoStatus = false;
            else
            {
                var se = ObterSolucaoEducacionalDropDown();

                if (se != null)
                    procederExibicaoStatus = se.CategoriaConteudo.PossuiGerenciamentoStatus();
            }

            if (procederExibicaoStatus)
            {
                divStatus.Visible = true;
                LblDtFinal.Text = "Data Final *";

                var prevista = new ListItem
                {
                    Value = ((int)enumStatusTurma.Prevista).ToString(),
                    Text = enumStatusTurma.Prevista.GetDescription()
                };
                var confirmada = new ListItem
                {
                    Value = ((int)enumStatusTurma.Confirmada).ToString(),
                    Text = enumStatusTurma.Confirmada.GetDescription()
                };
                var cancelada = new ListItem
                {
                    Value = ((int)enumStatusTurma.Cancelada).ToString(),
                    Text = enumStatusTurma.Cancelada.GetDescription()
                };
                var emAndamento = new ListItem
                {
                    Value = ((int)enumStatusTurma.EmAndamento).ToString(),
                    Text = enumStatusTurma.EmAndamento.GetDescription()
                };
                var realizada = new ListItem
                {
                    Value = ((int)enumStatusTurma.Realizada).ToString(),
                    Text = enumStatusTurma.Realizada.GetDescription()
                };

                // Se for cadastro, busca os Status iniciais.
                if (Request["ID"] == null)
                {
                    ddlStatus.Items.Add(prevista);
                    return;
                }

                turma = turma ?? new ManterTurma().ObterTurmaPorID(int.Parse(Request["Id"]));

                var itensExibir = new ListItem[] { };
                var exibirSelecione = true;

                switch (turma.Status)
                {
                    case null:
                        exibirSelecione = false;
                        itensExibir = new[] { prevista, confirmada, cancelada };
                        break;
                    case enumStatusTurma.Prevista:
                        itensExibir = new[] { prevista, confirmada, cancelada };
                        break;
                    case enumStatusTurma.Confirmada:
                        itensExibir = new[] { confirmada, cancelada };
                        break;
                    case enumStatusTurma.Cancelada:
                        itensExibir = new[] { cancelada, prevista, confirmada };

                        // Preencher Justificativa.
                        divJustificativa.Visible = true;
                        txtJustificativa.Text = turma.ObterJustificativa();

                        break;
                    case enumStatusTurma.EmAndamento:
                        itensExibir = new[] { emAndamento, cancelada };
                        break;
                    case enumStatusTurma.Realizada:
                        itensExibir = new[] { realizada };
                        break;
                }

                PopularDropdownStatus(itensExibir, exibirSelecione, turma.Status);
            }
            else
            {
                divStatus.Visible = false;
                LblDtFinal.Text = "Data Final";
            }
        }

        private void PopularDropdownStatus(ListItem[] itens, bool exibirSelecione, enumStatusTurma? statusSelecionado = null)
        {
            // Caso seja o Status selecionado, concatenar com a palavra "Selecionado";
            if (exibirSelecione && statusSelecionado != null)
                foreach (var item in itens.Where(item => int.Parse(item.Value) == (int)statusSelecionado))
                    item.Text = item.Text + " (atual)";

            ddlStatus.Items.AddRange(itens);
        }

        protected void ddlStatus_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var drop = (DropDownList)sender;

            // Exibir justificativa caso a turma esteja sendo cancelada
            divJustificativa.Visible = int.Parse(drop.SelectedValue) == (int)enumStatusTurma.Cancelada;
        }

        protected void txtQuestionarioEficacia_TextChanged(object sender, EventArgs e)
        {
            PreencherDataQuestEficacia(null);
            divQuestEficacia.Visible = true;
        }

        private void PreencherDataQuestEficacia(QuestionarioAssociacao questionario)
        {
            if (questionario != null)
            {
                txtDataDisparoLinkEficacia.Text = questionario.DataDisparoLinkEficacia.ToString();
                return;
            }

            txtDataDisparoLinkEficacia.Text = !string.IsNullOrEmpty(TxtDtFinal.Text) ? DateTime.Parse(TxtDtFinal.Text).AddDays(90).ToString() : "";
        }
    }

    public class CadastrarTurmaEventArgs : EventArgs
    {
        public Turma InformacoesDaTurmaCadastrada { get; set; }

        public CadastrarTurmaEventArgs(Turma turma)
        {
            InformacoesDaTurmaCadastrada = turma;
        }
    }
}