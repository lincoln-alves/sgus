using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.Dominio.Classes;
using classes = Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.BP;
using Sebrae.Academico.InfraEstrutura.Core.Extensions.Others;

namespace Sebrae.Academico.WebForms.Cadastros.TrilhaNivel
{
    public partial class EdicaoTrilhaNivel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;

            PreencherCampos();

            if (Request["Id"] != null)
            {
                int idNivel;
                if (int.TryParse(Request["Id"], out idNivel))
                {
                    var nivel = new ManterTrilhaNivel().ObterTrilhaNivelPorID(idNivel);
                    CarregarDadosDoCadastroDeTrilhaNivel(nivel);
                    ExibirEdicao(true);
                }
                else
                {
                    throw new AcademicoException("Não foi possível editar o nível selecionado");
                }
            }
            else
            {
                ExibirEdicao(false);
            }
        }

        private void PreencherCampos()
        {
            ucLupaUsuarioMonitor.Label = "Monitor";

            ViewState["_Trilhas"] = Helpers.Util.ObterListaAutocomplete(new ManterTrilha().ObterTodasTrilhasIQueryable());

            PreencherComboMapa();
            ucPermissoesNivel.PreencherListas();
            PreencherComboTemplateCertificado();
            PreencherComboTermoAceite();
            PreencherComboOrdem(null);
            WebFormHelper.PreencherComponenteComOpcoesSimNao(ddlAceitaNovasMatriculas, false);

            var url = new ManterConfiguracaoSistema().ObterConfiguracaoSistemaPorID((int)enumConfiguracaoSistema.MapaPreviewURL);
            if (url != null && !string.IsNullOrEmpty(url.Registro))
            {
                hdMapUrl.Value = url.Registro;
            }
        }

        private void CarregarDadosDoCadastroDeTrilhaNivel(classes.TrilhaNivel nivel)
        {
            txtTrilha.Text = nivel.Trilha.ID.ToString();
            txtTrilha.Enabled = false;

            txtNomeNivel.Text = nivel.Nome;

            PreencherComboOrdem(nivel.Trilha);

            //Aceita novas matriculas
            if (nivel.AceitaNovasMatriculas.HasValue)
            {
                ddlAceitaNovasMatriculas.ClearSelection();
                if (nivel.AceitaNovasMatriculas.Value)
                    WebFormHelper.SetarValorNaCombo("S", ddlAceitaNovasMatriculas);
                else
                    WebFormHelper.SetarValorNaCombo("N", ddlAceitaNovasMatriculas);
            }
            else
            {
                ddlAceitaNovasMatriculas.ClearSelection();
            }

            // Preencher lista de questionários.
            PreencherComboQuestionario(nivel);

            //Questionarios
            if (nivel.ListaQuestionarioAssociacao != null && nivel.ListaQuestionarioAssociacao.Count > 0)
            {
                if (nivel.ListaQuestionarioAssociacao.Any(
                        x =>
                            x.TipoQuestionarioAssociacao != null &&
                            x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pre &&
                            x.Questionario != null))
                {
                    var associacaoPre = nivel.ListaQuestionarioAssociacao.FirstOrDefault(
                        x =>
                            x.TipoQuestionarioAssociacao != null &&
                            x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pre &&
                            x.Questionario != null);

                    //if (associacaoPre != null)
                    //    WebFormHelper.SetarValorNaCombo(
                    //        associacaoPre
                    //            .Questionario.ID.ToString(), ddlQuestionarioPre);
                }

                if (nivel.ListaQuestionarioAssociacao.Any(
                        x =>
                            x.TipoQuestionarioAssociacao != null &&
                            x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pos &&
                            x.Questionario != null))
                {
                    var associacaoPos = nivel.ListaQuestionarioAssociacao.FirstOrDefault(
                        x =>
                            x.TipoQuestionarioAssociacao != null &&
                            x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pos &&
                            x.Questionario != null);

                    if (associacaoPos != null)
                        WebFormHelper.SetarValorNaCombo(
                            associacaoPos
                                .Questionario.ID.ToString(), ddlQuestionarioPos);
                }

                if (nivel.ListaQuestionarioAssociacao.Any(
                        x =>
                            x.TipoQuestionarioAssociacao != null &&
                            x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Prova &&
                            x.Questionario != null))
                {
                    var associacaoProva = nivel.ListaQuestionarioAssociacao.FirstOrDefault(
                        x =>
                            x.TipoQuestionarioAssociacao != null &&
                            x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Prova &&
                            x.Questionario != null);

                    if (associacaoProva != null)
                        WebFormHelper.SetarValorNaCombo(
                            associacaoProva
                                .Questionario.ID.ToString(), ddlQuestionarioProva);
                }
            }

            //Certificado
            if (nivel.CertificadoTemplate != null && nivel.CertificadoTemplate.ID > 0)
            {
                ddlCertificadoTemplate.SelectedValue = nivel.CertificadoTemplate.ID.ToString();
            }

            //Pré-Requisito
            IList<classes.TrilhaNivel> trilhaNivelfiltrada = nivel.Trilha.ListaTrilhaNivel.Where(x => x.Nome != nivel.Nome).ToList();
            WebFormHelper.PreencherLista(trilhaNivelfiltrada, ddlPreRequisito, false, true);
            if (nivel.PreRequisito != null)
                WebFormHelper.SetarValorNaComboPorTexto(nivel.PreRequisito.Nome, ddlPreRequisito);

            //Prazo
            txtPrazo.Text = nivel.QuantidadeDiasPrazo.ToString();

            //Porcentagens dos Troféus
            txtPorcentagensTrofeus.Value = !string.IsNullOrWhiteSpace(nivel.PorcentagensTrofeus)
                ? nivel.PorcentagensTrofeus
                : "33,66";

            //Carga Horaria
            txtCargaHoraria.Text = nivel.CargaHoraria.ToString();

            //Termo
            //txtTextoTermoDeAceite.Text = registro.TermoAceite;
            if (nivel.TermoAceite != null)
            {
                ddlTermoAceite.SelectedValue = nivel.TermoAceite.ID.ToString();
            }

            //Mapa
            ddlTipoMapa.SelectedValue = ((int)nivel.Mapa).ToString();

            exibePreviewMapa(Convert.ToInt32(ddlTipoMapa.SelectedValue), nivel.ID);

            //Descrição
            txtDescricaoTrilhaNivel.Text = nivel.Descricao;

            //Nota Minima
            txtValorNotaMinima.Text = nivel.NotaMinima.HasValue ? nivel.NotaMinima.Value.ToString() : "";

            //Monitor
            if (nivel.Monitor != null)
                ucLupaUsuarioMonitor.SelectedUser = nivel.Monitor;

            ddlOrdem.SelectedValue = nivel.ValorOrdem.ToString();
            Console.Write(ddlOrdem);

            //Limite dias cancelamento
            txtLimiteCancelamento.Text = nivel.LimiteCancelamento.ToString();

            if (nivel.QuantidadeMoedasProvaFinal != null)
            {
                txtQuantidadeMoedasProvaFinal.Text = nivel.QuantidadeMoedasProvaFinal.ToString();
            }

            if (nivel.QuantidadeMoedasPorCurtida != null)
            {
                txtQuantidadeMoedasPorCurtida.Text = nivel.QuantidadeMoedasPorCurtida.ToString();
            }

            if (nivel.QuantidadeMoedasPorDescurtida != null)
            {
                txtQuantidadeMoedasPorDescurtida.Text = nivel.QuantidadeMoedasPorDescurtida.ToString();
            }

            if (nivel.ValorPrataPorOuro != null && nivel.ValorPrataPorOuro > 0)
            {
                txtCambioMoedas.Text = nivel.ValorPrataPorOuro.ToString();
            }

            PreencherPermissoes(nivel.Trilha);
        }

        private void PreencherPermissoes(classes.Trilha trilha)
        {
            ucPermissoesNivel.PreencherListBoxComNiveisOcupacionaisGravadosNoBanco(trilha.ListaPermissao.Where(x => x.NivelOcupacional != null).Select(x => x.NivelOcupacional).ToList(), true);
            ucPermissoesNivel.PreencherListBoxComPerfisGravadosNoBanco(trilha.ListaPermissao.Where(x => x.Perfil != null).Select(x => x.Perfil).ToList(), false, true);
            ucPermissoesNivel.PreencherListBoxComUfsGravadasNoBanco(trilha.ListaPermissao.Where(x => x.Uf != null).Select(x => x.Uf).ToList(), false, true);
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

                //WebFormHelper.PreencherLista(listaQuestionariosDePre.OrderBy(x => x.Nome).ToList(), ddlQuestionarioPre, false, true);
                WebFormHelper.PreencherLista(listaQuestionariosDePos.OrderBy(x => x.Nome).ToList(), ddlQuestionarioPos, false, true);
                WebFormHelper.PreencherLista(listaQuestionariosDeAvaliacaoProva.OrderBy(x => x.Nome).ToList(), ddlQuestionarioProva, false, true);

            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        protected void exibePreviewMapa(int mapaId, int? nivelId)
        {
            string src = string.Format(hdMapUrl.Value + "{0}/{1}", mapaId, nivelId);
            if (nivelId == null)
                src = string.Format(hdMapUrl.Value + "{0}", mapaId);
            MapaFrame.Src = src;
        }

        private void PreencherComboMapa()
        {
            var mapas = Enum.GetValues(typeof(enumTrilhaMapa)).Cast<enumTrilhaMapa>().Select(x => new { Nome = x.GetDescription(), ID = (int)x }).ToList();

            WebFormHelper.PreencherLista(mapas, ddlTipoMapa);
        }

        private void PreencherComboTemplateCertificado()
        {
            var listaCertificadoTemplate =
                new ManterCertificadoTemplate().ObterTodosCertificadosSomenteIdNome()
                    .ToList();
            WebFormHelper.PreencherLista(listaCertificadoTemplate, ddlCertificadoTemplate, false, true);
        }

        private void PreencherComboTermoAceite()
        {
            WebFormHelper.PreencherLista(new ManterTermoAceite().ObterTodosTermoAceite(), ddlTermoAceite, false, true);
        }

        private classes.TrilhaNivel ObterTrilhaNivel()
        {
            classes.TrilhaNivel nivel = new classes.TrilhaNivel();

            if (Request["Id"] != null)
            {
                int idNivel;
                if (int.TryParse(Request["Id"], out idNivel))
                {
                    nivel = new ManterTrilhaNivel().ObterTrilhaNivelPorID(idNivel);
                }
            }
            else
            {
                int idTrilha;
                if (int.TryParse(txtTrilha.Text, out idTrilha))
                {
                    nivel.Trilha = new ManterTrilha().ObterTrilhaPorId(idTrilha);
                }
                else
                {
                    throw new AcademicoException("Você deve selecionar ao menos uma trilha");
                }
            }

            try
            {

                // Prazo.
                int qtdPrazo;
                if (!int.TryParse(txtPrazo.Text.Trim(), out qtdPrazo))
                    throw new AcademicoException("Valor Inválido para o campo Prazo.");

                // Nota Mínima.
                decimal valorNotaMinima;
                if (string.IsNullOrWhiteSpace(txtValorNotaMinima.Text))
                    throw new AcademicoException("Campo Valor da Nota Mínima é obrigatório.");
                if (!decimal.TryParse(txtValorNotaMinima.Text.Trim(), out valorNotaMinima))
                    throw new AcademicoException("Valor Inválido para o campo Valor da Nota Mínima.");
                if (valorNotaMinima > 10 || valorNotaMinima <= 0)
                    throw new AcademicoException("O valor da Nota Mínima deve ser de 0.1 a 10");

                // Carga Horária.
                int qtdCargaHoraria;
                if (!int.TryParse(txtCargaHoraria.Text.Trim(), out qtdCargaHoraria))
                {
                    throw new AcademicoException("Valor Inválido para a Carga Horária.");
                }

                nivel.Nome = txtNomeNivel.Text;

                if (!string.IsNullOrEmpty(ddlAceitaNovasMatriculas.SelectedValue))
                {
                    var valorInformadoParaInscricaoAberta = ddlAceitaNovasMatriculas.SelectedValue;
                    nivel.AceitaNovasMatriculas =
                                    valorInformadoParaInscricaoAberta.ToUpper().Equals("S");
                }
                else
                {
                    nivel.AceitaNovasMatriculas = false;
                }

                //Certificado
                if (!string.IsNullOrEmpty(ddlCertificadoTemplate.SelectedValue) &&
                    !ddlCertificadoTemplate.SelectedValue.Equals("0"))
                {
                    nivel.CertificadoTemplate =
                        new ManterCertificadoTemplate().ObterTodosCertificadosSomenteIdNome()
                            .FirstOrDefault(x => x.ID == int.Parse(ddlCertificadoTemplate.SelectedItem.Value));
                }
                else
                {
                    nivel.CertificadoTemplate = null;
                }

                //Pré-Requisito
                if (!string.IsNullOrEmpty(ddlPreRequisito.SelectedValue) &&
                    !ddlPreRequisito.SelectedValue.Equals("0"))
                {
                    var trilhaNivelPreRequisito =
                        nivel.Trilha.ListaTrilhaNivel.FirstOrDefault(
                            x => x.Nome == ddlPreRequisito.SelectedItem.Text);
                    if (trilhaNivelPreRequisito != null)
                    {
                        nivel.PreRequisito = trilhaNivelPreRequisito;
                    }
                }
                else
                {
                    //Retira o Pré-Requisito do nível da trilha, pois a opção selecione foi escolhida.
                    nivel.PreRequisito = null;
                }

                //Porcentagens Trofeus
                nivel.PorcentagensTrofeus = txtPorcentagensTrofeus.Value;

                //Prazo
                nivel.QuantidadeDiasPrazo = qtdPrazo;

                //Texto do Termo de Aceite
                //TrilhaDaSessao.ListaTrilhaNivel[indexOfTrilhaNivel].TermoAceite =
                //    !string.IsNullOrWhiteSpace(txtTextoTermoDeAceite.Text)
                //        ? txtTextoTermoDeAceite.Text.Trim()
                //        : null;

                //Texto do Termo de Aceite
                nivel.TermoAceite = ObterTermoDeAceite();

                //Descrição
                nivel.Descricao =
                                    !string.IsNullOrWhiteSpace(txtDescricaoTrilhaNivel.Text)
                                        ? txtDescricaoTrilhaNivel.Text.Trim()
                                        : null;

                //Valor da Ordem
                nivel.ValorOrdem =
                    byte.Parse(ddlOrdem.SelectedItem.Value);

                //Valor da Nota Minima
                nivel.NotaMinima = valorNotaMinima;

                //Carga Horária
                nivel.CargaHoraria = qtdCargaHoraria;

                //Monitor
                nivel.Monitor = ucLupaUsuarioMonitor.SelectedUser;

                //Permissoes
                IncluirPermissoesTrilha(ref nivel);

                //Questionario
                AdicionarQuestionarioATrilhaNivel(nivel);

                //Limite dias de cancelamento
                int limiteCancelamento;
                if (int.TryParse(txtLimiteCancelamento.Text, out limiteCancelamento))
                {
                    nivel.LimiteCancelamento = limiteCancelamento;
                }

                if (!string.IsNullOrEmpty(txtQuantidadeMoedasProvaFinal.Text))
                {
                    nivel.QuantidadeMoedasProvaFinal = int.Parse(txtQuantidadeMoedasProvaFinal.Text);
                }

                if (!string.IsNullOrEmpty(txtQuantidadeMoedasPorCurtida.Text))
                {
                    nivel.QuantidadeMoedasPorCurtida = int.Parse(txtQuantidadeMoedasPorCurtida.Text);
                }

                if (!string.IsNullOrEmpty(txtQuantidadeMoedasPorDescurtida.Text))
                {
                    nivel.QuantidadeMoedasPorDescurtida = int.Parse(txtQuantidadeMoedasPorDescurtida.Text);
                }

                if (ddlTipoMapa.SelectedValue != "")
                {
                    nivel.Mapa = (enumTrilhaMapa)Enum.Parse(typeof(enumTrilhaMapa), ddlTipoMapa.Text);
                }

                if (!string.IsNullOrEmpty(txtCambioMoedas.Text))
                {
                    nivel.ValorPrataPorOuro = int.Parse(txtCambioMoedas.Text);
                }
            }
            catch (AcademicoException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return nivel;
        }

        private void AdicionarQuestionarioATrilhaNivel(classes.TrilhaNivel trilhaNivel)
        {
            int idQuestionario;
            if (ddlQuestionarioProva.SelectedItem != null && int.TryParse(ddlQuestionarioProva.SelectedItem.Value, out idQuestionario) && idQuestionario != 0)
                TratarQuestionario(trilhaNivel, idQuestionario, false, enumTipoQuestionarioAssociacao.Prova);
            else
                TratarRemocao(trilhaNivel, false, enumTipoQuestionarioAssociacao.Prova);

            if (ddlQuestionarioProva.SelectedItem != null && int.TryParse(ddlQuestionarioPos.SelectedItem.Value, out idQuestionario) && idQuestionario != 0)
                TratarQuestionario(trilhaNivel, idQuestionario, false, enumTipoQuestionarioAssociacao.Pos);
            else
                TratarRemocao(trilhaNivel, false, enumTipoQuestionarioAssociacao.Pos);

            //if (ddlQuestionarioProva.SelectedItem != null && int.TryParse(ddlQuestionarioPre.SelectedItem.Value, out idQuestionario) && idQuestionario != 0)
            //    TratarQuestionario(trilhaNivel, idQuestionario, false, enumTipoQuestionarioAssociacao.Pre);
            //else
            //    TratarRemocao(trilhaNivel, false, enumTipoQuestionarioAssociacao.Pre);
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

        private void TratarRemocao(classes.TrilhaNivel trilhaNivel, bool evolutivo,
           enumTipoQuestionarioAssociacao tipoQuestionarioAssociacao)
        {
            var questionarioAssociacaoRemover =
                trilhaNivel.ListaQuestionarioAssociacao.FirstOrDefault(
                    x =>
                        x.TrilhaNivel == trilhaNivel && x.Evolutivo == evolutivo &&
                        x.TipoQuestionarioAssociacao.ID == (int)tipoQuestionarioAssociacao);

            if (questionarioAssociacaoRemover != null)
                trilhaNivel.ListaQuestionarioAssociacao.Remove(questionarioAssociacaoRemover);
        }

        private TermoAceite ObterTermoDeAceite()
        {
            int id;
            if (int.TryParse(ddlTermoAceite.SelectedValue, out id))
            {
                var termoDeAceite = new ManterTermoAceite().ObterTermoAceitePorID(id);
                if (termoDeAceite != null)
                {
                    return termoDeAceite;
                }
            }

            return null;
        }

        private void IncluirPermissoesTrilha(ref classes.TrilhaNivel nivelEdicao)
        {
            foreach (var nivelDto in ucPermissoesNivel.ObterNiveis())
            {
                var nivel = new classes.NivelOcupacional { ID = nivelDto.ID };

                if (nivelDto.IsSelecionado)
                {
                    nivelEdicao.AdicionarNivelOcupacional(nivel);
                }
                else
                {
                    nivelEdicao.RemoverNivelOcupacional(nivel);
                }
            }

            foreach (var perfilDto in ucPermissoesNivel.ObterPerfis())
            {
                var perfil = new classes.Perfil { ID = perfilDto.ID };

                if (perfilDto.IsSelecionado)
                {
                    nivelEdicao.AdicionarPerfil(perfil);
                }
                else
                {
                    nivelEdicao.RemoverPerfil(perfil);
                }
            }

            foreach (var ufDto in ucPermissoesNivel.ObterUfs())
            {
                var uf = new classes.Uf { ID = ufDto.ID };

                if (ufDto.IsSelecionado)
                {
                    nivelEdicao.AdicionarUfs(uf);
                }
                else
                {
                    nivelEdicao.RemoverUf(uf);
                }
            }
        }

        private void PreencherComboOrdem(Trilha trilha)
        {
            if (trilha != null)
            {
                for (int i = 1; i <= trilha.ListaTrilhaNivel.Count; i++)
                {
                    ddlOrdem.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }
            }
            else
            {
                ddlOrdem.Items.Add(new ListItem("0", "0"));
            }
        }

        protected void btnCancelarTrilha_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Cadastros/TrilhaNivel/ListarTrilhaNivel.aspx");
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                var manter = new ManterTrilhaNivel();
                int quantidadeDiasPrazoAtual = 0;
                int idNivel;
                if (int.TryParse(Request["Id"], out idNivel))
                {
                    var trilhaNivelAtual = manter.ObterTrilhaNivelPorID(idNivel);
                    quantidadeDiasPrazoAtual = trilhaNivelAtual.QuantidadeDiasPrazo;
                }

                var trilhaNivel = ObterTrilhaNivel();

                if (quantidadeDiasPrazoAtual > 0 && quantidadeDiasPrazoAtual != trilhaNivel.QuantidadeDiasPrazo)
                {
                    if (trilhaNivel.ListaUsuarioTrilha.Any())
                    {
                        foreach (var usuarioTrilha in trilhaNivel.ListaUsuarioTrilha)
                        {
                            usuarioTrilha.DataLimite = usuarioTrilha.DataInicio.AddDays(trilhaNivel.QuantidadeDiasPrazo);
                        }
                    }
                }

                bool statusTrilhaNivel = false;

                var trilhasNiveis = manter.ObterPorTrilha(trilhaNivel.Trilha.ID);
                foreach (var nivelTrilha in trilhasNiveis)
                {
                    if (nivelTrilha.AceitaNovasMatriculas == true)
                    {
                        statusTrilhaNivel = true;
                        break;
                    }
                }

                manter.Salvar(trilhaNivel);

                new ManterTrilha().AtualizarNodeIdDrupal(trilhaNivel.Trilha, null, null, null, null, statusTrilhaNivel);

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso !", "ListarTrilhaNivel.aspx");
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        protected void txtTrilha_TextChanged(object sender, EventArgs e)
        {
            int idTrilha;
            if (int.TryParse(txtTrilha.Text, out idTrilha))
            {
                ExibirEdicao(true);
                var trilha = new ManterTrilha().ObterTrilhaPorId(idTrilha);
                PreencherComboOrdem(trilha);
                PreencherPermissoes(trilha);
            }
            else
            {
                ExibirEdicao(false);
            }
        }

        protected void ExibirEdicao(bool exibir)
        {
            pnlItemTrilha.Visible = exibir;
            btnSalvar.Visible = exibir;
        }
    }
}