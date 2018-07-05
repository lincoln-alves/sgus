using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BP.DTO.Services.HistoricoAcademico;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System;
using Sebrae.Academico.BP.DTO.Dominio;

namespace Sebrae.Academico.BP.Services.SgusWebService
{
    public class TemplateServices : BusinessProcessServicesBase
    {

        public TemplateServices()
            : base()
        {

        }

        public DTOTemplate ConsultarTemplatePorId(int idTemplate)
        {
            DTOTemplate dtoTemplate = new DTOTemplate();

            if (idTemplate > 0)
            {
                Template template = new BMTemplate().ObterPorID(idTemplate);
                if (template != null)
                {
                    dtoTemplate.ID = template.ID;
                    dtoTemplate.DETemplate = template.DescricaoTemplate;
                    dtoTemplate.TXTemplate = template.TextoTemplate;
                }

            }

            return dtoTemplate;
        }
    }
}
