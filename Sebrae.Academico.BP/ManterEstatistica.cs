using System;
using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using Sebrae.Academico.BP.DTO.Services;
using Sebrae.Academico.Util.Classes;

namespace Sebrae.Academico.BP
{
    public class ManterEstatistica
    {

        public IList<DTOEstatisticaHomePortal> ConsultarUCPortalHomeStats()
        {
            var parametros = new Dictionary<string, object>() { };

            return SQLUtil.ExecutarProcedure<DTOEstatisticaHomePortal>("SP_UC_HOME_STATS", parametros);
        }
    }
}
