<%@ Page Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true"
    CodeBehind="ListarSolucaoSebrae.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.SolucaoSebrae.ListarSolucaoSebrae" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <fieldset>
            <div class="form-group">
                <asp:Label ID="Label5" runat="server" Text="Buscar por Trilha" AssociatedControlID="ddlTrilha"></asp:Label>
                <asp:DropDownList ID="ddlTrilha" runat="server" CssClass="form-control mostrarload" AutoPostBack="True" OnSelectedIndexChanged="ddlTrilha_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:Label ID="Label6" runat="server" Text="Buscar por Nível" AssociatedControlID="ddlTrilhaNivel"></asp:Label>
                <asp:DropDownList ID="ddlTrilhaNivel" runat="server" CssClass="form-control mostrarload" AutoPostBack="True" OnSelectedIndexChanged="ddlTrilhaNivel_OnSelectedIndexChanged">
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:Label ID="Label19" runat="server" Text="Buscar por Ponto Sebrae" AssociatedControlID="ddlPontoSebrae"></asp:Label>
                <asp:DropDownList ID="ddlPontoSebrae" runat="server" CssClass="form-control mostrarload" AutoPostBack="True" OnSelectedIndexChanged="ddlPontoSebrae_OnSelectedIndexChanged">
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:Label ID="Label20" runat="server" Text="Buscar por Missão" AssociatedControlID="ddlMissao"></asp:Label>
                <asp:DropDownList ID="ddlMissao" runat="server" CssClass="form-control">
                </asp:DropDownList>
            </div>

            <div class="form-group">
                <asp:Label ID="Label2" runat="server" Text="Buscar por nome" AssociatedControlID="txtNome"></asp:Label>
                <asp:TextBox ID="txtNome" runat="server" MaxLength="250" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="Label3" runat="server" Text="Buscar por ativos" AssociatedControlID="rblAtivo"></asp:Label>
                <asp:RadioButtonList ID="rblAtivo" runat="server" RepeatDirection="Horizontal"
                            CssClass="form-control">
                    <asp:ListItem Text="Sim" Value="S"></asp:ListItem>
                    <asp:ListItem Text="Não" Value="N"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div class="form-group">
                
                <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click" CssClass="btn btn-primary mostrarload" />
                <asp:Button ID="btnNovo" runat="server" Text="Criar novo" OnClick="btnNovo_Click" CssClass="btn btn-default mostrarload" />
            </div>
        </fieldset>
    </div>

    <asp:Panel ID="pnlItemTrilha" runat="server" Visible="false">

        <hr />
        <h4>Resultado da Busca</h4>

        <asp:GridView ID="dgvItemTrilha" CssClass="table col-sm-12" runat="server" OnRowCommand="dgvItemTrilha_RowCommand" AutoGenerateColumns="false" EnableModelValidation="True" GridLines="None">
            <Columns>
                <asp:TemplateField HeaderText="Trilha">
                    <ItemTemplate>
                        <%#Eval("Missao.PontoSebrae.TrilhaNivel.Trilha.Nome")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Nível">
                    <ItemTemplate>
                        <%#Eval("Missao.PontoSebrae.TrilhaNivel.Nome") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ponto Sebrae">
                    <ItemTemplate>
                        <%#Eval("Missao.PontoSebrae.Nome") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Nome do Item">
                    <ItemTemplate>
                        <%#Eval("Nome") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Forma de Aquisição">
                    <ItemTemplate>
                        <%#Eval("FormaAquisicao.Nome") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ativo">
                    <ItemTemplate>
                        <%#Eval("AtivoSimNao")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Auto Indicativa">
                    <ItemTemplate>
                        <%#Eval("UsuarioAssociadoSimNao")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False" ItemStyle-Width="90px">
                    <ItemStyle HorizontalAlign="center" />
                    <ItemTemplate>
                        <asp:LinkButton ID="lkbEditar" runat="server" CausesValidation="False" CommandName="editar" CommandArgument='<%# Eval("ID")%>' ToolTip="Editar" CssClass="mostrarload">
                            <span class="btn btn-default btn-xs">
								<span class="glyphicon glyphicon-pencil"></span>
							</span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lkbExcluir" runat="server" CausesValidation="False" CommandName="excluir" CommandArgument='<%# Eval("ID")%>' ToolTip="Excluir" OnClientClick="return ConfirmarExclusao();">
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
