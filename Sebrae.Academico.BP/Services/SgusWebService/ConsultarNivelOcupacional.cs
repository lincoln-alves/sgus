using System;
using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP.Services.SgusWebService
{
    public class ConsultarNivelOcupacional : BusinessProcessServicesBase
    {
        private BMNivelOcupacional nivelOcupacionalBM;

        public ConsultarNivelOcupacional()
        {

        }

        public IList<DTONivelOcupacional> ListarNivelOcupacional()
        {
            IList<DTONivelOcupacional> lstResult = null;

            try
            {
                nivelOcupacionalBM = new BMNivelOcupacional();
                lstResult = new List<DTONivelOcupacional>();

                foreach (NivelOcupacional noc in nivelOcupacionalBM.ObterTodos())
                {
                    DTONivelOcupacional nodto = new DTONivelOcupacional();
                    CommonHelper.SincronizarDominioParaDTO(noc, nodto);
                    lstResult.Add(nodto);
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
