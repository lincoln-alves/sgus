<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="ListarProfessor.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.ListarProfessor"
    MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <div class="form-group">
            <asp:Label ID="Label2" runat="server" Text="Buscar por nome"  AssociatedControlID="txtNome" ></asp:Label>
            <asp:TextBox ID="txtNome" runat="server" class="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click"
                CssClass="btn btn-primary mostrarload" />
            <asp:Button ID="btnNovo" runat="server" Text="Cadastrar novo" OnClick="btnNovo_Click" CssClass="btn btn-default mostrarload" />
        </div>
    </fieldset>
    <hr />
    <asp:Panel ID="pnlProfessor" runat="server" Visible="false">
        <h4>
            Resultado da Busca</h4>
        <asp:GridView ID="dgvProfessor" runat="server" CssClass="table col-sm-12" OnRowCommand="dgvProfessor_RowCommand"
            AutoGenerateColumns="false" EnableModelValidation="True" OnRowDataBound="dgvProfessor_OnRowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="Professor">
                    <ItemTemplate>
                        <%#Eval("Nome") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="CPF">
                    <ItemTemplate>
                        <%#Eval("CPF")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <ItemStyle Width="90px" HorizontalAlign="center" />
                    <ItemTemplate>
                        <asp:LinkButton ID="lkbEditar" runat="server" CausesValidation="False" CommandName="editar"
                            CssClass="mostrarload" CommandArgument='<%# Eval("ID")%>' ToolTip="Editar">
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
                        <asp:LinkButton ID="lkbSincronizarComMoodle" runat="server" CausesValidation="False" CommandName="sincronizarComMoodle"
                            ToolTip="Sincronizar usuário com o Moodle" CommandArgument='<%# Eval("ID")%>' OnClientClick="return ConfirmarExclusao('Sincronizar professor?');">
                                <span class="btn btn-default btn-xs">
									<span class="glyphicon glyphicon-send"></span>
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
