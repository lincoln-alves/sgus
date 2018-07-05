using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.Services.SgusWebService;
using System.Web.UI.WebControls;
using System.Data;
using Sebrae.Academico.Extensions;

namespace Sebrae.Academico.WebForms.UserControls
{
    /// <summary>
    /// User control utilizado na matricula de um aluno em uma oferta.
    /// </summary>
    public partial class ucMatriculaOferta : System.Web.UI.UserControl
    {
        #region "Eventos referentes ao Cancelamento"

        public delegate void MatriculaDeUmAlunoEmUmaOfertaCancelada(object sender, MatricularAlunoEmUmaOfertaEventArgs e
            );

        public event MatriculaDeUmAlunoEmUmaOfertaCancelada CancelouMatriculaDeUmAlunoEmUmaOferta;

        #endregion

        private bool IsAdminOrGestorUc { get; set; }

        #region "Eventos referentes à matricula de um aluno em uma oferta"

        public delegate void MatriculaDeUmAlunoEmUmaOfertaRealizada(object sender, MatricularAlunoEmUmaOfertaEventArgs e
            );

        public event MatriculaDeUmAlunoEmUmaOfertaRealizada MatriculouAlunoEmUmaOferta;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            var manterUsuario = new ManterUsuario();
            var usuarioLogado = manterUsuario.ObterUsuarioLogado();
            IsAdminOrGestorUc = usuarioLogado.IsAdministrador() || usuarioLogado.IsGestor();

            if (IsPostBack) return;

            WebFormHelper.PrevinirCliqueDuplo(new List<Button>() { btnEnviar }, this.Page);

            WebFormHelper.PreencherLista(
                new List<StatusMatricula> { new StatusMatricula { ID = 0, Nome = "- Selecione uma Turma -" } },
                this.ddlStatus, false, false);
        }

        #region "Atributos Privados"

