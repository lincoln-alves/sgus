<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="Boletim.aspx.cs" Inherits="Sebrae.Academico.WebForms.Relatorios.CertificadoConhecimento.Boletim" %>
<%@ Register Src="~/UserControls/ucSeletorListBox.ascx" TagName="ucSeletorListBox"TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-group" id="gruporelatorio">
        <div class="panel panel-default">
            <div class="panel-heading">
                <a data-toggle="collapse" data-parent="#gruporelatorio" href="#filtros">Filtros
                </a>
            </div>
            <asp:Panel ID="filtros" runat="server" ClientIDMode="Static" CssClass="panel-collapse collapse in">
                <div class="panel-body">
                    <fieldset>
                        <div class="form-group">
                            <asp:Label ID="Label1" runat="server" Text="Nome" AssociatedControlID="txtNome" />
                            <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label2" runat="server" Text="CPF" AssociatedControlID="txtCPF" />
                            <asp:TextBox ID="txtCPF" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label10" runat="server" Text="Inscrição" AssociatedControlID="txtInscricao" />
                            <asp:TextBox ID="txtInscricao" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group" id="dvUF" runat="server">
                            <asp:Label ID="Label9" runat="server" Text="UF" AssociatedControlID="ListBoxesUF" />
                            <uc2:ucSeletorListBox runat="server" ID="ListBoxesUF" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label3" runat="server" Text="Unidade" AssociatedControlID="txtUnidade" />
                            <asp:TextBox ID="txtUnidade" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label5" runat="server" Text="Ano de Certame" AssociatedControlID="txtAno" />
                            <asp:TextBox ID="txtAno" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label4" runat="server" Text="Área do boletim" AssociatedControlID="ddlTemaCertificado" />
                            <asp:DropDownList runat="server" ID="ddlTemaCertificado" CssClass="form-control"></asp:DropDownList>    
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label6" runat="server" Text="Status" AssociatedControlID="ddlStatus" />
                            <asp:DropDownList runat="server" ID="ddlStatus" CssClass="form-control">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem Value="1">Aprovado</asp:ListItem>
                                <asp:ListItem Value="0">Reprovado</asp:ListItem>
                            </asp:DropDownList>    
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label7" runat="server" Text="Boletim emitido" AssociatedControlID="ddlBoletimEmitido" />
                            <asp:DropDownList runat="server" ID="ddlBoletimEmitido" CssClass="form-control">
                                <asp:ListItem></asp:ListItem>
                                <asp:ListItem>Sim</asp:ListItem>
                                <asp:ListItem>Não</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label8" runat="server" Text="Data de download" AssociatedControlID="txtDataDownload" />
                            <asp:TextBox ID="txtDataDownload" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </fieldset>
                </div>
            </asp:Panel>
        </div>
    </div>

    <br />
    <asp:Button ID="btnPesquisar" runat="server" Text="Consultar" OnClick="btnPesquisar_OnClick"
        CssClass="btn btn-primary mostrarload" />
    <hr />

    <asp:GridView runat="server" AutoGenerateColumns="False" ID="dgvUsuariosCertame" CssClass="table table-bordered relatorios col-sm-12" AllowPaging="True" OnPageIndexChanging="dgvUsuariosCertame_OnPageIndexChanged" PageSize="10" >
        <Columns>
            <asp:BoundField DataField="Nome" HeaderText="Nome" SortExpression="Nome" />
            <asp:BoundField DataField="CPF" HeaderText="CPF" SortExpression="CPF" />
            <asp:BoundField DataField="NumeroInscricao" HeaderText="Inscrição" SortExpression="NumeroInscricao" />
            <asp:BoundField DataField="UF" HeaderText="UF" SortExpression="UF" />
            <asp:BoundField DataField="Unidade" HeaderText="Unidade" SortExpression="Unidade" />
            <asp:BoundField DataField="Ano" HeaderText="Ano de Certame" SortExpression="Ano" />
            <asp:BoundField DataField="TemaCertificacao" HeaderText="Área do boletim" SortExpression="TemaCertificacao" />
            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
            <asp:BoundField DataField="CertificadoEmitido" HeaderText="Boletim Emitido" SortExpression="CertificadoEmitido" />
            <asp:BoundField DataField="DataDownload" HeaderText="Data de Download" SortExpression="Data de Download" />
        </Columns>
        <EmptyDataTemplate>
            <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
        </EmptyDataTemplate>
    </asp:GridView>

    <fieldset runat="server" id="componenteGeracaoRelatorio" clientidmode="Static" visible="False">
        <hr />
        <div class="form-group">
            <uc:SaidaRel ID="ucFormatoSaidaRelatorio" runat="server" />
        </div>
        <div class="form-group">
            <asp:Button ID="btnGerarRelatorio" runat="server" Text="Gerar Relatório" OnClick="btnGerarRelatorio_OnClick"
                CssClass="btn btn-primary" />
        </div>
    </fieldset>
    <script>
        jQuery(function ($) {
            $("#<%= txtDataDownload.ClientID %>").mask("99/99/9999");
        });
    </script>
</asp:Content>
