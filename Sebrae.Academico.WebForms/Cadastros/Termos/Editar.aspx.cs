using System;
using System.Linq;
using System.Web.UI;
using AutoMapper;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.BP;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.WebForms.Cadastros.Termos
{
    public partial class Editar : Page
    {
        protected override void OnInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                ucCategorias1.PreencherCategorias(false, null);
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["Id"] != null)
                {
                    PreencherCampos(int.Parse(Request["Id"]));
                }
            }
        }

        private void PreencherCampos(int id)
        {
            var termoAceite = new ManterTermoAceite().ObterTermoAceitePorID(id);
            txtNome.Text = termoAceite.Nome;
            txtTermo.Text = termoAceite.Texto;
            txtPoliticaConseguencia.Text = termoAceite.PoliticaConsequencia;

            ucCategorias1.PreencherCategorias(false,
                termoAceite.ListaCategoriaConteudo.Select(x => x.CategoriaConteudo.ID).ToList());
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                var termoAceite = ObterObjetoTermoAceite();

                new ManterTermoAceite().Salvar(termoAceite);

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados gravados com sucesso!", "Listar.aspx");
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private TermoAceite ObterObjetoTermoAceite()
        {
            ValidarTermoAceite();

            Usuario usuarioLogado = null;

            using (var manterUsuario = new ManterUsuario())
            {
                usuarioLogado = manterUsuario.ObterUsuarioLogado();
            }

            TermoAceite termoAceite;

            if (Request["Id"] == null)
            {
                termoAceite = new TermoAceite();
                termoAceite.Nome = txtNome.Text;
            }
            else
            {
                var termoOriginal = new ManterTermoAceite().ObterTermoAceitePorID(int.Parse(Request["Id"]));
                // Retorna um novo termo de aceite se for para duplicar, utilizando o AutoMapper.
                if (Request["Duplicar"] != null)
                {
                    termoAceite = new TermoAceite();
                    termoAceite.Nome = (termoOriginal.Nome == txtNome.Text.Trim()) ? termoOriginal.Nome + " - Cópia" : txtNome.Text;
                } else
                {
                    termoAceite = termoOriginal;
                    termoAceite.Nome = txtNome.Text;
                }
            }

            // Caso esteja cadastrando, atualiza o gestor criador.
            if (termoAceite.ID == 0)
                VerificarGestor(termoAceite, usuarioLogado);

            termoAceite.Texto = txtTermo.Text;
            termoAceite.PoliticaConsequencia = txtPoliticaConseguencia.Text;
            termoAceite.Uf = usuarioLogado.UF;

            AdicionarOuRemoverCategoriaConteudo(termoAceite);

            return termoAceite;
        }

        private static void VerificarGestor(TermoAceite termoAceite, Usuario usuarioLogado)
        {
            termoAceite.Usuario = usuarioLogado.IsGestor()
                ? new ManterUsuario().ObterUsuarioPorID(usuarioLogado.ID)
                : usuarioLogado;
        }

        private void ValidarTermoAceite()
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                throw new AcademicoException("Campo nome é obrigatório");
            }

            if (string.IsNullOrWhiteSpace(txtTermo.Text))
            {
                throw new AcademicoException("Campo termo é obrigatório");
            }

            if (string.IsNullOrWhiteSpace(txtPoliticaConseguencia.Text))
            {
                throw new AcademicoException("Campo política de consequência é obrigatório");
            }
        }

        private void AdicionarOuRemoverCategoriaConteudo(TermoAceite termoAceite)
        {
            var categoriasMarcadas = ucCategorias1.IdsCategoriasMarcadas.ToList();

            var categoriasExistentes = new ManterCategoriaConteudo().ObterTodasCategoriasConteudo();

            foreach (var item in categoriasExistentes)
            {
                if (categoriasMarcadas.Contains(item.ID))
                {
                    if (termoAceite.ListaCategoriaConteudo.All(x => x.CategoriaConteudo.ID != item.ID))
                    {
                        var termoAceiteCategoriaConteudo = new TermoAceiteCategoriaConteudo
                        {
                            Auditoria = new Auditoria(new ManterUsuario().ObterUsuarioLogado().CPF),
                            CategoriaConteudo = categoriasExistentes.FirstOrDefault(x => x.ID == item.ID),
                            TermoAceite = termoAceite
                        };

                        termoAceite.ListaCategoriaConteudo.Add(termoAceiteCategoriaConteudo);
                    }
                }
                else
                {
                    if (termoAceite.ListaCategoriaConteudo.Any(x => x.CategoriaConteudo.ID == item.ID))
                    {
                        termoAceite.ListaCategoriaConteudo.Remove(
                            termoAceite.ListaCategoriaConteudo.FirstOrDefault(x => x.CategoriaConteudo.ID == item.ID));
                    }
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Cadastros/Termos/Listar.aspx");
        }
    }
}