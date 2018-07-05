<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeBehind="ListarHierarquiaAuxiliar.aspx.cs"
    Inherits="Sebrae.Academico.WebForms.Cadastros.HierarquiaAuxiliar.ListarHierarquiaAuxiliar" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <div class="form-group">
            <asp:Label ID="Label3" runat="server" Text="Buscar por nome do acessor" AssociatedControlID="txtNome"></asp:Label>
            <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click"
                CssClass="btn btn-primary mostrarload" />
            <asp:Button ID="Button2" runat="server" Text="Criar novo" OnClick="btnNovo_Click" CssClass="btn btn-default mostrarload" />
        </div>
    </fieldset>
    <hr />
    <asp:Panel ID="pnlFornecedor" runat="server" Visible="false">
        <h4>Resultado da Busca</h4>
        <asp:GridView ID="dgvHierarquiaAux" CssClass="table col-sm-12" GridLines="None" runat="server"
            OnRowCommand="dgvHierarquiaAux_RowCommand" AutoGenerateColumns="false" EnableModelValidation="True">
            <Columns>
                <asp:TemplateField HeaderText="Usuário">
                    <ItemTemplate>
                        <%# Eval("Usuario") != null ? Eval("Usuario.Nome") : ""%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Diretoria">
                    <ItemTemplate>
                        <%#Eval("NomeDiretoria") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <ItemStyle Width="90px" HorizontalAlign="center" />
                    <ItemTemplate>
                        <asp:LinkButton ID="lkbEditar" runat="server" CausesValidation="False" CommandName="editar"
                            CssClass="mostrarload" ToolTip="Editar" CommandArgument='<%# Eval("ID")%>'>
                            <span class="btn btn-default btn-xs">
								<span class="glyphicon glyphicon-pencil"></span>
							</span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lkbExcluir" runat="server" CausesValidation="False" CommandName="excluir"
                            ToolTip="Excluir" CommandArgument='<%# Eval("ID")%>' OnClientClick="return ConfirmarExclusao();">
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
</asp:Content>
