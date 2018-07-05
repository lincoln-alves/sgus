
namespace Sebrae.Academico.Dominio.Classes
{
    public class ViewOfertaPermissao
    {
        public virtual int NumeroLinha { get; set; }
        public virtual Oferta Oferta { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
