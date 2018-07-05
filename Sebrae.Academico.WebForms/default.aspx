﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Sebrae.Academico.WebForms.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>SGUS 2.0</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="description" content="">
    <meta name="author" content="">
    <!-- Bootstrap core CSS -->
    <script type="text/javascript" src="/js/jquery/jquery-1.10.2.min.js"></script>
    <link href="/css/bootstrap.css" rel="stylesheet">
    <!-- Custom styles for this template -->
    <link href="/css/fonts.css" rel="stylesheet">
    <link href="/css/style-sgus20.css" rel="stylesheet">
    <link href="/css/font-awesome-ext.css" rel="stylesheet">
    <!-- HTML5 shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
		  <script src="/js/html5shiv.js"></script>
		  <script src="/js/respond.min.js"></script>
		<![endif]-->
    <script type="text/javascript" src="/js/jquery/jquery.js"></script>
    <script type="text/javascript" src="/js/ckeditor/ckeditor.js"></script>
    <script type="text/javascript" src="/js/ckeditorConfig.js"></script>
    <script type="text/javascript" src="/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="/js/tooltip.js"></script>
    <script type="text/javascript" src="/js/noty/packaged/jquery.noty.packaged.min.js"></script>
    <script type="text/javascript" src="/js/spin.min.js"></script>
    <script type="text/javascript" src="/js/jquery/jquery.cookie.js"></script>
    <script type="text/javascript" src="/js/jquery/jquery.mask.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Literal ID="LitRetorno" runat="server"></asp:Literal>
            <div class="navbar navbar-fixed-top">
                <div class="col-sm-3 col-md-2 pull-right logon">
                    <span class="hidden-xs">Usuário não logado</span>
                </div>
            </div>
            <div class="col-sm-4 col-sm-offset-4">
                <div class="jumbotron">
                    <div class="titulo row">
                        <img class="img-responsive col-md-12 col-lg-6" src="img/logo-uc.jpg" />
                        <p class="col-md-12  col-lg-6 hidden-xs">
                            Bem-vindo ao SGUS da UC Sebrae
                        </p>
                    </div>
                    <hr />
                    <script type="text/javascript">
                        $(document).ready(function () {
                            $(".focusOnLoad").focus();
                            $(".focusOnLoad").mask('000.000.000-00', { reverse: true });
                        });
                    </script>
                    <div class="row">
                        <div class="form-group">
                            Usuário:
                    <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control focusOnLoad"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            Senha:&nbsp;
                    <asp:TextBox ID="txtSenha" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                        </div>
                        <asp:Button ID="btnLogin" runat="server" Text="Entrar" OnClick="btnLogin_Click" CssClass="btn btn-primary mostrarload" />
                    </div>
                </div>
            </div>
            <script type="text/javascript">
                $("body").addClass('login');
            </script>
        </div>
    </form>
</body>
</html>
