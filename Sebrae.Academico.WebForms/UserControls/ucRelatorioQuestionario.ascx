<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucRelatorioQuestionario.ascx.cs" Inherits="Sebrae.Academico.WebForms.UserControls.ucRelatorioQuestionario" %>
<%@ Import Namespace="NHibernate.Util" %>
<%@ Register Src="~/UserControls/ucCategorias.ascx" TagName="ucCategorias" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ucTotalizadorRelatorio.ascx" TagPrefix="uc" TagName="ucTotalizadorRelatorio" %>
<%@ Register Src="~/UserControls/ucSeletorListBox.ascx" TagName="ucSeletorListBox" TagPrefix="uc2" %>

<script type="text/javascript">
    var _preloadedListDemandas = <%= ViewState["_Demandas"] ?? "''" %>;
    AutoCompleteDefine(_preloadedListDemandas, '#txtDemandas', true, true, false);

    var _preloadedListQuestionarios = <%= ViewState["_Questionario"] ?? "''" %>;
    AutoCompleteDefine(_preloadedListQuestionarios, '#txtQuestionario', true, false, true);

    var _preloadedListSE = <%= ViewState["_SE"] ?? "''" %>;
    AutoCompleteDefine(_preloadedListSE, '#txtSolucaoEducacional', true, false, true);

    var _preloadedListOferta = <%= ViewState["_Oferta"] ?? "''" %>;
    AutoCompleteDefine(_preloadedListOferta, '#txtOferta', true, false, true);

    var _preloadedListTurma = <%= ViewState["_Turma"] ?? "''" %>;
    AutoCompleteDefine(_preloadedListTurma, '#txtTurma', false, false, true);
    
</script>
<style>
    .table tbody tr:hover td,
    .table tbody tr:hover th {
      background-color: transparent;
    }
