using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using JWT;

namespace Sebrae.Academico.WebForms.Relatorios.CertificadoTemplate
{
    public partial class VersoCertificadoTrilhas : Page
    {
        public UsuarioTrilha MatriculaSessao
        {
            get
            {
                return (UsuarioTrilha) Session["matricula_trilha_verso"];
            }
            set
            {
                Session["matricula_trilha_verso"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["token"] == null)
                return;

            var jwtToken = Request["token"];

            var payload = JsonWebToken.DecodeToObject(jwtToken, "", false) as IDictionary<string, object>;

            if (payload != null)
            {
                var usuario = new ManterUsuario().ObterUsuarioPorID((int)payload["id"]);

                try
                {
                    if (usuario != null && usuario.TrilhaTokenExpiry > DateTime.Now)
                    {
                        JsonWebToken.Decode(jwtToken, usuario.TrilhaToken);

                        var nivel = new ManterTrilhaNivel().ObterTrilhaNivelPorID((int)payload["nid"]);

                        if (!nivel.UsuarioPossuiMatricula((int)payload["id"]))
                            return;

                        var matricula = new ManterUsuarioTrilha().ObterPorUsuarioNivel(usuario.ID, nivel.ID);

                        if (matricula == null)
                            return;

                        MatriculaSessao = matricula;

                        new ManterUsuario().AdicionarTempoTokenTrilha(usuario);

                        ltrNomeNivel.Text = matricula.TrilhaNivel.Nome;

                        rptLojas.DataSource =
                            matricula.TrilhaNivel.ListaPontoSebrae
                                .Where(
                                    x =>
                                        x.ListaMissoes.SelectMany(m => m.ListaItemTrilha).Any(
                                            it =>
                                                it.Usuario == null &&
                                                it.ObterStatusParticipacoesItemTrilha(matricula) ==
                                                enumStatusParticipacaoItemTrilha.Aprovado));

                        rptLojas.DataBind();
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        protected void rptLojas_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var item = (TrilhaTopicoTematico)e.Item.DataItem;
                var rptObjetivos = (Repeater)e.Item.FindControl("rptObjetivos");

                rptObjetivos.DataSource =
                    item.ObterObjetivos()
                        .Where(
                            x =>
                                x.ListaItemTrilha.Any(
                                    it =>
                                        it.Usuario == null && it.ObterStatusParticipacoesItemTrilha(MatriculaSessao) ==
                                        enumStatusParticipacaoItemTrilha.Aprovado));
                rptObjetivos.DataBind();
            }
        }

        protected void rptObjetivos_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var item = (Objetivo)e.Item.DataItem;

                // Obter Soluções Sebrae que o aluno teve aprovação.
                var solucoesSebrae = item.ListaItemTrilha.Where(
                    x =>
                        x.Usuario == null &&
                        x.ObterStatusParticipacoesItemTrilha(MatriculaSessao) ==
                        enumStatusParticipacaoItemTrilha.Aprovado);

                var rptSolucoesSebrae = (Repeater)e.Item.FindControl("rptSolucoesSebrae");

                rptSolucoesSebrae.DataSource = solucoesSebrae;
                rptSolucoesSebrae.DataBind();
            }
        }
    }
}