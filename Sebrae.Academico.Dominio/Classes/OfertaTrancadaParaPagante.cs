namespace Sebrae.Academico.Dominio.Classes
{
    public class OfertaTrancadaParaPagante : EntidadeBasica
    {
        public virtual Oferta Oferta { get; set; }
        public virtual NivelOcupacional NivelOcupacional { get; set; }

        public OfertaTrancadaParaPagante()
        {
            Oferta = new Oferta();
            NivelOcupacional = new NivelOcupacional();
        }
    }
}
