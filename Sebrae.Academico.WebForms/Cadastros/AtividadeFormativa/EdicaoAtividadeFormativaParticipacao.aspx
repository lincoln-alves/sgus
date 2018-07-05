<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeBehind="EdicaoAtividadeFormativaParticipacao.aspx.cs"
    Inherits="Sebrae.Academico.WebForms.Cadastros.AtividadeFormativa.EdicaoAtividadeFormativaParticipacao" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdfIdUsuarioTrilha" runat="server" />
    <h3>Edição de Participação em Atividade Formativa</h3>
    <fieldset>
        <div class="form-group">
            <asp:Label ID="Label1" runat="server" Text="Trilha" AssociatedControlID="ddlTrilha"></asp:Label>
            <asp:DropDownList ID="ddlTrilha" runat="server" CssClass="form-control"></asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Label ID="Label9" runat="server" Text="Trilha Nível" AssociatedControlID="ddlTrilhaNivel"></asp:Label>
            <asp:DropDownList ID="ddlTrilhaNivel" runat="server" CssClass="form-control"></asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Label ID="Label3" runat="server" Text="Tópico Temático" AssociatedControlID="ddlTrilhaNivel" ></asp:Label>
            <asp:DropDownList ID="ddlTopicoTematico" runat="server" CssClass="form-control"></asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Label ID="Label2" runat="server" Text="Nome Aluno" AssociatedControlID="ddlNomeAluno"></asp:Label>
            <asp:DropDownList ID="ddlNomeAluno" runat="server" CssClass="form-control"></asp:DropDownList>
        </div>
        <!-- NAO COLOCAR CKEDITOR -->
        <div id="trTextoParticipacao" runat="server" visible="false" class="form-group">
            <asp:Label ID="lblTextoParticipacao" runat="server" AssociatedControlID="txtTextoParticipacao"></asp:Label>
            <asp:TextBox ID="txtTextoParticipacao" TextMode="MultiLine" runat="server" Rows="5" CssClass="form-control"></asp:TextBox>
        </div>
        <div id="trArquivoEnvio" runat="server" visible="false" class="form-group">
            <asp:Label ID="lblArquivoEnvioDe" runat="server" AssociatedControlID="fupldArquivoEnvio"></asp:Label>
            <asp:FileUpload ID="fupldArquivoEnvio" runat="server" CssClass="form-control" />
        </div>
        <div class="form-group">
            <asp:LinkButton ID="lkbArquivo" runat="server" OnClick="lkbArquivo_Click" />
        </div>
        <div class="form-group">
            <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click" CssClass="btn btn-default" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" CssClass="btn btn-default" />
        </div>
    </fieldset>
</asp:Content>
