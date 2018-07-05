<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="SincronizacaoGeral.aspx.cs" Inherits="Sebrae.Academico.WebForms.Cadastros.Sincronizacao.SincronizacaoGeral" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <fieldset>
            <div class="row">
                <div class="col-sm-offset-2 col-sm-8">
                    <div class="form-group">
                        <asp:CheckBox ID="ckbSomenteNaoSincronizados" Text=" Sincronizar somente dados não sincronizados" runat="server" Checked="True" />
                    </div>
                    <div class="form-group">
                        <asp:Button ID="btnSyncSolucoes" runat="server" Text="Sincronizar Soluções" OnClick="btnSyncSolucoes_OnClick" CssClass="btn btn-block btn-default mostrarload" />
                    </div>
                    <div class="form-group">
                        <asp:Button ID="btnSyncOfertas" runat="server" Text="Sincronizar Ofertas" OnClick="btnSyncOfertas_OnClick" CssClass="btn btn-block btn-default mostrarload" />
                    </div>
                    <div class="form-group">
                        <asp:Button ID="btnSyncProgramas" runat="server" Text="Sincronizar Programas e Capacitações" OnClick="btnSyncProgramas_OnClick" CssClass="btn btn-block btn-default mostrarload" />
                    </div>
                    <div class="form-group">
                        <asp:Button ID="btnSyncTrilhas" runat="server" Text="Sincronizar Trilhas" OnClick="btnSyncTrilhas_OnClick" CssClass="btn btn-block btn-default mostrarload" />
                    </div>
                    <hr />
            
                    <div class="form-group">
                        <asp:Button ID="btnSyncAll" runat="server" Text="Sincronizar Tudo" OnClick="btnSyncAll_OnClick" CssClass="btn btn-block btn-primary mostrarload" />
                    </div>
                </div>
            </div>
        </fieldset>
    </div>
</asp:Content>