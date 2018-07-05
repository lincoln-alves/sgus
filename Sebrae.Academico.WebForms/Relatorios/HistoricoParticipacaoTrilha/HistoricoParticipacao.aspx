<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true"
    CodeBehind="HistoricoParticipacao.aspx.cs" Inherits="Sebrae.Academico.WebForms.Relatorios.HistoricoParticipacaoTrilha.HistoricoParticipacao" %>

<%@ Register Src="~/UserControls/ucLupaUsuario.ascx" TagName="ucLupaUsuario" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/ucSeletorListBox.ascx" TagName="ucMultiplosUsuarios" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel-group" id="gruporelatorio">
        <div class="panel panel-default">
            <div class="panel-heading">
                <a data-toggle="collapse" data-parent="#gruporelatorio" href="#Filtros">Filtros</a>
            </div>
            <asp:Panel ID="Filtros" runat="server" ClientIDMode="Static" CssClass="panel-collapse collapse in">
                <div class="panel-body">
                    <fieldset>
                        <div class="form-group">
                            <asp:Label ID="lblNome" runat="server" Text="Trilha" AssociatedControlID="ddlTrilha" />
                            <asp:DropDownList runat="server" ID="ddlTrilha" AutoPostBack="True" OnSelectedIndexChanged="ddlTrilha_SelectedIndexChanged"
                                CssClass="form-control">
                            </asp:DropDownList>
                        </div>

                        <div class="form-group">
                            <asp:Label ID="Label2" runat="server" Text="Nível da trilha" AssociatedControlID="ddlTrilhaNivel" />
                            <asp:DropDownList runat="server" ID="ddlTrilhaNivel" OnSelectedIndexChanged="ddlTrilhaNivel_SelectedIndexChanged" CssClass="form-control" AutoPostBack="true">
                            </asp:DropDownList>
                        </div>

                        <div class="form-group">
                            <asp:Label ID="lblPontoSebrae" runat="server" Text="Ponto sebrae" AssociatedControlID="ddlPontoSebrae" />
                            <asp:DropDownList runat="server" ID="ddlPontoSebrae" CssClass="form-control" OnSelectedIndexChanged="ddlPontoSebrae_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </div>

                        <div class="form-group">
                            <asp:Label ID="Label3" runat="server" Text="Missão" AssociatedControlID="ddlMissao" />
                            <asp:DropDownList runat="server" ID="ddlMissao" CssClass="form-control" OnSelectedIndexChanged="ddlMissao_SelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label4" runat="server" Text="Tipo de Solução" AssociatedControlID="cblTipoSolucao" />
                            <asp:CheckBoxList ID="cblTipoSolucao" runat="server" OnSelectedIndexChanged="cblTipoSolucao_SelectedIndexChanged" AutoPostBack="true"></asp:CheckBoxList>                            
                        </div>
                        <asp:Panel ID="pnlSolucao" Visible="false" runat="server">
                            <div class="form-group">
                                <asp:Label ID="Label5" runat="server" Text="Solução Sebrae" AssociatedControlID="ddlSoucaoEducacional" />
                                <asp:DropDownList runat="server" ID="ddlSoucaoEducacional" CssClass="form-control" AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                        </asp:Panel>
                        <div class="form-group">
                            <uc:LupaUsuario ID="ucLupaUsuario" runat="server" />
                        </div>
                        <div class="form-group">
                            <asp:Label ID="LabelUF" runat="server" Text="UF" AssociatedControlID="ucMultiplosUF" />
                            <uc:ucMultiplosUsuarios ID="ucMultiplosUF" runat="server" />
                        </div>

                        <div class="form-group">
                            <asp:Label ID="Label6" runat="server" Text="Nível Ocupacional" AssociatedControlID="ucSelectNivelOcupacional" />
                            <uc:ucMultiplosUsuarios ID="ucSelectNivelOcupacional" runat="server" />
                        </div>

                        <div class="form-group">
                            <asp:Label ID="LabelDataLimite" runat="server" Text="Data Limite de Conclusão de Trilha" AssociatedControlID="txtDataLimite"></asp:Label>
                            <asp:TextBox ID="txtDataLimite" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>

                        <asp:Panel ID="pnlStatus" Visible="false" runat="server">
                            <div class="form-group">
                                <asp:Label ID="LabelStatus" runat="server" Text="Status" AssociatedControlID="ddlStatus"></asp:Label>
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control"></asp:DropDownList>
                            </div>
                        </asp:Panel>
                        <div class="form-group">
                            <asp:Label ID="LabelPeriodoInicial" runat="server" Text="Período Inicial da Inscrição" AssociatedControlID="txtPeriodoInicial"></asp:Label>
                            <asp:TextBox ID="txtPeriodoInicial" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <asp:Label ID="LabelPeriodoFinal" runat="server" Text="Período Final da Inscrição" AssociatedControlID="txtPeriodoFinal"></asp:Label>
                            <asp:TextBox ID="txtPeriodoFinal" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click" CssClass="btn btn-primary mostrarload" />
                        </div>
                    </fieldset>
                </div>
            </asp:Panel>
        </div>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            <a data-toggle="collapse" data-parent="#gruporelatorio" href="#Campos" title="As horas são agrupadas de acordo com os campos exibidos">Campos a Serem Exibidos
            </a>
        </div>
        <div id="Campos" class="accordion-body collapse">
            <div class="accordion-inner">
                <fieldset>
                    <div class="form-group">
                        <asp:CheckBoxList ID="chkListaCamposVisiveis" runat="server" RepeatDirection="Vertical"
                            RepeatLayout="UnorderedList" ClientIDMode="Static">
                            <asp:ListItem Selected="True" Value="CPF">CPF</asp:ListItem>
                            <asp:ListItem Selected="True" Value="PontoSebrae">Ponto Sebrae</asp:ListItem>
                            <asp:ListItem Selected="True" Value="TipoSolucao">Tipo de Solução</asp:ListItem>
                            <asp:ListItem Selected="True" Value="SolucaoEducacional">Nome da Solução Educacional</asp:ListItem>
                            <asp:ListItem Selected="True" Value="FormaAquisicao">Forma de Aquisição</asp:ListItem>
                            <asp:ListItem Selected="True" Value="DataAlteracaoStatusParticipacao">Data de Alteração do Status do Participante</asp:ListItem>
                            <asp:ListItem Selected="True" Value="NotaObtida">Nota Obtida</asp:ListItem>
                            <asp:ListItem Selected="True" Value="Missao">Missão</asp:ListItem>
                            <asp:ListItem Selected="True" Value="TotalCurtidas">Total de Curtidas</asp:ListItem>
                            <asp:ListItem Selected="True" Value="TotalDescurtidas">Total de Descurtidas</asp:ListItem>
                            <asp:ListItem Selected="True" Value="DataInclusaoTrilha">Data de Inclusão na Trilha</asp:ListItem>
                            <asp:ListItem Selected="True" Value="Ranking">Posição no Ranking</asp:ListItem>
                        </asp:CheckBoxList>
                    </div>
                </fieldset>
            </div>
        </div>
    </div>
    <hr />
    <asp:Panel ID="pnlParticipacaoTrilha" runat="server" Visible="false">
        <h4>Resultado da Busca</h4>

        <div class="table-responsive">

            <asp:Repeater ID="rptUsuariosTrilha" runat="server" OnItemDataBound="rptUsuariosTrilha_ItemDataBound">
                <ItemTemplate>
                    <table class="table table-hover">
                        <thead>
                            <tr>
                                <th class="FiltroNome">Nome</th>
                                <th class="FiltroCPF" runat="server" id="Th1">CPF</th>
                                <th class="FiltroNivelOcupacional">Nível Ocupacional</th>
                                <th class="FiltroUF">UF</th>
                                <th class="FiltroStatusMatricula">Status Matricula</th>
                                <th class="FiltroTrilha">Trilha</th>
                                <th class="FiltroTrilhaNivel">Trilha Nível</th>
                                <th class="filtroDataAlteracaoStatus" runat="server" id="Th2">Data de alteração do Status do Participante</th>
                                <th class="filtroDataInclusao" runat="server" id="Th3">Data de Inclusão na Trilha</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td class="FiltroNome"><%#Eval("Usuario.Nome")%></td>
                                <td class="FiltroCPF" runat="server" id="CPF"><%# Eval("Usuario.CPF")%></td>
                                <td class="FiltroNivelOcupacional"><%# Eval("NivelOcupacional.Nome")%></td>
                                <td class="FiltroUF"><%# Eval("Uf.Nome")%></td>
                                <td class="FiltroStatusMatricula"><%#Eval("StatusMatriculaFormatado")%></td>
                                <td class="FiltroTrilha"><%#Eval("TrilhaNivel.Trilha.Nome")%></td>
                                <td class="FiltroTrilhaNivel"><%#Eval("TrilhaNivel.Nome")%></td>
                                <td class="FiltroDataAlteracaoStatus" runat="server" id="DataAlteracaoStatusParticipacao"><%#Eval("DataAlteracao")%></td>
                                <td class="filtroDataInicio" runat="server" id="DataInclusaoTrilha"><%#Eval("DataInicio")%></td>
                            </tr>
                        </tbody>
                    </table>


                    <div class="trail-collapse">
                        <h4><span>-</span> Soluções da trilha</h4>
                        <div class="trail-collapse-content">
                            <table>
                                <tr>
                                    <td class="trail-labels">
                                        <table>
                                            <thead>
                                                <tr>
                                                    <th class="nome">Soluções Sebrae</th>
                                                </tr>
                                                <tr>
                                                    <th>Carga Horária</th>
                                                </tr>
                                                <tr>
                                                    <th>Quantidade de Moedas</th>
                                                </tr>
                                            </thead>
                                        </table>
                                    </td>
                                    <td class="trail-content-info">
                                        <div class="trail-scroller-content">
                                            <table>
                                                <tr>
                                                    <asp:Repeater ID="rptPontosSebraeUsuario" runat="server" OnItemDataBound="rptPontosSebraeUsuario_ItemDataBound">
                                                        <ItemTemplate>
                                                            <td>
                                                                <table>
                                                                    <tbody>

                                                                        <tr>
                                                                            <th class="nome"><%#Eval("Nome") %></th>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="CargaHoraria"><%#Eval("CargaHoraria") %>h</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="Objetivo"><%#Eval("Moedas") %></td>
                                                                        </tr>
                                                                    </tbody>
                                                                </table>
                                                            </td>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <hr />
                        <table>
                            <tr>
                                <th>Total Objeto/Solução</th>
                                <td runat="server" class="Moedas" id="rptPontosSebraeUsuarioObjetivos"></td>
                                <th>Total de Horas</th>
                                <td runat="server" class="MoedasProvaFinal" id="rptPontosSebraeUsuarioTotalHoras"></td>
                                <th>Total Moedas</th>
                                <td runat="server" class="Total" id="rptPontosSebraeUsuarioMoedas"></td>
                            </tr>
                        </table>
                    </div>

                    <div class="trail-collapse">
                        <h4><span>-</span> Cursos online UCSebrae</h4>
                        <div class="trail-collapse-content">
                            <table>
                                <tr>
                                    <td class="trail-labels">
                                        <table>
                                            <thead>
                                                <tr>
                                                    <th class="nome">Soluções Educacional</th>
                                                </tr>
                                                <tr>
                                                    <th>Carga Horária</th>
                                                </tr>
                                                <tr>
                                                    <th>Quantidade de Moedas</th>
                                                </tr>
                                                <tr>
                                                    <th>Período de realização</th>
                                                </tr>
                                            </thead>
                                        </table>
                                    </td>
                                    <td class="trail-content-info">
                                        <div class="trail-scroller-content">
                                            <table>
                                                <tbody>
                                                    <tr>
                                                        <asp:Repeater ID="rptsolucaoesDaTrilhaOnline" runat="server" OnItemDataBound="rptsolucaoesDaTrilhaOnline_ItemDataBound">
                                                            <ItemTemplate>
                                                                <td>
                                                                    <table>
                                                                        <tbody>
                                                                            <tr>
                                                                                <th class="nome"><%#Eval("Nome") %></th>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="CargaHoraria"><%#Eval("CargaHoraria") %> h</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="Moedas"><%#Eval("Moedas") %></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="Periodo"><small><%#Eval("DataInicio") %>  <%#Eval("DataFim") != null ? "A<br>" +Eval("DataFim") : "" %></small></td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </td>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </tr>
                                                </tbody>
                                            </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <hr />
                        <table>
                            <tr>
                                <th>Total Objeto/Solução</th>
                                <td runat="server" class="Moedas" id="rptsolucaoesDaTrilhaOnlineObjetivo"></td>
                                <th>Total de Horas</th>
                                <td runat="server" class="Total" id="rptsolucaoesDaTrilhaOnlineTotal"></td>
                                <th>Total Moedas</th>
                                <td runat="server" class="MoedasProvaFinal" id="rptsolucaoesDaTrilhaOnlineMoedasProvaFinal"></td>
                            </tr>
                        </table>
                    </div>

                    <div class="trail-collapse">
                        <h4><span>-</span> Soluções do Trilheiro</h4>
                        <div class="trail-collapse-content">
                            <table>
                                <tr>
                                    <td class="trail-labels">
                                        <table>
                                            <thead>
                                                <tr>
                                                    <th class="nome">Soluções Trilheiro</th>
                                                </tr>
                                                <tr>
                                                    <th>Total de moedas de Prata</th>
                                                </tr>
                                                <tr>
                                                    <th>Total de moedas de Ouro</th>
                                                </tr>
                                                <tr>
                                                    <th>Total de Horas</th>
                                                </tr>
                                            </thead>
                                        </table>
                                    </td>
                                    <td class="trail-content-info">
                                        <div class="trail-scroller-content">
                                            <table>
                                                <tbody>
                                                    <tr>
                                                        <asp:Repeater ID="rptsolucaoesDoTrilheiro" runat="server" OnItemDataBound="rptsolucaoesDoTrilheiro_ItemDataBound">
                                                            <ItemTemplate>
                                                                <td>
                                                                    <table>
                                                                        <tbody>
                                                                            <tr>
                                                                                <th class="nome"><%#Eval("Nome") %></th>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="Moedas"><%#Eval("MoedasPratas") %></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="Moedas"><%#Eval("MoedasOuro") %></td>
                                                                            </tr>                                                                            
                                                                            <tr>
                                                                                <td class="Total"><%#Eval("CargaHoraria") %>h</td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </td>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </tr>
                                                </tbody>
                                            </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>

                    <div class="trail-collapse">
                        <h4><span>-</span> Desempenho Geral</h4>
                        <div class="trail-collapse-content">
                            <table>
                                <tr>
                                    <td class="trail-labels" style="width: 390px;">
                                        <table>
                                            <thead>                                                
                                                <tr>
                                                    <th>Quantidade de moedas</th>
                                                </tr>
                                                <tr>
                                                    <th>Quantidade de medalhas</th>
                                                </tr>
                                                <tr>
                                                    <th>Troféus alcançados</th>
                                                </tr>
                                                <tr>
                                                    <th>Total de Horas certificadas na trilha/nível</th>
                                                </tr>
                                                <tr>
                                                    <th>Total de Horas registradas na trilha/nível</th>
                                                </tr>
                                                <tr>
                                                    <th>Total Objeto/Solução</th>
                                                </tr>
                                            </thead>
                                        </table>
                                    </td>
                                    <td class="trail-content-info">
                                        <div class="trail-scroller-content">
                                            <table>
                                                <tbody>
                                                    <tr>
                                                        <asp:Repeater ID="rptsolucaoesDesempenhoGeral" runat="server" OnItemDataBound="rptsolucaoesDesempenhoGeral_ItemDataBound">
                                                            <ItemTemplate>
                                                                <td>
                                                                    <table>
                                                                        <tbody>                                                                            
                                                                            <tr>
                                                                                <td class="CargaHoraria"><%#Eval("Moedas") %></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="CargaHoraria"><%#Eval("Medalhas") %></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="Moedas"><%#Eval("Trofeus") %></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="MoedasProvaFinal"><%#Eval("HorasCertificadas") %>h</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="Total"><%#Eval("HorasRegistradas") %>h</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="Total"><%#Eval("Solucoes") %></td>
                                                                            </tr>                                                                          
                                                                        </tbody>
                                                                    </table>
                                                                </td>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </tr>
                                                </tbody>
                                            </table>
                                    </td>
                                </tr>
                            </table>
                        </div>                        
                    </div>

                    <br />
                    <hr />
                    <br />
                    
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </asp:Panel>
    <fieldset runat="server" id="componenteGeracaoRelatorio" clientidmode="Static" visible="false">
        <hr />
        <div class="form-group">
            <asp:Label ID="Label1" runat="server" Text="Formato de Saída" AssociatedControlID="rblTipoSaida" />
            <asp:RadioButtonList ID="rblTipoSaida" runat="server" RepeatLayout="UnorderedList" CssClass="form-control file-types">
                <asp:ListItem Value="EXCEL" Selected="True"><i class="icon icon-ms-excel" title="EXCEL"></i></asp:ListItem>
            </asp:RadioButtonList>
        </div>
        <div class="form-group">
            <asp:Button ID="btnGerarRelatorio" runat="server" Text="Gerar Relatório" OnClick="btnGerarRelatorio_Click"
                CssClass="btn btn-primary" />
        </div>
    </fieldset>

    <script language="javascript" type="text/javascript">
        jQuery(function ($) {
            console.log('carregou');
            $("#<%= txtDataLimite.ClientID %>").mask("99/99/9999");
            $("#<%= txtPeriodoInicial.ClientID %>").mask("99/99/9999");
            $("#<%= txtPeriodoFinal.ClientID %>").mask("99/99/9999");

            resizeTable();
            resizeInnerTables();
            jQuery(window).resize(resizeTable);
            jQuery('.trail-collapse h4').on('click', function () {
                var _content = jQuery(this).siblings('.trail-collapse-content');
                _content.toggleClass('collapsed');
                jQuery(this).children('span').text(
                    _content.hasClass('collapsed') ? '+' : '-'
                )
            });
        });
        function resizeTable() {

            jQuery('.trail-scroller-content').each(function () {
                jQuery(this).width(
                    jQuery('.trail-collapse h4').width() - jQuery(this).parent().siblings('.trail-labels').width()
                );
            })
        }
        function resizeInnerTables() {
            jQuery('.trail-scroller-content th.nome').each(function () {

                var _parent = jQuery(this).parent().parent().parent();
                var _textSize = jQuery(this).text().length * 10 + 20;
                _parent.width(_textSize + 'px');
            })
        }
    </script>
</asp:Content>
