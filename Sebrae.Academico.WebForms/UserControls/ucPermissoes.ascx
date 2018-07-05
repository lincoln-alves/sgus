<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPermissoes.ascx.cs"
    Inherits="Sebrae.Academico.WebForms.UserControls.ucPermissoes" %>
<%@ Import Namespace="Sebrae.Academico.Dominio.Enumeracao" %>
<%@ Register Src="~/UserControls/ucAreas.ascx" TagPrefix="uc" TagName="ucAreas" %>

<div class="form-group" id="divPerfil" runat="server">
    <label id="titlePerfil" runat="server" clientidmode="Static">
        Perfil -
    </label>
    <asp:CheckBoxList ID="ckblstPerfil" runat="server" RepeatDirection="Vertical" RepeatLayout="UnorderedList"
        ClientIDMode="Static">
    </asp:CheckBoxList>
</div>
<div class="clearfix"></div>
<div class="form-group" id="divNivelOcupacional" runat="server">
    <label id="titleNivelocupacional" runat="server" clientidmode="Static">
        Nível Ocupacional - 
    </label>
    <asp:CheckBoxList ID="ckblstNivelOcupacional" runat="server" RepeatDirection="Vertical" RepeatLayout="UnorderedList" ClientIDMode="Static">
    </asp:CheckBoxList>
</div>
<div class="clearfix"></div>
<div id="divAreas" style="display: none">
    <uc:ucAreas runat="server" ID="ucAreas" />
</div>
<div class="clearfix"></div>
<div class="form-group">
    <label id="title-uf">
        UF - 
    </label>
    
    <div class="form-group" runat="server" ID="divModoDistribuicaoVagas" Visible="False">
        <asp:RadioButtonList ID="rblModoDistribuicaoVagas" runat="server" RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="rblModoDistribuicaoVagas_OnSelectedIndexChanged" CssClass="mostrarload">
            <asp:ListItem Value="0">Vagas por Ordem de Inscrição</asp:ListItem>
            <asp:ListItem Value="1">Distribuir vagas por UF</asp:ListItem>
        </asp:RadioButtonList>
    </div>

    <ul id="ckbUfs" runat="server" clientidmode="Static">
        <asp:Repeater ID="rptUFs" runat="server">
            <ItemTemplate>
                <li>
                    <asp:CheckBox ID="ckUF" CssClass="ckUF" runat="server" />
                    <label>
                        <asp:Literal ID="lblUF" runat="server" Text=""></asp:Literal>
                        <asp:Literal ID="lblVagas" runat="server" Text=" | Vagas: "></asp:Literal>
                    </label>
                    <asp:TextBox ID="txtVagas" CssClass="txtVagas" runat="server" Style="width: 50px; height: 20px;"></asp:TextBox>
                </li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
</div>
<div class="clearfix"></div>
<script>
    $(window).load(function () {
        
        // Botões de marcar e desmarcar.
        $.markAll('titlePerfil', '<%= ckblstPerfil.ClientID %>', 'Marcar todos', 'Desmarcar todos');
        $.markAll('titleNivelocupacional', '<%= ckblstNivelOcupacional.ClientID %>', 'Marcar todos', 'Desmarcar todos');
        $.markAll('title-uf', 'ckbUfs', 'Marcar todas', 'Desmarcar todas');
        
        // Exibição de áreas.
        var exibirAreas = <%= ExibirAreas.ToString().ToLower() %>;

        if (exibirAreas) {
            var checkBox = $('#<%=ckblstNivelOcupacional.ClientID%>');

            // Obter checkbox do credenciado.
            var credenciado = checkBox.find('li').find('input[value=<%=(int)enumNivelOcupacional.Credenciado%>]')[0];

            $(credenciado).on('change', function () {
                if($(this).is(":checked"))
                    $('#divAreas').show();
                else {
                    $('#divAreas').hide();
                }
            });

            if($(credenciado).is(":checked"))
                $('#divAreas').show();
            else {
                $('#divAreas').hide();
            }
        }
    });
</script>
