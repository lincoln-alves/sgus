
namespace Sebrae.Academico.Dominio.Classes
{
    public class ViewSistemaExternoPermissao
    {
        public virtual int NumeroLinha { get; set; }
        public virtual SistemaExterno SistemaExterno { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
