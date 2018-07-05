<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSeletorCheckboxes.ascx.cs" Inherits="Sebrae.Academico.WebForms.UserControls.ucSeletorCheckboxes" %>

<script type="text/javascript">

    $(function () {
        $("#<%= btnMarcarTodos.ClientID %>").each(function () {
            var btn = $(this);
            var target = btn.attr("for");
            var cks = btn.parent().find("#" + target).find("input[type=checkbox]");
            $(this).unbind('click').click(function () {
                ckStatus = !cks.prop('checked');
                cks.prop('checked', ckStatus);
                btn.html((ckStatus) ? "Desmarcar Todos" : "Marcar Todos");
            });
            cks.unbind('change').change(function () {
                ckStatus = cks.prop('checked');
                btn.html((ckStatus) ? "Desmarcar Todos" : "Marcar Todos");
            });
        });
    });
</script>



<asp:Label ID="lblDescricao" runat="server" Text="" AssociatedControlID="cblItens"></asp:Label>
-
<asp:Label ID="btnMarcarTodos" runat="server" Text="Marcar Todos" AssociatedControlID="cblItens"
CssClass="marcar-todos"></asp:Label>
<asp:CheckBoxList ID="cblItens" CssClass="inline-block top-align" runat="server" RepeatDirection="Vertical" RepeatLayout="UnorderedList"
ClientIDMode="Static">
</asp:CheckBoxList>
