<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    MaintainScrollPositionOnPostback="true" CodeBehind="EdicaoCategoriaConteudo.aspx.cs"
    Inherits="Sebrae.Academico.WebForms.Cadastros.CategoriaConteudo.EdicaoCategoriaConteudo" %>

<%@ Register Src="~/UserControls/ucPermissoes.ascx" TagName="ucPermissoes" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucUF.ascx" TagName="ucUF" TagPrefix="uc" %>
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
                            <asp:Label ID="Label4" runat="server" Text="Categoria Pai" AssociatedControlID="ddlCategoriaConteudoPai" ></asp:Label>
                            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip1" Chave="categoriaPai" />
                            <asp:DropDownList ID="ddlCategoriaConteudoPai" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlCategoriaConteudoPai_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label2" runat="server" Text="Nome" AssociatedControlID="txtNome" ></asp:Label>
                            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip2" Chave="nomeExibicao" />
                            <asp:TextBox ID="txtNome" runat="server" MaxLength="250" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label3" runat="server" Text="Descrição" AssociatedControlID="txtDescricao" />
                            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip3" Chave="txtDescricao" />
                            <asp:TextBox ID="txtDescricao" runat="server" CssClass="form-control" MaxLength="1024" TextMode="MultiLine"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label12" runat="server" Text="Texto de Apresentação Portal" AssociatedControlID="txtTextoApresentacao" />
                            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip4" Chave="textoExibicao" />
                            <asp:TextBox ID="txtTextoApresentacao" runat="server" TextMode="MultiLine" Columns="9"
                                Rows="10" CssClass="form-control ckeditor"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lbl" runat="server" Text="Termo de Aceite e Política de Consequência" AssociatedControlID="ddlTermoAceite" ></asp:Label>
                            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip5" Chave="ddlTermoAceite" />
                            <asp:DropDownList ID="ddlTermoAceite" runat="server" CssClass="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label1" runat="server" Text="Node Drupal" AssociatedControlID="txtIdNode" ></asp:Label>
                            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip6" Chave="idDrupal" />
                            <asp:TextBox ID="txtIdNode" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label5" runat="server" Text="Sigla" AssociatedControlID="txtSigla"></asp:Label>
                            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip7" Chave="txtSigla" />
                            <asp:TextBox ID="txtSigla" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:CheckBox Text="&nbsp;Ignorar regras de inscrição?" ID="chkLiberarValidacao" runat="server" Visible="true" />
                            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip8" Chave="chkLiberarValidacao" />
                        </div>

                        <div class="form-group" id="divPossuiGerenciamentoStatus" runat="server">
                            <asp:Label ID="Label6" runat="server" Text="Status" AssociatedControlID="ckbPossuiGerenciamentoStatus" ></asp:Label>
                            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip10" Chave="gerenciamentoStatus" />
                            <br />
                            <asp:CheckBox Text="&nbsp;Permitir gerenciamento de Status da Turma" ID="ckbPossuiGerenciamentoStatus" runat="server" />
                        </div>

                        <div class="form-group" id="divPossuiGerenciamentoAreas" runat="server">
                            <asp:Label ID="Label7" runat="server" Text="Áreas" AssociatedControlID="ckbPossuiGerenciamentoAreas" ></asp:Label>
                            <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip12" Chave="gerenciamentoAreas" />
                            <br />
                            <asp:CheckBox Text="&nbsp;Permitir gerenciamento de Áreas/Subáreas" ID="ckbPossuiGerenciamentoAreas" runat="server" />
                        </div>

                    </fieldset>
                </div>
            </div>
        </div>

        <div class="panel panel-default">
            <div class="panel-heading">
                <a data-toggle="collapse" data-parent="#accordion" href="#collapse4">Ufs da Categoria
                </a>
            </div>
            <div id="collapse4" class="panel-collapse collapse">
                <div class="panel-body">
                    <uc:ucUF ID="ucUF1" runat="server" />
                </div>
            </div>
        </div>

        <div class="panel panel-default">
            <div class="panel-heading">
                <a data-toggle="collapse" data-parent="#accordion" href="#collapse2">Status relacionados </a>
                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip14" Chave="statusSelecionado" />
            </div>
            <div id="collapse2" class="panel-collapse collapse">
                <div class="panel-body">
                    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <label id="titleCampos" runat="server" clientidmode="Static"></label>
                            <asp:CheckBoxList ID="cbStatusSelecionados" runat="server"></asp:CheckBoxList>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

        <div class="panel panel-default" runat="server" Visible="False">
            <div class="panel-heading">
                <a data-toggle="collapse" data-parent="#accordion" href="#collapseTags">Tags</a>
            </div>
            <div id="collapseTags" class="panel-collapse collapse">
                <div class="panel-body">
                    <uc:Tags ID="ucTags1" runat="server" />
                </div>
            </div>
        </div>

        <div class="panel panel-default">
            <div class="panel-heading">
                <a data-toggle="collapse" data-parent="#accordion" href="#ucPermissoes">Permissões
                </a>
            </div>
            <div id="ucPermissoes" class="panel-collapse collapse">
                <div class="panel-body">
                    <uc1:ucPermissoes ID="ucPermissoes1" runat="server" />
                </div>
            </div>
        </div>
    </div>
    <br />
    <div>
        <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
            CssClass="btn btn-primary mostrarload" />
        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click"
            CssClass="btn btn-default" />
    </div>
    <script type="text/javascript">
        $(document).ready(function() {
            // Botões de marcar e desmarcar.
            $.markAll('titleCampos', '<%= cbStatusSelecionados.ClientID %>', 'Marcar todos', 'Desmarcar todos');
        });
    </script>
</asp:Content>
