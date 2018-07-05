using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.Dominio.DTO;

namespace Sebrae.Academico.BM.Classes
{
    public class BMMapaTrilhas : BusinessManagerBase
    {
        public List<DTOLoja> ObterLojasMapa(int nivelId, int usuarioTrilhaId)
        {
            var parametros = new Dictionary<string, object>
            {
                {"ID_TrilhaNivel", nivelId},
                {"ID_UsuarioTrilha", usuarioTrilhaId}
            };
            
            return ExecutarProcedure<DTOLoja>("SP_OBTER_LOJAS_MAPA", parametros).ToList();
        }

        public List<DTOUsuarioMapa> ObterParticipantesMapa(List<int> participantesIds)
        {
            var idsStrings = participantesIds.Aggregate("", (current, id) => current + $",{id}");

            var parametros = new Dictionary<string, object>
            {
                {"IDS_Participantes", idsStrings}
            };
            
            return ExecutarProcedure<DTOUsuarioMapa>("SP_OBTER_PARTICIPANTES_MAPA", parametros).ToList();
        }
    }
}
