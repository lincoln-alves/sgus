<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="ListarMissao.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.Missao.ListarMissao" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
        var _preloadedList = <%= ViewState["_trilha"] ?? "''" %>;
        var _predloadedNivel = <%= ViewState["_niveis"] ?? "''" %>;
        AutoCompleteDefine(_preloadedList, '#txtTrilha', true, true, false);
        AutoCompleteDefine(_predloadedNivel, '#txtTrilhaNivel', true, true, false);
    </script>

    <div>
        <fieldset>
            <div class="form-group">
                <asp:Label ID="lblNome" runat="server" Text="Buscar por nome" AssociatedControlID="txtMissao" />
                <asp:TextBox ID="txtMissao" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="lblId" runat="server" Text="Busca por ID" AssociatedControlID="txtIdMissao" />
                <asp:TextBox ID="txtIdMissao" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="lblTrilha" runat="server" Text="Busca por trilha" AssociatedControlID="txtTrilha" />
                <asp:TextBox ID="txtTrilha" runat="server" CssClass="form-control" ClientIDMode="Static" OnTextChanged="txtTrilha_TextChanged"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="Label1" runat="server" Text="Busca por nível da trilha" AssociatedControlID="txtTrilhaNivel" />
                <asp:TextBox ID="txtTrilhaNivel" runat="server" CssClass="form-control" ClientIDMode="Static" data-mensagemVazia="Selecione uma Trilha"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click" CssClass="btn btn-primary mostrarload" />
                <asp:Button ID="btnNovo" runat="server" Text="Criar novo" OnClick="btnNovo_Click" CssClass="btn btn-default mostrarload" />
            </div>
        </fieldset>
    </div>
    <hr />
    <asp:Panel ID="pnlMissao" runat="server" Visible="false">
        <h4>Resultado da Busca</h4>
        <asp:GridView ID="dgvMissao" runat="server" CssClass="table col-sm-12"
            GridLines="None" OnRowCommand="dgvMissao_RowCommand" AutoGenerateColumns="false"
            EnableModelValidation="True">
            <Columns>
                <asp:TemplateField HeaderText="Missão">
                    <ItemTemplate>
                        <%#Eval("Nome") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ponto Sebrae">
                    <ItemTemplate>
                        <%#Eval("PontoSebrae.NomeExibicao") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Nível e Trilha">
                    <ItemTemplate>
                        <%#Eval("PontoSebrae.TrilhaNivel.Nome") %> - <%#Eval("PontoSebrae.TrilhaNivel.Trilha.Nome") %>
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
