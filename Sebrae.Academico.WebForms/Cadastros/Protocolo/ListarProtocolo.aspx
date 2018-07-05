<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="ListarProtocolo.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.Protocolo.ListarProtocolo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <fieldset>
            <div class="form-group">
                <asp:Label ID="Label2" runat="server" Text="Número do protocolo" AssociatedControlID="txtNumero"></asp:Label>
                <asp:TextBox ID="txtNumero" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click" CssClass="btn btn-primary mostrarload" />
            </div>
        </fieldset>
        <asp:GridView runat="server" ID="grdProtocolos" CssClass="table col-sm-12" AutoGenerateColumns="false" GridLines="None" OnRowCommand="grdProtocolos_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="Número">
                    <ItemTemplate><%# Eval("Numero") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Remetente">
                    <ItemTemplate><%# Eval("Remetente.Nome") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Destinatário">
                    <ItemTemplate><%# Eval("Destinatario.Nome") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Data de Envio">
                    <ItemTemplate><%# Eval("DataEnvio") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Data de Recebimento">
                    <ItemTemplate><%# Eval("DataRecebimento") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Recebido Por">
                    <ItemTemplate><%# Eval("UsuarioAssinatura.Nome") %></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Opções">
                    <ItemTemplate>
                        <asp:LinkButton ID="lkbEditar" runat="server" CausesValidation="False" CommandName="editar" CssClass="mostrarload" ToolTip="Visualizar"
                            CommandArgument='<%# Eval("ID")%>'>
                            <span class="btn btn-default btn-xs">
								<span class="glyphicon glyphicon-eye-open"></span>
							</span>
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