        public int IdOferta
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
            set { ViewState["ViewStateIdOferta"] = value; }
        }

        #endregion

        public void LimparCampos()
        {
            this.LupaUsuario.LimparCampos();
            this.ddlStatus.ClearSelection();
        }

        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            //this.SetarAcaoDaTela(enumAcaoTelaMatriculaOferta.NovaMatricula);
            var manterUsuario = new ManterUsuario();
            var usuarioLogado = manterUsuario.ObterUsuarioLogado();

            try
            {
                if (string.IsNullOrEmpty(ddlTurma.SelectedValue) || int.Parse(ddlTurma.SelectedValue) <= 0)
                {
                    throw new AcademicoException("Selecione uma turma para alterar a inscrição");
                }

                if (string.IsNullOrEmpty(ddlStatus.SelectedValue) || int.Parse(ddlStatus.SelectedValue) <= 0)
                {
                    throw new AcademicoException("Selecione um status");
                }

                var dtDataMatricula = DateTime.Now;
                var dtDataConclusao = DateTime.Now;

                if (IsAdminOrGestorUc && !string.IsNullOrEmpty(txtDataInscricao.Text))
                {
                    try
                    {
                        dtDataMatricula = Convert.ToDateTime(txtDataInscricao.Text);
                    }
                    catch
                    {
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Data de Matrícula inválida.");
                        return;
                    }
                    try
                    {
                        dtDataConclusao = Convert.ToDateTime(txtDataConclusao.Text);
                    }
                    catch
                    {
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Data de Conclusão inválida.");
                        return;
                    }
                }

                var manterTurma = new ManterTurma();

                if (!usuarioLogado.IsAdministrador())
                {
                    var matriculaOferta = this.ObterObjetoMatriculaOferta();
                    var manterMatriculaOferta = new ManterMatriculaOferta();

                    var idOferta = matriculaOferta.Oferta.ID;
                    var cpf = matriculaOferta.Usuario.CPF;
                    var idStatusMatricula = ddlStatus.SelectedIndex > 0 ? int.Parse(ddlStatus.SelectedValue) : 0;
                    var idTurma = ddlTurma.SelectedIndex > 0 ? int.Parse(ddlTurma.SelectedValue) : 0;

                    new ManterMatriculaOfertaService().MatriculaSolucaoEducacionalGestorUC(idOferta, cpf,
                        idStatusMatricula, idTurma, usuarioLogado.CPF);

                    //Dispara o evento informando que a matricula em uma oferta foi realizada
                    if (MatriculouAlunoEmUmaOferta == null) return;
                    var mo = manterMatriculaOferta.ObterPorUsuarioESolucaoEducacional(matriculaOferta.Usuario.ID,
                        matriculaOferta.Oferta.SolucaoEducacional.ID);

                    if (mo != null && mo.ToList().Any())
                    {
                        matriculaOferta = mo.FirstOrDefault();
                        if (matriculaOferta == null) return;
                        //Obtem as informações da matricula Oferta (inclusive a lista de turmas da oferta)
                        matriculaOferta = manterMatriculaOferta.ObterInformacoesDaMatricula(matriculaOferta.ID);
                    }

                    AdicionaOfertEmMatriculaTurma(matriculaOferta);

                    MatriculouAlunoEmUmaOferta(this, new MatricularAlunoEmUmaOfertaEventArgs(matriculaOferta));
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "myscript",
                        " $('#EdicaoNivel, #modal-background').removeClass('active');", true);

                    LimparCamposMatriculaAluno();

                    //Cadastrar Solucao Educacional com o SAS
                    var manterSEService = new ManterSolucaoEducacionalService();

                    if (matriculaOferta.Oferta.SolucaoEducacional.IntegracaoComSAS)
                        manterSEService.CadastrarSAS(matriculaOferta.Usuario, new BP.integracaoSAS.TreinamentoSasClient());

                    //Matricular Aluno no Moodle
                    manterSEService.MatricularAlunoMoodle(matriculaOferta.Usuario, matriculaOferta.Oferta);

                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Aluno cadastrado com sucesso.");
                }
                else
                {
                    if (!string.IsNullOrEmpty(txtDataInscricao.Text))
                    {
                        var idTurma = ddlTurma.SelectedIndex > 0 ? int.Parse(ddlTurma.SelectedValue) : 0;
                        var turma = manterTurma.ObterTurmaPorID(idTurma);

                        if (turma == null)
                        {
                            WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Turma não encontrada.");
                            return;
                        }
                        var dataValida = turma.Oferta.DataInicioInscricoes.HasValue &&
                                         turma.Oferta.DataFimInscricoes.HasValue &&
                                         dtDataMatricula.Date.Between(turma.Oferta.DataInicioInscricoes.Value.Date,
                                             turma.Oferta.DataFimInscricoes.Value.Date);

                        if (!dataValida)
                        {
                            WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro,
                                turma.Oferta.DataInicioInscricoes.HasValue && turma.Oferta.DataFimInscricoes.HasValue
                                    ? "Data de Matrícula deve ser entre " +
                                      turma.Oferta.DataInicioInscricoes.Value.Date.ToString("dd/MM/yyyy") + " e " +
                                      turma.Oferta.DataFimInscricoes.Value.Date.ToString("dd/MM/yyyy")
                                    : "Data de Matrícula inválida.");
                            return;
                        }

                        var dataValidaConclusao = dtDataConclusao.Date >= turma.DataInicio;

                        if (!dataValidaConclusao)
                        {
                            WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro,
                                "Data de Conclusão deve ser maior ou igual à: " + turma.DataInicio.ToString("dd/MM/yyyy"));
                            return;
                        }
                    }

                    var matriculaOferta = this.ObterObjetoMatriculaOferta();
                    var manterMatriculaOferta = new ManterMatriculaOferta();
                    if (matriculaOferta.Oferta != null)
                    {
                        if (
                            matriculaOferta.Oferta.ListaMatriculaOferta.Any(
                                x =>
                                    x.Usuario.ID == matriculaOferta.Usuario.ID &&
                                    (x.StatusMatricula == enumStatusMatricula.Inscrito ||
                                     x.StatusMatricula == enumStatusMatricula.PendenteConfirmacaoAluno)))
                            throw new AcademicoException("Usuário já inscrito nesta oferta");
                    }

                    // Verificar permissao de matricula de acordo com as regras de permissão da Oferta.
                    if (matriculaOferta.Oferta != null &&
                        !matriculaOferta.Oferta.UsuarioPossuiPermissao(matriculaOferta.Usuario))
                    {
                        throw new AcademicoException("Usuário não possui permissão para essa oferta");
                    }

                    if (!divDataInscricao.Visible)
                        (new ManterMatriculaOferta()).VerificarPoliticaDeConsequencia(matriculaOferta);

                    SalvarMatriculaOferta(manterMatriculaOferta, matriculaOferta);

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "myscript",
                        " $('#EdicaoNivel, #modal-background').removeClass('active');", true);

                    LimparCamposMatriculaAluno();

                    //Cadastrar Solucao Educacional com o SAS
                    var manterSEService = new ManterSolucaoEducacionalService();

                    if (matriculaOferta.Oferta.SolucaoEducacional.IntegracaoComSAS)
                        manterSEService.CadastrarSAS(matriculaOferta.Usuario, new BP.integracaoSAS.TreinamentoSasClient());

                    //Matricular Aluno no Moodle
                    manterSEService.MatricularAlunoMoodle(matriculaOferta.Usuario, matriculaOferta.Oferta);

                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Aluno cadastrado com sucesso.");
                }
            }
            catch (AcademicoException ex)
            {
                // Lógica para alteração de status quando o usuário for administrador ou gestor uc
                if (LupaUsuario.SelectedUser != null && (usuarioLogado.IsAdministrador() || usuarioLogado.IsGestor()))
                {
                    try
                    {
                        var matriculaOferta = ObterObjetoMatriculaOferta();
                    

                        //Status
                        if (ddlStatus.SelectedItem != null && !string.IsNullOrWhiteSpace(ddlStatus.SelectedItem.Value))
                        {
                            
                            if (ExisteComStatusRepetido((enumStatusMatricula)int.Parse(ddlStatus.SelectedItem.Value), matriculaOferta))
                            {
                                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Não é possível inserir uma nova matrícula com o mesmo status");
                                return;
                            }
                        }

                        lblAvisoModalConfirmacaoInscricao.Text = ex.Message;
                        pnlModalConfirmacaoInscricao.Visible = true;
                        return;
                    }
                    catch
                    {
                    }
                }

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                return;
            }
            catch (PoliticaConsequenciaException ex)
            {
                if (!IsAdminOrGestorUc)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }

                lblAvisoModalConfirmacaoInscricao.Text = ex.Message;
                pnlModalConfirmacaoInscricao.Visible = true;
            }
        }

        public bool ExisteComStatusRepetido (enumStatusMatricula status, MatriculaOferta matriculaOferta)
        {
            return (matriculaOferta.Oferta.ListaMatriculaOferta.Any(x => x.StatusMatricula == status && x.Usuario.ID == matriculaOferta.Usuario.ID));
        }

        private void SalvarMatriculaOferta(ManterMatriculaOferta manterMatriculaOferta, MatriculaOferta matriculaOferta,
            bool verificarPoliticaDeConsequencia = true)
        {
            manterMatriculaOferta.IncluirMatriculaOferta(matriculaOferta, verificarPoliticaDeConsequencia);

            var matriculaTurma = AdicionaOfertEmMatriculaTurma(matriculaOferta);
            if (matriculaOferta.MatriculaTurma == null) matriculaOferta.MatriculaTurma = new List<MatriculaTurma>();
            matriculaOferta.MatriculaTurma.Add(matriculaTurma);

            //Dispara o evento informando que a matricula em uma oferta foi realizada
            if (MatriculouAlunoEmUmaOferta == null) return;
            //Obtem as informações da matricula Oferta (inclusive a lista de turmas da oferta)
            matriculaOferta = manterMatriculaOferta.ObterInformacoesDaMatricula(matriculaOferta.ID);
            MatriculouAlunoEmUmaOferta(this, new MatricularAlunoEmUmaOfertaEventArgs(matriculaOferta));
        }

        protected void btnEnviarConfirmacao_OnClick(object sender, EventArgs e)
        {
            try
            {
                var matriculaOferta = this.ObterObjetoMatriculaOferta();
                var manterMatriculaOferta = new ManterMatriculaOferta();

                SalvarMatriculaOferta(manterMatriculaOferta, matriculaOferta, false);

                txtJustificativa.Text = "";
                lblAvisoModalConfirmacaoInscricao.Text = "";
                pnlModalConfirmacaoInscricao.Visible = false;

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Aluno cadastrado com sucesso.");
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                return;
            }
            catch (Exception ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                return;
            }
        }

        protected void OcultarModalAvisoConfirmacao_Click(object sender, EventArgs e)
        {
            txtJustificativa.Text = "";
            lblAvisoModalConfirmacaoInscricao.Text = "";
            pnlModalConfirmacaoInscricao.Visible = false;
        }

        protected void OcultarModalMatriculaAdminGestorConfirmacao_Click(object sender, EventArgs e)
        {
            txtJustificativa.Text = "";
            pnlMatriculaAdminGestor.Visible = false;
        }

        protected void btnCancelarConfirmacao_OnClick(object sender, EventArgs e)
        {
            txtJustificativa.Text = "";
            lblAvisoModalConfirmacaoInscricao.Text = "";
            pnlModalConfirmacaoInscricao.Visible = false;
        }

        private MatriculaTurma AdicionaOfertEmMatriculaTurma(MatriculaOferta matriculaOferta)
        {
            if (ddlTurma.SelectedIndex <= 0) return null;
            var matriculaTurma = ObterObjetoMatriculaTurma(matriculaOferta.ID);
            if (matriculaTurma == null) return null;

            int statusSelecionado = 0;
            int.TryParse(ddlStatus.SelectedValue.ToString(), out statusSelecionado);

            if (statusSelecionado == (int)enumStatusMatricula.Inscrito || statusSelecionado == (int)enumStatusMatricula.Ouvinte || statusSelecionado == (int)enumStatusMatricula.PendenteConfirmacaoAluno)
            {
                matriculaTurma.DataTermino = null;
            }

            (new BMMatriculaTurma()).Salvar(matriculaTurma);

            return matriculaTurma;
        }

        private void LimparCamposMatriculaAluno()
        {
            LupaUsuario.LimparCampos();
            txtDataInscricao.Text = "";
            txtDataConclusao.Text = "";
            ddlTurma.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;
        }

        private MatriculaOferta ObterObjetoMatriculaOferta()
        {
            ValidarMatriculaOferta();

            var matriculaOferta = new MatriculaOferta();

            //Usuário
            if (LupaUsuario.SelectedUser != null)
            {
                matriculaOferta.Usuario = this.LupaUsuario.SelectedUser;
            }

            //Oferta
            if (IdOferta > 0)
            {
                matriculaOferta.Oferta = new ManterOferta().ObterOfertaPorID(IdOferta);
            }

            //UF
            matriculaOferta.UF = matriculaOferta.Usuario.UF;

            //Nivel Ocupacional
            matriculaOferta.NivelOcupacional = matriculaOferta.Usuario.NivelOcupacional;

            //Status
            if (ddlStatus.SelectedItem != null && !string.IsNullOrWhiteSpace(ddlStatus.SelectedItem.Value))
            {
                matriculaOferta.StatusMatricula = (enumStatusMatricula)int.Parse(ddlStatus.SelectedItem.Value);
            }

            return matriculaOferta;
        }

        private void ValidarMatriculaOferta()
        {
            if (LupaUsuario.SelectedUser == null)
                throw new AcademicoException("Usuário é obrigatório.");

            if (ddlStatus.SelectedIndex == 0)
                throw new AcademicoException("Status é obrigatório.");

            if (ddlTurma.SelectedIndex == 0)
                throw new AcademicoException("Turma é obrigatória.");
        }

        protected void btnCancelarMatricula_Click(object sender, EventArgs e)
        {
            this.TratarCancelamento();
        }

        private void TratarCancelamento()
        {
            if (CancelouMatriculaDeUmAlunoEmUmaOferta == null) return;
            CancelouMatriculaDeUmAlunoEmUmaOferta(this, null);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "myscript",
                " $('#EdicaoNivel, #modal-background').removeClass('active');", true);
        }


        public void PreencherCamposDaMatriculaOferta(MatriculaOferta pMatriculaOferta)
        {
            if (pMatriculaOferta != null)
            {
                this.PrepararTelaParaEdicaoDeUmaMatriculaOferta(pMatriculaOferta);
            }
            else
            {
                this.PrepararTelaParaInclusaoDeUmaMatricula();
            }
        }

        private void PrepararTelaParaInclusaoDeUmaMatricula()
        {
            this.ZerarIds();
            this.SetarAcaoDaTela(enumAcaoTelaMatriculaOferta.NovaMatricula);
        }


        private void ZerarIds()
        {
            //TODO -> Customizar
        }

        private void PrepararTelaParaEdicaoDeUmaMatriculaOferta(MatriculaOferta pMatriculaOferta)
        {
            //Oferta
            if (pMatriculaOferta.Oferta != null && !pMatriculaOferta.Oferta.ID.Equals(0))
            {
                this.PreencherCombos();
            }

            this.SetarAcaoDaTela(enumAcaoTelaMatriculaOferta.EdicaoDeUmaMatricula);
        }

        #region "Métodos Privados"

        private void SetarAcaoDaTela(enumAcaoTelaMatriculaOferta acaoTelaMatriculaOferta)
        {
            this.AcaoDaTela = (int)acaoTelaMatriculaOferta;
        }

        #endregion

        public int AcaoDaTela
        {
            get
            {
                if (ViewState["ViewStateAcaoDaTelaDeMatriculaOferta"] != null)
                {
                    return (int)ViewState["ViewStateAcaoDaTelaDeMatriculaOferta"];
                }
                return 0;
            }
            set { ViewState["ViewStateAcaoDaTelaDeMatriculaOferta"] = value; }

        }

        #region "Enumeração"

        /// <summary>
        /// Enumeração referente às ações feitas na tela de matricula de oferta em uma turma.
        /// </summary>
        private enum enumAcaoTelaMatriculaOferta
        {
            NovaMatricula = 1,
            EdicaoDeUmaMatricula = 2,
        }

        #endregion

        private void PreencherCombos()
        {
            this.PreencherComboStatus();
        }

        public void ddlTurma_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTurma.SelectedIndex == 0) return;
            var idTurma = Convert.ToInt32(ddlTurma.SelectedValue);
            var manterTurma = new ManterTurma();
            var turma = manterTurma.ObterTurmaPorID(idTurma);
            if (turma == null)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Turma não encontrada.");
                return;
            }

            if (LupaUsuario.SelectedUser == null)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Selecione um Aluno");
                return;
            }
            var usuarioSelecionado = this.LupaUsuario.SelectedUser;


            //#3474 - Se o administrador ou gestor UC tentar cadastrar um aluno em uma turma já encerrada deve aparecer os status que são referentes a finalização da turma ou seja status referente a aprovação ou reprovação.
            //#839 - Inverter a logica de exibicao
            var ofertaAbertaParaInscricoes = !(IsAdminOrGestorUc && turma.Oferta.IsAbertaParaInscricoes());

            divDataInscricao.Visible = ofertaAbertaParaInscricoes;
            divDataConclusao.Visible = ofertaAbertaParaInscricoes;

            if (ofertaAbertaParaInscricoes)
            {
                txtDataInscricao.Text = turma.Oferta.DataFimInscricoes.HasValue ? turma.Oferta.DataFimInscricoes.Value.ToString("dd/MM/yyyy") : "";
                txtDataConclusao.Text = turma.DataFinal.HasValue ? turma.DataFinal.Value.ToString("dd/MM/yyyy") : "";
            }

            //#860 -  Se a Oferta possuir Fila de espera e a Quantidade de matriculas já alcancou o limite exibe somente o status de inscrever com Fila de Espera
            bool booFilaEspera = false;
            if (turma.Oferta.FiladeEspera)
            {
                // #1083 - Verifica restricoes de permissao levando em consideracao Perfil, Nivel Ocupacional, UF e Quantidade de Vagas.
                var permisaoMatricula = VerificarPermissaoMatricula(usuarioSelecionado, turma.Oferta);
                if (permisaoMatricula)
                {
                    if (turma.Oferta.DistribuicaoVagas == enumDistribuicaoVagasOferta.VagasPorUf)
                    {
                        if (!VerificarVagasDisponiveisUF(usuarioSelecionado, turma.Oferta))
                        {
                            booFilaEspera = true;
                        }
                    }
                    else
                    {
                        if (!VerificarVagasDisponiveis(turma.Oferta))
                        {
                            booFilaEspera = true;
                        }
                    }
                }
                else
                {
                    LimparCamposMatriculaAluno();
                }
            }
            PreencherComboStatus(ofertaAbertaParaInscricoes, booFilaEspera);
        }

        private void PreencherComboStatus(bool somenteStatusParaMatricula = false, bool booFilaEspera = false)
        {
            var status = new ManterStatusMatricula().ObterStatusParaMatricula(somenteStatusParaMatricula, booFilaEspera);

            WebFormHelper.PreencherLista(status, ddlStatus, false, true);
        }

        public void PreencherComboTurma()
        {
            if (this.IdOferta == 0) return;
            var turmas = new BMTurma().ObterTurmasPorOferta(new Oferta { ID = this.IdOferta });
            if (!turmas.Any()) return;
            ddlTurma.Enabled = true;
            WebFormHelper.PreencherLista(turmas, ddlTurma, false, true);
        }

        private MatriculaTurma ObterObjetoMatriculaTurma(int idMatriculaOferta)
        {
            var matriculaOferta = new BMMatriculaOferta().ObterPorID(idMatriculaOferta);

            MatriculaTurma matriculaTurma = new MatriculaTurma();

            /*
            Demanda #3474

             usuários com perfis de gestores UC e administradores, podem fazer inscrições de alunos fora do prazo de inscrição estipulado pela oferta.
            */

            if(matriculaOferta != null && matriculaOferta.MatriculaTurma != null)
            {
                matriculaTurma = matriculaOferta.MatriculaTurma.FirstOrDefault();
            }
            else
            {
                matriculaTurma = new MatriculaTurma
                {
                    Turma = new BMTurma().ObterPorID(int.Parse(ddlTurma.SelectedValue)),
                    MatriculaOferta = matriculaOferta,
                    DataMatricula =
                                        IsAdminOrGestorUc && !string.IsNullOrEmpty(txtDataInscricao.Text)
                                            ? Convert.ToDateTime(txtDataInscricao.Text)
                                            : DateTime.Now, //Demanda #3474
                    DataTermino =
                                        IsAdminOrGestorUc && !string.IsNullOrEmpty(txtDataConclusao.Text)
                                            ? Convert.ToDateTime(txtDataConclusao.Text)
                                            : DateTime.Now //Demanda #3474
                };
            }

            if (!string.IsNullOrEmpty(txtJustificativa.Text))
            {
                matriculaTurma.Observacao = txtJustificativa.Text;
            }

            var informarDataInscricao = matriculaTurma.Turma.Oferta.DataInicioInscricoes.HasValue &&
                                        matriculaTurma.Turma.Oferta.DataFimInscricoes.HasValue &&
                                        matriculaTurma.DataMatricula.Between(
                                            matriculaTurma.Turma.Oferta.DataInicioInscricoes.Value.Date,
                                            matriculaTurma.Turma.Oferta.DataFimInscricoes.Value.Date);
            if (!informarDataInscricao)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro,
                    matriculaTurma.Turma.Oferta.DataInicioInscricoes.HasValue &&
                    matriculaTurma.Turma.Oferta.DataFimInscricoes.HasValue
                        ? "Data de Matrícula deve ser entre " +
                          matriculaTurma.Turma.Oferta.DataInicioInscricoes.Value.Date.ToString("dd/MM/yyyy") + " e " +
                          matriculaTurma.Turma.Oferta.DataFimInscricoes.Value.Date.ToString("dd/MM/yyyy")
                        : "Data de Matrícula inválida.");
                return null;
            }

            matriculaTurma.DataLimite = matriculaTurma.CalcularDataLimite(matriculaOferta.Oferta);

            return matriculaTurma;
        }

        protected void btnEnviarArquivo_Click(object sender, EventArgs e)
        {

        }

        protected void btnEfetivarMatricula_Click(object sender, EventArgs e)
        {
            try
            {
                var matriculaOferta = ObterObjetoMatriculaOferta();
                var manterMatriculaOferta = new ManterMatriculaOferta();

                SalvarMatriculaOferta(manterMatriculaOferta, matriculaOferta);

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "myscript",
                    " $('#EdicaoNivel, #modal-background').removeClass('active');", true);

                LimparCamposMatriculaAluno();

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Aluno cadastrado com sucesso.");
            }
            catch(AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
            catch (Exception ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private void ExibirModalMatriculaAdminGestor()
        {
            pnlMatriculaAdminGestor.Visible = true;
        }

        private void OcultarModalMatriculaAdminGestor()
        {
            pnlMatriculaAdminGestor.Visible = false;
        }

        public bool VerificarPermissaoMatricula(Usuario usuario, Oferta oferta)
        {
            var listaPerfil = usuario.ListaPerfil.Select(x => x.Perfil.ID).ToList();

            var permissaoPerfil = oferta.ListaPermissao.Any(x => x.Perfil != null && listaPerfil.Contains(x.Perfil.ID));
            if (!permissaoPerfil)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Oferta não disponível para o Perfil de usuario do Aluno");
                return false;
            }

            var permissaoUf = oferta.ListaPermissao.Any(x => x.Uf != null && x.Uf.ID == usuario.UF.ID);
            if (!permissaoUf)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Oferta não disponível para a UF do Aluno");
                return false;
            }

            var permissaoNivel = oferta.ListaPermissao.Any(x => x.NivelOcupacional != null && x.NivelOcupacional.ID == usuario.NivelOcupacional.ID);
            if (!permissaoNivel)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Oferta não disponível para o Nível Ocupacional do Aluno");
                return false;
            }

            return true;
        }

        public bool VerificarVagasDisponiveisUF(Usuario usuario, Oferta oferta)
        {
            bool vagaDisponivel = true;
            int qtdVagasPorEstado = oferta.ListaPermissao.Where(x => x.Uf != null && x.Uf.ID == usuario.UF.ID).Select(x => x.QuantidadeVagasPorEstado).Sum();
            var qtdVagasUtilizadasEstado = oferta.ListaMatriculaOferta.Count(x => x.Usuario.UF.ID == usuario.UF.ID && !x.IsUtilizado());

            if (qtdVagasUtilizadasEstado >= qtdVagasPorEstado)
            {
                vagaDisponivel = false;
            }

            return vagaDisponivel;
        }

        public bool VerificarVagasDisponiveis(Oferta oferta)
        {
            bool vagaDisponivel = true;
            int qtdVagas = oferta.ObterQuantidadeVagas();
            var qtdVagasUtilizadas = oferta.ListaMatriculaOferta.Count(x => !x.IsUtilizado());

            if (qtdVagasUtilizadas >= qtdVagas)
            {
                vagaDisponivel = false;
            }

            return vagaDisponivel;
        }
    }

    public class MatricularAlunoEmUmaOfertaEventArgs : EventArgs
    {
        //public Usuario UsuarioSelecionado { get; set; }

        public MatriculaOferta MatriculaOfertaCadastrada { get; set; }

        public MatricularAlunoEmUmaOfertaEventArgs(MatriculaOferta matriculaOferta)
        {
            MatriculaOfertaCadastrada = matriculaOferta;
        }
    }
}