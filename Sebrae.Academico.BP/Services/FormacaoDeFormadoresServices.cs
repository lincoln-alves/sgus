using System;
using System.Linq;
using System.Threading.Tasks;
using Sebrae.Academico.BP.DTO.Services.ConsultarFormacaoDeMultiplicadores;
using Sebrae.Academico.Dominio.Enumeracao;


namespace Sebrae.Academico.BP.Services
{
    public class FormacaoDeFormadoresServices : BusinessProcessServicesBase
    {
        public DTOConsultaFormacaoDeMultiplicadores ConsultarFormacaoDeMultiplicadores()
        {
            return ConsultarFormacaoDeMultiplicadoresAsync().Result;
        }

        private async Task<DTOConsultaFormacaoDeMultiplicadores> ConsultarFormacaoDeMultiplicadoresAsync()
        {
            var manterFm = new ManterFormacaoDeMultiplicadores();

            var categoriasFm =
                new ManterCategoriaConteudo().ObterTodasCategoriasFilhas(
                    int.Parse(new ManterConfiguracaoSistema().ObterConfiguracaoSistemaPorID(
                        (int) enumConfiguracaoSistema.IdFormacaoDeFormadores).Registro)).Select(x => x.ID).ToList();

            var metas = new ManterMetaFm().ObterTodos();

            return new DTOConsultaFormacaoDeMultiplicadores
            {
                AnoAtual = DateTime.Now.Year,
                AnoAnterior = DateTime.Now.Year - 1,

                ParticipantesGestores = await manterFm.ObterParticipantesGestoresAsync(categoriasFm),
                ParticipantesFormadores = await manterFm.ObterParticipantesFormadoresAsync(categoriasFm),
                ParticipantesFacilitadores = await manterFm.ObterParticipantesFacilitadoresAsync(categoriasFm),

                CapacitacoesGestores = await manterFm.ObterCapacitacoesGestoresAsync(categoriasFm),
                CapacitacoesFormadores = await manterFm.ObterCapacitacoesFormadoresAsync(categoriasFm),
                CapacitacoesFacilitadores = await manterFm.ObterCapacitacoesFacilitadoresAsync(categoriasFm),

                Metas = await manterFm.ObterMetasAsync(metas)
            };
        }
    }
}