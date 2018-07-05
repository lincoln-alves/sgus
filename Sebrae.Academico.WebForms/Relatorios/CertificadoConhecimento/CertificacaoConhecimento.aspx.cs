using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.BP.Relatorios;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Dominio;

namespace Sebrae.Academico.WebForms.Relatorios.CertificadoConhecimento
{
    public partial class CertificacaoConhecimento : Page
    {
        public IEnumerable<DTORelatorioUsuarioCertificadoCertame> UsuariosRelatorio
        {
            get { return Session["dsRelatorio"] != null ? Session["dsRelatorio"] as IEnumerable<DTORelatorioUsuarioCertificadoCertame> : null; }
            set { Session["dsRelatorio"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CertificadoCertame manterTrilha = new CertificadoCertame();
                var bmCertificadoCertame = (new BMCertificadoCertame())
                    .ObterTodos()
                    .OrderBy(x => x.Ano)
                    .OrderBy(x => x.NomeCertificado);

                IList<DtoCertificadoCertame> ListaCertificadoCertame = bmCertificadoCertame.Select(x => new DtoCertificadoCertame
                {
                    Id = x.ID,
                    Nome = x.Ano.ToString() + " - " + x.NomeCertificado
                }).ToList();

                WebFormHelper.PreencherLista(ListaCertificadoCertame, this.ddlTemaCertificado, false, true);
            }

            if (LupaUsuario.SelectedUser == null)
            {
                LupaUsuario.LimparCampos();
            }
        }

        protected void btnPesquisar_OnClick(object sender, EventArgs e)
        {
            using (var manter = new RelatorioUsuarioCertificadoConhecimento())
            {
                var manterCertamesUsuario = manter.ObterCertamesUsuario();

                if (LupaUsuario.SelectedUser != null)
                {
                    var usuario = LupaUsuario.SelectedUser;

                    if (usuario != null)
                    {
                        manterCertamesUsuario = manterCertamesUsuario.Where(x => x.Usuario.ID == usuario.ID);
                    }
                }

                if (ddlTemaCertificado != null && ddlTemaCertificado.SelectedItem != null && int.Parse(ddlTemaCertificado.SelectedItem.Value) != 0
                    && !string.IsNullOrWhiteSpace(ddlTemaCertificado.SelectedItem.Value))
                {
                    manterCertamesUsuario = manterCertamesUsuario.Where(x => x.CertificadoCertame.ID == int.Parse(ddlTemaCertificado.SelectedItem.Value));
                }

                var certificados = manterCertamesUsuario.Select(x => new DTORelatorioUsuarioCertificadoCertame()
                {
                    Nome = x.Usuario.Nome,
                    CPF = x.Usuario.CPF,
                    UF = x.Usuario.UF.Sigla,
                    Unidade = x.Usuario.Unidade,
                    Ano = x.CertificadoCertame.Ano,
                    TemaCertificacao = x.CertificadoCertame.NomeCertificado,
                    Status = Enum.GetName(typeof(enumStatusCertificadoConhecimento), x.Status),
                    DataDownload = x.DataDownload != null ? x.DataDownload.Value.ToString("dd/MM/yyyy") : "",
                    Nota = x.Nota
                }).ToList();

                UsuariosRelatorio = certificados;

                componenteGeracaoRelatorio.Visible = certificados.Count > 0;

                WebFormHelper.PreencherGrid(certificados, dgvUsuariosCertame);

                WebFormHelper.PaginarGrid(certificados, dgvUsuariosCertame, 0);
            }
        }

        protected void btnGerarRelatorio_OnClick(object sender, EventArgs e)
        {
            if (UsuariosRelatorio == null)
                return;

            var dt = UsuariosRelatorio;

            WebFormHelper.GerarRelatorio("UsuarioCertificadoCertame.rptCertificadoCertame.rdlc", dt,
                ucFormatoSaidaRelatorio.TipoSaida, null);

        }

        protected void dgvUsuariosCertame_OnPageIndexChanged(object sender, GridViewPageEventArgs e)
        {
            if (UsuariosRelatorio != null)
            {
                WebFormHelper.PaginarGrid(UsuariosRelatorio.ToList(), dgvUsuariosCertame, e.NewPageIndex);
            }
        }
    }
}