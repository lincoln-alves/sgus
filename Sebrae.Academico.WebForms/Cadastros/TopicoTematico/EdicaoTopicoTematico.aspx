<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EdicaoTopicoTematico.aspx.cs"
    MasterPageFile="~/Pagina.master" Inherits="Sebrae.Academico.WebForms.Cadastros.TopicoTematico.EdicaoTopicoTematico" %>
<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <fieldset>
            <div class="form-group">
                <asp:Label ID="Label2" runat="server" Text="Nome" AssociatedControlID="txtNome"></asp:Label>
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip1" Chave="nome" />
                <asp:TextBox ID="txtNome" runat="server" CssClass="form-control" MaxLength="250"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="Label1" runat="server" Text="Nome de Exibição*" AssociatedControlID="txtNomeExibicao"></asp:Label>
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip2" Chave="nomeExibicao" />
                <asp:TextBox ID="txtNomeExibicao" runat="server" CssClass="form-control" MaxLength="250"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="Label3" runat="server" Text="Descrição do Texto de Envio" AssociatedControlID="txtDescTextoEnvio"></asp:Label>
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip3" Chave="topicoTextoEnvio" />
                <asp:TextBox ID="txtDescTextoEnvio" runat="server" CssClass="form-control" MaxLength="2000"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="Label4" runat="server" Text="Arquivo de Envio" AssociatedControlID="txtArqEnvio"></asp:Label>
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip4" Chave="topicoArquivoEnvio" />
                <asp:TextBox ID="txtArqEnvio" runat="server" CssClass="form-control" MaxLength="2000"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="Label5" runat="server" Text="Quantidade Mínima*" AssociatedControlID="txtQtdMinima"></asp:Label>
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip5" Chave="topicoQuantidadeMinima" />
                <asp:TextBox ID="txtQtdMinima" runat="server" CssClass="form-control" MaxLength="2"
                    onkeypress="return EhNumerico(event)"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="Label6" runat="server" Text="Arquivo da Atividade" AssociatedControlID="fupldArquivoEnvio"></asp:Label>
                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip6" Chave="topicoArquivo" />
                <asp:FileUpload ID="fupldArquivoEnvio" runat="server" CssClass="form-control" />
                <br />
                <asp:LinkButton ID="lkbArquivo" runat="server" OnClick="lkbArquivo_Click" Visible="false"><span class="glyphicon glyphicon-download-alt"> </span> Baixar Arquivo</asp:LinkButton>
            </div>
        </fieldset>
        <div class="form-group">
            <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
                CssClass="btn btn-primary mostrarload" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click"
                CssClass="btn btn-default mostrarload" />
        </div>
    </div>

    <script type="text/javascript">
        validarForm();
    </script>
</asp:Content>
