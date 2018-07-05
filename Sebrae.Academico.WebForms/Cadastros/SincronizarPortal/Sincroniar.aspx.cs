using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.BM.Classes.Moodle;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes.Moodle;
using System.Configuration;

namespace Sebrae.Academico.WebForms.Cadastros.SincronizarPortal
{
    public partial class Sincronizar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }

        protected void btnSincronizarSE_Click(object sender, EventArgs e)
        {
            var manterSe = new ManterSolucaoEducacional();
            var listaSe =
                manterSe.ObterTodosSolucaoEducacional()
                    .Where(
                        r =>
                            r.ListaAreasTematicas.Any() &&
                            r.ListaOferta.Any(
                                p =>
                                    p.DataInicioInscricoes.HasValue &&
                                    p.DataInicioInscricoes.Value.Year == DateTime.Now.Year))
                    .ToList();

            foreach (var item in listaSe)
            {
                try
                {
                    manterSe.AlterarSolucaoEducacional(item, ConfigurationManager.AppSettings["portal_url_node_id"]);
                }
                catch (Exception)
                {
                }
            }
            WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso,
                "Sincronia Realizada: " + listaSe.Count + " registros");
        }

        protected void btnSincronizarOferta_Click(object sender, EventArgs e)
        {
            var manter = new ManterOferta();
            var lista =
                manter.ObterTodasOfertas()
                    .Where(
                        p =>
                            p.SolucaoEducacional.ListaAreasTematicas.Any() &&
                            p.DataInicioInscricoes.HasValue && p.DataInicioInscricoes.Value.Year == DateTime.Now.Year)
                    .ToList();

            foreach (var item in lista)
            {
                try
                {
                    manter.AlterarOferta(item);
                }
                catch (Exception)
                {
                }
            }
            WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Sincronia Realizada: " + lista.Count + " registros");
        }

        protected void btnSincronizarTrilha_Click(object sender, EventArgs e)
        {
            var manter = new ManterTrilha();
            var lista = manter.ObterTodasTrilhas().Where(p => p.ListaAreasTematicas.Any()).ToList();
            foreach (var item in lista)
            {
                try
                {
                    manter.AlterarTrilha(item, ConfigurationManager.AppSettings["portal_url_node_id"]);
                }
                catch 
                {
                }
            }
            WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Sincronia Realizada: " + lista.Count + " registros");
        }

        protected void btnSincronizarPrograma_Click(object sender, EventArgs e)
        {
            var manter = new ManterPrograma();
            var lista = manter.ObterTodosProgramas().Where(p => p.ListaAreasTematicas.Any()).ToList();
            foreach (var item in lista)
            {
                try
                {
                    manter.AlterarPrograma(item);
                }
                catch
                {
                }
            }
            WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Sincronia Realizada: " + lista.Count + " registros");
        }

        protected void btnSincronizarAreaTematica_Click(object sender, EventArgs e)
        {
            var manter = new ManterAreaTematica();
            var lista = manter.ObterTodos();
            foreach (var item in lista)
            {
                try
                {
                    manter.AtualizarAreaTematica(item);
                }
                catch 
                {
                }
            }
            WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Sincronia Realizada: " + lista.Count + " registros");
        }

        protected void btnSincronizarCapacitacoes_Click(object sender, EventArgs e)
        {
            var manter = new ManterCapacitacao();
            var lista =
                manter.ObterTodasCapacitacoes()
                    .Where(
                        p =>
                            p.Programa.ListaAreasTematicas.Any() &&
                            (p.DataInicio.Year == DateTime.Now.Year ||
                             (p.DataInicioInscricao.HasValue && p.DataInicioInscricao.Value.Year == DateTime.Now.Year)))
                    .ToList();
            foreach (var item in lista)
            {
                try
                {
                    manter.AlterarCapacitacao(item);
                }
                catch (Exception)
                {
                }
            }
            WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Sincronia Realizada: " + lista.Count + " registros");
        }
    }
}