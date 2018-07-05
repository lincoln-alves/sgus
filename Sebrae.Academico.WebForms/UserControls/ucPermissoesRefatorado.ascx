<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPermissoesRefatorado.ascx.cs"
    Inherits="Sebrae.Academico.WebForms.UserControls.ucPermissoesRefatorado" %>

<div class="form-group" id="divPerfil" runat="server">
    <label id="titlePerfil" runat="server" ClientIDMode="Static">
        Perfil -
    </label>
    <asp:CheckBoxList ID="ckblstPerfil" runat="server" RepeatDirection="Vertical" RepeatLayout="UnorderedList">
    </asp:CheckBoxList>
</div>

<div class="clearfix"></div>

<div class="form-group" id="divNivelOcupacional" runat="server">
    <label id="titleNivelocupacional" runat="server" ClientIDMode="Static">
        Nível Ocupacional - 
    </label>
    <asp:CheckBoxList ID="ckblstNivelOcupacional" runat="server" RepeatDirection="Vertical" RepeatLayout="UnorderedList">
    </asp:CheckBoxList>
</div>

<div class="clearfix"></div>

<div class="form-group">
    <label id="title-uf">
        UF - 
    </label>
    <asp:Literal ID="liVagaParaTodos" runat="server" Text=" - Vagas para Todos:"></asp:Literal>
    <asp:TextBox ID="txtVagasTodos" CssClass="txtVagasTodos" runat="server" Style="width: 50px; height: 20px;"></asp:TextBox>

    <ul id="ckbUfs">
        <asp:Repeater ID="rptUFs" runat="server">
            <ItemTemplate>
                <li>
                    <asp:CheckBox ID="ckUF" CssClass="ckUF" runat="server" />
                    <asp:Label ID="lblUF" runat="server" Text=""></asp:Label>
                    <asp:TextBox ID="txtVagas" CssClass="txtVagas" runat="server" Style="width: 50px; height: 20px;"></asp:TextBox>
                </li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
</div>

<div class="clearfix"></div>

<script>
    $(document).ready(function () {
        // Botões de marcar e desmarcar.
        $.markAll('titlePerfil', '<%= ckblstPerfil.ClientID %>', 'Marcar todos', 'Desmarcar todos');
        $.markAll('titleNivelocupacional', '<%= ckblstNivelOcupacional.ClientID %>', 'Marcar todos', 'Desmarcar todos');
        $.markAll('title-uf', 'ckbUfs', 'Marcar todas', 'Desmarcar todas');
    });
</script>
