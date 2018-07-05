<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="ListarSeAutoindicativa.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.SeAutoIndicativa.ListarSeAutoindicativa" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div>
        <fieldset>
            <div class="form-group">
                <asp:Label ID="Label1" runat="server" Text="Trilha" AssociatedControlID="ddlTrilha"></asp:Label>
                <asp:DropDownList ID="ddlTrilha" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTrilha_OnSelectedIndexChanged">
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:Label ID="Label2" runat="server" Text="Nível da Trilha" AssociatedControlID="ddlTrilhaNivel"></asp:Label>
                <asp:DropDownList ID="ddlTrilhaNivel" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTrilhaNivel_OnSelectedIndexChanged"></asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:Label ID="Label3" runat="server" Text="Tópico Temático" AssociatedControlID="ddlTopicoTematico"></asp:Label>
                <asp:DropDownList ID="ddlTopicoTematico" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTopicoTematico_OnSelectedIndexChanged"></asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click" CssClass="btn btn-primary mostrarload" />
            </div>
        </fieldset>
    </div>
    <asp:Panel ID="pnlSolucoesEducacionaisSugeridasAprovacao" runat="server">
        <h4>Soluções Educacionais Autoindicativas</h4>
        <asp:Literal ID="Literal3" runat="server" />
        <asp:GridView ID="gvSolucoesEducacionaisSugeridasAprovacao" runat="server" CssClass="table col-sm-12" AutoGenerateColumns="false" GridLines="None" OnRowDataBound="gvSolucoesEducacionaisSugeridasAprovacao_RowDataBound" OnRowCommand="gvSolucoesEducacionaisSugeridasAprovacao_RowCommand" AllowPaging="True" OnPageIndexChanging="gvSolucoesEducacionaisSugeridasAprovacao_PageIndexChanging" PageSize="10">
            <Columns>
                <asp:TemplateField HeaderText="Usuário">
                    <ItemTemplate>
                        <%# Eval("Usuario") != null ? Eval("Usuario.Nome") : ""%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tipo">
                    <ItemTemplate>
                        <%#Eval("FormaAquisicao.Nome")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Trilha">
                    <ItemTemplate>
                        <%#Eval("TrilhaNivel.Trilha.Nome")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Nível">
                    <ItemTemplate>
                        <%#Eval("TrilhaNivel.Nome")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tópico Tematico">
                    <ItemTemplate>
                        <%#Eval("TrilhaTopicoTematico.NomeExibicao")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Título">
                    <ItemTemplate>
                        <%#Eval("Nome")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Objetivo">
                    <ItemTemplate>
                        <%#Eval("Objetivo.Nome")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <%#Eval("AprovadoStatus")%>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Data de envio">
                    <ItemTemplate>
                        <%#Eval("DataCriacao", "{0:dd/MM/yyyy}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Prazo para avaliação">
                    <ItemTemplate>
                        <%#Eval("PrazoAvaliacaoTutorFormatado")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ação">
                    <ItemTemplate>
                        <asp:LinkButton ID="lkbEditarItemTrilha" CommandArgument='<%# Eval("ID") %>' CommandName="editar" runat="server" CausesValidation="False" CssClass="mostrarload" Text="Editar" ToolTip="Editar">
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
                <itemtemplate>Nenhum registro encontrado</itemtemplate>
            </EmptyDataTemplate>
        </asp:GridView>
    </asp:Panel>
</asp:Content>
