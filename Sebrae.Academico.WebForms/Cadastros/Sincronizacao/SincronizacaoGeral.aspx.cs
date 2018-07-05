using System;
using System.Linq;
using System.Threading;
using System.Web.UI;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.WebForms.Cadastros.Sincronizacao
{
    public partial class SincronizacaoGeral : Page
    {
        protected void btnSyncAll_OnClick(object sender, EventArgs e)
        {
            IniciarThreadGeral();

            WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso,
                "Enviando todos os dados de sincronização para o portal. Isso deve levar bastante tempo.");
        }

        private void IniciarThreadGeral()
        {
            var manterSe = new ManterSolucaoEducacional();
            var manterOferta = new ManterOferta();
            var manterPr = new ManterPrograma();
            var manterCp = new ManterCapacitacao();
            var manterTr = new ManterTrilha();
            var bmConfiguracaoSistema = new BMConfiguracaoSistema();
            var bmLogSincronia = new BMLogSincronia();
            var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();
            
            var somenteNaoSincronizados = ckbSomenteNaoSincronizados.Checked;


            var thread = new Thread(() =>
            {
                EnviarSolucoes(manterSe, bmConfiguracaoSistema, bmLogSincronia, usuarioLogado, somenteNaoSincronizados);
                EnviarOfertas(manterOferta, bmConfiguracaoSistema, bmLogSincronia, usuarioLogado, somenteNaoSincronizados);
                EnviarProgramasCapacitacoes(manterPr, manterCp, bmConfiguracaoSistema, bmLogSincronia, usuarioLogado, somenteNaoSincronizados);
                EnviarTrilhas(manterTr, bmConfiguracaoSistema, bmLogSincronia, usuarioLogado, somenteNaoSincronizados);
            })
            {
                IsBackground = true,
                Name = Guid.NewGuid().ToString()
            };

            thread.Start();
        }

        protected void btnSyncSolucoes_OnClick(object sender, EventArgs e)
        {
            IniciarThreadSolucoes();

            WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso,
                "Enviando soluções para o portal. Isso deve levar algum tempo.");
        }

        protected void btnSyncOfertas_OnClick(object sender, EventArgs e)
        {
            IniciarThreadOfertas();

            WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso,
                "Enviando soluções para o portal. Isso deve levar algum tempo.");
        }

        protected void btnSyncProgramas_OnClick(object sender, EventArgs e)
        {
            IniciarThreadProgramasCapacitacoes();

            WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso,
                "Enviando soluções para o portal. Isso pode levar algum tempo.");
        }

        protected void btnSyncTrilhas_OnClick(object sender, EventArgs e)
        {
            IniciarThreadTrilhas();

            WebFormHelper.ExibirMensagem(enumTipoMensagem.Sucesso,
                "Enviando soluções para o portal. Isso deve levar algum tempo.");
        }

        private void IniciarThreadSolucoes()
        {
            var manterSe = new ManterSolucaoEducacional();
            var bmConfiguracaoSistema = new BMConfiguracaoSistema();
            var bmLogSincronia = new BMLogSincronia();
            var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();
            
            var somenteNaoSincronizados = ckbSomenteNaoSincronizados.Checked;

            var thread =
                new Thread(
                    () => EnviarSolucoes(manterSe, bmConfiguracaoSistema, bmLogSincronia, usuarioLogado, somenteNaoSincronizados))
                {
                    IsBackground = true,
                    Name = Guid.NewGuid().ToString()
                };

            thread.Start();
        }

        private void EnviarSolucoes(ManterSolucaoEducacional manterSe, BMConfiguracaoSistema bmConfiguracaoSistema,
            BMLogSincronia bmLogSincronia, Usuario usuarioLogado, bool somenteNaoSincronizados)
        {
            try
            {
                var solucoes =
                manterSe.ObterTodosSolucaoEducacional()
                    .Where(
                        x =>
                            x.ListaAreasTematicas.Any() &&
                            x.FormaAquisicao != null &&
                            x.FormaAquisicao.EnviarPortal &&
                            (somenteNaoSincronizados == false || !x.IdNodePortal.HasValue)).ToList();

                foreach (var solucao in solucoes)
                {
                    try
                    {
                        if (somenteNaoSincronizados && solucao.IdNodePortal.HasValue)
                            continue;

                        solucao.IdNodePortal = null;

                        manterSe.AtualizarNodeIdDrupal(solucao, bmConfiguracaoSistema, bmLogSincronia, usuarioLogado);
                    }
                    catch (Exception)
                    {
                        // ignored;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void IniciarThreadOfertas()
        {
            var manterOferta = new ManterOferta();
            var bmConfiguracaoSistema = new BMConfiguracaoSistema();
            var bmLogSincronia = new BMLogSincronia();
            var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();
            
            var somenteNaoSincronizados = ckbSomenteNaoSincronizados.Checked;

            var thread =
                new Thread(
                    () =>
                        EnviarOfertas(manterOferta, bmConfiguracaoSistema, bmLogSincronia, usuarioLogado, somenteNaoSincronizados))
                {
                    IsBackground = true,
                    Name = Guid.NewGuid().ToString()
                };

            thread.Start();
        }

        private void EnviarOfertas(ManterOferta manterOferta, BMConfiguracaoSistema bmConfiguracaoSistema,
            BMLogSincronia bmLogSincronia, Usuario usuarioLogado, bool somenteNaoSincronizados)
        {
            try
            {
                var ofertas =
                manterOferta.ObterTodasOfertas()
                    .Where(
                        x =>
                            x.DataInicioInscricoes.HasValue &&
                            x.DataFimInscricoes.HasValue &&
                            x.SolucaoEducacional.Ativo &&
                            x.SolucaoEducacional.FormaAquisicao != null &&
                            x.SolucaoEducacional.FormaAquisicao.EnviarPortal &&
                            x.SolucaoEducacional.ListaAreasTematicas.Any() &&
                            (somenteNaoSincronizados == false || !x.IdNodePortal.HasValue))
                    .OrderBy(x => x.Nome).ToList();

                foreach (var oferta in ofertas)
                {
                    try
                    {
                        oferta.IdNodePortal = null;

                        manterOferta.AtualizarNodeIdDrupal(oferta, bmConfiguracaoSistema, bmLogSincronia, usuarioLogado);
                    }
                    catch (Exception)
                    {
                        // ignored;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void IniciarThreadProgramasCapacitacoes()
        {
            var manterPr = new ManterPrograma();
            var manterCp = new ManterCapacitacao();
            var bmConfiguracaoSistema = new BMConfiguracaoSistema();
            var bmLogSincronia = new BMLogSincronia();
            var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();

            var somenteNaoSincronizados = ckbSomenteNaoSincronizados.Checked;

            var thread = new Thread(() =>
            {
                EnviarProgramasCapacitacoes(manterPr, manterCp, bmConfiguracaoSistema, bmLogSincronia, usuarioLogado, somenteNaoSincronizados);
            })
            {
                IsBackground = true,
                Name = Guid.NewGuid().ToString()
            };

            thread.Start();
        }

        private void EnviarProgramasCapacitacoes(ManterPrograma manterPr, ManterCapacitacao manterCp,
            BMConfiguracaoSistema bmConfiguracaoSistema, BMLogSincronia bmLogSincronia, Usuario usuarioLogado, bool somenteNaoSincronizados)
        {
            var programas = manterPr.ObterTodosProgramas();

            foreach (var programa in programas)
            {
                try
                {
                    if (somenteNaoSincronizados && programa.IdNodePortal.HasValue)
                        continue;

                    programa.IdNodePortal = null;

                    manterPr.AtualizarNodeIdDrupal(programa, bmConfiguracaoSistema, bmLogSincronia, usuarioLogado);
                }
                catch 
                {
                    // ignored;
                }
            }

            var capacitacoes = manterCp.ObterTodasCapacitacoes();

            foreach (var capacitacao in capacitacoes)
            {
                try
                {
                    if (somenteNaoSincronizados && capacitacao.IdNodePortal.HasValue)
                        continue;

                    manterCp.AtualizarNodeIdDrupal(capacitacao, bmConfiguracaoSistema, bmLogSincronia, usuarioLogado);
                }
                catch (Exception)
                {
                    // ignored;
                }
            }
        }

        private void IniciarThreadTrilhas()
        {
            var manterTr = new ManterTrilha();
            var bmConfiguracaoSistema = new BMConfiguracaoSistema();
            var bmLogSincronia = new BMLogSincronia();
            var usuarioLogado = new ManterUsuario().ObterUsuarioLogado();
            
            var somenteNaoSincronizados = ckbSomenteNaoSincronizados.Checked;

            var thread =
                new Thread(
                    () =>
                        EnviarTrilhas(manterTr, bmConfiguracaoSistema, bmLogSincronia, usuarioLogado, somenteNaoSincronizados))
                {
                    IsBackground = true,
                    Name = Guid.NewGuid().ToString()
                };

            thread.Start();
        }

        private void EnviarTrilhas(ManterTrilha manterTr,
            BMConfiguracaoSistema bmConfiguracaoSistema, BMLogSincronia bmLogSincronia, Usuario usuarioLogado,
            bool somenteNaoSincronizados)
        {
            var trilhas = manterTr.ObterTodasTrilhas();

            foreach (var trilha in trilhas)
            {
                try
                {
                    if (somenteNaoSincronizados && trilha.IdNodePortal.HasValue)
                        continue;

                    trilha.IdNodePortal = null;

                    manterTr.AtualizarNodeIdDrupal(trilha, manterTr,
                        bmConfiguracaoSistema, bmLogSincronia, usuarioLogado);
                }
                catch(Exception)
                {
                    // ignored;
                }
            }
        }
    }
}