<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="EdicaoMensagemGuia.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.MensagemGuia.EdicaoMensagemGuia" %>

<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>
<%@ Register Src="~/UserControls/ucSeletorListBox.ascx" TagPrefix="uc1" TagName="ucSeletorListBox" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <div class="form-group">
            <asp:Literal ID="ltrMomento" runat="server"></asp:Literal>
        </div>
        <div class="form-group">
            <asp:Label ID="Label1" runat="server" Text="Tipo" AssociatedControlID="ddlTipo"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip2" Chave="ddlTipo" />
            <asp:DropDownList ID="ddlTipo" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlTipo_OnSelectedIndexChanged" AutoPostBack="True">
                <asp:ListItem Text="-- Selecione --" Value="-1"/>
                <asp:ListItem Text="Personalizada" Value="0"/>
                <asp:ListItem Text="Tutorial" Value="1"/>
            </asp:DropDownList>
        </div>
        <div class="form-group" id="divTexto" runat="server" Visible="False">
            <asp:Label ID="Label2" runat="server" Text="Texto" AssociatedControlID="txtTexto"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip1" Chave="txtTexto" />
            <CKEditor:CKEditorControl ID="txtTexto" BasePath="/js/ckeditor/" runat="server"></CKEditor:CKEditorControl>
            
            <div ID="divHashTag" runat="server">
                <script type='text/javascript'>
                var Suggestions = {};

                Suggestions[0] = <%=HashTagsJson%>;
                
                CKEDITOR.on('instanceReady', function(evt) {
                    CKEDITOR.instances.<%=txtTexto.ClientID%>.execCommand('reloadSuggestionBox', 0 );
                });
            </script>
            </div>
        </div>
        <div class="form-group" id="divTutorial" runat="server" Visible="False">
            <asp:Label ID="lblTutorial" runat="server" Text="Tutorial de trilha" AssociatedControlID="uclistTutorial"></asp:Label>
            <uc1:ucSeletorListBox ID="uclistTutorial" runat="server" />
        </div>
        <hr />
        <div class="form-group">
            <div class="row">
                <div class="col-xs-12 col-sm-6 col-md-3">
                    <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click" CssClass="btn btn-primary btn-block mostrarload" />
                </div>
                <div class="col-xs-12 col-sm-6 col-md-3">
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" CssClass="btn btn-default btn-block mostrarload" />
                </div>
            </div>
        </div>
    </fieldset>
</asp:Content>
