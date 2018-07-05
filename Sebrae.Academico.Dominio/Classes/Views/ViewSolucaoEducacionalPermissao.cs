
namespace Sebrae.Academico.Dominio.Classes
{
    public class ViewSolucaoEducacionalPermissao
    {
        public virtual int NumeroLinha { get; set; }
        public virtual SolucaoEducacional SolucaoEducacional { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
