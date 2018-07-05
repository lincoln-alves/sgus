using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using classes = Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.BP;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.WebForms.UserControls;
using System.Web.Script.Serialization;
using Sebrae.Academico.Util.Classes;
using System.Collections;
using Sebrae.Academico.Dominio.DTO;

namespace Sebrae.Academico.WebForms.Cadastros.Etapa
{
    public partial class EditarEtapa : PageBase
    {
        private classes.Etapa _etapa = new classes.Etapa();
        private Processo _processo;
        private ManterEtapa manterEtapa = new ManterEtapa();
        private ManterProcesso manterProcesso = new ManterProcesso();
        private List<DTONomeCampo> listaNomeCampo = new List<DTONomeCampo>();
        private List<dynamic> listaTipoCampo = new List<dynamic>();
        private List<dynamic> listaTipoDado = new List<dynamic>();

        protected void Page_Load(object sender, EventArgs e)
        {
            criaListaNomeCampos();
            var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

            if (!Page.IsPostBack)
            {
                //Preenchendo checkboxes das permissões por UF/Nível Ocupacional/Perfil
                ucPermissoes.PreencherListas();
            }

            if (Request["Id"] != null)
            {
                var idModel = int.Parse(Request["Id"]);
                _etapa = manterEtapa.ObterPorID(idModel);
                _processo = _etapa.Processo;

                if (!Page.IsPostBack)
                {
                    if (!usuarioLogado.IsAdministrador())
                    {
                        ucLupaMultiplosUsuarios.Uf = usuarioLogado.UF;
                    }

                    popularCamposVinculados();

                    PreencherNucleos();

                    if (usuarioLogado.IsGestor() && (_etapa.Processo.Uf == null || _etapa.Processo.Uf.ID != usuarioLogado.UF.ID))
                    {
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Processo inexistente", "ListarDemanda.aspx");
                        return;
                    }

                    PreencherCampos(_etapa);
                    PreencherFormulariosPermissoes(_etapa.Permissoes);
                    AlterarVisibilidadePermissoes(_etapa.PrimeiraEtapa);
                }
            }

            if (Request["IdProcesso"] != null)
            {
                var idProcesso = int.Parse(Request["IdProcesso"]);
                _processo = manterProcesso.ObterPorID(idProcesso);

                if (usuarioLogado.IsGestor() && (_processo.Uf == null || _processo.Uf.ID != usuarioLogado.UF.ID))
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Processo inexistente", "../Demanda/ListarDemanda.aspx");
                    return;
                }

                txtProcesso.Text = _processo.Nome;

                AlterarVisibilidadePermissoes(!manterProcesso.PossuiEtapas(idProcesso));
            }

            //Disponibilizando variavel "isPostBack" no cliente
            var javascript = string.Format("var isPostBack = {0};", IsPostBack ? "true" : "false");
            ClientScript.RegisterClientScriptBlock(GetType(), "IsPostBack", javascript, true);
            ucLupaUsuarioAssinatura.Text = "Usuário assinatura";

        }

        private void PreencherListaAjuste(classes.Etapa etapa)
        {
            WebFormHelper.PreencherListaCustomizado(typeof(enumNomeAjusteDemanda), ddlNomeBotaoAjuste);

            if (etapa.NomeBotaoAjuste == enumNomeAjusteDemanda.Ajustar.GetDescription())
            {
                ddlNomeBotaoAjuste.SelectedValue = ((int)enumNomeAjusteDemanda.Ajustar).ToString();
                AjusteOutro.Visible = false;
            }
            else
            {
                ddlNomeBotaoAjuste.SelectedValue = ((int)enumNomeAjusteDemanda.Outros).ToString();
                AjusteOutro.Visible = true;
            }

        }

        private void criaListaNomeCampos()
        {
            //LISTA COM TIPOS DE CAMPOS
            listaTipoCampo.Add(new
            {
                ID = 1,
                Nome = "Texto"
            });
            listaTipoCampo.Add(new
            {
                ID = 2,
                Nome = "Área de Texto"
            });
            listaTipoCampo.Add(new
            {
                ID = 3,
                Nome = "Senha"
            });
            listaTipoCampo.Add(new
            {
                ID = 4,
                Nome = "Lista de Opções"
            });
            listaTipoCampo.Add(new
            {
                ID = 5,
                Nome = "Radiobutton"
            });
            listaTipoCampo.Add(new
            {
                ID = 6,
                Nome = "Checkbox"
            });
            listaTipoCampo.Add(new
            {
                ID = 7,
                Nome = "Arquivo"
            });
            listaTipoCampo.Add(new
            {
                ID = 8,
                Nome = "Campo do Usuario"
            });
            listaTipoCampo.Add(new
            {
                ID = 9,
                Nome = "Label"
            });
            listaTipoCampo.Add(new
            {
                ID = 10,
                Nome = "Conteúdo HTML"
            });
            listaTipoCampo.Add(new
            {
                ID = 11,
                Nome = "Somatório"
            });
            listaTipoCampo.Add(new
            {
                ID = 12,
                Nome = "Divisor"
            });
            listaTipoCampo.Add(new
            {
                ID = 13,
                Nome = "Multiplicador"
            });
            listaTipoCampo.Add(new
            {
                ID = 14,
                Nome = "Porcentagem"
            });
            listaTipoCampo.Add(new
            {
                ID = 15,
                Nome = "Questionário"
            });
            listaTipoCampo.Add(new
            {
                ID = 16,
                Nome = "Subtração"
            });
            listaTipoCampo.Add(new
            {
                ID = 17,
                Nome = "Múltiplos arquivos"
            });

            //LISTA COM TIPOS DE DADOS
            listaTipoDado.Add(new
            {
                ID = 1,
                Nome = "Inteiro"
            });
            listaTipoDado.Add(new
            {
                ID = 2,
                Nome = "Decimal"
            });
            listaTipoDado.Add(new
            {
                ID = 10,
                Nome = "Moeda"
            });
            listaTipoDado.Add(new
            {
                ID = 3,
                Nome = "Data"
            });
            listaTipoDado.Add(new
            {
                ID = 4,
                Nome = "Data e Hora"
            });
            listaTipoDado.Add(new
            {
                ID = 7,
                Nome = "CPF"
            });
            listaTipoDado.Add(new
            {
                ID = 8,
                Nome = "CNPJ"
            });
            listaTipoDado.Add(new
            {
                ID = 9,
                Nome = "Email"
            });
            listaTipoDado.Add(new
            {
                ID = 11,
                Nome = "Telefone"
            });
            listaTipoDado.Add(new
            {
                ID = 5,
                Nome = "Texto (Menos de 2000 caracteres)"
            });
            listaTipoDado.Add(new
            {
                ID = 6,
                Nome = "Texto (Mais de 2000 caracteres)"
            });
            listaTipoDado.Add(new
            {
                ID = 12,
                Nome = "Texto HTML"
            });

            //LISTA COM NOMES DE CAMPOS
            listaNomeCampo.Add(new DTONomeCampo()
            {
                ID = 1,
                Nome = "Carga-horária",
                TipoCampo = new List<int> { 1, 11 },
                TipoDado = new List<int> { 1 }
            });
            listaNomeCampo.Add(new DTONomeCampo()
            {
                ID = 2,
                Nome = "Data de Início da Capacitação",
                TipoCampo = new List<int> { 1 },
                TipoDado = new List<int> { 3 }
            });
            listaNomeCampo.Add(new DTONomeCampo()
            {
                ID = 3,
                Nome = "Data de Término da Capacitação",
                TipoCampo = new List<int> { 1 },
                TipoDado = new List<int> { 3 }
            });
            listaNomeCampo.Add(new DTONomeCampo()
            {
                ID = 4,
                Nome = "Local",
                TipoCampo = new List<int> { 1, 2 },
                TipoDado = new List<int> { 5, 6 }
            });
            listaNomeCampo.Add(new DTONomeCampo
            {
                ID = 5,
                Nome = "Título da Capacitação",
                TipoCampo = new List<int> { 1 },
                TipoDado = new List<int> { 5, 6 }
            });
            listaNomeCampo.Add(new DTONomeCampo
            {
                ID = 6,
                Nome = "Valor Previsto de Inscrição",
                TipoCampo = new List<int> { 1, 11, 12, 13 },
                TipoDado = new List<int> { 10 }
            });
            listaNomeCampo.Add(new DTONomeCampo
            {
                ID = 7,
                Nome = "Valor Previsto de Passagem",
                TipoCampo = new List<int> { 1, 11, 12, 13 },
                TipoDado = new List<int> { 10 }
            });
            listaNomeCampo.Add(new DTONomeCampo
            {
                ID = 8,
                Nome = "Valor Previsto de Diária",
                TipoCampo = new List<int> { 1, 11, 12, 13 },
                TipoDado = new List<int> { 10 }
            });
            listaNomeCampo.Add(new DTONomeCampo
            {
                ID = 9,
                Nome = "Valor Executado de Inscrição",
                TipoCampo = new List<int> { 1, 11, 12, 13 },
                TipoDado = new List<int> { 10 }
            });
            listaNomeCampo.Add(new DTONomeCampo
            {
                ID = 10,
                Nome = "Valor Executado de Passagem",
                TipoCampo = new List<int> { 1, 11, 12, 13 },
                TipoDado = new List<int> { 10 }
            });
            listaNomeCampo.Add(new DTONomeCampo
            {
                ID = 11,
                Nome = "Valor Executado de Diária",
                TipoCampo = new List<int> { 1, 11, 12, 13 },
                TipoDado = new List<int> { 10 }
            });
            listaNomeCampo.Add(new DTONomeCampo
            {
                ID = 12,
                Nome = "Outros"
            });
        }

        private void popularNomeCampos(int idEtapa, Campo campoEdicao = null)
        {
            var listaNomeCampoPreencher = listaNomeCampo;
            if (idEtapa > 0)
            {
                List<classes.Campo> listaCampos = new ManterCampo().ObterPorEtapa(idEtapa).OrderBy(d => d.Ordem).ToList();
                foreach (Campo campo in listaCampos)
                {
                    listaNomeCampoPreencher = listaNomeCampoPreencher.Where(x => x.Nome != campo.Nome).ToList();
                }
            }

            if (campoEdicao != null && campoEdicao.Nome != null)
            {
                var campoAtual = listaNomeCampo.Where(x => x.Nome == campoEdicao.Nome).FirstOrDefault();
                if (campoAtual != null) listaNomeCampoPreencher.Add(campoAtual);
            }

            WebFormHelper.PreencherListaCustomizado(listaNomeCampoPreencher, ddlNomeCampo, "ID", "Nome", false, false);

            var idNomeCampo = listaNomeCampoPreencher.Select(x => x.ID).FirstOrDefault();

            if (idNomeCampo == 12)
            {
                txtNomeCampo.Visible = true;
            }
            else
            {
                txtNomeCampo.Visible = false;
            }

            popularTipoCampo(idNomeCampo);
            popularTipoDado(idNomeCampo);
        }

