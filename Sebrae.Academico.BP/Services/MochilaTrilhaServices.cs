using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.Auth;
using Sebrae.Academico.BP.DTO.Services.Trilhas.ConheciGame;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP.Services
{
    public partial class TrilhaServices
    {
        public object ObterDadosMochila(int usuarioId, UserIdentity acessoAtual)
        {
            // Obter os dados da matrícula.
            var usuarioTrilha =
                new ManterUsuarioTrilha().ObterTodosIQueryable()
                    .Where(x => x.Usuario.ID == usuarioId && x.TrilhaNivel.ID == acessoAtual.Nivel.ID && x.StatusMatricula != enumStatusMatricula.CanceladoAluno)
                    .Select(x => new UsuarioTrilha
                    {
                        ID = x.ID,
                        TrilhaNivel = new TrilhaNivel
                        {
                            ID = x.TrilhaNivel.ID,
                            PorcentagensTrofeus = x.TrilhaNivel.PorcentagensTrofeus
                        }
                    })
                    .FirstOrDefault();

            if (usuarioTrilha == null)
            {
                throw new ResponseException(enumResponseStatusCode.UsuarioNaoMatriculado);
            }

            var liderancasAsync = ObterLiderancasAsync(usuarioTrilha);

            var statusMissoesAsync = ObterStatusMissoesAsync(usuarioTrilha);

            var dadosMochila = new BMMochilaTrilhas().ObterDadosMochila(usuarioId, acessoAtual.Nivel.ID);

            var solucoes = new BMSolucoesTrilhas().ObterSolucoesMochila(usuarioId, usuarioTrilha.ID);

            ObterLinkAcesso(solucoes);

            var imagensAsync = ObterImagensAsync(solucoes.Select(x => x.FormaAquisicaoId).Distinct().ToList());

            statusMissoesAsync.Wait();
            var missoes = statusMissoesAsync.Result;

            // Obter o nível do usuário no jogo. Precisa das moedas de ouro do usuário.
            var nivelNoJogo = usuarioTrilha.ObterNivel(dadosMochila.MoedasOuro, dadosMochila.TotalMoedasNivel);

            imagensAsync.Wait();
            liderancasAsync.Wait();

            var retorno = new
            {
                Nome = Usuario.ObterPrimeirosNomes(dadosMochila.Nome).ToUpper(),
                dadosMochila.MoedasOuro,
                dadosMochila.MoedasPrata,
                dadosMochila.Unidade,
                dadosMochila.Medalhas,
                Nivel = nivelNoJogo > 0 ? nivelNoJogo : 1,
                dadosMochila.Foto,
                SolucoesSebrae = solucoes.Where(x => x.Origem == enumOrigemItemTrilha.SolucaoSebrae).ToList(),
                SolucoesTrilheiro = solucoes.Where(x => x.Origem == enumOrigemItemTrilha.SolucaoTrilheiro).ToList(),
                Missoes = missoes,
                Liderancas = liderancasAsync.Result,
                dadosMochila.Ranking,
                dadosMochila.TutorialLido,
                ExibirExtrato = usuarioId == acessoAtual.Usuario.ID,
                Imagens = imagensAsync.Result
            };

            return retorno;
        }

        private static void ObterLinkAcesso(List<DtoTrilhaSolucaoSebrae> solucoes)
        {
            if (solucoes.Any(x => x.MatriculaOfertaId != null))
            {
                var matriculasIds =
                    solucoes.Where(x => x.MatriculaOfertaId != null).Select(x => x.MatriculaOfertaId.Value).ToList();


                var matriculas =
                    new ManterMatriculaOferta().ObterTodosIQueryable().Where(x => matriculasIds.Contains(x.ID))
                        .Join(new ManterOferta().ObterTodasIQueryable(), mo => mo.Oferta.ID, o => o.ID,
                            (mo, o) => new { mo, o })
                        .Join(new ManterSolucaoEducacional().ObterTodosIQueryable(),
                            previousJoin => previousJoin.o.SolucaoEducacional.ID, se => se.ID,
                            (previousJoin, se) => new MatriculaOferta
                            {
                                ID = previousJoin.mo.ID,
                                Oferta = new Oferta
                                {
                                    ID = se.ID,
                                    CodigoMoodle = previousJoin.o.CodigoMoodle,
                                    SolucaoEducacional = new SolucaoEducacional
                                    {
                                        ID = se.ID,
                                        Fornecedor = new Fornecedor
                                        {
                                            ID = se.Fornecedor.ID,
                                            LinkAcesso = se.Fornecedor.LinkAcesso,
                                            TextoCriptografia = se.Fornecedor.TextoCriptografia
                                        }
                                    }
                                },
                                Usuario = new Usuario
                                {
                                    ID = previousJoin.mo.Usuario.ID,
                                    CPF = previousJoin.mo.Usuario.CPF,
                                    Senha = previousJoin.mo.Usuario.Senha
                                }
                            })
                        .ToList();

                foreach (var solucao in solucoes.Where(x => x.MatriculaOfertaId.HasValue))
                {
                    var matriculaOferta = matriculas.FirstOrDefault(x => x.ID == solucao.MatriculaOfertaId.Value);

                    if (matriculaOferta == null)
                    {
                        continue;
                    }

                    solucao.LinkAcesso = new ConsultarMeusCursos().ConsultarLinkAcessoFornecedor(
                        matriculaOferta.Oferta.SolucaoEducacional.Fornecedor,
                        matriculaOferta.Usuario,
                        matriculaOferta.Oferta.CodigoMoodle.ToString());
                }
            }
        }

        public object informarParticipacaoConheciGame(DTOParticipacaoConheciGame dtoParicipacao)
        {
            var manterItemTrilha = new ManterItemTrilha();
            var usuarioTrilha = new ManterUsuarioTrilha().ObterPorId(dtoParicipacao.ID_UsuarioTrilha);
            var itemTrilha = manterItemTrilha.ObterItemTrilhaPorID(dtoParicipacao.ID_ItemTrilha);

            if (dtoParicipacao.QuantidadeAcertos >= itemTrilha.QuantidadeAcertosTema && itemTrilha.Moedas.HasValue)
            {
                var manterMoedas = new ManterUsuarioTrilhaMoedas();

                if (!manterMoedas.ObterTodosIQueryable()
                    .Select(x => new { ID_ItemTrilha = x.ItemTrilha.ID, ID_UsuaioTrilha = x.UsuarioTrilha.ID })
                    .Any(x => x.ID_ItemTrilha == itemTrilha.ID && x.ID_UsuaioTrilha == usuarioTrilha.ID))
                {
                    manterMoedas.Incluir(usuarioTrilha, itemTrilha, null, 0, dtoParicipacao.QuantidadeAcertos);
                    return new
                    {
                        Aprovado = true,
                        QuantidadeMoedas = itemTrilha.Moedas
                    };
                }
            }

            return new
            {
                Aprovado = false,
                QuantidadeMoedas = itemTrilha.Moedas
            };
        }

        public object InformarAvaliacao(int idItemTrilha, string resenha, int avaliacao, UsuarioTrilha matricula)
        {
            var manter = new ManterItemTrilhaAvaliacao();
            var manterItemTrilha = new ManterItemTrilha();

            var usuario = new ManterUsuarioTrilha().ObterPorId(matricula.ID);
            var itemTrilha = manterItemTrilha.ObterItemTrilhaPorID(idItemTrilha);

            var avaliacaoSolucaoSebrae = new ItemTrilhaAvaliacao(resenha, avaliacao, usuario, itemTrilha);

            manter.Salvar(avaliacaoSolucaoSebrae);

            return new
            {
                ItemTrilha = itemTrilha.ID,
                TotalAvaliacoes = itemTrilha.Avaliacoes.Count,
                MediaAvaliacoes = itemTrilha.ObterMediaAvaliacoes(),
                usuarioAvaliou = itemTrilha.ChecarAvaliacao(matricula)
            };
        }

        private Task<List<ImagemDto>> ObterImagensAsync(List<int> formasAquisicaoIds)
        {
            var manter = new ManterFormaAquisicao();

            return Task.Run(() => manter.ObterTodosIQueryable()
                    .Where(x => formasAquisicaoIds.Contains(x.ID))
                    .Select(x => new ImagemDto
                    {
                        Id = x.ID,
                        Imagem = x.Imagem
                    }).ToList());
        }

        private class ImagemDto
        {
            public int Id { get; set; }
            public string Imagem { get; set; }
        }

        private Task<List<DTOMissaoMochila>> ObterStatusMissoesAsync(UsuarioTrilha matricula)
        {
            return Task.Run(() => new BMRankingTrilhas().ObterStatusMissoes(matricula));
        }

        private Task<dynamic> ObterLiderancasAsync(UsuarioTrilha matricula)
        {
            return Task.Run(() => ObterLiderancas(matricula));
        }

        private dynamic ObterLiderancas(UsuarioTrilha matricula)
        {
            var liderancas = new BMLiderTrilhas().ObterLideres(matricula.TrilhaNivel.ID, matricula.ID);

            return liderancas.Select(x => new
            {
                x.Id,
                x.NomePontoSebrae,
                TempoConclusao = new TimeSpan(0, 0, x.Segundos)
            }).ToList();
        }
    }
}