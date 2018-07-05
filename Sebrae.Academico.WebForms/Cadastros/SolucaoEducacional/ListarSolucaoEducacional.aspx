<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="ListarSolucaoEducacional.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.SolucaoEducacional.ListarSolucaoEducacional" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       
    <script type="text/javascript">
        var _preloadedList = <%= ViewState["_SE"] %>;
        AutoCompleteDefine(_preloadedList, '#txtNome', true, true, false);
    </script>

    <asp:HiddenField ID="SolucoesEducacionaisPesquisa" runat="server" />
    <div>
        <fieldset>
            <div class="form-group">
                <asp:Label ID="lblNome" runat="server" Text="Buscar por nome" AssociatedControlID="txtNome" />
                <asp:TextBox ID="txtNome" runat="server" CssClass="form-control" ClientIDMode="Static" data-nome="" OnTextChanged="txtNome_OnTextChanged"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Button ID="btnPesquisar" OnClick="btnPesquisar_OnClick" runat="server" Text="Pesquisar" CssClass="btn btn-primary mostrarload" />
                <asp:Button ID="btnNovo" runat="server" Text="Criar nova" OnClick="btnNovo_Click" CssClass="btn btn-default mostrarload" />
            </div>
        </fieldset>
    </div>
    <hr />
    <asp:Panel ID="pnlSolucaoEducacional" runat="server" Visible="false">
        <h4>
            Resultado da Busca</h4>
        <asp:GridView ID="dgvSolucaoEducacional" runat="server" CssClass="table col-sm-12"
            GridLines="None" OnRowCommand="dgvSolucaoEducacional_RowCommand" AutoGenerateColumns="false"
            EnableModelValidation="True" AllowPaging="True" PageSize="50" OnPageIndexChanging="dgvSolucaoEducacional_OnPageIndexChanging">
            <Columns>
                <asp:TemplateField HeaderText="Solução Educacional">
                    <ItemTemplate>
                        <%#Eval("Nome") %>
                        <%# Eval("UFGestor")!=null ? " - (" + Eval("UFGestor.Sigla") + ")" : "" %>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Código">
                    <ItemTemplate>
                        <%#Eval("DescricaoSequencial")%>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Fornecedor">
                    <ItemTemplate>
                        <%#Eval("Fornecedor.Nome") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <ItemStyle Width="90px" HorizontalAlign="center" />
                    <ItemTemplate>
                        <asp:LinkButton ID="lkbEditar" runat="server" CausesValidation="False" CommandName="editar" CssClass="mostrarload" ToolTip="Editar"
                            CommandArgument='<%# Eval("ID")%>'>
                            <span class="btn btn-default btn-xs">
								<span class="glyphicon glyphicon-pencil"></span>
							</span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lkbExcluir" runat="server" CausesValidation="False" CommandName="excluir" ToolTip="Excluir"
                            CommandArgument='<%# Eval("ID")%>' Text="Excluir" OnClientClick="return ConfirmarExclusao();">
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
