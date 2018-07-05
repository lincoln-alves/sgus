<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Erro.aspx.cs" Inherits="Sebrae.Academico.WebForms.Erro" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>SGUS 2.0</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">

    <%-- Bundles --%>
    <asp:PlaceHolder runat="server">
        <%: System.Web.Optimization.Styles.Render("~/Content/css") %>
        <%: System.Web.Optimization.Scripts.Render("~/bundles/master") %>
    </asp:PlaceHolder>

    <link href="/css/style-sgus20.css" rel="stylesheet">
</head>
<body>
    <form id="form" runat="server" role="form">
        <div class="content-area">
            <div class="text-center">
                <div class="row">
                    <div class="col-md-6 col-md-offset-3 col-xs-12">
                        <div class="panel panel-default">
                            <div class="panel-body">
                                <img src="img/logo-uc.jpg" />
                            </div>
                        </div>
                    </div>
                </div>

                <h1 class="text-danger" style="font-size: 7vw;">Ops!
                </h1>
                <h2 style="font-size: 2vw">Houve um erro :(
                </h2>

                <hr />

                <p>
                    O erro ocorrido é inesperado, e nossa equipe vai trabalhar para resolvê-lo o mais rápido possível.
                </p>

                <hr />

                <div class="row">
                    <div class="col-md-6 col-md-offset-3 col-xs-12">
                        <div class="panel panel-danger">
                            <div class="panel-heading">
                                <h3 class="panel-title">Envie os dados abaixo para o suporte técnico
                                </h3>
                            </div>
                            <div class="panel-body">
                                <div class="form-group">
                                    <strong>Tipo:</strong>
                                    <br />
                                    <asp:Literal ID="ltrTipo" runat="server"></asp:Literal>
                                    <asp:Literal ID="ltrCod" runat="server"></asp:Literal>
                                </div>
                                <div class="form-group">
                                    <strong>Linha e método:</strong>
                                    <br />
                                    <asp:Literal ID="lrtLinha" runat="server"></asp:Literal>
                                    <br />
                                    <asp:Literal ID="lrtMetodo" runat="server"></asp:Literal>
                                </div>
                                <div class="form-group">
                                    <strong>Mensagem:</strong>
                                    <br />
                                    <asp:Literal ID="ltrMsg" runat="server"></asp:Literal>
                                </div>
                                <div class="form-group">
                                    <strong>Página:</strong>
                                    <br />
                                    <asp:Literal ID="ltrPagina" runat="server"></asp:Literal>
                                </div>
                                <div class="form-group">
                                    <strong>Mensagem interna do erro:</strong>
                                    <br />
                                    <small>
                                        <asp:Literal ID="ltrInnerMsg" runat="server"></asp:Literal>
                                    </small>
                                </div>
                            </div>
                        </div>

                        <hr />

                        <a class="btn btn-danger btn-block btn-lg" onclick="history.go(-1);" style="margin-bottom: 300px;">Voltar para página anterior</a>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
