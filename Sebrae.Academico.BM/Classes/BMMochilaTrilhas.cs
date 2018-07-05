using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.Dominio.DTO;

namespace Sebrae.Academico.BM.Classes
{
    public class BMMochilaTrilhas : BusinessManagerBase
    {
        public DTOMochila ObterDadosMochila(int usuarioId, int nivelId)
        {
            var parametros = new Dictionary<string, object>
            {
                {"ID_Usuario", usuarioId},
                {"ID_TrilhaNivel", nivelId}
            };

            return ExecutarProcedure<DTOMochila>("SP_DADOS_MOCHILA", parametros).ToList().FirstOrDefault();
        }
    }
}
