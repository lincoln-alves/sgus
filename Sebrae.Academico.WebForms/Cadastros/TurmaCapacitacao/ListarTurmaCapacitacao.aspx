<%@ Page Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" 
CodeBehind="ListarTurmaCapacitacao.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.TurmaCapacitacao.ListarTurmaCapacitacao" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <fieldset>
            <div class="form-group">
                <asp:Label ID="Label2" runat="server" Text="Nome" AssociatedControlID="txtNome"></asp:Label>
                <asp:TextBox ID="txtNome" runat="server" CssClass="form-control" ></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="Label1" runat="server" Text="Programa" AssociatedControlID="ddlPrograma"></asp:Label>
                <asp:DropDownList ID="ddlPrograma" runat="server" CssClass="form-control" AutoPostBack="true" 
                OnSelectedIndexChanged="ddlPrograma_OnSelectedIndexChanged"></asp:DropDownList>
            </div>
            <div class="form-group"> 
                <asp:Label ID="Label3" runat="server" Text="Oferta" AssociatedControlID="ddlCapacitacao"></asp:Label>
                <asp:DropDownList ID="ddlCapacitacao" runat="server" CssClass="form-control" Enabled="false" ></asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click"  CssClass="btn btn-primary mostrarload" />
                <asp:Button ID="btnNovo" runat="server" Text="Criar novo"  OnClick="btnNovo_Click" CssClass="btn btn-default mostrarload" />
            </div>
        </fieldset>
    </div>

    <asp:Panel ID="pnlTurmaCapacitacao" runat="server" Visible="false">

    
    <h4>Resultado da Busca</h4>
        <asp:Literal ID="litTable" runat="server" />
        <asp:GridView ID="gvTurmaCapacitacao" runat="server" OnRowCommand="dgvTurmaCapacitacao_RowCommand" CssClass="table col-sm-12" AutoGenerateColumns="false" GridLines="None">
            <Columns>
                <asp:TemplateField HeaderText="Turma">
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
                        <asp:LinkButton ID="lkbDuplicar" runat="server" CausesValidation="False" CommandName="duplicar" CommandArgument='<%# Eval("ID")%>' ToolTip="Duplicar">
                            <span class="btn btn-default btn-xs">
								<span class="glyphicon glyphicon-arrow-up"></span>
							</span>
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>

    <!-- MODAL -->
    <asp:Panel ID="pnlModal" runat="server" Visible="false">
        <div aria-hidden="false" aria-labelledby="myModalLabel" role="dialog" tabindex="-1"
            id="myModal" class="modal fade in" style="display: block;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true" runat="server"
                            onserverclick="OcultarModal_Click" id="btnFecharModal">
                            &times;</button>
                        <h4 class="modal-title">Duplicar Turma</h4>
                    </div>
                    <div class="modal-body">
                        <div class="panel-body">
                            <asp:HiddenField ID="hdIndexOfIdTurma" runat="server" />
                            <fieldset>
                                <div class="form-group">
                                    <asp:Label ID="lblTxtNome" runat="server" Text="Nome:" AssociatedControlID="txtNomeTurmaDuplicar" />
                                    <asp:TextBox ID="txtNomeTurmaDuplicar" runat="server" CssClass="form-control" MaxLength="250"></asp:TextBox>
                                </div>
                            </fieldset>
                        </div>
                        <div>
                            <asp:Button ID="btnEnviar" runat="server" Text="Enviar" OnClick="btnEnviar_Click"
                                CausesValidation="true" CssClass="btn btn-default mostrarload" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <!-- /.modal -->
</asp:Content>