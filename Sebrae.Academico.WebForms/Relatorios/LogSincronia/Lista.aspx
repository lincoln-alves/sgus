<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true"
    CodeBehind="Lista.aspx.cs" Inherits="Sebrae.Academico.WebForms.Relatorios.LogSincronia.Lista" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <div class="form-group">
            <asp:Button ID="btnSincronizar" runat="server" Text="Sincronizar Pendentes" CssClass="btn btn-primary mostrarload" OnClick="btnSincronizar_Click" />
        </div>
    </fieldset>
    <fieldset>
        <asp:GridView ID="dgRelatorio" runat="server" AutoGenerateColumns="False" OnRowCommand="dgvRelatorio_RowCommand" AllowSorting="True" CssClass="table col-sm-12" GridLines="None" OnSorting="dgRelatorio_Sorting" AllowPaging="True" OnPageIndexChanging="dgRelatorio_PageIndexChanging" PageSize="10">
            <Columns>
                <asp:BoundField DataField="DataCriacao" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Data da geração" SortExpression="DataCriacao" />
                <asp:TemplateField HeaderText="Usuário">
                    <ItemTemplate>
                        <%#Eval("Usuario.Nome")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Url">
                    <ItemTemplate>
                        <%#Eval("Url")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ação">
                    <ItemTemplate>
                        <%#Eval("NomeAcao")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False" ItemStyle-Width="90px">
                    <ItemStyle HorizontalAlign="center" />
                    <ItemTemplate>
                        <asp:LinkButton ID="lkbSincronizar" runat="server" CausesValidation="False" CommandName="sincronizar" CommandArgument='<%# Eval("ID")%>' ToolTip="Sincronizar" CssClass="mostrarload">
                                <span class="btn btn-default btn-xs">
								    <span class="glyphicon glyphicon-refresh"></span>
							    </span>
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <itemtemplate>Nenhuma sincronização está pendente.</itemtemplate>
            </EmptyDataTemplate>
        </asp:GridView>
    </fieldset>
</asp:Content>
