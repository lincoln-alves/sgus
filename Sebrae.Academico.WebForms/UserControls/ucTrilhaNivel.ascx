<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucTrilhaNivel.ascx.cs"
    ClientIDMode="Static" Inherits="Sebrae.Academico.WebForms.UserControls.ucTrilhaNivel" %>
<%@ Register Src="ucPermissoes.ascx" TagName="ucPermissoes" TagPrefix="uc2" %>

<div class="panel-group" id="accordionTN">
    <div class="panel panel-default">
        <div class="panel-heading">
            <a data-toggle="collapse" data-parent="#accordionTN" href="#collapse10">Dados </a>
        </div>
        <div id="collapse10" class="panel-collapse collapse in">
            <div class="panel-body">
                <fieldset>
                    <div class="form-group">
                        <asp:Label ID="Label4" runat="server" Text="Nome:" AssociatedControlID="txtNomeNivel" />
                        <asp:TextBox ID="txtNomeNivel" runat="server" CssClass="form-control" MaxLength="250"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label1" runat="server" Text="Inscrições Abertas:" AssociatedControlID="ddlAceitaNovasMatriculas" />
                        <asp:DropDownList ID="ddlAceitaNovasMatriculas" runat="server" CssClass="form-control" />
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label2" runat="server" Text="Questionário Pré:" AssociatedControlID="ddlQuestionarioPre" />
                        <asp:DropDownList ID="ddlQuestionarioPre" runat="server" CssClass="form-control" />
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label8" runat="server" Text="Entrevista:" AssociatedControlID="ddlQuestionarioPos" />
                        <asp:DropDownList ID="ddlQuestionarioPos" runat="server" CssClass="form-control" />
                    </div>
                    <div class="form-group">
                        <asp:CheckBox ID="chkDinamicoPrePos" runat="server" Text="Questionário Dinâmico Pré/Pós"
                            CssClass="form-control" />
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label3" runat="server" Text="Prova:" AssociatedControlID="ddlQuestionarioPre" />
                        <asp:DropDownList ID="ddlQuestionarioProva" runat="server" CssClass="form-control" />
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label7" runat="server" Text="Template do Certificado:" AssociatedControlID="ddlCertificadoTemplate" />
                        <asp:DropDownList ID="ddlCertificadoTemplate" runat="server" CssClass="form-control" />
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label5" runat="server" Text="Pré-Requisito:" AssociatedControlID="ddlPreRequisito" />
                        <asp:DropDownList ID="ddlPreRequisito" runat="server" CssClass="form-control" />
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label6" runat="server" Text="Prazo (Dias)" AssociatedControlID="txtPrazo" />
                        <asp:TextBox ID="txtPrazo" runat="server" CssClass="form-control" MaxLength="4" onkeypress="return EhNumerico(event)" />
                    </div>
                    <div class="form-group">
                        <asp:Label ID="lblCargaHoraria" runat="server" Text="Carga Horaria" AssociatedControlID="txtCargaHoraria" />
                        <asp:TextBox ID="txtCargaHoraria" runat="server" CssClass="form-control" MaxLength="4" onkeypress="return EhNumerico(event)" />
                    </div>
                    <%--<div class="form-group">
                        <asp:Label ID="Label11" runat="server" Text="Texto do Termo de Aceite:" AssociatedControlID="txtTextoTermoDeAceite" />
                        <asp:TextBox ID="txtTextoTermoDeAceite" runat="server" TextMode="MultiLine" CssClass="form-control" MaxLength="2000" />
                    </div>--%>
                    <div class="form-group">
                        <asp:Label ID="Label12" runat="server" Text="Valor da Nota Mínima:" AssociatedControlID="txtValorNotaMinima" />
                        <asp:TextBox ID="txtValorNotaMinima" runat="server" CssClass="form-control" MaxLength="4"></asp:TextBox>
                    </div>
                    <div class="form-group">        
                        <asp:Label ID="Label14" runat="server" Text="Prazo avaliação monitor (Dias úteis)" AssociatedControlID="txtPrazoMonitor"></asp:Label>
                        <asp:TextBox ID="txtPrazoMonitor" runat="server" MaxLength="2" CssClass="form-control" onkeypress="return EhNumerico(event)"></asp:TextBox>
                    </div>
                    <div class="form-group">        
                        <asp:Label ID="Label15" runat="server" Text="Limite de Cancelamento" AssociatedControlID="txtLimiteCancelamento"></asp:Label>
                        <asp:TextBox ID="txtLimiteCancelamento" runat="server" CssClass="form-control" data-help="Limite de cancelamento da matrícula"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:CheckBox ID="chbAvisarMonitor" runat="server" Text="Alertar monitor as participações atrasadas?"
                            CssClass="form-control" />
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label13" runat="server" Text="Ordem:" AssociatedControlID="ddlOrdem" />
                        <asp:DropDownList ID="ddlOrdem" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </fieldset>
            </div>
        </div>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            <a data-toggle="collapse" data-parent="#accordionTN" href="#collapse11">Permissões
            </a>
        </div>
        <div id="collapse11" class="panel-collapse collapse">
            <div class="panel-body">
                <uc2:ucPermissoes ID="ucPermissoesNivel" runat="server" />
            </div>
        </div>
    </div>
    <div>
        <asp:Button ID="btnEnviar" runat="server" Text="Enviar" OnClick="btnEnviar_Click"
            CausesValidation="true" CssClass="btn btn-default mostrarload" />
    </div>
</div>
