<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="ListarTurma.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.Turma.ListarTurma"
    MaintainScrollPositionOnPostback="true" %>
<%@ Import Namespace="Sebrae.Academico.Dominio.Enumeracao" %>
<%@ Import Namespace="Sebrae.Academico.Util.Classes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var _preloadedList = <%= ViewState["_SE"] ?? "''" %>;
        AutoCompleteDefine(_preloadedList, '#txtSolucaoEducacional', true, true, false);

        var _preloadedListOfertas = <%= ViewState["_Oferta"] ?? "''" %>;
        AutoCompleteDefine(_preloadedListOfertas, '#txtOferta', true, true, false);
    </script>

    <div>
        <fieldset>
            <div class="form-group">
                <asp:Label ID="Label2" runat="server" Text="Buscar por nome" AssociatedControlID="txtNome"></asp:Label>
                <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="Label4" runat="server" Text="Solução educacional" AssociatedControlID="txtSolucaoEducacional"></asp:Label>
                <asp:TextBox ID="txtSolucaoEducacional" ClientIDMode="Static" runat="server" OnTextChanged="txtSolucaoEducacional_OnTextChanged"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="Label1" runat="server" Text="Oferta" AssociatedControlID="txtOferta"></asp:Label>
                <asp:TextBox ID="txtOferta" ClientIDMode="Static" runat="server" OnTextChanged="txtOferta_OnTextChanged" data-mensagemVazia="Selecione uma Solução Educacional"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click"
                    CssClass="btn btn-primary mostrarload" />
                &nbsp;<asp:Button ID="btnNovo" runat="server" Text="Criar nova" OnClick="btnNovo_Click"
                    CssClass="btn btn-default mostrarload" />
            </div>
        </fieldset>
    </div>
    <asp:Panel ID="pnlTurma" runat="server" Visible="false">
        <hr />
        <h4>Resultado da Busca</h4>
        <asp:Literal ID="litTable" runat="server" />
        <asp:GridView ID="dgvTurma" runat="server" CssClass="table col-sm-12" GridLines="None"
            OnRowCommand="dgvTurma_RowCommand" AutoGenerateColumns="false" AllowPaging="True" PageSize="50" OnPageIndexChanging="dgvTurma_OnPageIndexChanging">
            <Columns>
                <asp:TemplateField HeaderText="Turma">
                    <ItemTemplate>
                        <%#Eval("Nome")%> - <%#Eval("Codigo")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <%# Eval("Status") == null ? "" : ((enumStatusTurma)Eval("Status")).GetDescription() %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Início">
                    <ItemTemplate>
                        <%# (Eval("DataInicio") == null ? "" : Convert.ToDateTime(Eval("DataInicio")).ToString("dd/MM/yyyy"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Fim">
                    <ItemTemplate>
                        <%# (Eval("DataFinal") == null ? "" : Convert.ToDateTime(Eval("DataFinal")).ToString("dd/MM/yyyy"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <ItemStyle Width="120px" HorizontalAlign="center" />
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="historico"
                            CommandArgument='<%# Eval("ID")%>' ToolTip="Histórico de Responsáveis e Consultores Educacionais">
                                        <span class="btn btn-default btn-xs">
								            <span class="glyphicon glyphicon-search"></span>
							            </span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lkbRepublicar" runat="server" CausesValidation="False" CommandName="republicar"
                            CommandArgument='<%# Eval("ID")%>' ToolTip="Duplicar">
                                        <span class="btn btn-default btn-xs">
								            <span class="glyphicon glyphicon-edit"></span>
							            </span>
                        </asp:LinkButton>
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
            <EmptyDataTemplate>
                <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
            </EmptyDataTemplate>
        </asp:GridView>
    </asp:Panel>
</asp:Content>
