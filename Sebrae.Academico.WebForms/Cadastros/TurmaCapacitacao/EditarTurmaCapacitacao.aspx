<%@ Page Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="EditarTurmaCapacitacao.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.TurmaCapacitacao.EditarTurmaCapacitacao" %>

<%@ Register Src="~/UserControls/ucPermissoes.ascx" TagName="ucPermissoes" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-group" id="accordion">
        <div id="collapse1" class="panel-collapse collapse in">
            <div class="panel-body">
                <fieldset>
                    <div class="form-group">
                        <asp:Label ID="Label1" runat="server" Text="Programa" AssociatedControlID="ddlPrograma"></asp:Label>
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip1" Chave="ddlPrograma" />
                        <asp:DropDownList ID="ddlPrograma" runat="server" CssClass="form-control" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlPrograma_OnSelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label3" runat="server" Text="Oferta" AssociatedControlID="ddlCapacitacao"></asp:Label>
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip2" Chave="ddlCapacitacao" />
                        <asp:DropDownList ID="ddlCapacitacao" runat="server" CssClass="form-control" Enabled="false"></asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label2" runat="server" Text="Nome" AssociatedControlID="txtNome"></asp:Label>
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip3" Chave="txtNome" />
                        <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label4" runat="server" Text="Questionário pré" AssociatedControlID="ddlQuestionarioPre"></asp:Label>
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip4" Chave="ddlQuestionarioPre" />
                        <asp:DropDownList ID="ddlQuestionarioPre" runat="server" CssClass="form-control" />
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label5" runat="server" Text="Questionário pós" AssociatedControlID="ddlQuestionarioPos"></asp:Label>
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip5" Chave="ddlQuestionarioPos" />
                        <asp:DropDownList ID="ddlQuestionarioPos" runat="server" CssClass="form-control" />
                    </div>
                    <div class="form-group">
                        <asp:Label ID="LblDtInicio" runat="server" Text="Data Início*" AssociatedControlID="txtDtInicio" ></asp:Label>
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip6" Chave="turmaDataInicio" />
                        <asp:TextBox ID="TxtDtInicio" runat="server" MaxLength="19" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="LblDtFinal" runat="server" Text="Data Final" AssociatedControlID="txtDtFinal" ></asp:Label>
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip7" Chave="turmaDataFinal" />
                        <asp:TextBox ID="TxtDtFinal" runat="server" MaxLength="19" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <a data-toggle="collapse" data-parent="#accordionOF" href="#collapse111">Permissões
                            </a>
                        </div>
                        <div id="collapse111" class="panel-collapse collapse">
                            <div class="panel-body">
                                <uc2:ucPermissoes ID="ucPermissoes2" runat="server" />
                            </div>
                        </div>
                    </div>
                    <br />
                    <br />
                    <div>
                        <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
                            CssClass="btn btn-primary mostrarload" />
                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click"
                            CssClass="btn btn-default" />
                    </div>
                </fieldset>
                <script>
                    jQuery(function ($) {
                        $("#<%= TxtDtInicio.ClientID %>").mask("99/99/9999");
                        $("#<%= TxtDtFinal.ClientID %>").mask("99/99/9999");
                    });
                </script>
            </div>
        </div>
    </div>
</asp:Content>
