using System;
using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using System.Linq;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioSolucaoEducacionalFormaAquisicao : BusinessProcessBaseRelatorio, IDisposable
    {
        protected override enumRelatorio Relatorio
        {
            get { return enumRelatorio.ListagemDeSolucoesEducacionaisPorFormaDeAquisicao; }
        }

        public IEnumerable<FormaAquisicao> ObterFormaAquisicaoTodos()
        {
            using (var formaBm = new BMFormaAquisicao())
            {
                return formaBm.ObterTodos();
            }
        }

        public IList<DTOSolucaoEducacionalFormaAquisicao> ConsultarSolucaEducacionalFormaAquisicao(
            List<int> formasDeAquisicao, List<int> categorias, List<int> UfResponsavel)
        {
            return (new ManterSolucaoEducacional()).ConsultarSolucaEducacionalFormaAquisicao(formasDeAquisicao, categorias, UfResponsavel);
        }

        public IList<Uf> ObterUFTodos()
        {
            using (var ufBM = new BMUf())
            {
                return ufBM.ObterTodos().OrderBy(x => x.Nome).ToList();
            }
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}