using Sebrae.Academico.Dominio.Classes;
using System.Collections.Generic;

namespace Sebrae.Academico.BP.DTO.Dominio
{
    public class DTOUsuarioUF : DTOEntidadeBasica
    {
        public virtual int IDUf { get; set; }
        public virtual string Sigla { get; set; }
        public virtual List<int> PerfisID { get; set; }
        public virtual List<int> NivelsOcupacionaisID { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
