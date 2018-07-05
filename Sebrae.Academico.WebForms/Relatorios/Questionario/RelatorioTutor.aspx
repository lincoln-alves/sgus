<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="RelatorioTutor.aspx.cs" Inherits="Sebrae.Academico.WebForms.Relatorios.QuestionarioPesquisa.RelatorioTutor" %>

<%@ Register Src="~/UserControls/ucRelatorioQuestionario.ascx" TagName="RelatorioQuestionario" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc:RelatorioQuestionario ID="ucRelatorioQuestionario" runat="server" RelatorioTutor="True"></uc:RelatorioQuestionario>
</asp:Content>