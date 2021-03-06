﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.Master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeBehind="ListarOrcamentoReembolso.aspx.cs"
    Inherits="Sebrae.Academico.WebForms.Cadastros.OrcamentoReembolso.ListarOrcamentoReembolso" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="/css/sebraeuc_fonts.css" rel="stylesheet" />
    <div>
        <fieldset>
            <div class="form-group">
                <asp:Label ID="Label2" runat="server" Text="Buscar por nome" AssociatedControlID="txtNome"></asp:Label>
                <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click"
                    CssClass="btn btn-primary mostrarload" />
                <asp:Button ID="Button2" runat="server" Text="Criar nova" OnClick="btnNovo_Click" CssClass="btn btn-default mostrarload" />
            </div>
        </fieldset>
    </div>
    <hr />
    <asp:Panel ID="pnlOrcamentoReembolso" runat="server" Visible="false">
        <h4>
            Resultado da Busca</h4>
        <asp:GridView ID="dgvOrcamentoReembolso" runat="server" AutoGenerateColumns="false"
            CssClass="table col-sm-12" GridLines="None"
            OnRowCommand="dgvOrcamentoReembolso_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="Ano">
                    <ItemTemplate>
                        <%#Eval("Ano") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Total">
                    <ItemTemplate>
                        <%#((decimal)Eval("Total")).ToString("C") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Utilizado">
                    <ItemTemplate>
                        <%#((decimal)Eval("Utilizado")).ToString("C") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Saldo">
                    <ItemTemplate>
                        <%#((decimal)Eval("Saldo")).ToString("C") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                        <ItemStyle Width="90px" HorizontalAlign="center" />
                        <ItemTemplate>
                            <asp:LinkButton ID="lkbEditar" runat="server" CausesValidation="False" CommandName="editar" ToolTip="Editar"
                                CommandArgument='<%# Eval("ID")%>' CssClass="mostrarload">
                                <span class="btn btn-default btn-xs">
									<span class="glyphicon glyphicon-pencil"></span>
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
