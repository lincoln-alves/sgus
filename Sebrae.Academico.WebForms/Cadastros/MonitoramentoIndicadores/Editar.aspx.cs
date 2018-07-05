using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.WebForms.Cadastros.MonitoramentoIndicadores{
    public partial class Editar : System.Web.UI.Page{
        protected void Page_Load(object sender, EventArgs e){
            if (IsPostBack) return;
            if (Request["Id"] == null) return;
            var manter = new ManterMonitoramentoIndicadores();
            var monitoramentoIndicador = manter.ObterMonitoramentoIndicadorPorID(Convert.ToInt32(Request["id"]));
            if (monitoramentoIndicador == null){
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Monitoramento de indicador não encontrado!");
                return;
            }
            txtAno.Text = monitoramentoIndicador.Ano.ToString();
            txtAno.Enabled = false;
            pnlMonitoramentoIndicador.Visible = true;

            txtColaboradoresPactuamMetaDesenvolvimentoPADI.Text = GetValueLista(monitoramentoIndicador, "ColaboradoresPactuamMetaDesenvolvimentoPADI");
            txtEficaciaDosProgramasEducacionaisPortifolioUC.Text = GetValueLista(monitoramentoIndicador, "EficaciaDosProgramasEducacionaisPortifolioUC");
            txtEficaciaDosProgramasAcademicos.Text = GetValueLista(monitoramentoIndicador, "EficaciaDosProgramasAcademicos");
            txtAcoesDeGestaoDoConhecimentoRegistradasNoPADI.Text = GetValueLista(monitoramentoIndicador, "AcoesDeGestaoDoConhecimentoRegistradasNoPADI");
            txtProducaoDeConteudoNaPlataformaSaberCrescimento.Text = GetValueLista(monitoramentoIndicador, "ProducaoDeConteudoNaPlataformaSaberCrescimento");
            txtColaboradoresCertificadosDoSistemaSebraeUniversoExcetoSP.Text = GetValueLista(monitoramentoIndicador, "ColaboradoresCertificadosDoSistemaSebraeUniversoExcetoSP");
            txtColaboradoresCertificadosDoSistemaSebraeInscritos.Text = GetValueLista(monitoramentoIndicador, "ColaboradoresCertificadosDoSistemaSebraeInscritos");
            txtColaboradoresCertificadosSebraeNA.Text = GetValueLista(monitoramentoIndicador, "ColaboradoresCertificadosSebraeNA");
            txtCapacitacoesMetodologicasCredenciados.Text = GetValueLista(monitoramentoIndicador, "CapacitacoesMetodologicasCredenciados");
            txtRelacaoFolhaPagamentoSistemaSebrae.Text = GetValueLista(monitoramentoIndicador, "RelacaoFolhaPagamentoSistemaSebrae");
            txtPercentualExecucaoOrcamento.Text = GetValueLista(monitoramentoIndicador, "PercentualExecucaoOrcamento");
            txtExecutado.Text = GetValueLista(monitoramentoIndicador, "Executado");
            txtExecucaoPorcentagem.Text = GetValueLista(monitoramentoIndicador, "ExecucaoPorcentagem");
        }
        protected void btnSalvar_Click(object sender, EventArgs e){
            try {
                var novo = Request["Id"] == null;
                var monitoramentoIndicador = new Dominio.Classes.MonitoramentoIndicadores();
                var manter = new ManterMonitoramentoIndicadores();
                if (!novo) {
                    monitoramentoIndicador = manter.ObterMonitoramentoIndicadorPorID(Convert.ToInt32(Request["id"]));
                    if (monitoramentoIndicador == null) {
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Monitoramento de indicador não encontrado!");
                        return;
                    }

                    SalvarNaLista(ref monitoramentoIndicador, "ColaboradoresPactuamMetaDesenvolvimentoPADI", txtColaboradoresPactuamMetaDesenvolvimentoPADI.Text);
                    SalvarNaLista(ref monitoramentoIndicador, "EficaciaDosProgramasEducacionaisPortifolioUC", txtEficaciaDosProgramasEducacionaisPortifolioUC.Text);
                    SalvarNaLista(ref monitoramentoIndicador, "EficaciaDosProgramasAcademicos", txtEficaciaDosProgramasAcademicos.Text);
                    SalvarNaLista(ref monitoramentoIndicador, "AcoesDeGestaoDoConhecimentoRegistradasNoPADI", txtAcoesDeGestaoDoConhecimentoRegistradasNoPADI.Text);
                    SalvarNaLista(ref monitoramentoIndicador, "ProducaoDeConteudoNaPlataformaSaberCrescimento", txtProducaoDeConteudoNaPlataformaSaberCrescimento.Text);
                    SalvarNaLista(ref monitoramentoIndicador, "ColaboradoresCertificadosDoSistemaSebraeUniversoExcetoSP", txtColaboradoresCertificadosDoSistemaSebraeUniversoExcetoSP.Text);
                    SalvarNaLista(ref monitoramentoIndicador, "ColaboradoresCertificadosDoSistemaSebraeInscritos", txtColaboradoresCertificadosDoSistemaSebraeInscritos.Text);
                    SalvarNaLista(ref monitoramentoIndicador, "ColaboradoresCertificadosSebraeNA", txtColaboradoresCertificadosSebraeNA.Text);
                    SalvarNaLista(ref monitoramentoIndicador, "CapacitacoesMetodologicasCredenciados", txtCapacitacoesMetodologicasCredenciados.Text);
                    SalvarNaLista(ref monitoramentoIndicador, "RelacaoFolhaPagamentoSistemaSebrae", txtRelacaoFolhaPagamentoSistemaSebrae.Text);
                    SalvarNaLista(ref monitoramentoIndicador, "PercentualExecucaoOrcamento", txtPercentualExecucaoOrcamento.Text);
                    SalvarNaLista(ref monitoramentoIndicador, "Executado", txtExecutado.Text);
                    SalvarNaLista(ref monitoramentoIndicador, "ExecucaoPorcentagem", txtExecucaoPorcentagem.Text);

                    manter.AtualizarMonitoramentoIndicador(monitoramentoIndicador);

                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso!","/Cadastros/MonitoramentoIndicadores/Listar.aspx");
                } else {
                    if (string.IsNullOrEmpty(txtAno.Text)) {
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "O campo Ano deve ser informado.");
                        return;
                    }
                    monitoramentoIndicador.Ano = Convert.ToInt32(txtAno.Text);
                    manter.IncluirMonitoramentoIndicador(monitoramentoIndicador);
                    Response.Redirect("Editar.aspx?Id=" + monitoramentoIndicador.ID);
                }
            } catch (AcademicoException ex) {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        static string GetValueLista(Dominio.Classes.MonitoramentoIndicadores monitoramentoIndicador, string name){
            if (monitoramentoIndicador == null) return "";
            return monitoramentoIndicador.ListaValores.Count <= 0 ? "" : (monitoramentoIndicador.ListaValores.FirstOrDefault(p=>p.Registro.Equals(name)) ?? new MonitoramentoIndicadoresValores()).Descricao;
        }

        static void SalvarNaLista(ref Dominio.Classes.MonitoramentoIndicadores monitoramentoIndicador, string name, string value){
            if (string.IsNullOrEmpty(value)){
                if(monitoramentoIndicador.ListaValores.Any(p => p.Registro.Equals(name))){
                    monitoramentoIndicador.ListaValores.Remove(monitoramentoIndicador.ListaValores.First(p=>p.Registro.Equals(name)));
                }
            }else{
                if (monitoramentoIndicador.ListaValores.Any(p => p.Registro.Equals(name))){
                    monitoramentoIndicador.ListaValores.First(p=>p.Registro.Equals(name)).Descricao = value;
                }else{
                    monitoramentoIndicador.ListaValores.Add(new MonitoramentoIndicadoresValores{
                        MonitoramentoIndicador = new Dominio.Classes.MonitoramentoIndicadores{ ID = monitoramentoIndicador.ID },
                        Registro = name,
                        Descricao = value                        
                    });
                }
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Listar.aspx");
        }
    }
}