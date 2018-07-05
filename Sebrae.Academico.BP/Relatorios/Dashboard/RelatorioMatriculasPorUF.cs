using Sebrae.Academico.BP.DTO.Relatorios.DashBoard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.Relatorios.Dashboard
{
    public class RelatorioMatriculasPorUF : BusinessProcessBaseRelatorio, IDisposable
    {
        protected override Dominio.Enumeracao.enumRelatorio Relatorio
        {
            get { throw new NotImplementedException(); }
        }

        public IList<DTOMatriculasPorUF> ObterMatriculas(DateTime? dataInicio,DateTime? dataFim)
        {
            this.RegistrarLogExecucao();

            return (new ManterMatricula()).ObterMatriculasPorUf(dataInicio, dataFim);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
