<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeBehind="ListarCertificadoTemplate.aspx.cs"
    Inherits="Sebrae.Academico.WebForms.Cadastros.CertificadoTemplate.ListarCertificadoTemplate" %>

<%@ Register Src="~/UserControls/ucVisualizaCertificadoTemplate.ascx" TagName="ucVisualizaCertificadoTemplate" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <div class="form-group">
            <asp:Label ID="lblNome" runat="server" Text="Buscar por nome" AssociatedControlID="txtNome" />
            <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Button ID="Button1" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click" CssClass="btn btn-primary mostrarload" />
            <asp:Button ID="Button2" runat="server" Text="Criar novo" OnClick="btnNovo_Click" CssClass="btn btn-default mostrarload" />
        </div>
    </fieldset>
    <hr />
    <asp:Panel ID="pnlTemplateCertificado" runat="server" Visible="false">
        <p><b>Resultado da Busca</b></p>
        <asp:Literal ID="litTable" runat="server" />
        <asp:GridView ID="dgvCertificadoTemplate" runat="server" CssClass="table col-sm-12"
            GridLines="None" OnRowCommand="dgvCertificadoTemplate_RowCommand" AutoGenerateColumns="false"
            EnableModelValidation="True">
            <Columns>
                <asp:TemplateField HeaderText="Nome do Template">
                    <ItemTemplate>
                        <%#Eval("Nome") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Uf">
                    <ItemTemplate>
                        <%#Eval("UF.Nome") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False" >
                    <ItemStyle Width="110px" HorizontalAlign="center" />
                    <ItemTemplate>
                        <asp:LinkButton ID="lkbVisualizar" runat="server" CausesValidation="False" CommandName="visualizar"
                            CssClass="mostrarload" CommandArgument='<%# Eval("ID")%>' ToolTip="Visualizar">
                                <span class="btn btn-default btn-xs">
								    <span class="glyphicon glyphicon-search"></span>
							    </span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lkbCopiar" runat="server" CausesValidation="False" CommandName="copiar"
                            CssClass="mostrarload" CommandArgument='<%# Eval("ID")%>' ToolTip="Copiar">
                                <span class="btn btn-default btn-xs">
								    <span class="glyphicon glyphicon-edit"></span>
							    </span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lkbEditar" id_uf='<%#Eval("UF.ID")%>' runat="server" CausesValidation="False" CommandName="editar"
                            CssClass="mostrarload" CommandArgument='<%# Eval("ID")%>' ToolTip="Editar" OnPreRender="lkbEditarExcluir_PreRender">
                                <span class="btn btn-default btn-xs">
								    <span class="glyphicon glyphicon-pencil"></span>
							    </span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lkbExcluir" id_uf='<%#Eval("UF.ID")%>' runat="server" CausesValidation="False" CommandName="excluir"
                            CommandArgument='<%# Eval("ID")%>' ToolTip="Excluir" OnPreRender="lkbEditarExcluir_PreRender"
                            OnClientClick="return ConfirmarExclusao();">
                                <span class="btn btn-default btn-xs">
								    <span class="glyphicon glyphicon-remove"></span>
							    </span>
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
            </EmptyDataTemplate>
        </asp:GridView>
    </asp:Panel>
    <uc1:ucVisualizaCertificadoTemplate ID="ucMostraPreviaRel" runat="server" />
</asp:Content>
