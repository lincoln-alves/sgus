using Sebrae.Academico.BP.DTO.Relatorios.DashBoard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.Relatorios.Dashboard
{
    public class RelatorioMatriculados : BusinessProcessBaseRelatorio, IDisposable
    {
        protected override Dominio.Enumeracao.enumRelatorio Relatorio
        {
            get { throw new NotImplementedException(); }
        }

        public DTOMatriculas ObterMatriculas(DateTime? inicio, DateTime? fim, int idUf)
        {
            RegistrarLogExecucao();

            return (new ManterMatricula()).ObterMatriculas(inicio, fim,idUf);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
