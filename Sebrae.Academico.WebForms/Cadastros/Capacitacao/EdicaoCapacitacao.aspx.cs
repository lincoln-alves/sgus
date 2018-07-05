using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.BP;
using Sebrae.Academico.Util.Classes;
using classes = Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.WebForms.Cadastros.Capacitacao
{
    public partial class EdicaoCapacitacao : Page
    {
        private ManterCapacitacao manterCapacitacao = new ManterCapacitacao();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                WebFormHelper.PreencherLista(new BMPrograma().ObterTodos().OrderBy(x => x.Nome).ToList(), ddlPrograma, false, true);
                WebFormHelper.PreencherLista(new ManterCertificadoTemplate().ObterTodosAtivos().OrderBy(x => x.Nome).ToList(), ddlCertificado, false, true);
                WebFormHelper.PreencherComponenteComOpcoesSimNao(rblAlterarSituacao);
                WebFormHelper.PreencherComponenteComOpcoesSimNao(rblPermitirCancelamentoMatricula);
                WebFormHelper.PreencherComponenteComOpcoesSimNao(rblPermiteMatriculaPeloGestor);
                if (Request["Id"] != null)
                {
                    var idCapacitacao = int.Parse(Request["Id"]);
                    var capacitacao = manterCapacitacao.ObterPorID(idCapacitacao);
                    PreencherCampos(capacitacao);
                }
            }
        }

        private void PreencherCampos(classes.Capacitacao capacitacaoEdicao)
        {
            if (capacitacaoEdicao == null) return;
            txtNome.Text = capacitacaoEdicao.Nome;
            txtDescricao.Text = capacitacaoEdicao.Descricao;
            txtDtInicio.Text = capacitacaoEdicao.DataInicio.ToString("dd/MM/yyyy");
            txtDtFim.Text = capacitacaoEdicao.DataFim.HasValue ? capacitacaoEdicao.DataFim.Value.ToString("dd/MM/yyyy") : "";
            txtDtInicioInscricao.Text = capacitacaoEdicao.DataInicioInscricao.HasValue ? capacitacaoEdicao.DataInicioInscricao.Value.ToString("dd/MM/yyyy") : "";
            txtDtFimInscricao.Text = capacitacaoEdicao.DataFimInscricao.HasValue ? capacitacaoEdicao.DataFimInscricao.Value.ToString("dd/MM/yyyy") : "";
            WebFormHelper.SetarValorNoRadioButtonList(capacitacaoEdicao.PermiteAlterarSituacao, rblAlterarSituacao);
            WebFormHelper.SetarValorNoRadioButtonList(capacitacaoEdicao.PermiteCancelarMatricula, rblPermitirCancelamentoMatricula);
            WebFormHelper.SetarValorNoRadioButtonList(capacitacaoEdicao.PermiteMatriculaPeloGestor, rblPermiteMatriculaPeloGestor);

            if (capacitacaoEdicao.Certificado != null)
                WebFormHelper.SetarValorNaCombo(capacitacaoEdicao.Certificado.ID.ToString(), ddlCertificado);

            if (capacitacaoEdicao.Programa != null)
                WebFormHelper.SetarValorNaCombo(capacitacaoEdicao.Programa.ID.ToString(), ddlPrograma);
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                classes.Capacitacao capacitacao = ObterObjetoCapacitacao();
                manterCapacitacao.AlterarCapacitacao(capacitacao);
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso !", "ListarCapacitacao.aspx");
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                return;
            }
        }

        private classes.Capacitacao ObterObjetoCapacitacao()
        {
            var retorno = new classes.Capacitacao();

            if (Request["Id"] != null)
            {
                int idCapacitacao = int.Parse(Request["Id"].ToString());
                retorno = new ManterCapacitacao().ObterPorID(idCapacitacao);
            }

            if (string.IsNullOrEmpty(txtNome.Text))
                throw new AcademicoException("Informar o nome da Oferta");
            else
                retorno.Nome = txtNome.Text;

            if (string.IsNullOrEmpty(txtDescricao.Text))
                throw new AcademicoException("informar a descrição da Oferta");
            else
                retorno.Descricao = txtDescricao.Text;

            if (ddlPrograma.SelectedIndex > 0)
                retorno.Programa.ID = int.Parse(ddlPrograma.SelectedValue);
            else
                throw new AcademicoException("Informar o programa da Oferta");

            if (string.IsNullOrEmpty(txtDtInicio.Text))
                throw new AcademicoException("Informar data de início da Realização da oferta");
            else
                retorno.DataInicio = Convert.ToDateTime(txtDtInicio.Text);

            retorno.PermiteCancelarMatricula = (rblPermitirCancelamentoMatricula.SelectedValue == "S");
            retorno.PermiteAlterarSituacao = (rblAlterarSituacao.SelectedValue == "S");
            retorno.PermiteMatriculaPeloGestor = (rblPermiteMatriculaPeloGestor.SelectedValue == "S");

            if (string.IsNullOrEmpty(txtDtInicioInscricao.Text)) throw new AcademicoException("Informar uma Data de Início das inscrições para a Oferta");
            if (string.IsNullOrEmpty(txtDtFimInscricao.Text)) throw new AcademicoException("Informar uma Data de Fim das inscrições para a Oferta");

            DateTime dtIni;
            DateTime dtFim;
            if (DateTime.TryParse(txtDtInicio.Text, out dtIni))
                retorno.DataInicio = dtIni;

            if (DateTime.TryParse(txtDtFim.Text, out dtFim))
                retorno.DataFim = dtFim;

            if (DateTime.TryParse(txtDtInicioInscricao.Text, out dtIni)) retorno.DataInicioInscricao = dtIni;
            if (DateTime.TryParse(txtDtFimInscricao.Text, out dtFim)) retorno.DataFimInscricao = dtFim;

            if (retorno.DataFim.HasValue)
            {
                if (retorno.DataFim.Value.Date < retorno.DataInicio.Date)
                {
                    throw new AcademicoException("A Data Final da Realização não pode ser maior que a Data de Inicio da Realização.");
                }
            }

            if (retorno.DataFimInscricao.HasValue && retorno.DataFim.HasValue)
            {
                if (retorno.DataFimInscricao.Value.Date > retorno.DataFim.Value.Date || retorno.DataFimInscricao.Value.Date < retorno.DataInicioInscricao.Value.Date)
                {
                    throw new AcademicoException("Informar uma Data de Fim das inscrições que esteja entre a Data Inicio e Data Fim da Realização.");
                }
            }

            int idCertificado;
            if (int.TryParse(ddlCertificado.SelectedValue, out idCertificado))
            {
                retorno.Certificado = new ManterCertificadoTemplate().ObterCertificadoTemplatePorID(idCertificado);
            }

            return retorno;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListarCapacitacao.aspx");
        }

        private IList<classes.TurmaCapacitacao> ListaTurmaCapacitacao
        {
            get
            {
                return (IList<classes.TurmaCapacitacao>)Session["_TurmaCapacitacao"];
            }
            set
            {
                Session["_TurmaCapacitacao"] = value;
            }
        }

        private List<int> ListaEditadaTurmaCapacitacao
        {
            get
            {
                return (List<int>)Session["_ListaEditadaTurmaCapacitacao"];
            }
            set
            {
                Session["_ListaEditadaTurmaCapacitacao"] = value;
            }
        }

        private List<int> ListaExclusaoTurmaCapacitacao
        {
            get
            {
                return (List<int>)Session["_ListaExclusaoTurmaCapacitacao"];
            }
            set
            {
                Session["_ListaExclusaoTurmaCapacitacao"] = value;
            }
        }

        private int TurmaCapacitacaoEmEdicao
        {
            get
            {
                return (int)Session["_TurmaCapacitacaoEmEdicao"];
            }
            set
            {
                Session["_TurmaCapacitacaoEmEdicao"] = value;
            }
        }
    }
}