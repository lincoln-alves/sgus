using Sebrae.Academico.BP.DTO.Relatorios.DashBoard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.Relatorios.Dashboard
{
    public class RelatorioMatriculasPorMes : BusinessProcessBaseRelatorio, IDisposable
    {
        protected override Dominio.Enumeracao.enumRelatorio Relatorio
        {
            get { throw new NotImplementedException(); }
        }

        public IList<DTOMatriculasPorMes> ObterMatriculas(DateTime? dataInicio, DateTime? dataFim, int idUf)
        {
            this.RegistrarLogExecucao();

            return (new ManterMatricula()).ObterMatriculasPorMes(dataInicio, dataFim, idUf);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
