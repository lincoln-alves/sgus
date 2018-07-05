<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="RelacaoRespondentes.aspx.cs" Inherits="Sebrae.Academico.WebForms.Relatorios.QuestionarioPesquisa.RelacaoRespondentes" %>

<%@ Register Src="~/UserControls/ucRelatorioQuestionario.ascx" TagName="RelatorioQuestionario" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc:RelatorioQuestionario ID="ucRelatorioQuestionario" runat="server" RelatorioTutor="False"></uc:RelatorioQuestionario>
</asp:Content>