<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucMatrilhaTrilha.ascx.cs"
    Inherits="Sebrae.Academico.WebForms.UserControls.ucMatrilhaTrilha" %>
<%@ Register Src="~/UserControls/ucLupaUsuario.ascx" TagName="LupaUsuario" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>

<div class="panel-group" id="accordionOF">
    <div class="panel panel-default">
        <div class="panel-heading">
            <a data-toggle="collapse" data-parent="#accordionOF" href="#collapse100" runat="server"
                id="spanAcao">Dados</a>
        </div>
        <div id="collapse100" class="panel-collapse collapse in">
            <div class="panel-body">
                <fieldset>
                    <div class="form-group">
                        <uc2:LupaUsuario ID="LupaUsuario1" runat="server" Chave="lupaUsuario" />
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label2" runat="server" Text="Trilha" AssociatedControlID="ddlTrilha" />
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip1" Chave="cadastroTrilha" />
                        <asp:DropDownList ID="ddlTrilha" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTrilha_SelectedIndexChanged"
                            CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label1" runat="server" Text="Nível" AssociatedControlID="ddlTrilhaNivel"/>
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip2" Chave="cadastroNivelTrilha" />
                        <asp:DropDownList ID="ddlTrilhaNivel" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label3" runat="server" Text="Status" AssociatedControlID="ddlStatus" />
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip3" Chave="statusAtivo" />
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label4" runat="server" Text="Data Término" AssociatedControlID="txtDtFim" />
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip4" Chave="dataTermino" />
                        <asp:TextBox ID="txtDtFim" runat="server" MaxLength="10" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label5" runat="server" Text="Data Limite" AssociatedControlID="txtDtLimite" />
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip5" Chave="dataLimite" />
                        <asp:TextBox ID="txtDtLimite" runat="server" MaxLength="10" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label6" runat="server" Text="Nota da Prova" AssociatedControlID="txtNotaProva" />
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip6" Chave="notaProva" />
                        <asp:TextBox ID="txtNotaProva" runat="server" CssClass="form-control" MaxLength="4" onkeypress="return EhNumericoOuVirgula(event)"></asp:TextBox>
                    </div>
                    <div class="form-group" runat="server" id="QtdMedalhas">
                        Qtd de Medalhas / Possíveis: 
                        <asp:Label ID="lblQtdEstrelas" runat="server"></asp:Label>
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip7" Chave="lblQtdEstrelas" />
                    </div>
                    <div class="form-group" id="divAcessoBloqueado" runat="server" visible="false" clientidmode="Static">
                        <asp:CheckBox ID="chkAcessoBloqueado" runat="server" CssClass="form-control" ClientIDMode="Static" Text="Bloquear Acesso"/>
                    </div>
                    <div class="form-group" runat="server" id="LiberarNovaProva">
                        <asp:CheckBox ID="chkLiberarNovaProva" runat="server" CssClass="form-control" ClientIDMode="Static" Text="Liberar Nova Prova" />
                    </div>
                    <div class="form-group" runat="server" id="LiberarNovaProvaData">
                        Data da Liberação da Nova Prova
                        <uc1:ucHelperTooltip runat="server" id="UcHelperTooltip8" Chave="dtLiberacaoNovaProva" />
                        <asp:TextBox ID="txtDataLiberacaoNovaProva" runat="server" CssClass="form-control" ClientIDMode="Static" MaxLength="10" />
                    </div>
                </fieldset>
            </div>
        </div>
    </div>
</div>
<script language="javascript" type="text/javascript">
    $("#chkLiberarNovaProva").click(function () {
        LiberarBloquearDataProva()
    });

    $(document).ready(function () {
        LiberarBloquearDataProva();
    });

    function LiberarBloquearDataProva() {
        if ($('#chkLiberarNovaProva').is(':checked')) {
            $("#txtDataLiberacaoNovaProva").removeAttr("disabled");
        } else {
            $("#txtDataLiberacaoNovaProva").attr("disabled", "disabled");
        }
    }
    jQuery(function ($) {
        $("#<%= txtDtFim.ClientID %>").mask("99/99/9999");
        $("#<%= txtDtLimite.ClientID %>").mask("99/99/9999");
        $("#<%= txtDataLiberacaoNovaProva.ClientID %>").mask("99/99/9999");
    });
</script>