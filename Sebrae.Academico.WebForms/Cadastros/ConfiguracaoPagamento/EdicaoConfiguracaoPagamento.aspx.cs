using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.Cadastros
{
    public partial class EdicaoConfiguracaoPagamento : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            IList<TipoPagamento> lstTipoPagamento;

            using (var manterConfiguracaoPagamento = new ManterConfiguracaoPagamento())
            {
                lstTipoPagamento = manterConfiguracaoPagamento.ObterListaTipoPagamento();
                ucPermissoes.PreencherListas();
            }

            WebFormHelper.PreencherLista(lstTipoPagamento.OrderBy(x => x.Nome).ToList(), cbxTipoPagamento, false, true);

            if (Request["Id"] != null && int.Parse(Request["Id"]) > 0)
                PreencherCampos();

        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListarConfiguracaoPagamento.aspx");
        }

        private void PreencherCampos()
        {
            using (var manCp = new ManterConfiguracaoPagamento())
            {

                var configuracaoPagamento = manCp.ObterConfiguracaoPagamentoPorId(int.Parse(Request["Id"].ToString()));

                txtFimCompetencia.Text = configuracaoPagamento.DataFimCompetencia.ToString("dd/MM/yyyy");
                txtInicioCompetencia.Text = configuracaoPagamento.DataInicioCompetencia.ToString("dd/MM/yyyy");
                txtQtdDiasInadimplencia.Text = configuracaoPagamento.QuantidadeDiasInadimplencia.ToString();
                txtQtdDiasRenovacao.Text = configuracaoPagamento.QuantidadeDiasRenovacao.ToString();
                txtQtdDiasValidade.Text = configuracaoPagamento.QuantidadeDiasValidade.ToString();
                txtValorAPagar.Text = configuracaoPagamento.ValorAPagar.ToString();
                cbxTipoPagamento.SelectedValue = configuracaoPagamento.TipoPagamento.ID.ToString();
                chkBloqueiaAcesso.Checked = configuracaoPagamento.BloqueiaAcesso;
                chkRecursiva.Checked = configuracaoPagamento.Recursiva;
                txtNomeConfiguracaoPagamento.Text = configuracaoPagamento.Nome.ToString();
                rgpSituacao.SelectedValue = configuracaoPagamento.Ativo.ToString();
                ucPermissoes.PreencherListBoxComNiveisOcupacionaisGravadosNoBanco(configuracaoPagamento.ListaConfiguracaoPagamentoPublicoAlvo.Where(x => x.NivelOcupacional != null).Select(x => x.NivelOcupacional).ToList());
                ucPermissoes.PreencherListBoxComUfsGravadasNoBanco(configuracaoPagamento.ListaConfiguracaoPagamentoPublicoAlvo.Where(x => x.UF != null).Select(x => x.UF).ToList());
                ucPermissoes.PreencherListBoxComPerfisGravadosNoBanco(configuracaoPagamento.ListaConfiguracaoPagamentoPublicoAlvo.Where(x => x.Perfil != null).Select(x => x.Perfil).ToList());
                txtQtdDiasPagamento.Text = configuracaoPagamento.QuantidadeDiasPagamento.ToString();

                //Termo de Adesão
                if (!string.IsNullOrWhiteSpace(configuracaoPagamento.TextoTermoAdesao))
                {
                    this.txtTermoAdesao.Text = configuracaoPagamento.TextoTermoAdesao;
                }
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {

            try
            {
                var configuracaoPagamento = ObterObjetoConfiguracaoPagamento();

                using (var manterConfiguracaoPagamento = new ManterConfiguracaoPagamento())
                {
                    if (Request["Id"] == null)
                    {
                        manterConfiguracaoPagamento.InserirConfiguracaoPagamento(configuracaoPagamento);
                    }
                    else
                    {
                        manterConfiguracaoPagamento.AlterarConfiguracaoPagamento(configuracaoPagamento);
                    }
                }
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso!", "ListarConfiguracaoPagamento.aspx");
                btnCancelar_Click(null, null);
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private ConfiguracaoPagamento ObterObjetoConfiguracaoPagamento()
        {
            ConfiguracaoPagamento obj;

            if (Request["Id"] == null)
                obj = new ConfiguracaoPagamento();
            else
                obj = new ManterConfiguracaoPagamento().ObterConfiguracaoPagamentoPorId(int.Parse(Request["Id"].ToString()));

            obj.BloqueiaAcesso = chkBloqueiaAcesso.Checked;
            obj.Recursiva = chkRecursiva.Checked;
            obj.Auditoria = new Auditoria(null);

            if (string.IsNullOrEmpty(txtInicioCompetencia.Text)) {
                throw new AcademicoException("Informe uma data início da competência");
            }
            if (string.IsNullOrEmpty(txtFimCompetencia.Text))
            {
                throw new AcademicoException("Informe uma data fim da competência");
            }


            DateTime dataInicioCompentenciaAux;
            if (!DateTime.TryParse(txtInicioCompetencia.Text, out dataInicioCompentenciaAux))
            {
                throw new AcademicoException("Valor inválido para o campo data início competência");
            }
            else
            {
                obj.DataInicioCompetencia = dataInicioCompentenciaAux;
            }
            
            DateTime dataFimCompentenciaAux;
            if (!DateTime.TryParse(txtFimCompetencia.Text, out dataFimCompentenciaAux))
            {
                throw new AcademicoException("Valor inválido para o campo data fim competência");
            }
            else
            {
                obj.DataFimCompetencia = dataFimCompentenciaAux;
            }

            obj.Ativo = rgpSituacao.SelectedValue == "1" ? true : false;

            //Qtd. Dias de Inadimplência
            var quantidadeDiasInadimplenciaAux = 0;
            if (!int.TryParse(txtQtdDiasInadimplencia.Text, out quantidadeDiasInadimplenciaAux))
            {
                throw new AcademicoException("Valor inválido para o campo Qtd. Dias de Inadimplência");
            }
            else
            {
                obj.QuantidadeDiasInadimplencia = quantidadeDiasInadimplenciaAux;
            }

            //Qtd. Dias Para Pagamento
            var quantidadeDiasPagamentoAux = 0;
            if (!int.TryParse(txtQtdDiasPagamento.Text, out quantidadeDiasPagamentoAux))
            {
                throw new AcademicoException("Valor inválido para o campo Qtd. Dias Para Pagamento");
            }
            else
            {
                obj.QuantidadeDiasPagamento = quantidadeDiasPagamentoAux;
            }

            //Qtd. Dias de Validade
            var quantidadeDiasValidadeAux = 0;
            if (!int.TryParse(txtQtdDiasValidade.Text, out quantidadeDiasValidadeAux))
            {
                throw new AcademicoException("Valor inválido para o campo Qtd. Dias de Validade");
            }
            else
            {
                obj.QuantidadeDiasValidade = quantidadeDiasValidadeAux;
            }

            //Qtd. Dias para Renovação
            var quantidadeDiasRenovacaoAux = 0;
            if (!int.TryParse(txtQtdDiasRenovacao.Text, out quantidadeDiasRenovacaoAux))
            {
                throw new AcademicoException("Valor inválido para o campo Qtd. Dias para Renovação");
            }
            else
            {
                obj.QuantidadeDiasRenovacao = quantidadeDiasRenovacaoAux;
            }

            obj.Nome = txtNomeConfiguracaoPagamento.Text;

            if (!string.IsNullOrWhiteSpace(cbxTipoPagamento.SelectedValue))
            {
                obj.TipoPagamento = new ManterConfiguracaoPagamento().ObterTipoPagamentoPorId(int.Parse(cbxTipoPagamento.SelectedValue));
            }
            else
            {
                throw new AcademicoException("Favor selecionar o tipo do pagamento");
            }

            //Valor a Pagar
            decimal valorAPagar = 0;
            if (!decimal.TryParse(txtValorAPagar.Text, out valorAPagar))
            {
                throw new AcademicoException("Valor inválido para o campo Valor a Pagar");
            }
            else
            {
                obj.ValorAPagar = valorAPagar;
            }

            //Termo de Adesão
            obj.TextoTermoAdesao = this.txtTermoAdesao.Text.Trim();

            obj.ListaConfiguracaoPagamentoPublicoAlvo.Clear();

            using (var manterConfiguracaoPagamento = new ManterConfiguracaoPagamento())
            {
                foreach (ListItem li in ucPermissoes.ObterTodosNiveisOcupacionais)
                {
                    if (li.Selected)
                    {
                        var no = manterConfiguracaoPagamento.ObterNivelOcupacionalPorID(int.Parse(li.Value));

                        var cfpa = new ConfiguracaoPagamentoPublicoAlvo()
                        {
                            ConfiguracaoPagamento = obj,
                            NivelOcupacional = no,
                            Auditoria = new Auditoria(null)
                        };


                        obj.ListaConfiguracaoPagamentoPublicoAlvo.Add(cfpa);
                    }
                }

                foreach (ListItem li in ucPermissoes.ObterTodosUfs)
                {

                    if (li.Selected)
                    {
                        var uf = manterConfiguracaoPagamento.ObterUFPorID(int.Parse(li.Value));
                        var cfpa = new ConfiguracaoPagamentoPublicoAlvo()
                        {
                            ConfiguracaoPagamento = obj,
                            UF = uf,
                            Auditoria = new Auditoria(null)
                        };


                        obj.ListaConfiguracaoPagamentoPublicoAlvo.Add(cfpa);
                    }

                }

                foreach (ListItem li in ucPermissoes.ObterTodosPerfis)
                {
                    if (li.Selected)
                    {
                        var p = manterConfiguracaoPagamento.ObterPerfilPorID(int.Parse(li.Value));
                        var cfpa = new ConfiguracaoPagamentoPublicoAlvo()
                        {
                            ConfiguracaoPagamento = obj,
                            Perfil = p,
                            Auditoria = new Auditoria(null)
                        };


                        obj.ListaConfiguracaoPagamentoPublicoAlvo.Add(cfpa);
                    }

                }


            }

            return obj;
        }
    }
}