<%@ Page Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="ListarTrilha.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.ListarTrilha"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <fieldset>
            <div class="form-group">
                <asp:Label ID="Label2" runat="server" Text="Buscar por nome"></asp:Label>
                <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click"
                    CssClass="btn btn-primary mostrarload" />
                <asp:Button ID="btnNovo" runat="server" Text="Criar nova" OnClick="btnNovo_Click" CssClass="btn btn-default mostrarload" />
            </div>
        </fieldset>
    </div>
    <hr />
    <asp:Panel ID="pnlTrilha" runat="server" Visible="false">
        <div>
            <h4>
                Resultado da Busca</h4>
            <asp:GridView ID="dgvTrilha" runat="server" CssClass="table col-sm-12" GridLines="None"
                OnRowCommand="dgvTrilha_RowCommand" AutoGenerateColumns="false">
                <Columns>
                    <asp:TemplateField HeaderText="#">
                        <ItemTemplate>
                            <%#Eval("ID") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Trilha">
                        <ItemTemplate>
                            <%#Eval("Nome") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False">
                        <ItemStyle Width="90px" HorizontalAlign="center" />
                        <ItemTemplate>
                            <asp:LinkButton ID="lkbEditar" runat="server" CausesValidation="False" CommandName="editar" ToolTip="Editar"
                                CommandArgument='<%# Eval("ID")%>' CssClass="mostrarload">
                                <span class="btn btn-default btn-xs">
									<span class="glyphicon glyphicon-pencil"></span>
								</span>
                            </asp:LinkButton>
                            <asp:LinkButton ID="lkbExcluir" runat="server" CausesValidation="False" CommandName="excluir" ToolTip="Excluir"
                                CommandArgument='<%# Eval("ID")%>' OnClientClick="return ConfirmarExclusao();">
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
        </div>
    </asp:Panel>
</asp:Content>
