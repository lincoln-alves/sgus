using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BM.Classes.Moodle;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using classes = Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.BP.Services.Credenciamento;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio;

namespace Sebrae.Academico.WebForms.Cadastros
{
    public partial class EdicaoSolucaoEducacional : Page
    {
        private List<int> ListaIdsSolucoesEducacionaisPreRequisito { get; set; }

        private classes.SolucaoEducacional _solucaoEducacionalEdicao;

        private ManterSolucaoEducacional _manterSolucaoEducacional = new ManterSolucaoEducacional();

        protected override void OnInit(EventArgs e)
        {
            ListaIdsSolucoesEducacionaisPreRequisito = new List<int>();

            ucCategorias1.TreeNodeCheckChanged += SelecionarCategoria;
            ucCategoriasPreRequisito.TreeNodeCheckChanged += SelecionarCategoriaPreRequisito;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;

            var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

            if (usuarioLogado.IsAdministrador())
            {
                divIdNode.Visible = divTxtCodigo.Visible = divSincronizarPortal.Visible = false;
                visibilidadeIntegracaoSAS.Visible = true;
            }

            if (usuarioLogado.IsGestor() && usuarioLogado.UF.ID == (int)enumUF.NA)
                visibilidadeIntegracaoSAS.Visible = true;

            if(usuarioLogado.UF.ID == (int)enumUF.NA)
            {
                PreencherComboProdutoSebrae();
                PreencherComboUnidadeDemandante();
            } 
            else
            {
                produtoSebrae.Visible = false;
                unidadeDemandante.Visible = false;
            }

            PreencherCombos();

            if (Request["Id"] != null)
            {
                var idSolucaoEducacional = int.Parse(Request["Id"]);
                _solucaoEducacionalEdicao = _manterSolucaoEducacional.ObterSolucaoEducacionalPorId(idSolucaoEducacional);

                if (SolucaoEducacionalEdicaoNula(_solucaoEducacionalEdicao)) return;

                var isGestor = usuarioLogado.IsGestor();

                // Se for gestor verifica se ele pode ver essa solução
                if (isGestor && !_solucaoEducacionalEdicao.PermiteVisualizacaoUf(usuarioLogado.UF.ID))
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta,
                        "Você não tem permissão para visualizar esta Solução Educacional. Contate um Administrador do sistema.");
                    Response.Redirect("ListarSolucaoEducacional.aspx");
                }

                PreencherCampos(_solucaoEducacionalEdicao);

                int selecionado;

                if (int.TryParse(ddlFornecedor.SelectedValue, out selecionado))
                {
                    switch ((enumFornecedor)selecionado)
                    {
                        case enumFornecedor.CargaInicial:
                            break;
                        case enumFornecedor.MoodleSebrae:
                            ExibirCategoriaMoodle();
                            break;
                        case enumFornecedor.WebAula:
                            break;
                        case enumFornecedor.Xys:
                            break;
                        case enumFornecedor.FGVSiga:
                            break;
                        case enumFornecedor.FGVOCW:
                            break;
                        case enumFornecedor.UCSebrae:
                            break;
                        case enumFornecedor.Credenciamento:
                            pnlEventosCredenciamento.Visible = true;
                            PreencherEventosCredenciamento();

                            if (_solucaoEducacionalEdicao.IDEvento != null)
                            {
                                WebFormHelper.SetarValorNaCombo(_solucaoEducacionalEdicao.IDEvento.Value.ToString(), ddlEventos);
                            }

                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    ExibirIDChaveExterna();
                }

                //Categoria no Moodle
                if (!string.IsNullOrEmpty(_solucaoEducacionalEdicao.IDChaveExterna))
                {
                    txtCategoriaMoodle.Text = _solucaoEducacionalEdicao.IDChaveExterna;
                }

                var idTermoAceite = _solucaoEducacionalEdicao != null
                    ? _solucaoEducacionalEdicao.TermoAceite != null ? _solucaoEducacionalEdicao.TermoAceite.ID : 0
                    : 0;
                var idCategoriaConteudo = _solucaoEducacionalEdicao != null
                    ? _solucaoEducacionalEdicao.CategoriaConteudo != null
                        ? _solucaoEducacionalEdicao.CategoriaConteudo.ID
                        : 0
                    : 0;

                PreencherTermosAceite(idCategoriaConteudo, idTermoAceite);
            }
            else
            {
                PreencherListaPerfil();
                PreencherListaNivelOcupacional();
                PreencherListaUfs();

                pnlNode.Visible = usuarioLogado.IsGestor();
                divTxtCodigo.Visible = false;
                divIdNode.Visible = false;
                EsconderCategoriaMoodleIdChaveExterna();
                PreencherTermosAceite();
                PreencherCategoriaConteudo(new classes.SolucaoEducacional());
                ucPermissoes1.SelecionarUf(new ManterUsuario().ObterUsuarioLogado().UF.ID);
            }
        }

