using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucMatrilhaTrilha : System.Web.UI.UserControl
    {

        public int? IdUsuarioTrilhaEdit { get; set; }

        #region "Métodos Privados"

        public void PreencherCombos()
        {
            try
            {
                PreencherComboTrilhas();
                PreencherComboStatus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void PreencherComboStatus()
        {
            var manterStatusMatricula = new ManterStatusMatricula();

            var listaStatus =
                manterStatusMatricula.ObterStatusMatriculaDeTrilhas()
                    .Where(p => p.ID != (int) enumStatusMatricula.Concluido)
                    .ToList();

            WebFormHelper.PreencherLista(listaStatus, ddlStatus, false, true);
        }

        private void PreencherComboTrilhas()
        {
            var manterTrilha = new ManterTrilha();

            var listaTrilhas = manterTrilha.ObterTodasTrilhas();

            WebFormHelper.PreencherLista(listaTrilhas, ddlTrilha, false, true);
        }


        #endregion

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
            IList<TrilhaNivel> ListaTrilhaNivel = new ManterTrilhaNivel().ObterPorTrilha(trilha);
            WebFormHelper.PreencherLista(ListaTrilhaNivel, this.ddlTrilhaNivel, false, true);
        }

        public UsuarioTrilha usuarioTrilhaEdicao { get; set; }

        internal void EsconderCheckBoxDeAcessoBloqueado()
        {
            this.divAcessoBloqueado.Visible = false;
        }

        internal void ExibirCheckBoxDeAcessoBloqueado()
        {
            this.divAcessoBloqueado.Visible = true;
        }

        internal void HabilitarComboDeStatus()
        {
            this.ddlStatus.Enabled = true;
        }

        internal void DesabilitarComboDeStatus()
        {
            this.ddlStatus.Enabled = false;
        }

        internal void PreencherCampos(Dominio.Classes.UsuarioTrilha usuarioTrilha)
        {
            if (usuarioTrilha != null)
            {
                WebFormHelper.SetarValorNaCombo(usuarioTrilha.TrilhaNivel.Trilha.ID.ToString(), ddlTrilha);

                IList<TrilhaNivel> ListaTrilhaNivel = new List<TrilhaNivel>();
                ListaTrilhaNivel.Add(new TrilhaNivel() { ID = usuarioTrilha.TrilhaNivel.ID, Nome = usuarioTrilha.TrilhaNivel.Nome });
                WebFormHelper.PreencherLista(ListaTrilhaNivel, ddlTrilhaNivel);

                WebFormHelper.SetarValorNaCombo(((int)usuarioTrilha.StatusMatricula).ToString(), ddlStatus);

                if (usuarioTrilha.DataFim.HasValue)
                    txtDtFim.Text = usuarioTrilha.DataFim.Value.ToShortDateString();

                if (usuarioTrilha.DataLimite != null)
                    txtDtLimite.Text = usuarioTrilha.DataLimite.ToShortDateString();

                if (usuarioTrilha.NotaProva.HasValue)
                    txtNotaProva.Text = usuarioTrilha.NotaProva.Value.ToString();

                if (usuarioTrilha.DataLiberacaoNovaProva.HasValue)
                    txtDataLiberacaoNovaProva.Text = usuarioTrilha.DataLiberacaoNovaProva.Value.ToString("dd/MM/yyyy");

                chkLiberarNovaProva.Checked = usuarioTrilha.NovaProvaLiberada;

                this.LupaUsuario1.SelectedUser = usuarioTrilha.Usuario;
                this.LupaUsuario1.DataBind();

                if (usuarioTrilha.QTEstrelasPossiveis.ToString().Length > 0)
                    lblQtdEstrelas.Text = string.Format("{0}/{1}", usuarioTrilha.QTEstrelas.ToString(),
                                                            usuarioTrilha.QTEstrelasPossiveis.ToString());
                else
                    lblQtdEstrelas.Text = "N/D";

                ddlStatus.Enabled = false;
            }

        }

        internal void DefinirQuantidadeDeRegistrosDeRetorno()
        {
            this.LupaUsuario1.QtdRegistros = 52;
        }

        public UsuarioTrilha ObterObjetoUsuarioTrilha(bool buscarDataAlteracaoStatus = false)
        {
            usuarioTrilhaEdicao = null;

            usuarioTrilhaEdicao = !IdUsuarioTrilhaEdit.HasValue ? new UsuarioTrilha() : new ManterMatriculaTrilha().ObterMatriculaTrilhaPorID(IdUsuarioTrilhaEdit.Value);

            var status_matricula_anterior = usuarioTrilhaEdicao.StatusMatricula;

            //Usuario
            usuarioTrilhaEdicao.Usuario = LupaUsuario1.SelectedUser;

            if (usuarioTrilhaEdicao.Usuario == null){
                throw new AcademicoException("Selecione o Usuário que irá ser matriculado na Trilha.");
            }
            usuarioTrilhaEdicao.NivelOcupacional = usuarioTrilhaEdicao.Usuario.NivelOcupacional;


            //Trilha Nivel
            if (this.ddlTrilhaNivel.Items.Count > 0 && !string.IsNullOrWhiteSpace(this.ddlTrilhaNivel.SelectedItem.Value)){
                usuarioTrilhaEdicao.TrilhaNivel = new BM.Classes.BMTrilhaNivel().ObterPorID(int.Parse(this.ddlTrilhaNivel.SelectedItem.Value));
            }

            //Status
            var idStatus = int.Parse(this.ddlStatus.SelectedItem.Value);
            if (idStatus != 0 ) usuarioTrilhaEdicao.StatusMatricula = (enumStatusMatricula)idStatus;
            
            if (buscarDataAlteracaoStatus && usuarioTrilhaEdicao.StatusMatricula != status_matricula_anterior)
            {
                usuarioTrilhaEdicao.DataAlteracaoStatus = DateTime.Now;
            }

            VerificarDatas();

            //Nota da Prova
            decimal notaProva = 0;

            if (!string.IsNullOrWhiteSpace(this.txtNotaProva.Text))
            {
                if (!decimal.TryParse(this.txtNotaProva.Text.Trim(), out notaProva))
                    throw new AcademicoException("Valor Inválido para o Campo Nota Prova.");
                usuarioTrilhaEdicao.NotaProva = notaProva;
            }


            var manterUf = new ManterUf();
            usuarioTrilhaEdicao.Uf = manterUf.ObterUfPorID(usuarioTrilhaEdicao.Usuario.UF.ID);

            usuarioTrilhaEdicao.NovaProvaLiberada = chkLiberarNovaProva.Checked;
            if (chkLiberarNovaProva.Checked == false){
                usuarioTrilhaEdicao.DataLiberacaoNovaProva = null;
            }

            if (usuarioTrilhaEdicao.NovaProvaLiberada){
                usuarioTrilhaEdicao.StatusMatricula = enumStatusMatricula.Inscrito;
            }
            
            return usuarioTrilhaEdicao;
        }

        private void VerificarDatas()
        {

            //Data Fim
            usuarioTrilhaEdicao.DataFim = CommonHelper.TratarData(this.txtDtFim.Text, "Data Fim");

            //Data Limite
            if (!string.IsNullOrWhiteSpace(this.txtDtLimite.Text)) { 
                var dataLimite = CommonHelper.TratarDataObrigatoria(this.txtDtLimite.Text, "Data limite");
                if (usuarioTrilhaEdicao.DataLimite != dataLimite){
                    if (dataLimite.Date > DateTime.Now){
                        usuarioTrilhaEdicao.StatusMatricula = enumStatusMatricula.Inscrito;
                    }
                    usuarioTrilhaEdicao.DataLimite = dataLimite;
                }
            }

            //Data Nova Prova
            usuarioTrilhaEdicao.DataLiberacaoNovaProva = CommonHelper.TratarData(this.txtDataLiberacaoNovaProva.Text, "Data liberação nova prova");
            if (usuarioTrilhaEdicao.DataLiberacaoNovaProva != null && usuarioTrilhaEdicao.DataLimite != null)
            {
                if ((usuarioTrilhaEdicao.DataLimite > usuarioTrilhaEdicao.DataLiberacaoNovaProva))
                {
                    TimeSpan ts = usuarioTrilhaEdicao.DataLimite.Date - usuarioTrilhaEdicao.DataLiberacaoNovaProva.Value.Date;
                    if (ts.Days < 7)
                    {
                        double incremento = (7 - ts.Days);
                        usuarioTrilhaEdicao.DataLimite.AddDays(incremento+1);
                    }
                }
                else
                {
                    usuarioTrilhaEdicao.DataLimite = usuarioTrilhaEdicao.DataLiberacaoNovaProva.Value.Date.AddDays(7.0);
                }
            }            
        }

        private bool StatusNaoAprovadoENovaProvaNaoLiberada
        {
            get
            {

                bool statusNaoAprovadoENovaProvaNaoLiberada = false;

                if (usuarioTrilhaEdicao.StatusMatricula.Equals(enumStatusMatricula.Reprovado) &&
                   usuarioTrilhaEdicao.NovaProvaLiberada.Equals(false))
                {
                    statusNaoAprovadoENovaProvaNaoLiberada = true;
                }

                return statusNaoAprovadoENovaProvaNaoLiberada;
            }
        }

        private bool StatusNaoAprovadoENovaProvaLiberada
        {
            get
            {

                bool statusNaoAprovadoENovaProvaLiberada = false;

                if (usuarioTrilhaEdicao.StatusMatricula.Equals(enumStatusMatricula.Reprovado) &&
                   (usuarioTrilhaEdicao.NovaProvaLiberada.Equals(true)))
                {
                    statusNaoAprovadoENovaProvaLiberada = true;
                }

                return statusNaoAprovadoENovaProvaLiberada;
            }
        }


        internal void TratarStatus(UsuarioTrilha usuarioTrilhaEdicao)
        {
            this.TratarStatusNaoAprovado(usuarioTrilhaEdicao);
        }

        private void TratarStatusNaoAprovado(UsuarioTrilha usuarioTrilhaEdicao)
        {
            this.usuarioTrilhaEdicao = usuarioTrilhaEdicao;
            //this.chkLiberarNovaProva.Enabled = false;
            //this.txtDataLiberacaoNovaProva.Enabled = false;

            if (this.StatusNaoAprovadoENovaProvaNaoLiberada)
            {
                //Exibe o check box "Liberar Nova Prova" 
                this.chkLiberarNovaProva.Enabled = true;
                this.txtDataLiberacaoNovaProva.Enabled = true;

                //Se informou a Data de Liberação da Nova Prova
                if (!string.IsNullOrWhiteSpace(txtDataLiberacaoNovaProva.Text))
                {
                    //usuarioTrilhaEdicao.DataLiberacaoNovaProva = CommonHelper.TratarData(this.txtDataLiberacaoNovaProva.Text, "Data Fim");
                    this.txtDataLiberacaoNovaProva.Text = usuarioTrilhaEdicao.DataLiberacaoNovaProva.Value.ToShortDateString();
                    this.txtDataLiberacaoNovaProva.Enabled = true;
                }

            }
            else if (StatusNaoAprovadoENovaProvaLiberada)
            {
                //Esconde o check box "Liberar Nova Prova" 
                this.chkLiberarNovaProva.Enabled = true;
                this.txtDataLiberacaoNovaProva.Enabled = true;
                this.chkLiberarNovaProva.Checked = true;
            }
        }


        internal void TratarInformacoesDaMatricula()
        {
            //Cadastro
            WebFormHelper.SetarValorNaCombo(((int)enumStatusMatricula.Inscrito).ToString(), ddlStatus, true);
            this.DesabilitarComboDeStatus();
            this.EsconderCheckBoxDeAcessoBloqueado();
        }



        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListarMatriculaTrilha.aspx");
        }



    }
}