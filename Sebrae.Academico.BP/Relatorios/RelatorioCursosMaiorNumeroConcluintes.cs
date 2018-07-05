using System;
using System.Collections.Generic;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioCursosMaiorNumeroConcluintes : BusinessProcessBaseRelatorio, IDisposable
    {
        protected override enumRelatorio Relatorio
        {
            get { return enumRelatorio.MaioresConcluintes; }
        }

        public IList<DTOCursosMaiorNumeroConcluintes> ConsultarConcluintes(int? IdUf, int? Ano)
        {
            this.RegistrarLogExecucao();

            return (new ManterSolucaoEducacional()).ConsultarConcluintes(IdUf, Ano);
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
