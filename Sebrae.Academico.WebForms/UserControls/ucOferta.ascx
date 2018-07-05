<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucOferta.ascx.cs" Inherits="Sebrae.Academico.WebForms.UserControls.ucOferta" %>
<%@ Register Src="~/UserControls/ucPermissoes.ascx" TagName="ucPermissoes" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>

<script type="text/javascript">
    var _preloadedList = <%= ViewState["_SE"] %>;
    AutoCompleteDefine(_preloadedList, '#txtSolucaoEducacional', true, true, false);
</script>

<div class="panel-group" id="accordionOF">
    <div class="panel panel-default">
        <div class="panel-heading">
            <a data-toggle="collapse" data-parent="#accordionOF" href="#collapse100" runat="server" id="spanAcao">Dados </a>
        </div>
        <div id="collapse100" class="panel-collapse collapse in">
            <div class="panel-body">
                <fieldset>
                    <div class="form-group" id="divTxtCodigo" runat="server">
                        <asp:Label ID="Label23" runat="server" Text="Código" AssociatedControlID="txtCodigo" />
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip1" Chave="codigoSequencialOferta" />
                        <asp:TextBox ID="txtCodigo" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                    </div>
                    <div class="form-group" runat="server" id="divNome" visible="true">
                        <asp:Label ID="Label3" runat="server" Text="Nome*" AssociatedControlID="txtNome" />
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip2" Chave="nome" />
                        <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label2" runat="server" Text="Solução Educacional*" AssociatedControlID="txtSolucaoEducacional" />
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip3" Chave="cadastroSolucaoEducacional" />
                        <asp:TextBox ID="txtSolucaoEducacional" runat="server" CssClass="form-control" ClientIDMode="Static" OnTextChanged="txtSolucaoEducacional_OnTextChanged"></asp:TextBox>
                    </div>
                    <asp:UpdatePanel ID="updatePanel" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="panelAlertaIDChaveExterna" runat="server" CssClass="form-group" Visible="false">
                                <div class="alert alert-danger">
                                    A chave externa <strong>
                                        <asp:Label ID="labelChaveExternaError" runat="server"></asp:Label></strong> da solução educacional <strong>
                                            <asp:Label ID="labelSolucaoEducacionalError" runat="server"></asp:Label></strong> é inválida.
                                </div>
                            </asp:Panel>
                            <div class="form-group" id="divChaveExterna" runat="server">
                                <asp:Label ID="Label6" runat="server" Text="ID da Chave Externa" AssociatedControlID="txtIDChaveExterna" />
                                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip4" Chave="chaveExterna" />
                                <asp:TextBox ID="txtIDChaveExterna" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group" id="divCodigoMoodle" runat="server">
                                <asp:Label ID="Label17" runat="server" Text="Código Moodle" AssociatedControlID="txtCodigoMoodle" />
                                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip5" Chave="idMoodle" />
                                <asp:TextBox ID="txtCodigoMoodle" runat="server" CssClass="form-control" MaxLength="5" onkeypress="return EhNumerico(event)"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lblLink" runat="server" Text="Link" AssociatedControlID="txtLink" />
                                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip7" Chave="txtLinkSolucaoEducacional" />
                                <asp:TextBox ID="txtLink" runat="server" CssClass="form-control" MaxLength="1024"></asp:TextBox>
                            </div>
                            <asp:Panel ID="panelAlertaCodigoMoodle" runat="server" CssClass="form-group" Visible="false">
                                <div class="alert alert-danger">
                                    <strong>Atenção!</strong> Para exibir os cursos do Moodle, insira um valor no campo <strong>CÓDIGO MOODLE</strong> na solução educacional acima.
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="panelCursosMoodle" runat="server" CssClass="form-group" Visible="false">
                                <asp:Label ID="Label24" runat="server" Text="CURSOS DO MOODLE" AssociatedControlID="ddlCursosMoodle" />
                                <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip6" Chave="cursosMoodle" />
                                <asp:DropDownList ID="ddlCursosMoodle" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlCursosMoodle_SelectedIndexChanged"></asp:DropDownList>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="form-group">
                        <asp:Label ID="Label5" runat="server" Text="Certificado do Aluno" AssociatedControlID="ddlCertificado" />
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip8" Chave="cadastroTemplatesCertificados" />
                        <asp:DropDownList ID="ddlCertificado" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label12" runat="server" Text="Declaração/certificado do professor" AssociatedControlID="ddlCertificadoProfessor" />
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip9" Chave="cadastroTemplatesCertificadosProfessor" />
                        <asp:DropDownList ID="ddlCertificadoProfessor" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label7" runat="server" Text="E-mail do Responsável" AssociatedControlID="txtEmailResponsavel" />
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip10" Chave="txtEmailResponsavel" />
                        <asp:TextBox ID="txtEmailResponsavel" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label8" runat="server" Text="Permitir Fila de Espera*" AssociatedControlID="rblFilaDeEspera" />
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip11" Chave="permitirFila" />
                        <asp:RadioButtonList ID="rblFilaDeEspera" runat="server" RepeatDirection="Horizontal"
                            CssClass="form-control" />
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label4" runat="server" Text="Tipo*" AssociatedControlID="ddlTipo" />
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip12" Chave="ofertaTipo" />
                        <asp:DropDownList ID="ddlTipo" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlTipo_SelectedIndexChanged" AutoPostBack="True">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group" runat="server" ID="divDiasPrazo" Visible="False">
                        <asp:Label ID="Label18" runat="server" Text="Prazo para realização (dias)*" AssociatedControlID="txtDiasPrazo" />
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip25" Chave="prazoRealizacao" />
                        <asp:TextBox ClientIDMode="Static" ID="txtDiasPrazo" runat="server" CssClass="form-control" MaxLength="4" onkeypress="return EhNumerico(event)"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label11" runat="server" Text="Data de Início das Inscrições*" AssociatedControlID="txtDtInicioInscricoes" />
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip15" Chave="dataInicioInscricoes" />
                        <asp:TextBox ID="txtDtInicioInscricoes" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="lblDtFimInscricoes" runat="server" Text="Data Fim das Inscrições*" AssociatedControlID="txtDtFimInscricoes" />
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip16" Chave="dataFimInscricoes" />
                        <asp:TextBox ID="txtDtFimInscricoes" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label13" runat="server" Text="Permitir inscrição on-line*" AssociatedControlID="rblInscricaoOnLine" />
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip17" Chave="permitirInscricoesOnline" />
                        <asp:RadioButtonList ID="rblInscricaoOnLine" runat="server" RepeatDirection="Horizontal"
                            CssClass="form-control" />
                    </div>
                    <div class="form-group" runat="server" ID="divPermitirInscricaoGestor">
                        <asp:Label ID="Label15" runat="server" Text="Permitir inscrição pelos Gestores*" AssociatedControlID="rblGestorUC" />
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip18" Chave="rblGestorUC" />
                        <asp:RadioButtonList ID="rblGestorUC" runat="server" RepeatDirection="Horizontal"
                            CssClass="form-control" />
                    </div>
                    <div class="form-group" runat="server" ID="divPermitirAlteracaoGestor">
                        <asp:Label ID="Label19" runat="server" Text="Permitir alteração pelos Gestores*" AssociatedControlID="rblGestorUC" />
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip19" Chave="rblAlteracaoPeloGestor" />
                        <asp:RadioButtonList ID="rblAlteracaoPeloGestor" runat="server" RepeatDirection="Horizontal"
                            CssClass="form-control" />
                    </div>
                    <div class="form-group" runat="server" ID="divPermitirCadastroGestor">
                        <asp:Label ID="Label21" runat="server" Text="Permitir cadastro de turma pelos Gestores" AssociatedControlID="rbl_PermiteCadastroTurmaPeloGestorUC" />
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip20" Chave="rbl_PermiteCadastroTurmaPeloGestorUC" />
                        <asp:RadioButtonList ID="rbl_PermiteCadastroTurmaPeloGestorUC" runat="server" RepeatDirection="Horizontal"
                            CssClass="form-control" />
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label14" runat="server" Text="Carga Horária*" AssociatedControlID="txtCargaHoraria" />
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip21" Chave="cargaHoraria" />
                        <asp:TextBox ID="txtCargaHoraria" runat="server" CssClass="form-control" MaxLength="4" onkeypress="return EhNumerico(event)"></asp:TextBox>
                    </div>
                    <div class="form-group" runat="server" ID="divValorPrevisto">
                        <asp:Label ID="Label22" runat="server" Text="Valor Previsto" AssociatedControlID="txtValorPrevisto" />
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip22" Chave="valorPrevisto" />
                        <asp:TextBox ID="txtValorPrevisto" runat="server" CssClass="form-control" MaxLength="11"></asp:TextBox>
                    </div>
                    <div class="form-group" runat="server" ID="divValorRealizado">
                        <asp:Label ID="Label25" runat="server" Text="Valor Realizado" AssociatedControlID="txtValorRealizado" />
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip23" Chave="valorRealizado" />
                        <asp:TextBox ID="txtValorRealizado" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label16" runat="server" Text="Quantidade máxima de Inscrições*" AssociatedControlID="txtQtdMaxInscricoes" />
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip24" Chave="txtQtdMaxInscricoes" />
                        <asp:TextBox ID="txtQtdMaxInscricoes" runat="server" CssClass="form-control" MaxLength="4" onkeypress="return EhNumerico(event)"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label20" runat="server" Text="Oferta Trancada Para Pagantes" AssociatedControlID="txtDiasPrazo" />
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip26" Chave="ofertaTrancadaParaPagantes" />
                        <asp:CheckBoxList ID="cblOfertaTrancadaParaPagantes" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="20" Text="ALI"></asp:ListItem>
                            <asp:ListItem Value="19" Text="AOE"></asp:ListItem>
                            <asp:ListItem Value="21" Text="CREDENCIADO"></asp:ListItem>
                            <asp:ListItem Value="4" Text="PARCEIRO"></asp:ListItem>
                            <asp:ListItem Value="22" Text="Funcionário Temporário"></asp:ListItem>
                        </asp:CheckBoxList>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label1" runat="server" Text="Informação Adicional" AssociatedControlID="txtInformacaoAdicional" />
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip27" Chave="infoAddCadastrOferta" />
                        <CKEditor:CKEditorControl ID="txtInformacaoAdicional" BasePath="/js/ckeditor/" runat="server"></CKEditor:CKEditorControl>
                    </div>
                </fieldset>
            </div>
        </div>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            <a data-toggle="collapse" data-parent="#accordionOF" href="#collapsePublicoAlvo">Publico Alvo
            </a>
        </div>
        <div id="collapsePublicoAlvo" class="panel-collapse collapse">
            <div class="panel-body">
                <div class="form-group clearfix">
                    <label id="titlePublicoAlvo" runat="server" ClientIDMode="Static">
                        Público Alvo -
                    </label>
                    <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip28" Chave="titlePublicoAlvo" />
                    <asp:CheckBoxList ID="ckblstPublicoAlvo" runat="server" RepeatDirection="Vertical"
                        RepeatLayout="UnorderedList" ClientIDMode="Static">
                    </asp:CheckBoxList>
                </div>
            </div>
        </div>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            <a data-toggle="collapse" data-parent="#accordionOF" href="#collapse111">Permissões
            </a>
        </div>
        <div id="collapse111" class="panel-collapse collapse">
            <div class="panel-body">
                <uc2:ucPermissoes ID="ucPermissoes2" runat="server" />
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function pad2(number) {
            return (number < 10 ? '0' : '') + number;
        }
        function validHour(value) {
            var comp = value.split(':');
            var h = parseInt(comp[0], 10);
            var m = parseInt(comp[1], 10);
            var s = parseInt(comp[2], 10);
            if (h > 23) h = 23;
            if (m > 59) m = 59;
            if (s > 59) s = 59;
            return pad2(h) + ':' + pad2(m) + ':' + pad2(s);
        }
        function validDate(value) {
            var comp = value.split('/');
            var d = parseInt(comp[0], 10);
            var m = parseInt(comp[1], 10);
            var y = parseInt(comp[2], 10);
            var date = new Date(y, m - 1, d);
            return {
                result: (date.getFullYear() === y && date.getMonth() + 1 === m && date.getDate() === d),
                value: value
            }
        }

        function calculaDiferenciaDias(dtInicio, dtFim) {
            try {
                var d = (dtInicio.val().split(' ')[0]).split('/');
                var df = (dtFim.val().split(' ')[0]).split('/');
                var date1 = new Date(d[1]+"/"+d[0]+"/"+d[2]);
                var date2 = new Date(df[1] + "/" + df[0] + "/" + df[2]);
                var timeDiff = Math.abs(date2.getTime() - date1.getTime());
                return Math.ceil(timeDiff / (1000 * 3600 * 24));
            } catch (ex0) {
                return null;
            }
        }
        (function ($) {
            $(document).ready(function() {
                $.markAll('titlePublicoAlvo', '<%= ckblstPublicoAlvo.ClientID %>', 'Marcar todos', 'Desmarcar todos');
            });

            var dtInicioInscricao = $("#<%= txtDtInicioInscricoes.ClientID %>");
            var dtFimInscricao = $("#<%= txtDtFimInscricoes.ClientID %>");
            var txtDiasPrazo = $('#txtDiasPrazo');

            //dtInicio.mask("99/99/9999 99:99:99", {
            //    autoclear: false
            //}).on('blur', function () {
            //    var valor = dtInicio.val();
            //    var dados = valor.split(' ');
            //    var data = validDate(dados[0]);
            //    if (!data.result) {
            //        dtInicio.val('');
            //    } else {
            //        dados[1] = dados[1].replace(/_/g, "0");
            //        dados[1] = validHour(dados[1]);
            //        dtInicio.val(dados.join(' '));
            //    }
            //    var dias = calculaDiferenciaDias(dtInicio, dtFim);
            //    if(dias !== null)txtDiasPrazo.val(dias);
            //});
            //dtFim.mask("99/99/9999 99:99:99", {
            //    autoclear: false
            //}).on('blur', function () {
            //    var valor = dtFim.val();
            //    var dados = valor.split(' ');
            //    var data = validDate(dados[0]);
            //    if (!data.result) {
            //        dtFim.val('');
            //    } else {
            //        dados[1] = dados[1].replace(/_/g, "9");
            //        dados[1] = validHour(dados[1]);
            //        dtFim.val(dados.join(' '));
            //    }
            //    var dias = calculaDiferenciaDias(dtInicio, dtFim);
            //    if (dias !== null) txtDiasPrazo.val(dias);
            //});

            dtInicioInscricao.mask("99/99/9999 99:99:99", {
                autoclear: false
            }).on('blur', function () {
                var valor = dtInicioInscricao.val();
                var dados = valor.split(' ');
                var data = validDate(dados[0]);
                if (!data.result) {
                    dtInicioInscricao.val('');
                } else {
                    dados[1] = dados[1].replace(/_/g, "0");
                    dados[1] = validHour(dados[1]);
                    dtInicioInscricao.val(dados.join(' '));
                }
                //if (dtInicio.val() === '') {
                //    dtInicio.val(dtInicioInscricao.val());
                //    var dias = calculaDiferenciaDias(dtInicio, dtFim);
                //    if (dias !== null) txtDiasPrazo.val(dias);
                //}
            });
            dtFimInscricao.mask("99/99/9999 99:99:99", {
                autoclear: false
            }).on('blur', function () {
                var valor = dtFimInscricao.val();
                var dados = valor.split(' ');
                var data = validDate(dados[0]);
                if (!data.result) {
                    dtFimInscricao.val('');
                } else {
                    dados[1] = dados[1].replace(/_/g, "9");
                    dados[1] = validHour(dados[1]);
                    dtFimInscricao.val(dados.join(' '));
                }
                //if (dtFim.val() === '') {
                //    dtFim.val(dtFimInscricao.val());
                //    var dias = calculaDiferenciaDias(dtInicio, dtFim);
                //    if (dias !== null) txtDiasPrazo.val(dias);
                //}
            });
        })(jQuery);
    </script>
</div>
