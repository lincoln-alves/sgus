
namespace Sebrae.Academico.Dominio.DTO
{
    /// <summary>
    /// Classe de DTO criada para armazenar informações 
    /// sobre matriculas em ofertas.
    /// </summary>
    public class DTOMatriculaOferta
    {
        public virtual int QuantidadeMatriculados { get; set; }
        public virtual string NomeStatusMatricula { get; set; }
        public virtual double PercentualMatriculados { get; set; }
    }
}
