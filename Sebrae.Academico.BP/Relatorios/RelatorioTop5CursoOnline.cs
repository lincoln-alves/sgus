using Sebrae.Academico.BP.DTO.Relatorios;
using System;
using System.Collections.Generic;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioTop5CursoOnline : BusinessProcessBaseRelatorio, IDisposable
    {

        public IList<DTOTop5CursoOnline> ObterCursosOnline(DateTime dataInicio, DateTime dataFim,int idUf) {
            return (new ManterSolucaoEducacional()).ObterCursosOnline(dataInicio, dataFim, idUf);
        }

        public void Dispose()
        {
            GC.Collect();
        }

        protected override Dominio.Enumeracao.enumRelatorio Relatorio
        {
            get { throw new NotImplementedException(); }
        }
    }
}
