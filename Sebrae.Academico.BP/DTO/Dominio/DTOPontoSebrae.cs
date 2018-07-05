using Sebrae.Academico.Dominio.Classes;
using System;

namespace Sebrae.Academico.BP.DTO.Dominio
{
    public class DTOPontoSebrae
    {
        public virtual string ID { get; set; }
        public virtual string NomePontoSebrae { get; set; }
        public virtual UsuarioTrilha UsuarioTrilha { get; set; }

    }
}