using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;
using FluentNHibernate.Conventions;
using NHibernate.Criterion;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BM.Classes
{
    public class BMPermissao : BusinessManagerBase, IDisposable
    {
        #region Atributos

        private RepositorioBase<EtapaPermissao> repositorio;

        #endregion

        #region "Construtor"

        public BMPermissao()
        {
            repositorio = new RepositorioBase<EtapaPermissao>();
        }

        #endregion

        public IList<EtapaPermissao> ObterTodos()
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<EtapaPermissao>();
            return query.ToList<EtapaPermissao>();
        }

        public EtapaPermissao ObterPorId(int pId)
        {
            //return repositorio.GetByProperty("Nome", pNome).FirstOrDefault();
            var query = repositorio.session.Query<EtapaPermissao>();
            return query.FirstOrDefault(x => x.ID == pId);
        }

        public IList<EtapaPermissao> ObterPorFiltro(EtapaPermissao modulo)
        {
            var query = repositorio.session.Query<EtapaPermissao>();

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

            return query.ToList<EtapaPermissao>();
        }

        /// <summary>
        /// Retorna todos os usuários configurados para analisar uma etapa
        /// </summary>
        public IList<Usuario> ObterUsuariosAAnalisar(int idEtapaResposta, bool possuiChefeDeGabineteAnalista)
        {
            return ObterUsuariosDasPermissoes(idEtapaResposta, true, false, possuiChefeDeGabineteAnalista);
        }

        /// <summary>
        /// Retorna todos os usuários configurados para receber as notificações de uma etapa
        /// </summary>
        public IList<Usuario> ObterUsuariosANotificar(int idEtapaResposta, bool possuiAssessorAnalista)
        {
            return ObterUsuariosDasPermissoes(idEtapaResposta, false, true, possuiAssessorAnalista);
        }

        public IList<Usuario> ObterUsuariosDasPermissoes(int idEtapaResposta, bool analisar, bool notificar, bool possuiChefeDeGabineteAnalista)
        {
            var etapaResposta = repositorio.session.Query<EtapaResposta>().First(c => c.ID == idEtapaResposta);

            var queryPermissoes = repositorio.session
                .Query<EtapaPermissao>()
                .Where(p => p.Etapa.ID == etapaResposta.Etapa.ID &&
                    (!analisar || p.Analisar == true) &&
                    (!notificar || p.Notificar == true));

            var demandante = etapaResposta.ProcessoResposta.Usuario;


            return ObterUsuariosPermissoes(demandante, queryPermissoes, possuiChefeDeGabineteAnalista, analisar);
        }

        public IList<Usuario> ObterUsuariosDasPermissoesPorEtapa(int idEtapa, Usuario demandante, bool analisar, bool notificar, bool possuiAssessorAnalista)
        {
            var queryPermissoes = repositorio.session
                .Query<EtapaPermissao>()
                .Where(p => p.Etapa.ID == idEtapa &&
                    (!analisar || p.Analisar == true) &&
                    (!notificar || p.Notificar == true));

            return ObterUsuariosPermissoes(demandante, queryPermissoes, possuiAssessorAnalista, analisar);
        }

        private IList<Usuario> ObterUsuariosPermissoes(Usuario demandante, IQueryable<EtapaPermissao> queryPermissoes, bool possuiChefeDeGabineteAnalista, bool analisar)
        {
            var listaUsuarios = queryPermissoes.Where(p => p.Usuario != null).Select(p => p.Usuario).ToList();

            var temPermissaoChefe = queryPermissoes.Any(p => p.ChefeImediato == true);

            var bmPermissao = new BMPermissao();

            if (temPermissaoChefe)
            {
                // Adicionar chefes imediatos.
                listaUsuarios.AddRange(bmPermissao.ObterChefeImediato(demandante.Email));
            }

            var temGerenteAdjunto = queryPermissoes.Any(p => p.GerenteAdjunto == true);

            if (temGerenteAdjunto)
            {
                // Adicionar gerentes adjuntos.
                var gerentesAdjuntos = bmPermissao.ObterGerentesAdjuntos(demandante.Email);

                if (gerentesAdjuntos != null)
                    listaUsuarios.AddRange(gerentesAdjuntos);
            }

            var temPermissaoDiretor = queryPermissoes.Any(p => p.DiretorCorrespondente == true);

            if (temPermissaoDiretor)
            {
                List<Usuario> diretores = new List<Usuario>();

                diretores = bmPermissao.ObterDiretoresCorrespondente(demandante.Email);

                // Atenção: Diretor só deve ser notificado na análise se checado o NotificaDiretorAnalise, SE for notificação avisa sempre
                if (!analisar || (analisar && queryPermissoes.First().Etapa.NotificaDiretorAnalise))
                {
                    if (diretores.Count > 0)
                        listaUsuarios.AddRange(diretores);
                }

                // A opção de notificar acessores só é possível atualmente para os analistas
                if (analisar && possuiChefeDeGabineteAnalista && diretores.Count > 0)
                {
                    var assessoresAnalistas = bmPermissao.ObterAssessoresAnalistas(diretores);

                    listaUsuarios.AddRange(assessoresAnalistas);
                }

            }

            // Adiciona o solitante se ele deve ser notificado
            var notificaSolicitante = queryPermissoes.Any(p => p.Solicitante == true);
            if (notificaSolicitante && demandante != null)
            {
                listaUsuarios.Add(demandante);
            }

            return listaUsuarios.Where(x => x != null).Distinct().ToList();
        }

        public List<Usuario> ObterChefeImediato(string email)
        {
            var retorno = new List<Usuario>();

            if (string.IsNullOrWhiteSpace(email))
                return retorno;

            var hierarquia = repositorio.session.Query<Hierarquia>().FirstOrDefault(x => x.Email == email);

            if (hierarquia != null)
            {
                // CodUnidade maior que 5 vai até o Chefe de Gabinete.
                if (hierarquia.CodUnidade.Length > 5)
                {
                    // Funcionário.
                    if (hierarquia.CargoFuncionario == null)
                    {
                        retorno.Add(ObterChefeImediatoDoFuncionario(hierarquia));
                    }
                    // Gerente adjunto.
                    else if (hierarquia.CargoFuncionario.ToLower().Contains("gerente adj") || hierarquia.CargoFuncionario.ToLower().Contains("secretário-geral"))
                    {
                        retorno.Add(ObterChefeImediatoDoGerenteAdjunto(hierarquia));
                    }
                    // Chefe imediato.                    
                    else if (hierarquia.CargoFuncionario.ToLower().Contains("gerente") || hierarquia.CargoFuncionario.ToLower().Contains("assessor") || hierarquia.CargoFuncionario.ToLower().Contains("secretário-geral"))
                    {
                        retorno.Add(ObterChefeImediatoDoGerente(hierarquia));

                        var bmPermissao = new BMPermissao();
                        var diretores = bmPermissao.ObterDiretoresCorrespondente(hierarquia.Email);
                        var assessoresAnalistas = bmPermissao.ObterAssessoresAnalistas(diretores);
                        retorno.AddRange(assessoresAnalistas);
                        assessoresAnalistas = bmPermissao.ObterAssessoresAnalistas(new List<Usuario> { (new BMUsuario()).ObterPorEmail(hierarquia.Email) });
                        retorno.AddRange(assessoresAnalistas);
                    }
                    else if (hierarquia.CargoFuncionario.ToLower().Contains("chefe"))
                    {
                        retorno.Add(ObterChefeImediatoDoChefeDeGabinete(hierarquia));
                    }
                }
                else
                {
                    // Caso seja diretor, obtém os outros diretores como "chefes imediatos".
                    if (hierarquia.CodUnidade.Length == 5)
                    {
                        var outrosDiretores =
                            repositorio.session.Query<Hierarquia>()
                                .Where(x => x.CodUnidade.Length == 5 && x.CodPessoa != hierarquia.CodPessoa);

                        var bmUsuario = new BMUsuario();

                        foreach (var diretor in outrosDiretores)
                        {
                            retorno.Add(bmUsuario.ObterPorEmail(diretor.Email));
                        }
                    }
                }
            }

            return retorno.Where(x => x != null).ToList();
        }

        private Usuario ObterChefeImediatoDoFuncionario(Hierarquia demandante)
        {
            // Alteração #2723 - Para pegar variações dos cargos de funcionario de Gerente Adj
            var hierarquiaChefe =
                repositorio.session.Query<Hierarquia>()
                    .FirstOrDefault(
                        x => x.CodUnidade == demandante.CodUnidade && (x.CargoFuncionario.ToLower().Contains("gerente adj") || x.CargoFuncionario.ToLower().Contains("secretário-geral adj")));

            return hierarquiaChefe == null ? null : new BMUsuario().ObterPorEmail(hierarquiaChefe.Email);
        }

        private Usuario ObterChefeImediatoDoGerenteAdjunto(Hierarquia demandante)
        {
            // Alteração #2723 - Para pegar variações dos cargos de funcionario de Gerente
            var hierarquiaChefe =
                repositorio.session.Query<Hierarquia>()
                    .FirstOrDefault(
                        x => x.CodUnidade == demandante.CodUnidade && (!x.CargoFuncionario.ToLower().Contains("gerente adj") && x.CargoFuncionario.ToLower().Contains("gerente") ||
                        (!x.CargoFuncionario.ToLower().Contains("secretário-geral adj") && x.CargoFuncionario.ToLower().Contains("secretário-geral"))));

            return hierarquiaChefe == null ? null : new BMUsuario().ObterPorEmail(hierarquiaChefe.Email);
        }

        private Usuario ObterChefeImediatoDoGerente(Hierarquia demandante)
        {
            // Alteração #2723 - Para pegar variações dos cargos de funcionario de Gerente
            var hierarquiaChefe =
                repositorio.session.Query<Hierarquia>()
                    .FirstOrDefault(
                        x => x.CodUnidade.Substring(0, 5) == demandante.CodUnidade.Substring(0, 5) && x.CargoFuncionario.ToLower().Contains("chefe"));

            return hierarquiaChefe == null ? null : new BMUsuario().ObterPorEmail(hierarquiaChefe.Email);
        }

        private Usuario ObterChefeImediatoDoChefeDeGabinete(Hierarquia demandante)
        {
            var hierarquiaChefe =
                repositorio.session.Query<Hierarquia>()
                    .FirstOrDefault(
                        x => x.CodUnidade == demandante.CodUnidade.Substring(0, 5));

            return hierarquiaChefe == null ? null : new BMUsuario().ObterPorEmail(hierarquiaChefe.Email);
        }

        public List<Usuario> ObterGerentesAdjuntos(string email)
        {
            var retorno = new List<Usuario>();

            if (string.IsNullOrWhiteSpace(email))
                return retorno;

            var hierarquia = repositorio.session.Query<Hierarquia>().FirstOrDefault(x => x.Email == email);

            if (hierarquia != null)
            {
                if (hierarquia.CodUnidade.Length > 5 && !(hierarquia.CargoFuncionario != null && hierarquia.CargoFuncionario.ToLower().Contains("chefe")))
                {
                    // Funcionário.
                    if (hierarquia.CargoFuncionario == null)
                    {
                        retorno.Add(ObterGerenteAdjuntoDoFuncionario(hierarquia));
                        // Gerente Adjunto
                    }
                    else if (hierarquia.CargoFuncionario.ToLower().Contains("gerente adj"))
                    {
                        retorno.Add(ObterGerenteAdjuntoDoGerenteAdjunto(hierarquia));
                    }
                    // Chefe imediato.
                    else if (hierarquia.CargoFuncionario.ToLower().Contains("gerente"))
                    {
                        retorno.Add(ObterGerenteAdjuntoDoGerente(hierarquia));
                    }
                }
                else
                {
                    // Chefe de gabinete.
                    retorno.AddRange(ObterGerentesAdjuntosDoChefeDeGabinete(hierarquia));
                }
            }

            return retorno.Where(x => x != null).ToList();
        }

        private Usuario ObterGerenteAdjuntoDoFuncionario(Hierarquia demandante)
        {
            var hierarquiaChefe =
                repositorio.session.Query<Hierarquia>()
                    .FirstOrDefault(
                        x => x.CodUnidade == demandante.CodUnidade && (x.CargoFuncionario.ToLower().Contains("gerente") && !x.CargoFuncionario.ToLower().Contains("gerente adj")));

            return hierarquiaChefe == null ? null : new BMUsuario().ObterPorEmail(hierarquiaChefe.Email);
        }

        private Usuario ObterGerenteAdjuntoDoGerenteAdjunto(Hierarquia demandante)
        {
            var hierarquiaChefe =
                repositorio.session.Query<Hierarquia>()
                    .FirstOrDefault(
                        x => x.CodUnidade.Substring(0, 5) == demandante.CodUnidade.Substring(0, 5) && x.CargoFuncionario.ToLower().Contains("chefe"));

            return hierarquiaChefe == null ? null : new BMUsuario().ObterPorEmail(hierarquiaChefe.Email);
        }

        private Usuario ObterGerenteAdjuntoDoGerente(Hierarquia demandante)
        {
            var hierarquiaChefe =
                repositorio.session.Query<Hierarquia>()
                    .FirstOrDefault(
                        x => x.CodUnidade.Length == 5 && x.CodUnidade == demandante.CodUnidade.Substring(0, 5));

            return hierarquiaChefe == null ? null : new BMUsuario().ObterPorEmail(hierarquiaChefe.Email);
        }

        private List<Usuario> ObterGerentesAdjuntosDoChefeDeGabinete(Hierarquia demandante)
        {
            // Somente diretores podem analisar processos de Chefes de Gabinetes
            var hierarquiasDiretores = repositorio.session.Query<Hierarquia>().Where(x => x.CodUnidade.Length == 5 && x.CodPessoa != demandante.CodPessoa);

            var retorno = new List<Usuario>();

            var bmUsuario = new BMUsuario();

            foreach (var diretor in hierarquiasDiretores)
            {
                retorno.Add(bmUsuario.ObterPorEmail(diretor.Email));
            }

            return retorno;
        }

        public List<Usuario> ObterAssessoresAnalistas(List<Usuario> diretores)
        {
            var retorno = new List<Usuario>();

            if (!diretores.Any())
                return retorno;

            var emails = diretores.Select(x => x.Email);

            // Busca os diretores por e-mail na tablea Hierarquia
            foreach (var email in emails)
            {
                var diretor = repositorio.session.Query<Hierarquia>().FirstOrDefault(x => x.Email == email);

                // Baseado no CodUnidade liga com a TB_HierarquiaAuxiliar para retornar os verdadeiros acessores
                var assessoresHierarquia = repositorio.session.Query<HierarquiaAuxiliar>().Where(x => x.CodUnidade.Substring(0, 5) == diretor.CodUnidade.Substring(0, 5)).ToList();

                foreach (var assessorHierarquia in assessoresHierarquia)
                {
                    retorno.Add(assessorHierarquia.Usuario);
                }
            }

            return retorno;
        }

        public List<Usuario> ObterDiretoresCorrespondente(string email)
        {
            var retorno = new List<Usuario>();

            if (string.IsNullOrWhiteSpace(email))
                return retorno;

            var bmUsuario = new BMUsuario();

            var hierarquia = repositorio.session.Query<Hierarquia>().FirstOrDefault(x => x.Email == email);

            if (hierarquia != null)
            {
                if (hierarquia.CodUnidade.Length == 5)
                {
                    // Caso o solicitante seja um diretor, obtém os outros diretores que não sejam ele.
                    var outrosDiretores =
                        repositorio.session.Query<Hierarquia>()
                            .Where(x => x.CodUnidade.Length == 5 && x.CodUnidade != hierarquia.CodUnidade)
                            .ToList();

                    retorno.AddRange(outrosDiretores.Select(diretor => bmUsuario.ObterPorEmail(diretor.Email)));
                }
                else
                {
                    // Caso não seja outro diretor, obtém o diretor do setor.
                    // Refs #2884 - Mais de um temporariamente enquanto não tem a opção de aprovação por chefe de gabinete
                    var diretores =
                        repositorio.session.Query<Hierarquia>()
                            .Where(
                                x =>
                                    x.CodUnidade.Length == 5 &&
                                    x.CodUnidade.Substring(0, 5) == hierarquia.CodUnidade.Substring(0, 5))
                            .ToList();

                    foreach (var diretor in diretores)
                    {
                        var usuarioDiretor = bmUsuario.ObterPorEmail(diretor.Email);

                        if (usuarioDiretor != null)
                            retorno.Add(usuarioDiretor);
                    }
                }
            }

            return retorno;
        }

        public void Salvar(EtapaPermissao model)
        {
            //Caso seja unico, descomentar a linha baixo e implementar
            //a verificacao por nome do programa.
            if (model.ID == 0)
            {
                if (this.ObterPorId(model.ID) != null)
                {
                    throw new AcademicoException("Já existe um registro.");
                }
            }

            repositorio.Salvar(model);

        }

        public void Excluir(EtapaPermissao model)
        {
            repositorio.Excluir(model);
        }

        public void ExcluirTodosDeEtapa(int idEtapa)
        {
            var permissoes = repositorio.session.Query<EtapaPermissao>().Where(p => p.Etapa.ID == idEtapa).ToList();

            foreach (var item in permissoes)
            {
                repositorio.Excluir(item);
            }
        }


        private void ValidarModuloInformada(EtapaPermissao model)
        {
            //throw new NotImplementedException();
        }


        public void Dispose()
        {
            repositorio.Dispose();
            GC.Collect();
        }

    }
}
