using Sebrae.Academico.BP.DTO.Relatorios.DashBoard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.Relatorios.Dashboard
{
    public class RelatorioConcluintes : BusinessProcessBaseRelatorio, IDisposable
    {
        protected override Dominio.Enumeracao.enumRelatorio Relatorio
        {
            get { throw new NotImplementedException(); }
        }

        public DTOMatriculas ObterMatriculas(DateTime? inicio, DateTime? fim,int idUf)
        {
            RegistrarLogExecucao();
            var manterMatricula = new ManterMatricula();
            return manterMatricula.ObterMatriculasConcluintes(inicio, fim, idUf);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
