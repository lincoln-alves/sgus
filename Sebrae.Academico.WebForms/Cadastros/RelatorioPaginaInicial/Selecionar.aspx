<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true"
    CodeBehind="Selecionar.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.RelatorioPaginaInicial.Selecionar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset>
        <asp:GridView ID="gdvRelatoriosPaginaInicial" runat="server" CssClass="table table-striped table-hover" AutoGenerateColumns="False" DataKeyNames="ID" EnableTheming="True" ShowHeader="False" BorderStyle="None" OnRowCreated="gdvRelatoriosPaginaInicial_OnRowCreated">
            <Columns>
                <asp:TemplateField Visible="False">
                    <ItemTemplate>
                        <asp:HiddenField runat="server" ID="paginaID" Value='<%#Eval("ID")%>' />
                    </ItemTemplate>
                    <HeaderStyle BorderStyle="None" />
                    <ItemStyle Width="100px" />
                </asp:TemplateField>

                <asp:TemplateField>
                    <ItemTemplate>
                        <%#Eval("Nome")%>
                    </ItemTemplate>
                    <ItemStyle BorderStyle="None" />
                </asp:TemplateField>

                <asp:TemplateField>
                    <ItemTemplate>
                        <button runat="server" id="btnAlterarPermissoes" onserverclick="btnAlterarPermissoes_OnServerClick" data-id='<%#Eval("ID")%>' class="btn btn-default btn-xs" data-toggle="tooltip" data-title="Alterar permissões" data-placement="left">
                            <span class="glyphicon glyphicon-pencil"></span>
                        </button>
                    </ItemTemplate>
                    <ItemStyle BorderStyle="None" Width="50px" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </fieldset>

    <asp:Panel ID="pnlCadastrarRelatorio" runat="server" Visible="False">
        <div class="modal fade in" id="modalCadastrarOagina" tabindex="-1" role="dialog" aria-labelledby="Cadastrar página" aria-hidden="true" style="display: block;">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button runat="server" type="button" class="close mostrarload" data-dismiss="modal" aria-label="Fechar" onserverclick="btnFecharModal_OnServerClick"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" id="modalCadastrarRelatorioTitulo" runat="server"></h4>
                    </div>
                    <div id="modalCadastrarRelatorioBody" runat="server" class="modal-body">

                        <asp:HiddenField ID="hdnIdRelatorio" runat="server" Value="0" />
                        
                        <div class="form-group">
                            <asp:Label ID="Label2" runat="server" Text="Título*" AssociatedControlID="txtNome" />
                            <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>

                        <div class="form-group" runat="server" Visible="False">
                            <asp:Label ID="Label4" runat="server" Text="Tag (desabilitado)" AssociatedControlID="txtTag" />
                            <asp:TextBox ID="txtTag" runat="server" CssClass="form-control disabled" Enabled="False"></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-6 col-xs-12">
                                    <div class="panel panel-default">
                                        <div class="panel-body">
                                            <asp:CheckBox ID="ckbTodosPerfis" runat="server" Text="&nbsp;Todos os perfis" Checked="True" AutoPostBack="True" CssClass="mostrarload" OnCheckedChanged="ckbTodosPerfis_OnCheckedChanged" />
                                        </div>
                                        <div runat="server" id="divPerfis" class="panel-body" visible="False">
                                            <asp:CheckBoxList ID="ckblPerfis" runat="server"></asp:CheckBoxList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6 col-xs-12">
                                    <div class="panel panel-default">
                                        <div class="panel-body">
                                            <asp:CheckBox ID="ckbTodasUfs" runat="server" Text="&nbsp;Todas as UFs" Checked="True" AutoPostBack="True" CssClass="mostrarload" OnCheckedChanged="ckbTodasUfs_OnCheckedChanged" />
                                        </div>
                                        <div runat="server" id="divUfs" class="panel-body" visible="False">
                                            <asp:CheckBoxList ID="ckblUfs" runat="server"></asp:CheckBoxList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default mostrarload" runat="server" id="btnFecharModal" onserverclick="btnFecharModal_OnServerClick">Fechar</button>
                        <button type="button" class="btn btn-primary mostrarload" runat="server" id="btnSalvarModal" onserverclick="btnSalvarModal_OnServerClick">Salvar</button>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>

</asp:Content>
