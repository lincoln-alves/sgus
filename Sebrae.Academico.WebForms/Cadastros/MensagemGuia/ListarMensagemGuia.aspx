<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeBehind="ListarMensagemGuia.aspx.cs"
    Inherits="Sebrae.Academico.WebForms.Cadastros.MensagemGuia.ListarMensagemGuia" %>
<%@ Import Namespace="Sebrae.Academico.Dominio.Enumeracao" %>
<%@ Import Namespace="Sebrae.Academico.Util.Classes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="pnlMensagemGuia" runat="server" Visible="false">
        <asp:Literal ID="litTable" runat="server" />
        <asp:GridView ID="dgvMensagemGuia" runat="server" CssClass="table col-sm-12"
            GridLines="None" OnRowCommand="dgvMensagemGuia_RowCommand" AutoGenerateColumns="false"
            EnableModelValidation="True">
            <Columns>
                <asp:TemplateField HeaderText="Mensagem da Guia">
                    <ItemTemplate>
                        <%# ((enumMomento)Eval("ID")).GetDescription() %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False" >
                    <ItemStyle Width="50px" HorizontalAlign="center" />
                    <ItemTemplate>
                        <asp:LinkButton ID="lkbEditar" runat="server" CausesValidation="False" CommandName="editar"
                            CssClass="mostrarload" CommandArgument='<%# Eval("ID_INT") %>' ToolTip="Editar">
                                <span class="btn btn-default btn-xs">
								    <span class="glyphicon glyphicon-pencil"></span>
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
</asp:Content>
