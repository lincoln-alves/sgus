using System.Collections.Generic;
using System.Linq;
using Sebrae.Academico.Dominio.Classes;
using Sebrae.Academico.Dominio.DTO;

namespace Sebrae.Academico.BM.Classes
{
    public class BMRankingTrilhas : BusinessManagerBase
    {
        public List<DTOTrilhaRanking> ObterRanking(TrilhaNivel nivel, int itensPorPagina = 10, int pagina = 1)
        {
            var parametros = new Dictionary<string, object>
            {
                {"ID_TrilhaNivel", nivel.ID},
                {"ItensPorPagina", itensPorPagina},
                {"Pagina", pagina}
            };
            
            return ExecutarProcedure<DTOTrilhaRanking>("SP_RANKING_TRILHAS", parametros).ToList();
        }

        public long? ObterPosicaoRanking(UsuarioTrilha matricula)
        {
            var parametros = new Dictionary<string, object>
            {
                {"ID_TrilhaNivel", matricula.TrilhaNivel.ID},
                {"ID_Usuario", matricula.Usuario.ID}
            };
            
            return ExecutarProcedure<DTOTrilhaRanking>("SP_POSICAO_RANKING_TRILHAS", parametros)
                    .FirstOrDefault()?.Ordem;
        }

        public DTOTrilhaRanking ObterUsuario(int nivelID, int usuarioID)
        {
            var parametros = new Dictionary<string, object>
            {
                {"ID_TrilhaNivel", nivelID},
                {"ItensPorPagina", 10000},
                {"Pagina", 1},
            };

            return ExecutarProcedure<DTOTrilhaRanking>("SP_RANKING_TRILHAS", parametros)
                    .Where(x => x.ID == usuarioID)
                    .FirstOrDefault();
        }

        public int? ObterID(UsuarioTrilha matricula)
        {
            var parametros = new Dictionary<string, object>
            {
                {"ID_TrilhaNivel", matricula.TrilhaNivel.ID},
                {"ID_Usuario", matricula.Usuario.ID}
            };

            return ExecutarProcedure<DTOTrilhaRanking>("SP_POSICAO_RANKING_TRILHAS", parametros)
                    .FirstOrDefault()?.ID;
        }
     

        public List<DTOMissaoMochila> ObterStatusMissoes(UsuarioTrilha matricula)
        {
            var parametros = new Dictionary<string, object>
            {
                {"ID_TrilhaNivel", matricula.TrilhaNivel.ID},
                {"ID_UsuarioTrilha", matricula.ID}
            };
            
            return ExecutarProcedure<DTOMissaoMochila>("SP_MOCHILA_STATUS_MISSOES", parametros).ToList();
        }
    }
}
