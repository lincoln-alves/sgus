<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="ListarTag.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.Tag.ListarTag" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <div class="form-group">
            <asp:Label ID="lblNome" runat="server" Text="Buscar por nome" AssociatedControlID="txtNome" />
            <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click"
                CssClass="btn btn-primary mostrarload" />
            <asp:Button ID="btnAdicionarTag" runat="server" Text="Criar nova" OnClick="btnNovo_Click"
                CssClass="btn btn-default mostrarload" />
        </div>
    </fieldset>
    <hr />
    <asp:Panel ID="pnlTag" runat="server" Visible="false">
        <p>
            <b>Resultado da Busca</b>
        </p>
        <asp:Literal ID="litTable" runat="server" />
        <asp:GridView ID="dgvTag" runat="server" CssClass="table col-sm-12" GridLines="None"
            OnRowCommand="dgvTag_RowCommand" AutoGenerateColumns="false" EnableModelValidation="True">
            <Columns>
                <asp:TemplateField HeaderText="Nome">
                    <ItemTemplate>
                        <%#Eval("Nome")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:LinkButton ID="lkbEditar" runat="server" CausesValidation="False" CommandName="editar"
                            CssClass="mostrarload" CommandArgument='<%# Eval("ID")%>' ToolTip="Editar">
                                <span class="btn btn-default btn-xs">
								    <span class="glyphicon glyphicon-pencil"></span>
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
