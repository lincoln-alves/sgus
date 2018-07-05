using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Sebrae.Academico.BP;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using classes = Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using System.Web.UI.WebControls;

namespace Sebrae.Academico.WebForms.Cadastros
{
    public partial class EdicaoPrograma : Page
    {
        private ManterPrograma _manterPrograma = new ManterPrograma();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
                return;

            PreencherCombos();

            if (Request["Id"] == null)
                return;

            var idPrograma = int.Parse(Request["Id"]);

            var programa = _manterPrograma.ObterProgramaPorID(idPrograma);

            PreencherCampos(programa);
        }

        private void PreencherCombos()
        {
            PreencherComboAreaTematica();
            ucTags1.PreencherTags();
            WebFormHelper.PreencherComponenteComOpcoesSimNao(rblAtivo);
            ucPermissoes1.PreencherListas();
        }

        private void PreencherComboAreaTematica()
        {
            var manter = new ManterAreaTematica();
            var lista = manter.ObterTodos();
            listBoxesAreaTematica.PreencherItens(lista, "ID", "Nome");
        }

        private void ObterAreasTematicasSelecionadas(ref Programa obj)
        {
            var manter = new ManterAreaTematica();

            var lsIds = listBoxesAreaTematica.RecuperarIdsSelecionados().Select(id => Convert.ToInt32(id)).ToList();
            var lsRmv = obj.ListaAreasTematicas.Where(p => !lsIds.Contains(p.AreaTematica.ID)).Select(p => p.AreaTematica.ID).ToList();
            foreach (var id in lsIds)
            {
                if (obj.ListaAreasTematicas.Any(p => p.AreaTematica.ID == id)) continue;
                obj.ListaAreasTematicas.Add(new ProgramaAreaTematica
                {
                    Programa = obj,
                    AreaTematica = manter.ObterPorId(id)
                });
            }
            foreach (var id in lsRmv)
            {
                obj.ListaAreasTematicas.Remove(obj.ListaAreasTematicas.First(p => p.AreaTematica.ID == id));
            }
        }

        private void PreencherCampos(Programa programaEdicao)
        {
            if (programaEdicao == null) return;

            //Nome
            txtNome.Text = programaEdicao.Nome;
            txtTextoApresentacao.Text = programaEdicao.Apresentacao;

            //Ativo
            WebFormHelper.SetarValorNoRadioButtonList(programaEdicao.Ativo, rblAtivo);

            PreencherListas(programaEdicao);

            // Areas Tematicas.
            listBoxesAreaTematica.MarcarComoSelecionados(programaEdicao.ListaAreasTematicas.Select(x => x.AreaTematica.ID));
            this.ucSolucaoEducacional1.PreencherListBoxComSolucoesEducacionaisGravadasNoBanco(programaEdicao.ListaSolucaoEducacional.Select(x => x.SolucaoEducacional).ToList());
        }

        private void PreencherListas(Programa programa)
        {
            //this.PreencherListaSolucaoEducacional(programa);
            PreencherListaUfs(programa);
            PreencherListaNivelOcupacional(programa);
            PreencherListaPerfil(programa);
            PreencherListaTag(programa);
        }

        private void PreencherListaPerfil(Programa programaEdicao)
        {
            //Obtém a lista de Perfis gravados no banco
            var listaPerfil = programaEdicao.ListaPermissao.Where(x => x.Perfil != null)
                    .Select(x => new Perfil { ID = x.Perfil.ID, Nome = x.Perfil.Nome }).ToList();

            var temPerfilPublico = false;

            if (listaPerfil.Count == 0)
            {
                temPerfilPublico = programaEdicao.ListaPermissao.Any(x => x.Perfil == null &&
                                                                          x.NivelOcupacional == null && x.Uf == null);
            }

            ucPermissoes1.PreencherListBoxComPerfisGravadosNoBanco(listaPerfil, temPerfilPublico);
        }

        private void PreencherListaTag(Programa programaEdicao)
        {
            //Obtém a lista de Tags gravados no banco
            var listaTags = programaEdicao.ListaTag.Where(x => x.Tag != null)
                .Select(x => new classes.Tag { ID = x.Tag.ID, Nome = x.Tag.Nome }).ToList();

            ucTags1.PreencherListViewComTagsGravadosNoBanco(listaTags);
        }


