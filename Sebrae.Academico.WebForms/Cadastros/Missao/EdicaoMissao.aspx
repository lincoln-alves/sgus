<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="EdicaoMissao.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.Missao.EdicaoMissao" %>

<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <div class="form-group">
            <asp:Label ID="Label1" runat="server" Text="Trilha" AssociatedControlID="ddlTrilha" />
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip1" Chave="ddlTrilha" />
            <asp:DropDownList ID="ddlTrilha" CssClass="form-control mostrarload" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTrilha_OnSelectedIndexChanged"></asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Label ID="Label3" runat="server" Text="Nível" AssociatedControlID="ddlTrilhaNivel" />
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip2" Chave="ddlTrilhaNivel" />
            <asp:DropDownList ID="ddlTrilhaNivel" CssClass="form-control mostrarload" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTrilhaNivel_OnSelectedIndexChanged"></asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Label ID="Label4" runat="server" Text="Ponto Sebrae" AssociatedControlID="ddlPontoSebrae" />
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip3" Chave="ddlPontoSebrae" />
            <asp:DropDownList ID="ddlPontoSebrae" CssClass="form-control" runat="server"></asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Label ID="Label2" runat="server" Text="Nome" AssociatedControlID="txtMissao" />
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip4" Chave="txtMissao" />
            <asp:TextBox ID="txtMissao" runat="server" CssClass="form-control" MaxLength="450"></asp:TextBox>
        </div>
        <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
            CssClass="btn btn-primary mostrarload" />
        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click"
            CssClass="btn btn-default" />
    </fieldset>

    <script>
        validarForm();
    </script>
</asp:Content>
