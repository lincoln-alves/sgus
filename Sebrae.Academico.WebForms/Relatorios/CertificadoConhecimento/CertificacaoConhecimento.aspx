<%@ Page Title="" Language="C#" MasterPageFile="~/Pagina.master" AutoEventWireup="true" CodeBehind="CertificacaoConhecimento.aspx.cs" Inherits="Sebrae.Academico.WebForms.Relatorios.CertificadoConhecimento.CertificacaoConhecimento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
       
        $(function () {

            var link = $("a[id*=LupaUsuario_LinkButton]");
            var spanLimpar = $(document.createElement('span'));
            spanLimpar.addClass('input-group-addon');
            spanLimpar.on('click', function () {
                $("input[id*=LupaUsuario_hdIdusuario]").attr('value', null);
                $("input[id*=LupaUsuario_txtNomeUsuarioSelecionado]").attr('value',null);

                return false;
            });

            spanLimpar.css("border", "none")
                .css("position", "absolute")
                .css("right", "100px")
                .css("margin-top", "-28px");

            var btnLimpar = $('<span>Limpar</span>');
            btnLimpar.addClass('label label-theme');
            btnLimpar.css("opacity", "0.5");
            btnLimpar.css("border-radius", "10px");
            btnLimpar.on("mouseout", function () { btnLimpar.css("opacity", "0.5"); });
            btnLimpar.on("mouseover", function () { btnLimpar.css("opacity", "1"); });
            spanLimpar.append(btnLimpar);

            link.append(spanLimpar);
        });


    </script>

    <div class="panel-group" id="gruporelatorio">
        <div class="panel panel-default">
            <div class="panel-heading">
                <a data-toggle="collapse" data-parent="#gruporelatorio" href="#filtros">Filtros
                </a>
            </div>
            <asp:Panel ID="filtros" runat="server" ClientIDMode="Static" CssClass="panel-collapse collapse in">
                <div class="panel-body">
                    <fieldset>
                        <uc:LupaUsuario ID="LupaUsuario" runat="server" />
                    </fieldset>
                    <br />
                    <fieldset>
                        <asp:Label ID="Label2" runat="server" Text="TEMA DO CERTIFICADO" AssociatedControlID="ddlTemaCertificado" />
                        <asp:DropDownList runat="server" ID="ddlTemaCertificado" CssClass="form-control"></asp:DropDownList>    
                    </fieldset>
                    
                </div>
            </asp:Panel>
        </div>
    </div>

    <br />
    <asp:Button ID="btnPesquisar" runat="server" Text="Consultar" OnClick="btnPesquisar_OnClick"
        CssClass="btn btn-primary mostrarload" />
    <hr />

    <asp:GridView runat="server" AutoGenerateColumns="False" ID="dgvUsuariosCertame" CssClass="table table-bordered relatorios col-sm-12" AllowPaging="True" OnPageIndexChanging="dgvUsuariosCertame_OnPageIndexChanged" PageSize="10" >
        <Columns>
            <asp:BoundField DataField="Nome" HeaderText="Nome" SortExpression="Nome" />
            <asp:BoundField DataField="CPF" HeaderText="CPF" SortExpression="CPF" />
            <asp:BoundField DataField="UF" HeaderText="UF" SortExpression="UF" />
            <asp:BoundField DataField="Unidade" HeaderText="Unidade" SortExpression="Unidade" />
            <asp:BoundField DataField="Ano" HeaderText="Ano do Certame" SortExpression="Ano" />
            <asp:BoundField DataField="TemaCertificacao" HeaderText="Tema da Certificação" SortExpression="TemaCertificacao" />
            <asp:BoundField DataField="Nota" HeaderText="Nota do Certame" SortExpression="Nota" />
            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
            <asp:BoundField DataField="CertificadoEmitido" HeaderText="Certificado Emitido" SortExpression="CertificadoEmitido" />
            <asp:BoundField DataField="DataDownload" HeaderText="Data de Download" SortExpression="Data de Download" />
        </Columns>
        <EmptyDataTemplate>
            <itemtemplate>Nenhum registro retornado para a solicitação efetuada</itemtemplate>
        </EmptyDataTemplate>
    </asp:GridView>

    <fieldset runat="server" id="componenteGeracaoRelatorio" clientidmode="Static" visible="False">
        <hr />
        <div class="form-group">
            <uc:SaidaRel ID="ucFormatoSaidaRelatorio" runat="server" />
        </div>
        <div class="form-group">
            <asp:Button ID="btnGerarRelatorio" runat="server" Text="Gerar Relatório" OnClick="btnGerarRelatorio_OnClick"
                CssClass="btn btn-primary" />
        </div>
    </fieldset>
</asp:Content>
