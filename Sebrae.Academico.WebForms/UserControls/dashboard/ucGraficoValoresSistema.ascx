<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucGraficoValoresSistema.ascx.cs"
    Inherits="Sebrae.Academico.WebForms.UserControls.ucGraficoValoresSistema" %>
<style>
     .fonte-letra-azul {
         color: #2493b1;
         font-weight: bold;
     }
     .table-striped > tbody > tr:nth-child(odd) > td, .table-striped > tbody > tr:nth-child(odd) > th {
         background-color: #cfd6d8;
     }
</style>
<div class="row">
    <div class="col-lg-6">
        <table class="table table-striped table-condensed" style="margin: 0">
            <tbody>
                <tr>
                    <td><b class="text-overflow">Ano</b></td>
                    <td class="text-right fonte-letra-azul"><%= Ano() %></td>
                </tr>
                <tr>
                    <td><b class="text-overflow">HHT</b></td>
                    <td class="text-right fonte-letra-azul"><%= Hht() %>h</td>
                </tr>
            </tbody>
        </table>
    </div>
    <div class="col-lg-6">
        <table class="table table-striped table-condensed" style="margin: 0">
            <tbody>
                <tr>
                    <td>
                        <span class="pull-right fonte-letra-azul"><b><%= ValorSistema("orcamento") %></b></span>
                        <b class="pull-left">Orçamento</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span class="pull-right fonte-letra-azul"><b><%= ValorSistema("executado") %></b></span>
                        <b class="pull-left">Executado</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span class="pull-right fonte-letra-azul"><b><%= ValorSistema("execucaoPorcentage") %></b></span>
                        <b class="pull-left">Execução %</b>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</div>
<script type="text/javascript">
    (function($) {
       
    })(jQuery);
</script>