<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeBehind="ListarAtividadeFormativaParticipacao.aspx.cs"
    Inherits="Sebrae.Academico.WebForms.Cadastros.AtividadeFormativa.ListarAtividadeFormativaParticipacao" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <fieldset>
            <div class="form-group">
                <asp:Label ID="Label1" runat="server" Text="Filtrar por trilha" AssociatedControlID="ddlTrilha"></asp:Label>
                <asp:DropDownList ID="ddlTrilha" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTrilha_SelectedIndexChanged"
                    CssClass="form-control mostrarload">
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:Label ID="Label9" runat="server" Text="Filtrar pelo nível da trilha" AssociatedControlID="ddlTrilhaNivel"></asp:Label>
                <asp:DropDownList ID="ddlTrilhaNivel" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTrilhaNivel_SelectedIndexChanged"
                    CssClass="form-control mostrarload">
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:Label ID="Label3" runat="server" Text="Filtrar por Ponto Sebrae" AssociatedControlID="ddlTopicoTematico"></asp:Label>
                <asp:DropDownList ID="ddlTopicoTematico" runat="server" CssClass="form-control">
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:Label ID="Label2" runat="server" Text="Filtrar pelo nome do aluno" AssociatedControlID="ddlNomeAluno"></asp:Label>
                <asp:DropDownList ID="ddlNomeAluno" runat="server" CssClass="form-control">
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
        <asp:GridView ID="dgvAtividadeFormativaParticipacao" runat="server" AutoGenerateColumns="false"
            CssClass="table col-sm-12" GridLines="None" OnRowDataBound="dgvAtividadeFormativaParticipacao_RowDataBound"
            OnRowCommand="dgvAtividadeFormativaParticipacao_RowCommand">
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
                <asp:TemplateField HeaderText="Ponto Sebrae">
                    <ItemTemplate>
                        <%#Eval("TopicoTematico.Nome")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <ItemStyle Width="90px" HorizontalAlign="center" />
                    <ItemTemplate>
                        <asp:LinkButton ID="lkbCadastrar" runat="server" CausesValidation="False" CommandName="cadastrar"
                            CssClass="mostrarload" EnableViewState="true" Visible="false" CommandArgument='<%# string.Concat( "idtrilha=", Eval("TrilhaOrigem.ID"), "&idtniv=", Eval("TrilhaNivelOrigem.ID"), "&idtop=", Eval("TopicoTematico.ID") , "&idusu=", Eval("UsuarioOrigem.ID") , "&idusuarioTrilha=", Eval("UsuarioTrilha.ID"))%>'
                            ToolTip="Cadastrar">
                            <span class="btn btn-default btn-xs">
								<span class="glyphicon glyphicon-plus"></span>
							</span> 
                            </asp:LinkButton>
                        <asp:LinkButton ID="lkbEditar" runat="server" CausesValidation="False" CommandName="editar"
                            EnableViewState="true" Visible="false" CommandArgument='<%# Eval("IdTrilhaAtividadeFormativaParticipacao")%>'
                            ToolTip="Editar" CssClass="mostrarload">
                            <span class="btn btn-default btn-xs">
								    <span class="glyphicon glyphicon-pencil"></span>
							    </span> 
                        </asp:LinkButton>
                        <asp:LinkButton ID="lkbExcluir" runat="server" CausesValidation="False" CommandName="excluir"
                            EnableViewState="true" Visible="false" CommandArgument='<%# Eval("IdTrilhaAtividadeFormativaParticipacao")%>'
                            ToolTip="Excluir" OnClientClick="return ConfirmarExclusao();">
                            <span class="btn btn-default btn-xs">
								<span class="glyphicon glyphicon-remove"></span>
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
