using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;
using Sebrae.Academico.BM.AutoMapper;
using AutoMapper;

namespace Sebrae.Academico.BM.Classes
{
    public class BMEtapa : BusinessManagerBase, IDisposable
    {
        #region Atributos

        private RepositorioBase<Etapa> repositorio;
        private RepositorioBase<EtapaPermissao> repositorioEtapaPermissao;
        private RepositorioBase<EtapaResposta> repositorioEtapaResposta;

        #endregion

        #region "Construtor"

        public BMEtapa()
        {
            repositorio = new RepositorioBase<Etapa>();
            repositorioEtapaPermissao = new RepositorioBase<EtapaPermissao>();
            repositorioEtapaResposta = new RepositorioBase<EtapaResposta>();
        }

        #endregion

        public IList<Etapa> ObterTodos()
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<Etapa>();
            return query.ToList<Etapa>();
        }

        public Etapa ObterPorId(int pId)
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<Etapa>();
            return query.Where(x => x.ID == pId).FirstOrDefault();
        }

        public Etapa ObterEtapasAposEtapaId(int etapaId)
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<Etapa>();
            return query.Where(x => x.ID == etapaId).FirstOrDefault();
        }

        public IList<Etapa> ObterPorFiltro(Etapa modulo)
        {
            var query = repositorio.session.Query<Etapa>();

            //if (!string.IsNullOrEmpty(modulo.Nome))
            //    query = query.Where(x => x.Nome.Contains(modulo.Nome));

            //if (modulo.Capacitacao.ID > 0)
            //{
            //    query = query.Where(x => x.Capacitacao.ID == modulo.Capacitacao.ID);
            //}
            //else if (modulo.Capacitacao.ID == 0 && modulo.Capacitacao.Programa.ID > 0)
            //{
            //    query = query.Where(x => x.Capacitacao.Programa.ID == modulo.Capacitacao.Programa.ID);
            //}

            return query.ToList<Etapa>();
        }

        public IList<Etapa> ObterPorProcessoId(int idProcesso)
        {
            var query = repositorio.session.Query<Etapa>();

            return query.Where(d => d.Processo.ID == idProcesso).OrderBy(x => x.Ordem).ToList();
        }

        public IQueryable<Etapa> ObterPorProcessoIdQuerableOrderByOrdem(int idProcesso)
        {
            return repositorio.session.Query<Etapa>()
                .Where(d => d.Processo.ID == idProcesso)
                .OrderBy(x => x.Ordem);
        }

        public IList<Etapa> ObterOrdemMenoresPorProcessoId(int IdProcesso, int Ordem)
        {
            var query = repositorio.session.Query<Etapa>();

            return query.Where(d => d.Processo.ID == IdProcesso && d.Ordem < Ordem).ToList();
        }

        public Etapa ObterPrimeiraEtapaDeProcesso(int IdProcesso)
        {
            var query = repositorio.session.Query<Etapa>();

            return query.Where(d => d.Processo.ID == IdProcesso).OrderBy(d => d.Ordem).First();
        }

        public Etapa ObterEtapaAtualDeProcesso(int IdProcessoResposta)
        {
            var query = repositorio.session.Query<Etapa>();

            Etapa etapaAtual = query.Where(d => d.ListaEtapaResposta.Any(f => f.Status == 0 && f.Ativo && f.ProcessoResposta.ID == IdProcessoResposta)).OrderByDescending(d => d.Ordem).FirstOrDefault();

            if (etapaAtual == null)
            {
                etapaAtual = query.Where(d => d.ListaEtapaResposta.Any(f => f.Status != 0 && f.Ativo && f.ProcessoResposta.ID == IdProcessoResposta)).OrderByDescending(d => d.Ordem).FirstOrDefault();
            }

            return etapaAtual;
        }

        //public Etapa ObterProximaEtapa(int IdProcesso, int idEtapaAtual)
        //{
        //    var query = repositorio.session.Query<Etapa>();

        //    return query.FirstOrDefault(d => d.Processo.ID == IdProcesso && d.eta);
        //}

        public byte ObterUltimaPosicaoOrdem()
        {
            var query = repositorio.session.Query<Etapa>();
            Etapa ultimaEtapa = query.OrderByDescending(d => d.Ordem).FirstOrDefault();

            if (ultimaEtapa != null)
            {
                return Convert.ToByte(ultimaEtapa.Ordem + 1);
            }
            else
            {
                return 0;
            }
        }

        public void Salvar(Etapa model)
        {
            repositorio.Salvar(model);
        }

        public void Salvar(List<Etapa> etapas)
        {
            repositorio.Salvar(etapas);
        }

        private void DuplicarPermissoes(Etapa etapaOriginal, Etapa etapaNova)
        {
            etapaNova.Permissoes = new List<EtapaPermissao>();

            foreach (var etapaPermissao in etapaOriginal.Permissoes)
            {
                var permissao = new EtapaPermissao
                {
                    Etapa = etapaNova,
                    Perfil = etapaPermissao.Perfil,
                    Usuario = etapaPermissao.Usuario,
                    NivelOcupacional = etapaPermissao.NivelOcupacional,
                    Uf = etapaPermissao.Uf,
                    Notificar = etapaPermissao.Notificar,
                    Analisar = etapaPermissao.Analisar,
                    ChefeImediato = etapaPermissao.ChefeImediato,
                    DiretorCorrespondente = etapaPermissao.DiretorCorrespondente,
                    GerenteAdjunto = etapaPermissao.GerenteAdjunto,
                    Solicitante = etapaPermissao.Solicitante
                };

                //repositorioEtapaPermissao.SalvarSemCommit(permissao);

                etapaNova.Permissoes.Add(permissao);
            }
        }


        public void Excluir(Etapa model)
        {
            repositorio.Excluir(model);
            this.AtualizarOrdemDasProximasEtapas(model);
        }

        private void AtualizarOrdemDasProximasEtapas(Etapa model)
        {
            var proximasEtapas = this.ObterTodosIQueryable().Where(x => x.Processo.ID == model.Processo.ID && x.Ordem > model.Ordem);

            foreach (var proximaEtapa in proximasEtapas)
            {
                proximaEtapa.Ordem -= 1;
                Salvar(proximaEtapa);
            }

        }

        public IQueryable<Etapa> ObterTodosIQueryable()
        {
            return repositorio.session.Query<Etapa>();
        }

        public Etapa DuplicarObjeto(Etapa etapaOriginal, bool concatenarCopiaAoNome, Processo processoNovo = null)
        {
            var etapaNova = new Etapa
            {
                OriginalID = etapaOriginal.ID,
                Nome = etapaOriginal.Nome + (concatenarCopiaAoNome ? " - Cópia" : ""),
                RequerAprovacao = etapaOriginal.RequerAprovacao,
                Ordem = etapaOriginal.Ordem,
                PrimeiraEtapa = etapaOriginal.PrimeiraEtapa,
                VisivelImpressao = etapaOriginal.VisivelImpressao,
                UsuarioAssinatura = etapaOriginal.UsuarioAssinatura,
                FileServer = etapaOriginal.FileServer != null
                    ? new FileServer
                    {
                        NomeDoArquivoOriginal = etapaOriginal.FileServer.NomeDoArquivoOriginal,
                        NomeDoArquivoNoServidor = etapaOriginal.FileServer.NomeDoArquivoNoServidor,
                        TipoArquivo = etapaOriginal.FileServer.TipoArquivo,
                        MediaServer = etapaOriginal.FileServer.MediaServer
                    }
                    : null,
                NomeFinalizacaoEtapa = etapaOriginal.NomeFinalizacaoEtapa,
                NomeBotaoFinalizacao = etapaOriginal.NomeBotaoFinalizacao,
                NomeReprovacaoEtapa = etapaOriginal.NomeReprovacaoEtapa,
                NomeBotaoReprovacao = etapaOriginal.NomeBotaoReprovacao,
                PodeSerAprovadoChefeGabinete = etapaOriginal.PodeSerAprovadoChefeGabinete,
                NotificaDiretorAnalise = etapaOriginal.NotificaDiretorAnalise,
                NotificarNucleo = etapaOriginal.NotificarNucleo,
                PrazoEncaminhamento = etapaOriginal.PrazoEncaminhamento,
                Processo = processoNovo ?? etapaOriginal.Processo,
                EtapaRetornoOriginalID = etapaOriginal.EtapaRetorno != null
                    ? (int?)etapaOriginal.EtapaRetorno.ID
                    : null
            };

            repositorio.SalvarSemCommit(etapaNova);

            var bmCampo = new BMCampo();

            DuplicarPermissoes(etapaOriginal, etapaNova);

            foreach (var campoOriginal in etapaOriginal.ListaCampos)
            {
                var campoNovo = DuplicarCampo(campoOriginal, etapaNova, bmCampo);

                etapaNova.ListaCampos.Add(campoNovo);
            }

            return etapaNova;
        }

        public void DuplicarSubDadosObjeto(Etapa etapa)
        {
            if (etapa.EtapaRetornoOriginalID.HasValue)
                etapa.EtapaRetorno =
                    etapa.Processo.ListaEtapas.FirstOrDefault(
                        x => x.OriginalID == etapa.EtapaRetornoOriginalID);

            repositorio.SalvarSemCommit(etapa);

            foreach (var campo in etapa.ListaCampos.Where(x => x.CamposVinculadosOriginaisIDs.Any()))
            {
                campo.ListaCamposVinculados =
                    etapa.ListaCampos.Where(
                        x =>
                            campo.CamposVinculadosOriginaisIDs.Contains(x.OriginalID)).ToList();
            }

            foreach (var campo in etapa.ListaCampos.Where(x => x.ListaAlternativas.Any(a => a.CampoVinculadoOriginalId.HasValue)))
            {
                var bmAlternativa = new BMAlternativa();

                foreach (
                    var alternativa in
                        campo.ListaAlternativas.Where(a => a.CampoVinculadoOriginalId.HasValue))
                {
                    alternativa.CampoVinculado =
                        etapa.ListaCampos.FirstOrDefault(
                            x =>
                                x.OriginalID == alternativa.CampoVinculadoOriginalId);

                    bmAlternativa.SalvarSemCommit(alternativa);
                }
            }
        }

        private Campo DuplicarCampo(Campo campoOriginal, Etapa etapa, BMCampo bmCampo)
        {
            var campoNovo = new Campo
            {
                OriginalID = campoOriginal.ID,

                Etapa = etapa,
                Nome = campoOriginal.Nome,
                Ordem = campoOriginal.Ordem,
                Tamanho = campoOriginal.Tamanho,
                TipoDado = campoOriginal.TipoDado,
                TipoCampo = campoOriginal.TipoCampo,
                PermiteNulo = campoOriginal.PermiteNulo,
                SomenteNumero = campoOriginal.SomenteNumero,
                SomenteLetra = campoOriginal.SomenteLetra,
                Largura = campoOriginal.Largura,
                ExibirImpressao = campoOriginal.ExibirImpressao,
                ExibirAjudaImpressao = campoOriginal.ExibirAjudaImpressao,
                CampoDivisor = campoOriginal.CampoDivisor,
                Ajuda = campoOriginal.Ajuda,
                Questionario = campoOriginal.Questionario,
                CamposVinculadosOriginaisIDs =
                    campoOriginal.ListaCamposVinculados.Select(c => new { c.ID }).Select(c => c.ID).ToList()
            };

            bmCampo.SalvarSemCommit(campoNovo);

            var bmAlternativa = new BMAlternativa();

            foreach (var alternativa in campoOriginal.ListaAlternativas)
            {
                var alternativaNova = new Alternativa
                {
                    Campo = campoNovo,
                    Nome = alternativa.Nome,
                    Ordem = alternativa.Ordem,
                    TipoCampo = alternativa.TipoCampo,
                    CampoVinculadoOriginalId =
                        alternativa.CampoVinculado != null ? (int?)alternativa.CampoVinculado.ID : null
                };

                bmAlternativa.SalvarSemCommit(alternativaNova);

                campoNovo.ListaAlternativas.Add(alternativaNova);
            }

            return campoNovo;
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public void FazerMerge(Etapa etapa)
        {
            repositorio.FazerMerge(etapa);
        }
    }
}