        private void PreencherNucleos()
        {
            if (_processo != null)
            {
                var idUfProcesso = _processo.Uf.ID;
                // Preenche permissões de análise do núcleo
                if (idUfProcesso > 0)
                {
                    var nucleos = new ManterHierarquiaNucleo().ObterTodos().Where(x => x.Ativo && x.Uf.ID == idUfProcesso && x.HierarquiaNucleoUsuarios.Any());
                    ucListBoxPermissoes.PreencherItens(nucleos, "ID", "Nome");
                }
            }
        }

        private void popularCamposVinculados()
        {
            var manterCampo = new ManterCampo();
            string etapa = Request["Id"];

            var listaCampos = !string.IsNullOrEmpty(etapa) ? manterCampo.ObterPorEtapa(int.Parse(etapa)) : manterCampo.ObterTodas();

            // Não retorna para seleção de alternativa o próprio campo de alternativas.
            listaCampos = listaCampos.Where(c => c.ListaAlternativas != null && c.ListaAlternativas.Count <= 0).ToList();
            WebFormHelper.PreencherLista(listaCampos, ddlCampoVinculado, false, true);
        }

        protected void ddlNomeCampo_OnSelectIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)(sender);
            int idSelecaoUsuario = int.Parse(ddl.SelectedValue);

            if (idSelecaoUsuario > 0)
            {
                if (idSelecaoUsuario == 12)
                {
                    txtNomeCampo.Visible = true;
                }
                else
                {
                    txtNomeCampo.Text = "";
                    txtNomeCampo.Visible = false;
                }

                popularTipoCampo(idSelecaoUsuario);
                popularTipoDado(idSelecaoUsuario);
            }
        }

        protected void popularTipoCampo(int idNomeCampo)
        {
            var listaTipoCampoSelecionados = listaTipoCampo;
            if (idNomeCampo > 0 && idNomeCampo != 12)
            {
                var campoNomeSelecionado = listaNomeCampo.Where(x => x.ID == idNomeCampo);
                listaTipoCampoSelecionados = listaTipoCampoSelecionados.Where(tipo => campoNomeSelecionado.Any(x => x.TipoCampo.Contains(tipo.ID))).ToList();
            }
            WebFormHelper.PreencherListaCustomizado(listaTipoCampoSelecionados, ddlTipoCampo, "ID", "Nome", false, false);
        }

        protected void popularTipoDado(int idNomeCampo)
        {
            var listaTipoDadoSelecionados = listaTipoDado;
            if (idNomeCampo > 0 && idNomeCampo != 12)
            {
                var campoNomeSelecionado = listaNomeCampo.Where(x => x.ID == idNomeCampo);
                listaTipoDadoSelecionados = listaTipoDadoSelecionados.Where(tipo => campoNomeSelecionado.Any(x => x.TipoDado.Contains(tipo.ID))).ToList();
            }
            WebFormHelper.PreencherListaCustomizado(listaTipoDadoSelecionados, ddlTipoDado, "ID", "Nome", false, false);
        }

        protected void ddlCampoDoUsuario_OnSelectIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)(sender);
            int idSelecaoUsuario = int.Parse(ddl.SelectedValue);
            pnlEtapaCampos.Visible = false;

            enumTipoCampo tipo = (enumTipoCampo)idSelecaoUsuario;

            ControlarExibicaoAtributosPorTipoCampo(tipo);

            if (tipo == enumTipoCampo.Field)
            {
                PreencherDDLCamposUsuario();
            }

            if (tipo == enumTipoCampo.Questionário)
            {
                PreencherQuestionarios();
            }

            if (tipo == enumTipoCampo.Divisor)
            {
                int idEtapa;
                if (Request["Id"] != null && int.TryParse(Request["Id"], out idEtapa))
                {
                    var campos = new ManterCampo()
                        .ObterPorEtapa(idEtapa)
                        .Where(x => x.TipoDado == (int)enumTipoDado.Inteiro || x.TipoDado == (int)enumTipoDado.Moeda || x.TipoDado == (int)enumTipoDado.Decimal);

                    foreach (Campo campo in campos)
                    {
                        ddlCampoDivisor.Items.Add(new ListItem() { Text = campo.Nome, Value = campo.ID.ToString() });
                        ddlCampoDividendo.Items.Add(new ListItem() { Text = campo.Nome, Value = campo.ID.ToString() });
                    }
                }
            }

            if (tipo == enumTipoCampo.Percentual || tipo == enumTipoCampo.Somatório || tipo == enumTipoCampo.Multiplicador || tipo == enumTipoCampo.Subtracao)
            {
                var listaTipoDadoSelecionados = listaTipoDado.Where(x => x.ID == (int)enumTipoDado.Decimal).ToList();

                WebFormHelper.PreencherListaCustomizado(listaTipoDadoSelecionados, ddlTipoDado, "ID", "Nome", false, false);

                PreencherSelecaoDeEtapas(_etapa);
            }

            Campo campoEditar = new Campo();
            campoEditar.TipoCampo = (byte)idSelecaoUsuario;
            campoEditar.TipoDado = (byte)int.Parse(ddlTipoDado.SelectedValue);
            loadMetaFields(campoEditar);

            var idNomeCampo = int.Parse(ddlNomeCampo.SelectedValue);
            if (idNomeCampo != 12)
            {
                popularTipoCampo(idNomeCampo);
            }
        }

        private void ControlarExibicaoAtributosPorTipoCampo(enumTipoCampo tipo)
        {
            pnlSomatorio.Visible = false;
            pnlDivisao.Visible = false;
            pnlTipoDado.Visible = false;
            pnlTamanhoCampo.Visible = false;
            txtTamanhoCampo.Text = "0";
            pnlPermitirNulo.Visible = false;
            pnlQuestionario.Visible = false;

            switch (tipo)
            {
                case enumTipoCampo.MultipleFileUpload:
                case enumTipoCampo.FileUpload:
                    pnlPermitirNulo.Visible = true;
                    pnlAjuda.Visible = true;
                    break;
                case enumTipoCampo.Field:
                    pnlCampoDoUsuario.Visible = true;
                    pnlAjuda.Visible = false;
                    break;
                case enumTipoCampo.Html:
                    labelConteudoHtml.Visible = true;
                    ucHelperTooltipAjudaHtml.Visible = true;
                    labelAjuda.Visible = false;
                    ucHelperTooltipAjuda.Visible = false;
                    txtAjuda.Height = 200;
                    break;
                case enumTipoCampo.Divisor:
                    pnlSomatorio.Visible = false;
                    pnlTipoDado.Visible = false;
                    pnlTamanhoCampo.Visible = false;
                    txtTamanhoCampo.Text = "100";
                    pnlPermitirNulo.Visible = false;
                    pnlQuestionario.Visible = false;
                    pnlDivisao.Visible = true;
                    break;
                case enumTipoCampo.Somatório:
                case enumTipoCampo.Multiplicador:
                case enumTipoCampo.Percentual:
                case enumTipoCampo.Subtracao:
                    pnlSomatorio.Visible = false;
                    pnlTipoDado.Visible = false;
                    pnlTamanhoCampo.Visible = false;
                    txtTamanhoCampo.Text = "100";
                    pnlPermitirNulo.Visible = false;
                    pnlQuestionario.Visible = false;
                    pnlDivisao.Visible = false;
                    break;
                case enumTipoCampo.Questionário:
                    break;
                default:
                    pnlDivisao.Visible = false;
                    pnlSomatorio.Visible = false;
                    pnlTipoDado.Visible = true;
                    pnlPermitirNulo.Visible = true;
                    pnlAjuda.Visible = true;
                    pnlTamanhoCampo.Visible = true;
                    txtTamanhoCampo.Text = "";
                    pnlQuestionario.Visible = false;

                    pnlCampoDoUsuario.Visible = false;

                    labelConteudoHtml.Visible = false;
                    ucHelperTooltipAjudaHtml.Visible = false;

                    labelAjuda.Visible = true;
                    ucHelperTooltipAjuda.Visible = true;
                    txtAjuda.Height = 50;
                    break;
            }

        }

        private void PreencherQuestionarios()
        {
            enumTipoQuestionario[] tipos = { enumTipoQuestionario.Demanda };

            var questionarios = new ManterQuestionario().ObterQuestionariosTipo(tipos);
            WebFormHelper.PreencherListaCustomizado(questionarios, ddlQuestionario, "Id", "Nome", false, false);
        }

        protected void ddlTipoDado_OnSelectIndexChanged(object sender, EventArgs e)
        {
            PreencheMetaFields();
        }

        protected void ddlEtapa_OnSelectIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)(sender);
            int idEtapaSelecionada = int.Parse(ddl.SelectedValue);

            var campos = new ManterCampo().ObterPorEtapa(idEtapaSelecionada).Where(x => x.TipoCampo == (int)enumTipoCampo.Text && (x.TipoDado == (int)enumTipoDado.Inteiro ||
                                                                                   x.TipoDado == (int)enumTipoDado.Moeda ||
                                                                                   x.TipoDado == (int)enumTipoDado.Decimal)).ToList();

            ucListBoxesCamposEtapa.PreencherItens(campos, "ID", "Nome", false, false);
        }

        private void PreencheMetaFields()
        {
            var campoEditar = new Campo
            {
                TipoCampo = (byte)int.Parse(ddlTipoCampo.SelectedValue),
                TipoDado = (byte)int.Parse(ddlTipoDado.SelectedValue)
            };
            loadMetaFields(campoEditar);

            var idNomeCampo = int.Parse(ddlNomeCampo.SelectedValue);
            if (idNomeCampo != 12)
            {
                popularTipoDado(idNomeCampo);
            }

        }

        private void PreencherDDLCamposUsuario()
        {
            var campos = Enum.GetValues(typeof(enumCampoUsuario)).Cast<enumCampoUsuario>().Select(campo =>
                new ListItem()
                {
                    Text = campo.GetDescription(),
                    Value = ((int)campo).ToString()
                }).OrderBy(campo => campo.Text).ToArray();

            ddlCampoDoUsuario.Items.AddRange(campos);
        }

        private void PreencherCampos(classes.Etapa model)
        {
            if (!string.IsNullOrWhiteSpace(model.Nome) && string.IsNullOrEmpty(txtNome.Text))
            {
                txtNome.Text = model.Nome;
            }

            txtProcesso.Text = model.Processo.Nome;

            if (model.UsuarioAssinatura != null)
                ucLupaUsuarioAssinatura.SelectedUser = model.UsuarioAssinatura;

            if (model.Permissoes.Any(p => p.DiretorCorrespondente == true))
            {
                ckbPodeSerAprovadoChefeGabinete.Checked = model.PodeSerAprovadoChefeGabinete;
                ckbNotificaDiretorAnalise.Checked = model.NotificaDiretorAnalise;
            }

            // Caso tenha permissões de núcleo, preencher os lists
            if (model.PermissoesNucleo.Any())
            {
                rblTipoPermissaoAnalise.SelectedValue = "NucleosUC";
                PreencherPemissoesNucleo(model);
            }

            //Arquivo Anexo
            if (model.FileServer != null && !string.IsNullOrWhiteSpace(model.FileServer.NomeDoArquivoOriginal))
            {
                pnlDownloadArquivo.Visible = true;
                lkbArquivo.Text = string.Concat("Abrir arquivo ", model.FileServer.NomeDoArquivoOriginal);
                lkbArquivo.NavigateUrl = ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.EnderecoSGUS).Registro + "/ExibirFileServer.ashx?Identificador=" + model.FileServer.NomeDoArquivoNoServidor;
            }
            else
            {
                pnlDownloadArquivo.Visible = false;
            }

            pnlCampos.Visible = true;
            List<classes.Campo> listaCampos = new ManterCampo().ObterPorEtapa(model.ID).OrderBy(d => d.Ordem).ToList();
            rptCampos.DataSource = listaCampos;
            rptCampos.DataBind();

            if (model.RequerAprovacao)
            {
                rblRequerAtivacao.Items.FindByValue("1").Selected = true;
                rblRequerAtivacao.Items.FindByValue("0").Selected = false;
                divContainerAjuste.Visible =
                divContainerNomes.Visible = true;

                // NOMES DA APROVAÇÃO
                var nomesAprovacao = new ManterNomeFinalizacaoEtapa().ObterTodos();

                nomesAprovacao.Add(new NomeFinalizacaoEtapa
                {
                    ID = -1,
                    Nome = "- Outros - "
                });

                WebFormHelper.PreencherLista(nomesAprovacao, ddlNomeFinalizacaoEtapa, false, true);

                ddlNomeFinalizacaoEtapa.SelectedValue = model.NomeFinalizacaoEtapa != null ? model.NomeFinalizacaoEtapa.ID.ToString() : model.NomeBotaoFinalizacao != null ? "-1" : "";

                if (model.NomeBotaoFinalizacao != null)
                {
                    divNomeFinalizacaoEtapaOutros.Visible = true;
                    txtNomeFinalizacaoOutro.Text = model.NomeBotaoFinalizacao;
                }


                // NOMES DA REPROVAÇÃO
                var nomesReprovacao = new ManterNomeReprovacaoEtapa().ObterTodos();

                nomesReprovacao.Add(new NomeReprovacaoEtapa
                {
                    ID = -1,
                    Nome = "- Outros - "
                });

                WebFormHelper.PreencherLista(nomesReprovacao, ddlNomeReprovacaoEtapa, false, true);

                ddlNomeReprovacaoEtapa.SelectedValue = model.NomeReprovacaoEtapa != null ? model.NomeReprovacaoEtapa.ID.ToString() : model.NomeBotaoReprovacao != null ? "-1" : "";

                if (model.NomeBotaoReprovacao != null)
                {
                    divNomeReprovacaoEtapaOutros.Visible = true;
                    txtNomeReprovacaoOutro.Text = model.NomeBotaoReprovacao;
                }

                if (model.EtapaRetorno != null)
                {
                    PreencherListaAjuste(_etapa);

                    rblBotaoAjuste.Items.FindByValue("1").Selected =
                    divContainerAjuste.Visible = true;

                    txtNomeAjuste.Text = _etapa.NomeBotaoAjuste;

                    rblBotaoAjuste.Items.FindByValue("1").Selected = true;
                    rblBotaoAjuste.Items.FindByValue("0").Selected = false;
                    pnlRetornarSelect.Visible = true;
                    PreencherRetornoEtapa(model.Processo.ID, model.Ordem);
                    ddlEtapaRetorno.ClearSelection();
                    ddlEtapaRetorno.Items.FindByValue(model.EtapaRetorno.ID.ToString()).Selected = true;
                }
                else
                {
                    rblBotaoAjuste.Items.FindByValue("0").Selected = true;

                    rblBotaoAjuste.Items.FindByValue("0").Selected = true;
                    rblBotaoAjuste.Items.FindByValue("1").Selected = false;

                    divContainerAjuste.Visible = false;
                }

                if (model.PodeSerReprovada)
                {
                    rblTeraReprovacao.SelectedValue = "1";
                    divContainerAjuste.Visible = true;
                }
                else
                {
                    rblTeraReprovacao.SelectedValue = "0";
                }
            }
            else
            {
                rblRequerAtivacao.Items.FindByValue("0").Selected = true;
                rblRequerAtivacao.Items.FindByValue("1").Selected = false;
            }

            rblVisivelImpressao.Items.FindByValue("0").Selected = !model.VisivelImpressao;
            rblVisivelImpressao.Items.FindByValue("1").Selected = model.VisivelImpressao;

            txtPrazoEncaminharDemanda.Text = model.PrazoEncaminhamento != null ? model.PrazoEncaminhamento.Value.ToString() : "";

            if (model.NotificarNucleo)
                cblTipoPermissaoNotificacao.SelectedValue = "NucleosUC";
        }

        private void PreencherPemissoesNucleo(classes.Etapa model)
        {
            if (model.PermissoesNucleo.Count > 0)
            {
                var idsHierarquiaNucleoUsuario = model.PermissoesNucleo.Select(x => x.HierarquiaNucleoUsuario.ID).ToList();
                var hierarquiaNucleoUsuario = new ManterHierarquiaNucleoUsuario().ObterTodosIQueryable().Where(x => idsHierarquiaNucleoUsuario.Contains(x.ID));
                // Recupera o núcleo dos usuários analistas
                var idsHierarquia = hierarquiaNucleoUsuario.GroupBy(x => x.HierarquiaNucleo.ID).Select(x => x.Key).ToList();
                ucListBoxPermissoes.MarcarComoSelecionados(idsHierarquia);
            }
        }

        protected void btnRemover_UsuarioAssinatura(object sender, EventArgs e)
        {
            ucLupaUsuarioAssinatura.SelectedUser = null;
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                SalvarEtapa();

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso!",
                    "/Cadastros/Demanda/EditarDemanda.aspx?Id=" + _etapa.Processo.ID);
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private void SalvarEtapa()
        {
            ValidarCampos();

            _etapa = Request["Id"] == null ? new classes.Etapa() : manterEtapa.ObterPorID(int.Parse(Request["Id"]));

            _etapa.Nome = txtNome.Text.Trim();
            _etapa.RequerAprovacao = rblRequerAtivacao.SelectedValue == "1";
            _etapa.VisivelImpressao = rblVisivelImpressao.SelectedValue == "1";
            _etapa.UsuarioAssinatura = ucLupaUsuarioAssinatura.SelectedUser;
            _etapa.NotificarNucleo = cblTipoPermissaoNotificacao.Items[4].Selected;

            // Prazo encaminhamento.
            int vlPrazo;
            if (int.TryParse(txtPrazoEncaminharDemanda.Text, out vlPrazo))
            {
                _etapa.PrazoEncaminhamento = int.Parse(txtPrazoEncaminharDemanda.Text);
            }

            popularNomeCampos(_etapa.ID, null);

            //Arquivo Anexo
            CarregarAnexo();

            PreencherCamposCasoRequeiraAprovacao();

            var permissoes = RecuperarPermissoes();
            var permissoesNucleo = RecuperarPermissoesNucleoSelecionadas(_etapa);

            if (rblTipoPermissaoAnalise.SelectedValue == "NucleosUC" && permissoesNucleo.Count == 0)
            {
                throw new AcademicoException("Selecione ao menos um Núcleo UC!");
            }
            // Se estiver cadastrando uma etapa nova, verifica se é a primeira etapa pela 
            // contagem de outras etapas dentro do mesmo processo.
            if (Request["IdProcesso"] != null && _etapa.ID == 0)
                _etapa.PrimeiraEtapa = manterEtapa.ObterPorProcessoId(int.Parse(Request["IdProcesso"])).Count == 0;

            // Toda etapa possuirá permissões.
            AdicionarPermissoesSelecionadas(permissoes);

            _etapa.PodeSerAprovadoChefeGabinete =
                _etapa.Permissoes.Any(p => p.DiretorCorrespondente == true) &&
                ckbPodeSerAprovadoChefeGabinete.Checked;

            _etapa.NotificaDiretorAnalise =
                _etapa.Permissoes.Any(p => p.DiretorCorrespondente == true) &&
                ckbNotificaDiretorAnalise.Checked;

            if (Request["Id"] != null)
            {
                RemoverPermissoesNaoSelecionadas(permissoes);

                ReordenarCampos();
            }

            if (Request["IdProcesso"] != null)
            {
                _etapa.Processo = _processo;
                _etapa.Ordem = manterEtapa.ObterUltimaPosicaoOrdem();
            }

            _etapa.PodeSerReprovada = rblTeraReprovacao.SelectedValue == "1";

            // Salvar a etapa.
            manterEtapa.Salvar(_etapa, permissoesNucleo);
        }

        private void ValidarCampos()
        {
            // Validar texto do botão de aprovação.
            if (rblRequerAtivacao.SelectedValue == "1")
            {
                if (ddlNomeFinalizacaoEtapa.SelectedValue == "0")
                {
                    throw new AcademicoException("Nome do botão de aprovação inválido");
                }

                if (ddlNomeFinalizacaoEtapa.SelectedValue == "-1" &&
                    string.IsNullOrWhiteSpace(txtNomeFinalizacaoOutro.Text.Trim()))
                {
                    throw new AcademicoException("Nome do botão de aprovação obrigatório.");
                }
            }

            // Validar texto do botão de reprovação.
            if (rblRequerAtivacao.SelectedValue == "1" && _etapa.PodeSerReprovada)
            {
                if (ddlNomeReprovacaoEtapa.SelectedValue == "0")
                {
                    throw new AcademicoException("Nome do botão de reprovação inválido");
                }

                if (ddlNomeReprovacaoEtapa.SelectedValue == "-1" &&
                    string.IsNullOrWhiteSpace(txtNomeReprovacaoOutro.Text.Trim()))
                {
                    throw new AcademicoException("Nome do botão de reprovação obrigatório.");
                }
            }
        }

        private void ReordenarCampos()
        {
            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new[] { new DynamicJsonConverter() });
            dynamic obj = serializer.Deserialize(hdnOrdenacaoCampos.Value, typeof(object));

            if (obj != null)
            {
                manterEtapa.AlterarOrdemCampos(obj);
            }
        }

        private void PreencherCamposCasoRequeiraAprovacao()
        {
            if (_etapa.RequerAprovacao)
            {
                if (rblBotaoAjuste.SelectedValue == "1")
                {
                    _etapa.EtapaRetorno = new classes.Etapa() { ID = Convert.ToInt16(ddlEtapaRetorno.SelectedValue) };
                }
                else
                {
                    _etapa.EtapaRetorno = null;
                }

                var idNomeAprovacao = int.Parse(ddlNomeFinalizacaoEtapa.SelectedValue);

                if (idNomeAprovacao > 0)
                {
                    _etapa.NomeFinalizacaoEtapa = new ManterNomeFinalizacaoEtapa().ObterPorId(idNomeAprovacao);
                    _etapa.NomeBotaoFinalizacao = null;
                }
                else if (idNomeAprovacao < 0)
                {
                    _etapa.NomeFinalizacaoEtapa = null;
                    _etapa.NomeBotaoFinalizacao = txtNomeFinalizacaoOutro.Text;
                }

                var idNomeReprovacao = int.Parse(ddlNomeReprovacaoEtapa.SelectedValue);

                if (idNomeReprovacao > 0)
                {
                    _etapa.NomeReprovacaoEtapa = new ManterNomeReprovacaoEtapa().ObterPorId(idNomeReprovacao);
                    _etapa.NomeBotaoReprovacao = null;
                }
                else if (idNomeReprovacao < 0)
                {
                    _etapa.NomeReprovacaoEtapa = null;
                    _etapa.NomeBotaoReprovacao = txtNomeReprovacaoOutro.Text;
                }


                int idNomeAjuste;
                if (int.TryParse(ddlNomeBotaoAjuste.SelectedValue, out idNomeAjuste) && idNomeAjuste > 0)
                {
                    _etapa.NomeBotaoAjuste = ((enumNomeAjusteDemanda)idNomeAjuste).GetDescription();
                }
                else
                {
                    _etapa.NomeBotaoAjuste = txtNomeAjuste.Text;
                }
            }
        }

        private void CarregarAnexo()
        {
            if (fupldArquivoEnvio?.PostedFile != null && fupldArquivoEnvio.PostedFile.ContentLength > 0)
            {
                try
                {
                    var caminhoDiretorioUpload =
                        ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.RepositorioUpload).Registro;
                    // WebFormHelper.ObterCaminhoFisicoDoDiretorioDeUpload();
                    var nomeAleatorioDoArquivoParaUploadCriptografado = WebFormHelper.ObterStringAleatoria();
                    var diretorioDeUploadComArquivo = string.Concat(caminhoDiretorioUpload, @"\",
                        nomeAleatorioDoArquivoParaUploadCriptografado);

                    try
                    {
                        //Salva o arquivo no caminho especificado
                        fupldArquivoEnvio.PostedFile.SaveAs(diretorioDeUploadComArquivo);
                    }
                    catch
                    {
                        //Todo: -> Logar o Erro
                        throw new AcademicoException("Ocorreu um erro ao Salvar o arquivo");
                    }

                    _etapa.FileServer = new FileServer
                    {
                        NomeDoArquivoNoServidor = nomeAleatorioDoArquivoParaUploadCriptografado,
                        NomeDoArquivoOriginal = fupldArquivoEnvio.FileName,
                        TipoArquivo = fupldArquivoEnvio.PostedFile.ContentType,
                        MediaServer = false
                    };
                }
                catch (AcademicoException ex)
                {
                    throw ex;
                }
                catch
                {
                    //Todo: -> Logar erro
                    throw new AcademicoException("Ocorreu um Erro ao Salvar o arquivo");
                }
            }
        }

        private void RemoverPermissoesNaoSelecionadas(List<EtapaPermissao> permissoes)
        {
            var tiposValoresFinais = _etapa.Permissoes.Select(x => x.ObterTipoEtapaPermissao());

            var tiposValoresSelecionados = permissoes.Select(x => x.ObterTipoEtapaPermissao()).ToList();

            var tiposValoresPermissoesExclusao =
                tiposValoresFinais
                    .Where(
                        x =>
                            tiposValoresSelecionados.Any(y => y.Key == x.Key && y.Value.Equals(x.Value)) == false)
                    .ToList();

            var manterEtapaPermissao = new ManterEtapaPermissao();

            foreach (var permissaoTipovalor in tiposValoresPermissoesExclusao)
            {
                var permissao =
                    _etapa.Permissoes.FirstOrDefault(
                        x => classes.Etapa.TipoKeyValueEqual(x.ObterTipoEtapaPermissao(), permissaoTipovalor));

                _etapa.RemoverPermissao(permissaoTipovalor);

                manterEtapaPermissao.Excluir(permissao);
            }
        }

        private void AdicionarPermissoesSelecionadas(List<EtapaPermissao> permissoes)
        {
            foreach (var permissao in permissoes)
            {
                _etapa.AdicionarPermissao(permissao);
            }
        }

        private List<EtapaPermissaoNucleo> RecuperarPermissoesNucleoSelecionadas(classes.Etapa etapa)
        {
            var permissoes = new List<EtapaPermissaoNucleo>();

            var nucleosSelecionados = ucListBoxPermissoes.RecuperarIdsSelecionados().Select(int.Parse);
            var usuariosNucleos = new ManterHierarquiaNucleoUsuario().ObterTodosIQueryable();
            usuariosNucleos = usuariosNucleos.Where(x => nucleosSelecionados.Contains(x.HierarquiaNucleo.ID));

            permissoes.AddRange(usuariosNucleos.Select(x => new EtapaPermissaoNucleo
            {
                HierarquiaNucleoUsuario = x,
                Etapa = etapa
            }));

            return permissoes.ToList();
        }

        protected void Repeater_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                dynamic item = e.Item.DataItem as dynamic;

                ((LinkButton)e.Item.FindControl("Editar")).CommandArgument = (e.Item.DataItem as classes.Campo).ID.ToString();
                ((LinkButton)e.Item.FindControl("Excluir")).CommandArgument = (e.Item.DataItem as classes.Campo).ID.ToString();
                ((LinkButton)e.Item.FindControl("Duplicar")).CommandArgument = (e.Item.DataItem as classes.Campo).ID.ToString();

                if ((e.Item.DataItem as classes.Campo).TipoCampo == (int)enumTipoCampo.CheckBox
                    || (e.Item.DataItem as classes.Campo).TipoCampo == (int)enumTipoCampo.DropDownList
                    || (e.Item.DataItem as classes.Campo).TipoCampo == (int)enumTipoCampo.RadioButton)
                {
                    ((LinkButton)e.Item.FindControl("btnAbrirAlternativas")).CommandArgument = (e.Item.DataItem as classes.Campo).ID.ToString();
                }
                else
                {
                    ((LinkButton)e.Item.FindControl("btnAbrirAlternativas")).Visible = false;
                }
            }
        }

        /// <summary>
        /// Preenche dropdown de etapas anteriores na seleção de campos vinculados a uma etapa
        /// </summary>
        /// <param name="_etapa">Etapa atual</param>
        public void PreencherSelecaoDeEtapas(classes.Etapa _etapa, Campo campoEditar = null)
        {
            if (_etapa != null && _etapa.Processo != null)
            {
                pnlEtapaCampos.Visible = true;

                // Adiciona no dropdown as etapas anteriores e a etapa atual
                var listaEtapas = new BMEtapa().ObterOrdemMenoresPorProcessoId(_etapa.Processo.ID, _etapa.Ordem).OrderBy(d => d.Ordem).ToList();

                int idTipoCampo;

                if (int.TryParse(ddlTipoCampo.SelectedValue, out idTipoCampo) && idTipoCampo != (int)enumTipoCampo.Percentual)
                {
                    listaEtapas.Add(_etapa);
                }

                WebFormHelper.PreencherListaCustomizado(listaEtapas, ddlEtapa, "ID", "Nome", false, true);
            }
        }

        protected void EditarCampo_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            int idCampo = int.Parse(btn.CommandArgument);

            PreencherCamposModal(idCampo);

            base.ExibirBackDrop();
            pnlCampoModal.Visible = true;
        }

        private void PreencherCamposModal(int id)
        {
            var campoEditar = new ManterCampo().ObterPorID(id);

            CampoEdicaoId.Value = id.ToString();

            popularTipoCampo(0);
            ddlTipoCampo.ClearSelection();

            popularNomeCampos(campoEditar.Etapa.ID, campoEditar);

            ddlNomeCampo.ClearSelection();
            if (listaNomeCampo.Where(x => x.Nome == campoEditar.Nome).Any())
            {
                var nomeCampoSelecionado = listaNomeCampo.Where(x => x.Nome == campoEditar.Nome).FirstOrDefault();
                ddlNomeCampo.Items.FindByValue(nomeCampoSelecionado.ID.ToString()).Selected = true;
                txtNomeCampo.Visible = false;

                popularTipoCampo(nomeCampoSelecionado.ID);
                popularTipoDado(nomeCampoSelecionado.ID);
            }
            else
            {
                var nomeCampoSelecionado = listaNomeCampo.Where(x => x.ID == 12).FirstOrDefault();
                ddlNomeCampo.Items.FindByValue(nomeCampoSelecionado.ID.ToString()).Selected = true;
                txtNomeCampo.Visible = true;
                txtNomeCampo.Text = campoEditar.Nome;

                popularTipoCampo(nomeCampoSelecionado.ID);
                popularTipoDado(nomeCampoSelecionado.ID);
            }

            var orcamentos = new ManterOrcamentoReembolso().ObterTodos()
                .Select(x => new Dominio.Classes.OrcamentoReembolso
                {
                    ID = x.ID,
                    Ano = x.Ano
                })
                .ToList();

            WebFormHelper.PreencherListaCustomizado(orcamentos, ddlOrcamentoReembolso, "ID", "Ano", false, true);

            if(campoEditar.OrcamentoReembolso != null)
            {
                ddlOrcamentoReembolso.SelectedValue = campoEditar.OrcamentoReembolso.ID.ToString();
            }

            ddlTipoCampo.Items.FindByValue(campoEditar.TipoCampo.ToString()).Selected = true;

            txtTamanhoCampo.Text = campoEditar.Tamanho.ToString();

            hdnLargura.Value = campoEditar.Largura.ToString();
            txtAjuda.Text = campoEditar.Ajuda;
            loadMetaFields(campoEditar);

            switch (campoEditar.TipoCampo)
            {
                //VERIFICA SE O TIPO DE CAMPO É CAMPO DO USUARIO PARA SELECIONAR A OPCAO E ESCONDER CAMPO TIPO DADO.
                case (byte)enumTipoCampo.Field:
                case (byte)enumTipoCampo.FileUpload:
                case (byte)enumTipoCampo.MultipleFileUpload:
                case (byte)enumTipoCampo.Html:
                    if (campoEditar.TipoCampo == (int)enumTipoCampo.Field)
                    {
                        PreencherDDLCamposUsuario();
                        pnlCampoDoUsuario.Visible = true;

                        ddlCampoDoUsuario.ClearSelection();
                        ddlCampoDoUsuario.Items.FindByValue(campoEditar.TipoDado.ToString()).Selected = true;
                    }

                    pnlTipoDado.Visible = false;
                    pnlTamanhoCampo.Visible = false;

                    pnlPermitirNulo.Visible = false;
                    pnlSomatorio.Visible = false;

                    if (campoEditar.TipoCampo == (int)enumTipoCampo.FileUpload || campoEditar.TipoCampo == (int)enumTipoCampo.MultipleFileUpload)
                    {
                        pnlPermitirNulo.Visible = true;
                    }

                    // Se for do tipo HTML muda o label do campo de ajuda
                    if (campoEditar.TipoCampo == (byte)enumTipoCampo.Html)
                    {
                        labelConteudoHtml.Visible = true;
                        ucHelperTooltipAjudaHtml.Visible = true;

                        labelAjuda.Visible = false;
                        ucHelperTooltipAjuda.Visible = false;
                        txtAjuda.Height = 200;
                    }

                    break;
                case (byte)enumTipoCampo.Somatório:
                case (byte)enumTipoCampo.Multiplicador:
                case (byte)enumTipoCampo.Percentual:
                case (byte)enumTipoCampo.Subtracao:

                    pnlSomatorio.Visible = false;
                    pnlTipoDado.Visible = false;
                    pnlTamanhoCampo.Visible = false;
                    txtTamanhoCampo.Text = "100";
                    pnlPermitirNulo.Visible = false;
                    pnlQuestionario.Visible = false;
                    pnlDivisao.Visible = false;

                    var listaTipoDadoSelecionados = listaTipoDado.Where(tipo => tipo.ID == (int)enumTipoDado.Decimal).ToList();
                    WebFormHelper.PreencherListaCustomizado(listaTipoDadoSelecionados, ddlTipoDado, "ID", "Nome", false, false);

                    if (_processo != null)
                    {
                        pnlEtapaCampos.Visible = true;
                        PreencherSelecaoDeEtapas(_etapa, campoEditar);
                    }

                    //ddlEtapa.SelectedValue
                    if (campoEditar.ListaCampoPorcentagem.Any() || campoEditar.ListaCamposVinculados.Any())
                    {
                        var idEtapaSelecionada = 0;

                        var campos = new List<Campo>();

                        if (campoEditar.TipoCampo == (int)enumTipoCampo.Percentual)
                        {
                            idEtapaSelecionada = campoEditar.ListaCampoPorcentagem.FirstOrDefault().CampoRelacionado.Etapa.ID;
                            campos = campoEditar.ListaCampoPorcentagem.Select(x => x.CampoRelacionado).ToList();
                        }
                        else
                        {
                            idEtapaSelecionada = campoEditar.ListaCamposVinculados.FirstOrDefault().Etapa.ID;
                            campos = campoEditar.ListaCamposVinculados.Select(x => x).ToList();
                        }

                        ddlEtapa.SelectedValue = idEtapaSelecionada.ToString();

                        var idsCamposSelecionados = new List<int>();

                        if (campoEditar.TipoCampo == (int)enumTipoCampo.Percentual)
                        {
                            idsCamposSelecionados = campoEditar.ListaCampoPorcentagem.Select(x => x.CampoRelacionado.ID).ToList();
                        }
                        else
                        {
                            idsCamposSelecionados = campoEditar.ListaCamposVinculados.Select(x => x.ID).ToList();
                        }

                        ucListBoxesCamposEtapa.PreencherItens(campos, "ID", "Nome");
                        ucListBoxesCamposEtapa.MarcarComoSelecionados(idsCamposSelecionados);
                    }

                    break;
                case (byte)enumTipoCampo.Divisor:

                    pnlTipoDado.Visible = false;
                    pnlPermitirNulo.Visible = false;
                    pnlAjuda.Visible = false;
                    pnlTamanhoCampo.Visible = false;
                    pnlSomatorio.Visible = false;
                    pnlDivisao.Visible = true;

                    int idEtapa;
                    if (Request["Id"] != null && int.TryParse(Request["Id"], out idEtapa) && chkLSomatorio.Items.Count <= 0)
                    {
                        var campos =
                            new ManterCampo().ObterPorEtapa(idEtapa).Where(x => x.TipoDado == (int)enumTipoDado.Inteiro ||
                                                                                x.TipoDado == (int)enumTipoDado.Moeda ||
                                                                                x.TipoDado == (int)enumTipoDado.Decimal);

                        foreach (var item in campoEditar.ListaCamposVinculados.Where(x => x.CampoDivisor))
                        {
                            ddlCampoDivisor.SelectedValue = item.ID.ToString();
                        }


                        if (!campos.Any())
                        {
                            lblSomatorio.Text = "Não existem campos disponíveis para realizar as operações.";
                            return;
                        }

                        ManterCampo mCampo = new ManterCampo();
                        var campoEdicao = mCampo.ObterPorID(id);

                        // Adiciona campos a lista, seleciona os campos que já existem escolhidos para o somatório da resposta
                        foreach (Campo campo in campos)
                        {
                            ddlCampoDivisor.Items.Add(new ListItem() { Text = campo.Nome, Value = campo.ID.ToString(), Selected = campoEdicao.ListaCamposVinculados.Any(x => x.ID == campo.ID && x.CampoDivisor) });
                            ddlCampoDividendo.Items.Add(new ListItem() { Text = campo.Nome, Value = campo.ID.ToString(), Selected = campoEdicao.ListaCamposVinculados.Any(x => x.ID == campo.ID && !x.CampoDivisor) });
                        }
                    }

                    break;

                case (byte)enumTipoCampo.Questionário:
                    pnlTipoDado.Visible = false;
                    pnlPermitirNulo.Visible = true;
                    pnlAjuda.Visible = true;
                    pnlTamanhoCampo.Visible = false;
                    pnlQuestionario.Visible = true;

                    if (ddlQuestionario.Items.Count <= 0)
                    {
                        PreencherQuestionarios();
                    }

                    WebFormHelper.SetarValorNaCombo(campoEditar.Questionario.ID.ToString(), ddlQuestionario);
                    break;

                default:
                    popularTipoDado(0);
                    ddlTipoDado.ClearSelection();
                    ddlTipoDado.Items.FindByValue(campoEditar.TipoDado.ToString()).Selected = true;
                    pnlPermitirNulo.Visible = true;
                    pnlSomatorio.Visible = false;

                    // Se não for campo do tipo HTML o ckeditor é usado como camp de texto de ajuda
                    labelConteudoHtml.Visible = false;
                    ucHelperTooltipAjudaHtml.Visible = false;
                    labelAjuda.Visible = true;
                    ucHelperTooltipAjuda.Visible = true;
                    txtAjuda.Height = 50;
                    break;
            }

            ddlNomeCampo.ClearSelection();
            if (listaNomeCampo.Where(x => x.Nome == campoEditar.Nome).Any())
            {
                var nomeCampoSelecionado = listaNomeCampo.Where(x => x.Nome == campoEditar.Nome).FirstOrDefault();
                ddlNomeCampo.Items.FindByValue(nomeCampoSelecionado.ID.ToString()).Selected = true;
                txtNomeCampo.Visible = false;

                popularTipoCampo(nomeCampoSelecionado.ID);

                popularTipoDado(nomeCampoSelecionado.ID);
            }
            else
            {
                var nomeCampoSelecionado = listaNomeCampo.Where(x => x.ID == 12).FirstOrDefault();
                ddlNomeCampo.Items.FindByValue(nomeCampoSelecionado.ID.ToString()).Selected = true;
                txtNomeCampo.Visible = true;
                txtNomeCampo.Text = campoEditar.Nome;
            }

            rdbPermitirNulo.Items.FindByValue(campoEditar.PermiteNulo ? "1" : "0").Selected = true;
            rdbPermitirNulo.Items.FindByValue(campoEditar.PermiteNulo ? "0" : "1").Selected = false;

            rdbExibirImpressao.Items.FindByValue(campoEditar.ExibirImpressao ? "1" : "0").Selected = true;
            rdbExibirImpressao.Items.FindByValue(campoEditar.ExibirImpressao ? "0" : "1").Selected = false;

            rdbExibirAjudaImpressao.Items.FindByValue(campoEditar.ExibirAjudaImpressao ? "1" : "0").Selected = true;
            rdbExibirAjudaImpressao.Items.FindByValue(campoEditar.ExibirAjudaImpressao ? "0" : "1").Selected = false;
        }

        public void loadMetaFields(Campo campoEditar)
        {
            // GET META FIELD
            IList<CampoMeta> metaFields = new BMCampoMeta().ObterPorCampo(campoEditar);
            rptMetaFields.DataSource = metaFields;
            rptMetaFields.DataBind();
        }

        protected void ExcluirCampo_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            int id = int.Parse(btn.CommandArgument);

            ExibirModalExcluirCampo(new ManterCampo().ObterPorID(id));
        }

        protected void DuplicarCampo_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            int id = int.Parse(btn.CommandArgument);

            try
            {
                try
                {
                    new ManterCampo().DuplicarObjeto(id);
                }
                catch (Exception)
                {
                    throw new AcademicoException("Erro ao duplicar o registro");
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                return;
            }

            WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Registro duplicado com sucesso!", "EditarEtapa.aspx?Id=" + _etapa.ID);
        }

        protected void AddicionarEtapa_Click(object sender, EventArgs e)
        {
            int idModel = int.Parse(Request["Id"].ToString());
            Response.Redirect("/Cadastros/Etapa/EditarEtapa.aspx?IdProcesso=" + idModel, false);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            var idProcesso = _etapa.Processo != null ? _etapa.Processo.ID : _processo.ID;
            Response.Redirect("/Cadastros/Demanda/EditarDemanda.aspx?Id=" + idProcesso, false);
        }

        private void PreencherRetornoEtapa(int idProcesso, int ordem)
        {
            ddlEtapaRetorno.Items.Clear();

            pnlRetornarSelect.Visible = true;
            int processoID = _etapa.Processo != null ? _etapa.Processo.ID : _processo.ID;
            var listaEtapas = new BMEtapa().ObterOrdemMenoresPorProcessoId(processoID, ordem).OrderBy(d => d.Ordem).ToList();
            int cont = 1;
            ddlEtapaRetorno.ClearSelection();
            listaEtapas.ForEach(d => ddlEtapaRetorno.Items.Add(
                new ListItem()
                {
                    Text = cont++ + "º Etapa :: " + d.Nome,
                    Value = d.ID.ToString(),
                    Selected = d.Processo.ID == _etapa.ID
                })
            );
        }

        private void AlterarVisibilidadePermissoes(bool isPrimeiraEtapa)
        {
            if (isPrimeiraEtapa)
            {
                pnlPermissoesAnalise.Visible = false;
            }
            else
            {
                pnlPermissoesParticipacao.Visible = false;
            }
        }

        protected void rblRequerAtivacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            var rdbAtivo = (RadioButtonList)sender;
            if (rdbAtivo.SelectedValue == "1")
            {
                divContainerNomes.Visible = true;

                var nomes = new ManterNomeFinalizacaoEtapa().ObterTodos();

                nomes.Add(new NomeFinalizacaoEtapa
                {
                    ID = -1,
                    Nome = "- Outros - "
                });

                WebFormHelper.PreencherLista(nomes, ddlNomeFinalizacaoEtapa, false, true);

                var nomesReprovacao = new ManterNomeReprovacaoEtapa().ObterTodos();

                nomesReprovacao.Add(new NomeReprovacaoEtapa
                {
                    ID = -1,
                    Nome = "- Outros - "
                });

                WebFormHelper.PreencherLista(nomesReprovacao, ddlNomeReprovacaoEtapa, false, true);
            }
            else
            {
                pnlRetornarSelect.Visible =
                divContainerNomes.Visible = false;
            }
        }

        protected void btnCancelarAlternativa_Click(object sender, EventArgs e)
        {
            txtNomeAlternativa.Text = "";
            base.OcultarBackDrop();
            pnlModalAlternativas.Visible = false;

        }

        protected override enumFuncionalidade Funcionalidade
        {
            get { return enumFuncionalidade.Etapa; }
        }

        protected override System.Collections.Generic.IList<enumPerfil> PerfisNecessariosParaAcessarAFuncionalidade
        {
            get { throw new NotImplementedException(); }
        }

        protected void AddicionarCampo_Click(object sender, EventArgs e)
        {
            try
            {
                SalvarEtapa();

                ExibirBackDrop();
                pnlCampoModal.Visible = true;

                var orcamentos = new ManterOrcamentoReembolso().ObterTodos()
                .Select(x => new Dominio.Classes.OrcamentoReembolso
                {
                    ID = x.ID,
                    Ano = x.Ano
                })
                .ToList();

                WebFormHelper.PreencherListaCustomizado(orcamentos, ddlOrcamentoReembolso, "ID", "Ano", false, true);

                PreencheMetaFields();
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao,
                    $"Corrija o seguinte erro antes de alterar os campos: {ex.Message}");
            }
        }

        protected void OcultarCampoModal_Click(object sender, EventArgs e)
        {
            txtNomeCampo.Text = "";
            txtTamanhoCampo.Text = "";
            ddlTipoCampo.ClearSelection();
            ddlTipoDado.ClearSelection();
            rdbPermitirNulo.Items.FindByValue("1").Selected = true;
            rdbExibirImpressao.Items.FindByValue("1").Selected = true;
            rdbExibirAjudaImpressao.Items.FindByValue("1").Selected = true;

            CampoEdicaoId.Value = "";

            OcultarBackDrop();
            pnlCampoModal.Visible = false;

            pnlTipoDado.Visible = true;
            pnlCampoDoUsuario.Visible = false;
            pnlTamanhoCampo.Visible = true;
            txtTamanhoCampo.Text = "";
            pnlPermitirNulo.Visible = true;

            txtAjuda.Text = "";

            pnlSomatorio.Visible = false;
        }

        protected void rptAlternativa_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                dynamic item = e.Item.DataItem as dynamic;
                ((LinkButton)e.Item.FindControl("Editar")).CommandArgument = (e.Item.DataItem as classes.Alternativa).ID.ToString();
                ((LinkButton)e.Item.FindControl("Excluir")).CommandArgument = (e.Item.DataItem as classes.Alternativa).ID.ToString();
            }
        }

        protected void OcultarAlternativaModal_Click(object sender, EventArgs e)
        {
            txtNomeAlternativa.Text = _etapa.Nome;
            hdnAlternativaId.Value = "";
            txtNomeAlternativaCampo.Text = "";

            base.OcultarBackDrop();
            pnlModalAlternativas.Visible = false;
        }

        protected void btnAbrirAlternativas_Click(object sender, EventArgs e)
        {
            ltAlternativaAcao.Text = "Adicionar Alternativa";
            btnSalvarAlternativa.Text = "Adicionar Alternativa";
            hdnAlternativaIdEdicao.Value = "0";
            LinkButton btn = (LinkButton)(sender);
            int idCampo = int.Parse(btn.CommandArgument);
            hdnCampoAtivo.Value = idCampo.ToString();
            Campo campo = new BMCampo().ObterPorId(idCampo);

            txtNomeAlternativaCampo.Text = (campo.Ordem + 1) + "º " + campo.Nome;
            BMAlternativa bmAlternativa = new BMAlternativa();
            List<Alternativa> listaAlternativas = bmAlternativa.ObterPorCampoId(idCampo).ToList();

            rptAlternativas.DataSource = listaAlternativas;
            rptAlternativas.DataBind();

            base.ExibirBackDrop();
            pnlModalAlternativas.Visible = true;
        }

        protected void EditarAlternativa_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            int idAlternativa = int.Parse(btn.CommandArgument);

            int idCampo = int.Parse(hdnCampoAtivo.Value);

            ManterAlternativa bmAlternativa = new ManterAlternativa();
            Alternativa alternativa = bmAlternativa.ObterPorID(idAlternativa);
            txtNomeAlternativa.Text = alternativa.Nome;
            hdnAlternativaIdEdicao.Value = alternativa.ID.ToString();

            if (alternativa.CampoVinculado != null)
            {
                int idCampoVinculado;
                if (int.TryParse(alternativa.CampoVinculado.ID.ToString(), out idCampoVinculado) && alternativa.CampoVinculado.ID > 0)
                {
                    try
                    {
                        ddlCampoVinculado.SelectedValue = alternativa.CampoVinculado.ID.ToString();
                    }
                    catch
                    {
                        ddlCampoVinculado.ClearSelection();
                    }
                }
            }
            else
            {
                ddlCampoVinculado.ClearSelection();
            }

            ltAlternativaAcao.Text = "Editar Alternativa";
            btnSalvarAlternativa.Text = "Salvar Alternativa";
        }

        protected void SalvarAlternativa_Click(object sender, EventArgs e)
        {
            try
            {
                int idCampo = int.Parse(hdnCampoAtivo.Value);

                Alternativa alternativa = new Alternativa();
                alternativa.ID = int.Parse(hdnAlternativaIdEdicao.Value);
                alternativa.Nome = txtNomeAlternativa.Text;
                alternativa.Campo = new Campo()
                {
                    ID = idCampo
                };

                int idCampoVinculado;
                if (int.TryParse(ddlCampoVinculado.SelectedValue, out idCampoVinculado))
                {
                    alternativa.CampoVinculado = new ManterCampo().ObterPorID(idCampoVinculado);
                }

                ManterAlternativa bmAlternativa = new ManterAlternativa();

                bmAlternativa.Incluir(alternativa);
                txtNomeAlternativa.Text = "";

                List<Alternativa> listaAlternativas = bmAlternativa.ObterPorCampoId(idCampo).ToList();
                rptAlternativas.DataSource = listaAlternativas;
                rptAlternativas.DataBind();

                ddlCampoVinculado.SelectedValue = "0";
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }

            WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso!");
        }

        protected void ExcluirAlternativa_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            int idAlternativa = int.Parse(btn.CommandArgument);

            ManterAlternativa bmAlternativa = new ManterAlternativa();

            try
            {
                try
                {
                    bmAlternativa.Excluir(idAlternativa);
                }
                catch (Exception)
                {
                    throw new AcademicoException("Não é possível excluir pois há outros dados dependentes deste registro");
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                return;
            }

            ltAlternativaAcao.Text = "Adicionar Alternativa";
            btnSalvarAlternativa.Text = "Adicionar Alternativa";
            hdnAlternativaIdEdicao.Value = "0";

            txtNomeAlternativa.Text = "";

            List<Alternativa> listaAlternativas = bmAlternativa.ObterPorCampoId(int.Parse(hdnCampoAtivo.Value)).ToList();
            rptAlternativas.DataSource = listaAlternativas;
            rptAlternativas.DataBind();

            WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Excluídos com Sucesso!");
        }

        protected void btnSalvarCampo_OnClick(object sender, EventArgs e)
        {
            try
            {
                var manterCampo = new ManterCampo();

                int idCampo;
                var isCampo = int.TryParse(CampoEdicaoId.Value, out idCampo);

                var campo = (isCampo ? manterCampo.ObterPorID(idCampo) : null) ?? new Campo();

                int idNomeCampoSelecionado = int.Parse(ddlNomeCampo.SelectedValue);
                if (idNomeCampoSelecionado != 12)
                {
                    campo.Nome = listaNomeCampo.Where(x => x.ID == idNomeCampoSelecionado).Select(y => y.Nome).FirstOrDefault(); ;
                }
                else
                {
                    campo.Nome = txtNomeCampo.Text;
                }

                int tamanho;

                int.TryParse(txtTamanhoCampo.Text, out tamanho);
                campo.Tamanho = int.Parse(ddlTipoCampo.SelectedValue) == (int)enumTipoCampo.Somatório || int.Parse(ddlTipoCampo.SelectedValue) == (int)enumTipoCampo.FileUpload || int.Parse(ddlTipoCampo.SelectedValue) == (int)enumTipoCampo.MultipleFileUpload ? int.MaxValue : tamanho;

                campo.TipoCampo = byte.Parse(ddlTipoCampo.SelectedValue);
                campo.TipoDado = byte.Parse(ddlTipoDado.SelectedValue);
                campo.PermiteNulo = rdbPermitirNulo.SelectedValue == "1";
                campo.ExibirImpressao = rdbExibirImpressao.SelectedValue == "1";
                campo.ExibirAjudaImpressao = rdbExibirAjudaImpressao.SelectedValue == "1";
                campo.Ajuda = txtAjuda.Text;
                
                if(string.IsNullOrWhiteSpace(ddlOrcamentoReembolso.Text) == false)
                {
                    campo.OrcamentoReembolso = new ManterOrcamentoReembolso().ObterPorId(int.Parse(ddlOrcamentoReembolso.Text));
                }

                if (campo.TipoCampo == (int)enumTipoCampo.Percentual)
                {
                    var campos = ucListBoxesCamposEtapa.RecuperarIdsSelecionados().Select(int.Parse);
                    var camposExcluidos = campo.ListaCampoPorcentagem.Where(x => !campos.Contains(x.CampoRelacionado.ID)).ToList();
                    foreach (var campoExcluido in camposExcluidos)
                    {
                        campo.ListaCampoPorcentagem.Remove(campoExcluido);
                    }

                    foreach (var idCampoRelacionado in campos)
                    {
                        if (!campo.ListaCampoPorcentagem.Any(x => x.CampoRelacionado.ID == idCampoRelacionado))
                        {
                            campo.ListaCampoPorcentagem.Add(new CampoPorcentagem()
                            {
                                Campo = campo,
                                CampoRelacionado = new Campo() { ID = idCampoRelacionado },
                                UltimaAtualizacao = DateTime.Now
                            });
                        }
                    }
                }

                int largura;
                int.TryParse(hdnLargura.Value, out largura);
                campo.Largura = largura == 0 ? 12 : largura;

                //SE O TIPO DE CAMPO FOR >>CAMPO DO USUARIO<< USAR TIPO DADO PARA GUARDAR O CAMPO DO USUARIO SELECIONADO
                if (ddlTipoCampo.SelectedValue == ((int)enumTipoCampo.Field).ToString())
                {
                    campo.TipoDado = byte.Parse(ddlCampoDoUsuario.SelectedValue);
                }

                if (!string.IsNullOrEmpty(ddlQuestionario.SelectedValue))
                {
                    int idQuestionario = int.Parse(ddlQuestionario.SelectedValue);
                    campo.Questionario = new ManterQuestionario().ObterQuestionarioPorID(idQuestionario);
                }

                if (CampoEdicaoId.Value != "")
                {
                    AtualizarCamposVinculados(campo);
                    manterCampo.Alterar(campo);
                }
                else
                {
                    PreencherCamposVinculados(campo);
                    campo.Etapa = _etapa;
                    manterCampo.Incluir(campo);
                }

                // Meta Fields - Campos customizados via banco
                var bmCampoMeta = new BMCampoMeta();
                var bmCampoMetaValue = new BMCampoMetaValue();

                foreach (RepeaterItem item in rptMetaFields.Items)
                {
                    // Campo com o valor a ser armazenado/atualizado
                    var inputMetaField = (TextBox)item.FindControl("metaField");

                    // CampoMeta ID
                    var hidMetaFieldId = (HiddenField)item.FindControl("metaFieldId");

                    // CampoMetaValue ID - Se for diferente de nulo está sendo atualizado
                    var hidMetaFieldValueId = (HiddenField)item.FindControl("metaFieldValueId");

                    var campoMeta = bmCampoMeta.ObterPorId(int.Parse(hidMetaFieldId.Value));

                    // Só não grava se for um novo valor e estiver vazio - segue para o próximo
                    if (hidMetaFieldValueId.Value == "" && inputMetaField.Text == "")
                    {
                        continue;
                    }

                    CampoMetaValue campoMetaValue;

                    // Atualizando valor de campo
                    campoMetaValue = hidMetaFieldValueId.Value != "" ? bmCampoMetaValue.ObterPorId(int.Parse(hidMetaFieldValueId.Value)) : new CampoMetaValue { Campo = campo, CampoMeta = campoMeta };

                    // Atualizar o valor
                    campoMetaValue.MetaValue = inputMetaField.Text;

                    bmCampoMetaValue.Salvar(campoMetaValue);
                }

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso!", "EditarEtapa.aspx?Id=" + _etapa.ID);
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private void PreencherCamposVinculados(Campo campo)
        {
            ManterCampo mCampo = new ManterCampo();

            try
            {
                List<Campo> campos = new List<Campo>();
                var mCampos = new ManterCampo();

                if (campo.TipoCampo == (int)enumTipoCampo.Divisor)
                {
                    // Adicionando campo base de divisao.
                    var campoEdicao = mCampos.ObterPorID(int.Parse(ddlCampoDivisor.SelectedItem.Value));
                    campoEdicao.CampoDivisor = true;
                    campos.Add(campoEdicao);

                    campoEdicao = mCampos.ObterPorID(int.Parse(ddlCampoDividendo.SelectedItem.Value));
                    campos.Add(campoEdicao);
                }
                else
                {
                    foreach (var id in ucListBoxesCamposEtapa.RecuperarIdsSelecionados())
                    {
                        campos.Add(mCampo.ObterPorID(int.Parse(id)));
                    }
                }

                mCampo.AdicionarCamposVinculados(campos, campo);
            }
            catch
            {
                throw new Exception("Não foi possível adicionar o campo ");
            }
        }

        private void AtualizarCamposVinculados(Campo campo)
        {
            if (campo.ListaCamposVinculados.Count() >= 0)
            {
                var mCampos = new ManterCampo();

                List<Campo> listaCampos = new List<Campo>();

                if (campo.TipoCampo == (int)enumTipoCampo.Divisor)
                {
                    // Adicionando campo base de divisao.
                    var campoEdicao = mCampos.ObterPorID(int.Parse(ddlCampoDivisor.SelectedItem.Value));
                    campoEdicao.CampoDivisor = true;
                    listaCampos.Add(campoEdicao);

                    campoEdicao = mCampos.ObterPorID(int.Parse(ddlCampoDividendo.SelectedItem.Value));
                    listaCampos.Add(campoEdicao);
                }
                else
                {
                    foreach (var id in ucListBoxesCamposEtapa.RecuperarIdsSelecionados())
                    {
                        listaCampos.Add(mCampos.ObterPorID(int.Parse(id)));
                    }
                }

                // Remove o que tinha anteriormente para salvar a nova realação
                mCampos.RemoverTodosCamposVinculados(campo);

                // Adiciona campos selecionados na edição
                mCampos.AdicionarCamposVinculados(listaCampos, campo);
            }
        }

        protected void btnCancelarCampo_OnClick(object sender, EventArgs e)
        {

        }

        protected void btnSalvarOrdenacao_Click(object sender, EventArgs e)
        {
            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new[] { new DynamicJsonConverter() });
            dynamic obj = serializer.Deserialize(hdnOrdenacaoAlternativas.Value, typeof(object));

            if (obj != null)
            {
                new ManterAlternativa().AlterarOrdemCampos(obj);

                List<Alternativa> listaAlternativas = new BMAlternativa().ObterPorCampoId(int.Parse(hdnCampoAtivo.Value)).OrderBy(d => d.Ordem).ToList();
                rptAlternativas.DataSource = listaAlternativas;
                rptAlternativas.DataBind();

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Excluídos com Sucesso!");
            }
        }

        private void PreencherFormulariosPermissoes(IEnumerable<EtapaPermissao> permissoes)
        {
            //Permissões de Análise
            var permissoesAnalise = permissoes.Where(p => p.Analisar == true);
            if (permissoesAnalise.Any(p => p.ChefeImediato == true))
                rblTipoPermissaoAnalise.SelectedValue = "ChefeImediato";

            else if (permissoesAnalise.Any(p => p.DiretorCorrespondente == true))
                rblTipoPermissaoAnalise.SelectedValue = "DiretorCorrespondente";

            else if (permissoesAnalise.Any(p => p.Solicitante == true))
                rblTipoPermissaoAnalise.SelectedValue = "Solicitante";

            else
            {
                var usuariosAnalise = permissoesAnalise.Where(p => p.Usuario != null)
                    .Select(p => new Usuario() { ID = p.Usuario.ID, Nome = p.Usuario.Nome, CPF = p.Usuario.CPF }).ToList();

                if (usuariosAnalise.Count > 0)
                {
                    ucMultiplosUsuariosAnalise.PreencherGridUsuarios(usuariosAnalise);
                    rblTipoPermissaoAnalise.SelectedValue = "Colaborador";
                }
            }

            var permissoesNotificacao = permissoes.Where(p => p.Notificar == true);

            if (permissoesNotificacao.Any(p => p.Usuario != null))
                cblTipoPermissaoNotificacao.Items[0].Selected = true;

            if (permissoesNotificacao.Any(p => p.Solicitante == true))
                cblTipoPermissaoNotificacao.Items[1].Selected = true;

            if (permissoesNotificacao.Any(p => p.ChefeImediato == true))
                cblTipoPermissaoNotificacao.Items[2].Selected = true;

            if (permissoesNotificacao.Any(p => p.DiretorCorrespondente == true))
                cblTipoPermissaoNotificacao.Items[3].Selected = true;

            var usuariosNotificacao = permissoesNotificacao.Where(p => p.Usuario != null)
                    .Select(p => new Usuario() { ID = p.Usuario.ID, Nome = p.Usuario.Nome, CPF = p.Usuario.CPF }).ToList();

            ucMultiplosUsuariosNotificacao.PreencherGridUsuarios(usuariosNotificacao);

            //Permissões de Participação
            var permissoesPerfil = permissoes.Where(p => p.Perfil != null).Select(p => new Perfil() { ID = p.Perfil.ID }).ToList();
            var permissoesOcupacao = permissoes.Where(p => p.NivelOcupacional != null).Select(p => new NivelOcupacional() { ID = p.NivelOcupacional.ID }).ToList();
            var permissoesUF = permissoes.Where(p => p.Uf != null).Select(p => new Uf() { ID = p.Uf.ID }).ToList();

            ucPermissoes.PreencherListBoxComPerfisGravadosNoBanco(permissoesPerfil);
            ucPermissoes.PreencherListBoxComNiveisOcupacionaisGravadosNoBancoForGestor(permissoesOcupacao);
            ucPermissoes.PreencherListBoxComUfsGravadasNoBanco(permissoesUF);
        }

        private List<EtapaPermissao> RecuperarPermissoes()
        {
            var permissoes = new List<EtapaPermissao>();

            switch (rblTipoPermissaoAnalise.SelectedValue)
            {
                case "Colaborador":

                    if (pnlPermissoesAnalise.Visible)
                    {
                        if (!recuperarIdsUsuariosPermissao(ucMultiplosUsuariosAnalise).Any())
                        {
                            throw new AcademicoException("Voce deve selecionar pelo menos um usuário na opção \"Colaboradores\" ");
                        }

                        foreach (var id in recuperarIdsUsuariosPermissao(ucMultiplosUsuariosAnalise))
                        {
                            permissoes.Add(new EtapaPermissao()
                            {
                                Analisar = true,
                                Usuario = new Usuario() { ID = id }
                            });
                        }
                    }
                    break;
                case "ChefeImediato":
                    permissoes.Add(new EtapaPermissao()
                    {
                        Analisar = true,
                        ChefeImediato = true
                    });
                    permissoes.Add(new EtapaPermissao()
                    {
                        Analisar = true,
                        GerenteAdjunto = true
                    });
                    break;
                case "DiretorCorrespondente":
                    permissoes.Add(new EtapaPermissao()
                    {
                        Analisar = true,
                        DiretorCorrespondente = true
                    });
                    break;
                case "Solicitante":
                    permissoes.Add(new EtapaPermissao()
                    {
                        Analisar = true,
                        Solicitante = true
                    });
                    break;
            }


            if (cblTipoPermissaoNotificacao.Items[0].Selected)
            {
                var listaIds = recuperarIdsUsuariosPermissao(ucMultiplosUsuariosNotificacao);

                foreach (var id in listaIds)
                {
                    permissoes.Add(new EtapaPermissao()
                    {
                        Notificar = true,
                        Usuario = new Usuario() { ID = id }
                    });
                }

                if (!listaIds.Any())
                {
                    throw new AcademicoException("Pelo menos um colaborador deve ser escolhido para receber notificação.");
                }
            }

            if (cblTipoPermissaoNotificacao.Items[1].Selected)
            {
                permissoes.Add(new EtapaPermissao()
                {
                    Notificar = true,
                    Solicitante = true
                });
            }

            if (cblTipoPermissaoNotificacao.Items[2].Selected)
            {
                permissoes.Add(new EtapaPermissao()
                {
                    Notificar = true,
                    ChefeImediato = true
                });
                permissoes.Add(new EtapaPermissao()
                {
                    Notificar = true,
                    GerenteAdjunto = true
                });
            }

            if (cblTipoPermissaoNotificacao.Items[3].Selected)
            {
                permissoes.Add(new EtapaPermissao()
                {
                    Notificar = true,
                    DiretorCorrespondente = true
                });
            }

            permissoes.AddRange(recuperarPermissoesParticipacao());

            return permissoes;
        }

        private IEnumerable<int> recuperarIdsUsuariosPermissao(ucLupaMultiplosUsuarios ucMultiplosUsuarios)
        {
            var gridView = (GridView)ucMultiplosUsuarios.FindControl("GridViewUsuariosSelecionados");

            foreach (GridViewRow row in gridView.Rows)
            {
                if (row.RowType != DataControlRowType.DataRow)
                    continue;

                CheckBox chkRow = (row.Cells[0].FindControl("chkRow") as CheckBox);

                if (!chkRow.Checked)
                    continue;

                yield return Convert.ToInt32(gridView.DataKeys[row.RowIndex].Value);
            }
        }

        private IList<EtapaPermissao> recuperarPermissoesParticipacao()
        {
            var permissoes = new List<EtapaPermissao>();

            CheckBoxList ckblstPerfil = (CheckBoxList)this.ucPermissoes.FindControl("ckblstPerfil");
            int[] perfisSelecionados = WebFormHelper.ObterValoresSelecionadosCheckBoxList(ckblstPerfil);

            CheckBoxList ckblstNivelOcupacional = (CheckBoxList)this.ucPermissoes.FindControl("ckblstNivelOcupacional");
            int[] niveisOcupacionaisSelecionados = WebFormHelper.ObterValoresSelecionadosCheckBoxList(ckblstNivelOcupacional);

            Repeater rptUFs = (Repeater)ucPermissoes.FindControl("rptUFs");
            int[] ufsSelecionadas = WebFormHelper.ObterValoresSelecionadosRepeaterCheckBox(rptUFs, "ckUf", "ID_UF");

            foreach (var id in perfisSelecionados)
            {
                permissoes.Add(new EtapaPermissao() { Perfil = new Perfil() { ID = id } });
            }

            foreach (var id in niveisOcupacionaisSelecionados)
            {
                permissoes.Add(new EtapaPermissao() { NivelOcupacional = new NivelOcupacional() { ID = id } });
            }

            foreach (var id in ufsSelecionadas)
            {
                permissoes.Add(new EtapaPermissao() { Uf = new Uf() { ID = id } });
            }

            return permissoes;
        }

        protected void ddlNomeFinalizacaoEtapa_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            divNomeFinalizacaoEtapaOutros.Visible = ddlNomeFinalizacaoEtapa.SelectedValue == "-1";

            txtNomeFinalizacaoOutro.Text = "";
        }

        protected void ddlNomeReprovacaoEtapa_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            divNomeReprovacaoEtapaOutros.Visible = ddlNomeReprovacaoEtapa.SelectedValue == "-1";

            txtNomeReprovacaoOutro.Text = "";
        }

        protected void rdbExibirImpressao_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            divExibirAjudaNaImpressao.Visible = rdbExibirImpressao.SelectedValue != "0";
        }

        private void ExibirModalExcluirCampo(Campo campo)
        {
            ltrExcluirCampo_Nome.Text = campo.Nome;

            hdnExcluirCampo_IdCampo.Value = campo.ID.ToString();

            // DADOS
            ltrExcluirCampo_Tipo.Text = ((enumTipoCampo)campo.TipoCampo).GetDescription();
            ltrExcluirCampo_Obrigatorio.Text = campo.PermiteNulo ? "Não" : "Sim";
            ltrExcluirCampo_Tamanho.Text = campo.Tamanho.ToString();
            ltrExcluirCampo_Largura.Text = ((campo.Largura * 100) / 12).ToString();

            var respostas = new ManterCampoResposta().ObterRespostas(campo);

            // DEPENDÊNCIAS
            ltrExcluirCampo_ContadorDependencias.Text = respostas.Count().ToString();

            gdvExcluirCampo_Dependencias.DataSource = respostas;
            gdvExcluirCampo_Dependencias.DataBind();

            ExibirBackDrop();
            pnlModalExcluirCampo.Visible = true;
        }

        private void EsconderModalExcluirCampo()
        {
            LimparModalExcluirCampo();
            OcultarBackDrop();
            pnlModalExcluirCampo.Visible = false;
        }

        private void LimparModalExcluirCampo()
        {
            btnRemoverCampo.Enabled =
            ckbConfirmacaoExclusaoCampo.Checked = false;

            hdnExcluirCampo_IdCampo.Value = "0";
        }

        protected void btnFecharModalExcluirCampo_OnServerClick(object sender, EventArgs e)
        {
            EsconderModalExcluirCampo();
        }

        protected void ckbConfirmacaoExclusaoCampo_OnCheckedChanged(object sender, EventArgs e)
        {
            btnRemoverCampo.Enabled = ckbConfirmacaoExclusaoCampo.Checked;
        }

        protected void btnRemoverCampo_OnClick(object sender, EventArgs e)
        {
            if (!ckbConfirmacaoExclusaoCampo.Checked)
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro,
                    "Para remover este campo, você deve primeiramente marcar a opção de confirmação de exclusão.");
            else
            {
                try
                {
                    var manterCampo = new ManterCampo();

                    var campo = manterCampo.ObterPorID(int.Parse(hdnExcluirCampo_IdCampo.Value));

                    if (campo == null)
                    {
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro,
                            "Campo inválido ou não existente. Tente novamente.");
                        return;
                    }

                    manterCampo.Excluir(campo);

                    EsconderModalExcluirCampo();

                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Campo removido com sucesso.");

                    if (_etapa == null)
                    {
                        int idModel = int.Parse(Request["Id"]);
                        _etapa = manterEtapa.ObterPorID(idModel);
                    }

                    // Atualiza os campos.
                    rptCampos.DataSource = new ManterCampo().ObterPorEtapa(_etapa.ID).OrderBy(d => d.Ordem).ToList();
                    rptCampos.DataBind();
                }
                catch (Exception ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro,
                        "Erro ao remover campo. Tente novamente. <p>Mensagem técnica do erro: " + ex.Message + "</p>");
                }
            }
        }

        protected void rblBotaoAjuste_SelectedIndexChanged(object sender, EventArgs e)
        {
            var rdbAtivo = (RadioButtonList)sender;
            if (rdbAtivo.SelectedValue == "1")
            {
                divContainerAjuste.Visible = true;
                WebFormHelper.PreencherListaCustomizado(typeof(enumNomeAjusteDemanda), ddlNomeBotaoAjuste);

                if (_etapa.Processo != null)
                {
                    PreencherRetornoEtapa(_etapa.Processo.ID, _etapa.Ordem);
                }
                else
                {
                    classes.Etapa ultimaEtapa = _processo.ListaEtapas.OrderByDescending(d => d.Ordem).FirstOrDefault();
                    PreencherRetornoEtapa(_processo.ID, ultimaEtapa != null ? ultimaEtapa.Ordem + 1 : 0);
                }
            }
            else
            {
                divContainerAjuste.Visible = false;
                pnlRetornarSelect.Visible = false;
            }
        }

        protected void ddlNomeBotaoAjuste_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)(sender);

            if (ddl.SelectedValue == "-1")
            {
                AjusteOutro.Visible = true;
            }
            else
            {
                AjusteOutro.Visible = false;
            }
        }

        protected void rblTeraReprovacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonList ddl = (RadioButtonList)(sender);

            if (ddl.SelectedValue == "1")
            {
                pnlBotaoReprovacao.Visible = true;
            }
            else
            {
                pnlBotaoReprovacao.Visible = false;
            }
        }
    }
}