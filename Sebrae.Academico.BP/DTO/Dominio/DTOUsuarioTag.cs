using System;

namespace Sebrae.Academico.BP.DTO.Dominio
{
    public class DTOUsuarioTag: DTOEntidadeBasicaPorId
    {
        public virtual DTOUsuario UsuarioOrigem { get; set; }
        public virtual DateTime? DataValidade { get; set; }
        public virtual bool? Adicionado { get; set; }
    }
}
