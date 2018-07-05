<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="EdicaoProtocolo.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.Protocolo.EdicaoProtocolo" %>
<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-group">
        <div class="panel panel-default">
            <div class="panel-heading">
                <a data-toggle="collapse" data-parent="#accordion" href="#collapseDados">Dados </a>
            </div>
            <div class="panel-body">
                <fieldset>
                    <div class="form-group">
                        <asp:Label Text="Número" runat="server" AssociatedControlID="txtNumero"  />
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip1" Chave="txtNumero" />
                        <asp:TextBox Text="" runat="server" ID="txtNumero" ReadOnly="true" CssClass="form-control" />
                    </div>

                    <div class="form-group">
                        <asp:Label Text="Remetente" runat="server" AssociatedControlID="txtRemetente" />
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip2" Chave="txtRemetente" />
                        <asp:TextBox Text="" runat="server" ID="txtRemetente" ReadOnly="true" CssClass="form-control" />
                    </div>

                    <div class="form-group">
                        <asp:Label Text="Destinatário" runat="server" AssociatedControlID="txtDestinatario" />
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip3" Chave="txtDestinatario" />
                        <asp:TextBox Text="" runat="server" ID="txtDestinatario" ReadOnly="true" CssClass="form-control" />
                    </div>

                    <div class="form-group">
                        <asp:Label Text="Data de Envio" runat="server" AssociatedControlID="txtDataDeEnvio" />
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip4" Chave="txtDataDeEnvio" />
                        <asp:TextBox Text="" runat="server" ID="txtDataDeEnvio" ReadOnly="true" CssClass="form-control" />
                    </div>

                    <div class="form-group">
                        <asp:Label Text="Data de Recebimento" runat="server" AssociatedControlID="txtDataDeRecebimento" />
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip5" Chave="txtDataDeRecebimento" />
                        <asp:TextBox Text="" runat="server" ID="txtDataDeRecebimento" ReadOnly="true" CssClass="form-control" />
                    </div>

                    <div class="form-group">
                        <asp:Label Text="Assinado por" runat="server" AssociatedControlID="txtAssinadoPor" />
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip6" Chave="txtAssinadoPor" />
                        <asp:TextBox Text="" runat="server" ID="txtAssinadoPor" ReadOnly="true" CssClass="form-control" />
                    </div>

                    <asp:Button Text="Voltar" ID="btnVoltar" CssClass="btn btn-default" runat="server" OnClick="btnVoltar_Click" />
                </fieldset>
            </div>
        </div>
    </div>
</asp:Content>