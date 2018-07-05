using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.Services.SgusWebService;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.WebForms.UserControls;
using SgusWebService = Sebrae.Academico.BP.Services.SgusWebService;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Services.Processo;

namespace Sebrae.Academico.WebForms.Cadastros.Hierarquia
{
    public partial class GerenciarHierarquia : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                PreencherHierarquia();
                LimparDadosSessao();
            }
        }

        private void LimparDadosSessao()
        {
            Session.Remove("Diretoria");
            Diretoria = null;
        }

        private void PreencherHierarquia()
        {
            if (Request["Id"] == null)
            {
                Response.Redirect("ListarHierarquia.aspx");
            }
            else
            {
                var ufId = int.Parse(Request["Id"]);

                var uf = new ManterUf().ObterUfPorID(ufId);

                if (uf == null)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "UF não existente.");
                    return;
                }

                UfSelecionada = uf;
                usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

                btnAdicionarUnidade.Visible = false;
                if (ObterPermissaoUsuario())
                {
                    btnAdicionarUnidade.Visible = true;
                }

                var cargosDiretores = new ManterCargo().ObterDiretoria(UfSelecionada.ID);

                // Cargo utilizado apenas no cadastro de SE, removendo Sebrae UF da listagem de cargos da unidade.
                cargosDiretores = cargosDiretores.Where(x => x.Nome != "Sebrae UF");

                rptDiretoria.DataSource = cargosDiretores;
                rptDiretoria.DataBind();
            }
        }

        public Uf UfSelecionada
        {
            get { return (Uf)Session["UfSelecionada"]; }
            set { Session["UfSelecionada"] = value; }
        }

        public Cargo Diretoria
        {
            get { return (Cargo)Session["Diretoria"]; }
            set { Session["Diretoria"] = value; }
        }

        public Usuario usuarioLogado { get; set; }


        public bool ObterPermissaoUsuario()
        {
            return usuarioLogado.IsAdministrador() || (usuarioLogado.IsGestor() && usuarioLogado.UF.ID == UfSelecionada.ID);
        }

        protected void rptDiretoria_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var cargoDiretor = (Cargo)e.Item.DataItem;

                var ltrCargoDiretoria = (Literal)e.Item.FindControl("ltrCargoDiretoria");
                ltrCargoDiretoria.Text = cargoDiretor.Nome.ToUpper();

                if (!cargoDiretor.Ativo)
                {
                    var divHeader = (Panel)e.Item.FindControl("divHeader");
                    divHeader.CssClass = "panel-body disabled";
                }

                // De acordo com o documento pedido, o cargo de diretor pode ter mais de um usuário.
                // Isso deverá mudar com a Hierarquia Auxiliar.
                var diretores = cargoDiretor.ObterDiretores(UfSelecionada);

                // Botão de incluir diretor.
                var btnAdicionarDiretor = (HtmlButton)e.Item.FindControl("btnAdicionarDiretor");
                btnAdicionarDiretor.Attributes["data-cargo"] = cargoDiretor.ID.ToString();

                btnAdicionarDiretor.Visible = false;

                if (diretores.Any())
                {
                    btnAdicionarDiretor.Attributes["data-cargo"] = cargoDiretor.ID.ToString();
                    var rptDiretores = (Repeater)e.Item.FindControl("rptDiretores");

                    rptDiretores.DataSource = diretores;
                    rptDiretores.DataBind();
                }
                else
                {
                    if (ObterPermissaoUsuario())
                    {
                        btnAdicionarDiretor.Visible = true;
                    }

                    ((Literal)e.Item.FindControl("ltrDiretorVazio")).Visible = true;
                }

                // Botão de editar diretoria
                var btnEditarDiretoria = (HtmlAnchor)e.Item.FindControl("btnEditarDiretoria");
                btnEditarDiretoria.Attributes["data-cargo"] = cargoDiretor.ID.ToString();


                // Toda diretoria deverá ter somente um cargo abaixo, o do Chefe de Gabinete.
                var cargoGabinete = cargoDiretor.CargosFilhos.Where(x => x.TipoCargo == EnumTipoCargo.Gabinete && x.Ativo).FirstOrDefault();

                if (cargoGabinete != null)
                {
                    var ltrChefeGabinete = (Literal)e.Item.FindControl("ltrChefeGabinete");
                    ltrChefeGabinete.Text = cargoGabinete.Nome;

                    // De acordo com o documento pedido, o cargo de chefe de gabinete pode ter mais de um usuário.
                    // Isso deverá mudar com a Hierarquia Auxiliar.
                    var chefesDeGabinete = cargoGabinete.ObterChefesGabinete(UfSelecionada).Where(x => x.Cargo.Ativo);

                    if (chefesDeGabinete.Any())
                    {
                        var rptChefesGabinete = (Repeater)e.Item.FindControl("rptChefesGabinete");

                        rptChefesGabinete.DataSource = chefesDeGabinete;
                        rptChefesGabinete.DataBind();
                    }
                    else
                    {
                        ((Literal)e.Item.FindControl("ltrChefeGabineteVazio")).Visible = true;
                    }

                    // Botão de incluir Chefe de Gabinete.
                    var btnAdicionarChefeGabinete = (HtmlButton)e.Item.FindControl("btnAdicionarChefeGabinete");
                    btnAdicionarChefeGabinete.Attributes["data-cargo"] = cargoGabinete.ID.ToString();

                    IEnumerable<UsuarioCargo> funcionarios;

                    var funcionario = cargoGabinete.CargosFilhos.FirstOrDefault(x => x.TipoCargo == EnumTipoCargo.Funcionario && x.Ativo);

                    funcionarios = funcionario != null ? funcionario.ObterFuncionarios(UfSelecionada) : new List<UsuarioCargo>();

                    if (funcionarios.Any())
                    {
                        var rptFuncionarios = (Repeater)e.Item.FindControl("rptFuncionarios");

                        rptFuncionarios.DataSource = funcionarios;
                        rptFuncionarios.DataBind();
                    }
                    else
                    {
                        ((Literal)e.Item.FindControl("ltrFuncionarioVazio")).Visible = true;
                    }

                    var btnAdicionarFuncionario = (HtmlButton)e.Item.FindControl("btnAdicionarFuncionario");
                    btnAdicionarFuncionario.Attributes["data-cargo"] = funcionario != null ? funcionario.ID.ToString() : "";

                    var rptUnidade = (Repeater)e.Item.FindControl("rptUnidade");

                    btnEditarDiretoria.Visible = true;
                    btnAdicionarChefeGabinete.Visible = cargoGabinete.UsuariosCargos.Any(x => x.Cargo.Ativo) == false;
                    btnAdicionarFuncionario.Visible = true;

                    if (!ObterPermissaoUsuario())
                    {
                        btnEditarDiretoria.Visible = false;
                        btnAdicionarChefeGabinete.Visible = false;
                        btnAdicionarFuncionario.Visible = false;
                    }

                    rptUnidade.DataSource = cargoGabinete.CargosFilhos.Where(x => x.TipoCargo != EnumTipoCargo.Funcionario && x.Ativo).OrderBy(x => x.Ordem);
                    rptUnidade.DataBind();
                }

            }
        }

        protected void rptUnidade_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var cargoGerencia = (Cargo)e.Item.DataItem;

                // Botão de incluir gerente.
                var btnAdicionarGerente = (HtmlButton)e.Item.FindControl("btnAdicionarGerente");
                btnAdicionarGerente.Attributes["data-cargo"] = cargoGerencia.ID.ToString();

                btnAdicionarGerente.Visible = false;
                if (ObterPermissaoUsuario())
                    btnAdicionarGerente.Visible = true;


                var gerentes = cargoGerencia.ObterGerentes(UfSelecionada);

                if (gerentes.Any())
                {
                    var rptGerentes = (Repeater)e.Item.FindControl("rptGerentes");

                    rptGerentes.DataSource = gerentes;
                    rptGerentes.DataBind();
                }
                else
                {
                    ((Literal)e.Item.FindControl("ltrGerenteVazio")).Visible = true;
                }

                var cargoGerenciaAdjunta = cargoGerencia.CargosFilhos.FirstOrDefault(x => x.Ativo);

                if (cargoGerenciaAdjunta != null)
                {
                    // Botão de incluir gerente adjunto.
                    var btnAdicionarGerenteAdjunto = (HtmlButton)e.Item.FindControl("btnAdicionarGerenteAdjunto");
                    btnAdicionarGerenteAdjunto.Attributes["data-cargo"] = cargoGerenciaAdjunta.ID.ToString();

                    btnAdicionarGerenteAdjunto.Visible = false;
                    if (ObterPermissaoUsuario())
                        btnAdicionarGerenteAdjunto.Visible = true;
                }

                IEnumerable<UsuarioCargo> gerentesAdjuntos;

                if (cargoGerenciaAdjunta != null && (gerentesAdjuntos = cargoGerenciaAdjunta.ObterGerentesAdjuntos(UfSelecionada).Where(x => x.Cargo.Ativo)) != null &&
                    gerentesAdjuntos.Any())
                {
                    var rptGerentesAdjuntos = (Repeater)e.Item.FindControl("rptGerentesAdjuntos");

                    rptGerentesAdjuntos.DataSource = gerentesAdjuntos;
                    rptGerentesAdjuntos.DataBind();
                }
                else
                {
                    ((Literal)e.Item.FindControl("ltrGerenteAdjuntoVazio")).Visible = true;
                }

                if (cargoGerenciaAdjunta != null)
                {
                    var cargoUnidade = cargoGerenciaAdjunta.CargosFilhos.FirstOrDefault(x => x.Ativo);

                    if (cargoUnidade != null)
                    {
                        // Botão de incluir funcionário.
                        var btnAdicionarFuncionario = (HtmlButton)e.Item.FindControl("btnAdicionarFuncionario");
                        btnAdicionarFuncionario.Attributes["data-cargo"] = cargoUnidade.ID.ToString();

                        btnAdicionarFuncionario.Visible = false;
                        if (ObterPermissaoUsuario())
                            btnAdicionarFuncionario.Visible = true;
                    }

                    IEnumerable<UsuarioCargo> funcionarios;

                    if (cargoUnidade != null && (funcionarios = cargoUnidade.ObterFuncionarios(UfSelecionada).Where(x => x.Cargo.Ativo)) != null &&
                        funcionarios.Any())
                    {
                        var rptFuncionarios = (Repeater)e.Item.FindControl("rptFuncionarios");

                        rptFuncionarios.DataSource = funcionarios;
                        rptFuncionarios.DataBind();
                    }
                    else
                    {
                        ((Literal)e.Item.FindControl("ltrFuncionarioVazio")).Visible = true;
                    }
                }
            }
        }

        protected void rptChefeGabinete_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var usuarioCargo = (UsuarioCargo)e.Item.DataItem;

                var btnRemoverUsuario = (HtmlButton)e.Item.FindControl("btnRemoverUsuario");
                btnRemoverUsuario.Attributes["data-usuariocargo"] = usuarioCargo.ID.ToString();

            }
        }

        protected void rptCargo_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var usuarioCargo = (UsuarioCargo)e.Item.DataItem;

                var btnRemoverUsuario = (HtmlButton)e.Item.FindControl("btnRemoverUsuario");
                btnRemoverUsuario.Visible = false;
                if (ObterPermissaoUsuario())
                    btnRemoverUsuario.Visible = true;

                btnRemoverUsuario.Attributes["data-usuariocargo"] = usuarioCargo.ID.ToString();
            }
        }

        protected void btnAdicionarDiretoria_OnClick(object sender, EventArgs e)
        {
            idCargo.Value = "0";

            LimparModalUnidade();

            ExibirModalUnidade("Adicionar Diretoria");
        }

        protected void btnEditarDiretoria_OnServerClick(object sender, EventArgs e)
        {
            var link = (HtmlAnchor)sender;

            int cargoId;
            if (int.TryParse(link.Attributes["data-cargo"], out cargoId))
            {
                LimparModalUnidade();

                EditarDiretoria(cargoId);
            }
        }

        private void EditarDiretoria(int cargoId)
        {
            if (cargoId > 0)
            {
                var cargoDiretoria = new ManterCargo().ObterPorId(cargoId);
                Diretoria = (Cargo)cargoDiretoria.Clone();

                carregaDiretoriaModal();
            }

            else
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Diretoria inválida");
                return;
            }
        }

        private void carregaDiretoriaModal()
        {
            LimparModalUnidade();

            if (Diretoria != null)
            {
                idCargo.Value = Diretoria.ID.ToString();
                txtTitulo.Text = Diretoria.Nome;
                txtSigla.Text = Diretoria.Sigla;
                rblStatus.SelectedValue = Diretoria.Ativo ? "1" : "0";

                // Toda diretoria deverá ter somente um cargo abaixo, o do Chefe de Gabinete.
                var cargoGabinete = Diretoria.CargosFilhos.Where(x => x.TipoCargo == EnumTipoCargo.Gabinete && x.Ativo).FirstOrDefault();

                if (cargoGabinete != null)
                {
                    txtGabinete.Text = cargoGabinete.Nome;

                    rptUnidadeDiretoria.DataSource = cargoGabinete.CargosFilhos.Where(x => x.TipoCargo != EnumTipoCargo.Funcionario && x.Ativo).OrderBy(x => x.Ordem);
                    rptUnidadeDiretoria.DataBind();
                }

                if (Diretoria.ID > 0)
                {
                    ExibirModalUnidade("Alterar Diretoria");
                }
                else
                {
                    ExibirModalUnidade("Inserir Diretoria");
                }

            }
            else
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Diretoria não encontrada");
                return;
            }
        }

        protected void rptUnidadeDiretoria_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var cargoUnidade = (Cargo)e.Item.DataItem;

                // Botão de editar unidade
                var btnEditarUnidade = (HtmlAnchor)e.Item.FindControl("btnEditarUnidade");
                btnEditarUnidade.Attributes["data-cargo"] = cargoUnidade.ID.ToString();

                // Botão de excluir unidade
                var btnExcluirUnidade = (HtmlAnchor)e.Item.FindControl("btnExcluirUnidade");
                btnExcluirUnidade.Attributes["data-cargo"] = cargoUnidade.ID.ToString();
            }
        }

        private void LimparModalUnidade()
        {
            idCargoPai.Value = "0";
            txtTitulo.Text = "";
            txtSigla.Text = "";
            txtGabinete.Text = "";
            txtGabinete.Enabled = true;
            dvGabinete.Visible = true;
            dvStatus.Visible = true;
            dvListaUnidades.Visible = true;

            btnSalvarDiretoria.Enabled = false;
            btnSalvarUnidade.Enabled = false;

            btnSalvarDiretoria.Visible = true;
            btnSalvarUnidade.Visible = false;

            rptUnidadeDiretoria.DataSource = null;
            rptUnidadeDiretoria.DataBind();
        }

        private void ExibirModalUnidade(string titulo)
        {
            if (Master != null && Master.Master != null)
            {
                ltrTituloModalUnidade.Text = titulo;

                if (!string.IsNullOrWhiteSpace(txtTitulo.Text) && (txtGabinete.Enabled && !string.IsNullOrWhiteSpace(txtGabinete.Text)))
                {
                    btnSalvarDiretoria.Enabled = true;
                    btnSalvarUnidade.Enabled = true;
                }

                var pnlbackdrop = ((Panel)Master.Master.FindControl("pnlbackdrop"));

                if (pnlbackdrop != null)
                {
                    pnlbackdrop.Visible = true;
                    pnlModalDiretoria.Visible = true;
                }
            }
        }

        protected void OcultarModalUnidade_Click(object sender, EventArgs e)
        {
            if (Master != null && Master.Master != null)
            {
                LimparModalUnidade();

                var pnlbackdrop = ((Panel)Master.Master.FindControl("pnlbackdrop"));

                if (pnlbackdrop != null)
                {
                    pnlModalDiretoria.Visible = false;
                    pnlbackdrop.Visible = false;
                }
            }

            PreencherHierarquia();
        }

        protected void btnSalvarDiretoria_OnClick(object sender, EventArgs e)
        {
            try
            {
                var cargoId = int.Parse(idCargo.Value);

                try
                {
                    if (string.IsNullOrWhiteSpace(txtTitulo.Text))
                    {
                        throw new AcademicoException("Nome da Diretoria Obrigatório");
                    }

                    if (txtGabinete.Enabled && string.IsNullOrWhiteSpace(txtGabinete.Text))
                    {
                        throw new AcademicoException("Nome do Gabinete Obrigatório");
                    }

                    Diretoria = preencheDiretoria(cargoId);
                }
                catch (Exception ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }

                //SE A UNIDADE FOR DIRETORIA E ESTIVER INATIVA, VERIFICA SE EXISTE ETAPA PENDENTES PARA ANALISE VINCULADA A ALGUM USUARIO DESSA HIERARQUIA
                List<EtapaResposta> processosQueFicaraoSemAnalista = new List<EtapaResposta>();
                if (Diretoria.TipoCargo == EnumTipoCargo.Diretoria && Diretoria.ID > 0 && Diretoria.Ativo == false)
                {
                    List<UsuarioCargo> cargosHierarquia = new List<UsuarioCargo>();

                    if (Diretoria.UsuariosCargos.Any())
                    {
                        cargosHierarquia.AddRange(Diretoria.UsuariosCargos.Select(usuarioCargo => usuarioCargo).ToList());
                    }

                    cargosHierarquia.AddRange(Diretoria.CargosFilhos.Where(x => x.UsuariosCargos.Any()).SelectMany(x => x.UsuariosCargos).ToList());

                    var unidades = Diretoria.CargosFilhos;
                    if (unidades.Any())
                    {
                        foreach (Cargo diretoria in unidades)
                        {
                            if (diretoria.CargosFilhos.Any())
                            {
                                cargosHierarquia.AddRange(diretoria.CargosFilhos.Where(x => x.UsuariosCargos.Any()).SelectMany(x => x.UsuariosCargos).ToList());

                                foreach (Cargo unidade in diretoria.CargosFilhos)
                                {
                                    cargosHierarquia.AddRange(unidade.CargosFilhos.Where(x => x.UsuariosCargos.Any()).SelectMany(x => x.UsuariosCargos).ToList());
                                }
                            }
                        }
                    }

                    cargosHierarquia = cargosHierarquia.Where(x => x.Cargo.TipoCargo != EnumTipoCargo.Funcionario).ToList();
                    if (cargosHierarquia.Any())
                    {
                        var etapasRespostas = new ManterEtapaResposta().ObterTodosIQueryable()
                            .Where(x => x.Ativo && x.Status == (int)enumStatusEtapaResposta.Aguardando && x.Etapa.Permissoes.Any(y => y.ChefeImediato == true)).ToList();

                        foreach (var usuarioCargo in cargosHierarquia)
                        {
                            var etapas = etapasRespostas.Where(er => er.ChefeImediato(usuarioCargo.Usuario)).ToList();
                            processosQueFicaraoSemAnalista.AddRange(etapas);
                        }
                    }
                }

                var manterCargo = new ManterCargo();
                if (processosQueFicaraoSemAnalista.Any() && Diretoria.Ativo == false)
                {
                    Diretoria.Ativo = true;

                    manterCargo.FazerMerge(Diretoria);

                    OcultarModalUnidade_Click(null, null);

                    var numerosDemandas = processosQueFicaraoSemAnalista.OrderBy(x => x.ProcessoResposta.ID).Select(x => $"#{x.ProcessoResposta.ID}").ToList();
                    ltrSolicitacoesSemAnalistaInativacaoDiretoria.Text = string.Join(", ", numerosDemandas);

                    hdnCargoIdInativar.Value = Diretoria.ID.ToString();

                    ltrDiretoriaInativar.Text = Diretoria.Nome;

                    ExibirBackdrop();

                    pnlAvisoInativarDiretoria.Visible = true;
                }
                else
                {
                    if (Diretoria.ID > 0)
                    {
                        manterCargo.FazerMerge(Diretoria);
                    }
                    else
                    {
                        manterCargo.Salvar(Diretoria);
                    }

                    LimparDadosSessao();

                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Diretoria salva com sucesso");

                    OcultarModalUnidade_Click(null, null);
                }

            }
            catch (Exception Ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro ao salvar Diretoria");
                return;
            }
        }

        protected void btnSalvarUnidade_OnClick(object sender, EventArgs e)
        {
            try
            {
                var cargoId = int.Parse(idCargo.Value);
                var cargoPaiId = int.Parse(idCargoPai.Value);

                Cargo cargoUnidade;
                try
                {
                    cargoUnidade = preencheCargo(cargoId, cargoPaiId);
                }
                catch (Exception ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                    return;
                }

                if (Diretoria != null)
                {
                    var gabinete = Diretoria.CargosFilhos.FirstOrDefault(x => x.TipoCargo == EnumTipoCargo.Gabinete);

                    if (cargoId > 0)
                    {
                        var cargoUnidadeAlteracao = gabinete.CargosFilhos.FirstOrDefault(x => x.ID == cargoUnidade.ID);
                        gabinete.CargosFilhos.Remove(cargoUnidadeAlteracao);
                    }
                    else
                    {
                        cargoUnidade.CargoPai = gabinete;
                    }
                    gabinete.CargosFilhos.Add(cargoUnidade);

                    carregaDiretoriaModal();
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Unidade salva com sucesso");
                }
                else
                {
                    throw new Exception("Erro ao salvar Unidade");
                }
            }
            catch (Exception Ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro ao salvar Unidade");
                return;
            }
        }

        private Cargo preencheDiretoria(int cargoId)
        {
            Cargo cargo;
            if (Diretoria == null && cargoId > 0)
            {
                var cargoBanco = new ManterCargo().ObterTodos().Where(x => x.ID == cargoId).ToList().FirstOrDefault();
                cargo = (Cargo)cargoBanco.Clone();

                Diretoria = cargo;
            }

            if (Diretoria != null)
            {
                cargo = Diretoria;
                if (cargo.CargoPai == null)
                {
                    // Toda diretoria deverá ter somente um cargo abaixo, o do Chefe de Gabinete.
                    var cargoGabinete = cargo.CargosFilhos.FirstOrDefault(x => x.TipoCargo == EnumTipoCargo.Gabinete);

                    if (cargoGabinete != null)
                    {
                        cargoGabinete.Nome = txtGabinete.Text;

                        if(!cargoGabinete.CargosFilhos.Any(x => x.TipoCargo == EnumTipoCargo.Funcionario))
                        {
                            cargoGabinete.CargosFilhos.Add(new Cargo()
                            {
                                CargoPai = cargoGabinete,
                                Nome = "Funcionários",
                                TipoCargo = EnumTipoCargo.Funcionario,
                                Uf = UfSelecionada,
                                Ativo = true,
                                CargosFilhos = new List<Cargo>()
                            });
                        }
                    }
                    else
                    {
                        cargoGabinete = new Cargo()
                        {
                            Nome = txtGabinete.Text,
                            CargoPai = cargo,
                            Uf = UfSelecionada,
                            TipoCargo = EnumTipoCargo.Gabinete,
                            Ativo = true,
                            CargosFilhos = new List<Cargo>()
                        };

                        cargoGabinete.CargosFilhos.Add(new Cargo()
                        {
                            CargoPai = cargoGabinete,
                            Nome = "Funcionários",
                            TipoCargo = EnumTipoCargo.Funcionario,
                            Uf = UfSelecionada,
                            Ativo = true,
                            CargosFilhos = new List<Cargo>()
                        });

                        cargo.CargosFilhos.Add(cargoGabinete);
                    }
                }

                cargo.Nome = txtTitulo.Text;
                cargo.Sigla = txtSigla.Text;
                cargo.Ativo = rblStatus.SelectedValue == "1";
            }
            else
            {
                cargo = new Cargo()
                {
                    Nome = txtTitulo.Text,
                    Sigla = txtSigla.Text,
                    Uf = UfSelecionada,
                    TipoCargo = EnumTipoCargo.Diretoria,
                    Ativo = rblStatus.SelectedValue == "1",
                    CargosFilhos = new List<Cargo>()
                };

                var cargoGabinete = new Cargo()
                {
                    Nome = txtGabinete.Text,
                    CargoPai = cargo,
                    Uf = UfSelecionada,
                    TipoCargo = EnumTipoCargo.Gabinete,
                    Ativo = true,
                    CargosFilhos = new List<Cargo>()
                };

                cargoGabinete.CargosFilhos.Add(new Cargo()
                {
                    CargoPai = cargoGabinete,
                    Nome = "Funcionários",
                    TipoCargo = EnumTipoCargo.Funcionario,
                    Uf = UfSelecionada,
                    Ativo = true,
                    CargosFilhos = new List<Cargo>()
                });

                cargo.CargosFilhos.Add(cargoGabinete);
            }

            return cargo;
        }

        private Cargo preencheCargo(int cargoId, int cargoPaiId)
        {
            if (string.IsNullOrWhiteSpace(txtTitulo.Text))
            {
                throw new AcademicoException("Nome da Diretoria Obrigatório");
            }

            Cargo cargo;
            if (cargoId > 0)
            {
                var cargoBanco = new ManterCargo().ObterPorId(cargoId);
                cargo = (Cargo)cargoBanco.Clone();

                cargo.Nome = txtTitulo.Text;
                cargo.Sigla = txtSigla.Text;
                cargo.Ativo = true;

                if (cargo.TipoCargo == EnumTipoCargo.Gerencia && !cargo.CargosFilhos.Any(x => x.TipoCargo == EnumTipoCargo.GerenciaAdjunta))
                {
                    var gerenciaAdjunta = new Cargo()
                    {
                        CargoPai = cargo,
                        Nome = "Gerencia Adjunta",
                        TipoCargo = EnumTipoCargo.GerenciaAdjunta,
                        Uf = UfSelecionada,
                        Ativo = true,
                        CargosFilhos = new List<Cargo>()
                    };

                    gerenciaAdjunta.CargosFilhos.Add(new Cargo()
                    {
                        CargoPai = gerenciaAdjunta,
                        Nome = "Funcionários",
                        TipoCargo = EnumTipoCargo.Funcionario,
                        Uf = UfSelecionada,
                        Ativo = true
                    });

                    cargo.CargosFilhos.Add(gerenciaAdjunta);
                }
            }
            else
            {
                cargo = new Cargo()
                {
                    Nome = txtTitulo.Text,
                    Sigla = txtSigla.Text,
                    Uf = UfSelecionada,
                    TipoCargo = EnumTipoCargo.Gerencia,
                    Ativo = true,
                    CargosFilhos = new List<Cargo>()
                };

                if (Diretoria != null)
                {
                    cargo.CargoPai = Diretoria.CargosFilhos.FirstOrDefault(x => x.TipoCargo == EnumTipoCargo.Gabinete);
                }

                if (!cargo.CargosFilhos.Any(x => x.TipoCargo == EnumTipoCargo.GerenciaAdjunta))
                {
                    var gerenciaAdjunta = new Cargo()
                    {
                        CargoPai = cargo,
                        Nome = "Gerencia Adjunta",
                        TipoCargo = EnumTipoCargo.GerenciaAdjunta,
                        Uf = UfSelecionada,
                        Ativo = true,
                        CargosFilhos = new List<Cargo>()
                    };

                    gerenciaAdjunta.CargosFilhos.Add(new Cargo()
                    {
                        CargoPai = gerenciaAdjunta,
                        Nome = "Funcionários",
                        TipoCargo = EnumTipoCargo.Funcionario,
                        Uf = UfSelecionada,
                        Ativo = true
                    });

                    cargo.CargosFilhos.Add(gerenciaAdjunta);
                }
            }

            return cargo;
        }

        protected void btnOrdenarDiretoria_OnClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hfOrdemDiretoria.Value))
            {
                var ordenacao = hfOrdemDiretoria.Value.Split(',');

                ManterCargo manterCargo = new ManterCargo();
                var cargosDiretores = new ManterCargo().ObterDiretoria(UfSelecionada.ID);
                for (var posicao = 0; posicao < ordenacao.Count(); posicao++)
                {
                    int idDiretoria;
                    if (int.TryParse(ordenacao[posicao], out idDiretoria))
                    {
                        var diretoria = cargosDiretores.Where(x => x.ID == idDiretoria).FirstOrDefault();
                        if (diretoria != null)
                        {
                            diretoria.Ordem = posicao;
                            manterCargo.Salvar(diretoria);
                        }
                    }
                }

                PreencherHierarquia();
            }
        }

        protected void btnAdicionarUnidade_OnServerClick(object sender, EventArgs e)
        {
            try
            {
                Diretoria = preencheDiretoria(int.Parse(idCargo.Value));
            }
            catch (Exception ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                return;
            }

            LimparModalUnidade();

            dvListaUnidades.Visible = false;
            txtGabinete.Enabled = false;
            dvGabinete.Visible = false;
            dvStatus.Visible = false;

            idCargoPai.Value = idCargo.Value;
            idCargo.Value = "0";

            btnSalvarDiretoria.Visible = false;
            btnSalvarUnidade.Visible = true;

            ExibirModalUnidade("Adicionar Unidade");
        }

        protected void btnEditarUnidade_OnServerClick(object sender, EventArgs e)
        {
            try
            {
                Diretoria = preencheDiretoria(int.Parse(idCargo.Value));
            }
            catch (Exception ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                return;
            }

            LimparModalUnidade();

            dvListaUnidades.Visible = false;
            txtGabinete.Enabled = false;
            dvGabinete.Visible = false;
            dvStatus.Visible = false;

            btnSalvarDiretoria.Visible = false;
            btnSalvarUnidade.Visible = true;

            btnSalvarUnidade.Enabled = true;

            int diretoriaId;
            if (int.TryParse(idCargoPai.Value, out diretoriaId))
            {
                idCargoPai.Value = idCargo.Value;
            }

            var link = (HtmlAnchor)sender;
            var cargoId = int.Parse(link.Attributes["data-cargo"]);

            if (cargoId > 0)
            {
                var cargo = new ManterCargo().ObterPorId(cargoId);

                if (cargo != null)
                {
                    idCargo.Value = cargo.ID.ToString();
                    txtTitulo.Text = cargo.Nome;
                    txtSigla.Text = cargo.Sigla;
                    rblStatus.SelectedValue = cargo.Ativo ? "1" : "0";

                    ExibirModalUnidade("Editar Unidade");
                }
                else
                {
                    if (diretoriaId > 0)
                    {
                        EditarDiretoria(diretoriaId);
                    }
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Unidade inválida");
                    return;
                }
            }
            else
            {
                if (diretoriaId > 0)
                {
                    EditarDiretoria(diretoriaId);
                }
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro ao editar Unidade");
                return;
            }
        }

        protected void btnExcluirUnidade_OnServerClick(object sender, EventArgs e)
        {
            var link = (HtmlAnchor)sender;
            var cargoId = int.Parse(link.Attributes["data-cargo"]);

            if (cargoId > 0)
            {
                try
                {
                    var manterCargo = new ManterCargo();
                    var gabinete = Diretoria.CargosFilhos.FirstOrDefault(x => x.TipoCargo == EnumTipoCargo.Gabinete);
                    var cargo = gabinete.CargosFilhos.FirstOrDefault(x => x.ID == cargoId);

                    if (cargo != null)
                    {
                        cargo.Ativo = false;
                    }
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Unidade excluida com sucesso");

                    rptUnidadeDiretoria.DataSource = gabinete.CargosFilhos.Where(x => x.Ativo);
                    rptUnidadeDiretoria.DataBind();
                }
                catch
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro ao excluir unidade");
                }
            }
        }

        protected void btnOrdenarUnidades_OnClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hfOrdemUnidades.Value))
            {
                ManterCargo manterCargo = new ManterCargo();

                var idDiretoria = int.Parse(idCargo.Value);
                var diretoria = new ManterCargo().ObterPorId(idDiretoria);
                var gabinete = diretoria.CargosFilhos.FirstOrDefault(x => x.TipoCargo == EnumTipoCargo.Gabinete);

                var ordenacao = hfOrdemUnidades.Value.Split(',');

                var cargosUnidades = gabinete.CargosFilhos;
                for (var posicao = 0; posicao < ordenacao.Count(); posicao++)
                {
                    int idUnidade;
                    if (int.TryParse(ordenacao[posicao], out idUnidade))
                    {
                        var cargo = cargosUnidades.FirstOrDefault(x => x.ID == idUnidade);
                        if (cargo != null)
                        {
                            cargo.Ordem = posicao;
                            manterCargo.Salvar(cargo);
                        }
                    }
                }

                if (gabinete != null)
                {
                    rptUnidadeDiretoria.DataSource = gabinete.CargosFilhos.Where(x => x.TipoCargo != EnumTipoCargo.Funcionario && x.Ativo).OrderBy(x => x.Ordem);
                    rptUnidadeDiretoria.DataBind();
                }
            }
        }

        protected void btnAlterarDiretor_OnServerClick(object sender, EventArgs e)
        {
            ltrTituloModal.Text = "Alterar Diretor";

            AbrirCadastro(sender);
        }

        protected void btnNovoDiretor_OnServerClick(object sender, EventArgs e)
        {
            ltrTituloModal.Text = "Cadastrar Diretor";

            AbrirCadastro(sender);
        }

        protected void btnAlterarChefeGabinete_OnServerClick(object sender, EventArgs e)
        {
            ltrTituloModal.Text = "Alterar Chefe de Gabinete";

            AbrirCadastro(sender);
        }

        protected void btnNovoChefeGabinete_OnServerClick(object sender, EventArgs e)
        {
            ltrTituloModal.Text = "Cadastrar Chefe de Gabinete";

            AbrirCadastro(sender);
        }

        protected void btnAdicionarDiretor_OnServerClick(object sender, EventArgs e)
        {
            ltrTituloModal.Text = "Cadastrar Diretor";

            AbrirCadastro(sender);
        }

        protected void btnAdicionarChefeGabinete_OnServerClick(object sender, EventArgs e)
        {
            ltrTituloModal.Text = "Cadastrar Chefe de Gabinete";

            AbrirCadastro(sender);
        }

        protected void btnAdicionarGerente_OnServerClick(object sender, EventArgs e)
        {
            ltrTituloModal.Text = "Cadastrar Gerente";

            AbrirCadastro(sender);
        }

        protected void btnAdicionarGerenteAdjunto_OnServerClick(object sender, EventArgs e)
        {
            ltrTituloModal.Text = "Cadastrar Gerente Adjunto";

            AbrirCadastro(sender);
        }

        protected void btnAdicionarFuncionario_OnServerClick(object sender, EventArgs e)
        {
            ltrTituloModal.Text = "Cadastrar Funcionário";

            AbrirCadastro(sender);
        }

        private void AbrirCadastro(object sender)
        {
            var button = (HtmlButton)sender;

            var cargoId = int.Parse(button.Attributes["data-cargo"]);

            ExibirModal(cargoId);
        }

        protected void btnRemoverUsuario_OnServerClick(object sender, EventArgs e)
        {
            var button = (HtmlButton)sender;

            var usuarioCargoId = int.Parse(button.Attributes["data-usuariocargo"]);

            try
            {
                var manterUsuarioCargo = new ManterUsuarioCargo();

                var usuarioCargo = manterUsuarioCargo.ObterPorId(usuarioCargoId);

                var processosQueFicaraoSemAnalista =
                    new SgusWebService.ManterProcesso().ConsultarEtapasPorUsuarioCargo(usuarioCargo.Usuario,
                        new List<Cargo> { usuarioCargo.Cargo }, true)
                        .Select(x => new { x.ProcessoResposta.ID })
                        .ToList()
                        .Select(x => $"#{x.ID}")
                        .ToList();

                if (processosQueFicaraoSemAnalista.Any())
                {
                    ltrSolicitacoesSemAnalistaExclusao.Text = string.Join(", ",
                        processosQueFicaraoSemAnalista);

                    hdnUsuarioCargoIdRemover.Value = usuarioCargoId.ToString();

                    ltrCargoRemover.Text = usuarioCargo.Cargo.TipoCargo.GetDescription();

                    ExibirBackdrop();

                    pnlAvisoExclusao.Visible = true;
                }
                else
                {
                    manterUsuarioCargo.Remover(usuarioCargo);

                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Usuário removido da hierarquia");

                    PreencherHierarquia();
                }
            }
            catch (Exception)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta,
                    "Erro ao remover usuário da hierarquia. Verifique se o mesmo foi removido.");
            }
        }

        protected void btnRemover_OnClick(object sender, EventArgs e)
        {
            var usuarioCargoId = int.Parse(hdnUsuarioCargoIdRemover.Value);

            new ManterUsuarioCargo().Remover(usuarioCargoId);

            WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Usuário removido da hierarquia");

            OcultarModalExclusao_Click(null, null);
        }


        protected void ConfirmarInativacaoDiretoria_OnClick(object sender, EventArgs e)
        {
            var cargoId = int.Parse(hdnCargoIdInativar.Value);

            var cargoDiretoria = new ManterCargo().ObterPorId(cargoId);

            cargoDiretoria.Ativo = false;

            new ManterCargo().Salvar(cargoDiretoria);

            WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Diretoria inativada na hierarquia");

            OcultarModalInativaDiretoria_Click(null, null);
        }

        protected void OcultarModalInativaDiretoria_Click(object sender, EventArgs e)
        {
            OcultarBackdrop();

            pnlAvisoInativarDiretoria.Visible = false;

            PreencherHierarquia();
        }

        private void ExibirModal(int cargoId)
        {
            var cargo = new ManterCargo().ObterPorId(cargoId);

            if (cargo == null)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro ao obter cargo");
                return;
            }

            string nome;

            switch (cargo.TipoCargo)
            {
                case EnumTipoCargo.Gabinete:
                    nome = cargo.CargoPai.Nome;
                    break;
                case EnumTipoCargo.Funcionario:
                    //nome = cargo.CargoPai.CargoPai.Nome;
                    nome = cargo.CargoPai.Nome;
                    break;
                default:
                    nome = cargo.Nome;
                    break;
            }

            txtCargoModal.Text = cargo.TipoCargo.GetDescription() + " de " + nome;
            pnlModal.Visible = true;
            // Preencher campos do Modal.
            hdnCargoId.Value = cargo.ID.ToString();
            txtUfModal.Text = UfSelecionada.Nome + " - " + UfSelecionada.Sigla;


            ExibirBackdrop();
        }

        private void LimparModal()
        {
            txtCargoModal.Text = "";
            hdnCargoId.Value = "";
        }

        protected void OcultarModal_Click(object sender, EventArgs e)
        {
            LupaUsuario.LimparCampos();

            OcultarBackdrop();

            pnlModal.Visible = false;

            LimparModal();

            PreencherHierarquia();
        }

        protected void OcultarModalVerificacao_Click(object sender, EventArgs e)
        {
            OcultarBackdrop();

            pnlAviso.Visible = false;

            PreencherHierarquia();
        }

        protected void OcultarModalExclusao_Click(object sender, EventArgs e)
        {
            OcultarBackdrop();

            pnlAvisoExclusao.Visible = false;

            PreencherHierarquia();
        }

        private void ExibirBackdrop()
        {
            if (Master != null && Master.Master != null)
            {
                var pnlbackdrop = ((Panel)Master.Master.FindControl("pnlbackdrop"));

                if (pnlbackdrop != null)
                {
                    pnlbackdrop.Visible = true;
                }
            }
        }

        private void OcultarBackdrop()
        {
            if (Master != null && Master.Master != null)
            {
                var pnlbackdrop = ((Panel)Master.Master.FindControl("pnlbackdrop"));

                if (pnlbackdrop != null)
                {
                    pnlbackdrop.Visible = false;
                }
            }
        }

        protected void btnSalvar_OnClick(object sender, EventArgs e)
        {
            var usuarioSelecionado = LupaUsuario.SelectedUser;

            if (usuarioSelecionado == null)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Usuário é obrigatório");
                return;
            }

            var cargo = new ManterCargo().ObterPorId(int.Parse(hdnCargoId.Value));

            var cargosUsuario = usuarioSelecionado.ListaUsuarioCargo.AsQueryable();

            var possuiAviso = false;

            var cargoSubstituiraOsOutros = cargo.UsuarioPodeRepetirNoCargo() == false ||
                                           cargosUsuario.Any(x => x.Cargo.UsuarioPodeRepetirNoCargo() == false);

            if (cargoSubstituiraOsOutros)
            {
                var cargosAtuais = cargosUsuario.Select(x => new { Nome = x.Cargo.ObterNomeCompleto() })
                    .ToList()
                    .Select(x => x.Nome)
                    .ToList();

                if (cargosAtuais.Any())
                {
                    possuiAviso = true;
                    ltrColaboradorEmOutrosCargos.Text = string.Join(", ", cargosAtuais);
                    ltrNomeColaboradorEmOutrosCargos.Text = usuarioSelecionado.Nome;
                    divColaboradorEmOutrosCargos.Visible = true;
                }
            }

            var todosCargosSaoRepetitiveis = cargo.UsuarioPodeRepetirNoCargo() &&
                                             cargosUsuario.All(x => x.Cargo.UsuarioPodeRepetirNoCargo());

            if (todosCargosSaoRepetitiveis)
            {
                divColaboradorEmOutrosCargosRepetiveis.Visible = true;
                ltrNomeColaboradorEmOutrosCargos.Text = usuarioSelecionado.Nome;
                btnMover.Text = "Mover";
                btnDuplicar.Visible = true;
                possuiAviso = true;
            }
            else
            {
                divColaboradorEmOutrosCargosRepetiveis.Visible = false;
                btnMover.Text = "Salvar";
                btnDuplicar.Visible = false;
            }

            if (cargoSubstituiraOsOutros || todosCargosSaoRepetitiveis)
            {
                // Obter os processos abertos que ficarão sem analistas caso o cargo do indivíduo seja alterado.
                var processosQueFicaraoSemAnalista =
                    new SgusWebService.ManterProcesso().ConsultarEtapasPorUsuarioCargo(usuarioSelecionado,
                        usuarioSelecionado.ListaUsuarioCargo.Select(x => x.Cargo).ToList(), true)
                        .Select(x => new { x.ProcessoResposta.ID })
                        .ToList()
                        .Select(x => $"#{x.ID}")
                        .ToList();

            }

            // Criar o UsuarioCargo temporariamente só para obter os novos processos disponíveis para o novo usuário.
            var usuarioCargo = new UsuarioCargo
            {
                Usuario = usuarioSelecionado,
                Cargo = cargo
            };

            var manterUsuarioCargo = new ManterUsuarioCargo();

            manterUsuarioCargo.Salvar(usuarioCargo);

            try
            {
                var novasSolicitacoesDisponiveis =
                new SgusWebService.ManterProcesso().ConsultarEtapasPorUsuarioCargo(usuarioSelecionado,
                    new List<Cargo> { cargo }, true)
                    .Select(x => new { x.ProcessoResposta.ID })
                    .ToList()
                    .Select(x => $"#{x.ID}")
                    .ToList();

                if (novasSolicitacoesDisponiveis.Any())
                {
                    possuiAviso = true;

                    ltrNovasSolicitacoesDisponiveis.Text = string.Join(", ",
                            novasSolicitacoesDisponiveis);

                    divNovasSolicitacoesDisponiveis.Visible = true;
                }
            }
            finally
            {
                // Remover UsuarioCargo temporário.
                manterUsuarioCargo.Remover(usuarioCargo);
            }


            // Caso possua avisos, esconde o modal de cadastro e exibe os avisos.
            if (possuiAviso)
            {
                pnlModal.Visible = false;
                pnlAviso.Visible = true;
            }
            else
            {
                // Se não possui avisos, já salva direto.
                SalvarUsuarioCargo(usuarioSelecionado, cargo);
                OcultarModal_Click(null, null);
            }
        }

        protected void btnMover_OnClick(object sender, EventArgs e)
        {
            DuplicarOuMover(true);
        }

        protected void btnDuplicar_OnClick(object sender, EventArgs e)
        {
            DuplicarOuMover(false);
        }

        private void DuplicarOuMover(bool mover)
        {
            var cargo = new ManterCargo().ObterPorId(int.Parse(hdnCargoId.Value));

            pnlAviso.Visible = false;

            SalvarUsuarioCargo(LupaUsuario.SelectedUser, cargo, mover);
            OcultarModalVerificacao_Click(null, null);
        }

        private void SalvarUsuarioCargo(Usuario usuarioSelecionado, Cargo cargo, bool mover = false)
        {
            try
            {
                // Salvar o usuário. Se for Gabinete ou Diretor, substitui o usuário atual.
                new ManterUsuarioCargo().Salvar(usuarioSelecionado, cargo, mover);

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Usuário incluído com sucesso.");
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
            catch (Exception)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro,
                    "Erro ao salvar o usuário no cargo. Verifique se foi salvo e tente novamente.");
            }
        }

        protected void Voltar_OnServerClick(object sender, EventArgs e)
        {
            Response.Redirect("ListarHierarquia.aspx");
        }
    }
}