using System;
using System.Collections.Generic;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioSituacaoCursos : BusinessProcessBaseRelatorio, IDisposable
    {
        protected override enumRelatorio Relatorio
        {
            get { return enumRelatorio.SituacaoCursos; }
        }

        public IList<DTOSituacaoCursos> ConsultarSituacoes(int? IdUf, int? Ano)
        {
            this.RegistrarLogExecucao();

            return (new ManterSolucaoEducacional()).ConsultarSituacoes(IdUf, Ano);
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
