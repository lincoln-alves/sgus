<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Teste.aspx.cs" Inherits="Sebrae.Academico.WebForms.Teste" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script
        src="https://code.jquery.com/jquery-3.2.1.js"
        integrity="sha256-DZAnKJ/6XZ9si04Hgrsxu/8s717jcIzLy3oi35EouyE="
        crossorigin="anonymous"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager runat="server" />
            <asp:UpdatePanel runat="server">
                <ContentTemplate>
                    <asp:Label Text="text" runat="server" ID="txtTeste" ClientIDMode="Static" />
                    <asp:LinkButton Text="text" runat="server" ID="lnkText" OnClick="lnkText_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>

            <asp:TextBox runat="server" ID="txtAlteracao" ClientIDMode="Static" />

        </div>
    </form>

    <script>
        $("#txtAlteracao").on("change", function () {
            console.log("deu trigger !");
            __doPostBack('lnkText', '');
        });
    </script>
</body>
</html>
