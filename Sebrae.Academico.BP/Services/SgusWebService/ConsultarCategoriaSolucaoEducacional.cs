using System.Collections.Generic;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.BP.Services.SgusWebService
{
    public class ConsultarCategoriaSolucaoEducacional : BusinessProcessServicesBase
    {
        BMCategoriaConteudo categoriaBM;


        public IList<DTOCategoriaConteudo> ListarCategoriaSolucaoEducacional()
        {
            IList<DTOCategoriaConteudo> lstResut = null;
            try
            {
                categoriaBM = new BMCategoriaConteudo();

                lstResut = new List<DTOCategoriaConteudo>();

                foreach (CategoriaConteudo cat in categoriaBM.ObterTodos())
                {
                    DTOCategoriaConteudo dto = new DTOCategoriaConteudo();
                    CommonHelper.SincronizarDominioParaDTO(cat, dto);
                    lstResut.Add(dto);
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

