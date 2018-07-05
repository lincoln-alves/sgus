using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Execution;
using JWT;
using Nancy.Extensions;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.Auth;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.BP.Services;
using Sebrae.Academico.BP.DTO.Services;
using Sebrae.Academico.BP.DTO.Services.Questionario;
using Sebrae.Academico.BP.DTO.Services.Trilhas;
using Sebrae.Academico.BP.DTO.Services.Trilhas.Loja;
using Sebrae.Academico.BP.DTO.Services.Trilhas.Mapa;
using Sebrae.Academico.BP.DTO.Services.Trilhas.Questionario;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.Util.Classes;
using DTOTermoAceite = Sebrae.Academico.BP.DTO.Dominio.DTOTermoAceite;
using Sebrae.Academico.BP.Services.SgusWebService;
using ConheciGame = Sebrae.Academico.BP.Classes.ConheciGame;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace Sebrae.Academico.BP.Services
{
    public partial class TrilhaServices : BusinessProcessServicesBase
    {
        public dynamic ObterTutorialPorCategoria(int categoriaId, int tutorialId, UserIdentity usuarioLogado)
        {
            return new ManterTrilhaTutorial().obterTrilhaTutorialPorCategoriaIdPaginado(categoriaId, tutorialId,
                usuarioLogado);
        }

        public dynamic ObterFaqPorId(int id)
        {
            var faq = new ManterTrilhaFaq().ObterPorId(id);

            if (faq != null)
            {
                return new
                {
                    nome = faq.Nome,
                    descricao = faq.Descricao
                };
            }
            else
            {
                throw new ResponseException(enumResponseStatusCode.RegistroNaoEncontrado);
            }

        }

        public object MarcarLidoAcessoTutorial(UsuarioTrilha matricula, int categoria_id)
        {
            (new ManterTrilhaTutorial()).MarcarLidoAcessoTutorial(matricula, categoria_id);

            // dummy return
            return new
            {
                status = 1
            };
        }

        public bool RespondeuEntrevista(UsuarioTrilha matricula, TrilhaNivel nivel)
        {
            // Verificar se já respondeu a entrevista.
            var respondeuEntrevista =
                nivel.ListaQuestionarioParticipacao.Any(
                    x =>
                        x.Usuario.ID == matricula.Usuario.ID
                        && x.TipoQuestionarioAssociacao.ID == (int)enumTipoQuestionarioAssociacao.Pos
                        && x.TrilhaNivel.ID == nivel.ID
                        && x.DataParticipacao != null);

            return respondeuEntrevista;
        }

        public List<DTOTrilhaNivel> ObterNiveisPorTrilhaUsuario(int trilhaId, int usuarioId)
        {
            var usuario = new ManterUsuario().ObterUsuarioPorID(usuarioId);

            if (usuario == null)
                return null;

            string preRequisitoNaoCumprido;

            // Converte tudo necessário para DTO.
            var niveis =
                new ManterTrilhaNivel().ObterPorTrilhaUsuario(trilhaId, usuario).ToList().Select(x => new DTOTrilhaNivel
                {
                    ID = x.ID,
                    Nome = x.Nome,
                    PrerequisitoNaoCumprido = (preRequisitoNaoCumprido = x.ObterPrerequisitoNaoCumprido(usuario)),
                    UsuarioPossuiMatricula = x.UsuarioPossuiMatricula(usuario),
                    Ordem = x.ValorOrdem,
                    CargaHoraria = x.CargaHoraria,
                    DiasPrazo = x.QuantidadeDiasPrazo,
                    TermoAceite = x.TermoAceite != null
                        ? new DTOTermoAceite
                        {
                            ID = x.TermoAceite.ID,
                            Nome = x.TermoAceite.Nome,
                            TextoPoliticaConsequencia = x.TermoAceite.PoliticaConsequencia,
                            TextoTermoAceite = x.TermoAceite.Texto
                        }
                        : null,


                    // Só envia os dados abaixo se o usuário não possuir pré-requisito, para aliviar a requisição.
                    Descricao = string.IsNullOrWhiteSpace(preRequisitoNaoCumprido) ? x.Descricao : null,
                    TopicosTematicos =
                        string.IsNullOrWhiteSpace(preRequisitoNaoCumprido)
                            ? x.ListaPontoSebrae.Select(p => new DTOTrilhaTopicoTematico
                            {
                                ID = p.ID,
                                Nome = p.NomeExibicao,
                                Objetivos = p.ListaMissoes.Where(m => m.ListaItemTrilha.Any()).Select(m => new DTOObjetivo
                                {
                                    ID = m.ID,
                                    Nome = m.Nome,
                                    Solucoes = m.ListaItemTrilha.Where(s => s.Usuario == null).Select(s => new DtoTrilhaSolucaoSebrae
                                    {
                                        CargaHoraria = s.CargaHoraria,
                                        Nome = s.Nome,
                                        Moedas = s.Moedas,
                                        FormaAquisicao = s.FormaAquisicao.Nome,
                                        FormaAquisicaoImagem = s.FormaAquisicao.Imagem

                                    }).ToList(),
                                    FormasAquisicao = m.ObterFormasAquisicao().Select(f => new DTOFormaAquisicao
                                    {
                                        Nome = f.Nome,
                                        Imagem = f.Imagem,
                                        Quantidade = m.ContarItensPorGrupoFormaAquisicao(f.ID)
                                    }).ToList()
                                }).ToList()
                            }).ToList()
                            : null

                });

            return niveis.OrderBy(x => x.Ordem).ToList();
        }

        private string convertMinutesToHours(int minutes)
        {
            string cargaHoraria = "0";
            if (minutes > 0)
            {
                int horas = (int)Math.Floor(minutes / 60.0);
                int minutos = minutes - (horas * 60);
                cargaHoraria = (horas > 9 ? (horas + "") : ("0" + horas))  + ":" + (minutos > 9 ? (minutos + "") : ("0" + minutos));
            }
            return cargaHoraria;
        }

        public dynamic ObterDadosSolucaoTrilheiro(int id)
        {
            var itemTrilha = new ManterItemTrilha().ObterItemTrilhaPorID(id);

            if (itemTrilha == null)
            {
                throw new ResponseException(enumResponseStatusCode.RegistroNaoEncontrado,
                    "Solução Sebrae não encontrada");
            }

            

            return new
            {
                Id = itemTrilha.ID,
                Nome = itemTrilha.Nome,
                Missao = itemTrilha.Objetivo != null ? itemTrilha.Objetivo.Nome : null,
                Tipo = itemTrilha.FormaAquisicao != null ? itemTrilha.FormaAquisicao.Nome : null,
                OndeEncontrar = itemTrilha.LinkConteudo != null ? itemTrilha.LinkConteudo : null,
                //conversão para horas
                CargaHoraria = convertMinutesToHours(itemTrilha.CargaHoraria),
                ReferenciaBibliografica =
                    itemTrilha.ReferenciaBibliografica != null ? itemTrilha.ReferenciaBibliografica : null,
                Descricao = itemTrilha.Local != null ? itemTrilha.Local : null,
                Anexo = ObterLinkAnexo(itemTrilha.FileServer),
                NomeAnexo = itemTrilha.FileServer != null ? itemTrilha.FileServer.NomeDoArquivoOriginal : null
            };
        }

        /// <summary>
        /// Retorna todas as curtidas e descurtidas do ItemTrilha.
        /// </summary>
        /// <param name="itemTrilhaId"></param>
        /// <param name="soCurtidas">Somente curtidas, sem trazer as descurtidas</param>
        /// <returns></returns>
        public DTOCurtidas ListarCurtidas(int itemTrilhaId, bool soCurtidas, UsuarioTrilha usuarioTrilha)
        {
            var enderecoSgus =
                new ManterConfiguracaoSistema().ObterConfiguracaoSistemaPorID(
                    (int)enumConfiguracaoSistema.EnderecoSGUS)
                    .Registro;

            var itemTrilha = new ManterItemTrilha().ObterItemTrilhaPorID(itemTrilhaId);

            if (itemTrilha == null)
            {
                throw new ResponseException(enumResponseStatusCode.RegistroNaoEncontrado,
                   "Solução do Trilheiro não encontrada");
            }

            // Agrupar as curtidas por usuários e buscar sempre a mais recente de cada usuário.
            var manterCurtidas = new ManterItemTrilhaCurtida();
            var curtidas = manterCurtidas.ObterCurtidasUsuario(itemTrilha, usuarioTrilha);


            ItemTrilhaCurtida curtidaUsuarioLogado = curtidas.FirstOrDefault(x => x.UsuarioTrilha.ID == usuarioTrilha.ID);
            enumTipoCurtida tipoCurtida = enumTipoCurtida.SemAcao;

            if (curtidaUsuarioLogado != null)
            {
                tipoCurtida = curtidaUsuarioLogado.TipoCurtida;
            }

            return new DTOCurtidas
            {
                QuantidadeCurtidas = manterCurtidas.ObterTotalCurtidasUsuario(itemTrilha, usuarioTrilha, enumTipoCurtida.Curtiu),
                QuantidadeDescurtidas = manterCurtidas.ObterTotalCurtidasUsuario(itemTrilha, usuarioTrilha, enumTipoCurtida.Descurtiu),
                TipoCurtida = tipoCurtida,
                ListaCurtidas =
                    curtidas.Where(x => !soCurtidas || x.TipoCurtida == enumTipoCurtida.Curtiu)
                        .Select(x => new DTOUsuarioCurtida
                        {
                            Id = x.UsuarioTrilha.Usuario.ID,
                            TipoCurtida = x.TipoCurtida,
                            Nome = x.UsuarioTrilha.Usuario.ObterPrimeirosNomes(),
                            Unidade = "Sebrae " + x.UsuarioTrilha.Usuario.UF.Sigla,
                            Foto = x.UsuarioTrilha.Usuario.ObterLinkImagem(enderecoSgus)
                        }).ToList()
            };
        }

        /// <summary>
        /// Define a curtida para o usuário atual do ItemTrilha.
        /// </summary>
        /// <param name="itemTrilhaId"></param>
        /// <param name="tipoCurtida"></param>
        /// <param name="matricula"></param>
        /// <returns></returns>
        public bool DefinirCurtida(int itemTrilhaId, enumTipoCurtida tipoCurtida, UsuarioTrilha matricula)
        {
            var itemTrilha = new ManterItemTrilha().ObterItemTrilhaPorID(itemTrilhaId);

            if (itemTrilha == null)
                throw new ResponseException(enumResponseStatusCode.RegistroNaoEncontrado,
                    "Solução do Trilheiro não encontrada");

            if (itemTrilha.Usuario.ID == matricula.Usuario.ID)
                throw new ResponseException(enumResponseStatusCode.CurtirSolucaoPropria);

            var itemTrilhaCurtida =
                new ManterItemTrilhaCurtida().ObterTodos()
                    .FirstOrDefault(
                        x => x.ItemTrilha.ID == itemTrilhaId && x.UsuarioTrilha.ID == matricula.ID);

            //Pega o tipo padrão que é sem ação.
            var tipoCurtidaAnterior = enumTipoCurtida.SemAcao;

            if (itemTrilhaCurtida == null)
            {
                itemTrilhaCurtida = new ItemTrilhaCurtida
                {
                    ItemTrilha = itemTrilha,
                    TipoCurtida = tipoCurtida,
                    UsuarioTrilha = matricula,
                    ValorCurtida = itemTrilha.Missao.PontoSebrae.TrilhaNivel.QuantidadeMoedasPorCurtida ?? 0,
                    ValorDescurtida = itemTrilha.Missao.PontoSebrae.TrilhaNivel.QuantidadeMoedasPorDescurtida ?? 0
                };
            }
            else
            {
                tipoCurtidaAnterior = itemTrilhaCurtida.TipoCurtida;

                itemTrilhaCurtida.TipoCurtida = tipoCurtida;

                if (tipoCurtidaAnterior == itemTrilhaCurtida.TipoCurtida)
                {
                    switch (itemTrilhaCurtida.TipoCurtida)
                    {
                        case enumTipoCurtida.SemAcao:
                            throw new ResponseException(enumResponseStatusCode.ErroRegraNegocioTrilhas,
                                "Não é possível fazer a mesma ação de cancelamento na mesma solução.");
                        case enumTipoCurtida.Curtiu:
                        case enumTipoCurtida.Descurtiu:
                            itemTrilhaCurtida.TipoCurtida = enumTipoCurtida.SemAcao;
                            break;
                    }
                }
            }

            itemTrilhaCurtida.Auditoria = new Auditoria(matricula.Usuario.CPF);
            new ManterItemTrilhaCurtida().Alterar(itemTrilhaCurtida);

            var usuarioDono =
                itemTrilha.Missao.PontoSebrae.TrilhaNivel.ListaUsuarioTrilha.FirstOrDefault(
                    x => x.Usuario.ID == itemTrilha.Usuario.ID);

            new ManterUsuarioTrilhaMoedas().Incluir(usuarioDono, null, itemTrilhaCurtida, 0, 0, tipoCurtidaAnterior);

            // Criando as notificações de lançamento de moedas
            switch (tipoCurtida)
            {
                case enumTipoCurtida.Curtiu:
                    new ManterNotificacao().IncluirNotificacaoTrilha(itemTrilhaCurtida.UsuarioTrilha,
                        string.Format("Você recebeu {0} na {1}", "curtidas", itemTrilhaCurtida.ItemTrilha.Nome), null);
                    //textoCurtida = string.Format("Você recebeu {0} na {1}", "curtidas", itemTrilhaCurtida.ItemTrilha.Nome);
                    break;
                case enumTipoCurtida.Descurtiu:
                    new ManterNotificacao().IncluirNotificacaoTrilha(itemTrilhaCurtida.UsuarioTrilha,
                        string.Format("Você recebeu {0} na {1}", "descurtidas", itemTrilhaCurtida.ItemTrilha.Nome), null);
                    //textoCurtida = string.Format("Você recebeu {0} na {1}", "descurtidas", itemTrilhaCurtida.ItemTrilha.Nome);
                    break;
                default:
                    break;
            }

            return true;
        }

        /// <summary>
        /// Inscrever um usuário em um nível de trilha. Este é o novo método para isso. O método antigo em MatriculaTrilhaServices.RegistrarMatriculatrilha está obsoleto.
        /// </summary>
        /// <param name="trilhaNivelId">ID do nível para inscrição</param>
        /// <param name="usuarioId">ID do usuário para inscrição</param>
        /// <param name="cpfAuditoria">CPF da tabela de auditoria</param>
        /// <returns></returns>
        public RetornoWebService InscreverUsuarioTrilhaNivel(int trilhaNivelId, int usuarioId, string cpfAuditoria)
        {
            var retorno = new RetornoWebService
            {
                Erro = 0,
                Mensagem = ""
            };

            var usuario = new ManterUsuario().ObterUsuarioPorID(usuarioId);

            if (usuario == null)
            {
                retorno.Erro = 1;
                retorno.Mensagem = "Usuário não encontrado";
                return retorno;
            }

            var nivel = new ManterTrilhaNivel().ObterTrilhaNivelPorID(trilhaNivelId);

            if (nivel == null)
            {
                retorno.Erro = 2;
                retorno.Mensagem = "Nível não encontrado";
                return retorno;
            }

            // Caso já esteja inscrito, pula o restante do método.
            if (nivel.ListaUsuarioTrilha.Any(x => x.Usuario.ID == usuarioId && x.NovasTrilhas == true && x.StatusMatricula != enumStatusMatricula.CanceladoAluno))
            {
                retorno.Erro = 3;
                retorno.Mensagem = "Usuário já inscrito neste nível";
                return retorno;
            }

            // Caso não tenha que fazer pré-requisitos.
            if (nivel.PreRequisito != null && nivel.PreRequisito.ListaUsuarioTrilha.All(x => x.Usuario.ID != usuarioId))
            {
                retorno.Erro = 4;
                retorno.Mensagem = "Pré-requisito " + nivel.PreRequisito.Nome + " não realizado.";
                return retorno;
            }

            // Se chegou até aqui, pode se inscrever.
            var usuarioTrilha = new UsuarioTrilha
            {
                DataInicio = DateTime.Now,
                DataLimite = DateTime.Now.AddDays(nivel.QuantidadeDiasPrazo),
                NivelOcupacional = usuario.NivelOcupacional,
                Uf = usuario.UF,
                TrilhaNivel = nivel,
                Usuario = usuario,
                StatusMatricula = enumStatusMatricula.Inscrito,
                Auditoria = new Auditoria(cpfAuditoria),
                AcessoBloqueado = false,
                NovasTrilhas = true
                // Identifica que essa matrícula é do novo sistema de trilhas, para separar das velhas matrículas.
            };

            var manterUsuarioTrilha = new ManterUsuarioTrilha();
            manterUsuarioTrilha.Salvar(usuarioTrilha);

            // Passa valores por referência para serem usados em thread async
            try
            {
                VerificarPontosConheciGame(nivel.ID, usuarioTrilha, new Classes.ConheciGame.ManterResposta(), new BMItemTrilha(), new BMTrilhaNivel(),
                    new ManterUsuarioTrilhaMoedas());

                // Enviar Email
                manterUsuarioTrilha.EnviarEmailBoasVindas(usuarioTrilha);
            }
            catch{

                //TODO: Criar log.
            }

            return retorno;
        }

        /// <summary>
        /// Verifica se o usuário já tem moedas nos pontos sebrae da trilha em que está se matriculando
        /// </summary>
        /// <param name="nivelId"></param>
        /// <param name="usuarioTrilha"></param>
        /// <param name="manterConheciGame"></param>
        /// <param name="manterItemTrilha"></param>
        /// <param name="manterTrilhaNivel"></param>
        /// <param name="manterMoedas"></param>
        private void VerificarPontosConheciGame(int nivelId, UsuarioTrilha usuarioTrilha, ConheciGame.ManterResposta manterConheciGame, BMItemTrilha manterItemTrilha,
            BMTrilhaNivel manterTrilhaNivel, ManterUsuarioTrilhaMoedas manterMoedas)
        {
            var conheciGame = manterConheciGame.ObterTodosQueryble();
            var nivel = manterTrilhaNivel.ObterPorID(nivelId);

            if (nivel.ListaPontoSebrae.Any(x => x.ListaMissoes.Any(m => m.ListaItemTrilha.Any(it => it.ID_TemaConheciGame > 0))))
            {
                foreach (var pontoSebrae in nivel.ListaPontoSebrae)
                {
                    foreach (var missao in pontoSebrae.ListaMissoes)
                    {
                        foreach (var itemTrilha in missao.ListaItemTrilha.Where(x => x.ID_TemaConheciGame > 0))
                        {
                            var acertos = conheciGame.Count(x => x.Usuario.ID_UsuarioSebrae == usuarioTrilha.Usuario.ID &&
                            x.Partida.Tema.ID == itemTrilha.ID_TemaConheciGame && x.Acertou == true);

                            if (acertos >= itemTrilha.QuantidadeAcertosTema)
                            {
                                manterMoedas.Incluir(usuarioTrilha, itemTrilha, null, 0, acertos);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Obter os dados dinâmicos do mapa do nível da trilha.
        /// </summary>
        /// <param name="dadosAcesso"></param>
        /// <returns>
        /// Para evitar criações desnecessárias de muitos DTOs, o resultado é dinâmico.
        /// Traz todos os dados necessários para popular o mapa.
        /// </returns>
        public dynamic ObterDadosMapa(UserIdentity dadosAcesso)
        {
            var lojasAsync = ObterLojasAsync(dadosAcesso.Matricula);

            ManterUsuarioTrilha manterUsuario = new ManterUsuarioTrilha();

            
            var matricula = dadosAcesso.Matricula;

            var dadosNivel = new ManterTrilhaNivel().ObterTodosTrilhaNivelIQueryable()
                .Where(x => x.ID == dadosAcesso.Nivel.ID)
                .Select(x =>
                    new
                    {
                        x.Mapa,
                        x.QuantidadeMoedasProvaFinal
                    })
                .FirstOrDefault();

            if (dadosNivel == null)
            {
                throw new ResponseException(enumResponseStatusCode.MapaNaoEncontrado);
            }

            var superAdmin = new ManterUsuario().ObterTodosIQueryable()
                .Join(new ManterUsuarioPerfil().ObterTodosIQueryable(), u => u.ID, p => p.Usuario.ID,
                    (usuario, usuarioPerfil) =>
                        new
                        {
                            usuario,
                            usuarioPerfil
                        }
                )
                .Where(x => x.usuario.ID == dadosAcesso.Usuario.ID &&
                            x.usuarioPerfil.Perfil.ID == (int)enumPerfil.AdministradorPortal)
                .Select(x => new { x.usuario.ID })
                .Any();


            var manterConfiguracaoSistema = new ManterConfiguracaoSistema();

            var repositorioUpload =
                manterConfiguracaoSistema.ObterConfiguracaoSistemaPorID((int)enumConfiguracaoSistema.RepositorioUpload)
                    .Registro;

            lojasAsync.Wait();

            //id será zero para o usuário de experimentação
            if(matricula.ID != 0)
            {
                // Retorno dinâmico, para dar flexibilidade ao resultado.
                return new
                {
                    LinkAcampamento = ConsultarLinkAcessoAcampamento(),

                    MapaId = (int)dadosNivel.Mapa,

                    //Buscar Admin Portal
                    SuperAdmin = superAdmin,

                    MoedasTotais = dadosNivel.QuantidadeMoedasProvaFinal,
                    MoedasConquistadas = lojasAsync.Result.Sum(x => x.MoedasConquistadas),

                    Lojas = lojasAsync.Result,

                    // Obter os participantes do nivel.
                    // São aleatórios e NÃO são agrupados por nada.
                    Participantes = ObterParticipantes(dadosAcesso.Matricula, dadosAcesso.Nivel, repositorioUpload),

                    Cronometro = matricula.Cronometro.TotalSeconds,

                    // Obtem o status atual do matriculado
                    StatusUsuario = matricula.StatusMatricula,

                    // Obtém se o usuário possui saldo para a prova final.
                    PossuiSaldoProvaFinal = matricula.PossuiSaldoProvaFinal()
                };
            }else{
                // Retorno dinâmico pro usuário de teste, para dar flexibilidade ao resultado.
                return new
                {
                    LinkAcampamento = ConsultarLinkAcessoAcampamento(),

                    MapaId = (int)dadosNivel.Mapa,

                    //Buscar Admin Portal
                    SuperAdmin = superAdmin,

                    MoedasTotais =  0,
                    MoedasConquistadas =  0,

                    Lojas = lojasAsync.Result,

                    // Obter os participantes do nivel.
                    // São aleatórios e NÃO são agrupados por nada.
                    Participantes = ObterParticipantes(dadosAcesso.Matricula, dadosAcesso.Nivel, repositorioUpload),

                    Cronometro =  0,

                    // Obtem o status atual do matriculado
                    StatusUsuario = matricula.StatusMatricula,

                    // Obtém se o usuário possui saldo para a prova final.
                    PossuiSaldoProvaFinal = false
                };

            }
            
        }

        private static dynamic ObterParticipantes(UsuarioTrilha usuarioTrilhaLogado, TrilhaNivel nivel,
            string repositorioUpload)
        {
            var participantesIds = nivel.ObterParticipantes(usuarioTrilhaLogado.ID);

            var participantes = new BMMapaTrilhas().ObterParticipantesMapa(participantesIds);

            var moedasNivel = new ManterTrilhaNivel().ObterTotalMoedasSolucoesSebrae(nivel.ID);

            var manterUsuario = new ManterUsuario();

            return participantes.Select(x =>
                new
                {
                    x.Id,
                    x.Nome,
                    x.Unidade,
                    Sexo = Usuario.ObterSexo(x.Sexo),
                    Nivel = new UsuarioTrilha
                    {
                        TrilhaNivel = nivel
                    }.ObterNivel(x.MoedasOuro, moedasNivel),
                    Imagem = manterUsuario.ObterImagem(x.Id, 180, x.FileServer, repositorioUpload)
                }).ToList();
        }

        private Task<List<DTOLoja>> ObterLojasAsync(UsuarioTrilha matricula)
        {
            return Task.Run(() => new BMMapaTrilhas().ObterLojasMapa(matricula.TrilhaNivel.ID, matricula.ID));
        }

        /// <summary>
        /// Obter os dados dinâmicos do mapa do nível da trilha.
        /// </summary>
        /// <param name="matricula">Matrícula do aluno no nível.</param>
        /// <param name="nivel">Nível da trilha (Mapa).</param>
        /// <returns>
        /// Para evitar criações desnecessárias de muitos DTOs, o resultado é dinâmico.
        /// Traz todos os dados necessários para popular o mapa.
        /// </returns>
        public dynamic ObterDadosMapaPreview(int nivelId)
        {
            var nivel = new ManterTrilhaNivel().ObterTrilhaNivelPorID(nivelId);

            // Fazer um "refresh" dos dados das lojas consultando novamente do banco para atualizar os sub objetos.
            var lojasIds = nivel.ListaPontoSebrae.Select(x => x.ID);

            var lojas =
                new ManterTrilhaTopicoTematico().ObterTodosTrilhaTopicoTematico().Where(x => lojasIds.Contains(x.ID));

            return lojas.Select(loja => new
            {
                Id = loja.ID,
                Nome = loja.NomeExibicao,
                //PossuiLider = loja.PossuiLider()
            }).ToList();
        }

        /// <summary>
        /// Obter dados dinâmicos da loja.
        /// </summary>
        /// <param name="acessoAtual">Matrícula do aluno no nível</param>
        /// <param name="lojaId">Nível da trilha (Mapa)</param>
        /// <param name="pontoSebrae">Loja selecionada (Tópico Temático)</param>
        /// <returns>
        /// Para evitar criações desnecessárias de muitos DTOs, o resultado é dinâmico.
        /// Traz todos os dados necessários para popular a loja.
        /// </returns>
        public dynamic ObterDadosLoja(UserIdentity acessoAtual, int lojaId, out PontoSebrae pontoSebrae)
        {
            // Propagar a loja para ser usada na busca das mensagens da guia.
            pontoSebrae = new ManterPontoSebrae().ObterTodosIqueryable().FirstOrDefault(x => x.ID == lojaId && x.Ativo);

            // Se a loja não existe, não permitir buscar dados.
            if (pontoSebrae == null)
                throw new ResponseException(enumResponseStatusCode.RegistroNaoEncontrado, "Este Ponto Sebrae está fechado no momento");
                        
            var missoes =
                new ManterMissao().ObterTodosIQueryable()
                    .Where(x => x.PontoSebrae.ID == lojaId && x.ListaItemTrilha.Count() > 0)
                    .Select(x => new {
                        ID = x.ID,
                        Nome = x.Nome,
                        TotalItens = x.ListaItemTrilha.Count(),
                        UsuarioConcluiu = x.UsuarioConcluiu(acessoAtual.Matricula),
                        UsuarioIniciou = x.UsuarioIniciou(acessoAtual.Matricula),
                    })
                    .ToList();

            var idsMissoes = missoes.Select(x => x.ID).ToList();

            var itensTrilha =
                ObterItensTrilha(
                    new ManterItemTrilha().ObterTodosIQueryable().Where(x => idsMissoes.Contains(x.Missao.ID))).ToList();

            PreencherParticipacoes(itensTrilha, acessoAtual.Matricula);

            // Propagar Ponto Sebrae
            foreach (var itemTrilha in itensTrilha)
            {
                itemTrilha.Missao.PontoSebrae = pontoSebrae;
            }

            var manterConfiguracaoSistema = new ManterConfiguracaoSistema();

            // Obter o endereço do SGUS para obter o caminho para as fotos dos perfis no mapeamento.
            var enderecoSgus =
                manterConfiguracaoSistema.ObterConfiguracaoSistemaPorID(
                    (int)enumConfiguracaoSistema.EnderecoSGUS)
                    .Registro;

            // Dummy pra receber os dados do líder, caso exista.
            KeyValuePair<UsuarioTrilha, TimeSpan>? dadosLider;

            var formasAquisicaoIds = itensTrilha.Select(x => x.FormaAquisicao.ID).Distinct().ToList();

            var imagens =
                new ManterFormaAquisicao().ObterTodosIQueryable()
                    .Where(x => formasAquisicaoIds.Contains(x.ID))
                    .Select(x => new
                    {
                        Id = x.ID,
                        x.Imagem
                    }).ToList();

            var manterUsuarioTrilhaMoedas = new ManterUsuarioTrilhaMoedas();

            int moedasPrata = 0;
            int moedasOuro = 0;
            bool possuiSaldoProvaFinal = false;

            if (acessoAtual.Matricula.ID != 0)
            {
                moedasPrata = manterUsuarioTrilhaMoedas.Obter(acessoAtual.Matricula, enumTipoMoeda.Prata);
                moedasOuro = manterUsuarioTrilhaMoedas.Obter(acessoAtual.Matricula, enumTipoMoeda.Ouro);
                possuiSaldoProvaFinal = acessoAtual.Matricula.PossuiSaldoProvaFinal();
            }
            
            var retorno = new
            {
                Nome = pontoSebrae.NomeExibicao.ToUpper(),
                MoedasPrata = moedasPrata,
                MoedasOuro = moedasOuro,
                CargaHoraria = (acessoAtual.Matricula.TrilhaNivel.CargaHoraria / 60),
                Cotacao = (acessoAtual.Matricula.TrilhaNivel.ValorPrataPorOuro == 0 ||
                           acessoAtual.Matricula.TrilhaNivel.ValorPrataPorOuro == null)
                    ? 10
                    : acessoAtual.Matricula.TrilhaNivel.ValorPrataPorOuro,
                // Deve ser maior do que zero para o sistema funcionar corretamente
                Missoes = missoes,
                SolucoesSebrae =
                    itensTrilha.Where(x => x.Usuario == null && x.Tipo != null)
                        .Select(it => ObterDadosSolucao(it, acessoAtual.Matricula, true, acessoAtual.Matricula))
                        .ToList(),
                SolucoesTrilheiro =
                    itensTrilha.Where(x => x.Usuario != null)
                        .Select(it => ObterDadosSolucao(it, acessoAtual.Matricula, false, acessoAtual.Matricula))
                        .ToList(),

                // Obtém se o usuário possui saldo para a prova final.
                PossuiSaldoProvaFinal = possuiSaldoProvaFinal,

                Imagens = imagens,
                Lider = (dadosLider = pontoSebrae.ObterLider()) != null
                    ? new
                    {
                        Id = dadosLider.Value.Key.Usuario.ID,
                        Nome = dadosLider.Value.Key.Usuario.ObterPrimeirosNomes(),
                        Imagem = dadosLider.Value.Key.Usuario.ObterLinkImagem(enderecoSgus),
                        TempoConclusao =
                            dadosLider.Value.Key.ObterTempoConclusaoFormatado(dadosLider.Value.Value)
                    }
                    : null
            };

            // Filtra soluções sebrae que não tem um link para acesso e que o usuário não esteja como "Não Inscrito"
            retorno.SolucoesSebrae.RemoveAll(
                x =>
                    x.Tipo == enumTipoItemTrilha.Solucoes && string.IsNullOrEmpty(x.LinkAcesso) &&
                    x.Status != enumStatusParticipacaoItemTrilha.NaoInscrito);

            // Armazenar log de líderes para o usuário logado.
            if(acessoAtual.Matricula.ID != 0)
                ArmazenarLogLiderLoja(dadosLider, acessoAtual.Matricula, pontoSebrae);

            return retorno;
        }

        private static IQueryable<ItemTrilha> ObterItensTrilha(IQueryable<ItemTrilha> query)
        {
            return query
                .Where(
                    x =>
                        x.Ativo == true
                        &&
                        (x.Usuario != null ||
                         (x.Usuario == null && x.Tipo != null &&

                          // Reprodução do método ItemTrilha.PodeExibir(). Não dá pra usar o método aqui
                          // porque o LINQtoSQL não consegue traduzir métodos customs pra SQL, então a
                          // manutenção pode ficar prejudicada, pois quando alterar o método tem que
                          // alterar aqui também.
                          (x.Tipo.ID != (int)enumTipoItemTrilha.Atividade || x.Questionario != null) &&
                          (x.Tipo.ID != (int)enumTipoItemTrilha.Solucoes || x.SolucaoEducacionalAtividade != null)
                             )
                            )
                ).Select(x => new ItemTrilha
                {
                    ID = x.ID,
                    Nome = x.Nome,
                    Local = x.Local,
                    Moedas = x.Moedas,
                    CargaHoraria = x.CargaHoraria,
                    SolucaoEducacionalAtividade = x.SolucaoEducacionalAtividade != null
                        ? new SolucaoEducacional
                        {
                            ID = x.SolucaoEducacionalAtividade.ID
                        }
                        : null,
                    Questionario = x.Questionario != null
                        ? new Questionario
                        {
                            ID = x.Questionario.ID
                        }
                        : null,
                    Missao = new Missao
                    {
                        ID = x.Missao.ID
                    },
                    Usuario = x.Usuario != null
                        ? new Usuario
                        {
                            ID = x.Usuario.ID
                        }
                        : null,
                    Tipo = x.Tipo != null
                        ? new TipoItemTrilha
                        {
                            ID = x.Tipo.ID
                        }
                        : null,
                    FormaAquisicao = new FormaAquisicao
                    {
                        ID = x.FormaAquisicao.ID
                    },
                    Avaliacoes = x.Avaliacoes
                });
        }

        private void ArmazenarLogLiderLoja(KeyValuePair<UsuarioTrilha, TimeSpan>? dadosLider, UsuarioTrilha matricula, PontoSebrae pontoSebrae)
        {
            var logLider = new LogLider
            {
                Aluno = matricula,
                Lider = dadosLider != null ? dadosLider.Value.Key : null,
                Tempo = dadosLider != null ? (TimeSpan?)dadosLider.Value.Value : null,
                PontoSebrae = pontoSebrae
            };

            new ManterLogLider().Salvar(logLider);
        }

        /// <summary>
        /// Obter o Ranking por Trilha.
        /// </summary>
        /// <param name="nivel">Nível da trilha do Ranking.</param>
        /// <param name="page">Página.</param>
        /// <param name="itensPerPage">Itens por página</param>
        /// <param name="nome">Nome do aluno para busca</param>
        /// <returns></returns>
        public dynamic ObterRanking(TrilhaNivel nivel, int page, string nome, string uf, int? itensPerPage, UserIdentity user)
        {
            itensPerPage = itensPerPage != null && itensPerPage != 0 ? itensPerPage.Value : 10;

            nome = nome?.ToLower().Trim();

            if (nome != null)
            {
                // Remover espaços duplicados.
                while (nome.Contains("  "))
                {
                    nome = nome.Replace("  ", " ");
                }
            }

            // Contar o total apenas se houver o filtro nome, pois seria uma query inútil.
            var total = string.IsNullOrWhiteSpace(nome) == false
                ? new ManterUsuarioTrilha().ObterTodosIQueryable()
                    .Where(x => x.TrilhaNivel.ID == nivel.ID)
                    .Select(x => new { x.ID }).Count()
                : 0;

            var bmRanking = new BMRankingTrilhas();

            List<DTOTrilhaRanking> ranking = new List<DTOTrilhaRanking>();
            if (string.IsNullOrWhiteSpace(nome) || nome.Equals("sebrae"))
            {
                ranking = bmRanking.ObterRanking(nivel, itensPerPage.Value, page).ToList();
            }
            else
            {
                ranking = bmRanking.ObterRanking(nivel, total).Where(x => x.Nome.ToLower().Contains(nome)).ToList();
            }

            if (uf != null)
                ranking = ranking.Where(x => x.Unidade.Contains(uf)).ToList();


            bool temUsuarioAtual = false;
            foreach (var r in ranking)
            {
                if(r.ID == user.Usuario.ID)
                {
                    temUsuarioAtual = true;
                    break;
                }
            }

            if (!temUsuarioAtual)
            {
                
                var m = new ManterUsuarioTrilha();
                
                var usuarioAtual = bmRanking.ObterUsuario(nivel.ID, user.Usuario.ID);
                if(usuarioAtual != null)
                {
                    ranking.RemoveAt(ranking.Count() - 1);
                    ranking.Add(usuarioAtual);
                }

            }

            // Refaz o total em caso de buscas pelo nome, pra obter o total correto.
            if (string.IsNullOrWhiteSpace(nome) == false)
            {
                total = ranking.Count();
            }

            return new
            {
                // Dados para paginação.
                Pagina = page,
                ItensPorPagina = itensPerPage.Value,
                Total = total,

                Ranking = ranking
            };
        }

        public enumTipoQuestionarioAssociacao ObterTipoQuestionario(UserIdentity AcessoAtual, int? tipo, int? itemTrilha, bool superAdmin = false)
        {
            if (tipo == null && itemTrilha == null)
                throw new ResponseException(enumResponseStatusCode.QuestionarioInvalido,
                    "Erro ao obter questionário. Tente novamente.");

            // Se o tipo não for informado, o itemTrilha está, então o tipo para o ItemTrilha
            // sempre será AtividadeTrilha.
            var tipoQuestionarioAssociacao = tipo != null
                ? (enumTipoQuestionarioAssociacao)tipo
                : enumTipoQuestionarioAssociacao.AtividadeTrilha;

            // Especificamente no caso do questionário pós (Entrevista), só pode ser respondida se a matrícula do aluno for aprovada.
            if (!superAdmin && tipoQuestionarioAssociacao == enumTipoQuestionarioAssociacao.Pos &&
                AcessoAtual.Matricula.StatusMatricula != enumStatusMatricula.Aprovado)
                throw new ResponseException(enumResponseStatusCode.EntrevistaBloqueada);

            switch (tipoQuestionarioAssociacao)
            {
                case enumTipoQuestionarioAssociacao.Pos:
                case enumTipoQuestionarioAssociacao.Pre:
                case enumTipoQuestionarioAssociacao.Prova:
                    if (!AcessoAtual.Nivel.ListaQuestionarioAssociacao.Any(
                        q =>
                            q.Evolutivo == false &&
                            q.TipoQuestionarioAssociacao.ID == (int)tipoQuestionarioAssociacao))
                        throw new ResponseException(enumResponseStatusCode.QuestionarioNaoEncontrado);

                    break;

                case enumTipoQuestionarioAssociacao.AtividadeTrilha:
                    if (itemTrilha == null)
                        throw new ResponseException(enumResponseStatusCode.SolucaoSebraeNaoInformada);
                    break;
                default:
                    throw new ResponseException(enumResponseStatusCode.TipoQuestionarioInvalido);
            }

            return tipoQuestionarioAssociacao;
        }

        /// <summary>
        /// Obter questionário para ser respondido de acordo com o usuário, nível e tipo de associação.
        /// </summary>
        /// <param name="user">Usuário que vai responder o questionário.</param>
        /// <param name="nivel">Nível da trilha (Mapa).</param>
        /// <param name="tipoAssociacao">Tipo de associação. Ex.: Pré, Pós ou Prova.</param>
        /// <param name="itemTrilhaId">ID do ItemTrilha caso questionário seja Atividade Trilha de algum item.</param>
        /// <param name="matricula">Matrícula do aluno que será relacionada com o Questionário tipo Atividade Trilha.</param>
        /// <param name="superAdmin"></param>
        /// <returns></returns>
        public dynamic ObterQuestionario(UserIdentity usuarioLogado,
            enumTipoQuestionarioAssociacao tipoAssociacao, int? itemTrilhaId = null, bool superAdmin = false)

        {
            // Fazer a ponte entre a nova interface de trilhas e a velha.
            // Só transmite os dados que interessam para os questionários de trilhas.
            var pQuestionario = new DTOCadastroQuestionarioParticipacao
            {
                IdUsuario = usuarioLogado.Usuario.ID,
                IdTrilhaNivel = usuarioLogado.Nivel.ID,
                IdItemTrilha = itemTrilhaId,
                Pre = tipoAssociacao == enumTipoQuestionarioAssociacao.Pre,
                Pos = tipoAssociacao == enumTipoQuestionarioAssociacao.Pos,
                Prova = tipoAssociacao == enumTipoQuestionarioAssociacao.Prova,
                AtividadeTriha = tipoAssociacao == enumTipoQuestionarioAssociacao.AtividadeTrilha
            };

            // Se for entrevista, verifica se já respondeu. Somente se não for SuperAdmin
            if (!superAdmin && tipoAssociacao == enumTipoQuestionarioAssociacao.Pos &&
                RespondeuEntrevista(usuarioLogado.Matricula, usuarioLogado.Nivel))
                throw new ResponseException(enumResponseStatusCode.EntrevistaRespondida,
                    enumResponseStatusCode.EntrevistaRespondida.GetDescription());

            // Verifica se for Prova Prova
            if (!superAdmin && tipoAssociacao == enumTipoQuestionarioAssociacao.Prova)
                ValidarAcessoProvaFinal(usuarioLogado);

            if (!superAdmin && itemTrilhaId.HasValue)
            {
                var itemTrilha = new ManterItemTrilha().ObterItemTrilhaPorID(itemTrilhaId.Value);

                if (itemTrilha.ObterStatusParticipacoesItemTrilha(usuarioLogado.Matricula) ==
                    enumStatusParticipacaoItemTrilha.Aprovado)
                    throw new ResponseException(enumResponseStatusCode.ErroRegraNegocioTrilhas,
                        "Não é possível executar esta operação. O Usuário já registrou sua participação.");
            }

            DTOQuestionarioParticipacao dtoQuestionario;

            // Retorna um DTO simulado caso seja SuperAdmin.
            if (superAdmin)
            {
                dtoQuestionario =
                    new SgusWebService.ManterQuestionarioParticipacao().ObterQuestionarioSimulado(pQuestionario, tipoAssociacao, usuarioLogado.Matricula);
            }
            else
            {
                dtoQuestionario =
                new SgusWebService.ManterQuestionarioParticipacao().ListarQuestionarioParticipacao(pQuestionario,
                    usuarioLogado.Usuario.CPF, true, usuarioLogado.Matricula);
            }

            if (dtoQuestionario.DataParticipacao != null)
                throw new ResponseException(enumResponseStatusCode.ErroRegraNegocioTrilhas,
                    "Não é possível executar esta operação. O Usuário já registrou sua participação.");

            var primeiraPergunta = dtoQuestionario.ListaItemQuestionarioParticipacao.FirstOrDefault();

            // Obter o primeiro agrupador de acordo com o primeiro informativo.
            var idAgrupador = primeiraPergunta != null &&
                              primeiraPergunta.TipoItemQuestionario.ID == (int)enumTipoItemQuestionario.AgrupadorDeQuestoes
                ? (int?)primeiraPergunta.ID
                : null;

            // Selecionar somente o que precisa e desprezar vários dados não necessários para aliviar a requisição.
            return new
            {
                Id = dtoQuestionario.ID,
                Trilha = usuarioLogado.Nivel.Trilha.Nome,
                TrilhaNivel = usuarioLogado.Nivel.Nome,
                EnunciadoPre = dtoQuestionario.TextoEnunciadoPre,
                EnunciadoPos = dtoQuestionario.TextoEnunciadoPos,
                dtoQuestionario.DataGeracao,
                DataLimite = dtoQuestionario.DataLimiteParticipacao,
                TipoAssociacao = dtoQuestionario.TipoQuestionarioAssociacao.ID,
                dtoQuestionario.TipoQuestionario,

                // As perguntas já vêm ordenadas da criação do DTO.
                ItensQuestionario = dtoQuestionario.ListaItemQuestionarioParticipacao.Select(iqp => new
                {
                    Id = iqp.ID,
                    Tipo = iqp.TipoItemQuestionario.ID,
                    iqp.Questao,
                    Feedback = iqp.Feedback,
                    // Obtém o agrupador das questões, para os casos em que questões precisam ficar agrupadas por enunciados do tipo Informação.
                    Agrupador =
                        (idAgrupador =
                            (iqp.TipoItemQuestionario.ID == (int)enumTipoItemQuestionario.AgrupadorDeQuestoes
                                ? iqp.ID
                                : idAgrupador)) == iqp.ID
                            ? null
                            : idAgrupador,

                    // Só exibir dado se a questão não for do tipo Informação.
                    Estilo =
                        iqp.TipoItemQuestionario.ID == (int)enumTipoItemQuestionario.AgrupadorDeQuestoes
                            ? null
                            : (iqp.EstiloItemQuestionario != null ? (int?)iqp.EstiloItemQuestionario.ID : null),

                    // As opções e as colunas precisam ser obtidas através de mágica avançada.
                    Opcoes = ObterOpcoes(iqp),
                    Colunas = ObterColunas(iqp),
                    RespostaObrigatoria = iqp.RespostaObrigatoria
                })
            };
        }

        /// <summary>
        /// Obtém as opções do ItemQuestionario.
        /// </summary>
        /// <param name="iqp"></param>
        /// <returns></returns>
        private static dynamic ObterOpcoes(DTOItemQuestionarioParticipacao iqp)
        {
            if (iqp.TipoItemQuestionario.ID == (int)enumTipoItemQuestionario.ColunasRelacionadas)
                return null;

            return iqp.ListaOpcoes.Select(iqpo => new
            {
                Id = iqpo.ID,
                Opcao = iqpo.Nome
            });
        }

        /// <summary>
        /// Obtém as opções separadas em grupos de colunas.
        /// </summary>
        /// <param name="iqp"></param>
        /// <returns></returns>
        private static dynamic ObterColunas(DTOItemQuestionarioParticipacao iqp)
        {
            // Se for Colunas Relacionadas, é diferentão, pioneiro.
            if (iqp.TipoItemQuestionario.ID == (int)enumTipoItemQuestionario.ColunasRelacionadas)
            {
                return new
                {
                    Coluna1 = iqp.ListaOpcoes.Where(x => x.OpcaoVinculada != null).Select(iqpo => new
                    {
                        Id = iqpo.ID,
                        Opcao = iqpo.Nome,
                        ValorResposta = iqpo.OpcaoVinculada?.IndexOpcaoSelecionada
                    }),
                    Coluna2 = iqp.ListaOpcoes.Where(x => x.OpcaoVinculada == null).Select(iqpo => new
                    {
                        Id = iqpo.ID,
                        Opcao = iqpo.Nome
                    })
                };
            }

            return null;
        }

        /// <summary>
        /// Informar respostas do questionário.
        /// </summary>
        /// <param name="matricula">Matrícula do usuário que está preenchendo o questionário.</param>
        /// <param name="respostasViewModel">ViewModel com as respostas informdas.</param>
        /// <returns></returns>
        public dynamic InformarRespostasQuestionario(UserIdentity acessoAtual,
            DTOInformarRespostaQuestionario respostasViewModel, bool superAdmin = false)
        {
            if (superAdmin)
            {
                var usuario = new ManterUsuario().ObterUsuarioPorID(acessoAtual.Usuario.ID);

                var podeSerSuperAdmin = usuario.ListaPerfil.Any(x => x.Perfil.ID == (int)enumPerfil.AdministradorPortal);

                if (podeSerSuperAdmin == false)
                    throw new ResponseException(enumResponseStatusCode.PermissaoNegadaSuperAcesso);
            }

            var dtoQuestionarioParticipacao = new DTOQuestionarioParticipacao();

            // apenas para provas das trilhas (Aprovado, Nota, Mensagem)
            Tuple<bool, decimal, string> retornoQuestionario;

            // Só executa se NÃO for SuperAdmin.
            if (superAdmin == false)
            {
                try
                {
                    // Fazer a tradução dos parâmetros novos para o que é esperado no método velho.
                    var questionarioParams = new DTOQuestionarioParticipacao
                    {
                        ID = respostasViewModel.Id,

                        ListaItemQuestionarioParticipacao =
                            respostasViewModel.Questoes.Select(iqp => new DTOItemQuestionarioParticipacao
                            {
                                ID = iqp.Id,
                                // Não sei porque ele espera uma List aqui, mas não posso alterar sem comprometer o resto do sistema. Lamento.
                                Resposta = new List<string> { iqp.Resposta },
                                ListaOpcoes = ObterOpcoesSelecionadas(iqp)
                            }).ToList()
                    };

                    // Informar as respostas.
                    new SgusWebService.ManterQuestionarioParticipacao(acessoAtual.Matricula.Usuario.CPF)
                        .InformarRespostas
                        (
                            questionarioParams,
                            acessoAtual.Matricula.Usuario.CPF, out retornoQuestionario,
                            ref dtoQuestionarioParticipacao,
                            true);
                }
                catch (AcademicoException ex)
                {
                    // Usuário já respondeu o questionário.
                    if (ex.ExceptionCode == 10)
                        throw new ResponseException(enumResponseStatusCode.ErroRegraNegocioSgus, ex.Message);

                    throw;
                }

                var tipoQuestionario = (enumTipoQuestionario)dtoQuestionarioParticipacao.TipoQuestionario;

                var questionarioParticipacao = tipoQuestionario == enumTipoQuestionario.AvaliacaoProva ||
                                               tipoQuestionario == enumTipoQuestionario.AtividadeTrilha
                    ? new ManterQuestionarioParticipacao().ObterQuestionarioParticipacaoPorId(
                        dtoQuestionarioParticipacao.ID)
                    : null;

                switch (tipoQuestionario)
                {
                    case enumTipoQuestionario.AvaliacaoProva:
                        return new
                        {
                            Aprovacao = retornoQuestionario.Item1,
                            Usuario = acessoAtual.Matricula.Usuario.Nome,
                            DataNovaProva = acessoAtual.Matricula.DataLiberacaoNovaProva != null
                                ? (retornoQuestionario.Item1
                                    ? 0
                                    : (acessoAtual.Matricula.DataLiberacaoNovaProva.Value.Date - DateTime.Now.Date)
                                        .Days)
                                : 0,
                            QuestoesGabarito = ObterGabaritoComFeedback(questionarioParticipacao),
                            NotaFinal = questionarioParticipacao != null ? questionarioParticipacao.ObterNota() : 0,
                            NotaMinima = acessoAtual.Nivel.NotaMinima ?? 0,
                            ExibirGabarito =
                                questionarioParticipacao != null
                                    ? questionarioParticipacao.ListaItemQuestionarioParticipacao.Any(
                                        x => x.ExibeFeedback == true && !string.IsNullOrWhiteSpace(x.Feedback))
                                    : false
                        };
                    case enumTipoQuestionario.AtividadeTrilha:
                        var itemTrilhaParticipacao =
                            questionarioParticipacao != null
                                ? questionarioParticipacao.ListaItemTrilhaParticipacao.FirstOrDefault()
                                : null;

                        if (itemTrilhaParticipacao == null)
                            throw new ResponseException(enumResponseStatusCode.SolucaoSebraeNaoVinculada);

                        if (itemTrilhaParticipacao.Autorizado == true)
                            new ManterUsuarioTrilhaMoedas().Incluir(acessoAtual.Matricula,
                                itemTrilhaParticipacao.ItemTrilha, null, 0,
                                itemTrilhaParticipacao.ItemTrilha.Moedas ?? 0);

                        return new
                        {
                            Aprovacao = retornoQuestionario.Item1,
                            Moedas = (int?)(itemTrilhaParticipacao.ItemTrilha.Moedas ?? 0),
                            PossuiSaldoProvaFinal = itemTrilhaParticipacao.UsuarioTrilha.PossuiSaldoProvaFinal(),
                            QuestoesGabarito = ObterGabaritoComFeedback(questionarioParticipacao),
                            NotaFinal = questionarioParticipacao.ObterNota(),
                            NotaMinima = questionarioParticipacao.NotaMinima ?? 0,
                            ExibirGabarito =
                                questionarioParticipacao.ListaItemQuestionarioParticipacao.Any(
                                    x => x.ExibeFeedback == true && !string.IsNullOrWhiteSpace(x.Feedback))
                        };
                    case enumTipoQuestionario.Pesquisa:
                        return true;
                    default:
                        throw new ResponseException(enumResponseStatusCode.QuestionarioInvalido);
                }
            }

            // Caso seja SuperAdmin, calcular as respostas e retornar diretamente pro usuário.
            var questionario = new ManterQuestionario().ObterQuestionarioPorID(respostasViewModel.Id);

            if (questionario == null)
                throw new ResponseException(enumResponseStatusCode.QuestionarioNaoEncontrado);

            switch ((enumTipoQuestionario)questionario.TipoQuestionario.ID)
            {
                case enumTipoQuestionario.AvaliacaoProva:
                    bool aprovado;

                    return new
                    {
                        Aprovacao = (aprovado = respostasViewModel.IsAprovado(questionario, acessoAtual.Nivel.NotaMinima ?? 0)),
                        Usuario = acessoAtual.Matricula.Usuario.Nome,
                        DataNovaProva =
                            aprovado
                                ? 0
                                : (DateTime.Now.AddDays(30) - DateTime.Now.Date).Days, // Em 30 dias como padrão.
                        QuestoesGabarito = respostasViewModel.ObterGabaritoComFeedback(questionario),
                        NotaFinal = respostasViewModel.ObterNota(questionario),
                        NotaMinima = acessoAtual.Nivel.NotaMinima ?? 0,
                        ExibirGabarito =
                                questionario.ListaItemQuestionario.Any(
                                    x => x.ExibeFeedback == true && !string.IsNullOrWhiteSpace(x.Feedback))
                    };
                case enumTipoQuestionario.AtividadeTrilha:
                    var itemTrilha =
                        questionario.ListaItemTrilha.FirstOrDefault();

                    int? moedas = 0;

                    if (itemTrilha != null)
                        moedas = itemTrilha.Moedas ?? 0;

                    return new
                    {
                        Aprovacao = respostasViewModel.IsAprovado(questionario),
                        Moedas = moedas,
                        QuestoesGabarito = respostasViewModel.ObterGabaritoComFeedback(questionario),
                        NotaFinal = respostasViewModel.ObterNota(questionario),
                        NotaMinima = questionario.NotaMinima ?? 0,
                        ExibirGabarito =
                                questionario.ListaItemQuestionario.Any(
                                    x => x.ExibeFeedback == true && !string.IsNullOrWhiteSpace(x.Feedback))
                    };
                case enumTipoQuestionario.Pesquisa:
                    return true;
                default:
                    throw new ResponseException(enumResponseStatusCode.QuestionarioInvalido);
            }
        }

        /// <summary>
        /// Verifica as missões concluidas pelo usuário e adiciona uma medalha por missão.
        /// </summary>
        /// <param name="lojaId"></param>
        /// <param name="acessoAtual"></param>
        public void VerificarConclusaoMissao(UserIdentity acessoAtual)
        {

           TrilhaNivel nivel = new ManterTrilhaNivel().ObterTrilhaNivelPorID(acessoAtual.Nivel.ID);
           List<Missao> missoes = nivel.ObterMissoes()
                                         .Where(x => x.UsuarioConcluiu(acessoAtual.Matricula)).ToList();

            foreach (var missao in missoes)
            {
                new ManterMissaoMedalha().registrarMedalha(new MissaoMedalha {
                    Medalhas = 1,
                    Missao = missao,
                    UsuarioTrilha = acessoAtual.Matricula,
                    DataRegistro = DateTime.Now
                });
            }            

        }

        /// <summary>
        /// Obter a participação do usuário no questionário do tipo múltipla escolha, permite apenas visualização do questionário em trilhas
        /// </summary>
        /// <param name="acessoAtual">Usuário logado no web service</param>
        /// <param name="itemTrilha">Id do item trilha vinculado ao questionário</param>
        /// <returns></returns>
        public dynamic ObterQuestionarioParticipacao(UserIdentity acessoAtual, int itemTrilha)
        {

            var usuarioAtual = new ManterUsuario().ObterUsuarioPorID(acessoAtual.Usuario.ID);

            var questionario = new ManterItemTrilha().ObterItemTrilhaPorID(itemTrilha).Questionario;            
            
            var primeiraPergunta = questionario.ListaItemQuestionario.FirstOrDefault();            

            // Obter o primeiro agrupador de acordo com o primeiro informativo.
            var idAgrupador = primeiraPergunta != null &&
                              primeiraPergunta.TipoItemQuestionario.ID == (int)enumTipoItemQuestionario.AgrupadorDeQuestoes
                ? (int?)primeiraPergunta.ID
                : null;

            var listaQuestionario = questionario.ListaQuestionarioParticipacao
                    .Where(x => x.IdItemTrilha == itemTrilha).LastOrDefault();

            List<QuestionarioParticipacao> listaQuestionarioList = new List<QuestionarioParticipacao>();
            listaQuestionarioList.Add(listaQuestionario);

            var participacoes = new Sebrae.Academico.BP.Services.SgusWebService.ManterQuestionarioParticipacao().GetListaDto(listaQuestionarioList);
            

            List<Object> listParticipacoes = new List<object>();

            foreach (var participacao in participacoes)
            {
                listParticipacoes.AddRange(participacao.ListaItemQuestionarioParticipacao.Select(iqp => new
                {
                    Id = iqp.ID,
                    Tipo = iqp.TipoItemQuestionario.ID,
                    iqp.Questao,
                    Feedback = iqp.Feedback,
                    // Obtém o agrupador das questões, para os casos em que questões precisam ficar agrupadas por enunciados do tipo Informação.
                    Agrupador =
                        (idAgrupador =
                            (iqp.TipoItemQuestionario.ID == (int)enumTipoItemQuestionario.AgrupadorDeQuestoes
                                ? iqp.ID
                                : idAgrupador)) == iqp.ID
                            ? null
                            : idAgrupador,

                    // Só exibir dado se a questão não for do tipo Informação.
                    Estilo =
                        iqp.TipoItemQuestionario.ID == (int)enumTipoItemQuestionario.AgrupadorDeQuestoes
                            ? null
                            : (iqp.EstiloItemQuestionario != null ? (int?)iqp.EstiloItemQuestionario.ID : null),

                    // As opções e as colunas precisam ser obtidas através de mágica avançada.
                    Opcoes = ObterOpcoes(iqp),
                    Colunas = ObterColunas(iqp)
                }));
            }

            return new
            {
                Id = questionario.ID,
                Visualizando = true,
                Trilha = acessoAtual.Nivel.Trilha.Nome,
                TrilhaNivel = acessoAtual.Nivel.Nome,
                TipoQuestionario = questionario.TipoQuestionario.ID,
                ItensQuestionario = listParticipacoes
            };
        }

        private static List<DTOItemQuestionarioParticipacaoOpcoes> ObterOpcoesSelecionadas(DTOQuestao iqp)
        {
            switch ((enumTipoItemQuestionario)iqp.Tipo)
            {
                case enumTipoItemQuestionario.MultiplaEscolha:
                    return
                        iqp.RespostasSelecionadas.Where(x => x.Checked)
                            .Select(o => new DTOItemQuestionarioParticipacaoOpcoes
                            {
                                ID = o.Id,
                                RespostaSelecionada = new List<bool?> { true }
                            }).ToList();
                case enumTipoItemQuestionario.ColunasRelacionadas:
                    return iqp.Opcoes.Select(o => new DTOItemQuestionarioParticipacaoOpcoes
                    {
                        ID = o.Id,
                        RespostaSelecionada = new List<bool?> { true },
                        IndexOpcaoSelecionada = o.Valor - 1
                    }).ToList();
                default:
                    var listaRetorno = new List<DTOItemQuestionarioParticipacaoOpcoes>();

                    if (iqp.RespostaSelecionada > 0)
                    {
                        listaRetorno.Add(
                            new DTOItemQuestionarioParticipacaoOpcoes
                            {
                                ID = iqp.RespostaSelecionada,
                                // Não sei porque ele espera uma List aqui, mas não posso alterar sem comprometer o resto do sistema. Lamento.
                                RespostaSelecionada = new List<bool?> { true }
                            });
                    }

                    return listaRetorno;
            }
        }

        private dynamic ObterGabaritoComFeedback(QuestionarioParticipacao questionarioParticipacao)
        {
            var porcentagemTotal = questionarioParticipacao.ListaItemQuestionarioParticipacao.Sum(x => x.ObterPeso());

            return questionarioParticipacao.ListaItemQuestionarioParticipacao.Select(iqp => new
            {
                Questao = iqp.Questao,
                Resposta = ObterRespostaSelecionada(iqp),
                Porcentagem = ObterPorcentagem(iqp, porcentagemTotal)
            });
        }

        /// <summary>
        /// Obtém a porcentagem da questão em relação às outras questões e ao total do questionário.
        /// </summary>
        /// <param name="iqp"></param>
        /// <param name="porcentagemTotal"></param>
        /// <returns></returns>
        private decimal? ObterPorcentagem(ItemQuestionarioParticipacao iqp, int porcentagemTotal)
        {
            if (porcentagemTotal == 0)
                return 0;

            var pesoQuestao = iqp.ObterPeso();

            return (pesoQuestao * 100) / porcentagemTotal;
        }

        /// <summary>
        /// Montar dinamicamente o gabarito com as respostas selecionadas, respostas corretas e com o texto de Feedback da questão.
        /// Retorna as respostas selecionadas e se elas estão corretas ou não. Caso uma resposta correta não esteja selecionada,
        /// ela também é retornada para referência.
        /// </summary>
        /// <param name="itemQuestionarioParticipacao"></param>
        /// <returns></returns>
        private dynamic ObterRespostaSelecionada(ItemQuestionarioParticipacao itemQuestionarioParticipacao)
        {
            var respostasSelecionadas = new List<dynamic>();
            var respostasCorretas = new List<dynamic>();

            switch ((enumTipoItemQuestionario)itemQuestionarioParticipacao.TipoItemQuestionario.ID)
            {
                case enumTipoItemQuestionario.Objetiva:
                case enumTipoItemQuestionario.VerdadeiroOuFalso:
                    var respostaSelecionada =
                        itemQuestionarioParticipacao.ListaOpcoesParticipacao.FirstOrDefault(
                            x => x.RespostaSelecionada == true);

                    if (respostaSelecionada != null)
                    {
                        respostasSelecionadas.Add(new
                        {
                            Texto = respostaSelecionada.Nome,
                            IsCorreto = respostaSelecionada.RespostaCorreta == true
                        });

                        if (respostaSelecionada.RespostaCorreta != true)
                        {
                            var respostaCorreta = itemQuestionarioParticipacao.ListaOpcoesParticipacao.FirstOrDefault(
                                x => x.RespostaCorreta == true);

                            if (respostaCorreta != null)
                                respostasCorretas.Add(respostaCorreta.Nome);
                        }
                    }

                    break;
                case enumTipoItemQuestionario.MultiplaEscolha:
                    foreach (var opcao in itemQuestionarioParticipacao.ListaOpcoesParticipacao)
                    {
                        if (opcao.RespostaSelecionada == true)
                        {
                            respostasSelecionadas.Add(new
                            {
                                Texto = opcao.Nome,
                                IsCorreto = opcao.RespostaCorreta == true
                            });
                        }
                        else
                        {
                            if (opcao.RespostaCorreta == true)
                                respostasCorretas.Add(opcao.Nome);
                        }
                    }

                    break;
                case enumTipoItemQuestionario.ColunasRelacionadas:
                    foreach (
                        var opcao in
                            itemQuestionarioParticipacao.ListaOpcoesParticipacao.Where(x => x.OpcaoSelecionada != null))
                    {
                        respostasSelecionadas.Add(new
                        {
                            Texto = opcao.Nome,
                            IsCorreto =
                                opcao.OpcaoVinculada != null && opcao.OpcaoVinculada.ID == opcao.OpcaoSelecionada.ID,
                            OpcaoSelecionada = opcao.OpcaoSelecionada.Nome,
                            RespostaCorreta =
                                opcao.OpcaoVinculada != null && opcao.OpcaoVinculada.ID == opcao.OpcaoSelecionada.ID
                                    ? opcao.OpcaoVinculada.Nome
                                    : null
                        });
                    }

                    break;
                case enumTipoItemQuestionario.Discursiva:
                case enumTipoItemQuestionario.AgrupadorDeQuestoes:
                case enumTipoItemQuestionario.Diagnostico:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new
            {
                TipoQuestao = itemQuestionarioParticipacao.TipoItemQuestionario.ID,
                Feedback =
                    itemQuestionarioParticipacao.ExibeFeedback == true ? itemQuestionarioParticipacao.Feedback : null,
                RespostasSelecionadas = respostasSelecionadas,
                RespostasCorretas = respostasCorretas
            };
        }

        private string ConsultarLinkAcessoAcampamento()
        {
            // TODO: Ainda será feita alteração relacionar o fornecedor com a trilha e para buscar a URL desse fornecedor.
            // TODO: Para ver como estava antes, consultar o commit dessa tarefa (#3581)
            return
                "https://sebrae.facebook.com/groups/255766778279792/?multi_permalinks=255770411612762&notif_id=1509369473620185&notif_t=group_activity";
        }

        private string ConsultarLinkJogo(ItemTrilha itemTrilha, UsuarioTrilha matricula)
        {
            try
            {
                if ((enumTipoItemTrilha)itemTrilha.Tipo.ID != enumTipoItemTrilha.Jogo)
                    return null;

                var fornecedorId =
                    int.Parse(
                        new ManterConfiguracaoSistema().ObterConfiguracaoSistemaPorID(
                            (int)enumConfiguracaoSistema.IDFornecedorVivencia).Registro);

                var fornecedor = new ManterFornecedor().ObterFornecedorPorID(fornecedorId);

                if (fornecedor == null)
                    return null;

                var url =
                    new ManterConfiguracaoSistema().ObterConfiguracaoSistemaPorID(
                        (int)enumConfiguracaoSistema.LinkAcessoVivencia).Registro;

                var token =
                    JsonWebToken.Encode(
                        new { id = matricula.ID, fase = (int)itemTrilha.FaseJogo, itrid = itemTrilha.ID },
                        fornecedor.TextoCriptografia, JwtHashAlgorithm.HS256);

                return string.Format("{0}?token={1}", url, token);
            }
            catch (Exception)
            {
                return null;
            }
        }


        /// <summary>
        /// Método que verifica o link do acesso do curso.
        /// O link tem a opção de vir pelo moodle, curso online ou pela oferta.
        /// </summary>
        /// <param name="itemTrilha"></param>
        /// <param name="matricula"></param>
        /// <returns></returns>
        private string ConsultarLinkAcessoCurso(ItemTrilha itemTrilha, UsuarioTrilha matricula)
        {
            try
            {
                if (itemTrilha.Tipo.ID == (int)enumTipoItemTrilha.ConheciGame)
                {
                    return ObterLinkConheciGame(itemTrilha, matricula);
                }


                // Esse método só interessa para itens trilha do tipo Soluções.
                if (itemTrilha.Tipo == null || itemTrilha.Tipo.ID != (int)enumTipoItemTrilha.Solucoes ||
                    itemTrilha.SolucaoEducacionalAtividade == null)
                    return null;

                var matriculaOferta = itemTrilha.ListaItemTrilhaParticipacao.FirstOrDefault(
                            x => x.UsuarioTrilha.ID == matricula.ID && x.MatriculaOferta != null)?.MatriculaOferta;

                if (matriculaOferta == null || !matriculaOferta.IsAprovado())
                {
                    return null;
                }

                // Busca o objeto que não vem carregado no lazy load
                matriculaOferta = new ManterMatriculaOferta().ObterMatriculaOfertaPorID(matriculaOferta.ID);

                var url =
                    new ConsultarMeusCursos().ConsultarLinkAcessoFornecedor(
                        matriculaOferta.Oferta.SolucaoEducacional.Fornecedor,
                        matriculaOferta.Usuario,
                        matriculaOferta.Oferta.CodigoMoodle.ToString());

                //Caso não possua o link em nenhuma outra forma, verifica se a oferta possui o link.
                if (string.IsNullOrEmpty(url))
                {
                    url = matriculaOferta.Oferta.Link;
                }

                return string.IsNullOrEmpty(url) ? null : url;
            }
            catch
            {
                return null;
            }
        }

        private string ObterLinkConheciGame(ItemTrilha itemTrilha, UsuarioTrilha matricula)
        {
            var conheciGame = new ManterConfiguracaoSistema().ObterConfiguracaoSistemaPorID((int)enumConfiguracaoSistema.UrlConheciGame);
            var url = conheciGame.Registro;

            var itemTrilhaBanco = new ManterItemTrilha().ObterItemTrilhaPorID(itemTrilha.ID);
            var ID_TemaConheciGame = itemTrilhaBanco.ID_TemaConheciGame;

            url += "?cpf=" + matricula.Usuario.CPF + "&guid=" + new ManterUsuarioServices().ObterSenhaGuid(matricula.Usuario) + "&theme_id="
                + ID_TemaConheciGame + "&usuario_trilha=" + matricula.ID + "&item_trilha=" + itemTrilha.ID;

            return url;
        }

        private DtoTrilhaSolucaoSebrae ObterDadosSolucao(ItemTrilha itemTrilha, UsuarioTrilha matricula,
            bool solucaoSebrae, UsuarioTrilha usuario)
        {
            var retorno = new DtoTrilhaSolucaoSebrae();

            retorno.FormaAquisicaoId = itemTrilha.FormaAquisicao.ID;
            retorno.FormaAquisicao = itemTrilha.FormaAquisicao.Nome;
            retorno.Nome = itemTrilha.Nome;
            retorno.CargaHoraria = itemTrilha.CargaHoraria;
            retorno.Id = itemTrilha.ID;
            retorno.Orientacao = !string.IsNullOrWhiteSpace(itemTrilha.Local) ? itemTrilha.Local : "Sem Orientação";
            retorno.DonoTrilha = (itemTrilha.Usuario != null && itemTrilha.Usuario.ID == matricula.Usuario.ID);
            retorno.PontoSebraeId = itemTrilha.Missao.PontoSebrae.ID;
            retorno.MissaoID = itemTrilha.Missao.ID;

            if (solucaoSebrae)
            {
                if (itemTrilha.Tipo == null)
                    throw new ResponseException(enumResponseStatusCode.ErroRegraNegocioSgus,
                        "Solução não possui Tipo vinculado.");

                retorno.Tipo = (enumTipoItemTrilha)itemTrilha.Tipo.ID;
                retorno.Moedas = itemTrilha.Moedas ?? 0;
                retorno.LinkAcesso =
                    ConsultarLinkAcessoCurso(itemTrilha, matricula);

                var status = itemTrilha.ObterStatusParticipacoesItemTrilha(matricula);

                if (status == null)
                    throw new ResponseException(enumResponseStatusCode.ErroRegraNegocioSgus, "Participações inválidas.");

                retorno.Status = status;                
                retorno.MediaAvaliacoes = itemTrilha.ObterMediaAvaliacoes();
                retorno.UsuarioAvaliou = itemTrilha.ChecarAvaliacao(usuario);
                retorno.TotalAvaliacoes = itemTrilha.Avaliacoes.Count;
            }

            return retorno;
        }

        public dynamic ConverterMoedasOuro(UserIdentity usuarioLogado)
        {
            var prataPorOuro = usuarioLogado.Nivel.ValorPrataPorOuro ?? 0;
            var moedasPrata = new ManterUsuarioTrilhaMoedas().Obter(usuarioLogado.Matricula, enumTipoMoeda.Prata);

            if (moedasPrata < prataPorOuro)
                throw new ResponseException(enumResponseStatusCode.ErroRegraNegocioTrilhas,
                    "Não há moedas de prata o suficiente.");

            int removerPrata = 0, adicionarOuro = 0, calc = (moedasPrata + removerPrata);

            while (calc > 0 && calc >= prataPorOuro)
            {
                removerPrata -= prataPorOuro;
                calc = (moedasPrata + removerPrata);
                adicionarOuro++;
            }

            return
                new
                {
                    NovosValores = new ManterUsuarioTrilhaMoedas()
                        .Incluir(usuarioLogado.Matricula, null, null, removerPrata, adicionarOuro),

                    // Obtém se o usuário possui saldo para a prova final.
                    PossuiSaldoProvaFinal = usuarioLogado.Matricula.PossuiSaldoProvaFinal()
                };
        }

        public RetornoTokenTrilha ObterTokenMapa(int usuarioId, int trilhaNivelId, string experimenta = null)
        {
            var manterUsuario = new ManterUsuario();

            var usuario = manterUsuario.ObterUsuarioPorID(usuarioId);

            var tokenGuid = manterUsuario.GerarTokenTrilha(usuario);

            var encode = new { id = 0, nid = 0, experimenta = string.Empty };
            if (experimenta != null && experimenta == "experimente")
            {
                encode = new { id = usuario.ID, nid = trilhaNivelId, experimenta = experimenta };
            }
            else
            {
                encode = new { id = usuario.ID, nid = trilhaNivelId, experimenta = string.Empty };
            }

            var token = JsonWebToken.Encode(encode, tokenGuid, JwtHashAlgorithm.HS256);

            var nivel = new ManterTrilhaNivel().ObterTrilhaNivelPorID(trilhaNivelId);

            return new RetornoTokenTrilha
            {
                Token = token,
                NomeTrilha = string.Format("{0} - {1}", nivel.Trilha.Nome, nivel.Nome)
            };
        }


        public dynamic ObterParticipacao(UsuarioTrilha matricula, TrilhaNivel nivel, int itemtrilhaId)
        {
            var itemTrilha = new ManterItemTrilha().ObterItemTrilhaPorID(itemtrilhaId);
            var usuarioTrilha = new ManterUsuarioTrilha().ObterPorUsuarioTrilhaNivel(matricula, nivel);

            return new
            {
                ItemTrilhaId = itemTrilha.ID,
                ItemTrilhaNome = itemTrilha.Nome,
                FormaAquisicaoId = itemTrilha.FormaAquisicao.ID,
                FormaAquisicaoNome = itemTrilha.FormaAquisicao.Nome,
                Orientacao = itemTrilha.Local,
                Anexo = ObterLinkAnexo(itemTrilha.FileServer),
                NomeAnexo = itemTrilha.FileServer != null ? itemTrilha.FileServer.NomeDoArquivoOriginal : null,
                Participacoes =
                    itemTrilha.ListaItemTrilhaParticipacao.Where(x => x.UsuarioTrilha.ID == usuarioTrilha.ID)
                        .Select(y => new
                        {
                            Id = y.ID,
                            Participacao = y.TextoParticipacao,
                            TipoParticipacao = y.TipoParticipacao,
                            Usuario = y.UsuarioTrilha.Usuario.Nome,
                            Anexo =
                                ObterLinkAnexo(y.FileServer),
                            dtEnvio = y.DataEnvio,
                            Autorizado = y.Autorizado
                        }).ToList()
            };
        }

        public dynamic ObterListaFaq()
        {
            // Pega todos os assuntos que tenham pelo menos uma pergunta
            var assuntos = new ManterTrilhaFaq().ObterTodosAssunto();

            var retornoAssunto = new List<dynamic>();
            var retornoPerguntas = new List<dynamic>();

            foreach (var assunto in assuntos)
            {
                if (assunto.ItensFaq.Any())
                {
                    retornoAssunto.Add(new
                    {
                        id = assunto.ID,
                        nome = assunto.Nome
                    });

                    foreach (var pergunta in assunto.ItensFaq)
                    {
                        retornoPerguntas.Add(new
                        {
                            id = pergunta.ID,
                            nome = pergunta.Nome,
                            assunto_id = pergunta.Assunto.ID
                        });
                    }
                }
            }

            return new
            {
                assuntos = retornoAssunto,
                perguntas = retornoPerguntas
            };
        }

        public dynamic CadastrarItemTrilhaParticipacao(DTOParticipacao dtoParticipacao, UsuarioTrilha matricula,
            TrilhaNivel nivel)
        {
            var usuarioTrilha = new ManterUsuarioTrilha().ObterPorUsuarioTrilhaNivel(matricula, nivel);
            if (usuarioTrilha == null)
                throw new ResponseException(enumResponseStatusCode.RegistroNaoEncontrado);

            return new ManterItemTrilhaParticipacao().CadastrarItemTrilhaParticipacao(usuarioTrilha.ID,
                dtoParticipacao.IdItemTrilha,
                dtoParticipacao.TxParticipacao, dtoParticipacao.NomeDoArquivoOriginal, dtoParticipacao.Base64,
                (int)enumTipoParticipacaoTrilha.ParticipacaoTrilheiro, matricula.Usuario.CPF);
        }

        /// <summary>
        /// Cadastrar Solucao Trilheiro
        /// </summary>
        /// <param name="dtoSolucaoTrilheiro"></param>
        /// <param name="user"></param>
        /// <returns>dynamic</returns>
        public dynamic CadastrarSolucaoTrilheiro(DTOSolucaoTrilheiro dtoSolucaoTrilheiro, UsuarioTrilha matricula,
            TrilhaNivel nivel)
        {
            var usuarioTrilha = new ManterUsuarioTrilha().ObterPorUsuarioTrilhaNivel(matricula, nivel);
            if (usuarioTrilha == null)
                throw new ResponseException(enumResponseStatusCode.RegistroNaoEncontrado,
                    "Usuario não vinculado a Trilha");

            if (dtoSolucaoTrilheiro.IdLoja == 0)
                throw new ResponseException(enumResponseStatusCode.ErroCampoObrigatorio, "Loja não informada");

            if (dtoSolucaoTrilheiro.MissaoId == 0)
                throw new ResponseException(enumResponseStatusCode.ErroCampoObrigatorio, "Missão não informada");

            if (string.IsNullOrWhiteSpace(dtoSolucaoTrilheiro.Nome))
                throw new ResponseException(enumResponseStatusCode.ErroCampoObrigatorio, "Nome não informado");

            if (dtoSolucaoTrilheiro.IdTipo == 0)
                throw new ResponseException(enumResponseStatusCode.ErroCampoObrigatorio, "Tipo não informado");

            return (new ManterItemTrilha().CadastrarSolucaoTrilheiro(usuarioTrilha, dtoSolucaoTrilheiro, matricula));
        }

        /// <summary>
        /// Obter dados para o cadastro de uma Solução Trilheiro
        /// </summary>
        /// <param name="nivel"></param>
        /// <param name="loja"></param>
        /// <param name="matricula"></param>
        /// <returns>dynamic</returns>
        public dynamic ObterDadosCadastroSolucaoTrilheiro(TrilhaNivel nivel, PontoSebrae pontoSebrae,
            UsuarioTrilha matricula)
        {
            var listaItemtrilha =
                new ManterItemTrilha().ObterTodosItemTrilha()
                    .Where(x => x.Usuario != null && x.Usuario.ID == matricula.Usuario.ID);

            var missoes = new ManterItemTrilha().ObterTodosItemTrilha()
                .Where(x => x.Usuario == null
                    && x.Missao.PontoSebrae.ID == pontoSebrae.ID
                    && x.Missao.PontoSebrae.TrilhaNivel.ID == nivel.ID
                ).Select(x => new { Id = x.Missao.ID, Nome = x.Missao.Nome }).Distinct();

            return new
            {
                missoes = missoes.ToList(),
                formasAquisicao =
                    pontoSebrae.ObterFormasAquisicao().Select(x => new { Id = x.ID, Nome = x.Nome }).ToList(),
                primeiraParticipacao = !listaItemtrilha.Any()
            };
        }

        public dynamic ObterStatusSolucaoSebrae(int solucaoSebraeId, UserIdentity usuarioLogado)
        {
            ItemTrilha itemTrilha = new ManterItemTrilha().ObterItemTrilhaPorID(solucaoSebraeId);

            UsuarioTrilha matricula = new ManterUsuarioTrilha().ObterPorId(usuarioLogado.Matricula.ID);

            VerificarMatriculaSolucaoEducacional(itemTrilha, matricula);

            ItemTrilhaParticipacao participacao = null;

            if(matricula != null && matricula.ID != 0)
                itemTrilha.ListaItemTrilhaParticipacao.LastOrDefault(x => x.UsuarioTrilha.ID == matricula.ID);

            if (participacao != null && participacao.MatriculaOferta != null && participacao.MatriculaOferta.IsCancelado())
            {
                var currentMatriculaOferta = new ManterMatriculaOferta()
                    .ObterTodosIQueryable()
                    .Where(x => x.Usuario.ID == matricula.Usuario.ID && x.Oferta.SolucaoEducacional.ID == itemTrilha.SolucaoEducacionalAtividade.ID)
                    .OrderByDescending(x => x.ID)
                    .FirstOrDefault();

                if (currentMatriculaOferta != null)
                {
                    var manterItemTrilhaParticipacao = new BP.ManterItemTrilhaParticipacao();

                    // Atualiza o vínculo de oferta na participação de trilha
                    participacao.MatriculaOferta = currentMatriculaOferta;
                    manterItemTrilhaParticipacao.Salvar(participacao);

                    // Verifica o status da participação, inserindo mo
                    manterItemTrilhaParticipacao.AtualizarStatusParticipacoesTrilhas(currentMatriculaOferta);
                }
            }

            string Anexo = null;
            string NomeAnexo = null;
            string DataInicio = null;
            string DataLimite = null;
            double CargaHoraria = itemTrilha.CargaHoraria;
            string NomeSE = null;
            switch (itemTrilha.Tipo.ID)
            {
                case (int)enumTipoItemTrilha.Solucoes:
                    NomeSE = itemTrilha.SolucaoEducacionalAtividade.Nome;                    
                    Oferta oferta = new ManterOferta().ObterOfertaPorSolucaoEducacional(itemTrilha.SolucaoEducacionalAtividade.ID).FirstOrDefault();
                    CargaHoraria = oferta.SolucaoEducacional.CargaHoraria;

                    if (participacao != null && participacao.MatriculaOferta != null && participacao.MatriculaOferta.MatriculaTurma.Any() && participacao.MatriculaOferta.Oferta.IsAbertaParaInscricoes())
                    {
                        MatriculaTurma matriculaTurma = participacao.MatriculaOferta.MatriculaTurma.FirstOrDefault();                                              
                        DataInicio = matriculaTurma.DataMatricula.ToString("dd/MM/yyyy");
                        DataLimite = matriculaTurma.DataLimite.ToString("dd/MM/yyyy");
                    }
                    break;
                case (int)enumTipoItemTrilha.Atividade:
                case (int)enumTipoItemTrilha.Discursiva:                    
                    Anexo = ObterLinkAnexo(itemTrilha.FileServer);
                    NomeAnexo = itemTrilha.FileServer != null ? itemTrilha.FileServer.NomeDoArquivoOriginal : null;
                    break;

            }
            
            if (participacao != null && participacao.Autorizado != true && participacao.MatriculaOferta != null && participacao.MatriculaOferta.IsAprovado())
            {
                (new BP.ManterItemTrilhaParticipacao()).AtualizarStatusParticipacoesTrilhas(participacao.MatriculaOferta);
            }

            enumStatusParticipacaoItemTrilha? status = itemTrilha.ObterStatusParticipacoesItemTrilha(usuarioLogado.Matricula);
            
            return new
            {
                LinkAcesso = ConsultarLinkAcessoCurso(itemTrilha, matricula),
                Anexo = Anexo,
                NomeAnexo = NomeAnexo,
                NomeSE = NomeSE,
                CargaHoraria = convertMinutesToHours((int) CargaHoraria),
                DataInicio = DataInicio,
                DataLimite = DataLimite,
                Moedas = itemTrilha.Moedas ?? 0,
                Status = (int?)status
            };
        }

        private static void VerificarMatriculaSolucaoEducacional(ItemTrilha itemTrilha, UsuarioTrilha matricula)
        {
            if (itemTrilha.Tipo.ID == (int)enumTipoItemTrilha.Solucoes &&
                itemTrilha.SolucaoEducacionalAtividade != null)
            {
                var matriculaOferta =
                    new ManterMatriculaOferta()
                        .ObterTodosIQueryable()
                        .Where(x => x.Usuario.ID == matricula.Usuario.ID &&
                                    x.Oferta.SolucaoEducacional.ID == itemTrilha.SolucaoEducacionalAtividade.ID)
                        .OrderByDescending(x => x.ID)
                        .FirstOrDefault();

                if (matriculaOferta != null)
                {
                    // Se for Solucoes, checar se o usuário se inscreveu pelo Portal e criar o vínculo com o Item Trilha.
                    if (!itemTrilha.ListaItemTrilhaParticipacao.Any(x =>
                            x.UsuarioTrilha.ID == matricula.ID
                            && x.Autorizado == true) &&
                        !itemTrilha.ListaItemTrilhaParticipacao.Any(x =>
                            x.UsuarioTrilha.ID == matricula.ID
                            && x.MatriculaOferta != null
                            && x.MatriculaOferta.ID == matriculaOferta.ID))
                    {
                        // Criar participação Dummy de matrícula oferta, inversalmente.
                        var participacao = new ItemTrilhaParticipacao
                        {
                            Autorizado =
                                matriculaOferta.StatusMatricula == enumStatusMatricula.Inscrito
                                    ? (bool?)null
                                    : matriculaOferta.IsAprovado(),
                            TipoParticipacao = enumTipoParticipacaoTrilha.SolucaoEducacional,
                            UsuarioTrilha = matricula,
                            ItemTrilha = itemTrilha,
                            DataEnvio = DateTime.Now,
                            MatriculaOferta = matriculaOferta,
                            Auditoria = new Auditoria(matriculaOferta.Usuario.CPF),
                            FileServer = null
                        };

                        new BP.ManterItemTrilhaParticipacao().Salvar(participacao);

                        if (participacao.Autorizado == true)
                        {
                            new ManterUsuarioTrilhaMoedas().Incluir(matricula, itemTrilha, null, 0, itemTrilha.Moedas ?? 0);
                        }

                        // Dar aquele Refresh maroto no ItemTrilha pra funciona mais pra frente.
                        itemTrilha.ListaItemTrilhaParticipacao.Add(participacao);
                    }
                }
            }
        }

        public dynamic ObterExtrato(UserIdentity usuarioLogado, int usuario, int nivel)
        {
            var usuarioTrilha = new ManterUsuarioTrilha().ObterPorUsuarioTrilhaNivel(usuario, nivel);

            if (usuarioTrilha == null) throw new ResponseException(enumResponseStatusCode.UsuarioNaoMatriculado);

            var manterUsuarioTrilhaMoedas = new ManterUsuarioTrilhaMoedas();

            return new
            {
                Saldo = manterUsuarioTrilhaMoedas.CalcularMoedas(usuarioTrilha),
                Extrato = manterUsuarioTrilhaMoedas.ObterExtrato(usuarioTrilha)
            };
        }

        public void ValidarAcessoProvaFinal(UserIdentity usuarioLogado)
        {
            var usuarioTrilha = new ManterUsuarioTrilha().ObterPorUsuarioTrilhaNivel(usuarioLogado.Matricula,
                usuarioLogado.Nivel);
            if (usuarioTrilha == null)
                throw new ResponseException(enumResponseStatusCode.RegistroNaoEncontrado,
                    "Usuário não matriculado na Trilha");

            //var manterUsuarioTrilhaMoedas = new ManterUsuarioTrilhaMoedas();
            var qtdMoedasOuro = new ManterUsuarioTrilhaMoedas().Obter(usuarioTrilha, enumTipoMoeda.Ouro);

            if (qtdMoedasOuro < usuarioLogado.Nivel.QuantidadeMoedasProvaFinal)
                throw new ResponseException(enumResponseStatusCode.ErroRegraNegocioTrilhas,
                    "Você ainda não possui a quantidade mínima e moedas de ouro para realizar a Prova Final (" +
                    usuarioLogado.Nivel.QuantidadeMoedasProvaFinal + " Moedas de Ouro)");

            //Verifica se aprovado na prova
            if (usuarioTrilha.StatusMatricula == enumStatusMatricula.Aprovado)
                throw new ResponseException(enumResponseStatusCode.UsuarioAprovadoProvaFinal,
                    string.Format(enumResponseStatusCode.UsuarioAprovadoProvaFinal.GetDescription(),
                        usuarioLogado.Usuario.Nome));

            //Verifica se reprovado na prova
            if (usuarioTrilha.StatusMatricula == enumStatusMatricula.Reprovado &&
                usuarioTrilha.DataLiberacaoNovaProva.HasValue)
                throw new ResponseException(enumResponseStatusCode.ErroRegraNegocioTrilhas,
                    "A data para liberação de realização de nova prova é " +
                    usuarioTrilha.DataLiberacaoNovaProva.Value.ToString("dd/MM/yyyy hh:mm:ss") +
                    ". Aproveite para estudar mais até lá");
        }

        public dynamic ObterNotificacoes(UsuarioTrilha usuario)
        {
            var manter = new ManterNotificacao();
            var notificacoes = manter.ObterNotificacoesNaoVisualizadas(usuario);

            if (notificacoes.Any())
            {
                var thread = new Thread(() =>
                {
                    var notificacoesVisualizadas = notificacoes.ToList();
                    foreach (var notificacao in notificacoesVisualizadas)
                    {
                        notificacao.Visualizado = true;
                    }

                    manter.SalvarEmLote(notificacoesVisualizadas, notificacoesVisualizadas.Count);
                })
                {
                    IsBackground = true,
                    Name = Guid.NewGuid().ToString()
                };

                thread.Start();
            }

            return notificacoes.ToList().Select(x => new
            {
                Mensagem = x.TextoNotificacao
            });
        }

        private static string ObterLinkAnexo(FileServer file)
        {
            if (file == null)
                return null;

            var enderecoSgus =
                new ManterConfiguracaoSistema().ObterConfiguracaoSistemaPorID(
                    (int)enumConfiguracaoSistema.EnderecoSGUS)
                    .Registro;

            return enderecoSgus + "/MediaServer.ashx?Identificador=" + file.ID;
        }

        //Excluir Solucao Trilheiro
        public dynamic ExcluirSolucaoTrilheiro(UserIdentity usuarioLogado, int itemTrilhaId)
        {
            try
            {
                var manterItemTrilha = new ManterItemTrilha();
                var itemTrilha = manterItemTrilha.ObterItemTrilhaPorID(itemTrilhaId);
                var manterMoedas = new ManterUsuarioTrilhaMoedas();

                if (itemTrilha.Usuario.ID == usuarioLogado.Usuario.ID)
                {
                    int saldoSolucaoTrilheiro = manterMoedas.Obter(usuarioLogado.Matricula, itemTrilha);

                    if (saldoSolucaoTrilheiro > 0)
                    {
                        var saldoTotal = manterMoedas.Obter(usuarioLogado.Matricula, enumTipoMoeda.Prata);

                        if (saldoTotal != 0)
                        {
                            if (saldoTotal > saldoSolucaoTrilheiro)
                            {
                                manterMoedas.Incluir(usuarioLogado.Matricula, null, null, -saldoSolucaoTrilheiro, 0,
                                    enumTipoCurtida.ExcluirSolucaoTrilheiro);
                            }
                            else
                            {
                                manterMoedas.Incluir(usuarioLogado.Matricula, null, null, -saldoTotal, 0,
                                    enumTipoCurtida.ExcluirSolucaoTrilheiro);
                            }
                        }
                    }
                    else
                    {
                        var saldo = (saldoSolucaoTrilheiro + (-1 * saldoSolucaoTrilheiro));
                        manterMoedas.Incluir(usuarioLogado.Matricula, null, null, -saldoSolucaoTrilheiro, 0,
                            enumTipoCurtida.ExcluirSolucaoTrilheiro);
                    }

                    //Inativar Solucao Trilheiro
                    itemTrilha.Ativo = false;
                    manterItemTrilha.AlterarItemTrilha(itemTrilha);

                    return new { mensagem = "Excluído com Sucesso" };
                }
                else
                {
                    throw new ResponseException(enumResponseStatusCode.ErroRegraNegocioSgus,
                        "Falha ao excluir Solução do Trilheiro");
                }
            }
            catch
            {
                throw new ResponseException(enumResponseStatusCode.ErroRegraNegocioSgus,
                    "Falha ao excluir Solução do Trilheiro");
            }

        }

        public bool UsuarioAprovado(UsuarioTrilha matricula)
        {
            return new ManterUsuarioTrilha().ObterPorId(matricula.ID).StatusMatricula == enumStatusMatricula.Aprovado;
        }

        public dynamic EmitirCertificado(UsuarioTrilha matricula, string jwtToken)
        {
            if (matricula.StatusMatricula != enumStatusMatricula.Aprovado)
                throw new ResponseException(enumResponseStatusCode.UsuarioNaoAprovado, "O Certificado é disponibilizado após a aprovação na bandeirada.");

            var dt = CertificadoTemplateUtil.GerarDataTableComCertificado(0, 0, matricula.ID,
                matricula.TrilhaNivel.CertificadoTemplate, jwtToken);

            return new
            {
                base64 =
                    Convert.ToBase64String(
                        CertificadoTemplateUtil.RetornarCertificado(matricula.TrilhaNivel.CertificadoTemplate, dt))
            };
        }

        public dynamic EmitirCertificadoModelo(UsuarioTrilha matricula)
        {
            var nivel = matricula.TrilhaNivel;

            if (nivel.CertificadoTemplate == null)
                throw new ResponseException(enumResponseStatusCode.CertificadoInexistente);

            //var cf = new BMCertificadoTemplate().ObterPorID(tn.CertificadoTemplate.ID);
            var dt = new DataTable();
            dt.Columns.Add(new DataColumn("TX_Certificado"));
            dt.Columns.Add(new DataColumn("OB_Imagem", typeof(byte[])));
            var dr = dt.NewRow();

            dr["TX_Certificado"] = nivel.CertificadoTemplate.TextoDoCertificado;

            dr["OB_Imagem"] = CertificadoTemplateUtil.ObterImagemBase64(nivel.CertificadoTemplate.Imagem);

            if (!string.IsNullOrEmpty(nivel.CertificadoTemplate.TextoCertificado2) &&
                !string.IsNullOrEmpty(nivel.CertificadoTemplate.Imagem2))
            {
                dt.Columns.Add(new DataColumn("TX_Certificado2"));
                dt.Columns.Add(new DataColumn("OB_Image2", typeof(byte[])));
                dr["TX_Certificado2"] = nivel.CertificadoTemplate.TextoCertificado2;
                dr["OB_Image2"] = CertificadoTemplateUtil.ObterImagemBase64(nivel.CertificadoTemplate.Imagem2);
            }
            dt.Rows.Add(dr);

            return new
            {
                base64 =
                    Convert.ToBase64String(
                        CertificadoTemplateUtil.RetornarCertificado(matricula.TrilhaNivel.CertificadoTemplate, dt))
            };
        }

        public void AtualizarStatusJogo(int solucaoId, bool conclusao, int matriculaId)
        {
            var solucaoSebrae = new ManterItemTrilha().ObterItemTrilhaPorID(solucaoId);

            if (solucaoSebrae == null || solucaoSebrae.Usuario != null)
                throw new ResponseException(enumResponseStatusCode.SolucaoSebraeNaoEncontrada);

            var matricula = new ManterUsuarioTrilha().ObterPorId(matriculaId);

            if (matricula == null)
                throw new ResponseException(enumResponseStatusCode.UsuarioNaoMatriculado);

            var ultimaParticipacao =
                solucaoSebrae.ListaItemTrilhaParticipacao.LastOrDefault(x => x.UsuarioTrilha.ID == matricula.ID && x.Autorizado == null);

            if (ultimaParticipacao == null)
            {
                // Se não houver uma matrícula válida, cria agora.
                // Criar uma participação que servirá de "Dummy" para relacionar o aluno com a aprovação do jogo.
                ultimaParticipacao = new ItemTrilhaParticipacao
                {
                    UsuarioTrilha = matricula,
                    ItemTrilha = solucaoSebrae,
                    DataEnvio = DateTime.Now,
                    TipoParticipacao = enumTipoParticipacaoTrilha.Jogo,
                    Auditoria = new Auditoria(matricula.Usuario.CPF),
                    FileServer = null
                };
            }

            ultimaParticipacao.Autorizado = conclusao;

            if (ultimaParticipacao.Autorizado == true)
                new ManterUsuarioTrilhaMoedas().Incluir(matricula,
                    ultimaParticipacao.ItemTrilha, null, 0,
                    ultimaParticipacao.ItemTrilha.Moedas ?? 0);

            // Criar participação.
            new BP.ManterItemTrilhaParticipacao().Salvar(ultimaParticipacao);
        }

        private static void PreencherParticipacoes(List<ItemTrilha> itensTrilha, UsuarioTrilha usuarioTrilha)
        {
            var idsItensTrilha = itensTrilha.Select(x => x.ID).ToList();

            var participacoes =
                new BP.ManterItemTrilhaParticipacao().ObterTodosIQueryable()
                    .Where(x => x.UsuarioTrilha.ID == usuarioTrilha.ID && idsItensTrilha.Contains(x.ItemTrilha.ID))
                    .Select(x => new ItemTrilhaParticipacao
                    {
                        ID = x.ID,
                        Autorizado = x.Autorizado,
                        TipoParticipacao = x.TipoParticipacao,
                        UsuarioTrilha = new UsuarioTrilha
                        {
                            ID = x.UsuarioTrilha.ID
                        },
                        MatriculaOferta = x.MatriculaOferta != null
                            ? new MatriculaOferta
                            {
                                ID = x.MatriculaOferta.ID
                            }
                            : null,
                        ItemTrilha = new ItemTrilha
                        {
                            ID = x.ItemTrilha.ID
                        }
                    }).ToList();

            var idsMatriculaOferta =
                participacoes.Where(x => x.MatriculaOferta != null).Select(x => x.MatriculaOferta.ID).ToList();

            if (idsMatriculaOferta.Any())
            {
                // Realizar consultas em batches de 2099 parâmetros.

                var matriculasOferta = new List<MatriculaOferta>();

                const int batchSize = 2095;

                var manterMatriculaOferta = new ManterMatriculaOferta();

                for (var batch = 1; batch <= Math.Ceiling((decimal)idsMatriculaOferta.Count() / batchSize); batch++)
                {
                    var idsBatch =
                        (batch != 1 ? idsMatriculaOferta.Skip((batch - 1) * batchSize).ToList() : idsMatriculaOferta).Take
                            (batchSize).ToList();

                    matriculasOferta.AddRange(manterMatriculaOferta.ObterTodosIQueryable()
                        .Where(x => idsBatch.Contains(x.ID))
                        .Select(x => new MatriculaOferta
                        {
                            ID = x.ID,
                            StatusMatricula = x.StatusMatricula,
                            Oferta = new Oferta
                            {
                                ID = x.Oferta.ID
                            },
                            Usuario = new Usuario
                            {
                                ID = x.Usuario.ID
                            }
                        }).ToList());
                }

                var matriculasTurma = new List<MatriculaTurma>();
                var manterMatriculaTurma = new ManterMatriculaTurma();

                for (var batch = 1; batch <= Math.Ceiling((decimal)idsMatriculaOferta.Count() / batchSize); batch++)
                {
                    var idsBatch =
                        (batch != 1 ? idsMatriculaOferta.Skip((batch - 1) * batchSize).ToList() : idsMatriculaOferta).Take
                            (batchSize).ToList();

                    matriculasTurma.AddRange(manterMatriculaTurma.ObterTodosIQueryable()
                        .Where(x => idsBatch.Contains(x.MatriculaOferta.ID))
                        .Select(x => new MatriculaTurma
                        {
                            ID = x.ID,
                            MatriculaOferta = new MatriculaOferta
                            {
                                ID = x.MatriculaOferta.ID
                            },
                            Turma = new Turma
                            {
                                ID = x.Turma.ID,
                                DataFinal = x.Turma.DataFinal
                            }
                        }).ToList());
                }

                var ofertasIds = matriculasOferta.Select(x => x.Oferta.ID).Distinct().ToList();

                var ofertas = new ManterOferta().ObterTodasIQueryable().Where(x => ofertasIds.Contains(x.ID))
                    .Select(x => new Oferta
                    {
                        ID = x.ID,
                        DataFimInscricoes = x.DataFimInscricoes,
                        DiasPrazo = x.DiasPrazo,
                        SolucaoEducacional = new SolucaoEducacional
                        {
                            ID = x.SolucaoEducacional.ID
                        }
                    }).ToList();

                var idsSes = ofertas.Select(x => x.SolucaoEducacional.ID).Distinct().ToList();

                var ses = new ManterSolucaoEducacional().ObterTodosIQueryable().Where(x => idsSes.Contains(x.ID))
                    .Select(x => new SolucaoEducacional
                    {
                        ID = x.ID,
                        Fornecedor = new Fornecedor
                        {
                            ID = x.Fornecedor.ID
                        }
                    }).ToList();

                var fornecedoresIds = ses.Select(x => x.Fornecedor.ID).Distinct().ToList();

                var fornecedores =
                    new ManterFornecedor().ObterTodosIQueryable().Where(x => fornecedoresIds.Contains(x.ID))
                        .Select(x => new Fornecedor
                        {
                            ID = x.ID,
                            LinkAcesso = x.LinkAcesso,
                            TextoCriptografia = x.TextoCriptografia
                        }).ToList();

                foreach (var se in ses)
                {
                    se.Fornecedor = fornecedores.FirstOrDefault(x => x.ID == se.Fornecedor.ID);
                }

                foreach (var oferta in ofertas)
                {
                    oferta.SolucaoEducacional = ses.FirstOrDefault(x => x.ID == oferta.SolucaoEducacional.ID);
                }

                var idsUsuarios = matriculasOferta.Select(x => x.Usuario.ID).Distinct().ToList();

                var usuarios = new ManterUsuario().ObterTodosIQueryable().Where(x => idsUsuarios.Contains(x.ID))
                    .Select(x => new Usuario
                    {
                        ID = x.ID,
                        CPF = x.CPF,
                        Senha = x.Senha
                    }).ToList();

                foreach (var matriculaOferta in matriculasOferta)
                {
                    // Preencher ofertas.
                    matriculaOferta.Oferta = ofertas.FirstOrDefault(x => x.ID == matriculaOferta.Oferta.ID);

                    // Preenchr matrícula turma.
                    matriculaOferta.MatriculaTurma =
                        matriculasTurma.Where(x => x.MatriculaOferta.ID == matriculaOferta.ID).ToList();

                    // Preencher usuário.
                    matriculaOferta.Usuario = usuarios.FirstOrDefault(x => x.ID == matriculaOferta.Usuario.ID);
                }

                foreach (var participacao in participacoes.Where(x => x.MatriculaOferta != null))
                {
                    participacao.MatriculaOferta =
                        matriculasOferta.FirstOrDefault(x => x.ID == participacao.MatriculaOferta.ID);
                }
            }

            // Propagar participações.
            foreach (var itemTrilha in itensTrilha)
            {
                itemTrilha.ListaItemTrilhaParticipacao =
                    participacoes.Where(x => x.ItemTrilha.ID == itemTrilha.ID).ToList();
            }
        }
        public byte[] GetBoletinInPdf(int userID)
        {

            var dic = new Dictionary<string, object>();
            dic.Add("@ID_Usuariario_Trilha", userID);
            var solucaoesDesempenhoGeral = new BMItemTrilha().ExecutarProcedure<DTOSolucaoesDesempenhoGeral>("SP_solucoes_do_desempenho_geral", dic);

            Document document = new Document();
            var streamOutput = new MemoryStream();
            var writer = PdfWriter.GetInstance(document, streamOutput);

            document.Open();

            int colunms = 9;

            PdfPTable table = new PdfPTable(colunms);

            table.SetWidthPercentage(new float[] { 2, 3, 3, 3, 3, 4, 5, 5, 3 }, new Rectangle(29, 39));

            table.SetWidths(new int[] { 2, 3, 3, 3, 3, 4, 5, 5, 3 });


            PdfPCell cell = new PdfPCell(new Phrase("Boletim de desempenho"));
            cell.Colspan = colunms;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);


            PdfPCell firthTitleCell = new PdfPCell(new Phrase("Quantidade de Moedas"));
            firthTitleCell.Colspan = 2;
            table.AddCell(firthTitleCell);

            table.AddCell("Quantidade de Medalhas");
            table.AddCell("Troféus Alcançados");            
            table.AddCell("Total Objeto/Solução");
            table.AddCell("Total de horas Certificadas na trilha/nível");
            table.AddCell("Total de horas registradas na trilha/nível");            

            solucaoesDesempenhoGeral
                 .ToList()
                 .ForEach(x =>
                 {
                     PdfPCell firthCell = new PdfPCell(new Phrase(x.Moedas));
                     firthCell.Colspan = 2;
                     table.AddCell(firthCell);

                     table.AddCell(x.Medalhas.ToString());
                     table.AddCell(x.Trofeus.ToString());
                     table.AddCell(x.Solucoes.ToString());                                          
                     table.AddCell(x.HorasCertificadas.ToString());
                     table.AddCell(x.HorasRegistradas.ToString());                     
                 });

            document.Add(table);
            document.Close();

            return streamOutput.ToArray();

        }
    
    }
}