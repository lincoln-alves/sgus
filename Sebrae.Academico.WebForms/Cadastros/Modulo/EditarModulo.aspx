<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="EditarModulo.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.Modulo.EditarModulo" %>

<%@ Register Src="~/UserControls/ucSolucaoEducacional.ascx" TagName="ucSolucaoEducacional" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-group" id="accordion">
        <div id="collapse1" class="panel-collapse collapse in">
            <div class="panel-body">
                <fieldset>
                    <div class="form-group">
                        <asp:Label ID="Label1" runat="server" Text="Programa" AssociatedControlID="ddlPrograma"></asp:Label>
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip1" Chave="ddlPrograma" />
                        <asp:DropDownList ID="ddlPrograma" AutoPostBack="true" OnSelectedIndexChanged="ddlPrograma_OnSelectedIndexChanged" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label4" runat="server" Text="Oferta" AssociatedControlID="ddlCapacitacao"></asp:Label>
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip2" Chave="ddlCapacitacao" />
                        <asp:DropDownList ID="ddlCapacitacao" AutoPostBack="true" OnSelectedIndexChanged="ddlCapacitacao_OnSelectedIndexChanged" runat="server" CssClass="form-control" Enabled="false"></asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label2" runat="server" Text="Nome" AssociatedControlID="txtNome"></asp:Label>
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip3" Chave="txtNome" />
                        <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label9" runat="server" Text="Data de Início da Realização*" AssociatedControlID="txtDtInicio" />
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip4" Chave="dataInicio" />
                        <asp:TextBox ID="txtDtInicio" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label10" runat="server" Text="Data fim da Realização" AssociatedControlID="txtDtFim" />
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip5" Chave="dataFim" />
                        <asp:TextBox ID="txtDtFim" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label3" runat="server" Text="Descrição" AssociatedControlID="txtDescricao"></asp:Label>
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip6" Chave="txtDescricao" />
                        <CKEditor:CKEditorControl ID="txtDescricao" BasePath="/js/ckeditor/" runat="server"></CKEditor:CKEditorControl>
                    </div>
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <a data-toggle="collapse" data-parent="#accordion" href="#collapse1a">Soluções Educacionais
                            </a>
                        </div>
                        <div id="collapse1a" class="panel-collapse">
                            <div class="panel-body">
                                <uc:ucSolucaoEducacional ID="ucSolucaoEducacional1" runat="server" Chave="solucao" />
                            </div>
                        </div>
                    </div>
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <a data-toggle="collapse" data-parent="#accordion" href="#collapse1b">Pré-requisito
                            </a>
                        </div>
                        <div id="collapse1b" class="panel-collapse">
                            <div class="panel-body">
                                <asp:GridView ID="gvModulosPreRequisitos" AutoGenerateColumns="false" runat="server" DataKeyNames="ID" Width="100%">
                                    <Columns>
                                        <asp:BoundField DataField="Nome" HeaderText="Nome" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <div style="text-align: center;">
                                                    <asp:CheckBox ID="ckbModuloPai" runat="server" Style="display: block;" />
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="form-group">
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
        });
    </script>
</asp:Content>
