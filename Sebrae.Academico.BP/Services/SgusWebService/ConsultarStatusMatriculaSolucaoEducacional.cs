using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.BP.Services.SgusWebService
{
    public class ConsultarStatusMatriculaSolucaoEducacional : BusinessProcessServicesBase
    {
        public DTODisponibilidadeSolucaoEducacional ConsultarDisponibilidadeMatriculaSolucaoEducacional(int usuarioId,
            int solucaoId)
        {
            var retorno = new DTODisponibilidadeSolucaoEducacional
            {
                CodigoDisponibilidade = (int) enumDisponibilidadeSolucaoEducacional.NaoPossuiDisponibilidade,
                TextoDisponibilidade = "Não existe oferta disponível no momento"
            };

            var usuario = new ManterUsuario().ObterUsuarioPorID(usuarioId);

            if (usuario == null)
                return retorno;

            if (UsuarioPossuiBloqueioInscricao(usuario, solucaoId, ref retorno))
                return retorno;

            //Verificar disponibilidade de ofertas normais e contínuas.
            if (VerificarDisponibilidadeOfertasPorPermissoes(usuario, solucaoId, ref retorno))
                return retorno;

            return retorno;
        }

        public DTODisponibilidadeSolucaoEducacional ConsultarDisponibilidadeTurma(int usuarioId, int turmaId)
        {
            var retorno = new DTODisponibilidadeSolucaoEducacional
            {
                CodigoDisponibilidade = (int) enumDisponibilidadeSolucaoEducacional.NaoPossuiDisponibilidade,
                TextoDisponibilidade = "Não existe oferta disponível no momento"
            };

            var turma = new ManterTurma().ObterTurmaPorID(turmaId);

            if (turma == null)
            {
                retorno.TextoDisponibilidade = "Curso não encontrado ou não disponível. Tente novamente.";

                return retorno;
            }

            retorno.IdTurma = turmaId;

            var solucaoId = turma.Oferta.SolucaoEducacional.ID;

            var usuario = new ManterUsuario().ObterUsuarioPorID(usuarioId);

            if (usuario == null)
                return retorno;

            if (UsuarioPossuiBloqueioInscricao(usuario, solucaoId, ref retorno))
                return retorno;

            // Verificar se o usuário pode se inscrever na turma.
            if (VerificarDisponibilidadeTurma(usuario, turma, ref retorno))
                return retorno;

            return retorno;
        }

        /// <summary>
        /// Verificar se o usuário informado pode se inscrever na solução informada.
        /// </summary>
        /// <param name="usuario">Usuário a ser matriculado.</param>
        /// <param name="solucaoId">ID da solução que o usuário deseja se matricular.</param>
        /// <param name="retorno">Objeto de retorno com DTO das informações de bloqueio de matrícula, caso necessário.</param>
        /// <returns>True: usuário não pode se inscrever por causa de algum bloqueio. False: usuário pode proceder com a inscrição.</returns>
        private static bool UsuarioPossuiBloqueioInscricao(Usuario usuario, int solucaoId,
            ref DTODisponibilidadeSolucaoEducacional retorno)
        {
            // Verifica pendências 
            if (PossuiQuestionarioPendente(usuario, ref retorno))
                return true;

            // Verifica se o usuário está matriculado no limite de cursos simultâneos.
            if (UsuarioPossuiLimiteInscricoesSimultaneas(usuario.ID, ref retorno))
                return true;

            // Obter todas as matrículas da Solução.
            var matriculasSe =
                new ManterMatriculaOferta().ObterPorUsuarioESolucaoEducacional(usuario.ID, solucaoId).ToList();

            // Verifica se o aluno já está matriculado na SE
            if (UsuarioPossuiMatriculaSolucao(matriculasSe, solucaoId, ref retorno))
                return true;

            // Verifica se existe alguma matricula como Pendente de Confirmação.
            if (UsuarioPossuiPendenciaConfirmacao(matriculasSe, usuario, ref retorno))
                return true;

            // Se chegou até aqui, pode retornar os dados do termo de aceite e da política de consequência.
            var solucaoEducacional = new ManterSolucaoEducacional().ObterSolucaoEducacionalPorId(solucaoId);

            // Verifica se o usuário tem permissão para se inscrever na SE.
            if (UsuarioPossuiPermissaoSolucao(usuario, solucaoEducacional, ref retorno))
                return true;

            // MÉTODO IMPORTANTE!!!
            // Verificar bloqueio de acordo com as políticas de consequência.
            if (UsuarioPossuiBloqueioPoliticaDeConsequencia(usuario.ID, solucaoId, ref retorno))
                return true;

            // Sse o aluno estiver inscrito em algum programa, verifica se existe algum pré-requisito não cursado.
            if (UsuarioPossuiPendenciaCapacitacaoPrograma(solucaoId, usuario, ref retorno))
                return true;

            if (solucaoEducacional.TermoAceite != null)
            {
                retorno.NomeTermoAceite = solucaoEducacional.TermoAceite.Nome;
                retorno.TextoTermoAceite = solucaoEducacional.TermoAceite.Texto;
                retorno.TextoPoliticaConsequencia = solucaoEducacional.TermoAceite.PoliticaConsequencia;
            }

            // Verifica se a Solução está Inativa.
            if (solucaoEducacional.Ativo == false)
            {
                retorno.TextoDisponibilidade = "Solução Educacional não está ativa no momento";
                return true;
            }

            return false;
        }

        private static bool VerificarDisponibilidadeTurma(Usuario usuario, Turma turma,
            ref DTODisponibilidadeSolucaoEducacional retorno)
        {
            var oferta = turma.Oferta;
            
            return oferta.IsAbertaParaInscricoes() &&
                   VerificarDisponibilidadeOferta(usuario, oferta, ref retorno);
        }

        private static bool VerificarDisponibilidadeOfertasPorPermissoes(Usuario usuario, int solucaoId,
            ref DTODisponibilidadeSolucaoEducacional retorno)
        {
            var manterOferta = new ManterOferta();

            //var ofertas = manterOferta.ObterOfertaPorSolucaoEducacional(solucaoId).Where(
            //    x =>
            //        ((x.DataFim.HasValue && x.DataFim.Value.Date >= DateTime.Today) || (!x.DataFim.HasValue)) &&
            //        (x.DataInicioInscricoes.HasValue && x.DataInicioInscricoes.Value.Date <= DateTime.Today) &&
            //        (x.TipoOferta == enumTipoOferta.Continua ||
            //         (x.DataFimInscricoes.HasValue && x.DataFimInscricoes.Value.Date >= DateTime.Today)) &&
            //        // Seria sábio utilizar o método PossuiTurmasDisponiveis() da Oferta.cs, se o NHibernte não pirasse.
            //        x.ListaTurma.Any(t => t.Status != enumStatusTurma.Cancelada && t.InAberta))
            //    .ToList()
            //    // Somente usuários que possuem permissão de inscrição.
            //    .Where(x => x.UsuarioPossuiPermissao(usuario))
            //    .OrderBy(x => x.DataInicio)
            //    .ToList();

            var ofertas = manterOferta.ObterOfertaPorSolucaoEducacional(solucaoId).Where(
                x =>
                    (x.DataInicioInscricoes.HasValue && x.DataInicioInscricoes.Value.Date <= DateTime.Today) &&
                    (x.TipoOferta == enumTipoOferta.Continua ||
                     (x.DataFimInscricoes.HasValue && x.DataFimInscricoes.Value.Date >= DateTime.Today)) &&
                    // Seria sábio utilizar o método PossuiTurmasDisponiveis() da Oferta.cs, se o NHibernte não pirasse.
                    x.ListaTurma.Any(t => (!t.Status.HasValue || t.Status != enumStatusTurma.Cancelada) && t.InAberta))
                .ToList()
                // Somente usuários que possuem permissão de inscrição.
                .Where(x => x.UsuarioPossuiPermissao(usuario))
                .OrderBy(x => x.DataInicioInscricoes)
                .ToList();            

            foreach (var oferta in ofertas)
            {
                VerificarDisponibilidadeOferta(usuario, oferta, ref retorno);
            }

            if (retorno.IdOferta.HasValue || retorno.OfertasDisponiveis.Any())
                return true;

            return false;
        }

        private static bool VerificarDisponibilidadeOferta(Usuario usuario, Oferta oferta,
            ref DTODisponibilidadeSolucaoEducacional retorno)
        {
            // Caso não seja oferta contínua e só tenha turmas canceladas, não permite a inscrição.
            if (oferta.TipoOferta != enumTipoOferta.Continua &&
                oferta.ListaTurma.All(x => (x.Status.HasValue && x.Status == enumStatusTurma.Cancelada)))
            {
                return false;
            }
            
            // Se possuir vagas, liberar para inscrição.
            if (oferta.ObterVagasDisponiveis(usuario.UF) > 0)
            {
                PreencherRetornoDadosCurso(usuario, oferta, enumDisponibilidadeSolucaoEducacional.EfetuarMatricula, ref retorno);
                return true;
            }

            // Verificar se existem turmas disponíveis caso a oferta seja contínua.
            if (VerificarDisponibilidadeTurmas(oferta, ref retorno))
                return true;
            
            if (oferta.TipoOferta == enumTipoOferta.Continua)
            {
                // Permitir inscrição.
                PreencherRetornoDadosCurso(usuario, oferta, enumDisponibilidadeSolucaoEducacional.EfetuarMatricula, ref retorno);
            }
            
            // TODO: Verificar a necessidade desta verificação de acordo com as regras de negócio.
            if (!oferta.FiladeEspera)
                return false;

            // Permitir inscrição na fila de espera.
            PreencherRetornoDadosCurso(usuario, oferta, enumDisponibilidadeSolucaoEducacional.FilaEspera, ref retorno);

            return true;
        }

        private static bool VerificarDisponibilidadeTurmas(Oferta oferta,
            ref DTODisponibilidadeSolucaoEducacional retorno)
        {
            if (oferta.TipoOferta == enumTipoOferta.Continua &&
                !oferta.ListaTurma.Any(x => x.DataInicio <= DateTime.Now &&
                                            (x.DataFinal == null || x.DataFinal.Value > DateTime.Now) && x.InAberta))
            {
                retorno.TextoDisponibilidade = "Não existe turma disponível para essa Solução Educacional";
                retorno.CodigoDisponibilidade =
                    (int) enumDisponibilidadeSolucaoEducacional.NaoPossuiDisponibilidade;

                return true;
            }

            return false;
        }

        private static bool UsuarioPossuiPendenciaCapacitacaoPrograma(int solucaoId, Usuario usuario,
            ref DTODisponibilidadeSolucaoEducacional retorno)
        {
            var filtroMatriculaCapacitacao = new MatriculaCapacitacao
            {
                StatusMatricula = enumStatusMatricula.Inscrito,
                Usuario = usuario
            };

            var listaMatriculaCapacitacao = new BMMatriculaCapacitacao().ObterPorFiltros(filtroMatriculaCapacitacao,
                true);

            // Verificar se a capacitação possui pré-capacitações.
            foreach (
                var modNecessario in
                    listaMatriculaCapacitacao.Select(
                        matriculaCapacitacao =>
                            new BMModuloSolucaoEducacional().CapacitacaoPossuiSolucao(
                                matriculaCapacitacao.Capacitacao.ID, solucaoId))
                        .Where(modSol => modSol != null)
                        .Select(modSol => new BMModuloPreRequisito().ListPreRequisitosPorModulo(modSol, usuario.ID))
                        .Where(modNecessario => modNecessario.ID != 0))
            {
                retorno.TextoDisponibilidade =
                    string.Format(
                        "Existe uma solução educacional do \"{0}\" da capacitação \"{1}\" do programa \"{2}\" que precisa ser concluída antes que possa ser realizada a inscrição nesse curso. <br /><br /><a href='" +
                        //modNecessario.Capacitacao.Programa.Acesso +
                        "'>Clique aqui</a> para acessar a página do Programa.", modNecessario.Nome,
                        modNecessario.Capacitacao.Nome, modNecessario.Capacitacao.Programa.Nome);

                retorno.CodigoDisponibilidade = (int) enumDisponibilidadeSolucaoEducacional.PossuiPreReqPrograma;

                return true;
            }

            return false;
        }

        private static bool UsuarioPossuiBloqueioPoliticaDeConsequencia(int usuarioId, int solucaoId,
            ref DTODisponibilidadeSolucaoEducacional retorno)
        {
            try
            {
                new ManterMatriculaOferta().VerificarPoliticaDeConsequencia(usuarioId, solucaoId);
            }
            catch (PoliticaConsequenciaException ex)
            {
                retorno.TextoDisponibilidade = ex.Message;
                retorno.CodigoDisponibilidade = (int) ex.Consequencia;

                return true;
            }

            return false;
        }

        private static bool UsuarioPossuiLimiteInscricoesSimultaneas(int usuarioId,
            ref DTODisponibilidadeSolucaoEducacional retorno)
        {
            var listaMatriculaOferta =
                new ManterMatriculaOferta().ObterPorUsuario(usuarioId).Select(x => new MatriculaOferta
                {
                    ID = x.ID,
                    StatusMatricula = x.StatusMatricula
                });

            var cursosInscrito = listaMatriculaOferta.Count(x => x.StatusMatricula == enumStatusMatricula.Inscrito);

            int limteCursosSimultaneos;

            if (
                int.TryParse(
                    ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.CursosSimultaneos).Registro,
                    out limteCursosSimultaneos))
            {
                if (cursosInscrito < limteCursosSimultaneos)
                    return false;

                retorno.TextoDisponibilidade = string.Format("Você só pode realizar {0} em simultâneo.",
                    limteCursosSimultaneos);
                retorno.CodigoDisponibilidade = (int) enumDisponibilidadeSolucaoEducacional.NaoPossuiDisponibilidade;

                return true;
            }

            return false;
        }

        private static bool UsuarioPossuiPermissaoSolucao(Usuario usuario, SolucaoEducacional solucao,
            ref DTODisponibilidadeSolucaoEducacional retorno)
        {
            // Verifica se o usuário não tiver acesso à Solução Educacional.
            if (solucao.UsuarioPossuiPermissaoMatricula(usuario))
                return false;

            retorno.TextoDisponibilidade = "Solução não disponível para seu perfil. Entre em contato no fale conosco.";
            retorno.CodigoDisponibilidade = (int) enumDisponibilidadeSolucaoEducacional.NaoPossuiDisponibilidade;

            return true;
        }

        private static bool UsuarioPossuiMatriculaSolucao(IList<MatriculaOferta> listaMatriculaOferta, int solucaoId,
            ref DTODisponibilidadeSolucaoEducacional retorno)
        {
            var existeAlgumaMatriculaComoInscrito =
                ExisteAlgumaMatriculaComoInscrito(listaMatriculaOferta, solucaoId);

            if (!existeAlgumaMatriculaComoInscrito)
                return false;

            retorno.TextoDisponibilidade = "Você já está matriculado nessa Solução Educacional";
            retorno.CodigoDisponibilidade = (int) enumDisponibilidadeSolucaoEducacional.Matriculado;

            return true;
        }

        private static bool UsuarioPossuiPendenciaConfirmacao(
            IList<MatriculaOferta> listaMatriculaOfertaNaSolucaoEducacional,
            Usuario usuario, ref DTODisponibilidadeSolucaoEducacional retorno)
        {
            var ofertaPendenteConfirmacao =
                listaMatriculaOfertaNaSolucaoEducacional.Where(
                    x => x.StatusMatricula == enumStatusMatricula.PendenteConfirmacaoAluno)
                    .Select(x => x.Oferta)
                    .FirstOrDefault();

            if (ofertaPendenteConfirmacao != null)
            {
                var disponibilidade = enumDisponibilidadeSolucaoEducacional.ConfirmarMatricula;

                PreencherRetornoDadosCurso(usuario, ofertaPendenteConfirmacao, disponibilidade, ref retorno);

                return true;
            }

            return false;
        }

        private static bool PossuiQuestionarioPendente(Usuario usuario, ref DTODisponibilidadeSolucaoEducacional retorno)
        {
            var turmasPendentes = new ManterTurma().ObterTurmasPendentes(usuario);

            if (turmasPendentes.Any())
            {
                Turma primeiraTurma = turmasPendentes.FirstOrDefault();

                //Valida se a data da matricula está no ano vigente, caso contrário, não apresenta questionário de abandono.
                //Verify if the registration date is in current year, otherwise, don't show the abandon quiz.
                if (!(primeiraTurma.DataFinal.Value.Year < DateTime.Now.Year))
                {
                    if ((primeiraTurma = turmasPendentes.FirstOrDefault()) != null)
                    {
                        retorno.CodigoDisponibilidade =
                            (int)enumDisponibilidadeSolucaoEducacional.PendenciaAbandono;

                        retorno.IdTurma = primeiraTurma.ID;

                        // Montar mensagem da pendência, com os plurais caso seja mais de uma turma.
                        retorno.TextoDisponibilidade =
                            string.Format(
                                "Você abandonou a{0} turma{0} no{0} curso{0} \"{1}\", e precisa responder o{0} questionário{0} de abandono para poder se inscrever em qualquer curso.",
                                turmasPendentes.Count() > 1 ? "s" : "",
                                string.Join(", ", turmasPendentes.Select(x => x.Oferta.SolucaoEducacional.Nome)));

                        return true;
                    }
                }
            }

            return false;
        }

        public static bool VerificarTrancadoParaPagante(Usuario usuario, Oferta oferta)
        {
            if (oferta.ListaNiveisTrancados != null && oferta.ListaNiveisTrancados.Any(x => x.ID == usuario.NivelOcupacional.ID) && !VerificarTrancadoParaPagante(usuario))
            {
                return true;
            }
            return false;
        }

        public static bool VerificarTrancadoParaPagante(Usuario usuario, Oferta oferta,
            enumDisponibilidadeSolucaoEducacional disponibilidade, ref DTODisponibilidadeSolucaoEducacional retorno)
        {
            if (disponibilidade == enumDisponibilidadeSolucaoEducacional.EfetuarMatricula)
            {
                if (oferta.ListaNiveisTrancados != null && oferta.ListaNiveisTrancados.Any(x => x.ID == usuario.NivelOcupacional.ID) && !VerificarTrancadoParaPagante(usuario))
                {
                    retorno.TextoDisponibilidade = "A oferta está trancada para pagamento, favor, efetuar o pagamento";
                    retorno.CodigoDisponibilidade = (int) enumDisponibilidadeSolucaoEducacional.EfetuarPagamento;
                    return true;
                }
            }
            return false;
        }

        public static bool VerificarTrancadoParaPagante(Usuario usuario)
        {
            // Caso tenha o pagamento confirmado (via arquivo de retorno), pagamento informado via portal pelo usuário
            // ou boleto colocado como pago (não sei por onde é inserida essa informação).
            return
                usuario.ListaHistoricoPagamento.Any(
                    x =>
                        x.DataInicioVigencia <= DateTime.Today && x.DataFimVigencia >= DateTime.Today &&
                        (x.PagamentoEfetuado || x.PagamentoInformado.HasValue && x.PagamentoInformado.Value ||
                         x.PagamentoConfirmado.HasValue && x.PagamentoConfirmado.Value)
                );
        }

        private static void PreencherRetornoDadosCurso(Usuario usuario, Oferta oferta,
            enumDisponibilidadeSolucaoEducacional disponibilidade,
            ref DTODisponibilidadeSolucaoEducacional retorno)
        {
            retorno.TextoDisponibilidade = string.Empty;
            retorno.TextoInformacaoAdicional = oferta.InformacaoAdicional;

            if (disponibilidade == enumDisponibilidadeSolucaoEducacional.ConfirmarMatricula)
                retorno.TextoDisponibilidade = "Você precisa confirmar a inscrição na Solução Educacional";

            if (disponibilidade == enumDisponibilidadeSolucaoEducacional.FilaEspera)
                retorno.TextoDisponibilidade =
                    "No momento não há vagas disponíveis para essa Solução Educacional. Você será inscrito na fila de espera.";
            
            retorno.IdOferta = oferta.ID;
            retorno.IdTipoOferta = (int) oferta.TipoOferta;
            retorno.PermiteFilaEspera = oferta.FiladeEspera;
            retorno.Prazo = oferta.DiasPrazo.ToString();
            retorno.CargaHoraria = oferta.CargaHoraria.ToString();
            //retorno.DataInicioOferta = oferta.DataInicio;
            //retorno.DataFimOferta = oferta.DataFim;

            retorno.DataInicioInscricoes =
                retorno.IdTipoOferta == 2 && oferta.SolucaoEducacional != null &&
                oferta.SolucaoEducacional.TeraOfertasContinuas
                    ? oferta.SolucaoEducacional.Inicio
                    : oferta.DataInicioInscricoes;

            retorno.DataFimInscricoes =
                retorno.IdTipoOferta == 2 && oferta.SolucaoEducacional != null &&
                oferta.SolucaoEducacional.TeraOfertasContinuas
                    ? oferta.SolucaoEducacional.Fim
                    : oferta.DataFimInscricoes;

            retorno.InscricaoOnline = (oferta.InscricaoOnline ?? false);
            retorno.CodigoDisponibilidade = (int) disponibilidade;
            retorno.IdSolucaoEducacional = oferta.SolucaoEducacional != null
                ? oferta.SolucaoEducacional.ID
                : 0;
            
            // Caso a oferta possua turmas disponíveis, retorna a oferta com todas as turmas disponíveis para matrícula.
            if (oferta.IsAbertaParaInscricoes() && oferta.PossuiTurmasDisponiveis())
            {
                DTOOferta dtoOferta = new DTOOferta(oferta);

                //Tranca as ofertas que precisam de pagamento.
                dtoOferta.RequerPagamento = VerificarTrancadoParaPagante(usuario, oferta);

                retorno.OfertasDisponiveis.Add(dtoOferta);
            }
        }

        public DTODisponibilidadeSolucaoEducacionalPorUsuarioT ConsultarDisponibilidadeMatriculaPorUsuario(
            int pIdUsuario, int? pagina)
        {
            var retorno = new DTODisponibilidadeSolucaoEducacionalPorUsuarioT();

            var usuario = new ManterUsuario().ObterUsuarioPorID(pIdUsuario);

            new ManterMatriculaOferta().ObterPorUsuario(pIdUsuario);

            var listaPermissaoSolucaoEducacional = new BMSolucaoEducacional().ObterListaDePermissoes(usuario.ID, 0);
            var idsSolucoesPermissoes = listaPermissaoSolucaoEducacional.Select(f => f.SolucaoEducacional.ID).ToList();

            var filtro = new SolucaoEducacional();

            var recuperarListaSolucaoEducacional = new ManterSolucaoEducacional().ObterPorFiltroPesquisa(filtro, true,
                idsSolucoesPermissoes);

            const int skip = 5;

            var paginaAtual = (pagina.HasValue && pagina.Value > 0) ? (pagina.Value - 1) : 0;

            retorno.PaginaAtual = (paginaAtual + 1);
            retorno.QtdeSolucoes = recuperarListaSolucaoEducacional.Count();
            retorno.QtdePaginas = Convert.ToInt32((retorno.QtdeSolucoes/skip));
            if ((retorno.QtdeSolucoes%skip) > 0)
                retorno.QtdePaginas++;

            foreach (var item in recuperarListaSolucaoEducacional.Skip((paginaAtual*skip)).Take(skip).ToList())
            {
                retorno.DTODisponibilidadeSolucaoEducacional.Add(
                    ConsultarDisponibilidadeMatriculaSolucaoEducacional(pIdUsuario, item.ID));
            }

            return retorno;
        }
        public DTO.Services.RetornoWebService EntrarNaFilaDeEspera(int pUsuario, int idOferta)
        {
            try
            {
                var usuario = new BMUsuario().ObterPorId(pUsuario);
                var oferta = new BMOferta().ObterPorId(idOferta);

                if (EstaInscrito(pUsuario, idOferta))
                {
                    return new DTO.Services.RetornoWebService
                    {
                        Erro = 0,
                        Mensagem = "Já Inscrito"
                    };
                }

                var matriculaOferta = new MatriculaOferta
                {
                    Usuario = usuario,
                    Oferta = oferta,
                    StatusMatricula = enumStatusMatricula.FilaEspera,
                    UF = usuario.UF,
                    NivelOcupacional = usuario.NivelOcupacional,
                    DataSolicitacao = DateTime.Now,
                    Auditoria = new Auditoria(usuario.CPF)
                };

                new BMMatriculaOferta().Salvar(matriculaOferta);

                var matriculaTurma = new MatriculaTurma
                {
                    Turma = oferta.ListaTurma.FirstOrDefault(),
                    MatriculaOferta = matriculaOferta,
                    DataMatricula = DateTime.Now,
                    Auditoria = new Auditoria(usuario.CPF)
                };

                matriculaTurma.DataLimite = matriculaTurma.CalcularDataLimite(matriculaOferta.Oferta);
                new BMMatriculaTurma().Salvar(matriculaTurma);

                EnvialEmailFilaEspera(usuario);

                return new DTO.Services.RetornoWebService
                {
                    Erro = 0,
                    Mensagem = "Success"
                };
            }
            catch (Exception ex)
            {
                return new DTO.Services.RetornoWebService
                {
                    Erro = 1,
                    Mensagem = "Falha",
                    Stack = ex.Message
                };
            }
        }

        private static void EnvialEmailFilaEspera(Usuario usuario)
        {
            var template = new ManterTemplate().ObterTemplatePorID((int)enumTemplate.FilaEspera);
            template.TextoTemplate = template.TextoTemplate.Replace("#ALUNO", usuario.NomeExibicao);

            EmailUtil.Instancia.EnviarEmail(usuario.Email, template.Assunto, template.TextoTemplate);
        }

        private static bool EstaInscrito(int pUsuario, int idOferta)
        {
            return new BMMatriculaOferta()
                .ObterTodosIQueryable()
                .Where(x => x.Oferta.ID == idOferta)
                .Where(x => x.Usuario.ID == pUsuario)
                .Any();
        }

        public DTOSolucoes ConsultarDisponibilidadeMatriculaPorUsuario(int pIdUsuario, int idSolucaoEducacional,
            int cargaHoraria)
        {
            var retorno = new DTOSolucoes();

            var usuario = new ManterUsuario().ObterUsuarioPorID(pIdUsuario);

            if (usuario != null)
            {
                var permissoesSoculaoEducacional =
                    new BMSolucaoEducacional().ObterListaDePermissoes(usuario.ID, idSolucaoEducacional)
                        .Select(f => f.SolucaoEducacional.ID)
                        .Distinct()
                        .ToList();
                var permissoesOfertas =
                    new BMOferta().ObterListaDePermissoes(usuario.ID).Select(f => f.IdOferta).Distinct().ToList();


                if (idSolucaoEducacional > 0 && permissoesSoculaoEducacional.Contains(idSolucaoEducacional))
                {
                    permissoesSoculaoEducacional.Clear();
                    permissoesSoculaoEducacional.Add(idSolucaoEducacional);
                }

                var solucoes = new ManterSolucaoEducacional().ObterPorFiltroPesquisa(new SolucaoEducacional(), true,
                    permissoesSoculaoEducacional);

                foreach (var solucaoEducacional in solucoes)
                {
                    var ofertas = solucaoEducacional.ListaOferta.Where(
                        x =>
                            (x.DataInicioInscricoes.HasValue &&
                             x.DataInicioInscricoes.Value.Date <= DateTime.Today) &&
                            x.DataFimInscricoes.HasValue &&
                            x.DataFimInscricoes.Value.Date >= DateTime.Today &&
                            permissoesOfertas.Contains(x.ID) &&
                            //x.ListaPermissao.Any(f=>f.Uf.ID == usuario.UF.ID) &&
                            !(x.ListaTurma.Any(
                                f =>
                                    (f.DataFinal == null || f.DataFinal.Value.Date > DateTime.Today) &&

                                    // Remover turmas canceladas das possibilidades de matrícula. #2997
                                    f.Status != enumStatusTurma.Cancelada &&
                                    f.InAberta == false)));

                    if (cargaHoraria > 0)
                        ofertas = ofertas.Where(f => f.CargaHoraria == cargaHoraria);

                    var se = new DTOCurso {IDSolucaoEducacional = solucaoEducacional.ID};

                    foreach (var oferta in ofertas.OrderBy(x => x.DataInicioInscricoes))
                    {
                        if (oferta.TipoOferta.Equals(enumTipoOferta.Exclusiva))
                        {
                            break;
                        }

                        //Verificando se existe alguma matricula na SE
                        if (ExisteAlgumaMatriculaComoInscrito(usuario.ListaMatriculaOferta, se.IDSolucaoEducacional))
                            break;

                        //VALIDAR SE O USUARIO POSSUI ACESSO A SE: 205 (permissão a se)
                        //VALIDADO NA BUSCA

                        //VALIDAR SE O USUARIO ESTA CURSANDO OUTRA SE
                        //VERIFICA O LIMITE DE CURSOS SIMULTANEOS
                        int cursosEmAndamento =
                            usuario.ListaMatriculaOferta.Where(
                                x => x.StatusMatricula.Equals(enumStatusMatricula.Inscrito)).Count();
                        int limteCursosSimultaneos =
                            int.Parse(
                                ConfiguracaoSistemaUtil.ObterInformacoes(enumConfiguracaoSistema.CursosSimultaneos)
                                    .Registro);
                        if (cursosEmAndamento >= limteCursosSimultaneos)
                            break;

                        //VALIDAR SE O USUARIO ESTA COM ALGUM ABANDONO ATIVO
                        if (new BMUsuarioAbandono().ValidarAbandonoAtivo(usuario.ID))
                            break;

                        //VERIFICA SE A OFERTA INFORMADA FOI ENCONTRADA
                        //NO CADASTRO

                        if (oferta.TipoOferta.Equals(enumTipoOferta.Continua))
                        {

                        }

                        if (oferta.TipoOferta.Equals(enumTipoOferta.Normal))
                        {

                        }

                        var qtdInscritosNaOferta =
                            oferta.ListaMatriculaOferta.Count(
                                x => (x.StatusMatricula != enumStatusMatricula.CanceladoAdm &&
                                      x.StatusMatricula != enumStatusMatricula.CanceladoAluno));

                        if (qtdInscritosNaOferta >= oferta.QuantidadeMaximaInscricoes && !oferta.FiladeEspera)
                        {
                            break;
                        }

                        var trancadoPorNaoPagante = false;

                        if (oferta.ListaNiveisTrancados != null &&
                            oferta.ListaNiveisTrancados.Any(x => x.ID == usuario.NivelOcupacional.ID))
                        {
                            if (!VerificarTrancadoParaPagante(usuario))
                            {
                                trancadoPorNaoPagante = true;
                            }
                        }

                        se.Ofertas.Add(new DTOOfertas
                        {
                            IDOferta = oferta.ID,
                            CargaHoraria = oferta.CargaHoraria,
                            TrancadoPorNaoPagamento = trancadoPorNaoPagante,
                            Nome = oferta.Nome
                        });
                    }

                    if (se.Ofertas.Any())
                    {
                        retorno.Cursos.Add(se);
                    }
                }
            }

            return retorno;
        }

        public DTOSolucoes ConsultarTurmaPorSolucaoEducacional(int idSolucaoEducacional, int idOferta, string cpfUsuario)
        {
            DTOSolucoes retorno = new DTOSolucoes();

            var usuario = new BMUsuario().ObterPorCPF(cpfUsuario);
            List<int> permissoesOfertas =
                new BMOferta().ObterListaDePermissoes(usuario.ID).Select(f => f.IdOferta).Distinct().ToList();

            var solucaoEducacional = new ManterSolucaoEducacional().ObterSolucaoEducacionalPorId(idSolucaoEducacional);

            IList<Oferta> ofertas = RecuperarOfertasValidas(solucaoEducacional, permissoesOfertas);

            if (idOferta > 0)
                ofertas = ofertas.Where(f => f.ID == idOferta).ToList();

            var se = new DTOCurso();
            se.IDSolucaoEducacional = solucaoEducacional.ID;

            foreach (var oferta in ofertas)
            {
                var o = new DTOOfertas
                {
                    IDOferta = oferta.ID,
                    PermiteAlterarStatusPeloGestor =
                        (oferta.AlteraPeloGestorUC.HasValue ? oferta.AlteraPeloGestorUC.Value : true)
                };

                var turmas =
                    oferta.ListaTurma.Where(
                        f => (f.DataFinal == null || f.DataFinal.Value.Date > DateTime.Today) && f.InAberta).ToList();

                foreach (var turma in turmas)
                {
                    o.ListaTurma.Add(new DTOOfertaTurma {IdTurma = turma.ID, Nome = turma.Nome});
                }

                se.Ofertas.Add(o);
            }

            if (se.Ofertas.Count() > 0)
            {
                retorno.Cursos.Add(se);
            }

            return retorno;
        }

        public IList<Oferta> RecuperarOfertasValidas(SolucaoEducacional solucaoEducacional, List<int> permissoesOfertas)
        {
            var ofertas =
                solucaoEducacional.ListaOferta.Where(
                    x => (x.DataInicioInscricoes.HasValue &&
                          x.DataInicioInscricoes.Value.Date <= DateTime.Today) &&
                         x.DataFimInscricoes.HasValue &&
                         x.DataFimInscricoes.Value >= DateTime.Today &&
                         (x.ListaTurma.Any(
                             f => (f.DataFinal == null || f.DataFinal.Value.Date > DateTime.Today) && f.InAberta)));

            if (permissoesOfertas != null && permissoesOfertas.Any())
                ofertas = ofertas.Where(x => permissoesOfertas.Contains(x.ID));


            return ofertas.OrderBy(x => x.DataInicioInscricoes).ToList();
        }

        private static bool ExisteAlgumaMatriculaComoInscrito(IList<MatriculaOferta> listaMatriculaOferta,
            int idSolucaoEducacional = 0)
        {
            return idSolucaoEducacional > 0
                ? listaMatriculaOferta.Any(
                    x =>
                        x.StatusMatricula == enumStatusMatricula.Inscrito &&
                        x.Oferta.SolucaoEducacional.ID == idSolucaoEducacional)
                : listaMatriculaOferta.Any(x => x.StatusMatricula == enumStatusMatricula.Inscrito);
        }

        public DTOCursosPorCategoria ConsultarCursosPorCategoria(int idNoCategoriaConteudo, string cpf, string nome,
            bool somenteComInscricoesAbertas)
        {
            var retorno = new DTOCursosPorCategoria
            {
                Categoria = new DTOCategoriaConteudo(),
                ListaCursos = new List<DTOSolucaoEducacional>()
            };

            var categoria = new BMCategoriaConteudo().ObterPorIdNode(idNoCategoriaConteudo);
            retorno.Categoria.ID = categoria.ID;
            retorno.Categoria.Nome = categoria.Nome;

            var usuario = new BMUsuario().ObterPorCPF(cpf);

            var permissoesSe =
                new BMSolucaoEducacional().ObterListaDePermissoes(usuario.ID, 0)
                    .Select(x => x.SolucaoEducacional.ID)
                    .ToList();
            var permissoesOf = new BMOferta().ObterListaDePermissoes(usuario.ID).Select(x => x.IdOferta).ToList();

            var solucoesEducacionais = new BMSolucaoEducacional().ObterTodos().AsEnumerable()
                .Where(x => x.Nome.ToLower().Contains(nome.ToLower()) &&
                            x.CategoriaConteudo != null &&
                            x.Ativo &&
                            x.CategoriaConteudo.IdNode == idNoCategoriaConteudo &&
                            //(x.IdNode.HasValue || !string.IsNullOrEmpty(x.Link)) &&
                            permissoesSe.Contains(x.ID) &&
                            x.ListaOferta != null &&
                            x.ListaOferta.Any(o => permissoesOf.Contains(o.ID)) &&
                            x.ListaOferta.Any(o => o.TipoOferta != enumTipoOferta.Exclusiva)
                ).ToList();

            foreach (var item in solucoesEducacionais)
            {
                var obj = new DTOSolucaoEducacional
                {
                    ID = item.ID,
                    IDChaveExterna = item.IDChaveExterna,
                    IdNode = item.IdNode,
                    Nome = item.Nome,
                    InscricoesAbertas = false
                };

                if (
                    item.ListaOferta.Any(
                        x =>
                            x.DataInicioInscricoes.HasValue && x.DataFimInscricoes.HasValue &&
                            x.DataInicioInscricoes.Value <= DateTime.Today &&
                            x.DataFimInscricoes.Value >= DateTime.Today))
                {
                    foreach (var oferta in item.ListaOferta)
                    {
                        if (string.IsNullOrEmpty(obj.Link))
                            obj.Link = (item.Fornecedor.ID == (int) enumFornecedor.FGVOCW ||
                                        item.Fornecedor.ID == (int) enumFornecedor.FGVSiga) &&
                                       !string.IsNullOrEmpty(oferta.Link)
                                ? oferta.Link
                                : "";

                        obj.InscricoesAbertas = oferta.ListaTurma.Any(x => x.DataInicio <= DateTime.Today &&
                                                                           x.DataFinal.HasValue &&
                                                                           x.DataFinal.Value >= DateTime.Today);
                    }
                }
                else
                {
                    obj.InscricoesAbertas = false;
                }

                bool adicionar = true;

                if (somenteComInscricoesAbertas)
                {
                    adicionar = obj.InscricoesAbertas;
                }

                if (adicionar)
                {
                    retorno.ListaCursos.Add(obj);
                }
            }

            return retorno;
        }
    }
}