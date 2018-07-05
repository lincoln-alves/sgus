using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP.DTO.Relatorios;

namespace Sebrae.Academico.WebForms.Relatorios.Questionario
{
    public partial class RelatorioQuestionario : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["relatorio"] == null)
                return;

            var tipo = Request["relatorio"].ToLower();

            switch (tipo)
            {
                case "respondente":

                    var relatorioRespondente = (Cache["dsRelatorioRespondente"] as DTORelatorioQuestionarioRespondente);

                    CamposSeremExibidos = (Cache["camposSeremExibidosRelatorioRespondente"] as List<string>);
                    
                    if (CamposSeremExibidos != null)
                    {
                        Questionario.Visible = CamposSeremExibidos.Any(x => x == "Questionario");
                        Curso.Visible = CamposSeremExibidos.Any(x => x == "Curso");
                        Nome.Visible = CamposSeremExibidos.Any(x => x == "Nome");
                        NivelOcupacional.Visible = CamposSeremExibidos.Any(x => x == "NivelOcupacional");
                        UF.Visible = CamposSeremExibidos.Any(x => x == "UF");
                        Data.Visible = CamposSeremExibidos.Any(x => x == "Data");
                    }

                    if (relatorioRespondente != null)
                    {
                        var enunciados = relatorioRespondente.Enunciados.OrderBy(x => x.Id).ToList();
                        var questoes = relatorioRespondente.Questoes.OrderBy(x => x.IdEnunciado).ThenBy(x => x.Id).ToList(); 
                        var respostas =
                            BP.Relatorios.RelatorioQuestionario.ConverterDtoRespostas(relatorioRespondente.Consulta.ToList(),
                                relatorioRespondente.Questoes);

                        rptCabecalho.DataSource = enunciados;
                        rptTopicos.DataSource = questoes;
                        rptParticipacao.DataSource = respostas;

                        rptCabecalho.DataBind();
                        rptTopicos.DataBind();
                        rptParticipacao.DataBind();

                        divRespondentes.Visible = true;
                    }

                    break;
                case "estatistico":
                    var estatistico = (Cache["dsRelatorioEstatistico"] as List<DTORelatorioQuestionarioEstatistico>);

                    rptEstatistico.DataSource = estatistico;
                    rptEstatistico.DataBind();

                    divEstatistico.Visible = true;
                    break;
                case "consolidado":
                    rptConsolidado.DataSource = (Cache["dsRelatorioConsolidado"] as List<DTOQuestionarioConsolidado>);
                    rptConsolidado.DataBind();

                    divConsolidado.Visible = true;
                    break;
            }
        }

        public List<string> CamposSeremExibidos { get; set; }
        
        protected void rptPesquisa_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var item = (DTORelatorioQuestionarioEnunciado)e.Item.DataItem;

                var thCabecalho = (Literal)e.Item.FindControl("thCabecalho");
                for (var i = 0; i < item.QuestoesRelacionadas.Count(); i++)
                {
                    thCabecalho.Text += "<th>" + item.Nome + "</th>";
                }
            }
        }

        private bool ChecarExibirItem(string nome, RepeaterItemEventArgs e)
        {
            if (CamposSeremExibidos.Any(x => x == nome) == false)
            {
                e.Item.FindControl(nome).Visible = false;
                return false;
            }

            return true;
        }

        protected void rptParticipacao_OnItemDataBound(object Sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ChecarExibirItem("Questionario", e);
                ChecarExibirItem("Curso", e);
                ChecarExibirItem("Nome", e);
                ChecarExibirItem("NivelOcupacional", e);
                ChecarExibirItem("UF", e);
                ChecarExibirItem("Data", e);

                var participacao = (DTORelatorioQuestionarioParticipacao)e.Item.DataItem;

                var rptNotasTutor = (Repeater)e.Item.FindControl("rptNotasTutor");

                var listaQuestoes = participacao.Respostas.Select(x => x.Questao).ToList();
                var listaRespostas = participacao.Respostas.ToList();

                var respostas = listaQuestoes.OrderBy(x => x.Id).Select(itemQuestao => new DTORelatoriQuestaoRespostas
                {
                    Questao = itemQuestao,
                    Respostas = listaRespostas.Where(itemRespostas => itemRespostas.Questao.Id == itemQuestao.Id
                    && itemRespostas.IdProfessor == itemQuestao.IdProfessor).ToList()
                }).ToList();

                rptNotasTutor.DataSource = respostas;
                rptNotasTutor.DataBind();
            }
        }

        protected void rptNotasTutor_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var Questao = (DTORelatoriQuestaoRespostas)e.Item.DataItem;

            var rptNotas = (Repeater)e.Item.FindControl("rptNotas");
            // Obter as questões por aqui.
            rptNotas.DataSource = Questao.Respostas;
            rptNotas.DataBind();
        }

    }
}