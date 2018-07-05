<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EdicaoFormaAquisicao.aspx.cs" MaintainScrollPositionOnPostback="true"
    MasterPageFile="~/Pagina.master" Inherits="Sebrae.Academico.WebForms.Cadastros.FormaAquisicao.EdicaoFormaAquisicao" %>

<%@ Register Src="~/UserControls/ucUpload.ascx" TagName="ucUpload" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <fieldset>
            <div class="form-group">
                <asp:Label ID="lblNome" runat="server" Text="Nome" AssociatedControlID="txtNome" />
                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip1" Chave="txtNome" />
                <asp:TextBox ID="txtNome" runat="server" CssClass="form-control" MaxLength="250"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="Label1" runat="server" Text="Descrição" AssociatedControlID="txtDescricao" />
                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip2" Chave="txtDescricao" />
                <asp:TextBox ID="txtDescricao" runat="server" CssClass="form-control" MaxLength="1024" TextMode="MultiLine"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="lblArquivo" runat="server" Text="Imagem*" AssociatedControlID="ucUpload1" />
                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip3" Chave="ucUpload1" />
                <uc1:ucUpload ID="ucUpload1" runat="server" CssClass="form-control" />
            </div>
            <div class="form-group">
                <asp:CheckBox ID="cbEnviarPortal" Text="&nbsp;Enviar para Portal UC Sebrae?" runat="server" />
                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip6" Chave="cbEnviarPortal" />
            </div>
            <div class="form-group">
                <asp:CheckBox ID="cbPresencial" Text="&nbsp;Curso presencial?" runat="server" />
                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip7" Chave="cbPresencial" />
            </div>
            <div class="form-group">
                <asp:Label Text="Tipo" runat="server" AssociatedControlID="ddlTipoFormaAquisicao" />
                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip4" Chave="ddlTipoFormaAquisicao" />
                <asp:DropDownList ID="ddlTipoFormaAquisicao" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoFormaAquisicao_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
            <div class="form-group" runat="server" id="divCargaHoraria" visible="false">
                <asp:Label ID="Label14" runat="server" Text="Carga Horária*" AssociatedControlID="txtCargaHoraria" />
                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip5" Chave="cargaHoraria" />
                <asp:TextBox ID="txtCargaHoraria" runat="server" CssClass="form-control" MaxLength="4" onkeypress="return EhNumerico(event)"></asp:TextBox>
            </div>
            <div class="form-group" runat="server" id="divPermiteAlterarCargaHoraria" visible="false">
                <asp:CheckBox ID="cbPermiteAlterarCargaHoraria" Text="&nbsp;Permite Alterar Carga Horária" runat="server" />
            </div>
            <div class="form-group">
                <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click" CssClass="btn btn-primary" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" CssClass="btn btn-default" />
            </div>
        </fieldset>
</asp:Content>
