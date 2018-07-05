<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucGraficoTop5Cursos.ascx.cs" Inherits="Sebrae.Academico.WebForms.UserControls.ucRelatorioTop5Cursos" %>

<style>
    .text-overflow {
        width: 75%;
        text-overflow: ellipsis;
        display: inline-block;
        white-space: nowrap;
        overflow: hidden;
    }

    .table-top5 {
        margin-bottom: 10px;
    }

    .top5-body {
        padding-top: 5px;
    }
</style>

<div class="row">
    <div class="col-xs-12">
        <div class="table-responsive">
            <table class="table table-top5 table-striped table-condensed" style="table-layout: fixed">
                <tr>
                    <th>Cursos On-line </th>
                </tr>
                <asp:Repeater runat="server" ID="rtOnline">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <span class="text-overflow l-tooltip" data-toggle="tooltip" data-placement="top" title="<%# Eval("NomeSolucaoEducacional") %>"><%# Eval("NomeSolucaoEducacional") %></span>
                                <span class="text-left pull-right fonte-letra-azul"><b><%# Eval("QuantidadeDeUsuariosInscritos") %></b></span>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
    <div class="col-xs-12" style="margin-top: 10px;">
        <div class="table-responsive">
            <table class="table table-top5 table-striped table-condensed table-responsive" style="table-layout: fixed">
                <tr>
                    <th>Cursos Presenciais</th>
                </tr>
                <asp:Repeater runat="server" ID="rtPresencial">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <span class="text-overflow l-tooltip" data-toggle="tooltip" data-placement="top" title="<%# Eval("NomeSolucaoEducacional") %>"><%# Eval("NomeSolucaoEducacional") %></span>
                                <span class="text-left pull-right fonte-letra-azul"><%# Eval("QuantidadeDeUsuariosInscritos") %></span>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>

</div>
