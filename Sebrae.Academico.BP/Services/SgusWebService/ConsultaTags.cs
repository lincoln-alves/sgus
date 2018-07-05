using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Dominio;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.BP.Services.SgusWebService
{
    public class ConsultaTags : BusinessProcessServicesBase
    {
        public List<DTOTag> ConsultarTags()
        {
            BMTag TagBM = null;
            List<DTOTag> result = null;

            try
            {
                TagBM = new BMTag();
                IList<Tag> lstTag = new List<Tag>();
                lstTag = TagBM.ObterTodos();
                result = new List<DTOTag>();
                
                foreach (Tag nivel0 in lstTag.Where(x => x.NumeroNivel == 0))
                {
                    DTOTag dtoTag0 = new DTOTag() { Nome = nivel0.Nome, ID = nivel0.ID, ListaFilhos = RetornaFilhosTags(nivel0), Sinonimo = nivel0.InSinonimo.HasValue ? nivel0.InSinonimo.Value : false, NumeroNivel = nivel0.NumeroNivel };
                    result.Add(dtoTag0);
                }
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }
            
            return result;
        }

        private List<DTOTag> RetornaFilhosTags(Tag registro)
        {
            List<DTOTag> retorno = null;

            try
            {
                retorno = new List<DTOTag>();
                
                foreach (var filho in registro.ListaTagFilhos)
                {
                    retorno.Add(new DTOTag { Nome = filho.Nome, ID = filho.ID, ListaFilhos = RetornaFilhosTags(filho), Sinonimo = filho.InSinonimo.HasValue ? filho.InSinonimo.Value : false, NumeroNivel = filho.NumeroNivel });
                }
            }
            catch (Exception ex)
            {
                ErroUtil.Instancia.TratarErro(ex);
            }
                       
            return retorno;
        }
    }
}
