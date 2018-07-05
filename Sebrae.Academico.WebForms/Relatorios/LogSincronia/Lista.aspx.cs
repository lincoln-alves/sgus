using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.Relatorios.LogSincronia
{
    public partial class Lista : PageBase
    {
        protected override IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade
        {
            get { throw new NotImplementedException(); }
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsPostBack) return;
                var lista =
                    new ManterLogSincronia().ObterTodos()
                        .Where(p => p.Sincronizado == false)
                        .OrderByDescending(x => x.DataCriacao)
                        .ToList();

                Session["dsListaLogSincronia"] = lista;

                dgRelatorio.DataSource = lista;
                dgRelatorio.DataBind();

                if (lista.Count <= 0) btnSincronizar.Enabled = false;
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        protected void btnSincronizar_Click(object sender, EventArgs e)
        {
            var lista =
                new ManterLogSincronia().ObterTodos()
                    .Where(p => p.Sincronizado == false)
                    .OrderByDescending(x => x.DataCriacao)
                    .ToList();

            var manter = new ManterLogSincronia();

            foreach (var item in lista)
            {
                manter.Sincronizar(item);
            }
        }

        protected override enumFuncionalidade Funcionalidade
        {
            get { return enumFuncionalidade.Configuracoes; }
        }

        protected void dgRelatorio_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            WebFormHelper.PaginarGrid((IList<Dominio.Classes.LogSincronia>) Session["dsListaLogSincronia"], dgRelatorio,
                e.NewPageIndex);
        }

        protected void dgRelatorio_Sorting(object sender, GridViewSortEventArgs e)
        {
            WebFormHelper.OrdenarListaGrid((IList<Dominio.Classes.LogSincronia>) Session["dsListaLogSincronia"],
                dgRelatorio, e.SortExpression, e.SortDirection, "dsListaLogSincronia");
        }

        protected void dgvRelatorio_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!e.CommandName.Equals("sincronizar")) return;
            try
            {
                var id = int.Parse(e.CommandArgument.ToString());
                Sincronizar(id);
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private static void Sincronizar(int id)
        {
            var manter = new ManterLogSincronia();
            manter.Sincronizar(manter.ObterPorId(id));
        }
    }
}