using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.BP.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.WebForms.Relatorios.Programas
{
    public partial class Programas : Page
    {
        private readonly ListItemCollection _lsItens = new ListItemCollection();

        protected void Page_Load(object sender, EventArgs e)
        {
            _lsItens.Add(new ListItem
            {
                Selected = true,
                Text = "Participante",
                Value = "Participante"
            });
            _lsItens.Add(new ListItem
            {
                Selected = true,
                Text = "CPF",
                Value = "CPF"
            });
            _lsItens.Add(new ListItem
            {
                Selected = true,
                Text = "E-mail",
                Value = "Email"
            });
            _lsItens.Add(new ListItem
            {
                Selected = true,
                Text = "Status Matrícula",
                Value = "StatusMatricula"
            });
            _lsItens.Add(new ListItem
            {
                Selected = true,
                Text = "UF",
                Value = "UF"
            });
            _lsItens.Add(new ListItem
            {
                Selected = true,
                Text = "Programa",
                Value = "Programa"
            });
            _lsItens.Add(new ListItem
            {
                Selected = true,
                Text = "Oferta",
                Value = "Oferta"
            });
            _lsItens.Add(new ListItem
            {
                Selected = true,
                Text = "Turma",
                Value = "Turma"
            });
            _lsItens.Add(new ListItem
            {
                Selected = true,
                Text = "Data de Inscrição",
                Value = "DataInscricao"
            });
            _lsItens.Add(new ListItem
            {
                Selected = true,
                Text = "Data da Mudança de Status de matrícula",
                Value = "DataMudStatusMatric"
            });

            if (IsPostBack) return;
            using (var relP = new RelatorioPrograma())
            {
                ViewState["_Programas"] = Helpers.Util.ObterListaAutocomplete(relP.ObterProgramas());
                WebFormHelper.PreencherLista(new List<Capacitacao>(), cbxOfertas, true);
                WebFormHelper.PreencherLista(new List<TurmaCapacitacao>(), cbxTurmas, true);
                WebFormHelper.PreencherLista(new List<Modulo>(), cbxModulos, true);
            }

            Session["dsRelatorio"] = null;
            AtualizarListaCamposVisiveis();
        }

        public string MontarTabela()
        {
            if (Session["dsRelatorio"] == null)
            {
                return "";
            }

            var html = "";

            var result = (DataTable)Session["dsRelatorio"];

            html += "<table class=\"table\">";
            html += "<thead><tr>";
            foreach (DataColumn item in result.Columns)
            {
                if (item.ColumnName.IndexOf("MD__") >= 0) continue;
                html += "<th>"+item.Caption+"</th>";
            }
            html += "</tr></thead>";

            html += "<tbody>";
            html += "<tr>";
            html += "<td colspan=\""+result.Columns.Count+"\">Nenhum Registro encontrado.</td>";
            html += "</tr>";
            html += "</tbody>";

            html += "</table>";

            return html;
        }

        protected void txtPrograma_OnTextChanged(object sender, EventArgs e)
        {
            var manterCapacitacoes = new ManterCapacitacao();
            var idPrograma = string.IsNullOrEmpty(txtPrograma.Text) ? 0 : int.Parse(txtPrograma.Text);
            WebFormHelper.PreencherLista(manterCapacitacoes.ObterPorIdPrograma(idPrograma).ToList(), cbxOfertas, true);

            var manterTurmaCapacitacao = new ManterTurmaCapacitacao();
            WebFormHelper.PreencherLista(manterTurmaCapacitacao.ObterPorIdPrograma(idPrograma).ToList(), cbxTurmas, true);

            AtualizarListaCamposVisiveis();
        }

        protected void cbxOfertas_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            AtualizarListaCamposVisiveis();
            var manterTurmaCapacitacao = new ManterTurmaCapacitacao();
            var idOferta = string.IsNullOrEmpty(cbxOfertas.SelectedValue) ? 0 : int.Parse(cbxOfertas.SelectedValue);
            WebFormHelper.PreencherLista(manterTurmaCapacitacao.ObterPorOferta(idOferta).ToList(), cbxTurmas, true);
        }

        protected void cbxModulos_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            AtualizarListaCamposVisiveis();
        }

        private IDictionary<string,string> AtualizarListaCamposVisiveis() {
            var temp = new ListItemCollection();
            foreach (ListItem item in chkListaCamposVisiveis.Items) {
                temp.Add(item);
            }
            //pegar lista já marcada nos checkbox
            var manterModulosProgramas = new ManterModulo();
            IList<Dominio.Classes.Modulo> ls;
            var resultado = new Dictionary<string, string>();
            var idPrograma = string.IsNullOrEmpty(txtPrograma.Text) ? 0 : int.Parse(txtPrograma.Text);
            var idOferta = string.IsNullOrWhiteSpace(cbxOfertas.SelectedValue) ? 0 : int.Parse(cbxOfertas.SelectedValue);
            var idModulo = string.IsNullOrWhiteSpace(cbxModulos.SelectedValue) ? 0 : int.Parse(cbxModulos.SelectedValue);

            if (idModulo != 0)
            {
                ls = new List<Modulo>
                {
                    manterModulosProgramas.ObterPorId(idModulo)
                };
            }
            else if (idOferta != 0)
            {
                ls = manterModulosProgramas.ObterPorCapacitacao(idOferta);

                WebFormHelper.PreencherLista(ls, cbxModulos, true);
            }
            else if (idPrograma != 0)
            {
                ls = manterModulosProgramas.ObterPorPrograma(idPrograma);
            }
            else
            {
                ls = manterModulosProgramas.ObterTodos();
            }

            foreach (var item in ls)
            {
                var itemPrazo = new ListItem
                {
                    Selected = true,
                    Text = item.Nome + " - Prazo",
                    Value = "MD__" + RemoveExtraChars(item.Nome) + "__prazo__" + item.ID + "__"
                };
                var itemSolucoesInscritas = new ListItem
                {
                    Selected = true,
                    Text = item.Nome + " - Soluções Inscritas",
                    Value = "MD__" + RemoveExtraChars(item.Nome) + "__SolucoesInscritas__" + item.ID + "__"
                };
                var itemSolucoesConcluidas = new ListItem
                {
                    Selected = true,
                    Text = item.Nome + " - Soluções Concluídas",
                    Value = "MD__" + RemoveExtraChars(item.Nome) + "__SolucoesConcluidas__" + item.ID + "__"
                };
                _lsItens.Add(itemPrazo);
                _lsItens.Add(itemSolucoesInscritas);
                _lsItens.Add(itemSolucoesConcluidas);

                resultado.Add(itemPrazo.Value, item.Nome);
                resultado.Add(itemSolucoesInscritas.Value, item.Nome);
                resultado.Add(itemSolucoesConcluidas.Value, item.Nome);
            }

            WebFormHelper.PreencherLista(_lsItens, chkListaCamposVisiveis);

            foreach (ListItem item in chkListaCamposVisiveis.Items) {
                var valor = true;
                if (temp.Count > 0) {
                    var item1 = item;
                    foreach (var tmp in temp.Cast<ListItem>().Where(tmp => tmp.Value == item1.Value)) {
                        valor = tmp.Selected;
                        break;
                    }
                }
                item.Selected = valor;
            }

            foreach (var item in ls)
            {
                foreach (var item2 in _lsItens.Cast<ListItem>().Where(item2 => item2.Text.IndexOf(item.Nome) >= 0))
                {
                    item2.Text = item2.Text.Replace(item.Nome + " - ", "");
                }
            }

            dgRelatorio.Columns.Clear();
            foreach (ListItem item in _lsItens)
            {
                dgRelatorio.Columns.Add(new BoundField
                {
                    HeaderText = item.Text,
                    DataField = item.Value,
                    SortExpression = item.Value
                });
            }

            return resultado;
        }

        private static string RemoveExtraChars(string value)
        {
            return Regex.Replace(RemoveDiacritics(value.ToLower()), @"[^0-9a-zA-Z]+", "");
        }

        private static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in from c in normalizedString
                              let unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c)
                              where unicodeCategory != UnicodeCategory.NonSpacingMark
                              select c)
            {
                stringBuilder.Append(c);
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        protected void OnDataBound(object sender, EventArgs e) {
            if (dgRelatorio.HeaderRow == null) return;
            if (dgRelatorio.HeaderRow.Parent.Controls.Count == 0) return;
            var row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            var r = dgRelatorio.HeaderRow.Parent.Controls[0];
            //estou assumindo que as colunas estão na ordem do chklistacamposvisiveis
            foreach (ListItem item in chkListaCamposVisiveis.Items)
            {
                if (item.Value.IndexOf("MD__") >= 0) continue;

                var c = (DataControlFieldHeaderCell)r.Controls[0];
                
                if (c.Text != item.Text && c.ContainingField.HeaderText != item.Text && c.Text != item.Value) continue;

                c.RowSpan = 2;
                r.Controls.Remove(r.Controls[0]);
                row.Controls.Add(c);
            }

            //pega os modulos para poder fazer o group dos modulos
            var manterModulosProgramas = new ManterModulo();
            IList<Dominio.Classes.Modulo> ls;

            var idPrograma = string.IsNullOrEmpty(txtPrograma.Text) ? 0 : int.Parse(txtPrograma.Text);
            var idOferta = string.IsNullOrWhiteSpace(cbxOfertas.SelectedValue) ? 0 : int.Parse(cbxOfertas.SelectedValue);
            var idModulo = string.IsNullOrWhiteSpace(cbxModulos.SelectedValue) ? 0 : int.Parse(cbxModulos.SelectedValue);

            if (idModulo != 0)
            {
                ls = new List<Modulo> { manterModulosProgramas.ObterPorId(idModulo) };
            }
            else if (idOferta != 0)
            {
                ls = manterModulosProgramas.ObterPorCapacitacao(idOferta);
            }
            else if (idPrograma != 0)
            {
                ls = manterModulosProgramas.ObterPorPrograma(idPrograma);
            }
            else
            {
                ls = manterModulosProgramas.ObterTodos();
            }

            foreach (var item in ls)
            {
                //calcula o columnSpan de acordo com a quantidade de colunas visiveis
                var qnt = chkListaCamposVisiveis.Items.Cast<ListItem>().Count(item2 => item2.Value.LastIndexOf("__" + item.ID + "__") != -1 && item2.Selected);
                var cell = new TableHeaderCell {Text = item.Nome, ColumnSpan = qnt };
                row.Controls.Add(cell);
            }
            dgRelatorio.HeaderRow.Parent.Controls.AddAt(0, row);
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            using (var relP = new RelatorioPrograma())
            {
                var id = string.IsNullOrEmpty(txtPrograma.Text) ? (int?)null : int.Parse(txtPrograma.Text);
                var idCapacitacao = string.IsNullOrWhiteSpace(cbxOfertas.SelectedValue) ? (int?)null : int.Parse(cbxOfertas.SelectedValue);
                var idTurmaCapacitacao = string.IsNullOrWhiteSpace(cbxTurmas.SelectedValue) ? (int?)null : int.Parse(cbxTurmas.SelectedValue);
                var idModulos = string.IsNullOrWhiteSpace(cbxModulos.SelectedValue) ? (int?)null : int.Parse(cbxModulos.SelectedValue);

                var lstResult = relP.ConsultarProgramas(id, idCapacitacao, idTurmaCapacitacao,idModulos);
                if (lstResult != null && lstResult.Rows.Count > 0) {
                    foreach (DataColumn item in lstResult.Columns) {
                        item.Caption = (from ListItem i in _lsItens where i.Value == item.ColumnName select i.Text).FirstOrDefault() ?? item.Caption;
                    }
                
                    // Converter os resultados do DataTable em DTO para o totalizador ficar feliz da vida.
                    var resultadoConvertido = WebFormHelper.GetDictionaryFromDataTable(lstResult).Select(resultado => new DTORelatorioPrograma
                    {
                        Cpf = resultado["Cpf"].ToString(),
                        UF = resultado["UF"].ToString(),
                        StatusMatricula = resultado["StatusMatricula"].ToString()
                    }).ToList();

                    // Converter os resultados em dados totalizadores.
                    var totalizadores = new List<DTOTotalizador>
                    {
                        TotalizadorUtil.GetTotalizador(resultadoConvertido, "Total da quantidade de alunos", "Cpf",
                            enumTotalizacaoRelatorio.ContarDistintos, false),
                        TotalizadorUtil.GetTotalizador(resultadoConvertido, "Total da quantidade por status", "StatusMatricula",
                            enumTotalizacaoRelatorio.Contar),
                        TotalizadorUtil.GetTotalizador(resultadoConvertido, "Total da quantidade de alunos UF", "UF",
                            enumTotalizacaoRelatorio.ContarDistintos)
                    };

                    ucTotalizadorRelatorio.PreencherTabela(totalizadores);

                    Session.Add("dsRelatorio", lstResult);
                    dgRelatorio.DataSource = lstResult;
                    dgRelatorio.DataBind();

                    componenteGeracaoRelatorio.Visible = true;
                    ucFormatoSaidaRelatorio.Visible = true;
                    btnPesquisar.CssClass = "btn btn-default mostrarload";
                    Filtros.CssClass = "panel-collapse collapse";
                }
                else
                {
                    ucTotalizadorRelatorio.LimparTotalizadores();
                    componenteGeracaoRelatorio.Visible = false;
                    ucFormatoSaidaRelatorio.Visible = false;
                }

                WebFormHelper.ValidarVisibilidadeCamposGrid(dgRelatorio, chkListaCamposVisiveis.Items);
            }
        }

        protected void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            if (Session["dsRelatorio"] == null)
                btnPesquisar_Click(null, null);

            var dt = (DataTable)Session["dsRelatorio"];

            //Ja assume que a Tabela esta vindo na ordem que precisa ser apresentada.
            var lsGroup = AtualizarListaCamposVisiveis();
            WebFormHelper.GerarRelatorioTable(new List<string> {"Dados do Programa"}, dt, ucFormatoSaidaRelatorio.TipoSaida, chkListaCamposVisiveis.Items, null, lsGroup);
        }
    }
}