</style>
<div class="panel-group" id="gruporelatorio">
    <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <a role="button" data-toggle="collapse" data-parent="#gruporelatorio" href="#Filtros" aria-expanded="true" aria-controls="collapseOne">Filtrar por
                </a>
            </h6>
        </div>
        <asp:Panel ID="Filtros" runat="server" ClientIDMode="Static" CssClass="panel-collapse collapse in">
            <div class="panel-body">
                <fieldset>
                    <div class="form-group">
                        <asp:Label ID="Label8" runat="server" Text="Tipo de Relatório" AssociatedControlID="rblTipoRelatorio" />
                        <asp:RadioButtonList ID="rblTipoRelatorio" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblTipoRelatorio_OnSelectedIndexChanged" AutoPostBack="True">
                            <asp:ListItem Value="Respondente" Text="Respondente" Selected="True"></asp:ListItem>
                            <asp:ListItem Value="Estatistico" Text="Estatístico"></asp:ListItem>
                            <asp:ListItem Value="Consolidado" Text="Consolidado"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="lblCategoria" runat="server" Text="CATEGORIA" AssociatedControlID="ucCategorias1" />
                        <uc1:ucCategorias ID="ucCategorias1" runat="server" />
                    </div>

                    <div class="form-group" runat="server" id="divTipoQuestionario">
                        <asp:Label ID="Label6" runat="server" Text="Tipo de questionário" AssociatedControlID="ddlTipoQuestionario" />
                        <asp:DropDownList ID="ddlTipoQuestionario" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlTipoQuestionario_OnSelectedIndexChanged"></asp:DropDownList>
                    </div>

                    <div class="form-group" id="divQuestionario" runat="server">
                        <asp:Label ID="Label1" runat="server" AssociatedControlID="txtQuestionario"
                            Text="Questionário" />
                        <asp:TextBox ID="txtQuestionario" ClientIDMode="Static" runat="server" data-mensagemVazia="-- Selecione --"
                            OnTextChanged="txtQuestionario_OnTextChanged"></asp:TextBox>
                    </div>

                    <div class="form-group" id="divTutor" runat="server" visible="false">
                        <asp:Label ID="Label7" runat="server" AssociatedControlID="cbxProfessor" Text="Tutor" />
                        <asp:DropDownList ID="cbxProfessor" runat="server" CssClass="form-control" AutoPostBack="true" 
                            OnSelectedIndexChanged="cbxProfessor_OnSelectedIndexChanged"></asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label2" runat="server" AssociatedControlID="txtSolucaoEducacional" Text="Solução Educacional" />
                        <asp:TextBox ID="txtSolucaoEducacional" ClientIDMode="Static" runat="server" data-mensagemVazia="Selecione um questionário" 
                            OnTextChanged="txtSolucaoEducacional_OnTextChanged"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label3" runat="server" AssociatedControlID="txtOferta" Text="Oferta" />
                        <asp:TextBox ID="txtOferta" ClientIDMode="Static" runat="server" data-mensagemVazia="Selecione uma Solução Educacional" 
                            OnTextChanged="txtOferta_OnTextChanged"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="Label4" runat="server" AssociatedControlID="txtTurma" Text="Turma" />
                        <asp:TextBox ID="txtTurma" ClientIDMode="Static" runat="server" data-mensagemVazia="Selecione uma Oferta" 
                            OnTextChanged="txtTurma_OnTextChanged"></asp:TextBox>
                    </div>
                    
                    <div class="form-group" runat="server" id="divDemandas">
                        <asp:Label ID="Label14" runat="server" Text="Demanda" AssociatedControlID="txtDemandas" />
                        <asp:TextBox ID="txtDemandas" ClientIDMode="Static" runat="server" OnTextChanged="txtDemandas_OnTextChanged"></asp:TextBox>
                    </div>

                    <div class="form-group" runat="server" id="divStatus">
                        <asp:Label ID="Label9" runat="server" Text="Status" AssociatedControlID="ListBoxesStatus" />
                        <uc2:ucSeletorListBox runat="server" ID="ListBoxesStatus" />
                    </div>
                    <div class="form-group" runat="server" id="divUf">
                        <asp:Label ID="Label5" runat="server" Text="UF" AssociatedControlID="ListBoxesUF" />
                        <uc2:ucSeletorListBox runat="server" ID="ListBoxesUF" />
                    </div>
                    <div class="form-group" runat="server" id="divNivelOcupacional">
                        <asp:Label ID="Label10" runat="server" Text="Nível Ocupacional" AssociatedControlID="ListBoxesNivelOcupacional" />
                        <uc2:ucSeletorListBox runat="server" ID="ListBoxesNivelOcupacional" />
                    </div>
                </fieldset>
            </div>
        </asp:Panel>

        <div class="panel-heading" id="filtroCamposExibidos" runat="server" visible="False">
            <h6 class="panel-title">
                <a role="button" data-toggle="collapse" href="#Campos" aria-expanded="true" aria-controls="collapseOne">Campos a serem exibidos</a>
            </h6>
        </div>
        <asp:Panel runat="server" ID="pnlFiltroCampos" Visible="True">
            <div id="Campos" class="accordion-body collapse">
                <div class="accordion-inner">
                    <fieldset>
                        <div class="form-group">
                            <asp:CheckBoxList ID="chkListaCamposVisiveis" runat="server" RepeatDirection="Vertical"
                                RepeatLayout="UnorderedList" ClientIDMode="Static">
                                <asp:ListItem Selected="True" Value="Questionario">Questionario</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Curso">Curso</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Nome">Nome</asp:ListItem>
                                <asp:ListItem Selected="True" Value="NivelOcupacional">Nivel Ocupacional</asp:ListItem>
                                <asp:ListItem Selected="True" Value="UF">UF</asp:ListItem>
                                <asp:ListItem Selected="True" Value="Data">Data</asp:ListItem>
                            </asp:CheckBoxList>
                        </div>
                    </fieldset>
                </div>
            </div>
        </asp:Panel>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading">
            <h6 class="panel-title">
                <a role="button" data-toggle="collapse" data-parent="#gruporelatorio" href="#CamposRespondente" aria-expanded="true" aria-controls="collapseTwo">Campos a serem exibidos
                </a>
            </h6>
        </div>
        <div id="CamposRespondente" class="accordion-body collapse">
            <div class="accordion-inner">
                <fieldset>
                    <div class="form-group">
                        <asp:CheckBoxList ID="chkListaCamposVisiveisRespondente" runat="server" RepeatDirection="Vertical"
                            RepeatLayout="UnorderedList" ClientIDMode="Static">
                            <asp:ListItem Selected="True" Enabled="False" Value="Questionario">Questionário</asp:ListItem>
                            <asp:ListItem Selected="True" Value="Curso">Curso</asp:ListItem>
                            <asp:ListItem Selected="True" Enabled="False" Value="Nome">Nome</asp:ListItem>
                            <asp:ListItem Selected="True" Value="NivelOcupacional">Nível Ocupacional</asp:ListItem>
                            <asp:ListItem Selected="True" Value="UF">UF</asp:ListItem>
                            <asp:ListItem Selected="True" Value="Data">Data</asp:ListItem>
                            <asp:ListItem Selected="True" Value="Tutor">Tutor/Professor</asp:ListItem>
                        </asp:CheckBoxList>
                    </div>
                </fieldset>
            </div>
        </div>
    </div>
    <br />
    <asp:Button ID="btnPesquisar" runat="server" CssClass="btn btn-primary mostrarload"
        Text="Consultar" OnClick="btnPesquisar_Click" />
    <hr />

    <div id="divRelatorioRespondente" runat="server" visible="False">

        <uc:ucTotalizadorRelatorio runat="server" ID="ucTotalizadorRelatorio" NomeTotalizador="Total de registros encontrados" />

        <div class="table-responsive">
            <table class="table table-hover">
                <thead>
                    <tr>
                        <th class="FiltroQuestionario" rowspan="2">Questionário</th>

                        <%= chkListaCamposVisiveisRespondente.Items.FindByValue("Curso").Selected
                                ? "<th class='FiltroCurso' rowspan='2'>Curso</th>"
                                :"" %>

                        <th class="FiltroNome" rowspan="2">Nome</th>

                        <%= chkListaCamposVisiveisRespondente.Items.FindByValue("NivelOcupacional").Selected
                                ? "<th class='FiltroNivelOcupacional' rowspan='2'>Nível Ocupacional</th>"
                                :"" %>
                        
                        <%= chkListaCamposVisiveisRespondente.Items.FindByValue("UF").Selected
                                ? "<th class='UF' rowspan='2'>UF</th>"
                                :"" %>
                        
                        <%= chkListaCamposVisiveisRespondente.Items.FindByValue("Data").Selected
                                ? "<th class='FiltroData' rowspan='2'>Data</th>"
                                :"" %>
                        
                        <%--<%= chkListaCamposVisiveisRespondente.Items.FindByValue("Tutor").Selected
                                ? "<th class='Tutor' rowspan='2'>Tutor/Professor</th>"
                                :"" %>--%>

                        <asp:Repeater ID="rptEnunciados" runat="server" OnItemDataBound="rptEnunciados_OnItemDataBound">
                            <ItemTemplate>
                                <asp:Literal ID="ThCabecalho" runat="server"></asp:Literal>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tr>

                    <tr style="background-color: #e88b8b; color: #fff;">
                        <asp:Repeater ID="rptQuestoes" runat="server">
                            <ItemTemplate>
                                <th class="text-center questao"><b><%# Eval("Nome")%></b></th>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptRespostas" runat="server" OnItemDataBound="rptRespostas_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td runat="server" ID="Questionario" class="FiltroQuestionario"><%# Eval("Questionario")%></td>
                                <td runat="server" ID="Curso" class="FiltroCurso"><%# Eval("Curso") ?? "--"%></td>
                                <td runat="server" ID="Nome" class="FiltroNome"><%# Eval("Nome")%></td>
                                <td runat="server" ID="NivelOcupacional" class="FiltroNivelOcupacional"><%# Eval("NivelOcupacional")%></td>
                                <td runat="server" ID="UF" class="FiltroUF"><%# Eval("UF")%></td>
                                <td runat="server" ID="Data" class="FiltroData"><%# Eval("Data")%></td>
                                <%--<td runat="server" ID="Tutor" class="FiltroTutor" style="width: 300px;">
				                    <table style="width: 100%">
					                    <asp:Repeater ID="rptTutores" runat="server">
						                    <ItemTemplate>
							                    <tr>
								                    <td><%# Eval("Tutor") ?? "--"%></td>
							                    </tr>
						                    </ItemTemplate>
					                    </asp:Repeater>
				                    </table>
			                    </td>--%>
			                    <asp:Repeater ID="rptNotasTutor" runat="server" OnItemDataBound="rptNotasTutor_ItemDataBound">
				                    <ItemTemplate>
					                    <td>
						                    <table style="width: 100%; height: 100%;">
							                    <asp:Repeater ID="rptNotas" runat="server">
								                    <ItemTemplate> 
									                    <tr>
										                    <td style="padding: 16px 0;"><%# Eval("NotaTexto") ?? "--"%></td>
									                    </tr>
								                    </ItemTemplate>               
							                    </asp:Repeater>
						                    </table>
					                    </td>
				                    </ItemTemplate>
			                    </asp:Repeater>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>

        <div runat="server" id="divPaginacaoRespondente" onprerender="divPaginacaoRespondente_OnPreRender" visible="False">
            <nav>
                <ul class="pagination">
                    <li runat="server" id="liPaginaAnterior">
                        <a href="#" runat="server" id="btnPaginaAnterior" onserverclick="AlterarPagina_ServerClick" aria-label="PáginaAnterior">
                            <span aria-hidden="true">&laquo;</span>
                        </a>
                    </li>
                    <asp:Repeater ID="rptPaginas" runat="server" OnItemDataBound="rptPaginas_OnItemDataBound">
                        <ItemTemplate>
                            <li runat="server" id="liPagina">
                                <a runat="server" id="linkPagina" onserverclick="AlterarPagina_ServerClick" class="mostrarload">
                                    <%# Eval("Numero") %>
                                </a>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                    <li runat="server" id="liProximaPagina">
                        <a href="#" runat="server" id="btnProximaPagina" onserverclick="AlterarPagina_ServerClick" aria-label="Próxima">
                            <span aria-hidden="true">&raquo;</span>
                        </a>
                    </li>
                </ul>
            </nav>
        </div>
    </div>

    <div id="divRelatorioEstatistico" runat="server" visible="false">
        <asp:GridView runat="server" ID="grdRelatorioEstatistico" AutoGenerateColumns="False" CssClass="table col-sm-12" CellSpacing="1" BorderWidth="1" PageSize="100" AllowSorting="True" GridLines="None">
            <Columns>
                <asp:BoundField DataField="Principal" HeaderText="Categoria" />
                <asp:BoundField DataField="Nome" HeaderText="Tópico Avaliado" />
                <asp:BoundField DataField="Media" HeaderText="Média" />
                <asp:BoundField DataField="DP" HeaderText="DP" />
                <asp:BoundField DataField="Moda" HeaderText="Moda" />
                <asp:BoundField DataField="Min" HeaderText="Min" />
                <asp:BoundField DataField="Max" HeaderText="Max" />
                <asp:BoundField DataField="QtdeItens" HeaderText="QtdeItens" />
                <asp:BoundField DataField="MediaFinal" HeaderText="Média Final" />
            </Columns>
            <HeaderStyle BackColor="#e88b8b" ForeColor="#ffffff" />
            <EmptyDataTemplate>
                <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
            </EmptyDataTemplate>
        </asp:GridView>
    </div>

    <div id="divRelatorioConsolidado" runat="server" visible="false">
        <asp:GridView ID="dgRelatorioConsolidado" runat="server" AutoGenerateColumns="False" AllowSorting="True"
            CssClass="table col-sm-12" GridLines="None" OnSorting="dgRelatorioConsolidado_OnSorting" AllowPaging="True"
            OnPageIndexChanging="dgRelatorioConsolidado_OnPageIndexChanging" PageSize="100">
            <Columns>
                <asp:BoundField DataField="NM_SolucaoEducacional" HeaderText="Solução Educacional" SortExpression="NM_SolucaoEducacional" />
                <asp:BoundField DataField="NM_Oferta" HeaderText="Oferta" SortExpression="NM_Oferta" />
                <asp:BoundField DataField="NM_Turma" HeaderText="Turma" SortExpression="NM_Turma" />
                <asp:BoundField DataField="DT_Inicio" HeaderText="Data Início" SortExpression="DT_Inicio" />
                <asp:BoundField DataField="DT_Final" HeaderText="Data Final" SortExpression="DT_Final" />
                <asp:BoundField DataField="QtdeAlunosTurma" HeaderText="Quantidade de alunos na turma" SortExpression="QtdeAlunosTurma" />
                <asp:BoundField DataField="QtdeAlunosResponderamQuestionario" HeaderText="Quantidade de alunos que respondeu o questionário" SortExpression="QtdeAlunosResponderamQuestionario" />
                <asp:BoundField DataField="QtdeAlunosFinalizaram" HeaderText="Quantidade de alunos que chegaram ao final do curso (Aprovado, Concluído, etc)" SortExpression="QtdeAlunosFinalizaram" />
                <asp:BoundField DataField="PctAlunosQueResponderamQuestionario" HeaderText="Percentual de alunos que responderam o questionário com relação ao total de alunos da turma" SortExpression="PctAlunosQueResponderamQuestionario" />
                <asp:BoundField DataField="PctAlunosFinalizaramQueResponderamQuestionario" HeaderText="Percentual de alunos que responderam o questionário com relação aos que chegaram ao final do curso (Aprovado, Concluído, etc)" SortExpression="PctAlunosFinalizaramQueResponderamQuestionario" />
            </Columns>
            <EmptyDataTemplate>
                <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
            </EmptyDataTemplate>
            <PagerSettings Mode="NumericFirstLast" />
        </asp:GridView>
    </div>

    <fieldset runat="server" id="componenteGeracaoRelatorio" clientidmode="Static" visible="false">
        <hr />
        <div class="form-group">
            <asp:Label ID="lblNome" runat="server" Text="Formato de Saída" AssociatedControlID="rblTipoSaida" />
            <asp:RadioButtonList ID="rblTipoSaida" runat="server" RepeatLayout="UnorderedList" CssClass="form-control file-types">
                <asp:ListItem Value="EXCEL" Selected="True"><i class="icon icon-ms-excel" title="EXCEL"></i></asp:ListItem>
            </asp:RadioButtonList>
        </div>
        <div class="form-group">
            <asp:Button ID="btnGerarRelatorio" runat="server" Text="Gerar Relatório" OnClick="btnGerarRelatorio_Click"
                CssClass="btn btn-primary" />
        </div>
    </fieldset>
</div>
