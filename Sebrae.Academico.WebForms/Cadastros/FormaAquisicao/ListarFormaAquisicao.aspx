<%@ Page Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true"
    CodeBehind="ListarFormaAquisicao.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.ListarFormaAquisicao" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <fieldset>
            <div class="form-group">
                <asp:Label ID="Label2" runat="server" Text="Buscar por nome" AssociatedControlID="txtNome"></asp:Label>
                <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click" CssClass="btn btn-primary mostrarload" />
                <asp:Button ID="Button2" runat="server" Text="Criar nova" OnClick="btnNovo_Click" CssClass="btn btn-default mostrarload" />
            </div>
        </fieldset>
    </div>

    <hr />

    <asp:Panel ID="pnlFormaAquisicao" runat="server" Visible="false">
    
        <h4>Resultado da Busca</h4>
        <asp:GridView ID="dgvFormaAquisicao" runat="server" CssClass="table col-sm-12" GridLines="None" OnRowCommand="dgvFormaAquisicao_RowCommand" AutoGenerateColumns="false" EnableModelValidation="True" OnRowDataBound="dgvFormaAquisicao_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="Forma de Aquisição">
                    <ItemTemplate>
                        <%#Eval("Nome") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tipo">
                    <ItemTemplate>
                        <%#Eval("TipoFormaDeAquisicao") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Imagem" HeaderStyle-HorizontalAlign="center">
                    <ItemStyle HorizontalAlign="center"/>
                    <ItemTemplate>
                        <asp:Image runat="server" ID="imgImagem" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Enviar Para o Portal?" >
                    <ItemStyle Width="110px" HorizontalAlign="center"/>
                    <ItemTemplate>
                        <%#Eval("EnviarPortalFormatado") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Carga Horária" >
                    <ItemStyle Width="110px" HorizontalAlign="center"/>
                    <ItemTemplate>
                        <%#Eval("CargaHorariaFormatada") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="UF">
                    <ItemStyle HorizontalAlign="center"/>
                    <ItemTemplate>
                        <%# Eval("Uf") != null ? Eval("Uf.Nome") : "" %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <ItemStyle Width="110px" HorizontalAlign="center"/>
                    <ItemTemplate>
                        <asp:LinkButton ID="lkbVisualizar" runat="server" CausesValidation="False" CommandName="visualizar"
                            CommandArgument='<%# Eval("ID")%>' ToolTip="Visualizar">
                                <span class="btn btn-default btn-xs">
								    <span class="glyphicon glyphicon-search"></span>
							    </span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lkbCopiar" runat="server" CausesValidation="False" CommandName="copiar"
                            CommandArgument='<%# Eval("ID")%>' ToolTip="Copiar">
                                <span class="btn btn-default btn-xs">
								    <span class="glyphicon glyphicon-edit"></span>
							    </span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lkbEditar" id_uf_gestor='<%# Eval("Uf") != null ? Eval("Uf.ID") : ""%>' runat="server" CausesValidation="False" CommandName="editar" CommandArgument='<%# Eval("ID")%>' ToolTip="Editar"
                            id_responsavel='<%# Eval("Responsavel") != null ? Eval("Responsavel.ID") : "" %>' 
                            OnPreRender="lkbBotoesAcesso_PreRender">
                            <span class="btn btn-default btn-xs">
								<span class="glyphicon glyphicon-pencil"></span>
							</span>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lkbExcluir" id_uf_gestor='<%# Eval("Uf") != null ? Eval("Uf.ID") : ""%>' 
                            id_responsavel='<%# Eval("Responsavel") != null ? Eval("Responsavel.ID") : "" %>' 
                            runat="server" CausesValidation="False" CommandName="excluir" CommandArgument='<%# Eval("ID")%>' ToolTip="Excluir"
                            OnPreRender="lkbBotoesAcesso_PreRender" OnClientClick="return ConfirmarExclusao();">
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
						<h4 class="modal-title">Visualização</h4>
					</div>
					<div class="modal-body">
                            <fieldset>
                                <div class="form-group">
                                    <asp:Label ID="lblNome" runat="server" Text="Nome" AssociatedControlID="txtNomeVisualizar" />
                                    <asp:TextBox ID="txtNomeVisualizar" runat="server" CssClass="form-control" MaxLength="250"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="Label1" runat="server" Text="Descrição" AssociatedControlID="txtDescricaoVisualizar" />
                                    <asp:TextBox ID="txtDescricaoVisualizar" runat="server" CssClass="form-control" MaxLength="1024" TextMode="MultiLine"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <asp:Label Text="Tipo" runat="server" AssociatedControlID="txtTipoFormaAquisicao" />
                                    <asp:TextBox ID="txtTipoFormaAquisicao" CssClass="form-control" runat="server"/>
                                </div>
                                <div class="form-group">
                                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="OcultarModal_Click" CssClass="btn btn-default" />
                                </div>
                            </fieldset>
					</div>
				</div>
			</div>
		</div>
	</asp:Panel>
	<!-- FIM - MODAL -->
</asp:Content>
