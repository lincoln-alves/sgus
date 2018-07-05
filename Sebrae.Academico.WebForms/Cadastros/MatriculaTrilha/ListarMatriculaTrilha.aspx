<%@ Page Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true"
    CodeBehind="ListarMatriculaTrilha.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.ListarMatriculaTrilha" %>
<%@ Register Src="~/UserControls/ucLupaUsuario.ascx" TagName="ucLupaUsuario" TagPrefix="uc" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <fieldset>
            <div class="form-group">
                <asp:Label ID="lblNome" runat="server" Text="Filtrar por trilha" AssociatedControlID="ddlTrilha" />
                <asp:DropDownList runat="server" ID="ddlTrilha" AutoPostBack="True" OnSelectedIndexChanged="ddlTrilha_SelectedIndexChanged"
                    CssClass="form-control">
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:Label ID="Label2" runat="server" Text="Filtrar pelo nível da trilha" AssociatedControlID="ddlTrilhaNivel" />
                <asp:DropDownList runat="server" ID="ddlTrilhaNivel" CssClass="form-control">
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:CheckBox Text="&nbsp;Somente novo sistema de trilhas?" ID="ckbNovasTrilhas" runat="server" Visible="true" />
            </div>

            <div class="form-group">
                <uc:LupaUsuario ID="ucLupaUsuario" runat="server" />
            </div>

            <div class="form-group">
                <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click"
                    CssClass="btn btn-primary mostrarload" />
                <asp:Button ID="btnNovo" runat="server" Text="Criar nova" OnClick="btnNovo_Click" CssClass="btn btn-default mostrarload" />
            </div>
        </fieldset>
    </div>
    <hr />
    <asp:Panel ID="pnlMatriculaTrilha" runat="server" Visible="false">
        <h4>Resultado da Busca</h4>
        <asp:GridView ID="dgvMatriculaTrilha" CssClass="table col-sm-12" runat="server" OnRowCommand="dgvMatriculaTrilha_RowCommand"
            AutoGenerateColumns="false" EnableModelValidation="True" GridLines="None">
            <Columns>
                <asp:TemplateField HeaderText="Nome">
                    <ItemTemplate>
                        <%#Eval("Usuario.Nome")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Nível Ocupacional">
                    <ItemTemplate>
                        <%#Eval("NivelOcupacional.Nome")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="CPF">
                    <ItemTemplate>
                        <%#Eval("Usuario.CPF")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <%#Eval("StatusMatriculaFormatado")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Data de Início">
                    <ItemTemplate>
                        <%#Eval("DataInicioFormatada")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Data Término">
                    <ItemTemplate>
                        <%#Eval("DataFimFormatada")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Data Limite">
                    <ItemTemplate>
                        <%#Eval("DataLimiteFormatada")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <ItemStyle Width="90px" HorizontalAlign="center" />
                    <ItemTemplate>
                        <asp:LinkButton ID="lkbEditar" runat="server" CausesValidation="False" CommandName="editar"
                            CommandArgument='<%# Eval("ID")%>' ToolTip="Editar" CssClass="mostrarload">
                        <span class="btn btn-default btn-xs">
						    <span class="glyphicon glyphicon-pencil"></span>
						</span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lkbExcluir" runat="server" CausesValidation="False" CommandName="excluir"
                            CommandArgument='<%# Eval("ID")%>' ToolTip="Excluir" OnClientClick="return ConfirmarExclusao();">
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
