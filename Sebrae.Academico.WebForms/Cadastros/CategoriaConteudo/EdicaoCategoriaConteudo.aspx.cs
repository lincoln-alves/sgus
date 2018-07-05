using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.Cadastros.CategoriaConteudo
{
    public partial class EdicaoCategoriaConteudo : Page
    {
        private Dominio.Classes.CategoriaConteudo _categoriaConteudoAtual;

        private ManterCategoriaConteudo _manterCategoriaConteudo = new ManterCategoriaConteudo();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;

            var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

            var isAdmin = usuarioLogado.IsAdministrador();
            AtualizarStatus();

            PreencherCombos();            

            if (Request["Id"] != null)
            {
                var idCategoriaConteudo = int.Parse(Request["Id"]);

                _categoriaConteudoAtual = _manterCategoriaConteudo.ObterCategoriaConteudoPorID(idCategoriaConteudo);

                RemoverOFilhoDaListaDePais();

                PreencherCampos(_categoriaConteudoAtual);
            }
            else
                ckbPossuiGerenciamentoAreas.Checked =
                    divPossuiGerenciamentoStatus.Visible =
                        divPossuiGerenciamentoAreas.Visible = isAdmin || usuarioLogado.IsGestor();
        }

        private void PreencherCampos(Dominio.Classes.CategoriaConteudo categoriaConteudo)
        {
            if (categoriaConteudo == null)
                return;

            txtNome.Text = categoriaConteudo.Nome;

            txtDescricao.Text = categoriaConteudo.Descricao;

            if (categoriaConteudo.CategoriaConteudoPai != null)
                WebFormHelper.SetarValorNaCombo(categoriaConteudo.CategoriaConteudoPai.ID.ToString(),
                    ddlCategoriaConteudoPai);

            txtIdNode.Text = categoriaConteudo.IdNode != null ? categoriaConteudo.IdNode.ToString() : string.Empty;

            txtTextoApresentacao.Text = !string.IsNullOrEmpty(categoriaConteudo.Apresentacao)
                ? categoriaConteudo.Apresentacao
                : string.Empty;

            txtSigla.Text = categoriaConteudo.ObterSigla();

            // Desabilita edição de sigla para categorias filhas
            txtSigla.Enabled = categoriaConteudo.CategoriaConteudoPai == null;


            if (categoriaConteudo.PossuiFiltroCategorias)
            {
                AtualizarStatus();
            }

            var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

            if (usuarioLogado.IsAdministrador() || usuarioLogado.IsGestor())
            {
                divPossuiGerenciamentoStatus.Visible = true;

                ckbPossuiGerenciamentoStatus.Checked = categoriaConteudo.PossuiStatus.HasValue &&
                                                       categoriaConteudo.PossuiStatus.Value;

                if (!categoriaConteudo.PossuiStatus.HasValue &&
                    categoriaConteudo.PossuiGerenciamentoStatus())
                {
                    ckbPossuiGerenciamentoStatus.InputAttributes.Add("disabled", "disabled");
                    ckbPossuiGerenciamentoStatus.Checked = true;
                }
                else
                {
                    ckbPossuiGerenciamentoStatus.InputAttributes.Remove("disabled");
                    ckbPossuiGerenciamentoStatus.Checked = categoriaConteudo.PossuiStatus == true;
                }

                ckbPossuiGerenciamentoAreas.Checked = categoriaConteudo.PossuiAreas.HasValue &&
                                                      categoriaConteudo.PossuiAreas.Value;

                if (!categoriaConteudo.PossuiAreas.HasValue &&
                    categoriaConteudo.PossuiGerenciamentoAreas())
                {
                    ckbPossuiGerenciamentoAreas.InputAttributes.Add("disabled", "disabled");
                    ckbPossuiGerenciamentoAreas.Checked = true;
                }
                else
                {
                    ckbPossuiGerenciamentoAreas.InputAttributes.Remove("disabled");
                    ckbPossuiGerenciamentoAreas.Checked = categoriaConteudo.PossuiAreas == true;
                }
            }

            chkLiberarValidacao.Checked = categoriaConteudo.LiberarInscricao;

            PreencherListas(categoriaConteudo);

            PreencherTermosAceite(categoriaConteudo.ID);
        }

        private void PreencherTermosAceite(int idCategoriaConteudo = 0)
        {
            var termos = new BMTermoAceite().ObterTodos().ToList();

            foreach (var item in termos)
            {
                ddlTermoAceite.Items.Insert(0, new ListItem(item.Nome, item.ID.ToString()));
            }

            ddlTermoAceite.Items.Insert(0, new ListItem("-- Sem Termo de Aceite --", string.Empty));

            if (idCategoriaConteudo != 0)
            {
                var termoSelecionado = new BMTermoAceite().ObterPorCategoriaConteudo(idCategoriaConteudo);

                if (termoSelecionado != null)
                {
                    ddlTermoAceite.Items.FindByValue(termoSelecionado.IdTermoAceite.ToString()).Selected = true;
                }
            }
        }

        private void PreencherCombos()
        {
            try
            {
                ucUF1.PreencherUFs();
                ucPermissoes1.PreencherListas();
                ucTags1.PreencherTags();
                PreencherComboCategoriasPai();
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
        }

        private void PreencherComboCategoriasPai()
        {
            var manterCategoria = new ManterCategoriaConteudo();

            var listaCategoria = manterCategoria.ObterTodasCategoriasConteudo();

            WebFormHelper.PreencherLista(listaCategoria, ddlCategoriaConteudoPai, false, true);
        }

        private void RemoverOFilhoDaListaDePais()
        {
            //Remove a tag id da lista de tags Pais
            var listaCategorias = (IList<Dominio.Classes.CategoriaConteudo>)ddlCategoriaConteudoPai.DataSource;

            var categoriaAtual = listaCategorias.FirstOrDefault(x => x.ID == _categoriaConteudoAtual.ID);

            listaCategorias.Remove(categoriaAtual);

            WebFormHelper.PreencherLista(listaCategorias, ddlCategoriaConteudoPai, false, true);
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                _categoriaConteudoAtual = ObterObjetoCategoriaConteudo();

                if (Request["Id"] == null)
                {
                    _manterCategoriaConteudo = new ManterCategoriaConteudo();
                    _manterCategoriaConteudo.IncluirCategoriaConteudo(_categoriaConteudoAtual);
                }
                else
                {
                    _categoriaConteudoAtual.TermoAceiteCategoriaCounteudo = null;
                    _manterCategoriaConteudo.AlterarCategoriaConteudo(_categoriaConteudoAtual);
                }

                if (_categoriaConteudoAtual.CategoriaConteudoPai == null)
                {
                    var filhas =
                        _manterCategoriaConteudo.ObterTodasCategoriasFilhas(_categoriaConteudoAtual.ID)
                            .Where(x => x.ID != _categoriaConteudoAtual.ID);

                    foreach (var filha in filhas)
                    {
                        filha.Sigla = _categoriaConteudoAtual.Sigla;
                        _manterCategoriaConteudo.AlterarCategoriaConteudo(filha);
                    }
                }

                var bmAceite = new BMTermoAceite();

                var termoSelecionado = bmAceite.ObterPorCategoriaConteudo(_categoriaConteudoAtual.ID);

                if (termoSelecionado != null)
                {
                    bmAceite.ExcluirTermoAceiteCategoriaConteudo(termoSelecionado.ID);
                }

                if (ddlTermoAceite.SelectedItem != null &&
                    !string.IsNullOrWhiteSpace(ddlTermoAceite.SelectedItem.Value))
                {
                    var termoCategoria = new TermoAceiteCategoriaConteudo
                    {
                        CategoriaConteudo = _categoriaConteudoAtual,
                        TermoAceite = bmAceite.ObterPorID(int.Parse(ddlTermoAceite.SelectedItem.Value))
                    };

                    bmAceite.SalvarTermoAceiteCategoriaConteudo(termoCategoria);
                }

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados gravados com sucesso",
                    "ListarCategoriaConteudo.aspx");
            }
            catch (AlertException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, ex.Message);
            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }
            catch (Exception)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, "Erro ao processar a solicitação");
            }

        }

        private Dominio.Classes.CategoriaConteudo ObterObjetoCategoriaConteudo()
        {
            _categoriaConteudoAtual = null;

            if (Request["Id"] != null)
            {
                _categoriaConteudoAtual =
                    new ManterCategoriaConteudo().ObterCategoriaConteudoPorID(int.Parse(Request["Id"]));
            }
            else
            {
                _categoriaConteudoAtual = new Dominio.Classes.CategoriaConteudo();
            }

            //Nome
            _categoriaConteudoAtual.Nome = txtNome.Text;

            _categoriaConteudoAtual.Apresentacao = txtTextoApresentacao.Text;

            _categoriaConteudoAtual.Descricao = txtDescricao.Text;

            //Tag Pai
            if (!string.IsNullOrWhiteSpace(ddlCategoriaConteudoPai.SelectedItem.Value))
            {
                _categoriaConteudoAtual.CategoriaConteudoPai =
                    new BMCategoriaConteudo().ObterPorID(int.Parse(ddlCategoriaConteudoPai.SelectedItem.Value));
            }
            else
            {
                _categoriaConteudoAtual.CategoriaConteudoPai = null;
            }

            // Status relacionados.
            if (_categoriaConteudoAtual.PossuiFiltroCategorias)
            {
                var statusSelecionados = new List<StatusMatricula>();

                foreach (ListItem checkBoxStatus in cbStatusSelecionados.Items)
                {
                    if (checkBoxStatus.Selected)
                        statusSelecionados.Add(
                            new ManterStatusMatricula().ObterStatusMatriculaPorID(int.Parse(checkBoxStatus.Value)));
                }

                _categoriaConteudoAtual.ListaStatusMatricula = statusSelecionados;
            }
            else
            {
                // Limpa os Status caso a opção não esteja marcado.
                _categoriaConteudoAtual.ListaStatusMatricula = new List<StatusMatricula>();
            }

            _categoriaConteudoAtual.AdicionarSigla(txtSigla.Text);
            _categoriaConteudoAtual.LiberarInscricao = chkLiberarValidacao.Checked;

            // Possui Status.
            var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();
            if (usuarioLogado.IsGestor() || usuarioLogado.IsAdministrador())
            {
                _categoriaConteudoAtual.PossuiStatus =
                    _categoriaConteudoAtual.CategoriaConteudoPai == null
                        ? (bool?)ckbPossuiGerenciamentoStatus.Checked
                        : null;

                _categoriaConteudoAtual.PossuiAreas =
                    _categoriaConteudoAtual.CategoriaConteudoPai == null
                        ? (bool?)ckbPossuiGerenciamentoAreas.Checked
                        : null;
            }

            IncluirUFs(ref _categoriaConteudoAtual);
            AdicionarPermissao(_categoriaConteudoAtual);
            AdicionarOuRemoverTags(_categoriaConteudoAtual);
            return _categoriaConteudoAtual;
        }

        private void IncluirUFs(ref Dominio.Classes.CategoriaConteudo categoria)
        {
            var manter = new ManterUf();
            var lsIds = ucUF1.IdsUfsMarcados;
            var lsRmv =
                categoria.ListaCategoriaConteudoUF.Where(p => !lsIds.Contains(p.UF.ID)).Select(p => p.UF.ID).ToList();
            foreach (var id in lsIds)
            {
                if (categoria.ListaCategoriaConteudoUF.Any(p => p.UF.ID == id)) continue;
                categoria.ListaCategoriaConteudoUF.Add(new CategoriaConteudoUF
                {
                    Categoria = categoria,
                    UF = manter.ObterUfPorID(id)
                });
            }
            foreach (var id in lsRmv)
            {
                categoria.ListaCategoriaConteudoUF.Remove(categoria.ListaCategoriaConteudoUF.First(p => p.UF.ID == id));
            }
        }

        private void AdicionarOuRemoverTags(Dominio.Classes.CategoriaConteudo categoria)
        {
            ucTags1.ObterInformacoesSobreAsTags();
            ucTags1.TagsSelecionadas.ForEach(x => categoria.AdicionarTag(x));
            ucTags1.TagsNaoSelecionadas.ForEach(x => categoria.RemoverTag(x));
        }

        private void AdicionarPermissao(Dominio.Classes.CategoriaConteudo categoria)
        {
            AdicionarOuRemoverPerfil(categoria);
            AdicionarOuRemoverUf(categoria);
            AdicionarOuRemoverNivelOcupacional(categoria);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListarCategoriaConteudo.aspx");
        }

        private void AdicionarOuRemoverNivelOcupacional(Dominio.Classes.CategoriaConteudo categoria)
        {
            var todosNiveisOcupacionais = ucPermissoes1.ObterTodosNiveisOcupacionais;

            if (todosNiveisOcupacionais != null && todosNiveisOcupacionais.Count > 0)
            {
                for (var i = 0; i < todosNiveisOcupacionais.Count; i++)
                {
                    var nivelOcupacionalSelecionado = new NivelOcupacional
                    {
                        ID = int.Parse(todosNiveisOcupacionais[i].Value),
                        Nome = todosNiveisOcupacionais[i].Text
                    };

                    if (todosNiveisOcupacionais[i].Selected)
                    {
                        categoria.AdicionarNivelOcupacional(nivelOcupacionalSelecionado);
                    }
                    else
                    {
                        categoria.RemoverNivelOcupacional(nivelOcupacionalSelecionado);
                    }
                }
            }
        }


        private void AdicionarOuRemoverUf(Dominio.Classes.CategoriaConteudo categoria)
        {
            try
            {
                var rptUFs = (Repeater)ucPermissoes1.FindControl("rptUFs");
                for (var i = 0; i < rptUFs.Items.Count; i++)
                {
                    var ckUf = (CheckBox)rptUFs.Items[i].FindControl("ckUF");
                    var lblUf = (Literal)rptUFs.Items[i].FindControl("lblUF");

                    var idUf = int.Parse(ckUf.Attributes["ID_UF"]);
                    var ufSelecionado = new Uf
                    {
                        ID = idUf,
                        Nome = lblUf.Text,
                    };

                    if (ckUf.Checked)
                    {
                        categoria.AdicionarUfs(ufSelecionado);
                    }
                    else
                    {
                        categoria.RemoverUfs(ufSelecionado);
                    }
                }
            }
            catch
            {
                throw new AcademicoException("Você deve informar a quantidade de vagas do estado");
            }
        }

        private void AdicionarOuRemoverPerfil(Dominio.Classes.CategoriaConteudo categoria)
        {
            var todosPerfis = this.ucPermissoes1.ObterTodosPerfis;

            if (todosPerfis != null && todosPerfis.Count > 0)
            {
                for (int i = 0; i < todosPerfis.Count; i++)
                {
                    var perfilSelecionado = new Perfil
                    {
                        ID = int.Parse(todosPerfis[i].Value),
                        Nome = todosPerfis[i].Text
                    };

                    if (todosPerfis[i].Selected)
                    {
                        categoria.AdicionarPerfil(perfilSelecionado);
                    }
                    else
                    {
                        categoria.RemoverPerfil(perfilSelecionado);
                    }
                }
            }
        }

        private void PreencherListas(Dominio.Classes.CategoriaConteudo categoria)
        {
            PreencherListaUfs(categoria);
            PreencherListaNivelOcupacional(categoria);
            PreencherListaPerfil(categoria);
            PreencherListaTag(categoria);
            ucUF1.PreencherUfsCategoria(categoria);
        }

        private void PreencherListaUfs(Dominio.Classes.CategoriaConteudo categoria)
        {
            var listaUFs = categoria.ListaPermissao.Where(x => x.Uf != null)
                .Select(x => new Uf { ID = x.Uf.ID, Nome = x.Uf.Nome }).ToList();

            ucPermissoes1.PreencherListBoxComUfsGravadasNoBanco(listaUFs);
        }

        private void PreencherListaNivelOcupacional(Dominio.Classes.CategoriaConteudo categoria)
        {
            var listaNivelOcupacional =
                categoria.ListaPermissao.Where(x => x.NivelOcupacional != null)
                    .Select(x => new NivelOcupacional { ID = x.NivelOcupacional.ID, Nome = x.NivelOcupacional.Nome })
                    .ToList();

            ucPermissoes1.PreencherListBoxComNiveisOcupacionaisGravadosNoBanco(listaNivelOcupacional);

        }

        private void PreencherListaPerfil(Dominio.Classes.CategoriaConteudo categoria)
        {
            var listaPerfil = categoria.ListaPermissao.Where(x => x.Perfil != null)
                .Select(x => new Perfil { ID = x.Perfil.ID, Nome = x.Perfil.Nome }).ToList();

            var temPerfilPublico = false;

            if (listaPerfil.Count == 0)
            {
                temPerfilPublico = categoria.ListaPermissao.Any(x => x.Perfil == null &&
                                                                     x.NivelOcupacional == null && x.Uf == null);
            }

            this.ucPermissoes1.PreencherListBoxComPerfisGravadosNoBanco(listaPerfil, temPerfilPublico);
        }

        private void PreencherListaTag(Dominio.Classes.CategoriaConteudo categoria)
        {
            IList<Dominio.Classes.Tag> listaTags = categoria.ListaTags.Where(x => x.Tag != null)
                .Select(x => new Dominio.Classes.Tag { ID = x.Tag.ID, Nome = x.Tag.Nome }).ToList<Dominio.Classes.Tag>();

            this.ucTags1.PreencherListViewComTagsGravadosNoBanco(listaTags);
        }

        protected void ddlCategoriaConteudoPai_SelectedIndexChanged(object sender, EventArgs e)
        {
            AtualizarStatus();

            int idCategoriaPai;

            var categoriaEdicao = Request["ID"] != null ?
                    new ManterCategoriaConteudo().ObterCategoriaConteudoPorID(int.Parse(Request["ID"])) : null;

            if (int.TryParse(ddlCategoriaConteudoPai.Text, out idCategoriaPai))
            {
                var categoriaPai = new ManterCategoriaConteudo().ObterCategoriaConteudoPorID(idCategoriaPai);

                if (categoriaPai == null)
                {
                    ckbPossuiGerenciamentoStatus.InputAttributes.Remove("disabled");
                    ckbPossuiGerenciamentoAreas.InputAttributes.Remove("disabled");

                    if (categoriaEdicao != null)
                    {
                        ckbPossuiGerenciamentoStatus.Checked = categoriaEdicao.PossuiStatus == true;
                        ckbPossuiGerenciamentoAreas.Checked = categoriaEdicao.PossuiAreas == true;
                    }
                    else
                    {
                        ckbPossuiGerenciamentoStatus.Checked =
                            ckbPossuiGerenciamentoAreas.Checked = false;
                    }

                    return;
                }
                else
                {
                    txtSigla.Text = categoriaPai.ObterSigla();
                    txtSigla.Enabled = string.IsNullOrEmpty(txtSigla.Text);

                    if (categoriaPai.PossuiGerenciamentoStatus())
                    {

                        ckbPossuiGerenciamentoStatus.InputAttributes.Add("disabled", "disabled");
                        ckbPossuiGerenciamentoStatus.Checked = true;
                    }
                    else
                    {
                        ckbPossuiGerenciamentoStatus.InputAttributes.Remove("disabled");

                        ckbPossuiGerenciamentoStatus.Checked = categoriaEdicao != null &&
                                                               categoriaEdicao.PossuiStatus == true;
                    }

                    if (categoriaPai.PossuiGerenciamentoAreas())
                    {
                        ckbPossuiGerenciamentoAreas.InputAttributes.Add("disabled", "disabled");
                        ckbPossuiGerenciamentoAreas.Checked = true;
                    }
                    else
                    {
                        ckbPossuiGerenciamentoAreas.InputAttributes.Remove("disabled");

                        ckbPossuiGerenciamentoAreas.Checked = categoriaEdicao != null &&
                                                              categoriaEdicao.PossuiAreas == true;
                    }
                }
            }
            else
            {
                txtSigla.Enabled = true;
                txtSigla.Text = "";
            }
        }

        protected void AtualizarStatus()
        {
            cbStatusSelecionados.Items.Clear();
            var listaStatusMatricula = new ManterStatusMatricula().ObterTodosIncluindoEspecificos();
            foreach (var status in listaStatusMatricula)
            {
                var itemStatus = new ListItem
                {
                    Text = "&nbsp;" + status.Nome,
                    Value = status.ID.ToString(),
                    Selected =
                        _categoriaConteudoAtual == null ||
                        _categoriaConteudoAtual.ListaStatusMatricula.Any(s => s.ID == status.ID)
                };

                cbStatusSelecionados.Items.Add(itemStatus);
            }
        }
    }
}