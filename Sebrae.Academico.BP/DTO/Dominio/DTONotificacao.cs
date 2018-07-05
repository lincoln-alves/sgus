using System;

namespace Sebrae.Academico.BP.DTO.Dominio
{
    public class DTONotificacao
    {
        public virtual int ID { get; set; }
        public virtual string Usuario { get; set; }
        public virtual string Link { get; set; }
        public virtual string TextoNotificacao { get; set; }
        public virtual bool Visualizado { get; set; }
        public virtual DateTime DataGeracao { get; set; }
        public virtual DateTime? DataVisualizacao { get; set; }
        public virtual int TipoNotificacao { get; set; }
    }
}
