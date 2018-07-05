<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCursos.ascx.cs" Inherits="Sebrae.Academico.WebForms.UserControls.ucCursos" %>

<asp:GridView ID="dgvMatriculaOferta" runat="server" AutoGenerateColumns="false"
    CssClass="table col-sm-12" GridLines="None" OnRowDataBound="dgvOferta_RowDataBound">
    <Columns>
        <asp:TemplateField HeaderText="Solução Educacional">
            <ItemTemplate>
                <%#Eval("NomeSolucaoEducacional")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Status Matrícula">
            <ItemTemplate>
                <%#Eval("StatusMatriculaFormatado")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Turma">
            <ItemTemplate>
                <itemtemplate>
                <%#Eval("NomeTurma")%>
            </itemtemplate>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>                
                <asp:LinkButton ID="lbEmitirCertificado" 
                                runat="server" 
                                CausesValidation="False" 
                                CommandName="emitirCertificado" 
                                Text="Emitir Certificado"
                                ToolTip="Emitir Certificado" 
                                CommandArgument='<%# Eval("ID")%>' 
                                OnClick="btnCertificado_Click">
                    <span class="btn btn-default btn-xs">
						<span class="glyphicon glyphicon-save"></span>
					</span>
                </asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField ShowHeader="False">
            <ItemTemplate>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <EmptyDataTemplate>
        <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
    </EmptyDataTemplate>
</asp:GridView>
