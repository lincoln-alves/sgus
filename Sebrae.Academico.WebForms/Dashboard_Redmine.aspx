<%@ Page Title="" Language="C#" MasterPageFile="~/Academico20.Master" AutoEventWireup="true"
    CodeBehind="Dashboard_Redmine.aspx.cs" Inherits="Sebrae.Academico.WebForms.Dashboard_Redmine" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <iframe id="FrameDashboard" runat="server" src="" marginheight="0" marginwidth="0" scrolling="no" frameborder="0" style="overflow:hidden !important;display:block;width:100%;height: 95vh;"></iframe>

    <script type="text/javascript">
        $(function () {
            $('#sidebar').hide();
            $('#tituloPagina').hide();
            $('#linhaTituloPagina').hide();
            $('#descricaoPagina').hide();
            $('#contentArea').removeClass('col-sm-9');
        });

        function atualizaIframe() {
            $("#FrameDashboard").prop("src", $("#FrameDashboard").src);
        }
    </script>

    <style type="text/css">
        html{
            overflow: hidden !important;
        }

        #sidebar{
            display: none;
        }

        #contentArea{
            width: 100% !important;
            padding: 0;
            margin: 0;
        }
    </style>
</asp:Content>
