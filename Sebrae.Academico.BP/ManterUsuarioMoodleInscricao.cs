using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.Web.Services3.Security;
using Microsoft.Web.Services3.Security.Tokens;
using Sebrae.Academico.BM.Classes;
using Sebrae.Academico.BM.Mapeamentos.Procedures;
using Sebrae.Academico.BP.wsSEBRAE;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.DTO;
using Sebrae.Academico.Dominio.Enumeracao;
using Sebrae.Academico.Util.Classes;
using Sebrae.Academico.InfraEstrutura.Core.Helper;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using Sebrae.Academico.Dominio.Classes.Moodle;

namespace Sebrae.Academico.BP
{
    public class ManterUsuarioMoodleInscricao : BusinessProcessBase
    {
        public void ExcluirPorUsuarioECurso(UsuarioMoodle usuarioMoodle, int? courseId)
        {
            if (courseId == null) return;
            if (usuarioMoodle == null || usuarioMoodle.ID == 0) return;

            IDictionary<string, object> lstParams = new Dictionary<string, object>();
            lstParams.Add("c_id", courseId);
            lstParams.Add("u_id", usuarioMoodle.ID);

            Sebrae.Academico.InfraEstrutura.Core.Helper.CommonHelper.ExecutarProcedureMysql("SP_DELETE_USER_DATA_FROM_COURSE", lstParams);
        }        
    }
}
