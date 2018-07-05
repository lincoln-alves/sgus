using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioConcluintes : BusinessProcessBaseRelatorio, IDisposable
    {
        protected override enumRelatorio Relatorio
        {
            get { return enumRelatorio.Concluintes; }
        }

        public IList<FormaAquisicao> ObterListaFormaAquisicao()
        {
            using (var niBm = new BMFormaAquisicao())
            {
                return niBm.ObterTodos().OrderBy(x => x.Nome).ToList();
            }
        }

        public IList<Uf> ObterListaUf()
        {
            using (var niBm = new BMUf())
            {
                return niBm.ObterTodos().OrderBy(x => x.Sigla).ToList();
            }
        }

        public IList<DTOConcluinte> ObterRelatorioConcluinte(int? pFormaAquisicao, int? pUf, IEnumerable<int> pUfResponsavel)
        {
            return (new ManterFormaAquisicao()).ObterRelatorioConcluinte(pFormaAquisicao, pUf, pUfResponsavel);
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
