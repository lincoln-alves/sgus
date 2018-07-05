<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="MonitoramentoTrilha.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.Monitoramento.MonitoramentoTrilha" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <fieldset>
            <div class="form-group">
                <asp:Label ID="Label1" runat="server" Text="Trilha" AssociatedControlID="ddlTrilha"></asp:Label>
                <asp:DropDownList ID="ddlTrilha" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTrilha_OnSelectedIndexChanged">
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:Label ID="Label2" runat="server" Text="Nível da Trilha" AssociatedControlID="ddlTrilhaNivel"></asp:Label>
                <asp:DropDownList ID="ddlTrilhaNivel" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTrilhaNivel_OnSelectedIndexChanged"></asp:DropDownList>
            </div>
            <div class="form-group">
                <asp:Label ID="Label3" runat="server" Text="Tópico Temático" AssociatedControlID="ddlTopicoTematico"></asp:Label>
                <asp:DropDownList ID="ddlTopicoTematico" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTopicoTematico_OnSelectedIndexChanged"></asp:DropDownList>
            </div>
            <div class="form-group" runat="server" id="divDataEnvioInicio">
                <asp:Label ID="lblDataEnvioInicio" runat="server" Text="Data de Envio Inicial" AssociatedControlID="dataEnvioInicio"  />
                <asp:TextBox ID="dataEnvioInicio" runat="server" CssClass="form-control" MaxLength="10" OnTextChanged="dataEnvioInicio_OnTextChanged" AutoPostBack="True"></asp:TextBox>
            </div>
            <div class="form-group" runat="server" id="divDataEnvioFinal">
                <asp:Label ID="lblDataEnvioFinal" runat="server" Text="Data de Envio Final" AssociatedControlID="dataEnvioFinal" />
                <asp:TextBox ID="dataEnvioFinal" runat="server" CssClass="form-control" MaxLength="10" OnTextChanged="dataEnvioFinal_OnTextChanged" AutoPostBack="True"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="Label15" runat="server" Text="Status"></asp:Label>
                <asp:CheckBoxList ID="cblStatus" runat="server" CssClass="form-control" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="cblStatus_OnSelectedIndexChanged">
                    <asp:ListItem>Aprovadas</asp:ListItem>
                    <asp:ListItem Selected="True">Em revisão</asp:ListItem>
                    <asp:ListItem>Pendentes</asp:ListItem>
                    <asp:ListItem>Suspensas</asp:ListItem>
                </asp:CheckBoxList>
            </div>
            <div class="form-group">
                <uc:LupaUsuario ID="ucLupaUsuario" runat="server" OnUserSelected="ucLupaUsuario_UserSelected" />
                <asp:Button runat="server" Text="Limpar Usuário" ID="btnLimparUsuario" OnClick="btnLimparUsuario_Click" CssClass="btn btn-default" />
            </div>
        </fieldset>
    </div>
    <asp:Panel ID="pnlSolucoesEducacionais" runat="server">
        <h4>Soluções Sebrae</h4>
        <asp:Literal ID="litTable" runat="server" />
        <asp:GridView ID="gvSolucoesEducacionais" runat="server" CssClass="table col-sm-12" AutoGenerateColumns="false" GridLines="None" OnRowDataBound="gvSolucoesEducacionais_RowDataBound" OnRowCommand="gvSolucoesEducacionais_RowCommand" AllowPaging="True" OnPageIndexChanging="gvSolucoesEducacionais_PageIndexChanging" PageSize="10">
            <Columns>
                <asp:TemplateField HeaderText="Usuário">
                    <ItemTemplate>
                        <%#Eval("UsuarioTrilha.Usuario.Nome")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Trilha">
                    <ItemTemplate>
                        <%#Eval("ItemTrilha.TrilhaNivel.Trilha.Nome")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status da Matrícula">
                    <ItemTemplate>
                        <%#Eval("StatusFormatado")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Nível">
                    <ItemTemplate>
                        <%#Eval("ItemTrilha.TrilhaNivel.Nome")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tópico Tematico">
                    <ItemTemplate>
                        <%#Eval("ItemTrilha.TrilhaTopicoTematico.NomeExibicao")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Solução">
                    <ItemTemplate>
                        <%#Eval("ItemTrilha.Nome")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Participação">
                    <ItemTemplate>
                        <%#Eval("TextoParticipacao")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <%#Eval("AutorizadoFormatado")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Data de envio">
                    <ItemTemplate>
                        <%#Eval("DataEnvio", "{0:dd/MM/yyyy}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Prazo para avaliação">
                    <ItemTemplate>
                        <%#Eval("DataPrazoAvaliacao", "{0:dd/MM/yyyy}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Data da avaliação">
                    <ItemTemplate>
                        <%#Eval("DataAvaliacao", "{0:dd/MM/yyyy}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ação">
                    <ItemTemplate>
                        <asp:LinkButton ID="lkbEditarMatriculaTurma" CommandArgument='<%# Eval("ID") %>' runat="server" CausesValidation="False" CssClass="mostrarload" Text="Monitorar" ToolTip="Monitorar participação">
                            <span class="btn btn-default btn-xs">
								<span class="glyphicon glyphicon-pencil"></span>
							</span>
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <itemtemplate>Nenhum registro encontrado</itemtemplate>
            </EmptyDataTemplate>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="pnlItemTrilha" runat="server" Visible="false">
        <div class="modal fade in" id="ModalItemTrilhaAprovacao" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
            aria-hidden="true" style="display: block;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button id="Button2" type="button" class="close" data-dismiss="modal" aria-hidden="true" onserverclick="OcultarItemTrilha_Click"
                            runat="server">
                            &times;</button>
                        <h4 class="modal-title">Soluções Educacionais Autoindicativas</h4>
                    </div>
                    <div class="modal-body">
                        <fieldset>
                            <div class="form-group">
                                <div class="form-group">
                                    <asp:Label ID="Label23" runat="server" Text="Tipo"></asp:Label><br />
                                    <asp:Label ID="lblTipoItemTrilha" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="Label24" runat="server" Text="Título"></asp:Label><br />
                                    <asp:Label ID="lblTituloItemTrilha" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="Label26" runat="server" Text="Objetivo"></asp:Label><br />
                                    <asp:Label ID="lblOjetivoItemTrilha" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="Label27" runat="server" Text="Link de Acesso"></asp:Label><br />
                                    <asp:HyperLink ID="lblLinkAcessoItemTrilha" CssClass="linkWRAP" runat="server" Target="_blank"></asp:HyperLink>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="Label28" runat="server" Text="Referência bibliográfica"></asp:Label><br />
                                    <asp:Label ID="lblReferenciaBibliograficaItemTrilha" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="Label29" runat="server" Text="Local"></asp:Label><br />
                                    <asp:Label ID="lblLocalItemTrilha" runat="server" Text=""></asp:Label>
                                </div>
                                <div id="divCargaHoraria" class="form-group" runat="server">
                                    <asp:Label ID="lblCargaHoraria" runat="server" Text="Carga Horária"></asp:Label><br />
                                    <asp:TextBox ID="txtCargHoraria" runat="server" Text="" />
                                </div>
                                <div class="form-group">
                                    <asp:Label ID="Label30" runat="server" Text="Observação"></asp:Label><br />
                                    <CKEditor:CKEditorControl ID="txtObservacaoItemTrilha" BasePath="/js/ckeditor/" runat="server"></CKEditor:CKEditorControl>
                                </div>

                                <asp:Button runat="server" Text="Aprovar" CssClass="btn" OnClick="btnAprovar_OnClick" OnClientClick="return ValidarAprovacao('Aprovar esta Solução Educacional?');" />
                                <asp:Button runat="server" Text="Não Aprovar" CssClass="btn" OnClick="btnReprovar_OnClick" OnClientClick="return ValidarAprovacao('Não Aprovar esta Solução Educacional?');" />
                            </div>
                        </fieldset>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlTrilhaParticipacao" runat="server" Visible="false">
        <div class="modal fade in" id="ModalMatriculaTurma" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
            aria-hidden="true" style="display: block;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button id="Button1" type="button" class="close" data-dismiss="modal" aria-hidden="true" onserverclick="OcultarMatriculaTurma_Click"
                            runat="server">
                            &times;</button>
                        <h4 class="modal-title">Participação</h4>
                    </div>
                    <div class="modal-body">
                        <fieldset>
                            <div class="form-group">
                                <asp:Label ID="Label18" runat="server" Text="Orientação de Participação"></asp:Label>
                                - <a href="javascript:void(0);" data-div="div-orientacaoParticipacao" class="mostrar_esconder">Esconder/Mostrar</a><br />
                                <div id="div-orientacaoParticipacao">
                                    <asp:Label ID="lblOrientacaoParticipacao" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                            <div id="divObjetivoServer" class="form-group" clientidmode="Static" runat="server">
                                <asp:Label ID="Label19" runat="server" Text="Objetivo"></asp:Label>
                                - <a href="javascript:void(0);" data-div="div-objetivo" class="mostrar_esconder">Esconder/Mostrar</a><br />
                                <div id="div-objetivo">
                                    <asp:Label ID="lblObjetivo" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                            <div id="divFormaAquisicao" class="form-group" clientidmode="Static" runat="server">
                                <asp:Label ID="Label21" runat="server" Text="Forma Aquisição"></asp:Label>
                                - <a href="javascript:void(0);" data-div="div-formaAquisicao" class="mostrar_esconder">Esconder/Mostrar</a><br />
                                <div id="div-formaAquisicao">
                                    <asp:Label ID="lblFormaAquisicao" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                            <div id="divReferenca" class="form-group" clientidmode="Static" runat="server">
                                <asp:Label ID="Label25" runat="server" Text="Referência bibliografica"></asp:Label>
                                - <a href="javascript:void(0);" data-div="div-referenciaBibliografica" class="mostrar_esconder">Esconder/Mostrar</a><br />
                                <div id="div-referenciaBibliografica">
                                    <asp:Label ID="lblReferenciaBibliografica" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                            <div id="divLinkConteudo" class="form-group" clientidmode="Static" runat="server">
                                <asp:Label ID="Label22" runat="server" Text="Link do conteúdo"></asp:Label>
                                - <a href="javascript:void(0);" data-div="div-linkConteudo" class="mostrar_esconder">Esconder/Mostrar</a><br />
                                <div id="div-linkConteudo">
                                    <asp:HyperLink ID="lblLinkConteudo" runat="server" Target="_blank"></asp:HyperLink>
                                </div>
                            </div>
                            <div id="divLinkAcesso" class="form-group" clientidmode="Static" runat="server">
                                <asp:Label ID="Label20" runat="server" Text="Link do anexo"></asp:Label>
                                - <a href="javascript:void(0);" data-div="div-linkAcesso" class="mostrar_esconder">Esconder/Mostrar</a><br />
                                <div id="div-linkAcesso">
                                    <asp:HyperLink ID="lblLinkAcessoConteudo" runat="server" Target="_blank"></asp:HyperLink>
                                </div>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label6" runat="server" Text="Usuário" AssociatedControlID="txtUsuarioModal"></asp:Label>
                                <asp:TextBox ID="txtUsuarioModal" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label5" runat="server" Text="Trilha" AssociatedControlID="txtTrilhaModal"></asp:Label>
                                <asp:TextBox ID="txtTrilhaModal" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label7" runat="server" Text="Nível" AssociatedControlID="txtNivelModal"></asp:Label>
                                <asp:TextBox ID="txtNivelModal" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label8" runat="server" Text="Tópico Temático" AssociatedControlID="txtTopicoTematicoModal"></asp:Label>
                                <asp:TextBox ID="txtTopicoTematicoModal" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </div>
                            <div class="form-group" runat="server" id="dvSolucaoEducacionalModal">
                                <asp:Label ID="Label9" runat="server" Text="Solução Educacional" AssociatedControlID="txtSolucaoEducacionalModal"></asp:Label>
                                <asp:TextBox ID="txtSolucaoEducacionalModal" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label10" runat="server" Text="Participação" AssociatedControlID="txtParticipacaoModal"></asp:Label>
                                <asp:TextBox ID="txtParticipacaoModal" TextMode="MultiLine" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label11" runat="server" Text="Validação" AssociatedControlID="txtAutorizado"></asp:Label>
                                <asp:TextBox ID="txtAutorizado" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label14" runat="server" Text="Arquivo" AssociatedControlID="hplnkCaminhoArquivo"></asp:Label><br />
                                <asp:HyperLink ID="hplnkCaminhoArquivo" runat="server"></asp:HyperLink>
                            </div>
                            <div class="form-group">
                                <asp:GridView ID="gvHistoricoParticipacaoModal" runat="server" CssClass="table col-sm-12" AutoGenerateColumns="false" GridLines="None" OnRowDataBound="gvHistoricoParticipacaoModal_RowDataBound">
                                    <Columns>
                                        <asp:BoundField HeaderText="Tipo" DataField="TipoParticipacaoFormatado" />
                                        <asp:BoundField HeaderText="Monitor" DataField="NomeMonitor" />
                                        <asp:BoundField HeaderText="Data do Envio" DataField="DataAlteracao" />
                                        <asp:BoundField HeaderText="Observação" DataField="TextoParticipacao" />
                                        <asp:TemplateField HeaderText="Anexo" HeaderStyle-HorizontalAlign="center">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="hplnkCaminhoArquivo" runat="server"></asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Visualizado" DataField="VisualizadoFormatado" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label13" runat="server" Text="Observação" AssociatedControlID="txtObservacao"></asp:Label>
                                <asp:TextBox ID="txtObservacao" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="Label12" runat="server" Text="Validação" AssociatedControlID="rblAutorizadoModal"></asp:Label>
                                <asp:RadioButtonList ID="rblAutorizadoModal" runat="server" CssClass="form-control" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblAutorizadoModal_OnSelectedIndexChanged" AutoPostBack="True">
                                    <asp:ListItem Value="S" Text="Aprovado"></asp:ListItem>
                                    <asp:ListItem Value="N" Text="Não aprovado"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div class="form-group" id="divOrientacao" runat="server" visible="False">
                                <asp:Label ID="Label17" runat="server" Text="Orientação ao trilheiro" AssociatedControlID="txtOrientacao"></asp:Label>
                                <asp:TextBox ID="txtOrientacao" TextMode="MultiLine" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>

                            <div class="form-group">
                                <asp:Label ID="Label16" runat="server" Text="Anexo" AssociatedControlID="fupldArquivoEnvio" />
                                <asp:FileUpload ID="fupldArquivoEnvio" runat="server" />
                            </div>
                            <div class="form-group">
                                <asp:CheckBox ID="chkEnviarEmail" runat="server" Text="Enviar mensagem por email" CssClass="form-control" Checked="true" />
                            </div>


                            <div class="form-group">
                                <asp:Button ID="btnEnviarObservacao" runat="server" Text="Enviar Observação e Status" CssClass="btn btn-primary" OnClick="btnEnviarObservacao_OnClick" OnClientClick="return ValidarTextoObservacao();" />
                            </div>
                        </fieldset>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <script type="text/javascript">
        var arrayObj = [];
        jQuery(function ($) {
            $('.mostrar_esconder').on('click', function () {
                var div = $(this).attr('data-div');
                if (!arrayObj[div]) {
                    arrayObj[div] = $('#' + div);
                    console.log('ok');
                }
                arrayObj[div].toggle();
            });

            $('#<%= dataEnvioInicio.ClientID %>').mask("99/99/9999", {
                autoclear: true
            });

            $('#<%= dataEnvioFinal.ClientID %>').mask("99/99/9999", {
                autoclear: true
            });
        });
        function ValidarAprovacao(texto) {
            return confirm(texto);
        }
        function ValidarTextoObservacao() {
            if (confirm('Enviar esta observação?')) {
                var qtd = $("#<%= txtObservacao.ClientID %>").val().length;

                if (typeof $('#<%=rblAutorizadoModal.ClientID %> input:checked').val() === "undefined") {
                    alert("O status é obrigatório");
                    return false;

                }

                <%--   if ($('#<%=rblAutorizadoModal.ClientID %> input:checked').val() == "N"  && qtd < 50) {
                    alert("A observação é muito curta");
                    return false;

                }--%>


                //MostarLoading();
                //return true;
            }
            else {
                return false;
            }
        }

    </script>
</asp:Content>

