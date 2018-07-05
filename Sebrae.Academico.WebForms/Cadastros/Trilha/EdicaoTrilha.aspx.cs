using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using classes = Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.WebForms.Cadastros
{
    public partial class EdicaoTrilha : PageBase
    {
        private Trilha TrilhaDaSessao
        {
            get
            {
                Trilha trilha;

                if (Session["Trilha_" + Request["Session"]] != null)
                {
                    trilha = (Trilha)Session["Trilha_" + Request["Session"]];
                }
                else
                {
                    trilha = new Trilha();
                    Session["Trilha_" + Request["Session"]] = trilha;
                }

                return trilha;
            }
            set { Session["Trilha_" + Request["Session"]] = value; }
        }

        private readonly ManterTrilha _manterTrilha = new ManterTrilha();

        protected void Page_Load(object sender, EventArgs e)
        {
            //var teste = new Sebrae.Academico.BM.Classes.BMTrilhaNivel().ObterPorID(1);
            if (!Page.IsPostBack)
            {

                TrilhaDaSessao = null;
                PreencherCombos();

                //Update, ou seja, estamos alterado os dados de uma trilha
                if (Request["Id"] != null)
                {
                    var idtrilha = int.Parse(Request["Id"]);
                    TrilhaDaSessao = _manterTrilha.ObterTrilhaPorId(idtrilha);

                    ExibirOcultarNodeDrupal(true);

                    PreencherCampos(TrilhaDaSessao);
                }
            }
        }

        private void PreencherCombos()
        {
            ucPermissoes.PreencherListas();

            PreencherComboAreaTematica();
            PreencherComboCategoriaConteudo();
            if (Request["Id"] == null)
            {
                ucCategorias1.PreencherCategorias(true);
            }
        }

        private void PreencherComboAreaTematica()
        {
            var manter = new ManterAreaTematica();
            var lista = manter.ObterTodos();
            listBoxesAreaTematica.PreencherItens(lista, "ID", "Nome");
        }

        private void ObterAreasTematicasSelecionadas(ref Trilha obj)
        {
            var manter = new ManterAreaTematica();

            var lsIds = listBoxesAreaTematica.RecuperarIdsSelecionados().Select(id => Convert.ToInt32(id)).ToList();
            var lsRmv =
                obj.ListaAreasTematicas.Where(p => !lsIds.Contains(p.AreaTematica.ID))
                    .Select(p => p.AreaTematica.ID)
                    .ToList();
            foreach (var id in lsIds)
            {
                if (obj.ListaAreasTematicas.Any(p => p.AreaTematica.ID == id)) continue;
                obj.ListaAreasTematicas.Add(new TrilhaAreaTematica
                {
                    Trilha = obj,
                    AreaTematica = manter.ObterPorId(id)
                });
            }
            foreach (var id in lsRmv)
            {
                obj.ListaAreasTematicas.Remove(obj.ListaAreasTematicas.First(p => p.AreaTematica.ID == id));
            }
        }

        private void PreencherComboCategoriaConteudo()
        {
            //ManterCategoriaConteudo manterCategoriaSolucaoEducacional = new ManterCategoriaConteudo();
            //IList<CategoriaConteudo> ListaCategoriaSolucaoEducacional = manterCategoriaSolucaoEducacional.ObterTodasCategoriasConteudo();
            //WebFormHelper.PreencherLista(ListaCategoriaSolucaoEducacional, ddlCategoria, false, true);
        }

        private void PreencherCampos(Trilha trilha)
        {
            if (trilha != null)
            {
                txtNome.Text = trilha.Nome;

                //Descrição
                if (!string.IsNullOrWhiteSpace(trilha.Descricao))
                {
                    txtDescricao.Text = trilha.Descricao;
                }

                //IDNome
                txtIdNode.Text = trilha.IdNode.HasValue ? trilha.IdNode.Value.ToString() : string.Empty;

                // Email Tutor
                txtEmailTutor.Text = trilha.EmailTutor;

                // Link de acesso a base de apoio do Moodle
                txtIDCodigoMoodle.Text = trilha.ID_CodigoMoodle.ToString();

                ucCategorias1.PreencherCategorias(true,
                    (trilha.CategoriaConteudo != null ? new List<int> { trilha.CategoriaConteudo.ID } : null));

                // Areas Tematicas.
                listBoxesAreaTematica.MarcarComoSelecionados(trilha.ListaAreasTematicas.Select(x => x.AreaTematica.ID));

                txtCreditoTrilha.Text = trilha.Credito;

                PreencherPermissoes(trilha);
            }
        }

        private void PreencherPermissoes(Trilha trilha)
        {
            ucPermissoes.PreencherListBoxComNiveisOcupacionaisGravadosNoBanco(trilha.ListaPermissao.Where(x => x.NivelOcupacional != null).Select(x => x.NivelOcupacional).ToList(), false);
            ucPermissoes.PreencherListBoxComPerfisGravadosNoBanco(trilha.ListaPermissao.Where(x => x.Perfil != null).Select(x => x.Perfil).ToList(), false);
            ucPermissoes.PreencherListBoxComUfsGravadasNoBanco(trilha.ListaPermissao.Where(x => x.Uf != null).Select(x => x.Uf).ToList(), false);
        }

        /// <summary>
        /// Exibe ou oculta o campo drupa node Id com base no parametro informado
        /// </summary>
        /// <param name="exibir">true para exibir, false para ocultar</param>
        private void ExibirOcultarNodeDrupal(bool exibir)
        {
            pnlNodeDrupal.Visible = exibir;
        }

        private void PreencherComboQuestionario(classes.TrilhaNivel nivel)
        {
            try
            {
                var listaQuestionariosDePre =
                    new ManterQuestionario().ObterQuestionariosDePesquisa().Where(x => x.Ativo == true).ToList();

                // Adicionar os questionários cadastrados nas listas, caso não já estejam, pois acima filtra aonde eles não são ativos.
                // Se o questionário vinculado não for mais ativo, adiciona ele na lista para manter o cadastro.

                // Associar pré não ativo
                QuestionarioAssociacao associacaoPre;
                var questionarioPre =
                    (associacaoPre = nivel.ListaQuestionarioAssociacao.FirstOrDefault(
                        x => x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pre)) != null ? associacaoPre.Questionario : null;

                if (questionarioPre != null && listaQuestionariosDePre.All(x => x.ID != questionarioPre.ID))
                    listaQuestionariosDePre.Add(questionarioPre);

                // Associar pós não ativo

                var listaQuestionariosDePos =
                    new ManterQuestionario().ObterQuestionariosDePesquisa().Where(x => x.Ativo == true).ToList();

                QuestionarioAssociacao associacaoPos;
                var questionarioPos =
                    (associacaoPos = nivel.ListaQuestionarioAssociacao.FirstOrDefault(
                        x => x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pos)) != null ? associacaoPos.Questionario : null;

                if (questionarioPos != null && listaQuestionariosDePos.All(x => x.ID != questionarioPos.ID))
                    listaQuestionariosDePos.Add(questionarioPos);

                // Associar prova não ativa;

                var listaQuestionariosDeAvaliacaoProva =
                    new ManterQuestionario().ObterQuestionariosDeAvaliacaoProva().Where(x => x.Ativo == true).ToList();

                QuestionarioAssociacao associacaoProva;
                var questionarioProva =
                    (associacaoProva = nivel.ListaQuestionarioAssociacao.FirstOrDefault(
                        x => x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Prova)) != null ? associacaoProva.Questionario : null;

                if (questionarioProva != null && listaQuestionariosDeAvaliacaoProva.All(x => x.ID != questionarioProva.ID))
                    listaQuestionariosDeAvaliacaoProva.Add(questionarioProva);
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private void TratarQuestionario(classes.TrilhaNivel trilhaNivel, int idQuestionario, bool evolutivo,
            enumTipoQuestionarioAssociacao tipoQuestionarioAssociacao)
        {
            var questionario = new ManterQuestionario().ObterQuestionarioPorID(idQuestionario);
            var questionarioAssociacaoEditar =
                trilhaNivel.ListaQuestionarioAssociacao.FirstOrDefault(
                    x =>
                        x.TrilhaNivel.ID == trilhaNivel.ID && x.Evolutivo == evolutivo &&
                        x.TipoQuestionarioAssociacao.ID == (int)tipoQuestionarioAssociacao);

            if (questionarioAssociacaoEditar == null)
            {
                var questionarioAssociacaoAdicionar = new QuestionarioAssociacao
                {
                    TipoQuestionarioAssociacao =
                        new ManterTipoQuestionarioAssociacao().ObterTipoQuestionarioAssociacaoPorID(
                            (int)tipoQuestionarioAssociacao),
                    Evolutivo = evolutivo,
                    TrilhaNivel = trilhaNivel,
                    Questionario = questionario,
                    Obrigatorio = true
                };

                trilhaNivel.ListaQuestionarioAssociacao.Add(questionarioAssociacaoAdicionar);
            }
            else
            {
                questionarioAssociacaoEditar.Questionario = questionario;
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            Trilha trilhaEdicao;
            try
            {
                trilhaEdicao = ObterObjetoTrilha();
            }
            catch (Exception ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                return;
            }
            try
            {
                if (Request["Id"] == null)
                {
                    _manterTrilha.IncluirTrilha(trilhaEdicao, ConfigurationManager.AppSettings["portal_url_node_id"]);
                }
                else
                {
                    //foreach (var trilhaNivel in trilhaEdicao.ListaTrilhaNivel)
                    //{
                    //    var lista = trilhaNivel.ListaQuestionarioAssociacao.Where(q => q.Questionario != null).ToList();
                    //    trilhaNivel.ListaQuestionarioAssociacao = lista;
                    //}

                    _manterTrilha.AlterarTrilha(trilhaEdicao, ConfigurationManager.AppSettings["portal_url_node_id"]);
                }

                TrilhaDaSessao = null;
            }
            catch (AlertException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, ex.Message);
                return;
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                return;
            }
            catch (Exception ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                return;
            }
            WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso !", "ListarTrilha.aspx");
        }

        private Trilha ObterObjetoTrilha()
        {
            var trilhaEdicao = TrilhaDaSessao;

            trilhaEdicao.Nome = txtNome.Text.Trim();

            if (!string.IsNullOrEmpty(txtDescricao.Text) && txtDescricao.Text.Length > 250)
            {
                throw new AcademicoException("O campo descrição deve ter 250 caracteres");
            }

            trilhaEdicao.Descricao = txtDescricao.Text;

            var categoria = trilhaEdicao.CategoriaConteudo;

            var idCategoriaConteudo = ucCategorias1.IdsCategoriasMarcadas.FirstOrDefault();

            if (categoria == null || idCategoriaConteudo != categoria.ID)
            {
                categoria = new ManterCategoriaConteudo().ObterCategoriaConteudoPorID(idCategoriaConteudo);
            }

            trilhaEdicao.CategoriaConteudo = categoria;

            // Link para a comunidade do Moodle
            if (!String.IsNullOrEmpty(txtIDCodigoMoodle.Text))
            {
                trilhaEdicao.ID_CodigoMoodle = Convert.ToInt32(txtIDCodigoMoodle.Text);
            }
            else
            {
                trilhaEdicao.ID_CodigoMoodle = null;
            }

            // E-mail do tutor responsável por essa trilha
            trilhaEdicao.EmailTutor = txtEmailTutor.Text;

            //Áreas Temáticas
            ObterAreasTematicasSelecionadas(ref trilhaEdicao);

            if (trilhaEdicao.ListaAreasTematicas == null)
            {
                throw new AcademicoException("Selecione uma área temática para a Trilha");
            }

            if (trilhaEdicao.ListaAreasTematicas.Count <= 0)
            {
                throw new AcademicoException("Selecione uma área temática para a Trilha");
            }
            if (trilhaEdicao.ListaAreasTematicas.Count > 3)
            {
                throw new AcademicoException("É possivel selecionar apenas 3 áreas temáticas para a Trilha");
            }

            if (!String.IsNullOrEmpty(txtCreditoTrilha.Text))
                trilhaEdicao.Credito = txtCreditoTrilha.Text;

            IncluirPermissoesTrilha(ref trilhaEdicao);

            return trilhaEdicao;
        }

        private void IncluirPermissoesTrilha(ref Trilha trilha)
        {
            foreach (var nivelDto in ucPermissoes.ObterNiveis())
            {
                var nivel = new classes.NivelOcupacional { ID = nivelDto.ID };

                if (nivelDto.IsSelecionado)
                {
                    trilha.AdicionarNivelOcupacional(nivel);
                }
                else
                {
                    trilha.RemoverNivelOcupacional(nivel);
                }
            }

            foreach (var perfilDto in ucPermissoes.ObterPerfis())
            {
                var perfil = new classes.Perfil { ID = perfilDto.ID };

                if (perfilDto.IsSelecionado)
                {
                    trilha.AdicionarPerfil(perfil);
                }
                else
                {
                    trilha.RemoverPerfil(perfil);
                }
            }

            foreach (var ufDto in ucPermissoes.ObterUfs())
            {
                var uf = new classes.Uf { ID = ufDto.ID };

                if (ufDto.IsSelecionado)
                {
                    trilha.AdicionarUfs(uf);
                }
                else
                {
                    trilha.RemoverUf(uf);
                }
            }
        }

        private string RecuperarDadosArquivo(FileUpload fuImg)
        {
            if (fuImg != null && fuImg.PostedFile != null && fuImg.PostedFile.ContentLength > 0)
            {
                var imagem = fuImg.PostedFile.InputStream;
                var imagemConvertidaEmBase64String = CommonHelper.ObterBase64String(imagem);
                var informacoesDoArquivoParaBase64 = CommonHelper.GerarInformacoesDoArquivoParaBase64(fuImg);
                return string.Concat(informacoesDoArquivoParaBase64, ",", imagemConvertidaEmBase64String);
            }

            return string.Empty;
        }


        protected void btnCancelarTrilha_Click(object sender, EventArgs e)
        {
            TrilhaDaSessao = null;
            Session.Remove("TrilhaEdit_" + Request["Session"]);
            Response.Redirect("ListarTrilha.aspx");
        }


        protected override enumFuncionalidade Funcionalidade
        {
            get { return enumFuncionalidade.SolucaoEducacional; }
        }

        protected override IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade
        {
            get { throw new NotImplementedException(); }
        }

        //protected void ddlTipoMapa_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    int? nivelId = null;

        //    if (!string.IsNullOrEmpty(hdIndexOfTrilhaNivel.Value))
        //        nivelId = TrilhaDaSessao.ListaTrilhaNivel[Convert.ToInt32(hdIndexOfTrilhaNivel.Value)].ID;

        //    exibePreviewMapa(Convert.ToInt32(ddlTipoMapa.SelectedValue), nivelId);
        //}
    }
}