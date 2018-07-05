<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="ListarPrograma.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.ListarPrograma" 
MaintainScrollPositionOnPostback="true"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <fieldset>
            <div class="form-group">
                <asp:Label ID="Label2" runat="server" Text="Nome" AssociatedControlID="txtNome"></asp:Label>
                <asp:TextBox ID="txtNome" runat="server" CssClass="form-control" ></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click" CssClass="btn btn-primary mostrarload" />
                <asp:Button ID="btnNovo" runat="server" Text="Criar novo" OnClick="btnNovo_Click" CssClass="btn btn-default mostrarload" />
            </div>
        </fieldset>
    </div>
    
<hr />
    <asp:Panel ID="pnlPrograma" runat="server" Visible="false">

    
    <h4>Resultado da Busca</h4>
        <asp:Literal ID="litTable" runat="server" />
        <asp:GridView ID="dgvPrograma" runat="server" OnRowCommand="dgvPrograma_RowCommand" CssClass="table col-sm-12" AutoGenerateColumns="false" GridLines="None">
            <Columns>
                <asp:TemplateField HeaderText="Código">
                    <ItemTemplate>
                        <%#Eval("DescricaoSequencial") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Programa">
                    <ItemTemplate>
                        <%#Eval("Nome") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <ItemStyle Width="90px" HorizontalAlign="center" />
                    <ItemTemplate>
                        <asp:LinkButton ID="lkbEditar" runat="server" CausesValidation="False" CommandName="editar" CommandArgument='<%# Eval("ID")%>' CssClass="mostrarload" ToolTip="Editar">
                            <span class="btn btn-default btn-xs">
								<span class="glyphicon glyphicon-pencil"></span>
							</span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lkbExcluir" runat="server" CausesValidation="False" CommandName="excluir" CommandArgument='<%# Eval("ID")%>' OnClientClick="return ConfirmarExclusao();" ToolTip="Excluir">
                            <span class="btn btn-default btn-xs">
								<span class="glyphicon glyphicon-remove"></span>
							</span>
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>
</asp:Content>
