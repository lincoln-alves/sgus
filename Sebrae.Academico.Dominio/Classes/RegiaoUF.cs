
namespace Sebrae.Academico.Dominio.Classes
{
    public class RegiaoUF : EntidadeBasicaPorId 
    {
        public virtual Regiao Regiao { get; set; }
        public virtual Uf UF { get; set; }
    }
}