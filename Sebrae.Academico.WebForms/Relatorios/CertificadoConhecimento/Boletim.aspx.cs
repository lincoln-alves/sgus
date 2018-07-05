using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.BP.Relatorios;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.DTO.Dominio;

namespace Sebrae.Academico.WebForms.Relatorios.CertificadoConhecimento
{
    public partial class Boletim : Page
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
                var bmCertificadoCertame = (new BMCertificadoCertame())
                    .ObterTodos()
                    .OrderBy(x => x.Ano)
                    .ThenBy(x => x.NomeCertificado);

                var listaCertificadoCertame = bmCertificadoCertame.Select(x => new DtoCertificadoCertame
                {
                    Id = x.ID,
                    Nome = x.Ano.ToString() + " - " + x.NomeCertificado
                }).ToList();


                ListBoxesUF.PreencherItens(new ManterUf().ObterTodosIQueryable().OrderBy(x => x.Sigla).ToList(), "ID", "Sigla");

                WebFormHelper.PreencherLista(listaCertificadoCertame, this.ddlTemaCertificado, false, true);
            }
        }

        protected void btnPesquisar_OnClick(object sender, EventArgs e)
        {
            using (var manter = new RelatorioUsuarioCertificadoConhecimento())
            {
                var boletins = manter.ObterCertamesUsuario();

                if (string.IsNullOrWhiteSpace(txtNome.Text) == false)
                {
                    boletins = boletins.Where(x => x.Usuario.Nome.Replace("  ", " ").Contains(txtNome.Text));
                }

                if (string.IsNullOrWhiteSpace(txtCPF.Text) == false)
                {
                    boletins = boletins.Where(x => x.Usuario.CPF.Contains(txtCPF.Text));
                }

                if (string.IsNullOrWhiteSpace(txtUnidade.Text) == false)
                {
                    boletins = boletins.Where(x => x.Usuario.Unidade.Contains(txtUnidade.Text));
                }

                if (string.IsNullOrWhiteSpace(txtInscricao.Text) == false)
                {
                    boletins = boletins.Where(x => x.NumeroInscricao.Contains(txtInscricao.Text));
                }

                var ufsSelecionadas = ListBoxesUF.RecuperarIdsSelecionados<int>().ToList();

                if (ufsSelecionadas.Any())
                {
                    boletins = boletins.Where(x => ufsSelecionadas.Contains(x.Usuario.UF.ID));
                }

                if (string.IsNullOrWhiteSpace(txtAno.Text) == false)
                {
                    int ano;
                    if (int.TryParse(txtAno.Text, out ano))
                    {
                        boletins = boletins.Where(x => x.CertificadoCertame.Ano == ano);
                    }
                    else
                    {
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Ano inválido");
                        return;
                    }
                }

                if (ddlTemaCertificado != null && ddlTemaCertificado.SelectedItem != null && int.Parse(ddlTemaCertificado.SelectedItem.Value) != 0
                    && !string.IsNullOrWhiteSpace(ddlTemaCertificado.SelectedItem.Value))
                {
                    boletins = boletins.Where(x => x.CertificadoCertame.ID == int.Parse(ddlTemaCertificado.SelectedItem.Value));
                }

                if (string.IsNullOrWhiteSpace(ddlStatus.SelectedValue) == false)
                {
                    int status;
                    if (int.TryParse(ddlStatus.SelectedValue, out status))
                    {
                        boletins = boletins.Where(x => x.Status == (enumStatusUsuarioCertificadoCertame)status);
                    }
                    else
                    {
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Status inválido");
                        return;
                    }
                }

                if (string.IsNullOrWhiteSpace(ddlBoletimEmitido.SelectedValue) == false)
                {
                    var emitido = ddlBoletimEmitido.SelectedValue == "Sim";

                    boletins = emitido
                        ? boletins.Where(x => x.DataDownloadBoletim != null)
                        : boletins.Where(x => x.DataDownloadBoletim == null);
                }

                DateTime dataDownload;

                if (!string.IsNullOrWhiteSpace(txtDataDownload.Text))
                {
                    if (DateTime.TryParse(txtDataDownload.Text, out dataDownload))
                    {
                        boletins = boletins.Where(x => x.DataDownloadBoletim != null && x.DataDownloadBoletim.Value.Date == dataDownload);
                    }
                    else
                    {
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Data de Download inválida");
                        return;
                    }
                }

                var certificados = boletins.Select(x => new DTORelatorioUsuarioCertificadoCertame
                {
                    Nome = x.Usuario.Nome,
                    CPF = x.Usuario.CPF,
                    NumeroInscricao = x.NumeroInscricao,
                    UF = x.Usuario.UF.Sigla,
                    Unidade = x.Usuario.Unidade,
                    Ano = x.CertificadoCertame.Ano,
                    TemaCertificacao = x.CertificadoCertame.NomeCertificado,
                    Status = Enum.GetName(typeof (enumStatusCertificadoConhecimento), x.Status),
                    DataDownload =
                        x.DataDownloadBoletim != null ? x.DataDownloadBoletim.Value.ToString("dd/MM/yyyy") : ""
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

            WebFormHelper.GerarRelatorio("UsuarioCertificadoCertame.rptBoletimCertame.rdlc", dt,
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