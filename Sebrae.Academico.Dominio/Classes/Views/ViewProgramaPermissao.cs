
namespace Sebrae.Academico.Dominio.Classes
{
    public class ViewProgramaPermissao
    {
        public virtual int NumeroLinha { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Programa Programa { get; set; }
             
    }
}
