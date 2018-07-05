<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeBehind="EdicaoFornecedor.aspx.cs"
    Inherits="Sebrae.Academico.WebForms.Cadastros.EdicaoFornecedor" %>

<%@ Register TagPrefix="ucUF" TagName="UF" Src="~/UserControls/ucUF.ascx" %>
<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel runat="server" ID="pnlEdicaoFornecedor" DefaultButton="btnSalvar">
        <fieldset>
            <div class="form-group">
                <asp:Label ID="Label5" runat="server" Text="Nome" AssociatedControlID="txtNome" ></asp:Label>
                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip1" Chave="nome" />
                <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"
                    MaxLength="250" ToolTip="trwtwsdf &lt;span&gt;asdad&lt;/span&gt;"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="Label2" runat="server" Text="Login*" AssociatedControlID="txtLogin" ></asp:Label>
                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip2" Chave="login" />
                <asp:TextBox ID="txtLogin" runat="server" CssClass="form-control" AutoCompleteType="None"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="Label1" runat="server" Text="Senha" AssociatedControlID="txtSenha" ></asp:Label>
                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip3" Chave="senha" />
                <asp:Button ID="btnAlterarSenha" runat="server" Text="Alterar Senha" OnClick="btnAlterarSenha_Click" CssClass="btn btn-default form-control" />
                <asp:TextBox ID="txtSenha" runat="server" TextMode="Password" CssClass="form-control" Visible="false"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="Label4" runat="server" Text="Texto de Criptografia*" AssociatedControlID="txtTextoCriptografia" ></asp:Label>
                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip4" Chave="criptografia" />
                <asp:TextBox ID="txtTextoCriptografia" runat="server" CssClass="form-control"
                    MaxLength="32"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="Label3" runat="server" AssociatedControlID="txtLinkAcesso" >Link de Acesso</asp:Label>
                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip5" Chave="fornecedorLink" />
                <asp:TextBox ID="txtLinkAcesso" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="Label6" runat="server" Text="Permite gestão SGUS*" AssociatedControlID="rblInGestaoSgus" ></asp:Label>
                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip6" Chave="fornecedorPermiteGestao" />
                <asp:RadioButtonList ID="rblInGestaoSgus" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" CssClass="form-control"
                    OnSelectedIndexChanged="rblInGestaoSgus_SelectedIndexChanged">
                </asp:RadioButtonList>
            </div>
            <div class="form-group" runat="server" id="divInCriarOferta">
                <asp:Label ID="Label7" runat="server" Text="Permite criar oferta*" AssociatedControlID="rblInCriarOferta" ></asp:Label>
                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip7" Chave="fornecedorPermiteOferta" />
                <asp:RadioButtonList ID="rblInCriarOferta" runat="server" RepeatDirection="Horizontal" CssClass="form-control">
                </asp:RadioButtonList>
            </div>
            <div class="form-group" runat="server" id="divInCriarTurma">
                <asp:Label ID="Label8" runat="server" Text="Permite criar turma*" AssociatedControlID="rblInCriarTurma" ></asp:Label>
                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip8" Chave="fornecedorPermiteTurma" />
                <asp:RadioButtonList ID="rblInCriarTurma" runat="server" RepeatDirection="Horizontal" CssClass="form-control">
                </asp:RadioButtonList>
            </div>
            
            <div class="form-group" runat="server" id="div1">
                <asp:Label ID="Label9" runat="server" Text="Apresentar Fornecedor no Portal*" AssociatedControlID="rblInApresentarComoFornecedorNoPortal" ></asp:Label>
                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip9" Chave="apresentarComoFornecedorNoPortal" />
                <asp:RadioButtonList ID="rblInApresentarComoFornecedorNoPortal" runat="server" RepeatDirection="Horizontal" CssClass="form-control"  AutoPostBack="true" OnSelectedIndexChanged="rblInApresentarComoFornecedorNoPortal_SelectedIndexChanged">
                </asp:RadioButtonList>
            </div>
            
            <div class="form-group" runat="server" id="divNomeApresentacao" Visible="false">
                <asp:Label ID="Label10" runat="server" Text="Nome da Instituição para Apresentação no Portal*" AssociatedControlID="txtNomeApresentacao" ></asp:Label>
                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip10" Chave="nomeApresentacaoPortalFornecedor" />
                <asp:TextBox ID="txtNomeApresentacao" runat="server" CssClass="form-control"
                    MaxLength="500"></asp:TextBox>
            </div>

            <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
                <div class="panel panel-default">
                    <div class="panel-heading" role="tab" id="headingOne">
                        <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOne">Ufs Liberadas Para o Gestor</a>
                    </div>
                    <div id="collapseOne" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
                        <div class="panel-body">
                            <ucUF:UF ID="ucUF" runat="server" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="form-group">
                <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
                    CssClass="btn btn-primary mostrarload" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click"
                    CssClass="btn btn-default" />
            </div>
        </fieldset>
    </asp:Panel>

    <!-- Modal ModalAjudaLinkAcesso-->
    <div class="modal fade" id="ModalAjudaLinkAcesso" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
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