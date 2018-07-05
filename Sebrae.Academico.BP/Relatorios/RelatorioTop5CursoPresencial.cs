using Sebrae.Academico.BP.DTO.Relatorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioTop5CursoPresencial : BusinessProcessBaseRelatorio, IDisposable
    {

        protected override Dominio.Enumeracao.enumRelatorio Relatorio
        {
            get { throw new NotImplementedException(); }
        }

        public IList<DTOTop5CursoPresencial> ObterCursoPresencial(DateTime dataInicio, DateTime dataFim, int idUf)
        {
            return (new ManterSolucaoEducacional()).ObterCursoPresencial(dataInicio, dataFim, idUf);
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
