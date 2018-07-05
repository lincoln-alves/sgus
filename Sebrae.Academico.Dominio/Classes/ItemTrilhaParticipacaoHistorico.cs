using System;

namespace Sebrae.Academico.Dominio.Classes
{
    public class ItemTrilhaParticipacaoHistorico : EntidadeBasica
    {
        public virtual bool? Visualizado { get; set; }
        public virtual ItemTrilhaParticipacao ItemTrilhaParticipacao { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual FileServer Anexo { get; set; }


        public virtual string VisualizadoFormatado
        {
            get
            {
                if (Visualizado.HasValue)
                {
                    if (Visualizado.Value)
                    {
                        return "Sim";
                    }
                    else
                    {
                        return "Não";
                    }
                }
                else
                {
                    return "";
                }
            }
        }
    }
}
