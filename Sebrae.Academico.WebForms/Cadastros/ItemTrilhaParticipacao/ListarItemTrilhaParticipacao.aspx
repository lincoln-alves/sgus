<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeBehind="ListarItemTrilhaParticipacao.aspx.cs"
    Inherits="Sebrae.Academico.WebForms.Cadastros.ListarItemTrilhaParticipacao" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h3>
        Filtro de Participação em Itens de Trilha</h3>
    <div>
        <fieldset>
            <div class="form-group">
                <asp:Label ID="lblTrilha" runat="server" Text="Filtrar por trilha" AssociatedControlID="ddlTrilha"></asp:Label>
                <asp:DropDownList ID="ddlTrilha" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTrilha_SelectedIndexChanged"
                    CssClass="form-control mostrarload">
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:Label ID="lbTrilhaNivel" runat="server" Text="Filtrar pelo nível da trilha" AssociatedControlID="ddlTrilhaNivel"></asp:Label>
                <asp:DropDownList ID="ddlTrilhaNivel" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTrilhaNivel_SelectedIndexChanged"
                    CssClass="form-control mostrarload">
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:Label ID="lbTopicoTematico" runat="server" Text="Filtrar por tópico temático" AssociatedControlID="ddlTopicoTematico"></asp:Label>
                <asp:DropDownList ID="ddlTopicoTematico" runat="server" CssClass="form-control">
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:Label ID="lbNomeAluno" runat="server" Text="Filtrar pelo nome do aluno" AssociatedControlID="ddlNomeAluno"></asp:Label>
                <asp:DropDownList ID="ddlNomeAluno" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlNomeAluno_SelectedIndexChanged"
                    CssClass="form-control">
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:Label ID="lblItemTrilha" runat="server" Text="Filtrar por item da trilha" AssociatedControlID="ddlItemTrilha"></asp:Label>
                <asp:DropDownList ID="ddlItemTrilha" runat="server" CssClass="form-control">
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click"
                    CssClass="btn btn-primary mostrarload" />
            </div>
        </fieldset>
    </div>
    <hr />
    <asp:Panel ID="pnlTrilha" runat="server" Visible="false">
        <h4>
            Resultado da Busca</h4>
        <asp:GridView ID="dgvItemTrilhaParticipacao" runat="server" CssClass="table col-sm-12"
            GridLines="None" AutoGenerateColumns="false" OnRowCommand="dgvItemTrilhaParticipacao_RowCommand"
            EnableModelValidation="True" OnRowDataBound="dgvItemTrilhaParticipacao_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="Usuário">
                    <ItemTemplate>
                        <%#Eval("UsuarioOrigem.Nome")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="CPF">
                    <ItemTemplate>
                        <%#Eval("UsuarioOrigem.CPF")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Trilha">
                    <ItemTemplate>
                        <%#Eval("TrilhaOrigem.Nome")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Trilha Nível">
                    <ItemTemplate>
                        <%#Eval("TrilhaNivelOrigem.Nome")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tópico">
                    <ItemTemplate>
                        <%#Eval("TopicoTematico.Nome")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Item Trilha">
                    <ItemTemplate>
                        <%#Eval("ItemTrilha.Nome")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <ItemStyle Width="90px" HorizontalAlign="center" />
                    <ItemTemplate>
                        <asp:LinkButton ID="lkbCadastrar" runat="server" CausesValidation="False" CommandName="cadastrar"
                            CssClass="mostrarload" CommandArgument='<%# string.Concat( "idtrilha=", Eval("TrilhaOrigem.ID"), "&idtniv=", Eval("TrilhaNivelOrigem.ID"), "&idtop=", Eval("TopicoTematico.ID") , "&idusu=", Eval("UsuarioOrigem.ID") , "&iditemtrilha=", Eval("ItemTrilha.ID"), "&idusuarioTrilha=", Eval("UsuarioTrilha.ID"))%>'
                            ToolTip="Cadastrar">
                            <span class="btn btn-default btn-xs">
								<span class="glyphicon glyphicon-plus"></span>
							</span>            
                        </asp:LinkButton>
                        <asp:LinkButton ID="lkbEditar" runat="server" CausesValidation="False" CommandName="editar"
                            CssClass="mostrarload" CommandArgument='<%# Eval("IdItemTrilhaParticipacao")%>'
                            ToolTip="Editar">
                            <span class="btn btn-default btn-xs">
								<span class="glyphicon glyphicon-pencil"></span>
							</span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lkbExcluir" runat="server" CausesValidation="False"
                            CommandName="excluir" CommandArgument='<%# Eval("IdItemTrilhaParticipacao")%>'
                            ToolTip="Excluir" OnClientClick="return ConfirmarExclusao();">
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
