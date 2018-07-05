<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="Editar.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.MonitoramentoIndicadores.Editar" %>
<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <fieldset>
            <div class="form-group">
                <asp:Label ID="lblAno" runat="server" Text="Ano" AssociatedControlID="lblAno"></asp:Label>
                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip1" Chave="txtAno" />
                <asp:TextBox ID="txtAno" runat="server" MaxLength="150" CssClass="form-control number-only"></asp:TextBox>
            </div>
            <asp:Panel ID="pnlMonitoramentoIndicador" runat="server" Visible="false">
                <h3>PARTICIPAÇÃO NAS CAPACITAÇÕES</h3>
                <div class="form-group">
                    <asp:Label ID="lblColaboradoresPactuamMetaDesenvolvimentoPADI" runat="server" Text="Colaboradores que pactuam meta de desenvolvimento (PADI)" AssociatedControlID="txtColaboradoresPactuamMetaDesenvolvimentoPADI"></asp:Label>
                    <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip2" Chave="txtColaboradoresPactuamMetaDesenvolvimentoPADI" />
                    <asp:TextBox ID="txtColaboradoresPactuamMetaDesenvolvimentoPADI" runat="server" MaxLength="150" CssClass="form-control number-only" data-separator="."></asp:TextBox>
                </div>
                <h3>EFICÁCIA E SATISFAÇÃO</h3>
                <div class="form-group">
                    <asp:Label ID="lblEficaciaDosProgramasEducacionaisPortifolioUC" runat="server" Text="Eficácia dos programas educacionais (Portfólio UC) - (%)" AssociatedControlID="txtEficaciaDosProgramasEducacionaisPortifolioUC"></asp:Label>
                    <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip3" Chave="txtEficaciaDosProgramasEducacionaisPortifolioUC" />
                    <asp:TextBox ID="txtEficaciaDosProgramasEducacionaisPortifolioUC" runat="server" MaxLength="150" CssClass="form-control number-only" data-separator=","></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Label ID="lblEficaciaDosProgramasAcademicos" runat="server" Text="Eficácia dos programas acadêmicos - (%)" AssociatedControlID="txtEficaciaDosProgramasAcademicos"></asp:Label>
                    <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip4" Chave="txtEficaciaDosProgramasAcademicos" />
                    <asp:TextBox ID="txtEficaciaDosProgramasAcademicos" runat="server" MaxLength="150" CssClass="form-control number-only" data-separator=","></asp:TextBox>
                </div>
                <h3>GESTÃO DO CONHECIMENTO</h3>
                <div class="form-group">
                    <asp:Label ID="lblAcoesDeGestaoDoConhecimentoRegistradasNoPADI" runat="server" Text="Ações de Gestão do Conhecimento registradas no PADI - (%)" AssociatedControlID="txtAcoesDeGestaoDoConhecimentoRegistradasNoPADI"></asp:Label>
                    <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip5" Chave="txtAcoesDeGestaoDoConhecimentoRegistradasNoPADI" />
                    <asp:TextBox ID="txtAcoesDeGestaoDoConhecimentoRegistradasNoPADI" runat="server" MaxLength="150" CssClass="form-control number-only" data-separator=","></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Label ID="lblProducaoDeConteudoNaPlataformaSaberCrescimento" runat="server" Text="Produção de conteúdos na Plataforma Saber (crescimento) - (%)" AssociatedControlID="txtProducaoDeConteudoNaPlataformaSaberCrescimento"></asp:Label>
                    <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip6" Chave="txtProducaoDeConteudoNaPlataformaSaberCrescimento" />
                    <asp:TextBox ID="txtProducaoDeConteudoNaPlataformaSaberCrescimento" runat="server" MaxLength="150" CssClass="form-control number-only" data-separator=","></asp:TextBox>
                </div>
                <h3>CERTIFICAÇÃO</h3>
                <div class="form-group">
                    <asp:Label ID="lblColaboradoresCertificadosDoSistemaSebraeUniversoExcetoSP" runat="server" Text="Colaboradores certificados do Sistema Sebrae (em relação ao universo, exceto SP) - (%)" AssociatedControlID="txtColaboradoresCertificadosDoSistemaSebraeUniversoExcetoSP"></asp:Label>
                    <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip7" Chave="txtColaboradoresCertificadosDoSistemaSebraeUniversoExcetoSP" />
                    <asp:TextBox ID="txtColaboradoresCertificadosDoSistemaSebraeUniversoExcetoSP" runat="server" MaxLength="150" CssClass="form-control number-only" data-separator=","></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Label ID="lblColaboradoresCertificadosDoSistemaSebraeInscritos" runat="server" Text="Colaboradores certificados do Sistema Sebrae (em relação aos inscritos) - (%)" AssociatedControlID="txtColaboradoresCertificadosDoSistemaSebraeInscritos"></asp:Label>
                    <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip8" Chave="txtColaboradoresCertificadosDoSistemaSebraeInscritos" />
                    <asp:TextBox ID="txtColaboradoresCertificadosDoSistemaSebraeInscritos" runat="server" MaxLength="150" CssClass="form-control number-only" data-separator=","></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Label ID="lblColaboradoresCertificadosSebraeNA" runat="server" Text="Colaboradores certificados do Sebrae-NA (em relação aos inscritos) - (%)" AssociatedControlID="txtColaboradoresCertificadosSebraeNA"></asp:Label>
                    <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip9" Chave="txtColaboradoresCertificadosSebraeNA" />
                    <asp:TextBox ID="txtColaboradoresCertificadosSebraeNA" runat="server" MaxLength="150" CssClass="form-control number-only" data-separator=","></asp:TextBox>
                </div>
                <h3>INVESTIMENTO</h3>
                <div class="form-group">
                    <asp:Label ID="lblCapacitacoesMetodologicasCredenciados" runat="server" Text="Em capacitações metodológicas para credenciados - (%)" AssociatedControlID="txtCapacitacoesMetodologicasCredenciados"></asp:Label>
                    <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip10" Chave="txtCapacitacoesMetodologicasCredenciados" />
                    <asp:TextBox ID="txtCapacitacoesMetodologicasCredenciados" runat="server" MaxLength="150" CssClass="form-control number-only" data-separator=","></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Label ID="lblRelacaoFolhaPagamentoSistemaSebrae" runat="server" Text="Em relação à folha de pagamento do Sistema Sebrae - (%)" AssociatedControlID="txtRelacaoFolhaPagamentoSistemaSebrae"></asp:Label>
                    <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip11" Chave="txtRelacaoFolhaPagamentoSistemaSebrae" />
                    <asp:TextBox ID="txtRelacaoFolhaPagamentoSistemaSebrae" runat="server" MaxLength="150" CssClass="form-control number-only" data-separator=","></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Label ID="lblPercentualExecucaoOrcamento" runat="server" Text="Percentual de execução do orçamento - (%)" AssociatedControlID="txtPercentualExecucaoOrcamento"></asp:Label>
                    <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip12" Chave="txtPercentualExecucaoOrcamento" />
                    <asp:TextBox ID="txtPercentualExecucaoOrcamento" runat="server" MaxLength="150" CssClass="form-control number-only" data-separator=","></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Label ID="lblExecutado" runat="server" data-help="" Text="Executado" AssociatedControlID="txtExecutado"></asp:Label>
                    <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip13" Chave="txtExecutado" />
                    <asp:TextBox ID="txtExecutado" runat="server" MaxLength="150" CssClass="form-control number-only"  data-separator="."></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Label ID="lblExecucaoPorcentagem" runat="server" data-help="" Text="Execução - (%)" AssociatedControlID="txtExecucaoPorcentagem"></asp:Label>
                    <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip14" Chave="txtExecucaoPorcentagem" />
                    <asp:TextBox ID="txtExecucaoPorcentagem" runat="server" MaxLength="150" CssClass="form-control number-only" data-separator=","></asp:TextBox>
                </div>
            </asp:Panel>
        </fieldset>
        <asp:Button ID="btnSalvar" CssClass="btn btn-primary" runat="server" Text="Salvar" OnClick="btnSalvar_Click" />
        <asp:Button ID="btnCancelar" CssClass="btn btn-default" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" />
    </div>
    <script src="/js/jquery.numeric.js"></script>
    <script type="text/javascript">
        (function ($) {
            if (!$) {
                alert('Jquery not found!');
                return;
            }
            $(document).ready(function () {
                $('.number-only').each(function() {
                    var e = $(this);
                    var separator = e.attr('data-separator') || '';
                    if (separator !== '') {
                        e.numeric(separator);
                    }
                    else e.numeric();
                });
            });
        })(jQuery)
    </script>
</asp:Content>
