<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="EdicaoFaq.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.Faq.EdicaoFaq" %>

<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>

        <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
            <div class="panel panel-default">
                <div class="panel-heading" role="tab" id="headingOne">
                     <asp:LinkButton runat="server" ClientIDMode="Static" ID="lbkAssuntos" OnClick="lbkAssuntos_Click"
                        Text="Gerenciar Assuntos"></asp:LinkButton>
                </div>
                <div id="collapseAlunos" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne" runat="server">
                    <div class="panel-body">
                        <asp:HiddenField runat="server" ID="hdAssuntoEdicao" />
                        <div class="form-group">
                            <asp:Label Text="Assunto" AssociatedControlID="txtAssuntoTrilhas" runat="server"></asp:Label>
                            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip8" Chave="txtAssuntoTrilhas" />
                            <asp:TextBox ID="txtAssuntoTrilhas" runat="server" Columns="3" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Button Text="Enviar" ID="btnEnviarAssunto" OnClick="btnEnviarAssunto_Click" runat="server" CssClass="btn btn-primary" />
                        </div>

                        <asp:GridView runat="server" ID="dvgAssuntos" CssClass="table col-sm-12" GridLines="None" AutoGenerateColumns="false" EnableModelValidation="true" Visible="true"
                            OnRowCommand="dvgAssuntos_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="Título">
                                    <ItemTemplate>
                                        <%# Eval("Nome") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="false">
                                    <ItemStyle Width="110px" HorizontalAlign="center" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lkbEditar" runat="server" CausesValidation="False" CommandName="editar" CommandArgument='<%# Eval("ID")%>' ToolTip="Editar">
                        <span class="btn btn-default btn-xs">
                            <span class="glyphicon glyphicon-pencil"></span>
                        </span>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="lkbExcluir" runat="server" CausesValidation="False" CommandName="excluir" CommandArgument='<%# Eval("ID")%>' ToolTip="Excluir"
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
                    </div>
                </div>
            </div>
        </div>

        <div class="form-group">
            <asp:Label Text="Assunto" runat="server" AssociatedControlID="ddlAssunto" />
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip3" Chave="ddlAssunto" />
            <asp:DropDownList ID="ddlAssunto" runat="server" CssClass="form-control"></asp:DropDownList>
        </div>

        <div class="form-group">
            <asp:Label Text="Nome" AssociatedControlID="txtNome" runat="server"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip1" Chave="txtNome" />
            <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label Text="Descrição" runat="server" AssociatedControlID="txtDescricao" />
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip2" Chave="txtDescricao" />
            <asp:TextBox ID="txtDescricao" runat="server" TextMode="MultiLine" CssClass="form-control ckeditor"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Button ID="btnSalvar" runat="server" Text="Salvar" CssClass="btn btn-primary" OnClick="btnSalvar_Click" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-default" OnClick="btnCancelar_Click" />
        </div>
    </fieldset>
    <script type="text/javascript">
        validarForm();
    </script>
</asp:Content>
