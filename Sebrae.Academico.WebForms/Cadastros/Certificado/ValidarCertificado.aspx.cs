using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web.UI;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.Cadastros.Certificado
{
    public partial class ValidarCertificado : Page
    {
        protected void btnVerificar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    var codigo = txtCodigo.Text.Trim();

                    if (!string.IsNullOrWhiteSpace(codigo))
                    {
                        dynamic objetoCertificado = new ExpandoObject();

                        // Caso seja certificado de tutor, fazer as verificações.
                        if (codigo.Length < 32 && codigo.StartsWith("cr"))
                        {
                            var verificarCertificado = new ManterCertificadoTemplate().VerificarCertificadoTutor(codigo);

                            if (verificarCertificado.Valido)
                            {
                                var turmaProfessor =
                                    new ManterTurmaProfessor().ObterTurmaProfessorPorTurma(verificarCertificado.IdTurma)
                                        .FirstOrDefault(x => x.Professor.ID == verificarCertificado.IdProfessor);

                                if (turmaProfessor != null)
                                {
                                    objetoCertificado.Nome = turmaProfessor.Professor.Nome;

                                    if (turmaProfessor.Turma.DataFinal.HasValue)
                                        objetoCertificado.DataGeracao = turmaProfessor.Turma.DataFinal.Value.ToString("dd/MM/yyyy");

                                    objetoCertificado.Curso = turmaProfessor.Turma.Oferta.SolucaoEducacional.Nome;
                                    objetoCertificado.Tipo = "Declaração de tutoria";
                                }
                            }
                        }
                        else
                        {
                            var manterMatriculaOferta = new ManterMatriculaOferta();

                            var buscaMatOferta = manterMatriculaOferta.ObterPorCodigoCertificado(codigo);

                            if (buscaMatOferta != null)
                            {
                                objetoCertificado.Nome = buscaMatOferta.Usuario.Nome;

                                if (buscaMatOferta.DataGeracaoCertificado != null)
                                    objetoCertificado.DataGeracao =
                                        buscaMatOferta.DataGeracaoCertificado.Value.ToString("dd/MM/yyyy");

                                objetoCertificado.Curso = buscaMatOferta.Oferta.SolucaoEducacional.Nome;
                                objetoCertificado.Tipo = "Curso Regular";
                            }
                            else
                            {
                                var manterUsuarioTrilha = new ManterUsuarioTrilha();
                                var buscaUsuarioTrilha = manterUsuarioTrilha.ObterPorCodigoCertificao(codigo);

                                if (buscaUsuarioTrilha != null)
                                {
                                    objetoCertificado.Nome = buscaUsuarioTrilha.Usuario.Nome;
                                    objetoCertificado.Curso = buscaUsuarioTrilha.TrilhaNivel.Trilha.Nome;
                                    objetoCertificado.Tipo = "Trilha";

                                    if (buscaUsuarioTrilha.DataGeracaoCertificado != null)
                                        objetoCertificado.DataGeracao =
                                            buscaUsuarioTrilha.DataGeracaoCertificado.Value.ToString("dd/MM/yyyy");
                                }
                            }
                        }

                        // Fazer output da validação do certificado a partir do objeto anônimo.
                        if (((IDictionary<string, object>)objetoCertificado).ContainsKey("Nome"))
                        {
                            LimparPnlNaoEncontrado();
                            pnlResultadoVerificacao.Visible = true;

                            lblResultado.Text = "O código informado pertence a um certificado válido.";

                            lblNome.Text = objetoCertificado.Nome;
                            lblCurso.Text = objetoCertificado.Curso;
                            lblTipo.Text = objetoCertificado.Tipo;

                            if(objetoCertificado.DataGeracao != null)
                                lblDataGeracao.Text = objetoCertificado.DataGeracao;
                        }
                        else
                        {
                            LimparPnlResultado();
                            pnlNaoEncontrado.Visible = true;
                            lblNaoEncontrado.Text = "O código informado não pertence a nenhum certificado válido.";
                        }
                    }
                    else
                    {
                        LimparPnlNaoEncontrado();
                        LimparPnlResultado();
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Informe o código do certificado.");
                    }
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
            }
        }

        protected void LimparPnlResultado()
        {
            pnlResultadoVerificacao.Visible = false;
            lblResultado.Text = "";
            lblNome.Text = "";
            lblCurso.Text = "";
        }

        protected void LimparPnlNaoEncontrado()
        {
            pnlNaoEncontrado.Visible = false;
            lblNaoEncontrado.Text = "";
        }
    }
}