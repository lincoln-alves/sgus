
namespace Sebrae.Academico.Dominio.Classes
{
    public class ViewTrilhaNivelPermissao
    {
        public virtual int NumeroLinha { get; set; }
        public virtual TrilhaNivel TrilhaNivel { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
