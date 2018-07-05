<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeBehind="EdicaoItemTrilhaParticipacao.aspx.cs"
    Inherits="Sebrae.Academico.WebForms.Cadastros.EdicaoItemTrilhaParticipacao" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h3>
        Edição de Item Trilha Participação</h3>
    <div>
        <fieldset>
            <asp:HiddenField ID="hdfIdUsuarioTrilha" runat="server" />
            <div class="form-group">
                <asp:Label ID="lblTrilha" runat="server" Text="Trilha" AssociatedControlID="ddlTrilha"></asp:Label>
                <asp:DropDownList ID="ddlTrilha" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTrilha_SelectedIndexChanged" CssClass="form-control">
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:Label ID="lblTrilhaNivel" runat="server" Text="Trilha Nível" AssociatedControlID="ddlTrilhaNivel"></asp:Label>
                <asp:DropDownList ID="ddlTrilhaNivel" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTrilhaNivel_SelectedIndexChanged" CssClass="form-control">
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:Label ID="lblTopicoTematico" runat="server" Text="Tópico Temático" AssociatedControlID="ddlTopicoTematico"></asp:Label>
                <asp:DropDownList ID="ddlTopicoTematico" runat="server" CssClass="form-control">
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:Label ID="lblNomeAluno" runat="server" Text="Nome Aluno" AssociatedControlID="ddlNomeAluno"></asp:Label>
                <asp:DropDownList ID="ddlNomeAluno" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlNomeAluno_SelectedIndexChanged" CssClass="form-control">
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:Label ID="lblItemTrilha" runat="server" Text="Item Trilha" AssociatedControlID="ddlItemTrilha"></asp:Label>
                <asp:DropDownList ID="ddlItemTrilha" runat="server" CssClass="form-control">
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:Label ID="lblTextoParticipacao" runat="server" Text="Texto Participação*" AssociatedControlID="txtTextoParticipacao"></asp:Label>
                <asp:TextBox ID="txtTextoParticipacao" runat="server"  TextMode="multiline" Rows="5" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="Label1" runat="server" Text="Arquivo de Envio*"></asp:Label>
                <asp:FileUpload ID="fupldArquivoEnvio" runat="server" />
                <asp:LinkButton ID="lkbArquivo" runat="server" OnClick="lkbArquivo_Click" />
            </div>
            <div class="form-group">
                <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click" CssClass="btn btn-default"/>
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" CssClass="btn btn-default"/>
            </div>
        </fieldset>
    </div>
</asp:Content>
