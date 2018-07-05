using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BM.Mapeamentos;
using Sebrae.Academico.BP;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using classes = Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Dominio.Classes;

namespace Sebrae.Academico.WebForms.Cadastros.TurmaCapacitacao
{
    public partial class EditarTurmaCapacitacao : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var turmaCapacitacao = new classes.TurmaCapacitacao();
                WebFormHelper.PreencherLista(new BMPrograma().ObterTodos().OrderBy(x => x.Nome).ToList(), ddlPrograma, true);

                var listaQuestionariosDePesquisa = new ManterQuestionario().ObterQuestionariosDePesquisa();

                if (listaQuestionariosDePesquisa.Any())
                {
                    WebFormHelper.PreencherLista(listaQuestionariosDePesquisa, ddlQuestionarioPre, false, true);
                    WebFormHelper.PreencherLista(listaQuestionariosDePesquisa, ddlQuestionarioPos, false, true);

                    ddlQuestionarioPre.Enabled = true;
                    ddlQuestionarioPos.Enabled = true;
                }
                else
                {
                    PreencherQuestionarioListaVazia();
                }

                if (Request["Id"] != null)
                {
                    var idTurmaCapacitacao = int.Parse(Request["Id"]);
                    turmaCapacitacao = new BMTurmaCapacitacao().ObterPorId(idTurmaCapacitacao);
                    PreencherCampos(turmaCapacitacao);
                }

