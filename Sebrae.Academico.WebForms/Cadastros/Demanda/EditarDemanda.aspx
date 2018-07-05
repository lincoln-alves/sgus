<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true"
    CodeBehind="EditarDemanda.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.Demanda.EditarDemanda" %>

<%@ Register Src="~/UserControls/ucHelperTooltip.ascx" TagPrefix="uc1" TagName="ucHelperTooltip" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="/js/jqueryUI/processo.js" type="text/javascript"></script>
    <link href="/css/fakeTable.css" rel="stylesheet" type="text/css" />
    <fieldset>
        <div class="form-group">
            <asp:Label ID="Label2" runat="server" Text="Nome*" AssociatedControlID="txtNome"></asp:Label>
            <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip1" Chave="nomeProcesso" />
            <asp:TextBox ID="txtNome" runat="server" MaxLength="150" CssClass="form-control"></asp:TextBox>
        </div>

         <div class="form-group">
            <asp:Label Text="Tipo*" runat="server" AssociatedControlID="ddlTipo" />
             <asp:DropDownList ID="ddlTipo" runat="server" CssClass="form-control">
             </asp:DropDownList>
         </div>

        <asp:Panel ID="pnlProcesso" runat="server" Visible="false">
            <div class="row fakeTh">
                <div class="col-xs-8">
                    Etapas
                </div>
                <div class="col-xs-4">
                    Solicitações por etapa
                </div>
            </div>
            <div class="ordenacaoHidden">
                <asp:HiddenField ID="hdnOrdenacao" runat="server" />
            </div>
            <ul class="fakeTable EtapasContainer boostraped">
                <asp:Repeater ID="rptEtapas" runat="server" OnItemDataBound="Repeater_OnItemDataBound">
                    <HeaderTemplate>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li class="row" guid="<%# DataBinder.Eval(Container, "DataItem.ID")%>" data-ordem="<%# DataBinder.Eval(Container, "DataItem.Ordem")%>">                           
                            <div class="col-xs-8">
                                <div class="ordem">
                                    <%# Container.ItemIndex == 0 ? "Inscrição" : (Container.ItemIndex) + " Etapa"%>
                                </div> 
                                <%# DataBinder.Eval(Container, "DataItem.Nome")%>
                            </div>
                            <div class="col-xs-1">
                                
                                <%# DataBinder.Eval(Container, "DataItem.TotalEtapasAbertas")%>
                            </div>                            
                            <div class="col-xs-2 pull-right text-right">
                                <asp:LinkButton ID="Editar" runat="server" CommandArgument="" OnClick="Editar_Click">
                                        <span class="btn btn-default btn-xs">
                                        <span class="glyphicon glyphicon-pencil">
                                        </span>
                                        </span>
                                </asp:LinkButton>
                                <asp:LinkButton ID="Duplicar" runat="server" CausesValidation="False" CommandName="duplicar" OnClick="Duplicar_Click"
                                    ToolTip="Duplicar" OnClientClick="return confirm('Deseja realmente duplicar o registro?');">
                                            <span class="btn btn-default btn-xs">
									            <span class="glyphicon glyphicon-floppy-saved"></span>
								            </span>
                                </asp:LinkButton>
                                <asp:LinkButton ID="Excluir" runat="server" CommandArgument="" OnClick="Excluir_Click"
                                    OnClientClick="return ConfirmarExclusao();">
                                        <span class="btn btn-default btn-xs">
                                        <span class="glyphicon glyphicon-remove">
                                        </span>
                                        </span>
                                </asp:LinkButton>
                            </div>
                        </li>
                    </ItemTemplate>
                    <FooterTemplate>
                        <li id="emptyCampo" runat="server" visible='<%# DataBinder.Eval(Container.Parent, "Items.Count").ToString() == "0" %>'>Sem Etapas cadastrados </li>
                    </FooterTemplate>
                </asp:Repeater>
            </ul>
            <div class="form-group">
                <asp:Button ID="AddicionarEtapa" runat="server" Text="Adicionar Etapa" CssClass="btn btn-primary pull-right"
                    OnClick="AddicionarEtapa_Click" />
                <div style="clear: both;">
                </div>
            </div>

            
            <div class="form-group">
                <asp:Label ID="lblStatusDemanda" runat="server" Text="Demanda Ativa?" AssociatedControlID="rblStatusDemanda"></asp:Label>
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip2" Chave="demandaStatus" />
                <asp:RadioButtonList ID="rblStatusDemanda" runat="server" RepeatDirection="Horizontal"
                    CssClass="form-control">
                    <asp:ListItem Text="Sim" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Não" Value="0"></asp:ListItem>
                </asp:RadioButtonList>
            </div>

            <hr />
            <div class="form-group">
                <asp:Label ID="Label1" runat="server" Text="Terá Ciclo Mensal?" AssociatedControlID="rblMensal"></asp:Label>
                <uc1:ucHelperTooltip runat="server" ID="UcHelperTooltip3" Chave="Mensal" />
                <asp:RadioButtonList ID="rblMensal" runat="server" RepeatDirection="Horizontal"
                    CssClass="form-control"
                    AutoPostBack="true"
                    OnSelectedIndexChanged="rblMensal_OnChange">
                    <asp:ListItem Text="Sim" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Não" Value="2"></asp:ListItem>
                </asp:RadioButtonList>
            </div>

            <div runat="server" id="datas">
                <div class="form-group">
                    <asp:Label ID="Label4" runat="server" Text="Dia Inicio" AssociatedControlID="txtDiaInicio"></asp:Label>
                    <asp:TextBox ID="txtDiaInicio" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                </div>

                <div class="form-group">
                    <asp:Label ID="Label3" runat="server" Text="Dia Final" AssociatedControlID="txtDiaFinal"></asp:Label>
                    <asp:TextBox ID="txtDiaFinal" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                </div>
            </div>

        </asp:Panel>
        <div class="form-group">
            <asp:Button ID="btnSalvar" runat="server" Text="Salvar" OnClick="btnSalvar_Click"
                CssClass="btn btn-primary" />
            <asp:Button ID="btnCancelar" runat="server" Text="Voltar" CssClass="btn btn-default"
                OnClick="btnCancelar_Click" />
        </div>
    </fieldset>
    <script type="text/javascript">
        (function ($) {
            $(document).ready(function () {
                $('#ContentPlaceHolder1_ContentPlaceHolder1_txtDiaInicio').mask('99');
                $('#ContentPlaceHolder1_ContentPlaceHolder1_txtDiaFinal').mask('99');
            });
        })(jQuery);
    </script>
</asp:Content>

