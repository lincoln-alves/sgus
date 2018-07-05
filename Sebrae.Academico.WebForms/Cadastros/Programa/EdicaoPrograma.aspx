<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="EdicaoPrograma.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.EdicaoPrograma"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/UserControls/ucSolucaoEducacional.ascx" TagName="ucSolucaoEducacional" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/ucSeletorListBox.ascx" TagName="ucSeletorListBox" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-group" id="accordion">
        <div class="panel panel-default">
            <div class="panel-heading">
                <a data-toggle="collapse" data-parent="#accordion" href="#collapse1">Dados </a>
            </div>
            <div id="collapse1" class="panel-collapse collapse in">
                <div class="panel-body">
                    <fieldset>
                        <div class="form-group">
                            <asp:Label ID="lblTopicoTematico" runat="server" Text="Nome" AssociatedControlID="txtNome"></asp:Label>
                            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip6" Chave="nome" />
                            <asp:TextBox ID="txtNome" runat="server" MaxLength="100" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label1" runat="server" Text="Ativo" AssociatedControlID="rblAtivo"></asp:Label>
                            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip1" Chave="ativo" />
                            <asp:RadioButtonList ID="rblAtivo" runat="server" RepeatDirection="Horizontal" CssClass="form-control">
                            </asp:RadioButtonList>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label12" runat="server" Text="Texto de Apresentação Portal" AssociatedControlID="txtTextoApresentacao" />
                            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip2" Chave="apresentacao" />
                            <CKEditor:CKEditorControl ID="txtTextoApresentacao" BasePath="/js/ckeditor/" runat="server"></CKEditor:CKEditorControl>
                        </div>
                        <div class="form-group" id="divAreaTematica" runat="server">
                            <asp:Label ID="lblAreaTematica" runat="server" AssociatedControlID="lblAreaTematica" Text="Área Temática"></asp:Label>
                            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip3" Chave="areaTematica" />
                            <uc2:ucSeletorListBox runat="server" ID="listBoxesAreaTematica" DescricaoDisponiveis="Lista de Áreas Temáticas" DescricaoSelecionados="Selecionados" />
                        </div>
                    </fieldset>
                </div>
            </div>
        </div>
        <div class="panel panel-default">
            <div class="panel-heading">
                <a data-toggle="collapse" data-parent="#accordion" href="#collapse1a">Soluções Educacionais
                </a>
            </div>
            <div id="collapse1a" class="panel-collapse collapse">
                <div class="panel-body">
                    <uc:ucSolucaoEducacional ID="ucSolucaoEducacional1" runat="server" Chave="solucao" />
                </div>
            </div>
        </div>
        <div class="panel panel-default" runat="server" Visible="False">
            <div class="panel-heading">
                <a data-toggle="collapse" data-parent="#accordion" href="#collapse2">Tags </a>
            </div>
            <div id="collapse2" class="panel-collapse collapse">
                <div class="panel-body">
                    <uc:Tags ID="ucTags1" runat="server" />
                </div>
            </div>
        </div>
        <div class="panel panel-default">
            <div class="panel-heading">
                <a data-toggle="collapse" data-parent="#accordion" href="#collapse3">Permissões
                </a>
            </div>
            <div id="collapse3" class="panel-collapse collapse">
                <div class="panel-body">
                    <uc:Permissoes ID="ucPermissoes1" runat="server" />
                </div>
            </div>
        </div>
    </div>
    <div>
        <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
            CssClass="btn btn-primary" />
        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click"
            CssClass="btn btn-default" />
    </div>
</asp:Content>
