<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="MonitorTrilhas.aspx.cs" Inherits="Sebrae.Academico.WebForms.Relatorios.MonitoramentoTrilhas.MonitorTrilhas"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/UserControls/ucTotalizadorRelatorio.ascx" TagPrefix="uc" TagName="ucTotalizadorRelatorio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        input.form-control {
            width: 100% !important;
        }
    </style>
    <div class="panel-group" id="gruporelatorio">
        <div class="panel panel-default">
            <div class="panel-heading">
                <a data-toggle="collapse" data-parent="#gruporelatorio" href="#Filtros">Filtros
                </a>
            </div>
            <asp:Panel ID="Filtros" runat="server" ClientIDMode="Static" CssClass="panel-collapse collapse in">
                <div class="panel-body">
                    <fieldset>
                        <div class="form-group">
                            <asp:Label ID="lblTrilhas" runat="server" Text="Trilhas" AssociatedControlID="cbxTrilha" />
                            <asp:DropDownList ID="cbxTrilha" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="cbxTrilha_OnSelectedIndexChanged"/>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label3" runat="server" Text="Nível da trilha" AssociatedControlID="cbxNivelTrilha" />
                            <asp:DropDownList ID="cbxNivelTrilha" runat="server" CssClass="form-control"  AutoPostBack="true" OnSelectedIndexChanged="cbxNivelTrilha_OnSelectedIndexChanged">
                                <asp:ListItem Selected="True" Text="-- Todos ou Selecione uma Trilha --" Value="">
                                </asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label1" runat="server" Text="Nome do Monitor" AssociatedControlID="cbxMonitor" />
                            <asp:DropDownList ID="cbxMonitor" runat="server" CssClass="form-control" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label2" runat="server" Text="Tipo de Participação" AssociatedControlID="cbxTipoParticipacao" />
                            <asp:DropDownList ID="cbxTipoParticipacao" runat="server" CssClass="form-control"  AutoPostBack="true" OnSelectedIndexChanged="cbxTipoParticipacao_OnSelectedIndexChanged">
                                <asp:ListItem Selected="True" Text="-- Todos --" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Solução Educacional" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Solução Educacional Auto indicativa (participação)" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Sprint" Value="3"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label4" runat="server" Text="Período data início" AssociatedControlID="txtDataInicio" />
                            <asp:TextBox ID="txtDataInicio" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label5" runat="server" Text="Período data fim" AssociatedControlID="txtDataFinal" />
                            <asp:TextBox ID="txtDataFinal" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </fieldset>
                </div>
            </asp:Panel>
        </div>
        <div class="panel panel-default">
            <div class="panel-heading">
                <a data-toggle="collapse" data-parent="#gruporelatorio" href="#Campos">Campos a Serem
                    Exibidos </a>
            </div>
            <div id="Campos" class="accordion-body collapse">
                <div class="accordion-inner">
                    <fieldset>
                        <div class="form-group">
                            <asp:CheckBoxList ID="chkListaCamposVisiveis" runat="server" RepeatDirection="Vertical"
                                RepeatLayout="UnorderedList" ClientIDMode="Static">
                                <asp:ListItem Selected="True" Value="Monitor">Monitor</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Trilha">Trilha</asp:ListItem>
                                <asp:ListItem Selected="True" Value="NivelTrilha">Nível da Trilha</asp:ListItem>
                                <asp:ListItem Selected="True" Value="QtdAcessoMonitorPeriodo">Quantidade de acessos do monitor</asp:ListItem>
                                <asp:ListItem Selected="True" Value="HoraDiaUltimoAcessoMonitor">Hora do último acesso do monitor</asp:ListItem>
                                <asp:ListItem Selected="True" Value="QtdParticipacaoSEAnalisadasMonitorPeriodo">Quantidade de participações em Soluções Educacionais analisadas pelo monitor</asp:ListItem>
                                <asp:ListItem Selected="True" Value="QtdParticipacoesSEPrazoVencido">Quantidade de participações em Soluções Educacionais com prazo de análise vencido</asp:ListItem>
                                <asp:ListItem Selected="True" Value="QtdParticipacoesSEPrazoVigente">Quantidade de participações em Soluções Educacionais com prazo de análise vigente</asp:ListItem>
                                <asp:ListItem Selected="True" Value="QtdParticipacoesSEEmRevisao">Quantidade de participações em Soluções Educacionais com status "Em revisão"</asp:ListItem>
                                <asp:ListItem Selected="True" Value="QtdParticipacoesSEPendente">Quantidade de participações em Soluções Educacionais com status "Pendentes"</asp:ListItem>
                                <asp:ListItem Selected="True" Value="QtdParticipacoesSEAprovadas">Quantidade de participações em Soluções Educacionais com status "Aprovadas"</asp:ListItem>
                                <asp:ListItem Selected="True" Value="QtdParticipacaoSEAIAnalisadasMonitorPeriodo">Quantidade de participações em Soluções Educacionais Auto Indicativas (participação) analisadas pelo monitor</asp:ListItem>
                                <asp:ListItem Selected="True" Value="QtdParticipacoesSEAIPrazoVencido">Quantidade de participações em Soluções Educacionais Auto indicativas (participação) com prazo de análise vencido</asp:ListItem>
                                <asp:ListItem Selected="True" Value="QtdParticipacoesSEAIPrazoVigente">Quantidade de participações em Soluções Educacionais Auto indicativas (participação) com prazo de análise vigente</asp:ListItem>
                                <asp:ListItem Selected="True" Value="QtdParticipacoesSEAIEmRevisao">Quantidade de participações em Soluções Educacionais Auto indicativas (participação) com status "Em revisão"</asp:ListItem>
                                <asp:ListItem Selected="True" Value="QtdParticipacoesSEAIPendente">Quantidade de participações em Soluções Educacionais Auto indicativas (participação) com status "Pendentes"</asp:ListItem>
                                <asp:ListItem Selected="True" Value="QtdParticipacoesSEAIAprovadas">Quantidade de participações em Soluções Educacionais Auto indicativas (participação) com status "Aprovadas"</asp:ListItem>
                                <asp:ListItem Selected="True" Value="QtdParticipacaoSprintAnalisadasMonitorPeriodo">Quantidade de participações em Sprint analisadas pelo monitor</asp:ListItem>
                                <asp:ListItem Selected="True" Value="QtdParticipacoesSprintPrazoVencido">Quantidade de participações em Sprint com prazo de análise vencido</asp:ListItem>
                                <asp:ListItem Selected="True" Value="QtdParticipacoesSprintPrazoVigente">Quantidade de participações em Sprint com prazo de análise vigente</asp:ListItem>
                                <asp:ListItem Selected="True" Value="QtdParticipacoesSprintEmRevisao">Quantidade de participações em Sprint com status "Em revisão"</asp:ListItem>
                                <asp:ListItem Selected="True" Value="QtdParticipacoesSprintPendente">Quantidade de participações em Sprint com status "Pendentes"</asp:ListItem>
                                <asp:ListItem Selected="True" Value="QtdParticipacoesSprintAprovadas">Quantidade de participações em Sprint com status "Aprovadas"</asp:ListItem>
                                <asp:ListItem Selected="True" Value="TotalDeParticipacoesVinculadasMonitor">Total de participações vinculadas ao Monitor</asp:ListItem>
                            </asp:CheckBoxList>
                        </div>
                    </fieldset>
                </div>
            </div>
        </div>
    </div>
    <br />
    <asp:Button ID="btnPesquisar" runat="server" Text="Consultar" CssClass="btn btn-primary mostrarload"
        OnClick="btnPesquisar_Click" />
    <hr />
    
    <uc:ucTotalizadorRelatorio runat="server" ID="ucTotalizadorRelatorio" />

    <asp:GridView ID="dgRelatorio" runat="server" AutoGenerateColumns="False" AllowSorting="True"
        CssClass="table col-sm-12" GridLines="None" OnSorting="dgRelatorio_Sorting" AllowPaging="True"
        OnPageIndexChanging="dgRelatorio_PageIndexChanging" PageSize="100">
        <Columns>
            <asp:BoundField DataField="Monitor" HeaderText="Monitor" SortExpression="Monitor" />
            <asp:BoundField DataField="Trilha" HeaderText="Trilha" SortExpression="Trilha" />
            <asp:BoundField DataField="NivelTrilha" HeaderText="Nível da Trilha" SortExpression="NivelTrilha" />
            <asp:BoundField DataField="QtdAcessoMonitorPeriodo" HeaderText="Quantidade de acessos do monitor" SortExpression="QtdAcessoMonitorPeriodo" />
            <asp:BoundField DataField="HoraDiaUltimoAcessoMonitor" HeaderText="Hora do último acesso do monitor" SortExpression="HoraDiaUltimoAcessoMonitor" />
            <asp:BoundField DataField="QtdParticipacaoSEAnalisadasMonitorPeriodo" HeaderText="Quantidade de participações em Soluções Educacionais analisadas pelo monitor" SortExpression="QtdParticipacaoSEAnalisadasMonitorPeriodo" />
            <asp:BoundField DataField="QtdParticipacoesSEPrazoVencido" HeaderText="Quantidade de participações em Soluções Educacionais com prazo de análise vencido" SortExpression="QtdParticipacoesSEPrazoVencido" />
            <asp:BoundField DataField="QtdParticipacoesSEPrazoVigente" HeaderText="Quantidade de participaçõesem Soluções Educacionais com prazo de análise vigente" SortExpression="QtdParticipacoesSEPrazoVigente" />
            <asp:BoundField DataField="QtdParticipacoesSEEmRevisao" HeaderText="Quantidade de participaçõesem Soluções Educacionais com status “Em revisão”" SortExpression="QtdParticipacoesSEEmRevisao" />
            <asp:BoundField DataField="QtdParticipacoesSEPendente" HeaderText="Quantidade de participaçõesem Soluções Educacionais com status “Pendentes”" SortExpression="QtdParticipacoesSEPendente" />
            <asp:BoundField DataField="QtdParticipacoesSEAprovadas" HeaderText="Quantidade de participaçõesem Soluções Educacionais com status “Aprovadas”" SortExpression="QtdParticipacoesSEAprovadas" />
            <asp:BoundField DataField="QtdParticipacaoSEAIAnalisadasMonitorPeriodo" HeaderText="Quantidade de participações em Soluções Educacionaisauto Indicativas (participação)analisadas pelo monitor" SortExpression="QtdParticipacaoSEAIAnalisadasMonitorPeriodo" />
            <asp:BoundField DataField="QtdParticipacoesSEAIPrazoVencido" HeaderText="Quantidade de participaçõesem Soluções Educacionais Auto indicativas (participação) com prazo de análise vencido" SortExpression="QtdParticipacoesSEAIPrazoVencido" />
            <asp:BoundField DataField="QtdParticipacoesSEAIPrazoVigente" HeaderText="Quantidade de participaçõesem Soluções Educacionais Auto indicativas (participação) com prazo de análise vigente" SortExpression="QtdParticipacoesSEAIPrazoVigente" />
            <asp:BoundField DataField="QtdParticipacoesSEAIEmRevisao" HeaderText="Quantidade de participações em Soluções Educacionais Auto indicativas (participação)com status “Em revisão”" SortExpression="QtdParticipacoesSEAIEmRevisao" />
            <asp:BoundField DataField="QtdParticipacoesSEAIPendente" HeaderText="Quantidade de participações em Soluções Educacionais Auto indicativas (participação)com status “Pendentes”" SortExpression="QtdParticipacoesSEAIPendente" />
            <asp:BoundField DataField="QtdParticipacoesSEAIAprovadas" HeaderText="Quantidade de participações em Soluções Educacionais Auto indicativas (participação)com status “Aprovadas”" SortExpression="QtdParticipacoesSEAIAprovadas" />
            <asp:BoundField DataField="QtdParticipacaoSprintAnalisadasMonitorPeriodo" HeaderText="Quantidade de participações em Sprint analisadas pelo monitor" SortExpression="QtdParticipacaoSprintAnalisadasMonitorPeriodo" />
            <asp:BoundField DataField="QtdParticipacoesSprintPrazoVencido" HeaderText="Quantidade de participações em Sprint com prazo de análise vencido" SortExpression="QtdParticipacoesSprintPrazoVencido" />
            <asp:BoundField DataField="QtdParticipacoesSprintPrazoVigente" HeaderText="Quantidade de participações em Sprint com prazo de análise vigente" SortExpression="QtdParticipacoesSprintPrazoVigente" />
            <asp:BoundField DataField="QtdParticipacoesSprintEmRevisao" HeaderText="Quantidade de participações em Sprint com status “Em revisão”" SortExpression="QtdParticipacoesSprintEmRevisao" />
            <asp:BoundField DataField="QtdParticipacoesSprintPendente" HeaderText="Quantidade de participações em Sprint com status “Pendentes”" SortExpression="QtdParticipacoesSprintPendente" />
            <asp:BoundField DataField="QtdParticipacoesSprintAprovadas" HeaderText="Quantidade de participações em Sprint com status “Aprovadas”" SortExpression="QtdParticipacoesSprintAprovadas" />
            <asp:BoundField DataField="TotalDeParticipacoesVinculadasMonitor" HeaderText="Total de participações vinculadas ao Monitor" SortExpression="TotalDeParticipacoesVinculadasMonitor" />
        </Columns>
        <EmptyDataTemplate>
            <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
        </EmptyDataTemplate>
        <PagerSettings Mode="NumericFirstLast" />
    </asp:GridView>
    <fieldset runat="server" id="componenteGeracaoRelatorio" clientidmode="Static" visible="false">
        <hr />
        <div class="form-group">
            <uc:SaidaRel ID="ucFormatoSaidaRelatorio" runat="server" />
        </div>
        <div class="form-group">
            <asp:Button ID="btnGerarRelatorio" runat="server" Text="Gerar Relatório" OnClick="btnGerarRelatorio_OnClick"
                CssClass="btn btn-primary" />
        </div>
    </fieldset>
    <script type="text/javascript">
        (function($) {
            $(document).ready(function () {
                $("#<%= txtDataInicio.ClientID %>").mask("99/99/9999");
                $("#<%= txtDataFinal.ClientID %>").mask("99/99/9999");
            });
        })(jQuery);
    </script>
</asp:Content>
