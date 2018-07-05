using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Services.ConsultarFormacaoDeMultiplicadores;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP
{
    public class ManterFormacaoDeMultiplicadores : BusinessProcessBase
    {
        private readonly BMMatriculaOferta _bmMatriculaOferta;
        private readonly BMOferta _bmOferta;

        public ManterFormacaoDeMultiplicadores()
        {
            _bmMatriculaOferta = new BMMatriculaOferta();
            _bmOferta = new BMOferta();
        }

        public async Task<int> ObterParticipantesGestoresAsync(List<int> categoriasFm)
        {
            try
            {
                var perfis = new List<int>
                {
                    (int) enumPerfil.GestorUC
                };

                return
                    await
                        Task.Run(
                            () =>
                                _bmMatriculaOferta.ObterPorPerfil(perfis)
                                    .Count(x => categoriasFm.Contains(x.Oferta.SolucaoEducacional.CategoriaConteudo.ID)));
            }
            catch (Exception ex)
            {
                throw new AcademicoException(ex.Message);
            }
        }

        public async Task<int> ObterParticipantesFormadoresAsync(List<int> categoriasFm)
        {
            try
            {
                var perfis = new List<int>
                {
                    (int) enumPerfil.Colaborador,
                    (int) enumPerfil.Administrador,
                    (int) enumPerfil.AdministradorPortal,
                    (int) enumPerfil.Terceiro,
                    (int) enumPerfil.Orientador,
                    (int) enumPerfil.AdministradorTrilha,
                    (int) enumPerfil.MonitorTrilha,
                    (int) enumPerfil.Professor,
                    (int) enumPerfil.ConsultorEducacional
                };

                return
                    await
                        Task.Run(
                            () =>
                                _bmMatriculaOferta.ObterPorPerfil(perfis)
                                    .Count(x => categoriasFm.Contains(x.Oferta.SolucaoEducacional.CategoriaConteudo.ID)));
            }
            catch (Exception ex)
            {
                throw new AcademicoException(ex.Message);
            }
        }

        public async Task<int> ObterParticipantesFacilitadoresAsync(List<int> categoriasFm)
        {
            try
            {
                var niveisOcupacionais = new List<int> {10, 19, 20, 21};

                return
                    await
                        Task.Run(
                            () =>
                                _bmMatriculaOferta.ObterPorNivelOcupacional(niveisOcupacionais)
                                    .Count(x => categoriasFm.Contains(x.Oferta.SolucaoEducacional.CategoriaConteudo.ID)));
            }
            catch (Exception ex)
            {
                throw new AcademicoException(ex.Message);
            }
        }

        public async Task<int> ObterCapacitacoesGestoresAsync(List<int> categoriasFm)
        {
            try
            {
                var publicosAlvo = new List<int> { 1 };

                return
                    await
                        Task.Run(
                            () =>
                                _bmOferta.ObterPorTipoPublicoAlvo(publicosAlvo)
                                    .Count(x => categoriasFm.Contains(x.SolucaoEducacional.CategoriaConteudo.ID)));
            }
            catch (Exception ex)
            {
                throw new AcademicoException(ex.Message);
            }
        }

        public async Task<int> ObterCapacitacoesFormadoresAsync(List<int> categoriasFm)
        {
            try
            {
                var publicosAlvo = new List<int> { 2 };

                return
                    await
                        Task.Run(
                            () =>
                                _bmOferta.ObterPorTipoPublicoAlvo(publicosAlvo)
                                    .Count(x => categoriasFm.Contains(x.SolucaoEducacional.CategoriaConteudo.ID)));
            }
            catch (Exception ex)
            {
                throw new AcademicoException(ex.Message);
            }
        }

        public async Task<int> ObterCapacitacoesFacilitadoresAsync(List<int> categoriasFm)
        {
            try
            {
                var publicosAlvo = new List<int> { 3 };

                return
                    await
                        Task.Run(
                            () =>
                                _bmOferta.ObterPorTipoPublicoAlvo(publicosAlvo)
                                    .Count(x => categoriasFm.Contains(x.SolucaoEducacional.CategoriaConteudo.ID)));
            }
            catch (Exception ex)
            {
                throw new AcademicoException(ex.Message);
            }
        }

        public async Task<List<DTOMetasFormacaoDeMultiplicadores>> ObterMetasAsync(IEnumerable<MetaFm> metas)
        {
            try
            {
                var anoAtual = DateTime.Now.Year;
                var anoAnterior = anoAtual - 1;

                return await Task.Run(() => metas.Select(
                    m => new DTOMetasFormacaoDeMultiplicadores
                    {
                        Numero = m.Numero,
                        Nome = m.Nome,
                        InscricoesAnoAtual = m.Categorias == null ? 0 : m.Categorias.Sum(c => c.ObterQuantidadeInscricoes(anoAtual)),
                        InscricoesAnoAnterior = m.Categorias == null ? 0 : m.Categorias.Sum(c => c.ObterQuantidadeInscricoes(anoAnterior))
                    }).ToList());
            }
            catch (Exception ex)
            {
                throw new AcademicoException(ex.Message);
            }
        }
    }
}