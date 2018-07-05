using System;
using System.Collections.Generic;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioAtividadeExtraCurricular : BusinessProcessBaseRelatorio, IDisposable
    {

        protected override enumRelatorio Relatorio
        {
            get { return enumRelatorio.AtividadeExtraCurricular; }
        }

        public IList<DTORelatorioAtividadeExtraCurricular> ConsultarRelatorioAtividadeExtraCurricular(DateTime? dataTerIni, DateTime? dataTerFim, DateTime? dataCadIni, DateTime? dataCadFim, int cargaHoraria)
        {
            RegistrarLogExecucao();

            return (new ManterHistoricoExtraCurricular()).ConsultarRelatorioAtividadeExtraCurricular(dataTerIni, dataTerFim, dataCadIni, dataCadFim, cargaHoraria);
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
