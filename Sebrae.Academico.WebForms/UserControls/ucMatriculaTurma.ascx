<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucMatriculaTurma.ascx.cs"
    Inherits="Sebrae.Academico.WebForms.UserControls.ucMatriculaTurma" %>
<fieldset>
    <div class="form-group">
        Status Matrícula
        <asp:TextBox ID="txtStatusMatricula" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
    </div>
    <div class="form-group">
        Data da Matrícula
        <asp:TextBox ID="txtDataMatricula" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
    </div>
    
    <div class="form-group">
        Data Limite:
        <asp:TextBox ID="txtDataLimite" runat="server" class="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        Data de Conclusão
        <asp:TextBox ID="txtDataTermino" runat="server" class="form-control"></asp:TextBox>
    </div>
    <div id="divNotas" runat="server">
        <div class="form-group">
            Nota 1:
            <asp:TextBox ID="txtNota1" runat="server" class="form-control" onkeypress="return EhNumericoOuVirgula(event)" MaxLength="5"></asp:TextBox>
        </div>
        <div class="form-group">
            Nota 2:
            <asp:TextBox ID="txtNota2" runat="server" class="form-control" onkeypress="return EhNumericoOuVirgula(event)" MaxLength="5"></asp:TextBox>
        </div>
        <div class="form-group">
            Média Final
            <asp:TextBox ID="txtMediaFinal" runat="server" class="form-control" onkeypress="return EhNumericoOuVirgula(event)" MaxLength="5"></asp:TextBox>
        </div>
    </div>
    <div class="form-group">
        Observações                
        <asp:TextBox ID="txtObservacoes" TextMode="MultiLine" MaxLength="60" runat="server"
            CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        Feedback                
        <asp:TextBox ID="txtFeedback" TextMode="MultiLine" MaxLength="60" runat="server"
            CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        Presenças Coletadas
        <asp:TextBox runat="server" ID="txtPresencas" ReadOnly="true" CssClass="form-control"></asp:TextBox>
    </div>
    <div class="form-group">
        Total de Presenças
        <asp:TextBox runat="server" ID="txtPresencasTotais" ReadOnly="true" CssClass="form-control"></asp:TextBox>
    </div>
    <br />
    <asp:Panel ID="pnlProvasRealizadas" runat="server" Visible="false">
        <h4>
            Provas Realizadas</h4>
        <asp:GridView ID="dgvProvasRealizadas" CssClass="table col-sm-12" runat="server"
            OnRowDataBound="dgvProvasRealizadas_RowDataBound" AutoGenerateColumns="false"
            GridLines="None" OnRowCommand="dgvProvasRealizadas_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="Data da Geração">
                    <ItemTemplate>
                        <%#Eval("DataGeracao")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Data/Hora de Participação">
                    <ItemTemplate>
                        <%#Eval("DataParticipacao")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Nota da Prova">
                    <ItemTemplate>
                        <asp:Label ID="lblNotaProva" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tipo">
                    <ItemTemplate>
                        <asp:Label ID="lblTipoQuestionarioAssociacao" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <ItemStyle Width="90px" HorizontalAlign="center" />
                    <ItemTemplate>
                        <asp:LinkButton ID="lkbVerProva" runat="server" CausesValidation="False" CommandName="verprova" CssClass="mostrarload"
                            CommandArgument='<%# Eval("ID")%>' Text="Ver Detalhes" ToolTip="Ver Informações detalhadas">
                        <span class="btn btn-default btn-xs">
						    <span class="glyphicon glyphicon-pencil"></span>
						</span>
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <itemtemplate>Nenhuma prova encontrada</itemtemplate>
            </EmptyDataTemplate>
        </asp:GridView>
    </asp:Panel>
    <asp:Button ID="btnEnviar" runat="server" CausesValidation="true" OnClick="btnEnviar_Click" CssClass="btn btn-default mostrarload"
        Text="Enviar" />
    <br />
</fieldset>
    <script type="text/javascript" src="/js/jquery.maskMoney.min.js"></script>
    <script type="text/javascript">
        jQuery(function ($) {
            $("#<%= txtDataLimite.ClientID %>, #<%= txtDataTermino.ClientID %>, #<%= txtDataMatricula.ClientID %>").mask("99/99/9999");

            $("#<%= txtNota1.ClientID %>, #<%= txtNota2.ClientID %>, #<%= txtMediaFinal.ClientID %>")
                .maskMoney({ allowNegative: false, decimal: ',', precision: 2 });
        });
    </script>