        private void PreencherListaNivelOcupacional(Programa programaEdicao)
        {
            //Obtém a lista de niveis ocupacionais gravadas no banco
            var listaNivelOcupacional = programaEdicao.ListaPermissao.Where(x => x.NivelOcupacional != null)
                .Select(x => new NivelOcupacional { ID = x.NivelOcupacional.ID, Nome = x.NivelOcupacional.Nome })
                .ToList();

            ucPermissoes1.PreencherListBoxComNiveisOcupacionaisGravadosNoBanco(listaNivelOcupacional);

        }

        //private void PreencherListaSolucaoEducacional(Programa programaEdicao)
        //{
        //    //Obtém a lista de soluções educacionais gravadas no banco
        //    IList<SolucaoEducacional> ListaSolucaoEducacional = programaEdicao.ListaSolucaoEducacional.Where(x => x.SolucaoEducacional != null)
        //            .Select(x => new SolucaoEducacional() { ID = x.SolucaoEducacional.ID, Nome = x.SolucaoEducacional.Nome }).ToList<SolucaoEducacional>();

        //    this.ucSolucaoEducacional1.PreencherListBoxComSolucoesEducacionaisGravadasNoBanco(ListaSolucaoEducacional);
        //}

        private void PreencherListaUfs(Programa programaEdicao)
        {

            //Obtém a lista de ufs
            var listaUFs = programaEdicao.ListaPermissao.Where(x => x.Uf != null)
                      .Select(x => new Uf { ID = x.Uf.ID, Nome = x.Uf.Nome }).ToList();

            ucPermissoes1.PreencherListBoxComUfsGravadasNoBanco(listaUFs);
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                Programa programa;

                if (Request["Id"] == null)
                {
                    _manterPrograma = new ManterPrograma();
                    programa = ObterObjetoPrograma();
                    _manterPrograma.IncluirPrograma(programa);
                }
                else {
                    programa = ObterObjetoPrograma();
                    _manterPrograma.AlterarPrograma(programa);
                }

                Session.Remove("ProgramaEdit");

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso, "Dados Gravados com Sucesso !", "ListarPrograma.aspx");

            }
            catch (AcademicoException ex)
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Erro, ex.Message);
            }


        }

        private Programa ObterObjetoPrograma()
        {
            Programa programa;

            if (Request["Id"] == null)
            {
                _manterPrograma = new ManterPrograma();
                programa = new Programa();
            }
            else
            {
                var idPrograma = int.Parse(Request["Id"]);
                programa = new ManterPrograma().ObterProgramaPorID(idPrograma);
            }

            //Nome
            if (!string.IsNullOrWhiteSpace(txtNome.Text)) programa.Nome = txtNome.Text.Trim();

            //Áreas Temáticas
            ObterAreasTematicasSelecionadas(ref programa);

            if (programa.ListaAreasTematicas == null)
            {
                throw new AcademicoException("Selecione uma área temática para o programa");
            }

            if (programa.ListaAreasTematicas.Count <= 0)
            {
                throw new AcademicoException("Selecione uma área temática para o programa");
            }

            if (programa.ListaAreasTematicas.Count > 3)
            {
                throw new AcademicoException("É possivel selecionar apenas 3 áreas temáticas para o programa");
            }

            programa.Apresentacao = txtTextoApresentacao.Text.Trim();

            //Ativo ?
            if (rblAtivo.SelectedItem != null && !string.IsNullOrWhiteSpace(rblAtivo.SelectedItem.Value))
            {
                var valorInformadoParaAtivo = rblAtivo.SelectedItem.Value;

                if (valorInformadoParaAtivo.ToUpper().Equals("S"))
                    programa.Ativo = true;
                else if (valorInformadoParaAtivo.ToUpper().Equals("N"))
                    programa.Ativo = false;
            }

            AdicionarPermissao(programa);
            AdicionarOuRemoverTags(programa);
            AdicionarOuRemoverSolucaoEducacional(programa);

            return programa;
        }

        private void AdicionarOuRemoverSolucaoEducacional(Programa programaEdicao)
        {
            var listaSelecionados = ucSolucaoEducacional1.ConverterListItemCollectionEmListaTipada(ucSolucaoEducacional1.ObterTodasSolucoesEducacionais) ?? new List<classes.SolucaoEducacional>();
            if (listaSelecionados.Count == 0) return;
            var listaIdsSelecionados = listaSelecionados.Select(x => x.ID).ToList();
            var listaDiferenca = programaEdicao.ListaSolucaoEducacional.Where(x => !listaIdsSelecionados.Contains(x.SolucaoEducacional.ID)).Select(x => x.SolucaoEducacional).ToList();
            var listaIdsDiferenca = listaDiferenca.Select(x => x.ID).ToList();

            foreach (var item in listaIdsDiferenca.Select(id => programaEdicao.ListaSolucaoEducacional.FirstOrDefault(x => x.SolucaoEducacional.ID == id)).Where(item => item != null))
            {
                programaEdicao.ListaSolucaoEducacional.Remove(item);
            }
            var manterUsuarioLogado = new ManterUsuario();
            var manterSe = new ManterSolucaoEducacional();
            var usuarioLogado = manterUsuarioLogado.ObterUsuarioLogado();
            foreach (var id in listaIdsSelecionados)
            {
                if (!programaEdicao.ListaSolucaoEducacional.Any(x => x.SolucaoEducacional.ID == id))
                {
                    programaEdicao.ListaSolucaoEducacional.Add(new ProgramaSolucaoEducacional
                    {
                        Programa = programaEdicao,
                        SolucaoEducacional = manterSe.ObterSolucaoEducacionalPorId(id),
                        Auditoria = usuarioLogado != null ? new Auditoria(usuarioLogado.CPF) : new Auditoria()
                    });
                }
            }
        }

        private void AdicionarPermissao(Programa programaEdicao)
        {
            AdicionarOuRemoverPerfil(programaEdicao);
            AdicionarOuRemoverUf(programaEdicao);
            AdicionarOuRemoverNivelOcupacional(programaEdicao);
        }

        private void AdicionarOuRemoverTags(Programa programa)
        {
            ucTags1.ObterInformacoesSobreAsTags();
            ucTags1.TagsSelecionadas.ForEach(programa.AdicionarTag);
            ucTags1.TagsNaoSelecionadas.ForEach(programa.RemoverTag);
        }

        private void AdicionarOuRemoverPerfil(Programa programa)
        {
            var todosPerfis = ucPermissoes1.ObterTodosPerfis;

            if (todosPerfis != null && todosPerfis.Count > 0)
            {
                for (var i = 0; i < todosPerfis.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(todosPerfis[i].Value))
                        continue;

                    var perfilSelecionado = new Perfil
                    {
                        ID = int.Parse(todosPerfis[i].Value),
                        Nome = todosPerfis[i].Text
                    };

                    if (todosPerfis[i].Selected)
                    {
                        programa.AdicionarPerfil(perfilSelecionado);
                    }
                    else
                    {
                        programa.RemoverPerfil(perfilSelecionado);
                    }
                }
            }
            else
            {
                if (programa.ListaPermissao == null)
                    return;

                var ofertaPermissao = new ProgramaPermissao { Programa = programa };
                programa.ListaPermissao.Add(ofertaPermissao);
            }
        }

        private void AdicionarOuRemoverUf(Programa programa)
        {
            try
            {
                var rptUFs = (Repeater)ucPermissoes1.FindControl("rptUFs");

                for (var i = 0; i < rptUFs.Items.Count; i++)
                {
                    var ckUf = (CheckBox)rptUFs.Items[i].FindControl("ckUF");
                    var lblUf = (Label)rptUFs.Items[i].FindControl("lblUF");

                    var idUf = int.Parse(ckUf.Attributes["ID_UF"]);

                    var ufSelecionado = new Uf
                    {
                        ID = idUf,
                        Nome = lblUf.Text
                    };

                    if (ckUf.Checked)
                        programa.AdicionarUfs(ufSelecionado);
                    else
                        programa.RemoverUf(ufSelecionado);
                }
            }
            catch
            {
                //throw new ExecutionEngineException("Você deve informar a quantidade de vagas do estado");
            }
        }

        private void AdicionarOuRemoverNivelOcupacional(Programa programa)
        {
            var todosNiveisOcupacionais = ucPermissoes1.ObterTodosNiveisOcupacionais;

            if (todosNiveisOcupacionais == null || todosNiveisOcupacionais.Count <= 0)
                return;

            for (var i = 0; i < todosNiveisOcupacionais.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(todosNiveisOcupacionais[i].Value))
                    continue;

                var nivelOcupacionalSelecionado = new NivelOcupacional
                {
                    ID = int.Parse(todosNiveisOcupacionais[i].Value),
                    Nome = todosNiveisOcupacionais[i].Text
                };

                if (todosNiveisOcupacionais[i].Selected)
                {
                    programa.AdicionarNivelOcupacional(nivelOcupacionalSelecionado);
                }
                else
                {
                    programa.RemoverNivelOcupacional(nivelOcupacionalSelecionado);
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Session.Remove("ProgramaEdit");
            Response.Redirect("ListarPrograma.aspx");
        }
    }
}