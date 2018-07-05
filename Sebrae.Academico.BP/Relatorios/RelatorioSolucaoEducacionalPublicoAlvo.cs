using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Relatorios;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;

namespace Sebrae.Academico.BP.Relatorios
{
    public class RelatorioSolucaoEducacionalPublicoAlvo : BusinessProcessBaseRelatorio, IDisposable
    {
        protected override enumRelatorio Relatorio
        {
            get { return enumRelatorio.ListagemDeSolucoesEducacionaisPorPublicoAlvo; }
        }

        public IList<PublicoAlvo> ObterPublicoAlvoTodos()
        {
            using (var publicoBm = new BMPublicoAlvo())
            {
                return publicoBm.ObterTodos().OrderBy(x => x.Nome).ToList();
            }
        }

        public IList<DTOSolucaoEducacionalPublicoAlvo> ConsultarSolucaEducacionalPublicoAlvo(List<int> publicosAlvo, List<int> categorias, IEnumerable<int> pUfResponsavel)
        {
            return (new ManterSolucaoEducacional()).ConsultarSolucaEducacionalPublicoAlvo(publicosAlvo, categorias, pUfResponsavel);
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