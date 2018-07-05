using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.Dominio.Classes;
using System.Collections.Generic;
using System.Linq;

namespace Sebrae.Academico.BP.Services.SgusWebService
{
    public class ManterEtapaPermissaoNucleoService : BusinessProcessServicesBase
    {
        public IEnumerable<DTOEtapaPermissaoNucleo> ObterEtapaPermissaoNucleo(Etapa proximaEtapa, Uf uf)
        {
            using (var manter = new ManterEtapaPermissaoNucleo())
            {
                var resultado = manter.ObterAnalistasEtapa(proximaEtapa, uf).Select(x => new DTOEtapaPermissaoNucleo()
                {
                    ID_EtapaPermissaoNucleo = x.ID,
                    ID_Nucleo = x.HierarquiaNucleoUsuario.HierarquiaNucleo.ID,
                    ID_Usuario = x.HierarquiaNucleoUsuario.Usuario.ID,
                    Nucleo = x.HierarquiaNucleoUsuario.HierarquiaNucleo.Nome,
                    ID_Etapa = x.Etapa.ID,
                    NomeUsuario = x.HierarquiaNucleoUsuario.Usuario.Nome,
                    Gestor = x.HierarquiaNucleoUsuario.IsGestor
                });

                return resultado.ToList();
            }
        }
    }
}
