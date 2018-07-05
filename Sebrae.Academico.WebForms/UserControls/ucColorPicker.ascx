<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucColorPicker.ascx.cs" Inherits="Sebrae.Academico.WebForms.UserControls.ucColorPicker" %>

<link rel="stylesheet" href="/css/colorpicker.css" type="text/css" />

<script type="text/javascript" src="/js/colorpicker.js"></script>
<script type="text/javascript" src="/js/eye.js"></script>


<style>
#<%= divCorFundo.ClientID %> {
	position: relative;
	width: 36px;
	height: 36px;
	background: url(/img/colorpicker/select.png);
}
#<%= divCorFundo.ClientID %> div {
	position: absolute;
	top: 3px;
	left: 3px;
	width: 30px;
	height: 30px;
	background: url(/img/colorpicker/select.png) center;
	background-color:<%= txtInputCorFundo.Value %>;
}
</style>

<script>
    $(function () {
        $('#<%= divCorFundo.ClientID %>').ColorPicker({
            color: $('#<%= txtInputCorFundo.ClientID %>').val(),
            onShow: function (colpkr) {
                $(colpkr).fadeIn(500);
                return false;
            },
            onHide: function (colpkr) {
                $(colpkr).fadeOut(500);
                return false;
            },
            onChange: function (hsb, hex, rgb) {
                $('#<%= divCorFundo.ClientID %> div').css('backgroundColor', '#' + hex);
                $('#<%= txtInputCorFundo.ClientID %>').val('#' + hex);
            }
        });
    });
</script>

<asp:HiddenField ID="txtInputCorFundo" runat="server"></asp:HiddenField>
<div ID="divCorFundo" runat="server"><div></div></div>
