<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="ListarOferta.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.Oferta.ListarOferta" EnableEventValidation="false" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var _preloadedList = <%= ViewState["_SE"] %>;
        AutoCompleteDefine(_preloadedList, '#txtSolucaoEducacional', '#txtSolucaoEducacionalSelecionada', true);
    </script>
    <div>
        <fieldset>
            <div class="form-group">
                <asp:Label ID="Label2" runat="server" Text="Buscar por nome" AssociatedControlID="txtNome"></asp:Label>
                <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="Label1" runat="server" Text="Solução educacional" AssociatedControlID="txtSolucaoEducacional"></asp:Label>
                <asp:TextBox ID="txtSolucaoEducacional" runat="server" ClientIDMode="Static" CssClass="mostrarload" OnTextChanged="txtSolucaoEducacional_OnTextChanged" />
            </div>
            <div class="form-group">
                <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click"
                    CssClass="btn btn-primary mostrarload" />
                &nbsp;<asp:Button ID="btnNovo" runat="server" Text="Criar nova" OnClick="btnNovo_Click"
                    CssClass="btn btn-default mostrarload" />
            </div>
        </fieldset>
    </div>
    <hr />
    <asp:Panel ID="pnlOferta" runat="server" Visible="false">

        <h4>Resultado da Busca</h4>
        <asp:Literal ID="litTable" runat="server" />
        <asp:GridView ID="dgvOferta" runat="server" CssClass="table col-sm-12" OnRowCommand="dgvOferta_RowCommand" AutoGenerateColumns="false" AllowPaging="True" PageSize="50" OnPageIndexChanging="dgvOferta_OnPageIndexChanging">
            <Columns>
                <asp:TemplateField HeaderText="Oferta">
                    <ItemTemplate>
                        <%#Eval("NomeExibicao")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Início">
                    <ItemTemplate>
                        <%# Eval("DataInicioInscricoes") == null ? "" : Convert.ToDateTime(Eval("DataInicioInscricoes")).ToString("dd/MM/yyyy") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Fim">
                    <ItemTemplate>
                        <%# (Eval("DataFimInscricoes") == null ? "" : Convert.ToDateTime(Eval("DataFimInscricoes")).ToString("dd/MM/yyyy")) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <ItemStyle Width="90px" HorizontalAlign="center" />
                    <ItemTemplate>
                        <asp:LinkButton ID="lkbRepublicar" runat="server" CausesValidation="False" CommandName="republicar"
                            CommandArgument='<%# Eval("ID")%>' ToolTip="Republicar">
                                        <span class="btn btn-default btn-xs">
								            <span class="glyphicon glyphicon-edit"></span>
							            </span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lkbEditar" runat="server" CausesValidation="False" CommandName="editar"
                            CommandArgument='<%# Eval("ID")%>' ToolTip="Editar">
                                        <span class="btn btn-default btn-xs">
								            <span class="glyphicon glyphicon-pencil"></span>
							            </span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lkbExcluir" runat="server" CausesValidation="False" CommandName="excluir"
                            CommandArgument='<%# Eval("ID")%>' ToolTip="Excluir" OnClientClick="return ConfirmarExclusao();">
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
