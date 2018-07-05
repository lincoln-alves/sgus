<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="ListarCadastros.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.SincronizarMoodle.ListarCadastros" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc:LupaUsuario ID="LupaUsuario" runat="server" OnUserSelected="UserSelectedHandler" />
</asp:Content>