using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioSolucaoEducacionalPrograma : BusinessProcessBaseRelatorio, IDisposable
    {
        protected override enumRelatorio Relatorio
        {
            get { return enumRelatorio.SolucaoEducacionalPorPrograma; }
        }

        public void Dispose()
        {
            GC.Collect();
        }

        public IList<Programa> ObterProgramaTodos()
        {
            using (BMPrograma progBM = new BMPrograma())
            {
                return progBM.ObterTodos().OrderBy(x => x.Nome).ToList();
            }
        }

        public IList<DTORelatorioSolucaoEducacionalPrograma> ObterSolucaoEducacionalPorPrograma(int pIdPrograma, IEnumerable<int> ufsResponsavel)
        {
            return ObterSolucaoEducacionalPorPrograma(pIdPrograma, 0, ufsResponsavel);
        }

        public IList<DTORelatorioSolucaoEducacionalPrograma> ObterSolucaoEducacionalPorPrograma(int pIdPrograma,
            int pIdUf, IEnumerable<int> ufsResponsavel)
        {
            RegistrarLogExecucao();

            return (new ManterSolucaoEducacional()).ObterSolucaoEducacionalPorPrograma(pIdPrograma, pIdUf, ufsResponsavel);
        }

        public IList<Uf> ObterUFTodos()
        {
            using (var ufBM = new BMUf())
            {
                return ufBM.ObterTodos().OrderBy(x => x.Nome).ToList();
            }
        }
    }
}
