using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.Dominio.DTO;

namespace Sebrae.Academico.BM.Classes
{
    public class BMLiderTrilhas : BusinessManagerBase
    {
        /// <summary>
        /// Obtém os líderes do nível e dos outros filtros especificados, se especificados.
        /// </summary>
        /// <param name="nivelId">ID do nível da trilha.</param>
        /// <param name="usuarioTrilhaId">ID da matrícula do usuário. Opcional.</param>
        /// <param name="pontoSebraeId">ID do ponto sebrae. Opcional.</param>
        /// <returns></returns>
        public List<DTOLider> ObterLideres(int nivelId, int? usuarioTrilhaId = null, int? pontoSebraeId = null)
        {
            var parametros = new Dictionary<string, object>
            {
                {"ID_TrilhaNivel", nivelId},
                {"ID_UsuarioTrilha", usuarioTrilhaId},
                {"ID_PontoSebrae", pontoSebraeId}
            };

            return ExecutarProcedure<DTOLider>("SP_LIDERES_TRILHAS", parametros).ToList();
        }
    }
}
