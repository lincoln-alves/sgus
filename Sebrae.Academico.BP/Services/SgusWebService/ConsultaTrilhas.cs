using System;
using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP.Services.SgusWebService
{
    public class ConsultaTrilhas: BusinessProcessServicesBase
    {
        public IList<DTOTrilha> ListarTrilhas()
        {
            IList<DTOTrilha> lstResult = new List<DTOTrilha>();
            BMTrilha trihaBM = null;
         
            try
            {
                trihaBM = new BMTrilha();

                foreach (Trilha tr in trihaBM.ObterTrilhas())
                {
                    DTOTrilha trdto = new DTOTrilha();
                    CommonHelper.SincronizarDominioParaDTO(tr, trdto);
                    lstResult.Add(trdto);
                }

            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }

            return lstResult;

        }
    }
}
