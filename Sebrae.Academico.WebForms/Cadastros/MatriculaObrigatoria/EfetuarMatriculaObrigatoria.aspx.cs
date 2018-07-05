using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.Cadastros.MatriculaObrigatoria
{
    public partial class EfetuarMatriculaObrigatoria : Page
    {
        protected void rblEfetuar_SelectedIndexChanged(object sender, EventArgs e)
        {
            cblNivelOcupacional.Items.Clear();
            cblSolucaoEducacional.Items.Clear();
            ddlNivelOcupacional.Items.Clear();
            ddlSolucaoEducacional.Items.Clear();

            switch (rblEfetuar.SelectedValue)
            {
                case "SolucaoEducacional":
                    divSolucaoEducacional.Visible = true;
                    divNivelOcupacional.Visible = false;

                    var solucoes = new ManterSolucaoEducacional().ObterObrigatorios()
                        .Select(x => x.SolucaoEducacional)
                        .Distinct()
                        .ToList();

                    WebFormHelper.PreencherLista(
                        solucoes,
                        ddlSolucaoEducacional, true);

                    break;
                case "NivelOcupacional":
                    divNivelOcupacional.Visible = true;
                    divSolucaoEducacional.Visible = false;

                    var niveis = new ManterSolucaoEducacional().ObterObrigatorios()
                        .Select(x => x.NivelOcupacional)
                        .Distinct()
                        .ToList();

                    WebFormHelper.PreencherLista(
                        niveis,
                        ddlNivelOcupacional, true);

                    break;
                default:
                    divSolucaoEducacional.Visible = false;
                    divNivelOcupacional.Visible = false;
                    break;
            }
        }

        protected void ddlNivelOcupacional_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            cblNivelOcupacional.Items.Clear();
            cblSolucaoEducacional.Items.Clear();
            ddlSolucaoEducacional.Items.Clear();

            if (ddlNivelOcupacional.SelectedIndex <= 0) return;

            var nivelSelecionado =
                new ManterNivelOcupacional().ObterNivelOcupacionalPorID(int.Parse(ddlNivelOcupacional.SelectedValue));

            var solucoesEducacionais =
                new ManterSolucaoEducacional().ObterObrigatorios(nivelSelecionado)
                    .Select(x => x.SolucaoEducacional)
                    .Distinct()
                    .ToList();

            WebFormHelper.PreencherLista(solucoesEducacionais, cblSolucaoEducacional);

            btnMatricular.Visible = true;
        }

        protected void ddlSolucaoEducacional_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            cblNivelOcupacional.Items.Clear();
            cblSolucaoEducacional.Items.Clear();
            ddlNivelOcupacional.Items.Clear();

            if (ddlSolucaoEducacional.SelectedIndex <= 0) return;

            var solucaoEducacional =
                new ManterSolucaoEducacional().ObterSolucaoEducacionalPorId(
                    int.Parse(ddlSolucaoEducacional.SelectedValue));

            var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

            if (usuarioLogado.IsGestor() && (solucaoEducacional.UFGestor == null ||
                solucaoEducacional.UFGestor.ID != usuarioLogado.UF.ID))
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Alerta, "Solução Educacional inválida.");
                return;
            }

            var niveisOcupacionais =
                solucaoEducacional.ListaSolucaoEducacionalObrigatoria.Select(x => x.NivelOcupacional)
                    .OrderBy(x => x.Nome)
                    .Distinct()
                    .ToList();

            WebFormHelper.PreencherLista(niveisOcupacionais, cblNivelOcupacional);

            btnMatricular.Visible = true;
        }

        protected void btnMatricular_Click(object sender, EventArgs e)
        {
            var idsSolucaoEducacionalSelecionados = new List<int>();
            var idsNivelOcupacionalSelecionados = new List<int>();

            switch (rblEfetuar.SelectedValue)
            {
                case "SolucaoEducacional":
                    idsSolucaoEducacionalSelecionados.Add(int.Parse(ddlSolucaoEducacional.SelectedValue));
                    ObterSelecionados(ref idsNivelOcupacionalSelecionados, ddlNivelOcupacional, cblNivelOcupacional);
                    break;
                case "NivelOcupacional":
                    idsNivelOcupacionalSelecionados.Add(int.Parse(ddlNivelOcupacional.SelectedValue));
                    ObterSelecionados(ref idsSolucaoEducacionalSelecionados, ddlSolucaoEducacional, cblSolucaoEducacional);
                    break;
            }

            // Obter soluções obrigatórias filtradas pela seleção.
            var solucoesObrigatorias = new ManterSolucaoEducacional()
                .ObterObrigatoriosPorSolucaoEducacionalNiveisOcupacionais(
                    idsSolucaoEducacionalSelecionados,
                    idsNivelOcupacionalSelecionados);

            // Filtra as soluções pelas que possuam Ofertas e Turmas vigentes.
            var listaSolucoesObrigatorias =
                solucoesObrigatorias.Where(
                    x =>
                        x.SolucaoEducacional.ListaOferta.Any(
                            o => Helpers.Util.ObterVigente(o.DataInicioInscricoes, o.DataFimInscricoes) &&
                                 o.ListaTurma.Any(t => Helpers.Util.ObterVigente(t.DataInicio, t.DataFinal)))).ToList();

            // Obtém somente os usuários dos níveis ocupacionais selecionados.
            var usuariosTodosNiveis =
                new ManterUsuario().ObterPorNiveisOcupacionais(idsNivelOcupacionalSelecionados)
                    .AsQueryable();

            var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

            // Filtrar os usuários pela UF do gestor logado, caso aplicável.
            if (usuarioLogado.IsGestor())
                usuariosTodosNiveis = usuariosTodosNiveis.Where(x => x.UF.ID == usuarioLogado.UF.ID);

            if (listaSolucoesObrigatorias.Any())
            {
                EfetuarMatriculas(listaSolucoesObrigatorias, usuariosTodosNiveis);

                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso,
                    string.Format(
                        "As matrículas estão sendo alteradas automaticamente, isto pode demorar, pois estão sendo efetuadas matrículas em {0} Soluções Educacionais, para usuários em {1} Níveis Ocupacionais.",
                        idsSolucaoEducacionalSelecionados.Count(),
                        idsNivelOcupacionalSelecionados.Count()));
            }
            else
            {
                WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso,
                    "Não foi encontrada nenhuma Solução Educacional obrigatória com alunos que precisam ser matrículados, de acordo com os filtros selecionados.");
            }
        }

        private void EfetuarMatriculas(IList<SolucaoEducacionalObrigatoria> solucoesObrigatorias, IQueryable<Usuario> usuariosTodosNiveis)
        {
            // Obtém somente os campos que serão utilizados abaixo;
            var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

            var manterMatriculaOferta = new ManterMatriculaOferta();
            var manterMatriculaTurma = new ManterMatriculaTurma();

            // Matricular usuário.
            var thread = new Thread(() =>
            {
                var currentThread = Thread.CurrentThread;

                try
                {
                    if (solucoesObrigatorias != null)
                    {
                        foreach (var obrigatorio in solucoesObrigatorias)
                        {
                            try
                            {
                                // Obter usuários do nível ocupacional da solução obrigatória.
                                var listaUsuariosNivelOcupacional =
                                    usuariosTodosNiveis.Where(x => x.NivelOcupacional.ID == obrigatorio.NivelOcupacional.ID);

                                // Obter somente os usuários que NÃO estão matriculados na solução.
                                listaUsuariosNivelOcupacional =
                                    listaUsuariosNivelOcupacional.Where(
                                        x =>
                                            x.ListaMatriculaOferta.All(
                                                y => y.Oferta.SolucaoEducacional.ID != obrigatorio.SolucaoEducacional.ID));

                                var oferta =
                                    obrigatorio.SolucaoEducacional.ListaOferta.FirstOrDefault(
                                        x =>
                                            Helpers.Util.ObterVigente(x.DataInicioInscricoes, x.DataFimInscricoes) &&
                                            x.ListaTurma.Any(t => Helpers.Util.ObterVigente(t.DataInicio, t.DataFinal)));

                                // Caso não exista oferta vigente com turma vigente, pula a solução.
                                if (oferta == null) currentThread.Abort();

                                // Seleciona somente os campos necessários.
                                listaUsuariosNivelOcupacional = listaUsuariosNivelOcupacional.Select(x => new Usuario
                                {
                                    ID = x.ID,
                                    UF = x.UF,
                                    NivelOcupacional = x.NivelOcupacional,
                                    ListaMatriculaOferta = x.ListaMatriculaOferta
                                });

                                foreach (var usuario in listaUsuariosNivelOcupacional)
                                {
                                    try
                                    {
                                        EfetuarMatricula(usuarioLogado, oferta, usuario, manterMatriculaOferta,
                                            manterMatriculaTurma);
                                    }
                                    catch (Exception)
                                    {
                                        // ignored.
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                // ignored.
                            }
                        }
                    }

                    currentThread.Abort();
                }
                catch (ThreadAbortException)
                {
                    // ignored
                }
            });

            thread.IsBackground = true;

            thread.Start();
        }


        private void EfetuarMatricula(Usuario usuarioLogado, Dominio.Classes.Oferta oferta, Usuario usuario, ManterMatriculaOferta manterMatriculaOferta, ManterMatriculaTurma manterMatriculaTurma)
        {
            var novaMatriculaOferta = new MatriculaOferta
            {
                Auditoria = new Auditoria(usuarioLogado.CPF),
                Oferta = oferta,
                Usuario = usuario,
                StatusMatricula = enumStatusMatricula.Inscrito,
                UF = usuario.UF,
                NivelOcupacional = usuario.NivelOcupacional,
                DataSolicitacao = DateTime.Today
            };

            manterMatriculaOferta.Salvar(novaMatriculaOferta);

            var novaMatriculaTurma = new MatriculaTurma
            {
                Auditoria = new Auditoria(usuarioLogado.CPF),
                Turma =
                    oferta.ListaTurma.FirstOrDefault(t => Helpers.Util.ObterVigente(t.DataInicio, t.DataFinal)),
                MatriculaOferta = novaMatriculaOferta,
                DataMatricula = DateTime.Today
            };

            novaMatriculaTurma.DataLimite = novaMatriculaTurma.CalcularDataLimite();

            manterMatriculaTurma.Salvar(novaMatriculaTurma);
        }

        protected void ObterSelecionados(ref List<int> lista, DropDownList ddl, CheckBoxList cbl)
        {
            lista.Clear();

            if (ddl.SelectedIndex >= 0 || cbl.SelectedIndex >= 0)
                lista = new List<int>();

            if (ddl.SelectedIndex >= 0)
            {
                lista.Add(int.Parse(ddl.SelectedValue));
            }

            if (cbl.SelectedIndex < 0) return;

            for (var i = 0; i < cbl.Items.Count; i++)
            {
                if (cbl.Items[i].Selected)
                {
                    lista.Add(int.Parse(cbl.Items[i].Value));
                }
            }
        }
    }
}