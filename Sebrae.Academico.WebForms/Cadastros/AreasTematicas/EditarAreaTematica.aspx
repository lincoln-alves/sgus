<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="EditarAreaTematica.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.AreasTematicas.EditarAreaTematica" %>
<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="/css/sebraeuc_fonts.css" rel="stylesheet" />
    <div>
        <div class="panel-group" id="accordion">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <a data-toggle="collapse" data-parent="#accordion" href="#collapse1">Dados </a>
                </div>
                <div id="collapse1" class="panel-collapse collapse in">
                    <div class="panel-body">
                        <fieldset>
                            <div class="form-group">
                                <asp:Label ID="Label1" runat="server" Text="Ícone" AssociatedControlID="hdClassFont" />
                                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip1" Chave="hdClassFont" />
                                <asp:HiddenField ID="hdClassFont" runat="server" Value="" ClientIDMode="Static"/>
                                <div class="dropdown">
                                  <button class="btn btn-default dropdown-toggle dropdown-toggle-icons" type="button" id="btnClassFont" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                                    <span class="nome">- Selecione -</span>
                                    <span class="caret caret-black"></span>
                                  </button>
                                  <ul class="dropdown-menu dropdown-menu-icons scrollable-menu" aria-labelledby="btnClassFont">
                                      <li><a href="javascript:void(0)" data-icon="icon-left-open-big"><span class="icone icon-left-open-big icone-fonte-20"></span> icon-left-open-big</a></li>
                                      <li><a href="javascript:void(0)" data-icon="icon-right-open-big"><span class="icone icon-right-open-big icone-fonte-20"></span> icon-right-open-big</a></li>
                                      <li><a href="javascript:void(0)" data-icon="icon-home"><span class="icone icon-home icone-fonte-20"></span> icon-home</a></li>
                                      <li><a href="javascript:void(0)" data-icon="icon-chart"><span class="icone icon-chart icone-fonte-20"></span> icon-chart</a></li>
                                      <li><a href="javascript:void(0)" data-icon="icon-shirtandtie"><span class="icone icon-shirtandtie icone-fonte-20"></span> icon-shirtandtie</a></li>
                                      <li><a href="javascript:void(0)" data-icon="icon-signture"><span class="icone icon-signture icone-fonte-20"></span> icon-signture</a></li>
                                      <li><a href="javascript:void(0)" data-icon="icon-atendimento"><span class="icone icon-atendimento icone-fonte-20"></span> icon-atendimento</a></li>
                                      <li><a href="javascript:void(0)" data-icon="icon-courthouse"><span class="icone icon-courthouse icone-fonte-20"></span> icon-courthouse</a></li>
                                      <li><a href="javascript:void(0)" data-icon="icon-lightbulb"><span class="icone icon-lightbulb icone-fonte-20"></span> icon-lightbulb</a></li>
                                      <li><a href="javascript:void(0)" data-icon="icon-cracha"><span class="icone icon-cracha icone-fonte-20"></span> icon-cracha</a></li>
                                      <li><a href="javascript:void(0)" data-icon="icon-business_card"><span class="icone icon-business_card icone-fonte-20"></span> icon-business_card</a></li>
                                      <li><a href="javascript:void(0)" data-icon="icon-suitecase"><span class="icone icon-suitecase icone-fonte-20"></span> icon-suitecase</a></li>
                                      <li><a href="javascript:void(0)" data-icon="icon-boy"><span class="icone icon-boy icone-fonte-20"></span> icon-boy</a></li>
                                      <li><a href="javascript:void(0)" data-icon="icon-sustentabilidade"><span class="icone icon-sustentabilidade icone-fonte-20"></span> icon-sustentabilidade</a></li>
                                      <li><a href="javascript:void(0)" data-icon="icon-megaphone"><span class="icone icon-megaphone icone-fonte-20"></span> icon-megaphone</a></li>
                                      <li><a href="javascript:void(0)" data-icon="icon-profile"><span class="icone icon-profile icone-fonte-20"></span> icon-profile</a></li>
                                      <li><a href="javascript:void(0)" data-icon="icon-note-check"><span class="icone icon-note-check icone-fonte-20"></span> icon-note-check</a></li>
                                      <li><a href="javascript:void(0)" data-icon="icon-graduation-cap"><span class="icone icon-graduation-cap icone-fonte-20"></span> icon-graduation-cap</a></li>
                                      <li><a href="javascript:void(0)" data-icon="icon-th-list"><span class="icone icon-th-list icone-fonte-20"></span> icon-th-list</a></li>
                                      <li><a href="javascript:void(0)" data-icon="icon-book-2"><span class="icone icon-book-2 icone-fonte-20"></span> icon-book-2</a></li>
                                      <li><a href="javascript:void(0)" data-icon="icon-megaphone-1"><span class="icone icon-megaphone-1 icone-fonte-20"></span> icon-megaphone-1</a></li>
                                      <li><a href="javascript:void(0)" data-icon="icon-graduation-cap-1"><span class="icone icon-graduation-cap-1 icone-fonte-20"></span> icon-graduation-cap-1</a></li>
                                      <li><a href="javascript:void(0)" data-icon="icon-medalha"><span class="icone icon-medalha icone-fonte-20"></span> icon-medalha</a></li>
                                      <li><a href="javascript:void(0)" data-icon="icon-certificate"><span class="icone icon-certificate icone-fonte-20"></span> icon-certificate</a></li>
                                      <li><a href="javascript:void(0)" data-icon="icon-th-list-1"><span class="icone icon-th-list-1 icone-fonte-20"></span> icon-th-list-1</a></li>
                                      <li><a href="javascript:void(0)" data-icon="icon-chartboard"><span class="icone icon-chartboard icone-fonte-20"></span> icon-chartboard</a></li>
                                      <li><a href="javascript:void(0)" data-icon="icon-worker"><span class="icone icon-worker icone-fonte-20"></span> icon-worker</a></li>
                                      <li><a href="javascript:void(0)" data-icon="icon-team"><span class="icone icon-team icone-fonte-20"></span> icon-team</a></li>
                                      <li><a href="javascript:void(0)" data-icon="icon-verified"><span class="icone icon-verified icone-fonte-20"></span> icon-verified</a></li>
                                      <li><a href="javascript:void(0)" data-icon="icon-shakeonit"><span class="icone icon-shakeonit icone-fonte-20"></span> icon-shakeonit</a></li>
                                      <li><a href="javascript:void(0)" data-icon="icon-books"><span class="icone icon-books icone-fonte-20"></span> icon-books</a></li>
                                      <li><a href="javascript:void(0)" data-icon="icon-pc"><span class="icone icon-pc icone-fonte-20"></span> icon-pc</a></li>
                                      <li><a href="javascript:void(0)" data-icon="icon-map"><span class="icone icon-map icone-fonte-20"></span> icon-map</a></li>
                                      <li><a href="javascript:void(0)" data-icon="icon-logout"><span class="icone icon-logout icone-fonte-20"></span> icon-logout</a></li>
                                      <li><a href="javascript:void(0)" data-icon="icon-monitor"><span class="icone icon-monitor icone-fonte-20"></span> icon-monitor</a></li>
                                      <li><a href="javascript:void(0)" data-icon="icon-monitor-ok"><span class="icone icon-monitor-ok icone-fonte-20"></span> icon-monitor-ok</a></li>
                                      <li><a href="javascript:void(0)" data-icon="icon-book"><span class="icone icon-book icone-fonte-20"></span> icon-book</a></li>
                                  </ul>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lblNome" runat="server" Text="Nome" AssociatedControlID="txtNome"></asp:Label>
                                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip2" Chave="txtNome" />
                                <asp:TextBox ID="txtNome" runat="server" MaxLength="250" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label12" runat="server" Text="Texto de Apresentação Portal" AssociatedControlID="txtTextoApresentacao" />
                                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip3" Chave="txtTextoApresentacao" />
                                <CKEditor:CKEditorControl ID="txtTextoApresentacao" BasePath="/js/ckeditor/" runat="server"></CKEditor:CKEditorControl>
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
                        <uc:Permissoes ID="ucPermissoes1" runat="server" />
                    </div>
                </div>
            </div>
        </div>
        <asp:Button ID="btnSalvar" CssClass="btn btn-primary" runat="server" Text="Salvar" OnClick="btnSalvar_Click" />
        <asp:Button ID="btnCancelar" CssClass="btn btn-default" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" />
    </div>
    <script type="text/javascript">
        (function ($) {
            if (!$) {
                console.log('no Jquery!');
                return;
            }
            $(document).ready(function () {
                var btn = $('#btnClassFont');
                var hdClassFont = $('#hdClassFont');
                $('.dropdown-menu-icons a').on('click', function () {
                    var item = $(this);
                    btn.html(item.html() + ' <span class="caret caret-black"></span>');
                    hdClassFont.val(item.attr('data-icon'));
                });
                if (hdClassFont.val() !== "") {
                    btn.html('<span class="icone ' + hdClassFont.val() + ' icone-fonte-20"></span> ' + hdClassFont.val() + ' <span class="caret caret-black"></span>');
                }
            });
        })(jQuery);
    </script>
</asp:Content>
