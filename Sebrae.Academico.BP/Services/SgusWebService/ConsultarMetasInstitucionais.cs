using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;

namespace Sebrae.Academico.BP.Services.SgusWebService
{
    public class ConsultarMetasInstitucionais: BusinessProcessServicesBase
    {
        private BMMetaInstitucional metaInstitucionalBM;


        public IList<DTOMetaInstitucional> ObterTodos()
        {
            metaInstitucionalBM = new BMMetaInstitucional(unitOfWork);

            IList<DTOMetaInstitucional> lstResult = new List<DTOMetaInstitucional>();

            foreach (MetaInstitucional mi in metaInstitucionalBM.ObterTodos())
            {
                DTOMetaInstitucional midto = new DTOMetaInstitucional();
                CommonHelper.SincronizarDominioParaDTO(mi, midto);
                lstResult.Add(midto);
            }


            return lstResult;

        }
    }
}
