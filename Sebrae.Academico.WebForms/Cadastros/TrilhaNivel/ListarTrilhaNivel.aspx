<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="ListarTrilhaNivel.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.TrilhaNivel.ListarTrilhaNivel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var _preloadedListTrilha = <%= ViewState["_Trilhas"] ?? "''" %>;
        AutoCompleteDefine(_preloadedListTrilha, '#txtTrilha', true, true, false);
    </script>

    <div>
        <fieldset>
            <div class="form-group">
                <asp:Label ID="Label2" runat="server" Text="Buscar por nome" AssociatedControlID="txtNome"></asp:Label>
                <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="Label4" runat="server" Text="Trilha" AssociatedControlID="txtTrilha"></asp:Label>
                <asp:TextBox ID="txtTrilha" ClientIDMode="Static" runat="server" OnTextChanged="txtTrilha_TextChanged"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click"
                    CssClass="btn btn-primary mostrarload" />
                &nbsp;<asp:Button ID="btnNovo" runat="server" Text="Criar nova" OnClick="btnNovo_Click"
                    CssClass="btn btn-default mostrarload" />
            </div>
        </fieldset>
    </div>

    <asp:Panel ID="pnlBusca" runat="server">
        <hr />
        <asp:Panel runat="server" ID="pnlResultadoBusca" Visible="false">
            <h4>Resultado da Busca</h4>
        </asp:Panel>
        <asp:GridView runat="server" ID="dgvTrilhaNivel" AutoGenerateColumns="false" CssClass="table col-sm-12" GridLines="None"
            AllowPaging="True" OnRowCommand="dgvTrilhaNivel_RowCommand" OnPageIndexChanging="dgvTrilhaNivel_PageIndexChanging">
            <Columns>
                <asp:TemplateField HeaderText="Trilha">
                    <ItemTemplate>
                        <%# Eval("Trilha.Nome") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Nível">
                    <ItemTemplate>
                        <%# Eval("Nome") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Descrição">
                    <ItemTemplate>
                        <%# Eval("Descricao") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <ItemStyle Width="120px" HorizontalAlign="center" />
                    <ItemTemplate>
                        <asp:LinkButton ID="lkbEditar" runat="server" CausesValidation="False" CommandName="editar" ToolTip="Editar"
                            CommandArgument='<%# Eval("ID")%>' CssClass="mostrarload">
                            <span class="btn btn-default btn-xs">
								<span class="glyphicon glyphicon-pencil mostrarload"></span>
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
        </asp:GridView>
    </asp:Panel>

</asp:Content>