        private bool SolucaoEducacionalEdicaoNula(classes.SolucaoEducacional _solucaoEducacionalEdicao)
        {
            var solucaoNula = _solucaoEducacionalEdicao == null;

            if (solucaoNula)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Informacao, "Solução Educacional Não Existe");
            }

            return solucaoNula;
        }

        private void PreencherTermosAceite(int idCategoriaConteudo = 0, int idTermoAceite = 0)
        {
            try
            {
                ddlTermoAceite.Items.Clear();

                if (idCategoriaConteudo != 0)
                {
                    var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

                    var termos = new ManterTermoAceite().ObterListaPorCategoriaConteudo(idCategoriaConteudo, idTermoAceite, usuarioLogado);

                    foreach (var item in termos)
                    {
                        ddlTermoAceite.Items.Insert(0, new ListItem(item.Nome, item.ID.ToString()));
                    }
                }

                ddlTermoAceite.Items.Insert(0, new ListItem("-- Sem Termo de Aceite --", string.Empty));

                WebFormHelper.SetarValorNaCombo(idTermoAceite.ToString(), ddlTermoAceite);
            }
            catch (Exception)
            {
                ddlTermoAceite.Items.Clear();

                ddlTermoAceite.Items.Insert(0, new ListItem("-- Sem Termo de Aceite --", string.Empty));
            }
        }

        private void PreencherCombos()
        {
            try
            {
                PreencherComboFornecedor();
                PreencherComboFormaAquisicao();
                PreencherComboCategoriasMoodle();
                PreencherComboSolucaoEducacional();

                ucTags1.PreencherTags();
                WebFormHelper.PreencherComponenteComOpcoesSimNao(rblAtivo, inserirOpcaoSelecione: false, simOuNao: false);
                WebFormHelper.PreencherComponenteComOpcoesSimNao(rblIntergracaoSAS, inserirOpcaoSelecione: false, simOuNao: false);
                WebFormHelper.PreencherComponenteComOpcoesSimNao(rblTeraOfertasContinuas, simOuNao: false);
                WebFormHelper.PreencherComponenteComOpcoesSimNao(rblMarcarComoSolucaoObrigatoria, simOuNao: false);

                
                PreencherComboAreaTematica();

                if (new ManterUsuario().PerfilAdministrador())
                {
                    chkSincronizar.Visible = true;
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }
        private void PreencherComboProdutoSebrae()
        {
            var lista = new BMProdutoSebre().repositorio.ObterTodosIQueryable();
            listBoxesProdutoSebrae.PreencherItens(lista, "ID", "Nome");
        }

        private void PreencherComboAreaTematica()
        {
            var manter = new ManterAreaTematica();
            var lista = manter.ObterTodos();
            listBoxesAreaTematica.PreencherItens(lista, "ID", "Nome");
        }
        private void PreencherComboUnidadeDemandante()
        {
            var cargos = new ManterCargo()
                .ObterTodos()
                .Where(x => x.Uf.ID == (int)enumUF.NA)
                .Where(x => x.TipoCargo != EnumTipoCargo.Funcionario && x.TipoCargo != EnumTipoCargo.Diretoria)
                .Distinct()
                .ToList();

            listBoxesUnidadeDemandante.PreencherItens(cargos, "ID", "Nome");
        }
        public void AdicionarProdutosSebrae (ref classes.SolucaoEducacional solucao)
        {
            var ids = listBoxesProdutoSebrae
                .RecuperarIdsSelecionadosNumerico()
                .ToList();

            var lsRmv = solucao.ListProdutosSebrae
                .Where(p => !ids.Contains(p.ProdutoSebrae.ID))
                .Select(p => p.ProdutoSebrae.ID)
                .ToList();

            var produtos = new BMProdutoSebre().repositorio
                .ObterTodosIQueryable()
                .Where(x => ids.Contains(x.ID))
                .ToList();

            foreach(var produto in produtos)
            {
                if (solucao.ListProdutosSebrae.Any(p => p.ID == produto.ID)) continue;

                solucao.ListProdutosSebrae.Add(new SolucaoEducacionalProdutoSebrae
                {
                    ProdutoSebrae = produto,
                    SolucaoEducacional = solucao
                });
            }

            foreach (var id in lsRmv)
            {
                solucao.ListProdutosSebrae.Remove(solucao.ListProdutosSebrae.First(p => p.ProdutoSebrae.ID == id));
            }

        }

        public void AdicionarUnidadesDemantes(ref classes.SolucaoEducacional solucao)
        {
            var ids = listBoxesUnidadeDemandante
                .RecuperarIdsSelecionadosNumerico()
                .ToList();

            var lsRmv = solucao.ListUnidadesDemandates
               .Where(p => !ids.Contains(p.Cargo.ID))
               .Select(p => p.Cargo.ID)
               .ToList();

            var cargos = new ManterCargo()
                .ObterTodos()
                .Where(x => ids.Contains(x.ID))
                .ToList();

            foreach(var cargo in cargos)
            {
                if (solucao.ListUnidadesDemandates.Any(p => p.Cargo.ID == cargo.ID)) continue;

                solucao.ListUnidadesDemandates.Add(new SolucaoEducacionalUnidadeDemantes
                {
                    Cargo = cargo,
                    SolucaoEducacional = solucao
                });
            }

            foreach (var id in lsRmv)
            {
                solucao.ListUnidadesDemandates.Remove(solucao.ListUnidadesDemandates.First(p => p.Cargo.ID == id));
            }
        }
        private void ObterAreasTematicasSelecionadas(ref classes.SolucaoEducacional obj)
        {
            var manter = new ManterAreaTematica();

            var lsIds = listBoxesAreaTematica.RecuperarIdsSelecionados()
                .Select(id => Convert.ToInt32(id))
                .ToList();

            var lsRmv = obj.ListaAreasTematicas
                .Where(p => !lsIds.Contains(p.AreaTematica.ID))
                .Select(p => p.AreaTematica.ID)
                .ToList();

            foreach (var id in lsIds)
            {
                if (obj.ListaAreasTematicas.Any(p => p.AreaTematica.ID == id)) continue;

                obj.ListaAreasTematicas.Add(new classes.SolucaoEducacionalAreaTematica
                {
                    SolucaoEducacional = obj,
                    AreaTematica = manter.ObterPorId(id)
                });
            }

            foreach (var id in lsRmv)
            {
                obj.ListaAreasTematicas.Remove(obj.ListaAreasTematicas.First(p => p.AreaTematica.ID == id));
            }
        }

        private void PreencherComboFormaAquisicao()
        {
            ManterFormaAquisicao manterFormaAquisicao = new ManterFormaAquisicao();
            IList<classes.FormaAquisicao> ListaFormaAquisicao =
                manterFormaAquisicao.ObterTodasFormaAquisicao()
                    .Where(f => f.TipoFormaDeAquisicao == enumTipoFormaAquisicao.SolucaoEducacional)
                    .ToList();

            WebFormHelper.PreencherLista(ListaFormaAquisicao, this.ddlFormaAquisicao, false, true);
        }

        private void PreencherComboFornecedor()
        {
            var fornecedores = new ManterFornecedor().ObterTodosFornecedores().Where(x => x.FornecedorSistema.HasValue);

            var usuario = new ManterUsuario().ObterUsuarioLogado();

            var fornecedoresBanco = Enum.GetValues(typeof(enumFornecedor)).Cast<int>();


            fornecedores = (from f in fornecedores
                            where
                                f.FornecedorSistema == enumFornecedor.UCSebrae ||
                                f.FornecedorSistema != null
                                && f.ListaFornecedorUF.Any(x => x.UF.ID == usuario.UF.ID)
                                && f.PermiteGestaoSGUS == true
                            select new classes.Fornecedor
                            {
                                ID = f.FornecedorSistema.HasValue ? (int)f.FornecedorSistema.Value : f.ID,
                                Nome = f.Nome
                            }).ToList();


            WebFormHelper.PreencherLista(fornecedores.ToList(), this.ddlFornecedor, false, true);
        }

       
        private void PreencherComboCategoriasMoodle()
        {
            var listaCategoriaMoodle = new BMCategoriaMoodle().ObterTodosIQueryable().OrderBy(x => x.Nome);
            ViewState["_CATEGORIAMOODLE"] = Helpers.Util.ObterListaAutocomplete(listaCategoriaMoodle);
        }

        private void PreencherCampos(classes.SolucaoEducacional solucaoEducacional)
        {
            if (solucaoEducacional != null)
            {
                                txtCargaHoraria.Text =  solucaoEducacional.CargaHoraria > 0 ? solucaoEducacional.CargaHoraria.ToString() : string.Empty;

                txtNome.Text = solucaoEducacional.Nome;                

                //Fornecedor
                WebFormHelper.SetarValorNaCombo(solucaoEducacional.Fornecedor.ID.ToString(), ddlFornecedor);

                //Forma de Aquisição
                WebFormHelper.SetarValorNaCombo(solucaoEducacional.FormaAquisicao.ID.ToString(), ddlFormaAquisicao);

                //Categoria
                if (solucaoEducacional.CategoriaConteudo != null)
                {
                    ucCategorias1.CategoriaSelecionada(solucaoEducacional.CategoriaConteudo.ID);
                    txtCategoriaMoodle.Text = solucaoEducacional.CategoriaConteudo.ID.ToString();
                }

                //Código
                txtCodigo.Text = solucaoEducacional.DescricaoSequencial;

                if (solucaoEducacional.Fornecedor.Login != "moodle")
                {
                    divddlCategoriaMoodle.Visible = false;
                }

                //Id da Chave Externa
                if (!string.IsNullOrWhiteSpace(solucaoEducacional.IDChaveExterna))
                    txtIDChaveExterna.Text = solucaoEducacional.IDChaveExterna;

                // Areas Tematicas.
                listBoxesAreaTematica
                    .MarcarComoSelecionados(solucaoEducacional.ListaAreasTematicas.Select(x => x.AreaTematica.ID));
                
                listBoxesProdutoSebrae
                    .MarcarComoSelecionados(solucaoEducacional.ListProdutosSebrae.Select(x => x.ProdutoSebrae.ID));

                listBoxesUnidadeDemandante
                    .MarcarComoSelecionados(solucaoEducacional.ListUnidadesDemandates.Select(x => x.Cargo.ID));

                //Texto de Apresentação
                if (!string.IsNullOrWhiteSpace(solucaoEducacional.Apresentacao))
                    txtTextoApresentacao.Text = solucaoEducacional.Apresentacao;

                //Ativo
                WebFormHelper.SetarValorNoRadioButtonList(solucaoEducacional.Ativo, rblAtivo);

                //Integração com SAS
                WebFormHelper.SetarValorNoRadioButtonList(solucaoEducacional.IntegracaoComSAS, rblIntergracaoSAS);

                //Terá Ofertas Contínuas
                WebFormHelper.SetarValorNoRadioButtonList(solucaoEducacional.TeraOfertasContinuas,
                    rblTeraOfertasContinuas);

                divDatasInicioFim.Visible = solucaoEducacional.TeraOfertasContinuas;

                if (solucaoEducacional.IdNode.HasValue)
                    txtIdNode.Text = solucaoEducacional.IdNode.ToString();

                // Categoria Moodle caso seja fornecedor Moodle
                if (solucaoEducacional.Fornecedor.ID == (int)enumFornecedor.MoodleSebrae)
                {
                    if (solucaoEducacional.CategoriaConteudo != null)
                    {
                        txtCategoriaMoodle.Text = solucaoEducacional.CategoriaConteudo.ID.ToString();
                    }

                    ExibirCategoriaMoodle();
                }
                else if (solucaoEducacional.Fornecedor.ID == (int)enumFornecedor.UCSebrae)
                {
                    EsconderCategoriaMoodleIdChaveExterna();
                }
                else
                {
                    divChaveExterna.Visible = true;
                    divddlCategoriaMoodle.Visible = false;
                }

                if (solucaoEducacional.TeraOfertasContinuas)
                {
                    // Data de início da SE
                    if (solucaoEducacional.Inicio.HasValue)
                        txtDtInicio.Text = solucaoEducacional.Inicio.Value.ToString("dd/MM/yyyy HH:mm:ss");

                    // Data de fim da SE
                    if (solucaoEducacional.Fim.HasValue)
                        txtDtFim.Text = solucaoEducacional.Fim.Value.ToString("dd/MM/yyyy HH:mm:ss");
                }

                // Preencher todas as listas e combos.
                PreencherListas(solucaoEducacional);
            }
        }

        private void PreencherListas(classes.SolucaoEducacional solucaoEducacional)
        {
            PreencherListaUfs(solucaoEducacional);
            PreencherListaNivelOcupacional(solucaoEducacional);
            PreencherListaPerfil(solucaoEducacional);

            PreencherListaTag(solucaoEducacional);
            PreencherListaSolucaoObrigatoria(solucaoEducacional);
            PreencherComboSolucaoEducacional();
            PreencherCategoriaConteudo(solucaoEducacional);

            if (solucaoEducacional.ListaPreRequisito.Any())
                PreencherCategoriasPreRequisito(solucaoEducacional);

            ucCategoriasPreRequisito.PreencherCategorias(true);
        }

        private void PreencherCategoriaConteudo(classes.SolucaoEducacional solucaoEducacional)
        {
            var listaCategoriaConteudo = new List<int>();

            if (solucaoEducacional.ID != 0 && solucaoEducacional.CategoriaConteudo != null)
            {
                listaCategoriaConteudo = new List<int> { solucaoEducacional.CategoriaConteudo.ID };
            }

            var usuario = new ManterUsuario().ObterUsuarioLogado();

            ucCategorias1.PreencherCategorias(true, listaCategoriaConteudo, usuario);
        }

        private void PreencherComboSolucaoEducacional()
        {
            var categoriasMarcadas = ucCategoriasPreRequisito.IdsCategoriasMarcadas.ToList();

            var ses = categoriasMarcadas.Any()
                ? new ManterSolucaoEducacional().ObterListaSolucaoEducacionalPorCategoria(categoriasMarcadas).AsQueryable()
                : new ManterSolucaoEducacional().ObterTodosSolucaoEducacional();

            ViewState["_SE"] = Helpers.Util.ObterListaAutocomplete(ses);
        }

        private void PreencherListaSolucaoObrigatoria(classes.SolucaoEducacional solucaoEducacional)
        {
            if (solucaoEducacional != null && solucaoEducacional.ListaSolucaoEducacionalObrigatoria.Any())
                divNiveisOcupacionaisObrigatorios.Visible = true;

            var listaNivelOcupacional = new ManterNivelOcupacional().ObterTodosNivelOcupacional();

            WebFormHelper.PreencherLista(listaNivelOcupacional, cblNivelOcupacionalObrigatorio);

            if (solucaoEducacional != null && solucaoEducacional.ListaSolucaoEducacionalObrigatoria.Any())
            {
                rblMarcarComoSolucaoObrigatoria.SelectedValue = "S";

                for (var i = 0; i < cblNivelOcupacionalObrigatorio.Items.Count; i++)
                {
                    if (solucaoEducacional.ListaSolucaoEducacionalObrigatoria.Any(
                            x => x.NivelOcupacional.ID == int.Parse(cblNivelOcupacionalObrigatorio.Items[i].Value)))
                    {
                        cblNivelOcupacionalObrigatorio.Items[i].Selected = true;
                    }
                }
            }
        }

        private void PreencherListaUfs(classes.SolucaoEducacional solucaoEducacional = null)
        {
            var ufsDto = new ManterUf().ObterTodosUf()
                .Select(x => new DTOUf
                {
                    ID = x.ID,
                    Nome = x.Sigla,
                    IsHabilitado = true,
                    IsSelecionado =
                        solucaoEducacional != null &&
                        solucaoEducacional.ListaPermissao.Any(p => p.Uf != null && p.Uf.ID == x.ID)
                }).ToList();

            ucPermissoes1.PreencherUfs(ufsDto, false);
        }

        private void PreencherListaNivelOcupacional(classes.SolucaoEducacional solucaoEducacional = null)
        {
            var niveisDto = new ManterNivelOcupacional().ObterTodosNivelOcupacional()
                .Select(x => new DTONivelOcupacional
                {
                    ID = x.ID,
                    Nome = x.Nome,
                    IsHabilitado = true,
                    IsSelecionado = solucaoEducacional != null &&
                                    solucaoEducacional.ListaPermissao.Any(
                                        p => p.NivelOcupacional != null && p.NivelOcupacional.ID == x.ID)
                }).ToList();

            ucPermissoes1.PreencherNiveisOcupacionais(niveisDto);
        }

        private void PreencherListaPerfil(classes.SolucaoEducacional solucaoEducacional = null)
        {
            var perfisDto = new ManterPerfil().ObterTodosPerfis().Select(x => new DTOPerfil
            {
                ID = x.ID,
                Nome = x.Nome,
                IsHabilitado = true,
                IsSelecionado =
                    solucaoEducacional != null &&
                    solucaoEducacional.ListaPermissao.Any(p => p.Perfil != null && p.Perfil.ID == x.ID)
            }).ToList();

            ucPermissoes1.PreencherPerfis(perfisDto);
        }

        private void PreencherListaTag(classes.SolucaoEducacional solucaoEducacional)
        {
            var listaTags = solucaoEducacional.ListaTags.Where(x => x.Tag != null)
                .Select(x => new classes.Tag { ID = x.Tag.ID, Nome = x.Tag.Nome }).ToList();

            ucTags1.PreencherListViewComTagsGravadosNoBanco(listaTags);
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    _solucaoEducacionalEdicao = ObterObjetoSolucaoEducacional();

                    if (Request["Id"] == null)
                        _manterSolucaoEducacional.IncluirSolucaoEducacional(_solucaoEducacionalEdicao,
                            ConfigurationManager.AppSettings["portal_url_node_id"]);
                    else
                        _manterSolucaoEducacional.AlterarSolucaoEducacional(_solucaoEducacionalEdicao,
                            ConfigurationManager.AppSettings["portal_url_node_id"]);

                    Session.Remove("SolucaoEducacionalEdit");

                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso !",
                        "ListarSolucaoEducacional.aspx");
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
                catch (AlertException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao, ex.Message);
                }
            }
        }

        private classes.SolucaoEducacional ObterObjetoSolucaoEducacional()
        {
            _solucaoEducacionalEdicao = Request["Id"] != null
                ? new ManterSolucaoEducacional().ObterSolucaoEducacionalPorId((int.Parse(Request["Id"])))
                : new classes.SolucaoEducacional();

            //Ativo
            if (rblAtivo.SelectedItem != null && !string.IsNullOrWhiteSpace(rblAtivo.SelectedItem.Value))
            {
                _solucaoEducacionalEdicao.Ativo = rblAtivo.SelectedItem.Value.Trim().ToUpper().Equals("S");
            }
            else
            {
                throw new AcademicoException("O campo \"ativo\" é obrigatório");
            }


            //Integração com SAS
            _solucaoEducacionalEdicao.IntegracaoComSAS = rblIntergracaoSAS.SelectedItem.Value.Trim().ToUpper().Equals("S");


            //Áreas Temáticas
            ObterAreasTematicasSelecionadas(ref _solucaoEducacionalEdicao);

            var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();
            if (usuarioLogado.UF.ID == (int)enumUF.NA)
            {
                //Produtos Sebrae
                AdicionarProdutosSebrae(ref _solucaoEducacionalEdicao);

                AdicionarUnidadesDemantes(ref _solucaoEducacionalEdicao);
            }

            if (_solucaoEducacionalEdicao.ListaAreasTematicas == null)
            {
                throw new AcademicoException("Selecione uma área temática para a solução educacional");
            }

            if (_solucaoEducacionalEdicao.ListaAreasTematicas.Count <= 0)
            {
                throw new AcademicoException("Selecione uma área temática para a solução educacional");
            }

            if (_solucaoEducacionalEdicao.ListaAreasTematicas.Count > 3)
            {
                throw new AcademicoException("É possivel selecionar apenas 3 áreas temáticas para a solução educacional");
            }

            //Nome
            _solucaoEducacionalEdicao.Nome = txtNome.Text.Trim();

            int cargaHoraria;
            if (!string.IsNullOrEmpty(txtCargaHoraria.Text))
            {
                if (int.TryParse(txtCargaHoraria.Text, out cargaHoraria))
                {
                    _solucaoEducacionalEdicao.CargaHoraria = cargaHoraria;
                }
                else
                {
                    throw new AcademicoException("O campo Carga horaria deve ser númerico e em minutos.");
                }
            }


            //Fornecedor
            if (ddlFornecedor.SelectedItem != null && !string.IsNullOrWhiteSpace(ddlFornecedor.SelectedItem.Value))
            {
                _solucaoEducacionalEdicao.Fornecedor = new classes.Fornecedor { ID = int.Parse(ddlFornecedor.SelectedItem.Value) };
            }

            // UF
            _solucaoEducacionalEdicao.UFGestor = _solucaoEducacionalEdicao.UFGestor ??
                                                 new ManterUf().ObterUfPorID(
                                                     new ManterUsuario().ObterUsuarioLogado().UF.ID);

            //Forma de Aquisição
            if (ddlFormaAquisicao.SelectedItem != null &&
                !string.IsNullOrWhiteSpace(ddlFormaAquisicao.SelectedItem.Value))
            {
                _solucaoEducacionalEdicao.FormaAquisicao =
                    (new ManterFormaAquisicao()).ObterFormaAquisicaoPorID(int.Parse(ddlFormaAquisicao.SelectedItem.Value));
            }

            //Termo Aceite
            if (ddlTermoAceite.SelectedItem != null && !string.IsNullOrWhiteSpace(ddlTermoAceite.SelectedItem.Value))
            {
                _solucaoEducacionalEdicao.TermoAceite =
                    new ManterTermoAceite().ObterTermoAceitePorID(int.Parse(ddlTermoAceite.SelectedItem.Value));
            }
            else
            {
                _solucaoEducacionalEdicao.TermoAceite = null;
            }


            var categoria = _solucaoEducacionalEdicao.CategoriaConteudo;

            var idCategoriaConteudo = ucCategorias1.IdsCategoriasMarcadas.FirstOrDefault();

            if (_solucaoEducacionalEdicao.CategoriaConteudo == null ||
                (_solucaoEducacionalEdicao.CategoriaConteudo != null &&
                 idCategoriaConteudo != _solucaoEducacionalEdicao.CategoriaConteudo.ID))
            {
                categoria = new ManterCategoriaConteudo().ObterCategoriaConteudoPorID(idCategoriaConteudo);
            }

            if (Request["Id"] != null)
            {
                if (_manterSolucaoEducacional.AlterouCategoria(_solucaoEducacionalEdicao.ID, categoria))
                    _solucaoEducacionalEdicao.Sequencia =
                        _manterSolucaoEducacional.ObterProximoCodigoSequencial(categoria);
            }
            else
            {
                _solucaoEducacionalEdicao.Sequencia = _manterSolucaoEducacional.ObterProximoCodigoSequencial(categoria);
            }

            if (categoria != null)
            {
                _solucaoEducacionalEdicao.CategoriaConteudo = categoria;
            }
            else
            {
                throw new AcademicoException("Selecione uma categoria para a solução educacional");
            }

            //Id da Chave Externa
            _solucaoEducacionalEdicao.IDChaveExterna = txtIDChaveExterna.Text.Trim();

            if (string.IsNullOrEmpty(_solucaoEducacionalEdicao.IDChaveExterna))
                _solucaoEducacionalEdicao.IDChaveExterna = null;

            //Texto de Apresentação
            _solucaoEducacionalEdicao.Apresentacao = txtTextoApresentacao.Text.Trim();

            //Terá ofertas contínuas
            if (rblTeraOfertasContinuas.SelectedValue != null &&
                !string.IsNullOrWhiteSpace(rblTeraOfertasContinuas.SelectedItem.Value))
            {
                var ofertasContinuas = rblTeraOfertasContinuas.SelectedItem.Value.Trim();

                _solucaoEducacionalEdicao.TeraOfertasContinuas = ofertasContinuas.ToUpper().Equals("S");
            }

            // Datas
            VerificarDatas(_solucaoEducacionalEdicao);

            //Obtém obrigatórios
            for (var i = 0; i < cblNivelOcupacionalObrigatorio.Items.Count; i++)
            {
                var idNivelOcupacional = int.Parse(cblNivelOcupacionalObrigatorio.Items[i].Value);
                var cpfUsuarioLogado = string.Empty;

                if (cblNivelOcupacionalObrigatorio.Items[i].Selected)
                {
                    cpfUsuarioLogado = string.IsNullOrEmpty(cpfUsuarioLogado)
                        ? new ManterUsuario().ObterUsuarioLogado().CPF
                        : cpfUsuarioLogado;

                    if (
                        _solucaoEducacionalEdicao.ListaSolucaoEducacionalObrigatoria.All(
                            x => x.NivelOcupacional.ID != idNivelOcupacional))
                    {
                        _solucaoEducacionalEdicao.ListaSolucaoEducacionalObrigatoria.Add(
                            new classes.SolucaoEducacionalObrigatoria(cpfUsuarioLogado)
                            {
                                SolucaoEducacional = _solucaoEducacionalEdicao,
                                NivelOcupacional =
                                    new ManterNivelOcupacional().ObterNivelOcupacionalPorID(idNivelOcupacional)
                            });
                    }
                }
                else
                {
                    if (
                        _solucaoEducacionalEdicao.ListaSolucaoEducacionalObrigatoria.Any(
                            x => x.NivelOcupacional.ID == idNivelOcupacional))
                    {
                        _solucaoEducacionalEdicao.ListaSolucaoEducacionalObrigatoria.Remove(
                            _solucaoEducacionalEdicao.ListaSolucaoEducacionalObrigatoria.FirstOrDefault(
                                x => x.NivelOcupacional.ID == idNivelOcupacional));
                    }
                }
            }

            if (ddlEventos.SelectedItem != null && !string.IsNullOrWhiteSpace(ddlEventos.SelectedItem.Value))
            {
                _solucaoEducacionalEdicao.IDEvento = int.Parse(ddlEventos.SelectedValue);
            }


            ManterPreRequisito(_solucaoEducacionalEdicao);

            AdicionarPermissao(_solucaoEducacionalEdicao);
            AdicionarOuRemoverTags(_solucaoEducacionalEdicao);

            return _solucaoEducacionalEdicao;
        }

        private void AdicionarOuRemoverTags(classes.SolucaoEducacional solucaoEducacionalEdicao)
        {
            ucTags1.ObterInformacoesSobreAsTags();
            ucTags1.TagsSelecionadas.ForEach(x => solucaoEducacionalEdicao.AdicionarTag(x));
            ucTags1.TagsNaoSelecionadas.ForEach(x => solucaoEducacionalEdicao.RemoverTag(x));
        }

        private void AdicionarPermissao(classes.SolucaoEducacional solucaoEducacionalEdicao)
        {
            AdicionarOuRemoverPerfil(solucaoEducacionalEdicao);
            AdicionarOuRemoverNivelOcupacional(solucaoEducacionalEdicao);
            AdicionarOuRemoverUf(solucaoEducacionalEdicao);
        }

        private void AdicionarOuRemoverPerfil(classes.SolucaoEducacional solucaoEducacionalEdicao)
        {
            var perfisDto = ucPermissoes1.ObterPerfis();

            foreach (var perfilDto in perfisDto)
            {
                var perfil = new classes.Perfil
                {
                    ID = perfilDto.ID
                };

                if (perfilDto.IsSelecionado)
                    solucaoEducacionalEdicao.AdicionarPerfil(perfil);
                else
                    solucaoEducacionalEdicao.RemoverPerfil(perfil);
            }
        }

        private void AdicionarOuRemoverNivelOcupacional(classes.SolucaoEducacional solucaoEducacionalEdicao)
        {
            var niveisDto = ucPermissoes1.ObterNiveis();

            foreach (var nivelDto in niveisDto)
            {
                var nivel = new classes.NivelOcupacional { ID = nivelDto.ID };

                if (nivelDto.IsSelecionado)
                    solucaoEducacionalEdicao.AdicionarNivelOcupacional(nivel);
                else
                    solucaoEducacionalEdicao.RemoverNivelOcupacional(nivel);
            }
        }

        private void AdicionarOuRemoverUf(classes.SolucaoEducacional solucaoEducacionalEdicao)
        {
            var ufsDto = ucPermissoes1.ObterUfs();

            foreach (var ufDto in ufsDto)
            {
                var uf = new classes.Uf { ID = ufDto.ID };

                if (ufDto.IsSelecionado)
                    solucaoEducacionalEdicao.AdicionarUfs(uf, ufDto.Vagas ?? 0);
                else
                    solucaoEducacionalEdicao.RemoverUfs(uf);
            }
        }

        protected void rblMarcarComoSolucaoObrigatoria_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblMarcarComoSolucaoObrigatoria.SelectedValue == "S")
            {
                PreencherListaSolucaoObrigatoria(null);
                divNiveisOcupacionaisObrigatorios.Visible = true;
            }
            else
            {
                divNiveisOcupacionaisObrigatorios.Visible = false;
            }
        }

        public bool PreRequisitoJaSelecionado(int idSolucaoEducacional)
        {
            return this.ListaIdsSolucoesEducacionaisPreRequisito.IndexOf(idSolucaoEducacional) >= 0;
        }

        public List<classes.SolucaoEducacional> ListaSolucaoEducacionalPreRequisito()
        {
            var lista = new List<classes.SolucaoEducacional>();
            for (int i = 0; i < gvSolucaoEducacionalPreRequisito.Rows.Count; i++)
            {
                int idSolucaoEducacional = int.Parse(gvSolucaoEducacionalPreRequisito.DataKeys[i].Value.ToString());
                lista.Add(new ManterSolucaoEducacional().ObterSolucaoEducacionalPorId(idSolucaoEducacional));
                this.ListaIdsSolucoesEducacionaisPreRequisito.Add(idSolucaoEducacional);
            }
            return lista;
        }

        public bool ExisteNaLista(classes.SolucaoEducacional solucaoEducacional,
            List<classes.SolucaoEducacionalPreRequisito> lista)
        {
            return lista.Any(x => x.PreRequisito.ID == solucaoEducacional.ID);
        }

        private void PreencherCategoriasPreRequisito(classes.SolucaoEducacional solucaoEducacional)
        {
            IList<int> listaCategorias = new List<int>();
            foreach (var item in solucaoEducacional.ListaPreRequisito)
            {
                var idCategoriaConteudo = item.PreRequisito.CategoriaConteudo.ID;
                if (listaCategorias.IndexOf(idCategoriaConteudo) < 0) listaCategorias.Add(idCategoriaConteudo);
            }
            pnlSolucaoEducacionalPreRequisito.Visible = true;
            var solucoesListaGeral =
                new ManterSolucaoEducacional().ObterListaSolucaoEducacionalPorCategoria(listaCategorias);
            var solucoes = new List<classes.SolucaoEducacional>();
            foreach (var item in solucoesListaGeral)
            {
                if (ExisteNaLista(item, solucaoEducacional.ListaPreRequisito.ToList())) solucoes.Add(item);
            }
            WebFormHelper.PreencherGrid(solucoes, gvSolucaoEducacionalPreRequisito);

            for (int i = 0; i < gvSolucaoEducacionalPreRequisito.Rows.Count; i++)
            {
                int idSolucaoEducacional = int.Parse(gvSolucaoEducacionalPreRequisito.DataKeys[i].Value.ToString());
                CheckBox cbSolucaoEducacionalPreRequisito =
                    (CheckBox)gvSolucaoEducacionalPreRequisito.Rows[i].FindControl("cbSolucaoEducacionalPreRequisito");

                if (cbSolucaoEducacionalPreRequisito != null)
                {
                    cbSolucaoEducacionalPreRequisito.Checked =
                        solucaoEducacional.ListaPreRequisito.Any(x => x.PreRequisito.ID == idSolucaoEducacional);
                }
            }
        }

        public void ManterPreRequisito(classes.SolucaoEducacional solucaoEducacionalEdicao)
        {
            for (int i = 0; i < gvSolucaoEducacionalPreRequisito.Rows.Count; i++)
            {
                int idSolucaoEducacional = int.Parse(gvSolucaoEducacionalPreRequisito.DataKeys[i].Value.ToString());
                CheckBox cbSolucaoEducacionalPreRequisito =
                    (CheckBox)gvSolucaoEducacionalPreRequisito.Rows[i].FindControl("cbSolucaoEducacionalPreRequisito");

                if (cblNivelOcupacionalObrigatorio != null)
                {
                    if (cbSolucaoEducacionalPreRequisito.Checked)
                    {
                        if (
                            !solucaoEducacionalEdicao.ListaPreRequisito.Any(
                                x => x.PreRequisito.ID == idSolucaoEducacional))
                        {
                            classes.SolucaoEducacionalPreRequisito preRequisito = new classes.SolucaoEducacionalPreRequisito();
                            preRequisito.SolucaoEducacional = solucaoEducacionalEdicao;
                            preRequisito.PreRequisito =
                                new ManterSolucaoEducacional().ObterSolucaoEducacionalPorId(idSolucaoEducacional);
                            solucaoEducacionalEdicao.ListaPreRequisito.Add(preRequisito);
                        }
                    }
                    else
                    {
                        if (
                            solucaoEducacionalEdicao.ListaPreRequisito.Any(
                                x => x.PreRequisito.ID == idSolucaoEducacional))
                        {
                            solucaoEducacionalEdicao.ListaPreRequisito.Remove(
                                solucaoEducacionalEdicao.ListaPreRequisito.FirstOrDefault(
                                    x => x.PreRequisito.ID == idSolucaoEducacional));
                        }
                    }
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Session.Remove("SolucaoEducacionalEdit");
            Response.Redirect("ListarSolucaoEducacional.aspx");
        }

        protected void ddlFornecedor_TextChanged(object sender, EventArgs e)
        {
            try
            {
                switch ((enumFornecedor)int.Parse(ddlFornecedor.SelectedValue))
                {
                    case enumFornecedor.CargaInicial:
                        break;
                    case enumFornecedor.MoodleSebrae:
                        ExibirCategoriaMoodle();
                        break;
                    case enumFornecedor.WebAula:
                        break;
                    case enumFornecedor.Xys:
                        break;
                    case enumFornecedor.FGVSiga:
                        break;
                    case enumFornecedor.FGVOCW:
                        break;
                    case enumFornecedor.UCSebrae:
                        EsconderCategoriaMoodleIdChaveExterna();
                        break;
                    case enumFornecedor.Credenciamento:
                        PreencherEventosCredenciamento();
                        break;
                    default:
                        ExibirIDChaveExterna();
                        break;
                }
            }
            catch (Exception ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private void PreencherEventosCredenciamento()
        {
            ExibirCredenciamento(true);

            var eventos = new ManterCredenciamento().ObterEventos();
            WebFormHelper.PreencherListaCustomizado(eventos.ToList(), ddlEventos, "ID", "Titulo", false, true);
        }

        private void ExibirCredenciamento(bool exibir)
        {
            pnlEventosCredenciamento.Visible = exibir;
        }

        protected void ddlCategoriaMoodle_TextChanged(object sender, EventArgs e)
        {
            txtIDChaveExterna.Text = txtCategoriaMoodle.Text;
        }

        private void ExibirCategoriaMoodle()
        {
            divChaveExterna.Visible = false;
            divddlCategoriaMoodle.Visible = true;
        }

        private void EsconderCategoriaMoodleIdChaveExterna()
        {
            divChaveExterna.Visible = false;
            divddlCategoriaMoodle.Visible = false;
        }

        private void ExibirIDChaveExterna()
        {
            divChaveExterna.Visible = true;
            txtIDChaveExterna.Text = "";
            divddlCategoriaMoodle.Visible = false;
        }

        protected void SelecionarCategoria(object sender, EventArgs e)
        {
            collapsePreRequisito.Attributes.Remove("class");
            collapsePreRequisito.Attributes.Add("class", "panel-collapse collapse");
            collapseDados.Attributes.Remove("class");
            collapseDados.Attributes.Add("class", "panel-collapse in");

            if (ucCategorias1.IdsCategoriasMarcadas.Any())
            {
                var id = ucCategorias1.IdsCategoriasMarcadas.FirstOrDefault();

                var idTermoAceite = _solucaoEducacionalEdicao != null ? _solucaoEducacionalEdicao.TermoAceite.ID : 0;

                PreencherTermosAceite(id, idTermoAceite);
            }
        }

        protected void SelecionarCategoriaPreRequisito(object sender, EventArgs e)
        {
            collapsePreRequisito.Attributes.Remove("class");
            collapsePreRequisito.Attributes.Add("class", "panel-collapse in");
            collapseDados.Attributes.Remove("class");
            collapseDados.Attributes.Add("class", "panel-collapse collapse");

            PreencherComboSolucaoEducacional();
        }

        public void ValidaDatas()
        {
            txtDtInicio.Text = CommonHelper.FormataDataHora(txtDtInicio.Text);
            txtDtFim.Text = CommonHelper.FormataDataHora(txtDtFim.Text);
        }

        private void VerificarDatas(classes.SolucaoEducacional solucaoEducacional)
        {
            if (solucaoEducacional.TeraOfertasContinuas)
            {
                ValidaDatas();

                var inicio = CommonHelper.TratarData(txtDtInicio.Text, "Data de Início");

                var fim = CommonHelper.TratarData(txtDtFim.Text, "Data Fim");

                if (inicio.HasValue && fim.HasValue && inicio.Value > fim.Value)
                {
                    throw new AcademicoException(
                        "Data de início do clico não pode ser maior que a data de fim do ciclo.");
                }

                //Data de Início
                if (!string.IsNullOrWhiteSpace(txtDtInicio.Text) && inicio.HasValue)
                    solucaoEducacional.Inicio = inicio.Value;

                //Data Fim
                if (!string.IsNullOrWhiteSpace(txtDtFim.Text) && fim.HasValue)
                    solucaoEducacional.Fim = fim.Value;
            }
            else
            {
                solucaoEducacional.Inicio =
                    solucaoEducacional.Fim = null;
            }
        }

        protected void txtSolucaoEducacional_OnTextChanged(object sender, EventArgs e)
        {
            collapsePreRequisito.Attributes.Remove("class");
            collapsePreRequisito.Attributes.Add("class", "panel-collapse in");
            var solucoes = ListaSolucaoEducacionalPreRequisito();

            solucoes.Add(
                new ManterSolucaoEducacional().ObterSolucaoEducacionalPorId(int.Parse(txtSolucaoEducacional.Text)));

            pnlSolucaoEducacionalPreRequisito.Visible = true;

            WebFormHelper.PreencherGrid(solucoes, gvSolucaoEducacionalPreRequisito);

            for (var i = 0; i < gvSolucaoEducacionalPreRequisito.Rows.Count; i++)
            {
                var cbSolucaoEducacionalPreRequisito =
                    (CheckBox)gvSolucaoEducacionalPreRequisito.Rows[i].FindControl("cbSolucaoEducacionalPreRequisito");
                if (cbSolucaoEducacionalPreRequisito != null)
                {
                    cbSolucaoEducacionalPreRequisito.Checked = true;
                }
            }

            txtSolucaoEducacional.Text = "";
        }

        protected void rblTeraOfertasContinuas_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            divDatasInicioFim.Visible = rblTeraOfertasContinuas.SelectedValue.ToUpper() == "S";
        }
    }
}