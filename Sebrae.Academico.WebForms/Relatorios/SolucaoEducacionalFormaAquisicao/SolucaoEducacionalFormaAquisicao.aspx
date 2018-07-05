<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="SolucaoEducacionalFormaAquisicao.aspx.cs" Inherits="Sebrae.Academico.WebForms.Relatorios.SolucaoEducacionalFormaAquisicao.SolucaoEducacionalFormaAquisicao"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/UserControls/ucTotalizadorRelatorio.ascx" TagPrefix="uc" TagName="ucTotalizadorRelatorio" %>
<%@ Register Src="~/UserControls/ucCategorias.ascx" TagName="ucCategorias" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucSeletorListBox.ascx" TagName="ucSeletorListBox" TagPrefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                            <asp:Label ID="Label10" runat="server" Text="CATEGORIAS" AssociatedControlID="ucCategorias1" />
                            <uc1:ucCategorias ID="ucCategorias1" runat="server" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label14" runat="server" Text="Forma de aquisição" AssociatedControlID="ListBoxesFormaDeAquisicao" />
                            <uc1:ucSeletorListBox runat="server" ID="ListBoxesFormaDeAquisicao" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label6" runat="server" Text="UF Responsável" AssociatedControlID="ListBoxesUFResponsavel" />
                            <uc1:ucSeletorListBox runat="server" ID="ListBoxesUFResponsavel" />
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
                                <asp:ListItem Selected="True" Value="FormaAquisicao">Forma de Aquisição</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Solucoes">Soluções</asp:ListItem>
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
            <asp:BoundField DataField="FormaAquisicao" HeaderText="Forma de Aquisição" SortExpression="FormaAquisicao" />
            <asp:BoundField DataField="SolucoesFormatado" HeaderText="Soluções" SortExpression="Solucoes" />
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
</asp:Content>
