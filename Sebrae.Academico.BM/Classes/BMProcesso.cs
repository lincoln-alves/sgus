using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Utils;
using NHibernate.Linq;
using NHibernate.Util;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using Antlr.Runtime;

namespace Sebrae.Academico.BM.Classes
{
    public class BMProcesso : BusinessManagerBase, IDisposable
    {
        #region Atributos

        private RepositorioBase<Processo> repositorio;
        private RepositorioBase<Etapa> repositorioEtapa;
        private RepositorioBase<EtapaPermissao> repositorioEtapaPermissao;
        private RepositorioBase<ProcessoResposta> repositorioProcessoResposta;

        #endregion

        #region "Construtor"

        public BMProcesso()
        {
            repositorio = new RepositorioBase<Processo>();
            repositorioEtapaPermissao = new RepositorioBase<EtapaPermissao>();
            repositorioEtapa = new RepositorioBase<Etapa>();
            repositorioProcessoResposta = new RepositorioBase<ProcessoResposta>();
        }

        #endregion

        public IList<Processo> ObterTodos()
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<Processo>();
            return query.ToList();
        }

        public IQueryable<Processo> ObterTodosIQueryable()
        {
            return repositorio.session.Query<Processo>();
        }

        public Processo ObterPorId(int pId)
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<Processo>();
            return query.FirstOrDefault(x => x.ID == pId);
        }

        public Etapa ObterProximaEtapaNaoRespondida(int idProcesso, int IdProcessoResposta = 0)
        {
            // Obterm próxima etapa do versionamento de etapas
            if (IdProcessoResposta > 0)
            {
                var processoResposta = repositorio.session.Query<ProcessoResposta>().FirstOrDefault(x => x.ID == IdProcessoResposta);
                var queryVersao = repositorio.session.Query<EtapaVersao>().Where(x => processoResposta != null && processoResposta.VersaoEtapa != null &&
                x.Etapa.Processo.ID == processoResposta.Processo.ID && x.Versao == processoResposta.VersaoEtapa);

                if (queryVersao != null && queryVersao.Any())
                {
                    var etapas = queryVersao.OrderBy(x => x.Ordem).Select(x => x.Etapa);
                    var result = etapas.Where(d => d.Processo.ID == idProcesso && !d.ListaEtapaResposta.Any(f => f.ProcessoResposta.ID == IdProcessoResposta && f.Ativo)).FirstOrDefault();
                    return result;
                }
            }

            var query = repositorio.session.Query<Etapa>();

            return
                query.Where(
                    d =>
                        d.Processo.ID == idProcesso &&
                        !d.ListaEtapaResposta.Any(f => f.ProcessoResposta.ID == IdProcessoResposta && f.Ativo))
                    .OrderBy(g => g.Ordem)
                    .FirstOrDefault();
        }

        public IList<Processo> ObterPorFiltro(Processo model)
        {
            var query = repositorio.session.Query<Processo>();

            if (!string.IsNullOrEmpty(model.Nome))
                query = query.Where(x => x.Nome.Contains(model.Nome));

            if (model.Uf != null)
                query = query.Where(x => x.Uf.ID == model.Uf.ID);

            if (model.Status != null)
                query = query.Where(x => x.Ativo == model.Status);

            return query.OrderBy(q => q.Nome).ToList<Processo>();
        }

        public bool PossuiEtapas(int idProcesso)
        {
            return repositorio.session.Query<Etapa>().Any(e => e.Processo.ID == idProcesso);
        }

        public void Salvar(Processo model)
        {

            //demanda #3587
            var obter = ObterPorFiltro(model).FirstOrDefault(p => p.ID != model.ID);
            if (obter != null)
            {
                throw new AcademicoException("Já existe no banco de dados um registro com esse nome.");
            }
            //fim demanda #3587

            repositorio.Salvar(model);

        }

        public IList<DTOHistoricoProcessos> BuscarHistorico(int? idProcesso, Usuario usuario, DateTime? dataInicio,
            DateTime? dataFim, int idProcessoResposta = 0, Uf uf = null, List<int> ufs = null, int idEtapa = 0)
        {
            var query = repositorio.session.Query<ProcessoResposta>();

            if (idProcesso.HasValue)
                if (idProcesso.Value != 0) query = query.Where(q => q.Processo.ID == idProcesso.Value);

            if (usuario != null)
            {
                query = query.Where(q => q.Usuario.ID == usuario.ID);
            }

            if (dataInicio.HasValue)
                query = query.Where(q => q.DataAlteracao >= dataInicio.Value);

            if (dataFim.HasValue)
                query = query.Where(q => q.DataAlteracao <= dataFim.Value.AddDays(1).AddMinutes(-1));

            if (idProcessoResposta > 0)
                query = query.Where(q => q.ID == idProcessoResposta);

            if(idEtapa > 0)
            {
                query = query.Where(x => x.ListaEtapaResposta.OrderByDescending(e => e.ID).Select(y => y.Etapa.ID).FirstOrDefault() == idEtapa);
            }

            if (uf != null)
                query = query.Where(x => x.Processo.Uf.ID == uf.ID);

            if (ufs.Any())
            {
                query = query.Where(x => x.Processo.Uf != null && ufs.Contains(x.Processo.Uf.ID));
            }

            return query.Select(q => new DTOHistoricoProcessos()
            {
                IdProcessoResposta = q.ID,
                Processo = q.Processo.Nome,
                EtapaConcluida =
                    q.ListaEtapaResposta.OrderByDescending(e => e.ID).Select(e => e.Etapa.Nome).FirstOrDefault(),
                UsuarioDemandante = q.Usuario.Nome,
                DataAbertura = q.DataSolicitacao,
                Status = q.Status,
                DataAlteracao = q.ListaEtapaResposta.Where(x => x.DataAlteracao.HasValue).Select(e => e.DataAlteracao).OrderByDescending(x => x).FirstOrDefault()
            }).OrderByDescending(q => q.IdProcessoResposta).ToList();

        }

        public void Excluir(Processo model)
        {
            repositorio.Excluir(model);
        }

        public void DuplicarObjeto(int idProcesso)
        {
            using (var transaction = repositorio.ObterTransacao())
            {
                try
                {
                    var processoOriginal = ObterPorId(idProcesso);

                    if (processoOriginal != null)
                    {
                        var processoNovo = new Processo
                        {
                            ID = 0,
                            Nome = processoOriginal.Nome + " - Cópia",
                            Ativo = processoOriginal.Ativo,
                            ListaProcessoResposta = null,
                            ListaEtapas = new List<Etapa>(),
                            Uf = processoOriginal.Uf,
                            Tipo = processoOriginal.Tipo
                        };

                        repositorio.SalvarSemCommit(processoNovo);

                        var bmEtapa = new BMEtapa();

                        foreach (var etapaOriginal in processoOriginal.ListaEtapas)
                        {
                            processoNovo.ListaEtapas.Add(bmEtapa.DuplicarObjeto(etapaOriginal, false, processoNovo));
                        }

                        // Reloopar para obter os dados relacionados.
                        foreach (var etapa in processoNovo.ListaEtapas)
                        {
                            bmEtapa.DuplicarSubDadosObjeto(etapa);
                        }

                        transaction.Commit();
                    }
                }
                catch (Exception)
                {
                    transaction.Rollback();

                    throw;
                }
            }
        }

        public void ExcluirProcessoEtapasPermissoes(int IdProcesso)
        {
            var processo = repositorio.ObterPorID(IdProcesso);

            if (processo != null)
            {
                foreach (var etapa in processo.ListaEtapas)
                {
                    foreach (var permissao in etapa.Permissoes)
                    {
                        repositorioEtapaPermissao.Excluir(permissao);
                    }

                    repositorioEtapa.Excluir(etapa);
                }

                repositorio.Excluir(processo);
            }
        }

        public ProcessoResposta ObterProcessoResposta(int id)
        {
            return repositorioProcessoResposta.ObterPorID(id);
        }

        public void SalvarProcessoResposta(ProcessoResposta processo)
        {
            repositorioProcessoResposta.Salvar(processo);
        }

        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

        public List<EtapaResposta> ConsultarEtapasAAnalisar(Usuario usuario, int? numero, int? demandanteId, int? processoId, int? etapaId,
            List<Cargo> cargos = null, bool somenteEtapasComUmAnalista = false)
        {
            var diretores = new List<UsuarioCargo>();
            var cargoUsuario = usuario.ObterCargo();
            if (cargoUsuario != null)
            {
                // Buscar todos os diretores, pela UF do UsuarioCargo.
                diretores = new BMUsuarioCargo().ObterPorTipoCargo(EnumTipoCargo.Diretoria).Where(x => x.Cargo.Uf.ID == cargoUsuario.Cargo.Uf.ID).ToList();
            }

            var bmProcessoResposta = new BMProcessoResposta();
            var bmEtapaResposta = new BMEtapaResposta();
            var bmUsuario = new BMUsuario();
            var bmEtapaPermissao = new BMEtapaPermissao();

            var idsEtapasRespostasAbertas = bmEtapaResposta.ObterTodosIQueryable()
                .Where(x => x.Ativo)
                .Where(x => x.ProcessoResposta.Status != 0)
                .Where(x => x.Status == (int)enumStatusEtapaResposta.Aguardando)
                .Where(x => x.DataPreenchimento == null)
                .Select(x => x.ID).ToList();

            var idsProcessosRespostasAbertos =
               bmProcessoResposta.ObterTodosIQueryable()
                   .Where(x => x.ListaEtapaResposta.AsQueryable()
                            .Select(er => new { er.ID })
                            //.Where(er => idsEtapasRespostasAbertas.Contains(er.ID)).Count() > 0
                            .Any(er => idsEtapasRespostasAbertas.Contains(er.ID))
                         );

            var ids = idsProcessosRespostasAbertos.Select(x => x.ID).AsEnumerable().Take(10000);

            var demandantes = bmUsuario.ObterTodosIQueryable()
                             .Where(x => x.ListaProcessoResposta.Any(pr => idsProcessosRespostasAbertos.Select(a => a.ID).Contains(pr.ID)))//.ToList()
                             .Select(x => new
                             {
                                 Usuario = new Usuario
                                 {
                                     ID = x.ID,
                                     Nome = x.Nome,
                                     Email = x.Email,
                                     CPF = x.CPF,
                                     Unidade = x.Unidade,
                                     UF = x.UF,
                                     ListaUsuarioCargo = x.ListaUsuarioCargo
                                 },
                                 Processos = x.ListaProcessoResposta.Select(y => y.ID)
                                             .Where(prId => ids.Contains(prId)).ToList()
                             }).ToList();

            var etapasRespostasAnalistas =
                bmEtapaResposta.ObterTodosIQueryable()
                    .Where(x => ids.Contains(x.ProcessoResposta.ID))
                    .Select(x => new EtapaResposta
                    {
                        ID = x.ID,
                        ProcessoResposta = new ProcessoResposta
                        {
                            ID = x.ProcessoResposta.ID
                        },
                        Analista = x.Analista != null
                            ? new Usuario
                            {
                                ID = x.Analista.ID
                            }
                            : null
                    }).ToList();

            foreach (var processoResposta in idsProcessosRespostasAbertos)
            {
                processoResposta.Usuario =
                    demandantes.FirstOrDefault(d => d.Processos.Contains(processoResposta.ID))?.Usuario;

                processoResposta.ListaEtapaResposta =
                    etapasRespostasAnalistas.Where(x => x.ProcessoResposta.ID == processoResposta.ID).ToList();
            }

            var etapasRespostas = bmEtapaResposta.ObterTodosIQueryable()
                .Where(x => idsEtapasRespostasAbertas.Contains(x.ID))
                .Select(er => new EtapaResposta
                {
                    ID = er.ID,
                    DataPreenchimento = er.DataPreenchimento,
                    DataAlteracao = er.DataAlteracao,
                    PrazoEncaminhamento = er.PrazoEncaminhamento,                    
                    PermissoesNucleoEtapaResposta = er.PermissoesNucleoEtapaResposta,
                    ListaEtapaEncaminhamentoUsuario = er.ListaEtapaEncaminhamentoUsuario
                }).ToList();

            var etapas = new BMEtapa().ObterTodosIQueryable()
                .Where(x => x.ListaEtapaResposta.Any(er => idsEtapasRespostasAbertas.Contains(er.ID)))
                .Select(x => new Etapa
                {
                    ID = x.ID,
                    PodeSerAprovadoChefeGabinete = x.PodeSerAprovadoChefeGabinete,
                    Nome = x.Nome,
                    ListaEtapaResposta = x.ListaEtapaResposta.Select(er => new EtapaResposta { ID = er.ID }).ToList()
                }).ToList();

            var etapasIds = etapas.Select(x => x.ID).ToList();

            var permissoes =
                bmEtapaPermissao.ObterTodosIQueryable().Where(x => etapasIds.Contains(x.Etapa.ID))
                    .Select(x => new EtapaPermissao
                    {
                        ID = x.ID,
                        Notificar = x.Notificar,
                        Analisar = x.Analisar,
                        ChefeImediato = x.ChefeImediato,
                        DiretorCorrespondente = x.DiretorCorrespondente,
                        GerenteAdjunto = x.GerenteAdjunto,
                        Solicitante = x.Solicitante,
                        Etapa = new Etapa
                        {
                            ID = x.Etapa.ID
                        }
                    })
            .ToList();

            var idsPermissoes = permissoes.Select(x => x.ID).ToList();

            // Quebra a lista de permissões para não supera o limite do LINQ de 2100 registros em um IN
            var idPremissoesGroupList = splitList(idsPermissoes, 2000);

            foreach (var idPremissoesGroup in idPremissoesGroupList)
            {
                var usuariosPermissoes = bmUsuario.ObterTodosIQueryable()
                    .Where(x => x.ListaEtapaPermissao.Any(p => idPremissoesGroup.Contains(p.ID)))
                    .Select(x => new Usuario { ID = x.ID })
                    .ToList()
                    .Select(x => new
                    {
                        Usuario = x,
                        Permissoes =
                            bmEtapaPermissao.ObterTodosIQueryable()
                                .Where(p => etapasIds.Contains(p.Etapa.ID) && p.Usuario != null && p.Usuario.ID == x.ID)
                                .Select(p => new { p.ID })
                                .Select(p => p.ID)
                                .ToList()
                    }).ToList();

                foreach (var permissao in permissoes)
                {
                    permissao.Usuario =
                        usuariosPermissoes.FirstOrDefault(x => x.Permissoes.Any(pId => pId == permissao.ID))?.Usuario;
                }
            }

            var vinculoEtapaComEtapaResposta =
                bmEtapaResposta.ObterTodosIEnumerable().AsQueryable()
                    .Where(x => etapasIds.Contains(x.Etapa.ID))
                    .Select(x => new { EtapaId = x.Etapa.ID, EtapaRespostaId = x.ID })
                    .ToList();

            foreach (var etapa in etapas)
            {
                etapa.Permissoes = permissoes.Where(p => p.Etapa.ID == etapa.ID).ToList();

                etapa.ListaEtapaResposta =
                    vinculoEtapaComEtapaResposta.Where(x => x.EtapaId == etapa.ID)
                        .Select(x => new EtapaResposta { ID = x.EtapaRespostaId }).ToList();
            }

            var idsProcessosRespostasAbertos2 = idsProcessosRespostasAbertos.ToList();

            foreach (var etapaResposta in etapasRespostas)
            {
                etapaResposta.ProcessoResposta =
                    idsProcessosRespostasAbertos2.FirstOrDefault(pr => pr.ListaEtapaResposta.Any(er => er.ID == etapaResposta.ID));

                etapaResposta.Etapa =
                    etapas.AsEnumerable().FirstOrDefault(
                        x =>
                            x.ListaEtapaResposta
                                .Select(er => new { er.ID }).AsEnumerable()
                                .ToList()
                                .Any(er => er.ID == etapaResposta.ID));
            }

            var retorno = etapasRespostas.Where(er => UsuarioPodeAnalisar(diretores, er, numero, demandanteId,
                processoId, etapaId, usuario, cargos, somenteEtapasComUmAnalista)).ToList();

            //CASO ETAPA ENCAMINHADA PELO USUARIO ESTIVER PENDENTE, A ETAPA NAO EXIBE PARA O USUARIO
            retorno = retorno.Where(x => x.ListaEtapaEncaminhamentoUsuario != null && x.ListaEtapaEncaminhamentoUsuario.Select(y => y.UsuarioEncaminhamento.ID).Contains(usuario.ID) ?
                        x.ListaEtapaEncaminhamentoUsuario.Any(z => z.UsuarioEncaminhamento.ID == usuario.ID && z.StatusEncaminhamento != (int)enumStatusEncaminhamentoEtapa.Aguardando)
                        : true).ToList();

            //CASO ETAPA ENCAMINHADA PELO USUARIO NAO ESTIVER PENDENTE, A ETAPA NAO EXIBE PARA O USUARIO
            retorno = retorno.Where(x => x.ListaEtapaEncaminhamentoUsuario != null && x.ListaEtapaEncaminhamentoUsuario.Select(y => y.EtapaPermissaoNucleo.HierarquiaNucleoUsuario.Usuario.ID).Contains(usuario.ID) ?
                        x.ListaEtapaEncaminhamentoUsuario.Any(z => z.EtapaPermissaoNucleo.HierarquiaNucleoUsuario.Usuario.ID == usuario.ID && z.StatusEncaminhamento != (int)enumStatusEncaminhamentoEtapa.Negado)
                        : true).ToList();

            return retorno;
        }

        public bool UsuarioPodeAnalisar(List<UsuarioCargo> diretores, EtapaResposta er, int? numero, int? demandanteId,
            int? processoId, int? etapaId, Usuario usuario, List<Cargo> cargos = null,
            bool somenteEtapasComUmAnalista = false)
        {   

            return
                ((numero == null || er.ProcessoResposta.ID == numero) &&
                (demandanteId == null || er.ProcessoResposta.Usuario.ID == demandanteId) &&
                (processoId == null || er.ProcessoResposta.Processo.ID == processoId) &&
                (etapaId == null || er.Etapa.ID == etapaId) &&
                (!usuario.IsGestor() || er.ProcessoResposta.Processo.Uf.ID == usuario.UF.ID) &&                
                // Aqui que filtra se o usuário logado está na lista de analistas da etapa.
                UsuarioEstaNaListaDeAnalistas(er.ObterAnalistas(diretores, usuario, cargos), usuario,
                    somenteEtapasComUmAnalista));
        }

        private bool UsuarioEstaNaListaDeAnalistas(List<Usuario> usuarios, Usuario analista, bool somenteEtapasComUmAnalista = false)
        {
            // Filtra para retornar apenas etapas que possuem somente um analista.
            if (somenteEtapasComUmAnalista)
            {
                return usuarios.Count() == 1 && usuarios.Select(x => x.ID).Contains(analista.ID);
            }
            return usuarios.Select(x => x.ID).Contains(analista.ID);            
        }

        public IEnumerable<EtapaResposta> ConsultarEtapasConcluidas(Usuario usuario, int? numero, int? demandanteId, int? processoId, int? etapaId)
        {
            // Obter todas as últimas etapas de todos os processos.
            var ultimasEtapasRespostas = (
                from pr in repositorioProcessoResposta.ObterTodosIQueryable()
                where pr.ListaEtapaResposta.Any() && pr.Usuario.ID == usuario.ID &&
                      (pr.Status == enumStatusProcessoResposta.Cancelado || pr.Concluido == true)
                select
                    pr.ListaEtapaResposta.OrderByDescending(er => er.DataPreenchimento ?? er.DataAlteracao)
                        .FirstOrDefault())
                .AsEnumerable();

            var etapas =
                from etapaResposta in ultimasEtapasRespostas
                where
                    (numero == null || etapaResposta.ProcessoResposta.ID == numero) &&
                    (demandanteId == null || etapaResposta.ProcessoResposta.Usuario.ID == demandanteId) &&
                    (processoId == null || etapaResposta.ProcessoResposta.Processo.ID == processoId) &&
                    (etapaId == null || etapaResposta.Etapa.ID == etapaId)
                select etapaResposta;

            return etapas.OrderByDescending(x => x.ProcessoResposta.DataAlteracao);
        }

        public IEnumerable<EtapaResposta> ConsultarEtapasEmAndamento(Usuario usuario, int? numero, int? demandanteId, int? processoId)
        {
            // Obter todas as últimas etapas de todos os processos.
            var ultimasEtapasRespostas = (
                from pr in repositorioProcessoResposta.ObterTodosIQueryable()
                where pr.ListaEtapaResposta.Any() && pr.Usuario.ID == usuario.ID &&
                      (pr.Status == enumStatusProcessoResposta.Ativo && pr.Concluido == false)
                select
                    pr.ListaEtapaResposta.OrderByDescending(er => er.DataPreenchimento ?? er.DataAlteracao)
                        .FirstOrDefault())
                .AsEnumerable();

            List<UsuarioCargo> diretores = new List<UsuarioCargo>();;
            var cargoUsuario = usuario.ObterCargo();
            if (cargoUsuario != null)
            {
                // Buscar todos os diretores, pela UF do UsuarioCargo.
                diretores = new BMUsuarioCargo().ObterPorTipoCargo(EnumTipoCargo.Diretoria).Where(x => x.Cargo.Uf.ID == cargoUsuario.Cargo.Uf.ID).ToList();
            }

            // Usar LINQ sem lambda para eficiência máxima da query.
            var etapas =
                from etapaResposta in ultimasEtapasRespostas
                where
                    (numero == null || etapaResposta.ProcessoResposta.ID == numero) &&
                    (demandanteId == null || etapaResposta.ProcessoResposta.Usuario.ID == demandanteId) &&
                    (processoId == null || etapaResposta.ProcessoResposta.Processo.ID == processoId)

                select etapaResposta;

            return etapas;
        }

        public List<ProcessoResposta> ConsultarEtapasAnalisada(Usuario usuario, int? numero,
            int? demandanteId, int? processoId)
        {
            var idsProcessoRespostas = repositorio.session.Query<EtapaResposta>()
                .Fetch(x => x.Etapa)
                .Fetch(x => x.ProcessoResposta)
                .Fetch(x => x.Analista)
                .Where(x =>
                    x.Status != (int)enumStatusEtapaResposta.Aguardando &&
                    x.Etapa != null && x.Etapa.PrimeiraEtapa == false &&
                    x.ProcessoResposta.Concluido == false &&
                    x.Analista != null &&
                    x.Analista.ID == usuario.ID &&
                    (numero == null || x.ProcessoResposta.ID == numero) &&
                    (demandanteId == null || x.ProcessoResposta.Usuario.ID == demandanteId) &&
                    (processoId == null || x.ProcessoResposta.Processo.ID == processoId)
                ).ToList()
                .Select(x => x.ProcessoResposta.ID)
                .Distinct().ToList();

            var processosRespostas = repositorio.session.Query<ProcessoResposta>()
                .Where(x => idsProcessoRespostas.Contains(x.ID)).ToList();

            var processos = new BMProcessoResposta().ObterTodosIQueryable()
                .Select(x => new
                {
                    Processo = new Processo
                    {
                        ID = x.Processo.ID,
                        Nome = x.Processo.Nome
                        // TODO: Outros campos do Procesos que precisas.
                    },
                    Usuario = new Usuario
                    {
                        ID = x.Usuario.ID,
                        Nome = x.Usuario.Nome,
                        Unidade = x.Usuario.Unidade,
                        CPF = x.Usuario.CPF
                    },
                    IdProcessoResposta = x.ID
                })
                .Where(x => idsProcessoRespostas.Contains(x.IdProcessoResposta))
                .ToList();

            foreach(var processoResposta in processosRespostas)
            {
                var dado = processos.FirstOrDefault(x => x.IdProcessoResposta == processoResposta.ID);

                processoResposta.Processo = dado.Processo;
                processoResposta.Usuario = dado.Usuario;
            }

            return processosRespostas;
        }

        public static List<List<int>> splitList(List<int> locations, int nSize = 30)
        {
            var list = new List<List<int>>();

            for (int i = 0; i < locations.Count; i += nSize)
            {
                list.Add(locations.GetRange(i, Math.Min(nSize, locations.Count - i)));
            }

            return list;
        }
    }
}