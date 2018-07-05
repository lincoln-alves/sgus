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
using Sebrae.Academico.Dominio.Classes.Moodle;

namespace Sebrae.Academico.WebForms.Cadastros.Sincronizador
{
    public partial class Moodle : System.Web.UI.Page
    {
        private BMSolucaoEducacional bmSolucaoEducacional = new BMSolucaoEducacional();
        private BMOferta bmOferta = new BMOferta();

        private BMSgusMoodleCurso bmSgusMoodleCurso = new BMSgusMoodleCurso();
        private BMSgusMoodleOferta bmSgusMoodleOferta = new BMSgusMoodleOferta();

        private BMCurso bmCurso = new BMCurso();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var solucoesEducacionais = bmSolucaoEducacional.ObterTodos().ToList();
                WebFormHelper.PreencherLista(solucoesEducacionais, ddlSolucaoEducacional, false, true);
            }
        }

        protected void ddlSolucaoEducacional_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSolucaoEducacional.SelectedIndex > 0)
            {
                var ofertas = bmOferta.ObterPorFiltro(string.Empty, string.Empty, int.Parse(ddlSolucaoEducacional.SelectedValue));
                WebFormHelper.PreencherLista(ofertas, ddlOferta, false, true);
            }
        }        

        protected void btnObterChaveExterna_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIDCurso.Text) ||
                ddlSolucaoEducacional.SelectedIndex <= 0 ||
                ddlOferta.SelectedIndex <= 0)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Você deve informar o ID do curso no Moodle a Solução Educacional e a Oferta");
            }
            else
            {
                var solucaoEducacional = bmSolucaoEducacional.ObterPorId(int.Parse(ddlSolucaoEducacional.SelectedValue));

                if (solucaoEducacional.Fornecedor.ID != (int)enumFornecedor.MoodleSebrae)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Este curso não é fornecido pelo Moodle");
                }
                else
                {
                    var oferta = bmOferta.ObterPorId(int.Parse(ddlOferta.SelectedValue));

                    int idCurso = int.Parse(txtIDCurso.Text);

                    var curso = bmCurso.ObterPorID(idCurso);

                    if (curso != null)
                    {
                        var sgusMoodleCurso = bmSgusMoodleCurso.ObterPorCodigoCurso(curso.ID);
                        if (sgusMoodleCurso == null)
                        {
                            sgusMoodleCurso = new SgusMoodleCurso();
                            sgusMoodleCurso.CodigoCategoria = curso.CodigoCategoria;
                            sgusMoodleCurso.CodigoCurso = curso.ID;
                            sgusMoodleCurso.Nome = curso.NomeCompleto;
                            sgusMoodleCurso.DataCriacao = DateTime.Now;
                            sgusMoodleCurso.DataAtualizacao = DateTime.Now;
                            sgusMoodleCurso.Desabilitado = 1;
                            bmSgusMoodleCurso.Cadastrar(sgusMoodleCurso);
                        }

                        var sgusMoodelOferta = bmSgusMoodleOferta.ObterPorCodigoCurso(curso.ID);
                        if (sgusMoodelOferta == null)
                        {
                            sgusMoodelOferta = new SgusMoodleOferta();
                            sgusMoodelOferta.CodigoCategoria = curso.CodigoCategoria;
                            sgusMoodelOferta.CodigoCurso = curso.ID;
                            sgusMoodelOferta.Nome = curso.NomeCompleto;
                            sgusMoodelOferta.DataCriacao = DateTime.Now;
                            sgusMoodelOferta.DataAtualizacao = DateTime.Now;
                            sgusMoodelOferta.Desabilitado = 1;
                            bmSgusMoodleOferta.Cadastrar(sgusMoodelOferta);
                        }

                        if (string.IsNullOrEmpty(solucaoEducacional.IDChaveExterna))
                        {
                            solucaoEducacional.IDChaveExterna = curso.CodigoCategoria.ToString();
                            bmSolucaoEducacional.Salvar(solucaoEducacional);
                        }

                        bool alterouOferta = false;
                        if (string.IsNullOrEmpty(oferta.IDChaveExterna))
                        {
                            oferta.IDChaveExterna = sgusMoodelOferta.ID.ToString();
                            alterouOferta = true;
                        }

                        if (oferta.CodigoMoodle.HasValue == false)
                        {
                            oferta.CodigoMoodle = sgusMoodelOferta.CodigoCurso;
                            alterouOferta = true;
                        }

                        if (alterouOferta)
                        {
                            bmOferta.Salvar(oferta);
                        }

                        dvResumo.Visible = true;
                        string resumo = string.Empty;

                        resumo += "Chave Externa da Solução Educacional: " + sgusMoodleCurso.CodigoCategoria + "<br>";
                        resumo += "Chave Externa da Oferta: " + sgusMoodelOferta.ID + "<br>";
                        resumo += "Código Moodle da Oferta: " + sgusMoodelOferta.CodigoCurso + "<br><hr>";

                        litResumo.Text = resumo;
                    }
                    else
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Este curso não foi encontrado no Moodle");
                }
            }
        }
    }
}