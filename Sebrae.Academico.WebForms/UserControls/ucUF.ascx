<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucUF.ascx.cs" Inherits="Sebrae.Academico.WebForms.UserControls.ucUF" %>

  <label id="title-ufs">
            Ufs - 
  </label>

<asp:CheckBoxList ID="chkUF" runat="server" RepeatDirection="Vertical" RepeatLayout="UnorderedList" CssClass="nobullet"></asp:CheckBoxList>

<script>
    $(window).ready(function() {
        $.markAll('title-ufs', '<%= chkUF.ClientID %>', 'Marcar todas', 'Desmarcar todas');
    });
</script>