using System;
namespace Sebrae.Academico.Dominio.Classes
{
    public class UsuarioTrilhaMensagemGuia
    {
        public virtual int ID { get; set; }
        public virtual DateTime? Visualizacao { get; set; }
        public virtual MensagemGuia MensagemGuia { get; set; }
        public virtual UsuarioTrilha UsuarioTrilha { get; set; }
        public virtual LogLider LogLider { get; set; }
        public virtual ItemTrilha ItemTrilha { get; set; }
        public virtual Missao Missao { get; set; }
    }
}