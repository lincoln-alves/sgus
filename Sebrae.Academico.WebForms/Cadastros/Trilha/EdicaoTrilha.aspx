<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="EdicaoTrilha.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.EdicaoTrilha"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/UserControls/ucPermissoes.ascx" TagName="ucPermissoes" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucColorPicker.ascx" TagName="ColorPicker" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/ucCategorias.ascx" TagName="ucCategorias" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucSeletorListBox.ascx" TagName="ucSeletorListBox" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="panel-body">
        <fieldset>
            <div class="form-group">
                <asp:Label ID="Label2" runat="server" Text="Nome" AssociatedControlID="txtNome" />
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip2" Chave="nomeReduzido" />
                <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="Label3" runat="server" Text="Descrição" AssociatedControlID="txtDescricao" />
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip3" Chave="descricao" />
                <asp:TextBox ID="txtDescricao" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="4"></asp:TextBox>
            </div>
            <div class="form-group" id="divAreaTematica" runat="server">
                <asp:Label ID="lblAreaTematica" runat="server" AssociatedControlID="lblAreaTematica" Text="Área Temática"></asp:Label>
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip5" Chave="listBoxesAreaTematica" />
                <uc2:ucSeletorListBox runat="server" ID="listBoxesAreaTematica" DescricaoDisponiveis="Lista de Áreas Temáticas" DescricaoSelecionados="Selecionados" />
            </div>
            <div class="form-group">
                <asp:Label ID="Label5" runat="server" Text="CATEGORIAS" AssociatedControlID="ucCategorias1" />
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip1" Chave="ucCategorias1" />
                <uc1:ucCategorias ID="ucCategorias1" runat="server" />
            </div>
            <asp:Panel ID="pnlNodeDrupal" runat="server" Visible="false">
                <div class="form-group">
                    <asp:Label ID="Label7" runat="server" Text="Referência Drupal Node" AssociatedControlID="txtIdNode" />
                    <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip9" Chave="idDrupal" />
                    <asp:TextBox ID="txtIdNode" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
            </asp:Panel>
            <div class="form-group">
                <asp:Label ID="Label23" runat="server" Text="E-mail da Guia" AssociatedControlID="txtEmailTutor" />
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip11" Chave="trilhasEmailTutor" />
                <asp:TextBox ID="txtEmailTutor" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="Label24" runat="server" Text="ACAMPAMENTO (CÓDIGO MOODLE)" AssociatedControlID="txtIDCodigoMoodle" />
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip12" Chave="trilhasIDCodigoMoodle" />
                <asp:TextBox ID="txtIDCodigoMoodle" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="Label27" runat="server" Text="CRÉDITO DA TRILHA" AssociatedControlID="txtCreditoTrilha" />
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip24" Chave="txtCreditoTrilha" />
                <asp:TextBox runat="server" ID="txtCreditoTrilha" TextMode="MultiLine" CssClass="form-control ckeditor" Rows="4" />
            </div>

            <div class="panel panel-default">
                <div class="panel-heading">
                    <a data-toggle="collapse" data-parent="#accordionTN" href="#collapse11">Permissões
                    </a>
                </div>
                <div id="collapse11" class="panel-collapse collapse">
                    <div class="panel-body">
                        <uc1:ucPermissoes runat="server" ID="ucPermissoes" />
                    </div>
                </div>
            </div>

        </fieldset>
        <br />
    </div>
    <div>
        <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
            CssClass="btn btn-primary mostrarload" />
        <asp:Button ID="btnCancelarTrilha" runat="server" Text="Cancelar" OnClientClick="return confirm('Deseja Realmente Cancelar o Cadastro desta Trilha ? Todos os dados não salvos serão perdidos');"
            OnClick="btnCancelarTrilha_Click" CssClass="btn btn-default" />
    </div>

    <script type="text/javascript">
        validarForm();
        // Adicionando limite de 200 caracteres para o campo descrição
        $("#<%= txtDescricao.ClientID %>").attr("maxlength", "250");
    </script>
</asp:Content>
