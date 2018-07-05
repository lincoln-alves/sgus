using Sebrae.Academico.Dominio.Classes;
using System.Collections.Generic;

namespace Sebrae.Academico.Dominio.DTO
{
    public class DTOFiltrosHistoricoParticipacaoTrilha
    {
        public string ddlPontoSebrae { get; set; }
        public string ddlMissao { get; set; }
        public string rblTipoSolucao { get; set; }
        public string ddlStatus { get; set; }
        public UsuarioTrilha usuarioTrilha { get; set; }
        public List<int> niveisSelecionados { get; set; }
        


    }
}
