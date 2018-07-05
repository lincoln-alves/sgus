<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucUpload.ascx.cs" Inherits="Sebrae.Academico.WebForms.UserControls.ucUpload" %>
<asp:Label runat="server" ID="lblImagem"></asp:Label>
<asp:FileUpload ID="fupldImagemEnviada" runat="server" EnableViewState="true" ViewStateMode="Enabled"  CssClass="form-control"/>
<br />
<asp:RequiredFieldValidator ID="rfvImagem" runat="server" ControlToValidate="fupldImagemEnviada"
    Enabled="false" ErrorMessage="Imagem. Campo Obrigatório" Display="Dynamic"></asp:RequiredFieldValidator>
<br />
<img alt="" id="imgImagem" runat="server" style="max-width: 200px;" />
