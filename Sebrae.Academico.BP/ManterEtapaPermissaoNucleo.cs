using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Nhibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sebrae.Academico.BP
{
    public class ManterEtapaPermissaoNucleo : RepositorioBase<EtapaPermissaoNucleo>, IDisposable
    {

        public void ExcluirTodosEtapa(int idEtapa)
        {
            var permissoes = ObterTodos().Where(x => x.Etapa.ID == idEtapa);
            ExcluirTodos(permissoes);
        }

        public IList<EtapaPermissaoNucleo> ObterAnalistasEtapa(Etapa etapa, Uf uf)
        {
            return ObterTodos().Where(x => x.Etapa.ID == etapa.ID && x.HierarquiaNucleoUsuario.Uf.ID == uf.ID).Select(x => new EtapaPermissaoNucleo
            {
                Etapa = x.Etapa,
                ID = x.ID,
                HierarquiaNucleoUsuario = new HierarquiaNucleoUsuario
                {
                    ID = x.HierarquiaNucleoUsuario.ID,
                    HierarquiaNucleo = new ManterHierarquiaNucleo().ObterPorId(x.HierarquiaNucleoUsuario.HierarquiaNucleo.ID),
                    Usuario = x.HierarquiaNucleoUsuario.Usuario,
                    IsGestor = x.HierarquiaNucleoUsuario.IsGestor
                },
            }).ToList();
        }

        public IQueryable<EtapaPermissaoNucleo> ObterPorHierarquiaNucleoUsario(int idsHieraquiaNucleoUsuario)
        {
           return ObterTodosIQueryable().Where(x => x.HierarquiaNucleoUsuario.ID == idsHieraquiaNucleoUsuario);
            
        }

        public IQueryable<EtapaPermissaoNucleo> ObterPorHierarquiaNucleoUsarioIn(List<int> idsHieraquiaNucleoUsuario)
        {
            return ObterTodosIQueryable().Where(x => idsHieraquiaNucleoUsuario.Contains(x.HierarquiaNucleoUsuario.ID));

        }
    }
}
