using System;
using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP.Services.SgusWebService
{
    public class ConsultarPerfil: BusinessProcessServicesBase
    {

        BMPerfil perfilBM;

        public IList<DTOPerfil> ListarPerfil()
        {
            perfilBM = new BMPerfil();
            IList<DTOPerfil> lstResut = null;
   
            try
            {
                lstResut = new List<DTOPerfil>();

                foreach (Perfil pf in perfilBM.ObterTodos())
                {
                    DTOPerfil pfdto = new DTOPerfil();
                    CommonHelper.SincronizarDominioParaDTO(pf, pfdto);
                    lstResut.Add(pfdto);
                }

            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }

            return lstResut;

        }
    }
}
