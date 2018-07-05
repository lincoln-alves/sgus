using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.BP.SGC;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Classes.SGC;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.WebForms.UserControls
{
    public partial class ucPermissoes : System.Web.UI.UserControl
    {
        // Usado para bloquear seletores de "marcar todos";
        public bool IsGestor { get; set; }

        public string DataAttribute { get; set; }

        private bool _exibirAreas;

        private readonly ManterSubarea _manterSubarea = new ManterSubarea();

        public bool ExibirAreas
        {
            get
            {
                return _exibirAreas;
            }
            set
            {
                //Obter todas as subareas.
                var existemAreas = _manterSubarea.ObterTodos().Any();

                ucAreas.CarregarDados = value && existemAreas;

                if (value && existemAreas)
                {
                    ucAreas.VerificarDados(OfertaSelecionada);
                }

                _exibirAreas = value && existemAreas;
            }
        }

        private Oferta _ofertaSelecionada;

        public Oferta OfertaSelecionada
        {
            get
            {
                return _ofertaSelecionada;
            }
            set
            {
                _ofertaSelecionada = value;
            }
        }

        public ListItemCollection ObterTodosPerfis
        {
            get
            {
                return this.ckblstPerfil.Items;
            }
        }

        public ListItemCollection ObterTodosUfs
        {
            get
            {
                return new ListItemCollection();// this.chklstUF.Items;
            }
        }

        public ListItemCollection ObterTodosNiveisOcupacionais
        {
            get
            {
                return this.ckblstNivelOcupacional.Items;
            }
        }
    
        internal void PreencherListBoxComNiveisOcupacionaisGravadosNoBancoForGestor(IList<NivelOcupacional> listaNivelOcupacional)
        {
            IsGestor = true;
            PreencherListBoxComNiveisOcupacionaisGravadosNoBanco(listaNivelOcupacional);
        }

        internal void PreencherListBoxComNiveisOcupacionaisGravadosNoBanco(IList<NivelOcupacional> ListaNivelOcupacional, bool bloquear = false)
        {
            for (int i = 0; i < ckblstNivelOcupacional.Items.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(ckblstNivelOcupacional.Items[i].Value)) continue;

                var nivelOcupacionalFoiEscolhido = VerificarSeONivelOcupacionalFoiEscolhido(ListaNivelOcupacional,
                    int.Parse(ckblstNivelOcupacional.Items[i].Value));

                ckblstNivelOcupacional.Items[i].Selected = nivelOcupacionalFoiEscolhido;

                if (bloquear && !nivelOcupacionalFoiEscolhido)
                {
                    ckblstNivelOcupacional.Items[i].Enabled = false;
                }
            }

            if (bloquear || IsGestor)
            {
                IsGestor = true;
            }
        }

        private bool VerificarSeONivelOcupacionalFoiEscolhido(IList<NivelOcupacional> listaNivelOcupacional, int idNivelOcupacional)
        {
            return listaNivelOcupacional.Any(x => x.ID == idNivelOcupacional);
        }


        internal void PreencherListBoxComPerfisGravadosNoBanco(IList<Perfil> listaPerfil, bool temPerfilPublico = false, bool bloquear = false)
        {
            if (listaPerfil != null && listaPerfil.Count > 0)
            {
                for (var i = 0; i < ckblstPerfil.Items.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(ckblstPerfil.Items[i].Value)) continue;

                    var perfilFoiEscolhido = VerificarSeOPerfilFoiEscolhido(listaPerfil,
                        int.Parse(ckblstPerfil.Items[i].Value));

                    ckblstPerfil.Items[i].Selected = perfilFoiEscolhido;

                    if (bloquear && !perfilFoiEscolhido)
                    {
                        ckblstPerfil.Items[i].Enabled = false;
                    }
                }
            }
            else
            {
                ckblstPerfil.ClearSelection();
            }
        }

        private bool VerificarSeOPerfilFoiEscolhido(IList<Perfil> listaPerfil, int idPerfil)
        {
            return listaPerfil.Any(x => x.ID == idPerfil);
        }

        internal void PreencherListBoxComUfsGravadasNoBanco(IList<Uf> listaUf)
        {
            PreencherListBoxComUfsGravadasNoBanco(listaUf, false);
        }

        internal void PreencherSomenteUfs(bool HideOtherSectionTitles = true)
        {
            if (HideOtherSectionTitles)
            {
                titlePerfil.Visible = false;
                titlePerfil.Visible = false;

                titleNivelocupacional.Visible = false;
                titleNivelocupacional.Visible = false;
            }

            PreencherUfs(Request.Url.LocalPath.ToLower().Contains("edicaooferta.aspx") ||
                         Request.Url.LocalPath.ToLower().Contains("gerenciamentomatricula.aspx") ||
                         Request.Url.LocalPath.ToLower().Contains("editarturmacapacitacao.aspx"));
        }

        internal void PreencherListBoxComUfsGravadasNoBanco(IList<Uf> listaUf, bool vagasPorUf, bool bloquear = false)
        {
            PreencherListBoxComUfsGravadasNoBanco(listaUf, vagasPorUf, null, null, null, bloquear);
        }

        internal void PreencherListBoxComUfsGravadasNoBanco(IList<Uf> listaUf, bool vagasPorUf, IList<SolucaoEducacionalPermissao> listaSolucaoEducacionalPermissao)
        {
            PreencherListBoxComUfsGravadasNoBanco(listaUf, vagasPorUf, listaSolucaoEducacionalPermissao, null, null);
        }

        internal void PreencherListBoxComUfsGravadasNoBanco(IList<Uf> listaUf, bool vagasPorUf, IList<OfertaPermissao> ListaOfertaPermissao)
        {
            PreencherListBoxComUfsGravadasNoBanco(listaUf, vagasPorUf, null, ListaOfertaPermissao, null);
        }

        internal void PreencherListBoxComUfsGravadasNoBanco(IList<Uf> listaUf, bool vagasPorUf, IList<TurmaCapacitacaoPermissao> ListaTurmaCapacitacaoPermissao)
        {
            PreencherListBoxComUfsGravadasNoBanco(listaUf, vagasPorUf, null, null, ListaTurmaCapacitacaoPermissao);
        }

        internal void PreencherListBoxComUfsGravadasNoBanco(IList<Uf> listaUf, bool vagasPorUf,
            IList<SolucaoEducacionalPermissao> listaSolucaoEducacionalPermissao,
            IList<OfertaPermissao> listaOfertaPermissao,
            IList<TurmaCapacitacaoPermissao> listaTurmaCapacitacaoPermissao,
            bool bloquear = false)
        {
            if (rptUFs.Items.Count > 0)
            {
                for (var i = 0; i < rptUFs.Items.Count; i++)
                {
                    var ckUf = (CheckBox)rptUFs.Items[i].FindControl("ckUF");
                    var txtVagas = (TextBox)rptUFs.Items[i].FindControl("txtVagas");

                    var idUf = int.Parse(ckUf.Attributes["ID_UF"]);

                    var campoHabilitado = !bloquear;

                    ckUf.Checked = listaUf.Any(x => x.ID == idUf);

                    if (ckUf.Checked && vagasPorUf)
                    {
                        if (listaSolucaoEducacionalPermissao != null && listaSolucaoEducacionalPermissao.Any())
                        {
                            var solucaoEducacionalPermissao =
                                listaSolucaoEducacionalPermissao.FirstOrDefault(f => f.Uf != null && f.Uf.ID == idUf);
                            if (solucaoEducacionalPermissao !=
                                null)
                                txtVagas.Text = solucaoEducacionalPermissao.QuantidadeVagasPorEstado.ToString();
                        }

                        if (listaOfertaPermissao != null && listaOfertaPermissao.Any())
                        {
                            var firstOrDefault =
                                listaOfertaPermissao.FirstOrDefault(f => f.Uf != null && f.Uf.ID == idUf);
                            if (firstOrDefault != null)
                                txtVagas.Text = firstOrDefault.QuantidadeVagasPorEstado.ToString();
                        }

                        if (listaTurmaCapacitacaoPermissao != null && listaTurmaCapacitacaoPermissao.Any())
                        {
                            var turmaCapacitacaoPermissao =
                                listaTurmaCapacitacaoPermissao.FirstOrDefault(f => f.Uf != null && f.Uf.ID == idUf);
                            if (turmaCapacitacaoPermissao !=
                                null)
                                txtVagas.Text = turmaCapacitacaoPermissao.QuantidadeVagasPorEstado.ToString();
                        }
                    }

                    ckUf.Enabled = campoHabilitado;
                    txtVagas.Enabled = campoHabilitado;
                }
            }
        }

        internal void LimparCampos()
        {
            foreach (ListItem registro in ckblstPerfil.Items)
            {
                registro.Selected = false;
            }

            foreach (ListItem registro in ckblstNivelOcupacional.Items)
            {
                registro.Selected = false;
            }
        }

        internal void PreencherListas(IList<Perfil> perfis = null, bool selectAll = false, bool exibirVagasUfs = false,
            enumDistribuicaoVagasOferta? modoDistribuicao = null)
        {
            PreencherPerfis(selectAll: selectAll);

            PreencherUfs(exibirVagasUfs &&
                         (modoDistribuicao == null || modoDistribuicao.Value == enumDistribuicaoVagasOferta.VagasPorUf));

            PreencherNiveisOcupacionais();

            if (modoDistribuicao != null)
            {
                VerificarModoDistribuicaoVagas(modoDistribuicao.Value);
            }
        }

        private void PreencherPerfis(IList<Perfil> perfis = null, bool selectAll = false)
        {
            try
            {
                var listaPerfil = perfis ?? new ManterPerfil().ObterTodosPerfis();

                var usuarioLogado = (new ManterUsuario()).ObterUsuarioLogado();
                if (usuarioLogado.IsGestor())
                {
                    // Gambiarra feira pela demanda #3519
                    var listaPerfisPermitidos = new List<int>
                    {
                        (int) enumPerfil.ConsultorEducacional,
                        (int) enumPerfil.GestorUC,
                        (int) enumPerfil.Orientador,
                        (int) enumPerfil.Professor,
                        (int) enumPerfil.Terceiro,
                        (int) enumPerfil.Colaborador
                    };
                    listaPerfil = listaPerfil.Where(p => listaPerfisPermitidos.Contains(p.ID)).ToList();
                }

                WebFormHelper.PreencherLista(listaPerfil, ckblstPerfil, pSelectAll: selectAll);
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private void PreencherNiveisOcupacionais()
        {
            try
            {
                var manterNivelOcupacional = new ManterNivelOcupacional();
                var listaNivelOcupacional = manterNivelOcupacional.ObterTodosNivelOcupacional();

                WebFormHelper.PreencherLista(listaNivelOcupacional, ckblstNivelOcupacional);
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        public void VerificarModoDistribuicaoVagas(enumDistribuicaoVagasOferta modoDistribuicao)
        {
            divModoDistribuicaoVagas.Visible = true;
            rblModoDistribuicaoVagas.SelectedValue = ((int)modoDistribuicao).ToString();
        }

        private void PreencherUfs(bool exibirVagasPorUf)
        {
            try
            {
                var ufs = new ManterUf().ObterTodosUf();

                rptUFs.DataSource = ufs;
                rptUFs.DataBind();

                for (var i = 0; i < ufs.Count(); i++)
                {
                    var ckUf = (CheckBox)rptUFs.Items[i].FindControl("ckUF");
                    var lblUf = (Literal)rptUFs.Items[i].FindControl("lblUF");
                    var txtVagas = (TextBox)rptUFs.Items[i].FindControl("txtVagas");
                    var lblVagas = (Literal)rptUFs.Items[i].FindControl("lblVagas");

                    ckUf.ToolTip = ufs[i].Nome;
                    ckUf.Attributes.Add("ID_UF", ufs[i].ID.ToString());

                    lblUf.Text = ufs[i].Sigla;

                    lblVagas.Visible =
                    txtVagas.Visible = exibirVagasPorUf;
                }
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        /// <summary>
        /// Marcar a Uf enviada.
        /// </summary>
        /// <param name="ufId">Id da UF a ser marcada</param>
        /// <param name="exibirVagas">Se deseja exibir o txtVagas</param>
        public void SelecionarUf(int ufId, bool exibirVagas = false)
        {
            SelecionarUfs(new List<int> { ufId }, exibirVagas);
        }

        /// <summary>
        /// Marcar as Ufs caso existam na lista informada.
        /// </summary>
        /// <param name="listaUfs">Lista de ids de UFs.</param>
        /// <param name="exibirVagas">Se deseja exibir o txtVagas</param>
        public void SelecionarUfs(IList<int> listaUfs, bool exibirVagas = false)
        {
            for (var i = 0; i < rptUFs.Controls.Count; i++)
            {
                var ckUf = (CheckBox)rptUFs.Items[i].FindControl("ckUF");
                var txtUf = (TextBox)rptUFs.Items[i].FindControl("txtVagas");

                int ufSelecionado;

                if (int.TryParse(ckUf.Attributes["ID_UF"], out ufSelecionado) && listaUfs.Contains(ufSelecionado))
                {
                    ckUf.Checked = true;
                }
                else
                {
                    ckUf.Checked = false;
                }

                // Sempre bloqueado para edição de SE.
                txtUf.Visible = exibirVagas;
            }
        }

        public List<Subarea> ObterSubareasSelecionadas()
        {
            return ucAreas.ObterSubareasSelecionadas();
        }

        public void PreencherNiveisOcupacionais(IList<DTONivelOcupacional> niveisDto, params string[] dataAttribute)
        {
            ckblstNivelOcupacional.Items.Clear();
            ckblstNivelOcupacional.ClearSelection();

            if (!niveisDto.Any()) return;

            if (dataAttribute != null && dataAttribute.Length == 2)
                ckblstNivelOcupacional.Attributes.Add(dataAttribute[0], dataAttribute[1]);

            var retorno = niveisDto.Select(nivel => new ListItem
            {
                Text = nivel.Nome,
                Value = nivel.ID.ToString(),
                Enabled = nivel.IsHabilitado,
                Selected = nivel.IsSelecionado
            }).ToArray();

            // Tem que atribuir a lista manualmente, sem usar os Helpers.
            ckblstNivelOcupacional.DataTextField = "Text";
            ckblstNivelOcupacional.DataValueField = "Value";
            ckblstNivelOcupacional.ClearSelection();
            ckblstNivelOcupacional.Items.AddRange(retorno);
        }

        public void PreencherPerfis(IList<DTOPerfil> perfisDto, params string[] dataAttribute)
        {
            ckblstPerfil.Items.Clear();
            ckblstPerfil.ClearSelection();

            if (!perfisDto.Any()) return;

            if (dataAttribute != null && dataAttribute.Length == 2)
                ckblstPerfil.Attributes.Add(dataAttribute[0], dataAttribute[1]);

            // Tem que atribuir a lista manualmente, sem usar os Helpers.
            ckblstPerfil.DataTextField = "Text";
            ckblstPerfil.DataValueField = "Value";
            ckblstPerfil.ClearSelection();
            ckblstPerfil.Items.AddRange(perfisDto.Select(perfil => new ListItem
            {
                Text = perfil.Nome,
                Value = perfil.ID.ToString(),
                Enabled = perfil.IsHabilitado,
                Selected = perfil.IsSelecionado
            }).ToArray());
        }

        public void PreencherUfs(IList<DTOUf> ufsDto, bool exibirVagasPorUf, params string[] dataAttribute)
        {
            rptUFs.DataSource = null;
            rptUFs.DataBind();

            if (ufsDto.Any())
            {

                if (dataAttribute != null && dataAttribute.Length == 2)
                    ckbUfs.Attributes.Add(dataAttribute[0], dataAttribute[1]);

                rptUFs.Visible = true;
                rptUFs.DataSource = ufsDto;
                rptUFs.DataBind();

                for (var i = 0; i < ufsDto.Count(); i++)
                {
                    var uf = ufsDto[i];

                    var ckUf = (CheckBox)rptUFs.Items[i].FindControl("ckUF");
                    var lblUf = (Literal)rptUFs.Items[i].FindControl("lblUF");
                    var txtVagas = (TextBox)rptUFs.Items[i].FindControl("txtVagas");
                    var lblVagas = (Literal)rptUFs.Items[i].FindControl("lblVagas");

                    ckUf.Text = uf.Nome;
                    ckUf.ToolTip = uf.Nome;
                    ckUf.Attributes.Add("ID_UF", uf.ID.ToString());
                    ckUf.Enabled = uf.IsHabilitado;
                    ckUf.Checked = uf.IsSelecionado;

                    lblUf.Text = uf.Sigla;

                    lblVagas.Visible =
                    txtVagas.Visible = exibirVagasPorUf;

                    if (exibirVagasPorUf && uf.Vagas.HasValue)
                        txtVagas.Text = uf.Vagas.Value.ToString();
                }
            }
        }

        public List<DTOUf> ObterUfs(Oferta oferta = null, bool somenteSelecionados = false)
        {
            var ufs = new List<DTOUf>();

            for (var i = 0; i < rptUFs.Controls.Count; i++)
            {
                var ckUf = (CheckBox)rptUFs.Items[i].FindControl("ckUF");
                var txtVagas = (TextBox)rptUFs.Items[i].FindControl("txtVagas");

                var vagas = 0;

                if (!string.IsNullOrEmpty(txtVagas.Text) && vagas == 0)
                    vagas = int.Parse(txtVagas.Text);

                if (oferta != null)
                {
                    if (!oferta.FiladeEspera && vagas > oferta.QuantidadeMaximaInscricoes)
                        throw new AcademicoException(string.Format("O máximo de quantidade de vagas por estado é {0}.", oferta.QuantidadeMaximaInscricoes));
                }

                int ufId;

                if (int.TryParse(ckUf.Attributes["ID_UF"], out ufId))
                    ufs.Add(new DTOUf { ID = ufId, Vagas = vagas, IsSelecionado = ckUf.Checked });
            }

            if (somenteSelecionados)
                ufs = ufs.Where(x => x.IsSelecionado).ToList();

            return ufs;
        }

        public List<DTONivelOcupacional> ObterNiveis(bool somenteSelecionados = false)
        {
            var retorno = new List<DTONivelOcupacional>();

            retorno.AddRange(
                ckblstNivelOcupacional.Items.Cast<ListItem>()
                    .Select(item => new DTONivelOcupacional
                    {
                        ID = int.Parse(item.Value),
                        IsSelecionado = item.Selected
                    }));

            if (somenteSelecionados)
                retorno = retorno.Where(x => x.IsSelecionado).ToList();

            return retorno;
        }

        public List<DTOPerfil> ObterPerfis(bool somenteSelecionados = false)
        {
            var retorno = new List<DTOPerfil>();

            retorno.AddRange(
                ckblstPerfil.Items.Cast<ListItem>()
                    .Select(item => new DTOPerfil
                    {
                        ID = int.Parse(item.Value),
                        IsSelecionado = item.Selected
                    }));

            if (somenteSelecionados)
                retorno = retorno.Where(x => x.IsSelecionado).ToList();

            return retorno;
        }

        public void VerificarQuantidadeDeVagas(int vagas)
        {
            var vagasTotal = 0;

            for (var i = 0; i < rptUFs.Items.Count; i++)
            {
                var ckUf = (CheckBox)rptUFs.Items[i].FindControl("ckUF");
                var txtVagas = (TextBox)rptUFs.Items[i].FindControl("txtVagas");

                if (ckUf.Checked)
                {
                    int vagasUf;
                    if (int.TryParse(txtVagas.Text, out vagasUf))
                    {
                        vagasTotal += vagasUf;
                    }
                    else
                    {
                        throw new AcademicoException("Não foi possível identificar a quantidade de vagas selecionadas para a uf");
                    }
                }
            }

            if (vagasTotal > vagas)
            {
                throw new AcademicoException("A quantidade de vagas por uf não pode ser maior que a quantidade de vagas informada");
            }
        }

        public enumDistribuicaoVagasOferta? ObterDistribuicaoVagas()
        {
            try
            {
                var valorSelecionado = int.Parse(rblModoDistribuicaoVagas.SelectedValue);

                return (enumDistribuicaoVagasOferta)valorSelecionado;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Exibir ou esconder a quantidade de vagas por UF de acordo com o valor selecionado.
        /// </summary>
        protected void rblModoDistribuicaoVagas_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var modoDistribuicaoVagas =
                (enumDistribuicaoVagasOferta)(int.Parse(rblModoDistribuicaoVagas.SelectedValue));

            for (var i = 0; i < rptUFs.Items.Count; i++)
            {
                var lblVagas = (Literal)rptUFs.Items[i].FindControl("lblVagas");
                var txtVagas = (TextBox)rptUFs.Items[i].FindControl("txtVagas");

                lblVagas.Visible =
                    txtVagas.Visible = modoDistribuicaoVagas == enumDistribuicaoVagasOferta.VagasPorUf;
            }
        }
    }
}