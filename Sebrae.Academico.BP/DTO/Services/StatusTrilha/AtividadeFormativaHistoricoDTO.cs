using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sebrae.Academico.BP.DTO.Services.StatusTrilha
{
    public class AtividadeFormativaHistoricoDTO
    {
        public string NomeUsuario { get; set; }
        public string Comentario { get; set; }
        public DateTime? Data { get; set; }
        public bool ComentarioParticipante { get; set; }
        public string Anexo { get; set; }
    }
}
