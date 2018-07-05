using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BP.Relatorios;
using Sebrae.Academico.BP.DTO.Relatorios;

using Sebrae.Academico.InfraEstrutura.Core.Extensions.Others;
using System.Collections;
using Sebrae.Academico.Dominio.Classes;
using System.Dynamic;
using Sebrae.Academico.BP.DTO.Dominio;
using System.Web.UI.HtmlControls;
using Sebrae.Academico.BP.Helpers;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.WebForms.Relatorios.UsuarioCadastrado
{
    public partial class PorFiltro : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            using (var relUsuarioCadastrado = new RelatorioUsuarioCadastrado())
            {
                ucMultiplosPerfil.PreencherItens(relUsuarioCadastrado.ObterPerfilTodos(), "ID", "Nome");

                ucMultiplosNivelOcupacional.PreencherItens(relUsuarioCadastrado.ObterNivelOcupacionalTodos(), "ID", "Nome");

                ucMultiplosUF.PreencherItens(new ManterUf().ObterTodosIQueryable(), "ID", "Sigla");

                Session["dsRelatorio"] = null;
            }
        }
        
        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            // Perfis selecionados
            var perfisSelecionados = ucMultiplosPerfil.RecuperarIdsSelecionados<int>();

            // Niveis Ocupacionais selecionados
            var niveisOcupacionaisSelecionados = ucMultiplosNivelOcupacional.RecuperarIdsSelecionados<int>();

            // Ufs selecionados
            var ufsSelecionados = ucMultiplosUF.RecuperarIdsSelecionados<int>();

            //Usuario Selecionado
            var usuario = ucLupaUsuario.SelectedUser;

            var queryUsuarios = new ManterUsuario().ObterTodos();
            queryUsuarios = perfisSelecionados.Any() ? queryUsuarios.Where(x => x.ListaPerfil.Any(p => perfisSelecionados.Contains(p.Perfil.ID))) : queryUsuarios;
            queryUsuarios = niveisOcupacionaisSelecionados.Any() ? queryUsuarios.Where(x => niveisOcupacionaisSelecionados.Contains(x.NivelOcupacional.ID)) : queryUsuarios;
            queryUsuarios = ufsSelecionados.Any() ? queryUsuarios.Where(x => ufsSelecionados.Contains(x.UF.ID)) : queryUsuarios;
            queryUsuarios = usuario != null ? queryUsuarios.Where(x => x.ID == usuario.ID) : queryUsuarios;

            if (queryUsuarios.Count() > 0)
            {
                var resultado = queryUsuarios.Select(x => new {
                    ID = x.UF.ID,
                    Sigla = x.UF.Sigla
                })
                .Distinct()
                .ToList()
                .OrderBy(x => x.ID)
                .Select(x => new DTOUsuarioUF{
                    IDUf = x.ID,
                    Sigla = x.Sigla,
                    Usuario = usuario,
                    PerfisID = perfisSelecionados.ToList(),
                    NivelsOcupacionaisID = niveisOcupacionaisSelecionados.ToList()
                }).ToList();

                rptPerfilUf.DataSource = resultado;
                rptPerfilUf.DataBind();

                var relatorio = queryUsuarios.ToList().Select(x => new DTORelatorioDadosPessoais{
                    UF = x.UF.Sigla,
                    Nome = x.Nome,
                    CPF = x.CPF,
                    Email = x.Email,
                    NivelOcupacional = x.NivelOcupacional.Nome,
                    Perfil = String.Join("\n\r",x.ListaPerfil.Select(y => y.Perfil.Nome))
                }).ToList();


                Session.Add("dsRelatorio", relatorio);

                litContador.Text = queryUsuarios.Count().ToString();
                // Converter os resultados em dados totalizadores.
                var totalizadores = new List<DTOTotalizador>();

                totalizadores.Add(TotalizadorUtil.GetTotalizador(queryUsuarios.AsEnumerable(),
                        "Totalizador", "ID", enumTotalizacaoRelatorio.Contar, false));

                UF.Visible = chkListaCamposVisiveis.Items.FindByValue("UF").Selected;
                Nome.Visible = chkListaCamposVisiveis.Items.FindByValue("Nome").Selected;
                CPF.Visible = chkListaCamposVisiveis.Items.FindByValue("CPF").Selected;
                Email.Visible = chkListaCamposVisiveis.Items.FindByValue("Email").Selected;
                NivelOcupacional.Visible = chkListaCamposVisiveis.Items.FindByValue("NivelOcupacional").Selected;
                Perfil.Visible = chkListaCamposVisiveis.Items.FindByValue("Perfil").Selected;
                
                dvContador.Visible = true;
                pnlPerfilUsuario.Visible = true;
                componenteGeracaoRelatorio.Visible = true;
            }
            else
            {
                pnlPerfilUsuario.Visible = false;
                componenteGeracaoRelatorio.Visible = false;
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Nenhuma Informação Encontrada");
            }
        }

        protected void rptPerfilUf_ItemDataBound(object sender, RepeaterItemEventArgs e)
       {
            var ufsUsuario = (DTOUsuarioUF)e.Item.DataItem;

            var rptUsuario = (Repeater)e.Item.FindControl("rptUsuario");

            var queryUsuarios = new ManterUsuario().ObterTodos();

            queryUsuarios = ufsUsuario.PerfisID.Any() ? queryUsuarios.Where(x => x.ListaPerfil.Any(p => ufsUsuario.PerfisID.Contains(p.Perfil.ID))) : queryUsuarios;
            queryUsuarios = ufsUsuario.NivelsOcupacionaisID.Any() ? queryUsuarios.Where(x => ufsUsuario.NivelsOcupacionaisID.Contains(x.NivelOcupacional.ID)) : queryUsuarios;
            queryUsuarios = ufsUsuario.IDUf > 0 ? queryUsuarios.Where(x => ufsUsuario.IDUf == x.UF.ID) : queryUsuarios;
            queryUsuarios = ufsUsuario.Usuario != null ? queryUsuarios.Where(x => x.ID == ufsUsuario.Usuario.ID) : queryUsuarios;

            var resultado = queryUsuarios
                .OrderBy(x => x.Nome)
                .Select(x => new DTOUsuarioPerfil
                {
                    ID = x.ID,
                    Nome = x.Nome,
                    CPF = x.CPF,
                    Email = x.Email,
                    NivelOcupacional = x.NivelOcupacional.Nome
                }).ToList();

            rptUsuario.DataSource = resultado;

            rptUsuario.DataBind();

            ChecarExibirItem("UF", e);

            var colspan = (HtmlTableCell)e.Item.FindControl("colspan");
            if (!chkListaCamposVisiveis.Items.FindByValue("Nome").Selected) colspan.ColSpan = colspan.ColSpan - 1;
            if (!chkListaCamposVisiveis.Items.FindByValue("CPF").Selected) colspan.ColSpan = colspan.ColSpan - 1;
            if (!chkListaCamposVisiveis.Items.FindByValue("Email").Selected) colspan.ColSpan = colspan.ColSpan - 1;
            if (!chkListaCamposVisiveis.Items.FindByValue("NivelOcupacional").Selected) colspan.ColSpan = colspan.ColSpan - 1;
            if (!chkListaCamposVisiveis.Items.FindByValue("Perfil").Selected) colspan.ColSpan = colspan.ColSpan - 1;
        }

        protected void rptUsuario_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var usuario = (DTOUsuarioPerfil)e.Item.DataItem;
            
            var rptPerfil = (Repeater)e.Item.FindControl("rptPerfil");

            var queryUsuarios = new ManterUsuario().ObterTodos().Where(x => x.ID == usuario.ID);
            var perfis = queryUsuarios.SelectMany(x => x.ListaPerfil).Select(y => y.Perfil);

            rptPerfil.DataSource = perfis
                .OrderBy(x => x.Nome)
                .Select(x => new {
                    Nome = x.Nome
                }).ToList();

            rptPerfil.DataBind();
            
            ChecarExibirItem("Nome", e);
            ChecarExibirItem("CPF", e);
            ChecarExibirItem("Email", e);
            ChecarExibirItem("NivelOcupacional", e);
            ChecarExibirItem("Perfil", e);
        }

        private void ChecarExibirItem(string nome, RepeaterItemEventArgs e)
        {
            if (chkListaCamposVisiveis.Items.FindByValue(nome).Selected == false)
                e.Item.FindControl(nome).Visible = false;
            else
                e.Item.FindControl(nome).Visible = true;
        }

        protected void btnGerarRelatorio_Click(object sender, EventArgs e)
        {
            if (Session["dsRelatorio"] == null)
                btnPesquisar_Click(null, null);

            var dt = Session["dsRelatorio"];

            var totalizadores = Session["dsTotalizador"];

            WebFormHelper.GerarRelatorio("DadosPessoais.rptPorFiltro.rdlc", dt, ucFormatoSaidaRelatorio.TipoSaida, 
                chkListaCamposVisiveis.Items);
        }
    }
}