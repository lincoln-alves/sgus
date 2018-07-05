using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.WebForms.Cadastros
{

    public partial class InclusaoUsuario : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                PreencherListas();
            }
        }

        private void PreencherListas()
        {
            this.ucUsuario1.PreencherListas();
            //    this.ucTags1.PreencherTags();
            //    this.ucPermissoes1.PreencherListas();
        }

        [ScriptMethod]
        [WebMethod]
        public static List<string> BuscarUf(string prefixText, int count)
        {
            if (HttpContext.Current.Application["listUF"] == null)
                HttpContext.Current.Application.Add("listUF", new ManterUf().ObterDoUsuarioLogado());

            IList<Uf> lstUf = (List<Uf>)HttpContext.Current.Application["listUF"];

            IList<string> lstResust =
                (lstUf.Where(
                    x =>
                        x.Sigla.ToLower().StartsWith(prefixText.ToLower().Replace("_", string.Empty)) &&
                        !x.Sigla.ToUpper().Contains("NA")).OrderBy(x => x.Sigla).ToList()).Select(uf => uf.Sigla)
                    .ToList();

            return lstResust.ToList();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListarUsuario.aspx");
        }

        private Usuario ObterObjetoUsuario()
        {
            Usuario usuario = this.ucUsuario1.ObterObjetoUsuario(null);
            // this.AdicionarOuRemoverTags(usuario);

            return usuario;
        }

        protected void btnSalvar_Click1(object sender, EventArgs e)
        {
            string senhagerada;

            try
            {
                var userSalvar = ObterObjetoUsuario();
                senhagerada = txtSenha.Text;

                if (string.IsNullOrEmpty(senhagerada))
                    senhagerada = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.SenhaPadrao).Registro;

                //userSalvar.SenhaLms = WebFormHelper.ObterHashMD5(senhagerada);
                userSalvar.Senha = CriptografiaHelper.Criptografar(senhagerada);

                var manterUsuario = new ManterUsuario();

                manterUsuario.Salvar(userSalvar);

                manterUsuario.EnviarEmailBoasVindas(userSalvar);

                MatricularNovoUsuarioEmCursosObrigatorios(userSalvar);
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                return;
            }

            WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Usuário cadastrado com sucesso!\nA senha gerada para o usuário criado foi: " + senhagerada + ".", "ListarUsuario.aspx");

        }

        private void MatricularNovoUsuarioEmCursosObrigatorios(Usuario usuario)
        {
            try
            {
                var listaSolucoesEducacionais =
                    new ManterSolucaoEducacional().ObterObrigatorios(usuario.NivelOcupacional)
                        .Select(x => x.SolucaoEducacional);

                foreach (var solucaoEducacional in listaSolucoesEducacionais)
                {
                    //VER SE O USUÁRIO JÁ ESTÁ MATRICULADO
                    if (usuario.ListaMatriculaOferta.All(x => x.Oferta.SolucaoEducacional != solucaoEducacional))
                    {
                        var oferta =
                            solucaoEducacional.ListaOferta.FirstOrDefault(
                                x =>
                                    Helpers.Util.ObterVigente(x.DataInicioInscricoes, x.DataFimInscricoes) &&
                                    x.ListaTurma.Any(t => Helpers.Util.ObterVigente(t.DataInicio, t.DataFinal)));

                        var novaMatriculaOferta = new MatriculaOferta
                        {
                            Auditoria = new Auditoria(new ManterUsuario().ObterUsuarioLogado().CPF),
                            Oferta = oferta,
                            Usuario = usuario,
                            StatusMatricula = enumStatusMatricula.Inscrito,
                            UF = usuario.UF,
                            NivelOcupacional = usuario.NivelOcupacional,
                            DataSolicitacao = DateTime.Today
                        };

                        new ManterMatriculaOferta().Salvar(novaMatriculaOferta);

                        if (oferta == null) continue;

                        var novaMatriculaTurma = new MatriculaTurma
                        {
                            Auditoria = new Auditoria(new ManterUsuario().ObterUsuarioLogado().CPF),
                            Turma =
                                oferta.ListaTurma.FirstOrDefault(
                                    t => Helpers.Util.ObterVigente(t.DataInicio, t.DataFinal)),
                            MatriculaOferta = novaMatriculaOferta,
                            DataMatricula = DateTime.Today
                        };

                        novaMatriculaTurma.DataLimite = novaMatriculaTurma.CalcularDataLimite();

                        new ManterMatriculaTurma().Salvar(novaMatriculaTurma);
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}