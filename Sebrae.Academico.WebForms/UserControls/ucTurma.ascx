<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucTurma.ascx.cs" Inherits="Sebrae.Academico.WebForms.UserControls.ucTurma" %>
<%@ Register Src="~/UserControls/ucSeletorListBox.ascx" TagName="ucSeletorListBox" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/ucLupaUsuario.ascx" TagName="ucLupaUsuario" TagPrefix="uc" %>
<%--<%= ddlSolucao.Enabled ? "<script type='text/javascript'>AutocompleteCombobox('" + ddlSolucao.ClientID + "', true);</script>" : "" %>--%>
<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>

<div>
    <fieldset>
        <div class="form-group" id="divAcao" runat="server">
            <h3 id="textoAcao" runat="server"></h3>
        </div>

        <asp:TextBox ID="txtIdTurma" runat="server" Visible="False"></asp:TextBox>

        <div class="form-group" id="divCodigoTurma" runat="server">
            <asp:Label ID="Label23" runat="server" Text="Código da Turma" AssociatedControlID="txtCodigo" />
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip1" Chave="codigoSequencialTurma" />
            <asp:TextBox ID="txtCodigo" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
        </div>
        <div class="form-group">
            <h3 id="H1" runat="server"></h3>
        </div>
        <div class="form-group">
            <asp:Label ID="LblSolucao" runat="server" AssociatedControlID="txtSolucao" Text="Solução educacional"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip2" Chave="cadastroSolucaoEducacional" />
            <asp:TextBox ID="txtSolucao" ClientIDMode="Static" runat="server" OnTextChanged="txtSolucao_OnTextChanged"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="LblOferta" runat="server" Text="Oferta" AssociatedControlID="txtOfertaTurma"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip3" Chave="cadastroOferta" />
            <asp:TextBox ID="txtOfertaTurma" ClientIDMode="Static" runat="server" OnTextChanged="txtOfertaTurma_OnTextChanged" data-mensagemVazia="Selecione uma Solução Educacional"></asp:TextBox>
        </div>
        <div class="form-group" id="divGruposMoodle" runat="server" visible="false">
            <asp:Label ID="lblGrupoMoodle" runat="server" Text="Grupo no Moodle" AssociatedControlID="ddlGruposMoodle" data-help="Grupo Existente no Moodle"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip4" Chave="ddlGruposMoodle" />
            <asp:DropDownList ID="ddlGruposMoodle" runat="server" CssClass="form-control">
            </asp:DropDownList>
        </div>
        <div class="form-group" runat="server" id="divNome" visible="true">
            <asp:Label ID="LblNomeTurma" runat="server" Text="Nome da turma *" AssociatedControlID="LblNomeTurma"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip5" Chave="cadastroTurma" />
            <asp:TextBox ID="txtNomeTurma" runat="server" MaxLength="250" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="LblTipoTutoria" runat="server" Text="Tipo de Tutoria" AssociatedControlID="ddlTipoTutoria"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip6" Chave="tipoTutoria" />
            <asp:DropDownList ID="ddlTipoTutoria" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlTipoTutoria_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem>- Selecione -</asp:ListItem>
                <asp:ListItem Value="Pró-Ativa">Proativa: presencial (professor)</asp:ListItem>
                <asp:ListItem Value="Reativa">Reativa: para instrutor</asp:ListItem>
                <asp:ListItem Value="Sem Tutoria">Sem Tutoria</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div id="divTutores" runat="server">
            <div class="form-group">
                <asp:Label ID="LblProfessor" runat="server" AssociatedControlID="LblProfessor" Text="Professor"></asp:Label>
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip7" Chave="cadastroProfessor" />
                <uc2:ucSeletorListBox runat="server" ID="ListBoxesProfessor" DescricaoDisponiveis="Lista de Professores" DescricaoSelecionados="Selecionados" />
            </div>

            <div class="row" id="divResponsavelConsultor" runat="server" visible="false">
                <div class="col-md-6 col-xs-12">
                    <div class="form-group">
                        <uc:ucLupaUsuario runat="server" ID="ucLupaUsuario" Text="Responsável pela turma" />
                    </div>
                </div>
                <div class="col-md-6 col-xs-12">
                    <div class="form-group">
                        <asp:Label ID="Label9" runat="server" AssociatedControlID="txtConsultorEducacional" Text="Consultor Educacional"></asp:Label>
                        <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip8" Chave="txtConsultorEducacional" />
                        <asp:TextBox ID="txtConsultorEducacional" ClientIDMode="Static" runat="server" data-mensagemVazia="Não há consultores educacionais"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="form-group" id="divStatus" runat="server" visible="False">
            <asp:Label ID="Label10" runat="server" Text="Status *" AssociatedControlID="ddlStatus"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip9" Chave="ddlStatus" />
            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlStatus_OnSelectedIndexChanged" />
        </div>
        <div class="form-group" id="divJustificativa" runat="server" visible="False">
            <asp:Label ID="Label11" runat="server" Text="Justificativa de cancelamento *" AssociatedControlID="txtJustificativa"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip10" Chave="txtJustificativa" />
            <asp:TextBox ID="txtJustificativa" TextMode="MultiLine" runat="server" Rows="2" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group" id="divChaveExterna" runat="server">
            <asp:Label ID="LblChaveExterna" runat="server" Text="Chave Externa" AssociatedControlID="txtChaveExterna"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip11" Chave="chaveExterna" />
            <asp:TextBox ID="TxtChaveExterna" runat="server" MaxLength="50" CssClass="form-control" ReadOnly="true"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="LblLocal" runat="server" Text="Local" AssociatedControlID="txtLocal"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip12" Chave="turmaLocal" />
            <asp:TextBox ID="txtLocal" runat="server" MaxLength="250" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="LblDtInicio" runat="server" Text="Data Início *" AssociatedControlID="txtDtInicio"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip13" Chave="turmaDataInicio" />
            <asp:TextBox ID="TxtDtInicio" runat="server" MaxLength="19" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="LblDtFinal" runat="server" Text="Data Final *" AssociatedControlID="txtDtFinal"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip14" Chave="turmaDataFinal" />
            <asp:TextBox ID="TxtDtFinal" runat="server" MaxLength="19" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label2" runat="server" Text="Questionário Pré" AssociatedControlID="txtQuestionarioPre" />
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip15" Chave="questionarioPre" />
            <asp:TextBox ID="txtQuestionarioPre" runat="server" CssClass="form-control" />
        </div>
        <div class="form-group">
            <asp:Label ID="Label8" runat="server" Text="Avaliação de Reação" AssociatedControlID="txtQuestionarioPos" />
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip16" Chave="questionarioPos" />
            <asp:TextBox ID="txtQuestionarioPos" runat="server" CssClass="form-control" />
        </div>
        <div class="form-group" id="divDataDisparoLinkPesquisa" runat="server" visible="false">
            <asp:Label ID="tbtDataDisparoLinkPesquisa" runat="server" Text="Data de Envio de Pesquisa do Questionário Pós" AssociatedControlID="txtDataDisparoLinkPesquisa"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip17" Chave="txtDataDisparoLinkPesquisa" />
            <asp:TextBox ID="txtDataDisparoLinkPesquisa" runat="server" MaxLength="19" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label7" runat="server" Text="Questionário Abandono" AssociatedControlID="txtQuestionarioAbandono" />
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip18" Chave="questionarioAbandono" />
            <asp:TextBox ID="txtQuestionarioAbandono" runat="server" CssClass="form-control" />
        </div>
        <div class="form-group">
            <asp:Label ID="Label3" runat="server" Text="Questionário Cancelamento" AssociatedControlID="txtQuestionarioCancelamento" />
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip19" Chave="questionarioCancelamento" />
            <asp:TextBox ID="txtQuestionarioCancelamento" runat="server" CssClass="form-control" />
        </div>
        <div class="form-group">
            <asp:Label ID="Label13" runat="server" Text="Avaliação de Eficácia" AssociatedControlID="txtQuestionarioEficacia" />
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip26" Chave="questionarioEficacia" />
            <asp:TextBox ID="txtQuestionarioEficacia" runat="server" CssClass="form-control" OnTextChanged="txtQuestionarioEficacia_TextChanged" />
        </div>

        <div class="form-group" id="divQuestEficacia" runat="server" visible="false">
            <asp:Label ID="Label14" runat="server" Text="Data de Envio de Pesquisa do Questionário Eficácia" AssociatedControlID="txtDataDisparoLinkEficacia"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip27" Chave="txtDataDisparoLinkEficacia" />
            <asp:TextBox ID="txtDataDisparoLinkEficacia" runat="server" MaxLength="19" CssClass="form-control"></asp:TextBox>
        </div>

        <div class="form-group" runat="server" id="divAvalizacaoAprendizagem">
            <asp:Label ID="Label15" runat="server" Text="Possui avaliação de aprendizagem?" AssociatedControlID="rblInAvaliacaoAprendizagem" />
            <uc1:ucHelperTooltip runat="server" ID="avaliacaoAprendizagem" Chave="avaliacaoAprendizagem" />
            <asp:RadioButtonList ID="rblInAvaliacaoAprendizagem" runat="server" RepeatDirection="Horizontal" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="rblInAvaliacaoAprendizagem_OnSelectedIndexChanged"></asp:RadioButtonList>
        </div>

        <div class="form-group" runat="server" id="divNotaMinima" visible="false">
            <asp:Label ID="Label12" runat="server" Text="Valor da Nota Mínima" AssociatedControlID="txtValorNotaMinima" />
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip20" Chave="notaMinima" />
            <asp:TextBox ID="txtValorNotaMinima" runat="server" CssClass="form-control" MaxLength="4"
                placeholder="Exemplo: 7,5" onkeypress="return EhNumericoOuVirgula(event)" Text="7"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="Label6" runat="server" Text="Aberta para novas inscrições" AssociatedControlID="rblInAberta"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip21" Chave="turmaAberta" />
            <asp:RadioButtonList ID="rblInAberta" runat="server" RepeatDirection="Horizontal" CssClass="form-control"></asp:RadioButtonList>
        </div>
        <div class="form-group" id="divAcessoPosTermino" runat="server">
            <asp:Label runat="server" Text="Pode ser acessado após o término?" AssociatedControlID="rblAcessoAposConclusao"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip22" Chave="turmaAcessoAposConlusao" />
            <asp:RadioButtonList ID="rblAcessoAposConclusao" runat="server" RepeatDirection="Horizontal" CssClass="form-control">
            </asp:RadioButtonList>
        </div>
        <div class="form-group">
            <asp:Label ID="Label1" runat="server" Text="Quantidade Máxima de Inscrições" AssociatedControlID="txtQTMaxInscricoes"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip23" Chave="turmaQuantidadeDeAcessos" />
            <p runat="server" id="lblTxtMaxInscricoesHelp" class="help-block">
                Escolha uma oferta para exibir a quantidade de vagas restantes.
            </p>
            <asp:TextBox ID="txtQTMaxInscricoes" runat="server" RepeatDirection="Horizontal" CssClass="form-control"></asp:TextBox>
        </div>
        <asp:Panel ID="pnlWIFI" runat="server" Visible="true">
            <div class="form-group">
                <asp:Label ID="Label5" runat="server" Text="Acesso a WI-FI" AssociatedControlID="rblSelecionaAcessoWifi"></asp:Label>
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip24" Chave="rblSelecionaAcessoWifi" />
                <asp:RadioButtonList ID="rblSelecionaAcessoWifi" runat="server"
                    RepeatDirection="Horizontal" CssClass="form-control"
                    OnSelectedIndexChanged="rblSelecionaAcessoWifi_OnSelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Value="1" Text="Sim"></asp:ListItem>
                    <asp:ListItem Value="0" Text="Não"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div class="form-group" id="divAcessoWifi" runat="server">
                <asp:Label ID="Label4" runat="server" Text="Locais" AssociatedControlID="rblAcessoWifi"></asp:Label>
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip25" Chave="rblAcessoWifi" />
                <asp:RadioButtonList ID="rblAcessoWifi" runat="server" RepeatDirection="Horizontal" CssClass="form-control">
                    <asp:ListItem Value="1" Text="Asa Norte"></asp:ListItem>
                    <asp:ListItem Value="2" Text="Asa Sul"></asp:ListItem>
                    <asp:ListItem Value="3" Text="Ambos"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </asp:Panel>
        <br />
        <div class="form-group" id="divBtnSalvar" runat="server">
            <asp:Button ID="btnSalvar" runat="server" OnClick="btnSalvar_OnClick" Text="Salvar"
                CssClass="btn btn-default mostrarload" />
        </div>
    </fieldset>

    <asp:Panel ID="pnlModalConfirmacaoExclusaoNotasMatriculasTurma" runat="server" Visible="false">
        <div aria-hidden="false" aria-labelledby="myModalLabel" role="dialog" tabindex="-1"
            id="modalSessao" class="modal fade in" style="display: block;">
            <div class="modal-dialog modal-aviso">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true" runat="server" onserverclick="OcultarModalConfirmacaoExclusaoNotasMatriculasTurma_Click" id="Button1">
                            &times;</button>
                        <h2 class="modal-title">Registros Pendentes de Edição</h2>
                    </div>
                    <div class="modal-body">
                        <p>Essa Alteração excluirá a nota de <span id="txtNumeroMatriculasTurmaAfetados" runat="server"></span> alunos. Confirmar a alteração?</p>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="Button5" runat="server" Text="Confirmar" CssClass="btn btn-primary mostrarload" OnClick="btnConfirmaExclusaoNotasMatriculasTurma_Click" />
                        <asp:Button ID="Button6" runat="server" Text="Cancelar" CssClass="btn btn-default" OnClick="btnCancelarConfirmacaoExclusaoNotasMatriculasTurma_Click" />
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>

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
        jQuery(function ($) {
            $("#<%= TxtDtInicio.ClientID %>").mask("99/99/9999 99:99:99", {
                autoclear: false
            }).on('blur', function () {
                var valor = $("#<%= TxtDtInicio.ClientID %>").val();
                var dados = valor.split(' ');
                var data = validDate(dados[0]);
                if (!data.result) {
                    $("#<%= TxtDtInicio.ClientID %>").val('');
                } else {
                    dados[1] = dados[1].replace(/_/g, "0");
                    dados[1] = validHour(dados[1]);
                    $("#<%= TxtDtInicio.ClientID %>").val(dados.join(' '));
                }
            });
            $("#<%= TxtDtFinal.ClientID %>").mask("99/99/9999 99:99:99", {
                autoclear: false
            }).on('blur', function () {
                var valor = $("#<%= TxtDtFinal.ClientID %>").val();

                $("#<%= txtDataDisparoLinkPesquisa.ClientID %>").val(valor);

                var dados = valor.split(' ');
                var data = validDate(dados[0]);
                if (!data.result) {
                    $("#<%= TxtDtFinal.ClientID %>").val('');
                } else {
                    dados[1] = dados[1].replace(/_/g, "9");
                    dados[1] = validHour(dados[1]);
                    $("#<%= TxtDtFinal.ClientID %>").val(dados.join(' '));
                }
            });
            $("#<%= txtDataDisparoLinkPesquisa.ClientID %>").mask("99/99/9999 99:99:99", {
                autoclear: false
            }).on('blur', function () {
                var valor = $("#<%= txtDataDisparoLinkPesquisa.ClientID %>").val();
                var dados = valor.split(' ');
                var data = validDate(dados[0]);
                if (!data.result) {
                    $("#<%= txtDataDisparoLinkPesquisa.ClientID %>").val('');
                } else {
                    dados[1] = dados[1].replace(/_/g, "9");
                    dados[1] = validHour(dados[1]);
                    $("#<%= txtDataDisparoLinkPesquisa.ClientID %>").val(dados.join(' '));
                }
            });

            $("#<%= txtDataDisparoLinkEficacia.ClientID %>").mask("99/99/9999 99:99:99", {
                autoclear: false
            }).on('blur', function () {
                var valor = $("#<%= txtDataDisparoLinkEficacia.ClientID %>").val();
                var dados = valor.split(' ');
                var data = validDate(dados[0]);
                if (!data.result) {
                    $("#<%= txtDataDisparoLinkEficacia.ClientID %>").val('');
                } else {
                    dados[1] = dados[1].replace(/_/g, "9");
                    dados[1] = validHour(dados[1]);
                    $("#<%= txtDataDisparoLinkEficacia.ClientID %>").val(dados.join(' '));
                }
            });
        });

        $("#<%= txtValorNotaMinima.ClientID %>").mask("99,99", { reverse: false, autoclear: false });

        var _preloadedListSeUcTurma = <%= ViewState["_Se_ucTurma"] ?? "''" %>;
        AutoCompleteDefine(_preloadedListSeUcTurma, '#txtSolucao', true, false, true);

        var _preloadedListOfertaUcTurma = <%= ViewState["_Oferta_ucTurma"] ?? "''" %>;
        AutoCompleteDefine(_preloadedListOfertaUcTurma, '#txtOfertaTurma', true, false, true);

        var _preloadedListConsultoresEducacionais = <%= ViewState["_ConsultoresEducacionais"] ?? "''" %>;
        AutoCompleteDefine(_preloadedListConsultoresEducacionais, '#txtConsultorEducacional');

        var _preloadedListaQuestionarioPre = <%= ViewState["_SE_QuestionarioPre"] ?? "''" %>;
        AutoCompleteDefine(_preloadedListaQuestionarioPre, <%=txtQuestionarioPre.ClientID%>);
        
        var _preloadedListQuestionarioPos = <%= ViewState["_SE_QuestionarioPos"] ?? "''" %>;
        AutoCompleteDefine(_preloadedListQuestionarioPos, <%=txtQuestionarioPos.ClientID%>);
        
        var _preloadedListQuestionarioCancelamento = <%= ViewState["_SE_QuestionarioCancelamento"] ?? "''" %>;
        AutoCompleteDefine(_preloadedListQuestionarioCancelamento, <%=txtQuestionarioCancelamento.ClientID%>);
        
        var _preloadedListQuestionarioReprovacao = <%= ViewState["_SE_QuestionarioAbandono"] ?? "''" %>;
        AutoCompleteDefine(_preloadedListQuestionarioReprovacao, <%=txtQuestionarioAbandono.ClientID%>);

        var _preloadedListQuestionarioEficacia = <%= ViewState["_SE_QuestionarioEficacia"] ?? "''" %>;
        AutoCompleteDefine(_preloadedListQuestionarioEficacia, <%=txtQuestionarioEficacia.ClientID%>, true, false, true);
    </script>
</div>
