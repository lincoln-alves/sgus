<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="EdicaoCapacitacao.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.Capacitacao.EdicaoCapacitacao" %>
<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-group" id="accordion">
        <div id="collapse1" class="panel-collapse collapse in">
            <div class="panel-body">
                <fieldset>
                    <div class="form-group">
                        <asp:Label ID="Label1" runat="server" Text="Programa" AssociatedControlID="ddlPrograma"></asp:Label>
                        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip6" Chave="programa" />
                        <asp:DropDownList ID="ddlPrograma" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label2" runat="server" Text="Nome" AssociatedControlID="txtNome"></asp:Label>
                        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip1" Chave="nome" />
                        <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label9" runat="server" Text="Data de Início da Realização*" AssociatedControlID="txtDtInicio" />
                        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip2" Chave="dataInicio" />
                        <asp:TextBox ID="txtDtInicio" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label10" runat="server" Text="Data fim da Realização" AssociatedControlID="txtDtFim" />
                        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip3" Chave="dataFim" />
                        <asp:TextBox ID="txtDtFim" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label4" runat="server" Text="Data de Início das Inscrições*" AssociatedControlID="txtDtInicioInscricao" />
                        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip4" Chave="inicioInscricoes" />
                        <asp:TextBox ID="txtDtInicioInscricao" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label6" runat="server" Text="Data fim das Inscrições*" AssociatedControlID="txtDtFimInscricao" />
                        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip5" Chave="fimInscricoes" />
                        <asp:TextBox ID="txtDtFimInscricao" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label8" runat="server" Text="Permitir cancelamento de matrícula pelo aluno" AssociatedControlID="rblPermitirCancelamentoMatricula" />
                        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip7" Chave="permitirCancelamentoMatricula" />
                        <asp:RadioButtonList ID="rblPermitirCancelamentoMatricula" runat="server" RepeatDirection="Horizontal"
                            CssClass="form-control" />
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label7" runat="server" Text="Permitir Alteração de Situação da Matrícula" AssociatedControlID="rblAlterarSituacao" />
                        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip8" Chave="permitirCancelamentoMatricula" />
                        <asp:RadioButtonList ID="rblAlterarSituacao" runat="server" RepeatDirection="Horizontal"
                            CssClass="form-control" />
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label11" runat="server" Text="Permitir Inscrições Pelo Gestor" AssociatedControlID="rblPermiteMatriculaPeloGestor" />
                        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip9" Chave="permitirMatriculaGestor" />
                        <asp:RadioButtonList ID="rblPermiteMatriculaPeloGestor" runat="server" RepeatDirection="Horizontal"
                            CssClass="form-control" />
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label3" runat="server" Text="Descrição" AssociatedControlID="txtDescricao"></asp:Label>
                        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip10" Chave="descricao" />
                        <CKEditor:CKEditorControl ID="txtDescricao" BasePath="/js/ckeditor/" runat="server"></CKEditor:CKEditorControl>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label5" runat="server" Text="Certificado" AssociatedControlID="ddlCertificado" />
                        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip11" Chave="certificado" />
                        <asp:DropDownList ID="ddlCertificado" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <br />
                    <div>
                        <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
                            CssClass="btn btn-primary mostrarload" />
                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click"
                            CssClass="btn btn-default" />
                    </div>
                </fieldset>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        jQuery(function ($) {
            $("#<%= txtDtInicio.ClientID %>").mask("99/99/9999");
            $("#<%= txtDtFim.ClientID %>").mask("99/99/9999");
            $("#<%= txtDtInicioInscricao.ClientID %>").mask("99/99/9999");
            $("#<%= txtDtFimInscricao.ClientID %>").mask("99/99/9999");
        });
    </script>
</asp:Content>
