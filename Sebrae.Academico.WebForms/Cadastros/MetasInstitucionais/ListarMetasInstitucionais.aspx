<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeBehind="ListarMetasInstitucionais.aspx.cs"
    Inherits="Sebrae.Academico.WebForms.Cadastros.MetasInstitucionais.ListarMetasInstitucionais" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <div class="form-group">
            <asp:Label ID="Label1" runat="server" Text="Buscar por nome" AssociatedControlID="txtNome"></asp:Label>
            <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label2" runat="server" Text="Buscar por data de início do ciclo" AssociatedControlID="txtDataInicioCiclo"></asp:Label>
            <asp:TextBox ID="txtDataInicioCiclo" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label3" runat="server" Text="Buscar por data de fim do ciclo" AssociatedControlID="txtDataFimCiclo"></asp:Label>
            <asp:TextBox ID="txtDataFimCiclo" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click" CssClass="btn btn-primary mostrarload" />
            <asp:Button ID="btnNovo" runat="server" Text="Criar nova" OnClick="btnNovo_Click" CssClass="btn btn-default mostrarload" />
        </div>
    </fieldset>
    <hr />
    <asp:Panel ID="pnlMetaInstitucuional" runat="server" Visible="false">
        <asp:GridView ID="dgvMetasInstitucionais" runat="server" CssClass="table col-sm-12"
            GridLines="None" AutoGenerateColumns="False" OnRowCommand="dgvMetasInstitucionais_RowCommand">
            <Columns>
                <asp:BoundField DataField="Nome" HeaderText="Nome" />
                <asp:BoundField DataField="DataInicioCiclo" HeaderText="Data de Inicio do Ciclo"
                    DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="DataFimCiclo" HeaderText="Data de Fim do Ciclo" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:TemplateField ShowHeader="False">
                    <ItemStyle Width="90px" HorizontalAlign="center" />
                    <ItemTemplate>
                        <asp:LinkButton ID="lkbEditar" runat="server" CausesValidation="False" CommandName="editar"
                            CssClass="mostrarload" CommandArgument='<%# Eval("ID")%>' ToolTip="Editar">
                            <span class="btn btn-default btn-xs">
						        <span class="glyphicon glyphicon-pencil"></span>
						    </span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lkbExcluir" runat="server" CausesValidation="False" CommandName="excluir"
                            CommandArgument='<%# Eval("ID")%>' ToolTip="Excluir"
                            OnClientClick="return ConfirmarExclusao();">
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
    <script type="text/javascript">
        jQuery(function ($) {
            $("#<%= txtDataInicioCiclo.ClientID %>").mask("99/99/9999");
           $("#<%= txtDataFimCiclo.ClientID %>").mask("99/99/9999");
       });
    </script>
</asp:Content>
