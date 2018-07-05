<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="GerenciarOfertaRecusaVaga.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.Vagas.GerenciarOfertaRecusaVaga" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <div class="form-group">
            <asp:Label ID="lblExibir" runat="server" Text="Exibir" AssociatedControlID="rblExibir" />
            <asp:RadioButtonList ID="rblExibir" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblExibir_SelectedIndexChanged" CssClass="form-control" RepeatDirection="Horizontal">
                <asp:ListItem Value="Novas" Text="Novas" Selected="True"></asp:ListItem>
                <asp:ListItem Value="Respondidas" Text="Respondidas"></asp:ListItem>
                <asp:ListItem Value="NaoRespondidas" Text="Não Respondidas"></asp:ListItem>
                <asp:ListItem Value="Aprovadas" Text="Aprovadas"></asp:ListItem>
                <asp:ListItem Value="Recusadas" Text="Recusadas"></asp:ListItem>
            </asp:RadioButtonList>
        </div>
        <div class="form-group">
            <asp:GridView ID="gvOferta" runat="server" CssClass="table col-sm-12" GridLines="None" 
            OnRowCommand="gvOferta_RowCommand" OnRowDataBound="gvOferta_OnRowDataBound" AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField HeaderText="NOme">
                    <ItemTemplate>
                        <%# Eval("Usuario.Nome") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Solução Educacional">
                    <ItemTemplate>
                        <%# Eval("Oferta.SolucaoEducacional.Nome")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Oferta">
                    <ItemTemplate>
                        <%# Eval("Oferta.Nome")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="UF">
                    <ItemTemplate>
                         <%# Eval("UF.Sigla")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Recusas">
                    <ItemTemplate>
                         <%# Eval("VagasRecusadas")%>/<%# Eval("VagasAnteriores")%>
                    </ItemTemplate>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Vigente">
                    <ItemTemplate>
                         <%# Convert.ToBoolean(Eval("Vigente")) ? "Sim" : "Não" %>
                    </ItemTemplate>
                </asp:TemplateField>  
                <asp:TemplateField HeaderText="Contemplado">
                    <ItemTemplate>
                         <%# Convert.ToBoolean(Eval("Contemplado")) ? "Sim" : "Não" %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:LinkButton ID="lkbEditar" runat="server" CausesValidation="False" CommandName="editar"
                            CssClass="btn btn-default btn-xs" CommandArgument='<%# Eval("ID")%>' Text="Editar">
                            <span class="glyphicon glyphicon-pencil"></span></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <asp:Panel ID="pnlLupaOfertaHistoricoVagas" runat="server" Visible="false">
        <div class="modal fade in" id="OfertaHistoricoVagas" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: block;">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                    <button id="Button5" type="button" class="close" data-dismiss="modal" aria-hidden="true" onserverclick="OcultarModal_Click"
                            runat="server" >
                            &times;</button>
                            <h4 class="modal-title">Oferta</h4>
                    </div>
                    <div class="modal-body">                          
                        <div class="form-group">
                            <asp:Label ID="Label2" runat="server" Text="Solução Educacional" />
                            <asp:TextBox ID="txtSeModal" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label4" runat="server" Text="Oferta" />
                            <asp:TextBox ID="txtOfertaModal" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label5" runat="server" Text="UF" />
                            <asp:TextBox ID="txtUfModal" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label6" runat="server" Text="Quantidade de vagas originalmente para a UF" />
                            <asp:TextBox ID="txtQtdeVagasUfModal" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label1" runat="server" Text="Quantidade de vagas recusadas para a UF" />
                            <asp:TextBox ID="txtQtdeVagasRecusadas" runat="server" Enabled="false" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label3" runat="server" Text="Observação" />
                            <asp:TextBox ID="txtObservacaoRecusa" runat="server" Enabled="false" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:RadioButtonList ID="rblAprovar" runat="server" CssClass="form-control" RepeatDirection="Horizontal">
                                <asp:ListItem Value="Aprovar" Text="Aprovar"></asp:ListItem>
                                <asp:ListItem Value="Recusar" Text="Recusar"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="Label7" runat="server" Text="Observação" />
                            <asp:TextBox ID="txtResposta" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Button ID="btnEnviarObservacao" runat="server" Text="Enviar" CssClass="btn btn-primary" OnClick="btnEnviarObservacao_OnClick" OnClientClick="return ConfirmarExclusao('Enviar esta observação?');" />
                        </div>
                        <div class="form-group">
                            <asp:GridView ID="gvOfertaGerenciadorVaga" runat="server" CssClass="table col-sm-12" GridLines="None" 
                                OnRowCommand="gvOfertaGerenciadorVaga_RowCommand" OnRowDataBound="gvOfertaGerenciadorVaga_OnRowDataBound" AutoGenerateColumns="false" DataKeyNames="ID">
                                <Columns>
                                    <asp:TemplateField HeaderText="Usuário">
                                        <ItemTemplate>
                                             <%# Eval("Usuario.Nome") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="UF">
                                        <ItemTemplate>
                                             <%# Eval("UF.Sigla")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Descrição">
                                        <ItemTemplate>
                                             <%# Eval("Descricao")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Data">
                                        <ItemTemplate>
                                             <%# Eval("DataAlteracao")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Responder">
                                        <ItemTemplate>
                                             <asp:RadioButton ID="rblResponder" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Respondido">
                                        <ItemTemplate>
                                             <asp:CheckBox ID="cbRespondido" Enabled="false" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Marcar Como Visto">
                                        <ItemTemplate>
                                             <asp:CheckBox ID="cbMarcarComoVisto" runat="server" AutoPostBack="true" OnCheckedChanged="cbMarcarComoVisto_OnCheckedChanged" ToolTip='<%# Eval("ID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
        </fieldset>
</asp:Content>
