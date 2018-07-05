<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="EdicaoSistemaExterno.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.EdicaoSistemaExterno" %>

<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-group" id="accordion">
        <div class="panel panel-default">
            <div class="panel-heading">
                <a data-toggle="collapse" data-parent="#accordion" href="#collapse1">Dados </a>
            </div>
            <div id="collapse1" class="panel-collapse collapse in">
                <div class="panel-body">
                    <fieldset>
                        <div class="form-group">
                            <asp:Label ID="Label2" runat="server" Text="Nome" AssociatedControlID="txtNome" />
                            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip1" Chave="nome" />
                            <asp:TextBox ID="txtNome" runat="server" CssClass="form-control" MaxLength="200"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label3" runat="server" AssociatedControlID="txtLink" Text="Link de Acesso" />
                            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip2" Chave="sistemaExternoLink" />
                            <asp:TextBox ID="txtLink" runat="server" CssClass="form-control" MaxLength="200"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblDescricao" runat="server" AssociatedControlID="txtDescricao" Text="Descrição" />
                            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip3" Chave="txtDescricao" />
                            <CKEditor:CKEditorControl ID="txtDescricao" BasePath="/js/ckeditor/" runat="server"></CKEditor:CKEditorControl>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label15" runat="server" Text="Público" AssociatedControlID="rblPublico" />
                            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip4" Chave="rblPublico" />
                            <asp:RadioButtonList ID="rblPublico" runat="server" RepeatDirection="Horizontal" CssClass="form-control" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label1" runat="server" Text="English Town" AssociatedControlID="rblEnglishTown" />
                            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip5" Chave="rblEnglishTown" />
                            <asp:RadioButtonList ID="rblEnglishTown" runat="server" RepeatDirection="Horizontal" CssClass="form-control" />
                        </div>
                    </fieldset>
                </div>
            </div>
        </div>
        <div class="panel panel-default">
            <div class="panel-heading">
                <a data-toggle="collapse" data-parent="#accordion" href="#collapse3">Permissões
                </a>
            </div>
            <div id="collapse3" class="panel-collapse collapse">
                <div class="panel-body">

                    <fieldset>
                        <div class="form-group">
                            <uc:Permissoes ID="ucPermissoes" runat="server" />
                        </div>
                    </fieldset>
                </div>
            </div>
        </div>
    </div>

    <div class="form-group">
        <asp:Button ID="Button1" runat="server" Text="Salvar" OnClick="btnSalvar_Click" CssClass="btn btn-primary" />
        <asp:Button ID="Button2" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" CssClass="btn btn-default" />
    </div>

    <!-- Modal ModalAjudaLink-->
    <div class="modal fade" id="ModalAjudaLink" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="myModalLabel">Link de Acesso</h4>
                </div>
                <div class="modal-body">
                    <p>Indica como vai ser montado o link de acesso para os fornecedores. Hastags permitidas:</p>
                    <ul>
                        <li><b>#CPF</b> - CPF do usuário</li>
                        <li><b>#TOKEN</b> - CPF do usuário criptografado de acordo com a chave do Fornecedor</li>
                        <li><b>#SENHAMD5</b> - Senha do usuário no formato MD5</li>
                    </ul>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
