using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP.Services.SgusWebService
{
    public class ConsultarUF: BusinessProcessServicesBase
    {
        BMUf ufBM;

       
        public IList<DTOUf> ListarUF()
        {
            ufBM = new BMUf();

            IList<DTOUf> lstResut = new List<DTOUf>();

            foreach (Uf uf in ufBM.ObterTodos())
            {
                DTOUf ufdto = new DTOUf();
                CommonHelper.SincronizarDominioParaDTO(uf, ufdto);
                lstResut.Add(ufdto);
            }

            return lstResut;


        }

    }
}
