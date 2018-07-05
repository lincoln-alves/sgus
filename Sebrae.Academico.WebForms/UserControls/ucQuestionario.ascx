<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucQuestionario.ascx.cs" Inherits="Sebrae.Academico.WebForms.UserControls.ucQuestionario" %>

<asp:HiddenField ID="hdnIdQuestionario" runat="server" />
<asp:HiddenField ID="hdnIdQuestionarioParticipacao" runat="server" />
<asp:HiddenField ID="hdnIdMatriculaTurma" runat="server" />
<asp:HiddenField ID="hdnIdTurma" runat="server" />
<asp:HiddenField ID="hdnIdStatusMatricula" runat="server" />
<asp:HiddenField ID="hdnNotaFinal" runat="server" />

<asp:Repeater ID="rptQuestionario" runat="server" OnItemDataBound="rptQuestionario_OnItemDataBound">
    <ItemTemplate>
        <div class="form-group">
            <asp:HiddenField ID="hdnIdItemQuestionario" runat="server" />
            <label for="">
                <%# Container.ItemIndex + 1 %> - <%# Eval("Questao")%>
            </label>
            <asp:TextBox ID="txtResposta" runat="server" CssClass="form-control" Rows="3" TextMode="MultiLine"></asp:TextBox>
        </div>
    </ItemTemplate>
</asp:Repeater>

<asp:Panel runat="server" ID="pnlDataConclusao" Visible="false">
    <div class="form-group">
        <asp:Label Text="Data de Conclusão da Turma" runat="server" />
        <asp:TextBox runat="server" ID="txtDataConclusao" MaxLength="19" CssClass="form-control txtDataconclusao" AutoPostBack="true" OnTextChanged="txtDataConclusao_TextChanged"  />
    </div>
</asp:Panel>

 <!-- Alert MATRICULA ADMIN/GESTOR-->
    <asp:Panel ID="pnlAlteracaoData" runat="server" Visible="false">

        <div class="alert alert-danger alert-dismissible fade in" role="alert">
            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                <span aria-hidden="true">×</span></button>
            <h4>Alteração na data final da turma</h4>
            <p>
                <asp:Label ID="lblRestricaoMatricula" runat="server"></asp:Label>
            </p>
            <p>A data de conclusão difere da data final da turma, confirmar o cancelamento ?</p>
            <p>
                <asp:Button Text="Não" runat="server" CssClass="btn btn-default" ID="btnCancelarAlteracao" OnClick="btnCancelarAlteracao_Click" />
                <asp:Button Text="Sim" runat="server" CssClass="btn btn-danger" ID="btnConfirmarAlteracao" OnClick="btnConfirmarAlteracao_Click" />
            </p>
        </div>
    </asp:Panel>

<script type="text/javascript">
    $("#<%= txtDataConclusao.ClientID %>").mask("99/99/9999 99:99:99", {
        autoclear: false
    }).on('blur', function () {
        var valor = $("#<%= txtDataConclusao.ClientID %>").val();
        var dados = valor.split(' ');
        var data = validDate(dados[0]);
        if (!data.result) {
            $("#<%= txtDataConclusao.ClientID %>").val('');
        } else {
            dados[1] = dados[1].replace(/_/g, "9");
            dados[1] = validHour(dados[1]);
            $("#<%= txtDataConclusao.ClientID %>").val(dados.join(' '));
        }
    })(jQuery);

    function bloquearEnvio() {
        $("#ContentPlaceHolder1_ContentPlaceHolder1_btnEnviarQuestionario").prop("disabled", true);
    }

    function habilitarEnvio() {
        $("#ContentPlaceHolder1_ContentPlaceHolder1_btnEnviarQuestionario").prop("disabled", false);
    }
</script>
