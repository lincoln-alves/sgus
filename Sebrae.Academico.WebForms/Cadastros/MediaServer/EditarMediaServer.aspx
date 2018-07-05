<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="EditarMediaServer.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="Sebrae.Academico.WebForms.Cadastros.MediaServer.EditarMediaServer" %>

<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <div class="form-group">
            <asp:Label ID="Label2" runat="server" Text="Arquivo" AssociatedControlID="fupldArquivoEnvio"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip1" Chave="arquivo" />
            <asp:FileUpload ID="fupldArquivoEnvio" runat="server" CssClass="form-control input-arquivo" />
            <asp:Image ID="imgFile" Style="max-width: 200px" runat="server" />
        </div>
        <div class="form-group" ID="dvNomeArquivo" runat="server" Visible="false">
            <asp:Label ID="Label4" runat="server" Text="Nome do Arquivo"  AssociatedControlID="txtNomeDoArquivoOriginal"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip4" Chave="txtNomeDoArquivoOriginal" />
            <asp:TextBox runat="server" ID="txtNomeDoArquivoOriginal" CssClass="form-control nome-arquivo"></asp:TextBox>
        </div>
        <div class="form-group" ID="dvUrlCompleta" runat="server" Visible="false">
            <asp:Label ID="Label1" runat="server" Text="Url Completa" AssociatedControlID="txtUrlCompleta"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip2" Chave="txtUrlCompleta" />
            <asp:TextBox runat="server" ID="txtUrlCompleta" ReadOnly="true" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group" ID="dvUrReduzida" runat="server" Visible="false">
            <asp:Label ID="Label3" runat="server" Text="Url Reduzida" AssociatedControlID="txtUrlReduzida"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip3" Chave="txtUrlReduzida" />
            <asp:TextBox runat="server" ID="txtUrlReduzida" ReadOnly="true" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click" CssClass="btn btn-primary" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" CssClass="btn btn-default" />
        </div>

        <script type="text/javascript">
            $().ready(function () {                
                var nomeAtual = $('.nome-arquivo').val();

                $('.input-arquivo').change(function (e) {
                    if ($(this).val() != '') {                        
                        $('.nome-arquivo').val(e.target.files[0].name);
                    } else {
                        $('.nome-arquivo').val(nomeAtual);
                    }
                    
                });
            });
        </script>

    </fieldset>
</asp:Content>
