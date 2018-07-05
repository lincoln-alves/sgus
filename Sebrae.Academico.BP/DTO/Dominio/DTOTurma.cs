using System;

namespace Sebrae.Academico.BP.DTO.Dominio
{
    public class DTOTurma : DTOEntidadeBasica
    {
        public virtual DTOOferta Oferta { get; set; }
        public virtual DTOProfessor Professor { get; set; }
        public virtual string IDChaveExterna { get; set; }
        public virtual string Local { get; set; }
        public virtual DateTime? DataInicio { get; set; }
        public virtual DateTime? DataFinal { get; set; }
        public virtual string TipoTutoria { get; set; }
        public virtual int VagasDisponiveis { get; set; }
    }
}