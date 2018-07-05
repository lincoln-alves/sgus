<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="SEOfertaMatricula.aspx.cs" Inherits="Sebrae.Academico.WebForms.Relatorios.SEOfertaMatricula.SEOfertaMatricula"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/UserControls/ucTotalizadorRelatorio.ascx" TagPrefix="uc" TagName="ucTotalizadorRelatorio" %>
<%@ Register Src="~/UserControls/ucSeletorListBox.ascx" TagName="ucSeletorListBox" TagPrefix="uc2" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var _preloadedList = <%= ViewState["_SE"] ?? "''" %>;
        AutoCompleteDefine(_preloadedList, '#txtSolucaoEducacional', false, true, false);
    </script>
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
                            <asp:Label ID="lblFormaAquisicao" runat="server" AssociatedControlID="cbxFormaAquisicao"
                                Text="Forma de Aquisição:" />
                            <asp:DropDownList ID="cbxFormaAquisicao" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbxFormaAquisicao_SelectedIndexChanged"
                                CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label1" runat="server" AssociatedControlID="txtSolucaoEducacional"
                                Text="Solução Educacional" />
                            <asp:TextBox ID="txtSolucaoEducacional" runat="server"  ClientIDMode="Static" OnValueChanged="txtSolucaoEducacional_OnValueChanged" data-mensagemVazia="Selecione uma forma de aquisição"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label2" runat="server" AssociatedControlID="cbxTipoOferta" Text="Tipo de Oferta:" />
                            <asp:DropDownList ID="cbxTipoOferta" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label3" runat="server" Text="Data Início de Inscrição" AssociatedControlID="txtDataInicio" />
                            <asp:TextBox ID="txtDataInicio" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label4" runat="server" Text="Data Fim de Inscrição" AssociatedControlID="txtDataFim" />
                            <asp:TextBox ID="txtDataFim" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label6" runat="server" Text="UF Responsável" AssociatedControlID="ListBoxesUFResponsavel" />
                            <uc2:ucSeletorListBox runat="server" ID="ListBoxesUFResponsavel" />
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
                                <asp:ListItem Selected="True" Value="SolucaoEducacional">Solução Educacional</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Oferta">Oferta</asp:ListItem>
                                <asp:ListItem Selected="True" Value="TipoOferta">Tipo de Oferta</asp:ListItem>
                                <asp:ListItem Selected="True" Value="DataInicioInscricoes">Data Inicio de Inscrições</asp:ListItem>
                                <asp:ListItem Selected="True" Value="DataFimInscricoes">Data Fim de Inscrições</asp:ListItem>
                                <asp:ListItem Selected="True" Value="QtdMaxInscricoes">Qtd. Máxima de Inscrições</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Inscritos">Inscritos</asp:ListItem>
                                <asp:ListItem Selected="True" Value="FilaEspera">Fila de Espera</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Solicitado">Solicitado</asp:ListItem>
                                <asp:ListItem Selected="True" Value="UFResponsavel">UF Responsável</asp:ListItem>
                            </asp:CheckBoxList>
                        </div>
                    </fieldset>
                </div>
            </div>
        </div>
    </div>
    <br />
    <asp:Button ID="btnPesquisar" runat="server" CssClass="btn btn-primary mostrarload"
        Text="Consultar" OnClick="btnPesquisar_Click" />
    <hr />
    
    <uc:ucTotalizadorRelatorio runat="server" ID="ucTotalizadorRelatorio" />

    <asp:GridView ID="dgRelatorio" runat="server" AutoGenerateColumns="False" AllowSorting="True"
        CssClass="table col-sm-12" GridLines="None" OnSorting="dgRelatorio_Sorting" AllowPaging="True"
        OnPageIndexChanging="dgRelatorio_PageIndexChanging" PageSize="100">
        <Columns>
            <asp:BoundField DataField="SolucaoEducacional" HeaderText="Solução Educacional" SortExpression="SolucaoEducacional" />
            <asp:BoundField DataField="Oferta" HeaderText="Oferta" SortExpression="Oferta" />
            <asp:BoundField DataField="TipoOferta" HeaderText="Tipo de Oferta" SortExpression="TipoOferta" />
            <asp:BoundField DataField="DataInicioInscricoes" HeaderText="Data Inicio das Inscricoes"
                SortExpression="DataInicioInscricoes" DataFormatString="{0:dd/MM/yyyy}" />
            <asp:BoundField DataField="DataFimInscricoes" HeaderText="Data Fim das Inscricoes"
                SortExpression="DataFimInscricoes" DataFormatString="{0:dd/MM/yyyy}" />
            <asp:BoundField DataField="QtdMaxInscricoes" HeaderText="Qtd. Máx. de Inscrições"
                SortExpression="QtdMaxInscricoes" />
            <asp:BoundField DataField="Inscritos" HeaderText="Inscritos" SortExpression="Inscritos" />
            <asp:BoundField DataField="FilaEspera" HeaderText="Fila de Espera" SortExpression="FilaEspera" />
            <asp:BoundField DataField="Solicitado" HeaderText="Solicitado" SortExpression="Solicitado" />
            <asp:BoundField DataField="UFResponsavel" HeaderText="UF Responsável" SortExpression="UFResponsavel" />
        </Columns>
        <EmptyDataTemplate>
            <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
        </EmptyDataTemplate>
        <PagerSettings Mode="NumericFirstLast" />
    </asp:GridView>
    <fieldset runat="server" id="componenteGeracaoRelatorio" clientidmode="Static" visible="false">
        <hr/>
        <div class="form-group">
            <uc:SaidaRel ID="ucFormatoSaidaRelatorio" runat="server" />
        </div>
        <div class="form-group">
            <asp:Button ID="btnGerarRelatorio" runat="server" Text="Gerar Relatório" OnClick="btnGerarRelatorio_Click"
                CssClass="btn btn-primary" />
        </div>
    </fieldset>
    <script type="text/javascript">
        jQuery(function ($) {
            $("#<%= txtDataInicio.ClientID %>").mask("99/99/9999");
            $("#<%= txtDataFim.ClientID %>").mask("99/99/9999");
        });
    </script>
</asp:Content>
