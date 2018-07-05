using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.Cadastros.Informe
{
    public partial class EdicaoInforme : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request["Id"] == null)
                {
                    PreencherCampos();
                }
                else
                {
                    HabilitarBotoesVisualizarEnviar();

                    var informe = new ManterInforme().ObterPorId(int.Parse(Request["Id"]));

                    if (informe == null)
                    {
                        WebFormHelper.ExibirMensagem(enumTipoMensagem.Atencao, "Informe inválido. Tente novamente.");

                        Response.Redirect("ListarInformes.aspx");
                    }
                    else
                    {
                        PreencherCampos(informe);
                    }
                }
            }
        }

        private void BuscarSolucoesJson()
        {
            // Buscar todas a SEs e remover as que todas as ofertas já possuam todas as turmas selecionadas.
            var solucoes =
                new ManterSolucaoEducacional().ObterTodosSolucaoEducacional().Where(x => x.ListaOferta.Any());

            ViewState["_Solucao"] =
                Helpers.Util.ObterListaAutocomplete(solucoes);
        }

        private void BuscarOfertasJson(int solucaoId)
        {
            var turmasSelecionadas = ObterTurmasSelecionadas(false).Select(x => x.ID).ToList();

            // Buscar ofertas da SE e remover ofertas cujas todas as turmas já estejam selecionadas.
            var ofertas =
                new ManterOferta().ObterOfertaPorSolucaoEducacional(solucaoId).Where(x => x.ListaTurma.Any() && !x.ListaTurma.Select(t => t.ID).All(turmaId => turmasSelecionadas.Contains(turmaId)));
            
            ViewState["_Oferta"] =
                Helpers.Util.ObterListaAutocomplete(ofertas);
        }

        private void BuscarTurmasJson(int ofertaId)
        {
            var turmasSelecionadas = ObterTurmasSelecionadas(false).Select(x => x.ID).ToList();

            // Buscar turmas da oferta e remover turmas selecionadas da busca.
            var turmas =
                new ManterTurma().ObterTurmasPorOferta(ofertaId, false)
                    .Where(x => !turmasSelecionadas.Contains(x.ID));

            if (turmas.Any())
            {
                ViewState["_Turma"] =
                    Helpers.Util.ObterListaAutocomplete(turmas);
            }
            else
            {
                // Caso nao tenha mais turmas, limpar as ofertas e carregar novamente.
                txtOferta.Text = "";

                LimparSeOfertaTurma();

                BuscarOfertasJson(int.Parse(txtSolucao.Text));
            }
        }

        /// <summary>
        /// Preencher campos no cadastro.
        /// </summary>
        private void PreencherCampos()
        {
            BuscarSolucoesJson();
        }

        /// <summary>
        /// Preencher campos na edição.
        /// </summary>
        /// <param name="informe">Objeto Informe para preencher os dados.</param>
        private void PreencherCampos(Dominio.Classes.Informe informe)
        {
            // Chama o preenchimento dos campos na edição. É um comportamento específico dessa tela.
            PreencherCampos();

            txtNumero.Text = informe.Numero.ToString();
            txtMesAno.Text = informe.ObterMesAno();

            AtualizarGridTurmas(informe.Turmas.ToList());
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListarInformes.aspx");
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                var informe = SalvarInforme();

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados gravados com sucesso.", "EdicaoInforme.aspx?Id=" + informe.ID);
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
            catch (Exception)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Houve um erro inesperado no cadastro do Informe. Tente novamente.");
            }
        }

        private Dominio.Classes.Informe ObterObjetoInforme()
        {
            ValidarInforme();

            var informe = Request["Id"] != null
                ? new BMInforme().ObterPorId(int.Parse(Request["Id"]))
                : new Dominio.Classes.Informe();

            informe.Numero = int.Parse(txtNumero.Text);

            var mesAno = txtMesAno.Text.Trim().Split('/');
            informe.Mes = int.Parse(mesAno[0]);
            informe.Ano = int.Parse(mesAno[1]);

            informe.Turmas = ObterTurmasSelecionadas();

            return informe;
        }

        private void ValidarInforme()
        {
            if (string.IsNullOrWhiteSpace(txtNumero.Text))
                throw new AcademicoException("Campo \"Número\" é obrigatório.");

            int id;
            if (!int.TryParse(txtNumero.Text.Trim(), out id))
                throw new AcademicoException("Campo \"Número\" é inválido.");

            if (string.IsNullOrWhiteSpace(txtMesAno.Text))
                throw new AcademicoException("Campo \"Mês e Ano\" é obrigatório.");

            if(gvTurmas.Rows.Count == 0)
                throw new AcademicoException("Selecione pelo menos uma turma.");

            try
            {
                var mesAno = txtMesAno.Text.Split('/');

                var mes = int.Parse(mesAno[0]);

                if (mes < 1)
                    throw new AcademicoException("O mês não pode ser menor que 1.");

                if (mes > 12)
                    throw new AcademicoException("O mês não pode ser maior que 12.");

                int ano;

                if (!int.TryParse(mesAno[1], out ano))
                    throw new AcademicoException(string.Format("O ano \"{0}\" é invalido.", ano));
            }
            catch (Exception)
            {
                throw new AcademicoException("Campo \"Mês e Ano\" é inválido.");
            }
        }

        protected void txtSolucao_OnTextChanged(object sender, EventArgs e)
        {
            LimparSeOfertaTurma();

            int idSolucao;

            if(!string.IsNullOrWhiteSpace(txtSolucao.Text) && int.TryParse(txtSolucao.Text, out idSolucao))
                BuscarOfertasJson(idSolucao);
        }

        protected void txtOferta_OnTextChanged(object sender, EventArgs e)
        {
            LimparSeOfertaTurma();

            int idOferta;

            if (!string.IsNullOrWhiteSpace(txtOferta.Text) && int.TryParse(txtOferta.Text, out idOferta))
                BuscarTurmasJson(idOferta);
            else
            {
                int idSolucao;

                if (!string.IsNullOrWhiteSpace(txtSolucao.Text) && int.TryParse(txtSolucao.Text, out idSolucao))
                    BuscarOfertasJson(idSolucao);
            }
        }

        private void LimparSeOfertaTurma()
        {
            if (string.IsNullOrWhiteSpace(txtSolucao.Text))
            {
                txtOferta.Text = "";
                ViewState["_Oferta"] = null;
            }

            if (string.IsNullOrWhiteSpace(txtOferta.Text))
            {
                txtTurma.Text = "";
                ViewState["_Turma"] = null;
            }
        }

        public List<Dominio.Classes.Turma> ObterTurmasSelecionadas(bool buscarObjetoCompleto = true)
        {
            var lista = new List<Dominio.Classes.Turma>();

            for (var i = 0; i < gvTurmas.Rows.Count; i++)
            {
                var dataKey = gvTurmas.DataKeys[i];

                if (dataKey != null)
                {
                    var idTurma = int.Parse(dataKey.Value.ToString());

                    // Adiciona na lista de retorno verficando se deseja pegar o objeto do banco ou criar um novo, dependendo da necessidade.
                    lista.Add(buscarObjetoCompleto
                        ? new ManterTurma().ObterTurmaPorID(idTurma)
                        : new Dominio.Classes.Turma {ID = idTurma});
                }
            }

            return lista;
        }

        protected void btnAdicionarTurma_OnClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTurma.Text))
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Selecione uma SE, uma Oferta e uma Turma.");
            }
            else
            {
                var turmasSelecionadas = ObterTurmasSelecionadas();

                turmasSelecionadas.Add(new ManterTurma().ObterTurmaPorID(int.Parse(txtTurma.Text)));

                AtualizarGridTurmas(turmasSelecionadas);

                txtSolucao.Text = "";

                LimparSeOfertaTurma();
            }
        }

        protected void RemoverTurma_OnServerClick(object sender, EventArgs e)
        {
            try
            {
                // Obter uma nova lista de turma sem a turma que se ordenou a remoção, e repreencher o grid.
                var turmasSelecionadas = ObterTurmasSelecionadas();

                var turmaRemovidaId = int.Parse(((HtmlButton)sender).Attributes["TurmaId"]);

                var novasTurmas =
                    turmasSelecionadas.Where(x => x.ID != turmaRemovidaId).ToList();

                AtualizarGridTurmas(novasTurmas);

                // Atualizar a lista de turmas caso a turma removida esteja na lista que o usuário está visualizando atualmente.
                int ofertaId;
                if (int.TryParse(txtOferta.Text, out ofertaId))
                {
                    var turmaRemovida = turmasSelecionadas.FirstOrDefault(x => x.ID == turmaRemovidaId);

                    if (turmaRemovida != null && ofertaId == turmaRemovida.Oferta.ID)
                        BuscarTurmasJson(int.Parse(txtOferta.Text));
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private void AtualizarGridTurmas(IList<Dominio.Classes.Turma> turmas)
        {
            contadorTurmas.InnerText = turmas.Count().ToString();

            WebFormHelper.PreencherGrid(turmas, gvTurmas);
        }

        protected void gvTurmas_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Apeans exibir o título caso dê databind.
            TituloTurmas.Visible = true;
        }

        protected void btnEnviar_OnClick(object sender, EventArgs e)
        {

            if (Request["Id"] == null)
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Salve o Informe primeiro.");
            else
            {
                try
                {
                    // Salvar antes de enviar para preservar alterações.
                    SalvarInforme();

                    Response.Redirect("EnvioInforme.aspx?Id=" + Request["Id"]);
                }
                catch (AcademicoException ex)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
                }
                catch (Exception)
                {
                    WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Houve um erro inesperado no cadastro do Informe. Tente novamente.");
                }
            }
        }

        private Dominio.Classes.Informe SalvarInforme()
        {
            var informe = ObterObjetoInforme();

            new ManterInforme().Salvar(informe);

            return informe;
        }

        private void HabilitarBotoesVisualizarEnviar()
        {
            //btnVisualizar.Enabled = true;
            btnEnviar.Enabled = true;
        }
    }
}