                PreencherListaUfs(turmaCapacitacao);
            }

        }

        private void PreencherQuestionarioListaVazia()
        {
            var listaVazia = new List<Questionario>
            {
                new Questionario {ID = 0, Nome = "Não há questionários"}
            };

            WebFormHelper.PreencherLista(listaVazia, ddlQuestionarioPre);
            WebFormHelper.PreencherLista(listaVazia, ddlQuestionarioPos);

            ddlQuestionarioPre.Enabled = false;
            ddlQuestionarioPos.Enabled = false;
        }

        private void PreencherCampos(classes.TurmaCapacitacao turmaCapacitacao)
        {
            if (turmaCapacitacao != null)
            {
                ddlPrograma.SelectedValue = turmaCapacitacao.Capacitacao.Programa.ID.ToString();
                ddlPrograma_OnSelectedIndexChanged(null, null);
                ddlCapacitacao.SelectedValue = turmaCapacitacao.Capacitacao.ID.ToString();
                txtNome.Text = turmaCapacitacao.Nome;
                TxtDtInicio.Text = turmaCapacitacao.DataInicio.HasValue ? turmaCapacitacao.DataInicio.Value.ToString("dd/MM/yyyy") : "";
                TxtDtFinal.Text = turmaCapacitacao.DataFim.HasValue ? turmaCapacitacao.DataFim.Value.ToString("dd/MM/yyyy") : "";
                // Questionários.
                var questionario = turmaCapacitacao.ListaQuestionarioAssociacao.FirstOrDefault(p => p.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pre);
                if (questionario != null) WebFormHelper.SetarValorNaCombo(questionario.Questionario.ID.ToString(), ddlQuestionarioPre);

                questionario = turmaCapacitacao.ListaQuestionarioAssociacao.FirstOrDefault(p => p.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pos);
                if (questionario != null) WebFormHelper.SetarValorNaCombo(questionario.Questionario.ID.ToString(), ddlQuestionarioPos);

                ddlPrograma.Enabled = 
                ddlCapacitacao.Enabled = false;
            }
        }

        protected void ddlPrograma_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPrograma.SelectedIndex > 0)
            {
                ddlCapacitacao.Enabled = true;
                var filtro = new classes.Capacitacao();
                filtro.Programa.ID = int.Parse(ddlPrograma.SelectedValue);
                WebFormHelper.PreencherLista(new BMCapacitacao().ObterPorFiltro(filtro).ToList(), ddlCapacitacao, true, false);
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                var turmaCapacitacao = ObterObjetoTurmaCapacitacao();

                new BMTurmaCapacitacao().Salvar(turmaCapacitacao);

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso !", "ListarTurmaCapacitacao.aspx");
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private void PrepararQuestionarios(int idTurmaCapacitacao = 0)
        {
            var manterTurma = new BMTurmaCapacitacao();
            classes.TurmaCapacitacao turma = null;

            if (idTurmaCapacitacao != 0)
            {
                turma = manterTurma.ObterPorId(idTurmaCapacitacao);
            }
            else if (Request["id"] != null)
            {
                turma = manterTurma.ObterPorId(int.Parse(Request["id"]));
            }

            if (turma == null) return;

            var lsIds = new List<int>();
            int id;

            int.TryParse(ddlQuestionarioPre.SelectedValue, out id);

            if (id == 0)
            {
                if (turma.ListaQuestionarioAssociacao != null)
                {
                    var questionarioAssociacao = turma.ListaQuestionarioAssociacao.FirstOrDefault(p => p.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pre);
                    if (questionarioAssociacao != null) lsIds.Add(questionarioAssociacao.ID);
                }
            }

            int.TryParse(ddlQuestionarioPos.SelectedValue, out id);

            if (id == 0)
            {
                if (turma.ListaQuestionarioAssociacao != null)
                {
                    var questionarioAssociacao = turma.ListaQuestionarioAssociacao.FirstOrDefault(p => p.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pos);
                    if (questionarioAssociacao != null) lsIds.Add(questionarioAssociacao.ID);
                }
            }
            foreach (var i in lsIds)
            {
                (new BMQuestionarioAssociacao()).Excluir(new QuestionarioAssociacao { ID = i });
            }
        }

        private classes.TurmaCapacitacao ObterObjetoTurmaCapacitacao()
        {
            var retorno = new classes.TurmaCapacitacao();

            if (Request["Id"] != null)
            {
                PrepararQuestionarios();

                retorno = new BMTurmaCapacitacao().ObterPorId(Convert.ToInt32(Request["Id"]));
            }

            if (string.IsNullOrEmpty(txtNome.Text))
                throw new AcademicoException("Informar o nome da turma");
            else
                retorno.Nome = txtNome.Text;

            if (ddlPrograma.SelectedIndex <= 0)
                throw new AcademicoException("Informar o programa da turma");

            if (ddlCapacitacao.SelectedIndex <= 0)
                throw new AcademicoException("Informar a oferta da turma");
            else{
                var objCapacitacao = new BMCapacitacao().ObterPorId(int.Parse(ddlCapacitacao.SelectedValue));
                if (objCapacitacao != null)
                   retorno.Capacitacao = objCapacitacao;
                else
                    throw new AcademicoException("A oferta não foi encontrada no banco de dados.");
            }

            // Data Início
            if (string.IsNullOrEmpty(TxtDtInicio.Text))
                throw new AcademicoException("Informar a Data Início da turma");
            else
                retorno.DataInicio = CommonHelper.TratarData(TxtDtInicio.Text, "Data de Início");


            // Validacao de Data Final
            if (!string.IsNullOrEmpty(TxtDtFinal.Text))
            {
                // Data Final
                var dataFinal = CommonHelper.TratarData(TxtDtFinal.Text.Trim(), "Data Final");
                if (dataFinal < retorno.DataInicio)
                    throw new AcademicoException("A Data Final não pode ser menor que a Data Início");
                else
                    retorno.DataFim = dataFinal;
            }

            // Questionários Associação.
            retorno.ListaQuestionarioAssociacao = ObterQuestionariosAssociacao(retorno);

            AdicionarOuRemoverUf(retorno);

            return retorno;
        }

        private List<QuestionarioAssociacao> ObterQuestionariosAssociacao(classes.TurmaCapacitacao turma)
        {
            var listaRetorno = turma.ListaQuestionarioAssociacao ?? new List<QuestionarioAssociacao>();

            // Obter Questionário Pré.
            listaRetorno = AtualizarQuestionarioAssociacao(listaRetorno.ToList(), turma, int.Parse(ddlQuestionarioPre.SelectedValue), enumTipoQuestionarioAssociacao.Pre);

            // Obter Questionário Pós.
            listaRetorno = AtualizarQuestionarioAssociacao(listaRetorno.ToList(), turma, int.Parse(ddlQuestionarioPos.SelectedValue), enumTipoQuestionarioAssociacao.Pos);

            return listaRetorno.ToList();
        }

        private IList<QuestionarioAssociacao> AtualizarQuestionarioAssociacao(IList<QuestionarioAssociacao> listaRetorno, classes.TurmaCapacitacao turma, int idQuestionario, enumTipoQuestionarioAssociacao tipo)
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
            if (questionarioAssociacao == null) listaRetorno.Add(ObterQuestionarioAssociacao(turma, tipo, questionario));
            else
            {
                var index = listaRetorno.IndexOf(questionarioAssociacao);
                listaRetorno[index].Questionario = questionario;
                listaRetorno[index].TipoQuestionarioAssociacao = new ManterTipoQuestionarioAssociacao().ObterTipoQuestionarioAssociacaoPorID((int)tipo);
            }
            return listaRetorno;
        }

        private QuestionarioAssociacao ObterQuestionarioAssociacao(classes.TurmaCapacitacao turma, enumTipoQuestionarioAssociacao tipo, Questionario questionario)
        {
            var questionarioPreAssociacao = new QuestionarioAssociacao
            {
                TipoQuestionarioAssociacao = new ManterTipoQuestionarioAssociacao().ObterTipoQuestionarioAssociacaoPorID((int)tipo),
                Questionario = questionario,
                Evolutivo = false,
                TurmaCapacitacao = turma,
                Obrigatorio = true
            };
            return questionarioPreAssociacao;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListarTurmaCapacitacao.aspx");
        }

        private void PreencherListaUfs(classes.TurmaCapacitacao turmaCapacitacao)
        {
            ucPermissoes2.PreencherListas(exibirVagasUfs: true);
            OcultarOutrasPermissoes();

            //Obtém a lista de ufs
            var listaUFs = turmaCapacitacao.ListaPermissao.Where(x => x.Uf != null)
                .Select(x => new Uf {ID = x.Uf.ID, Nome = x.Uf.Nome}).ToList();

            ucPermissoes2.PreencherListBoxComUfsGravadasNoBanco(listaUFs, true, turmaCapacitacao.ListaPermissao);
        }

        private void OcultarOutrasPermissoes()
        {
            try
            {
                var divPerfil = this.ucPermissoes2.FindControl("divPerfil").Visible = false;
                var divNivelOcupacional = this.ucPermissoes2.FindControl("divNivelOcupacional").Visible = false;
            }
            catch
            {
            }
        }

        private void AdicionarOuRemoverUf(classes.TurmaCapacitacao turmaCapacitacao)
        {
            Repeater todosUfs = (Repeater)this.ucPermissoes2.FindControl("rptUFs");

            turmaCapacitacao.ListaPermissao.Clear();

            if (todosUfs != null && todosUfs.Items.Count > 0)// todosUfs.Count > 0)
            {

                Uf ufSelecionado = null;
                
                for (int i = 0; i < todosUfs.Items.Count; i++)
                {
                    CheckBox ckUF = (CheckBox)todosUfs.Items[i].FindControl("ckUF");
                    Literal lblUF = (Literal)todosUfs.Items[i].FindControl("lblUF");
                    TextBox txtVagas = (TextBox)todosUfs.Items[i].FindControl("txtVagas");
                    ufSelecionado = new Uf()
                    {
                        ID = int.Parse(ckUF.Attributes["ID_UF"]),// int.Parse(todosUfs[i].Value),
                        Nome = ckUF.Text//  todosUfs[i].Text
                    };

                    if (ckUF.Checked) // (todosUfs[i].Selected)
                    {
                        int vagas = 0;
                        if (!string.IsNullOrEmpty(txtVagas.Text))
                            vagas = int.Parse(txtVagas.Text);

                        turmaCapacitacao.ListaPermissao.Add(new TurmaCapacitacaoPermissao
                        {
                            TurmaCapacitacao = turmaCapacitacao,
                            Uf = ufSelecionado,
                            QuantidadeVagasPorEstado = vagas                            
                        });
                    }
                }
            }
        }
    }
}