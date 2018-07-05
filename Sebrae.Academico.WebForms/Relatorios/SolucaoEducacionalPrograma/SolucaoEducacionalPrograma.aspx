<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="SolucaoEducacionalPrograma.aspx.cs" Inherits="Sebrae.Academico.WebForms.Relatorios.SolucaoEducacionalPrograma.SolucaoEducacionalPrograma"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/UserControls/ucTotalizadorRelatorio.ascx" TagPrefix="uc" TagName="ucTotalizadorRelatorio" %>
<%@ Register Src="~/UserControls/ucSeletorListBox.ascx" TagName="ucSeletorListBox" TagPrefix="uc2" %>

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
                            <asp:Label ID="lblPrograma" runat="server" Text="Programa" AssociatedControlID="cbxPrograma" />
                            <asp:DropDownList ID="cbxPrograma" runat="server"  CssClass="form-control"/>
                        </div>
                        <asp:CheckBoxList ID="chkListaCamposVisiveis" runat="server" CssClass="checkbox-inline"
                            ClientIDMode="Static" RepeatLayout="Flow">
                            <asp:ListItem Selected="True" Value="SolucaoEducacional">Solução Educacional</asp:ListItem>
                            <asp:ListItem Selected="True" Value="TemOferta">Tem Oferta?</asp:ListItem>
                        </asp:CheckBoxList>
                        <div class="form-group" id="divUfResposanvel" runat="server">
                            <asp:Label ID="lblUfResposanvel" runat="server" Text="UF responsável" AssociatedControlID="ListBoxesUFResponsavel" />
                            <uc2:ucSeletorListBox runat="server" ID="ListBoxesUFResponsavel" />
                        </div>
                    </fieldset>
                </div>
            </asp:Panel>
         </div>
    </div>

    <asp:Button ID="btnPesquisar" runat="server" CssClass="btn btn-primary mostrarload"
        Text="Consultar" OnClick="btnPesquisar_Click" />
    <hr />
    
    <uc:ucTotalizadorRelatorio runat="server" ID="ucTotalizadorRelatorio" />

    <asp:GridView ID="dgRelatorio" runat="server" AutoGenerateColumns="False" AllowSorting="True"
        CssClass="table col-sm-12" GridLines="None" OnSorting="dgRelatorio_Sorting" AllowPaging="True"
        OnPageIndexChanging="dgRelatorio_PageIndexChanging" PageSize="100">
        <Columns>
            <asp:BoundField DataField="Programa" HeaderText="Programa" SortExpression="Programa" />
            <asp:BoundField DataField="SolucaoEducacional" HeaderText="Solução Educacional" SortExpression="SolucaoEducacional" />
            <asp:BoundField DataField="TemOferta" HeaderText="Tem Oferta?" SortExpression="TemOferta" />
        </Columns>
        <EmptyDataTemplate>
            <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
        </EmptyDataTemplate>
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
